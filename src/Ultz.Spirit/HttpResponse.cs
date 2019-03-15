// 
// HttpResponse.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Ultz.Spirit.Core;
using Ultz.Spirit.Headers;

#endregion

namespace Ultz.Spirit
{
    public sealed class HttpResponse : IHttpResponse
    {
        private readonly Stream _headerStream = new MemoryStream();

        public HttpResponse(HttpResponseCode code, string content, bool closeConnection)
            : this(code, "text/html; charset=utf-8", StringToStream(content), closeConnection)
        {
        }

        public HttpResponse
        (
            HttpResponseCode code,
            string content,
            IEnumerable<KeyValuePair<string, string>> headers,
            bool closeConnection
        )
            : this(code, "text/html; charset=utf-8", StringToStream(content), closeConnection, headers)
        {
        }

        public HttpResponse(string contentType, Stream contentStream, bool closeConnection)
            : this(HttpResponseCode.Ok, contentType, contentStream, closeConnection)
        {
        }

        public HttpResponse
        (
            HttpResponseCode code,
            string contentType,
            Stream contentStream,
            bool keepAliveConnection,
            IEnumerable<KeyValuePair<string, string>> headers
        )
        {
            ContentStream = contentStream;

            CloseConnection = !keepAliveConnection;

            ResponseCode = code;
            Headers = new ListHttpHeaders
            (
                new[]
                    {
                        new KeyValuePair<string, string>("Date", DateTime.UtcNow.ToString("R")),
                        new KeyValuePair<string, string>("Connection", CloseConnection ? "Close" : "Keep-Alive"),
                        new KeyValuePair<string, string>("Content-Type", contentType),
                        new KeyValuePair<string, string>
                            ("Content-Length", ContentStream.Length.ToString(CultureInfo.InvariantCulture))
                    }.Concat(headers)
                    .ToList()
            );
        }

        public HttpResponse(HttpResponseCode code, string contentType, Stream contentStream, bool keepAliveConnection) :
            this
            (
                code, contentType, contentStream, keepAliveConnection, Enumerable.Empty<KeyValuePair<string, string>>()
            )
        {
        }


        public HttpResponse(HttpResponseCode code, byte[] contentStream, bool keepAliveConnection)
            : this(code, "text/html; charset=utf-8", new MemoryStream(contentStream), keepAliveConnection)
        {
        }

        public HttpResponse(HttpResponseCode code, string contentType, string content, bool closeConnection)
            : this(code, contentType, StringToStream(content), closeConnection)
        {
        }

        private Stream ContentStream { get; }

        public async Task WriteBody(StreamWriter writer)
        {
            ContentStream.Position = 0;
            await ContentStream.CopyToAsync(writer.BaseStream).ConfigureAwait(false);
        }

        public HttpResponseCode ResponseCode { get; }

        public IHttpHeaders Headers { get; }

        public bool CloseConnection { get; }

        public static HttpResponse CreateWithMessage
            (HttpResponseCode code, string message, bool keepAliveConnection, string body = "")
        {
            return new HttpResponse
            (
                code,
                string.Format
                (
                    "<html><head><title>{0}</title></head><body><h1>{0}</h1><hr>{1}</body></html>",
                    message, body
                ), keepAliveConnection
            );
        }

        private static MemoryStream StringToStream(string content)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            return stream;
        }

        public async Task WriteHeaders(StreamWriter writer)
        {
            _headerStream.Position = 0;
            await _headerStream.CopyToAsync(writer.BaseStream).ConfigureAwait(false);
        }
    }
}
