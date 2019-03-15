// 
// ErrorHandler.cs
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

#endregion

namespace DemoOne
{
    public class ErrorHandler : IHttpRequestHandler
    {
        public Task Handle(IHttpContext context, Func<Task> next)
        {
            context.Response = new HttpResponse
                (HttpResponseCode.NotFound, "These are not the droids you are looking for.", true);
            return Task.Factory.GetCompleted();
        }
    }
}
