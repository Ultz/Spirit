// 
// HttpMethodProviderCache.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

#region

using System;
using System.Collections.Concurrent;
using Ultz.Spirit.Core;

#endregion

namespace Ultz.Spirit.Http.Common
{
    public class HttpMethodProviderCache : IHttpMethodProvider
    {
        private readonly ConcurrentDictionary<string, HttpMethods> _cache =
            new ConcurrentDictionary<string, HttpMethods>();

        private readonly Func<string, HttpMethods> _childProvide;

        public HttpMethodProviderCache(IHttpMethodProvider child)
        {
            _childProvide = child.Provide;
        }

        public HttpMethods Provide(string name)
        {
            return _cache.GetOrAdd(name, _childProvide);
        }
    }
}
