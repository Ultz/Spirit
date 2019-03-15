// 
// IHttpHeaders.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

#region

using System.Collections.Generic;

#endregion

namespace Ultz.Spirit.Core
{
    public interface IHttpHeaders : IEnumerable<KeyValuePair<string, string>>
    {
        string GetByName(string name);

        bool TryGetByName(string name, out string value);
    }
}
