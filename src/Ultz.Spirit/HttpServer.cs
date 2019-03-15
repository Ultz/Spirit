// 
// HttpServer.cs
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
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Ultz.Spirit.Core;
using Ultz.Spirit.Extensions;

#endregion

namespace Ultz.Spirit
{
    public sealed class HttpServer : IDisposable
    {
        private readonly IList<IHttpRequestHandler> _handlers = new List<IHttpRequestHandler>();
        private readonly IList<IHttpListener> _listeners = new List<IHttpListener>();
        private readonly IHttpRequestProvider _requestProvider;
        private bool _isActive;

        public HttpServer(IHttpRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        [CanBeNull] public ILoggerProvider LoggerProvider { get; private set; }

        [CanBeNull] private ILogger Logger { get; set; }

        public void Dispose()
        {
            _isActive = false;
        }

        public void Use(IHttpRequestHandler handler)
        {
            _handlers.Add(handler);
        }

        public void Use(IHttpListener listener)
        {
            _listeners.Add(listener);
        }

        public void Use(ILoggerProvider logger)
        {
            Logger = logger.CreateLogger("HttpServer");
            LoggerProvider = logger;
        }

        public void Start()
        {
            _isActive = true;

            foreach (var listener in _listeners)
            {
                var tempListener = listener;

                Task.Factory.StartNew(() => Listen(tempListener));
            }

            Logger?.LogInformation("Spirit has started.");
        }

        private async void Listen(IHttpListener listener)
        {
            var aggregatedHandler = _handlers.Aggregate();

            while (_isActive)
                try
                {
                    _requestProvider.Handle
                    (
                        await listener.GetClient().ConfigureAwait(false), aggregatedHandler, _requestProvider,
                        LoggerProvider?.CreateLogger("ClientHandler")
                    );
                }
                catch (Exception e)
                {
                    Logger?.LogWarning("Error while getting client", e);
                }

            Logger?.LogInformation("Spirit has stopped.");
        }
    }
}
