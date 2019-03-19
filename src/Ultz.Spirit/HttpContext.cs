// 
// HttpContext.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

#region

using System.Dynamic;
using System.Net;
using Ultz.Spirit.Core;
using Ultz.Spirit.Headers;

#endregion

namespace Ultz.Spirit
{
    public class HttpContext : IHttpContext
    {
        private readonly ExpandoObject _state = new ExpandoObject();

        public HttpContext(IHttpRequest request, EndPoint remoteEndPoint, IClient client)
        {
            Request = request;
            RemoteEndPoint = remoteEndPoint;
            Cookies = new CookiesStorage(Request.Headers.GetByNameOrDefault("cookie", string.Empty));
            Client = client;
        }

        public IHttpRequest Request { get; }

        public IHttpResponse Response { get; set; }

        public ICookiesStorage Cookies { get; }

        public IClient Client { get; }

        public dynamic State => _state;

        public EndPoint RemoteEndPoint { get; }
    }
}
