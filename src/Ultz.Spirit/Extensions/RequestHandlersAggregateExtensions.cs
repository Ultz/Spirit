// 
// RequestHandlersAggregateExtensions.cs
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

namespace Ultz.Spirit.Extensions
{
    public static class RequestHandlersAggregateExtensions
    {
        public static Func<IHttpContext, Task> Aggregate(this IList<IHttpRequestHandler> handlers)
        {
            return handlers.Aggregate(0);
        }

        private static Func<IHttpContext, Task> Aggregate(this IList<IHttpRequestHandler> handlers, int index)
        {
            if (index == handlers.Count) return null;

            var currentHandler = handlers[index];
            var nextHandler = handlers.Aggregate(index + 1);

            return context => currentHandler.Handle(context, () => nextHandler(context));
        }
    }
}
