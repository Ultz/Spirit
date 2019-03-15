// 
// StringHttpResponse.cs
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
using System.Text;
using System.Threading.Tasks;
using Ultz.Spirit.Core;
using Ultz.Spirit.Headers;

#endregion

namespace Ultz.Spirit.Responses
{
    public sealed class StringHttpResponse : HttpResponseBase
    {
        private readonly string _body;

        public StringHttpResponse(string body, HttpResponseCode code, IHttpHeaders headers) : base(code, headers)
        {
            _body = body;
        }

        public static IHttpResponse Create
        (
            string body,
            HttpResponseCode code = HttpResponseCode.Ok,
            string contentType = "text/html; charset=utf-8",
            bool keepAlive = true
        )
        {
            return new StringHttpResponse
            (
                body, code, new ListHttpHeaders
                (
                    new[]
                    {
                        new KeyValuePair<string, string>("Date", DateTime.UtcNow.ToString("R")),
                        new KeyValuePair<string, string>("content-type", contentType),
                        new KeyValuePair<string, string>("connection", keepAlive ? "keep-alive" : "close"),
                        new KeyValuePair<string, string>
                            ("content-length", Encoding.UTF8.GetByteCount(body).ToString(CultureInfo.InvariantCulture))
                    }
                )
            );
        }

        public override async Task WriteBody(StreamWriter writer)
        {
            await writer.WriteAsync(_body).ConfigureAwait(false);
        }
    }
}
