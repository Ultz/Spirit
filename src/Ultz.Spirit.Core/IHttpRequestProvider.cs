// 
// IHttpRequestProvider.cs
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

#endregion

namespace Ultz.Spirit.Core
{
    public interface IHttpRequestProvider
    {
        /// <summary>
        ///     Provides an <see cref="IHttpRequest" /> based on the context of the stream,
        ///     May return null / throw exceptions on invalid requests.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="onRequest"></param>
        /// <returns></returns>
        Task<bool> Provide(Stream stream, Action<IHttpRequest> onRequest, ILogger logger);

        HttpClientHandlerBase Handle
        (
            IClient client,
            Func<IHttpContext, Task> requestHandler,
            IHttpRequestProvider requestProvider,
            ILogger logger
        );
    }
}
