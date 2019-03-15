// 
// ListHttpHeaders.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

#region

using System;
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
    public class ListHttpHeaders : IHttpHeaders
    {
        private readonly IList<KeyValuePair<string, string>> _values;

        public ListHttpHeaders(IList<KeyValuePair<string, string>> values)
        {
            _values = values;
        }

        internal int Count => _values.Count;

        public string GetByName(string name)
        {
            return _values.Where
                    (kvp => kvp.Key.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                .Select(kvp => kvp.Value)
                .First();
        }

        public bool TryGetByName(string name, out string value)
        {
            value = _values.Where
                    (kvp => kvp.Key.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                .Select(kvp => kvp.Value)
                .FirstOrDefault();

            return value != default;
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
