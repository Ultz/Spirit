// 
// IndexHandler.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

#region

using System;
using System.Text;
using System.Threading.Tasks;
using Ultz.Spirit;
using Ultz.Spirit.Core;
using Ultz.Spirit.Extensions;
using Ultz.Spirit.Headers;

#endregion

namespace DemoOne
{
    public class IndexHandler : IHttpRequestHandler
    {
        private readonly HttpResponse _keepAliveResponse;
        private readonly HttpResponse _response;

        public IndexHandler()
        {
            var contents = Encoding.UTF8.GetBytes("Welcome to the Index.");
            _keepAliveResponse = new HttpResponse(HttpResponseCode.Ok, contents, true);
            _response = new HttpResponse(HttpResponseCode.Ok, contents, false);
        }

        public Task Handle(IHttpContext context, Func<Task> next)
        {
            context.Response = context.Request.Headers.KeepAliveConnection() ? _keepAliveResponse : _response;
            return Task.Factory.GetCompleted();
        }
    }
}
