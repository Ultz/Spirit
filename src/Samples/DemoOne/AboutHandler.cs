// 
// AboutHandler.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

#region

using System;
using System.Threading.Tasks;
using Ultz.Spirit;
using Ultz.Spirit.Core;
using Ultz.Spirit.Extensions;
using Ultz.Spirit.Headers;

#endregion

namespace DemoOne
{
    public class AboutHandler : IHttpRequestHandler
    {
        public Task Handle(IHttpContext context, Func<Task> next)
        {
            context.Response = HttpResponse.CreateWithMessage
                (HttpResponseCode.Ok, "Sample http-request-handler", context.Request.Headers.KeepAliveConnection());

            return Task.Factory.GetCompleted();
        }
    }
}
