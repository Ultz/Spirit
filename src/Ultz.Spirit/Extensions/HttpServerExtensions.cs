// 
// HttpServerExtensions.cs
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
    public static class HttpServerExtensions
    {
        public static void Use(this HttpServer server, Func<IHttpContext, Func<Task>, Task> method)
        {
            server.Use(new AnonymousHttpRequestHandler(method));
        }
    }
}
