// 
// IHttpClientHandler.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

#region

using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

#endregion

namespace Ultz.Spirit.Core
{
    public abstract class HttpClientHandlerBase
    {
        protected readonly EndPoint RemoteEndPoint;
        protected readonly Func<IHttpContext, Task> RequestHandler;
        protected readonly IHttpRequestProvider RequestProvider;
        protected readonly Stream Stream;

        protected HttpClientHandlerBase
        (
            IClient client,
            Func<IHttpContext, Task> requestHandler,
            IHttpRequestProvider requestProvider,
            ILogger logger
        )
        {
            Logger = logger;
            RemoteEndPoint = client.RemoteEndPoint;
            Client = client;
            RequestHandler = requestHandler;
            RequestProvider = requestProvider;

            Stream = Client.Stream;

            Logger?.LogInformation("Got Client {0}", RemoteEndPoint);

            Task.Factory.StartNew(Process);
        }

        protected ILogger Logger { get; }

        public IClient Client { get; }

        protected abstract void Process();

        public void ForceClose()
        {
            Client.Close();
        }
    }
}
