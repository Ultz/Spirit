// 
// HttpMethodProvider.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

#region

using System;
using Ultz.Spirit.Core;

#endregion

namespace Ultz.Spirit.Http.Common
{
    public class HttpMethodProvider : IHttpMethodProvider
    {
        public static readonly IHttpMethodProvider Default = new HttpMethodProviderCache(new HttpMethodProvider());

        internal HttpMethodProvider()
        {
        }

        public HttpMethods Provide(string name)
        {
            var capitalName = name.Substring(0, 1).ToUpper() + name.Substring(1).ToLower();
            return (HttpMethods) Enum.Parse(typeof(HttpMethods), capitalName);
        }
    }
}
