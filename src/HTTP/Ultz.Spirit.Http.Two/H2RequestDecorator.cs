// 
// H2Request.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

using System;
using Http2;
using Ultz.Spirit.Core;

namespace Ultz.Spirit.Http.Two
{
    public class H2RequestDecorator : IHttpRequest
    {
        public H2RequestDecorator(IHttpRequest @base, IStream h2stream)
        {
            Base = @base;
            Stream = h2stream;
        }
        public IHttpRequest Base { get; }
        public IStream Stream { get; }
        public IHttpHeaders Headers => Base.Headers;
        public HttpMethods Method => Base.Method;
        public string Protocol => Base.Protocol;
        public Uri Uri => Base.Uri;
        public string[] RequestParameters => Base.RequestParameters;
        public IHttpPost Post => Base.Post;
        public IHttpHeaders QueryString => Base.QueryString;
    }
}
