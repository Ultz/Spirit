// 
// HttpHeadersDebuggerProxy.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

#region

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Ultz.Spirit.Core;

#endregion

namespace Ultz.Spirit.Headers
{
    internal class HttpHeadersDebuggerProxy
    {
        private readonly IHttpHeaders _real;

        public HttpHeadersDebuggerProxy(IHttpHeaders real)
        {
            _real = real;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public HttpHeader[] Headers
        {
            get { return _real.Select(kvp => new HttpHeader(kvp)).ToArray(); }
        }

        [DebuggerDisplay("{Value,nq}", Name = "{Key,nq}")]
        internal class HttpHeader
        {
            private readonly KeyValuePair<string, string> _header;

            public HttpHeader(KeyValuePair<string, string> header)
            {
                _header = header;
            }

            public string Value => _header.Value;

            public string Key => _header.Key;
        }
    }
}
