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
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Ultz.Spirit.Core;
using Ultz.Spirit.Headers;
using Ultz.Spirit.Http.Common;

#endregion

namespace Ultz.Spirit.Http.One
{
    public class HttpRequestProvider : IHttpRequestProvider
    {
        private static readonly char[] Separators = {'/'};

        public async Task<bool> Provide(StreamReader streamReader, Action<IHttpRequest> onRequest, ILogger logger)
        {
            // parse the http request
            var request = await streamReader.ReadLineAsync().ConfigureAwait(false);

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
            var post = await GetPostData(streamReader, headers).ConfigureAwait(false);

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
            return new HttpClientHandler(client, requestHandler, requestProvider, logger);
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

        public static async Task<IHttpPost> GetPostData(StreamReader streamReader, IHttpHeaders headers)
        {
            int postContentLength;
            IHttpPost post;
            if (headers.TryGetByName("content-length", out postContentLength) && postContentLength > 0)
                post = await HttpPost.Create(streamReader, postContentLength).ConfigureAwait(false);
            else
                post = EmptyHttpPost.Empty;
            return post;
        }

        private KeyValuePair<string, string> SplitHeader(string header)
        {
            var index = header.IndexOf(": ", StringComparison.InvariantCultureIgnoreCase);
            return new KeyValuePair<string, string>(header.Substring(0, index), header.Substring(index + 2));
        }
    }
}
