// 
// HttpRequestProviderMethodOverrideDecorator.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

#region

using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Ultz.Spirit.Core;
using Ultz.Spirit.Http.Common;

#endregion

namespace Ultz.Spirit.Http.One
{
    public class HttpRequestProviderMethodOverrideDecorator : IHttpRequestProvider
    {
        private readonly IHttpRequestProvider _child;

        public HttpRequestProviderMethodOverrideDecorator(IHttpRequestProvider child)
        {
            _child = child;
        }

        public async Task<bool> Provide(StreamReader streamReader, Action<IHttpRequest> onRequest, ILogger logger)
        {
            return await _child.Provide(streamReader, (childValue) =>
            {
                string methodName;
                if (!childValue.Headers.TryGetByName("X-HTTP-Method-Override", out methodName)) onRequest(childValue);

                onRequest(new HttpRequestMethodDecorator(childValue, HttpMethodProvider.Default.Provide(methodName)));
            }, logger).ConfigureAwait(false);
        }

        public HttpClientHandlerBase Handle
        (
            IClient client,
            Func<IHttpContext, Task> requestHandler,
            IHttpRequestProvider requestProvider,
            ILogger logger
        )
        {
            return _child.Handle(client, requestHandler, requestProvider, logger);
        }
    }
}
