// 
// AnonymousHttpRequestHandler.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

#region

using System;
using System.Threading.Tasks;
using Ultz.Spirit.Core;

#endregion

namespace Ultz.Spirit.Extensions
{
    public class AnonymousHttpRequestHandler : IHttpRequestHandler
    {
        private readonly Func<IHttpContext, Func<Task>, Task> _method;

        public AnonymousHttpRequestHandler(Func<IHttpContext, Func<Task>, Task> method)
        {
            _method = method;
        }


        public Task Handle(IHttpContext context, Func<Task> next)
        {
            return _method(context, next);
        }
    }
}
