// 
// H2Headers.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Http2.Hpack;
using Ultz.Spirit.Core;

namespace Ultz.Spirit.Http.Two
{
    public class H2Headers : IHttpHeaders
    {
        public IEnumerable<HeaderField> Headers { get; set; }
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return Headers.Select(x => new KeyValuePair<string, string>(x.Name, x.Value)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public string GetByName(string name)
        {
            return Headers.First(x => string.Equals(x.Name, name, StringComparison.CurrentCultureIgnoreCase)).Value;
        }

        public bool TryGetByName(string name, out string value)
        {
            value = null;
            if (!Headers.Any(x => string.Equals(x.Name, name, StringComparison.CurrentCultureIgnoreCase))) return false;
            value = GetByName(name);
            return true;
        }
    }
}
