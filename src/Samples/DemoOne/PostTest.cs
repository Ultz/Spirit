// 
// PostTest.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

using System;
using System.Threading.Tasks;
using Ultz.Spirit.Builders;
using Ultz.Spirit.Core;
using Ultz.Spirit.Extensions;

namespace DemoOne
{
    internal class PostTest : IHttpRequestHandler
    {
        public Task Handle(IHttpContext context, Func<Task> next)
        {
            if (context.Request.GetRawUrl().StartsWith("/hello"))
            {
                if (context.Request.Method == HttpMethods.Get)
                {
                    context.Response = new HttpResponseBuilder().WithContent
                        (
                            "<h1>What's your name?</h1><form method=\"POST\">" +
                            "Name: <input type=\"text\" name=\"name\" />" + "<input type=\"submit\"></form>"
                        )
                        .Build();
                    return Task.CompletedTask;
                }
                else
                {
                    context.Response = new HttpResponseBuilder().WithContent
                        (
                            "<h1>Hello, " + context.Request.Post.Parsed.GetByName("name") +
                            "</h1><a href=\".\">< Return</a>"
                        )
                        .Build();
                    return Task.CompletedTask;
                }
            }

            return next();
        }
    }
}