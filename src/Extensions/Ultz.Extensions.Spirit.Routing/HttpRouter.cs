// 
// HttpRouter.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

#region

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ultz.Spirit.Core;

#endregion

namespace Ultz.Extensions.Spirit.Routing
{
    public class HttpRouter : IHttpRequestHandler
    {
        private readonly IDictionary<string, IHttpRequestHandler> _handlers =
            new Dictionary<string, IHttpRequestHandler>(StringComparer.InvariantCultureIgnoreCase);

        public Task Handle(IHttpContext context, Func<Task> nextHandler)
        {
            var function = string.Empty;

            if (context.Request.RequestParameters.Length > 0) function = context.Request.RequestParameters[0];

            IHttpRequestHandler value;
            if (_handlers.TryGetValue(function, out value)) return value.Handle(context, nextHandler);


            // Route not found, Call next.
            return nextHandler();
        }

        public HttpRouter With(string function, IHttpRequestHandler handler)
        {
            _handlers.Add(function, handler);

            return this;
        }
    }
}
