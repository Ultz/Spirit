// 
// EmptyHttpResponse.cs
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
using System.Threading.Tasks;
using Ultz.Spirit.Core;
using Ultz.Spirit.Extensions;
using Ultz.Spirit.Headers;

#endregion

namespace Ultz.Spirit.Responses
{
    public sealed class EmptyHttpResponse : HttpResponseBase
    {
        public EmptyHttpResponse(HttpResponseCode code, IHttpHeaders headers) : base(code, headers)
        {
        }

        public static IHttpResponse Create(HttpResponseCode code = HttpResponseCode.Ok, bool keepAlive = true)
        {
            return new EmptyHttpResponse
            (
                code, new ListHttpHeaders
                (
                    new[]
                    {
                        new KeyValuePair<string, string>("Date", DateTime.UtcNow.ToString("R")),
                        new KeyValuePair<string, string>("content-type", "text/html"),
                        new KeyValuePair<string, string>("connection", keepAlive ? "keep-alive" : "close"),
                        new KeyValuePair<string, string>("content-length", "0")
                    }
                )
            );
        }

        public override Task WriteBody(StreamWriter writer)
        {
            return Task.Factory.GetCompleted();
        }
    }
}
