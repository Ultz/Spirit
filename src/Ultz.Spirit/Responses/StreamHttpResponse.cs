// 
// StreamHttpResponse.cs
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
using System.Threading.Tasks;
using Ultz.Spirit.Core;
using Ultz.Spirit.Headers;

#endregion

namespace Ultz.Spirit.Responses
{
    public sealed class StreamHttpResponse : HttpResponseBase
    {
        private readonly Stream _body;

        public StreamHttpResponse(Stream body, HttpResponseCode code, IHttpHeaders headers) : base(code, headers)
        {
            _body = body;
        }

        public static IHttpResponse Create
        (
            Stream body,
            HttpResponseCode code = HttpResponseCode.Ok,
            string contentType = "text/html; charset=utf-8",
            bool keepAlive = true
        )
        {
            return new StreamHttpResponse
            (
                body, code, new ListHttpHeaders
                (
                    new[]
                    {
                        new KeyValuePair<string, string>("Date", DateTime.UtcNow.ToString("R")),
                        new KeyValuePair<string, string>("content-type", contentType),
                        new KeyValuePair<string, string>("connection", keepAlive ? "keep-alive" : "close"),
                        new KeyValuePair<string, string>
                            ("content-length", body.Length.ToString(CultureInfo.InvariantCulture))
                    }
                )
            );
        }

        public override async Task WriteBody(StreamWriter writer)
        {
            await writer.FlushAsync().ConfigureAwait(false);
            await _body.CopyToAsync(writer.BaseStream).ConfigureAwait(false);
            await writer.BaseStream.FlushAsync().ConfigureAwait(false);
        }
    }
}
