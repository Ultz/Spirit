// 
// TimingHandler.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

#region

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Ultz.Spirit.Core;

#endregion

namespace DemoOne
{
    public class TimingHandler : IHttpRequestHandler
    {
        public async Task Handle(IHttpContext context, Func<Task> next)
        {
            var stopWatch = Stopwatch.StartNew();
            await next();

            Console.WriteLine("request {0} took {1}", context.Request.Uri, stopWatch.Elapsed);
        }
    }
}
