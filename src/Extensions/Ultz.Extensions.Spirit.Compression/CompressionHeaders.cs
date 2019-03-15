// 
// CompressionHeaders.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

#region

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ultz.Spirit.Core;

#endregion

namespace Ultz.Extensions.Spirit.Compression
{
    public class CompressionHeaders : IHttpHeaders
    {
        private readonly IList<KeyValuePair<string, string>> _base;

        public CompressionHeaders(IList<KeyValuePair<string, string>> @base)
        {
            _base = @base;
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _base.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public string GetByName(string name)
        {
            return _base.First(x => x.Key == name).Value;
        }

        public bool TryGetByName(string name, out string value)
        {
            value = null;
            if (_base.All(x => x.Key != name)) return false;
            value = _base.First(x => x.Key == name).Value;
            return true;
        }
    }
}
