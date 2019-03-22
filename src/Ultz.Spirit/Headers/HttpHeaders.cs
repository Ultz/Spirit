// 
// HttpHeaders.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

#region

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Ultz.Spirit.Core;

#endregion

namespace Ultz.Spirit.Headers
{
    [DebuggerDisplay("{Count} Headers")]
    [DebuggerTypeProxy(typeof(HttpHeadersDebuggerProxy))]
    public class HttpHeaders : IHttpHeaders
    {
        private readonly List<KeyValuePair<string, string>> _values;

        public HttpHeaders(List<KeyValuePair<string, string>> values)
        {
            _values = values;
        }


        internal int Count => _values.Count;

        public string GetByName(string name)
        {
            return _values.First(x => x.Key.ToLower() == name.ToLower()).Value;
        }

        public bool TryGetByName(string name, out string value)
        {
            value = null;
            if (_values.All(x => x.Key.ToLower() != name.ToLower())) return false;
            value = GetByName(name);
            return true;
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
