// 
// HttpRequestProvider.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Http2;
using Http2.Hpack;
using Microsoft.Extensions.Logging;
using Ultz.Spirit.Core;
using Ultz.Spirit.Headers;
using Ultz.Spirit.Http.Common;
using Ultz.Spirit.Http.One;

#endregion

namespace Ultz.Spirit.Http.Two
{
    public class H2RequestProvider : IHttpRequestProvider
    {
        private static readonly char[] Separators = {'/'};
        public static readonly byte[] Preface = Encoding.ASCII.GetBytes("PRI * HTTP/2.0\r\n\r\nSM\r\n\r\n");

        public async Task<bool> Provide(StreamReader streamReader, Action<IHttpRequest> onRequest, ILogger logger)
        {
            var buffer = new byte[Preface.Length];
            var len = await streamReader.BaseStream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
            var readPreface = new ArraySegment<byte>(buffer, 0, len).ToArray();
            var newSr = new StreamReader(new MemoryBackedStream(new MemoryStream(readPreface), streamReader.BaseStream));
            if (readPreface.SequenceEqual(Preface))
            {
                logger.LogTrace("Received HTTP/2 header!");
                return await ProvideHttp2(newSr, onRequest, logger);
            }
            else
            {
                return await ProvideHttp1
                    (streamReader, onRequest, logger, await newSr.ReadLineAsync().ConfigureAwait(false));
            }
        }

        public Task<bool> ProvideHttp2(StreamReader streamReader, Action<IHttpRequest> onRequest, ILogger logger)
        {
            var wrap = streamReader.BaseStream.CreateStreams();
            var connection = new Connection(new ConnectionConfigurationBuilder(true)
                .UseStreamListener(stream =>
                {
                    var headers = new H2Headers()
                    {
                        Headers = stream.ReadHeadersAsync().ConfigureAwait(false).GetAwaiter().GetResult()
                    };
                    var post = GetPostData(stream, headers).ConfigureAwait(false).GetAwaiter().GetResult();
                    var url = headers.GetByName(":path");
                    var uri = new Uri(url, UriKind.Relative);
                    var method = HttpMethodProvider.Default.Provide(headers.GetByName(":method"));
                    if (headers.TryGetByName(":authority", out var authority) && !headers.TryGetByName("host", out _))
                    {
                        headers.Headers = headers.Headers.Concat
                        (
                            new[]
                            {
                                new HeaderField()
                                    {Name = "Host", Value = authority.Replace("http://", "").Replace("https://", "")}
                            }
                        );
                    }

                    headers.Headers = headers.Headers.Where(x => !x.Name.StartsWith(":"));
                    onRequest
                    (
                        new HttpRequest
                        (
                            headers, method, "HTTP/2.0", uri,
                            uri.OriginalString.Split(Separators, StringSplitOptions.RemoveEmptyEntries),
                            HttpRequestProvider.GetQueryStringData(ref url), post
                        )
                    );
                    return true;
                })
                .UseSettings(Settings.Max)
                .UseHuffmanStrategy(HuffmanStrategy.IfSmaller)
                .Build(), wrap.ReadableStream, wrap.WriteableStream, new Connection.Options(){Logger = logger});
            var remoteGoAwayTask = connection.RemoteGoAwayReason;
            var closeWhenRemoteGoAway = Task.Run(async () =>
            {
                await remoteGoAwayTask;
                await connection.GoAwayAsync(ErrorCode.NoError, true);
            });
            return Task.FromResult(true);
        }

        public async Task<bool> ProvideHttp1(StreamReader streamReader, Action<IHttpRequest> onRequest, ILogger logger, string request)
        {
            
            if (request == null)
                return false;

            var firstSpace = request.IndexOf(' ');
            var lastSpace = request.LastIndexOf(' ');

            var tokens = new[]
            {
                request.Substring(0, firstSpace),
                request.Substring(firstSpace + 1, lastSpace - firstSpace - 1),
                request.Substring(lastSpace + 1)
            };

            if (tokens.Length != 3) return false;


            var httpProtocol = tokens[2];

            var url = tokens[1];
            var queryString = GetQueryStringData(ref url);
            var uri = new Uri(url, UriKind.Relative);

            var headersRaw = new List<KeyValuePair<string, string>>();

            // get the headers
            string line;

            while (!string.IsNullOrEmpty(line = await streamReader.ReadLineAsync().ConfigureAwait(false)))
            {
                var currentLine = line;

                var headerKvp = SplitHeader(currentLine);
                headersRaw.Add(headerKvp);
            }

            IHttpHeaders headers = new HttpHeaders
                (headersRaw);
            var post = await HttpRequestProvider.GetPostData(streamReader, headers).ConfigureAwait(false);

            if (!headers.TryGetByName("_method", out var verb)) verb = tokens[0];
            var httpMethod = HttpMethodProvider.Default.Provide(verb);
            onRequest(new HttpRequest
            (
                headers, httpMethod, httpProtocol, uri,
                uri.OriginalString.Split(Separators, StringSplitOptions.RemoveEmptyEntries), queryString, post
            ));
            return true;
        }

        public HttpClientHandlerBase Handle
        (
            IClient client,
            Func<IHttpContext, Task> requestHandler,
            IHttpRequestProvider requestProvider,
            ILogger logger
        )
        {
            return new H2ClientHandler(client, requestHandler, requestProvider, logger);
        }

        public static IHttpHeaders GetQueryStringData(ref string url)
        {
            var queryStringIndex = url.IndexOf('?');
            IHttpHeaders queryString;
            if (queryStringIndex != -1)
            {
                queryString = new QueryStringHttpHeaders(url.Substring(queryStringIndex + 1));
                url = url.Substring(0, queryStringIndex);
            }
            else
            {
                queryString = EmptyHttpHeaders.Empty;
            }

            return queryString;
        }

        private static async Task<IHttpPost> GetPostData(IStream s, IHttpHeaders headers)
        {
            IHttpPost post;
            if (headers.TryGetByName("content-length", out int postContentLength) && postContentLength > 0)
            {
                var buffer = new ArraySegment<byte>(new byte[postContentLength]);
                await s.ReadAsync(buffer).ConfigureAwait(false);
                post = new HttpPost(buffer.ToArray());
            }
            else
            {
                post = EmptyHttpPost.Empty;
            }

            return post;
        }

        private KeyValuePair<string, string> SplitHeader(string header)
        {
            var index = header.IndexOf(": ", StringComparison.InvariantCultureIgnoreCase);
            return new KeyValuePair<string, string>(header.Substring(0, index), header.Substring(index + 2));
        }
    }
}
