// 
// HttpRequestParameters.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

#region

using System;
using System.Collections.Generic;

#endregion

namespace Ultz.Spirit.Requests
{
    public sealed class HttpRequestParameters
    {
        private static readonly char[] Separators = {'/'};
        private readonly string[] _params;

        public HttpRequestParameters(Uri uri)
        {
            var url = uri.OriginalString;
            _params = url.Split(Separators, StringSplitOptions.RemoveEmptyEntries);
        }

        public IList<string> Params => _params;
    }
}
