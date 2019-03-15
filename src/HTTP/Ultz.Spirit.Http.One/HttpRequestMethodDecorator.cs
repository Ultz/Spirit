// 
// HttpRequestMethodDecorator.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

#region

using System;
using Ultz.Spirit.Core;

#endregion

namespace Ultz.Spirit.Http.One
{
    internal class HttpRequestMethodDecorator : IHttpRequest
    {
        private readonly IHttpRequest _child;

        public HttpRequestMethodDecorator(IHttpRequest child, HttpMethods method)
        {
            _child = child;
            Method = method;
        }

        public IHttpHeaders Headers => _child.Headers;

        public HttpMethods Method { get; }

        public string Protocol => _child.Protocol;

        public Uri Uri => _child.Uri;

        public string[] RequestParameters => _child.RequestParameters;

        public IHttpPost Post => _child.Post;

        public IHttpHeaders QueryString => _child.QueryString;
    }
}
