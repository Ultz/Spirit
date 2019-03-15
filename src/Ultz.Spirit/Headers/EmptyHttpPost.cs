// 
// EmptyHttpPost.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

#region

using Ultz.Spirit.Core;

#endregion

namespace Ultz.Spirit.Headers
{
    public class EmptyHttpPost : IHttpPost
    {
        private static readonly byte[] EmptyBytes = new byte[0];

        public static readonly IHttpPost Empty = new EmptyHttpPost();

        private EmptyHttpPost()
        {
        }

        #region IHttpPost implementation

        public byte[] Raw => EmptyBytes;

        public IHttpHeaders Parsed => EmptyHttpHeaders.Empty;

        #endregion
    }
}
