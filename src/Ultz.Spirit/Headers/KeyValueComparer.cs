// 
// KeyValueComparer.cs
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

namespace Ultz.Spirit.Headers
{
    public class KeyValueComparer<TKey, TValue, TOutput> : IEqualityComparer<KeyValuePair<TKey, TValue>>
    {
        private readonly IEqualityComparer<TOutput> _outputComparer;
        private readonly Func<KeyValuePair<TKey, TValue>, TOutput> _outputFunc;

        public KeyValueComparer
            (Func<KeyValuePair<TKey, TValue>, TOutput> outputFunc, IEqualityComparer<TOutput> outputComparer)
        {
            _outputFunc = outputFunc;
            _outputComparer = outputComparer;
        }

        public bool Equals(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y)
        {
            return _outputComparer.Equals(_outputFunc(x), _outputFunc(y));
        }

        public int GetHashCode(KeyValuePair<TKey, TValue> obj)
        {
            return _outputComparer.GetHashCode(_outputFunc(obj));
        }
    }
}
