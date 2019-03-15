// 
// CookiesStorage.cs
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
using System.Text;
using Ultz.Spirit.Core;

#endregion

namespace Ultz.Spirit
{
    public class CookiesStorage : ICookiesStorage
    {
        private static readonly string[] CookieSeparators = {"; ", "="};

        private readonly Dictionary<string, string> _values;

        public CookiesStorage(string cookie)
        {
            var keyValues = cookie.Split(CookieSeparators, StringSplitOptions.RemoveEmptyEntries);
            _values = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

            for (var i = 0; i < keyValues.Length; i += 2)
            {
                var key = keyValues[i];
                var value = keyValues[i + 1];

                _values[key] = value;
            }
        }

        public bool Touched { get; private set; }

        public string ToCookieData()
        {
            var builder = new StringBuilder();

            foreach (var kvp in _values)
                builder.AppendFormat("Set-Cookie: {0}={1}{2}", kvp.Key, kvp.Value, Environment.NewLine);

            return builder.ToString();
        }


        public void Upsert(string key, string value)
        {
            _values[key] = value;

            Touched = true;
        }

        public void Remove(string key)
        {
            if (_values.Remove(key)) Touched = true;
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public string GetByName(string name)
        {
            return _values[name];
        }

        public bool TryGetByName(string name, out string value)
        {
            return _values.TryGetValue(name, out value);
        }
    }
}
