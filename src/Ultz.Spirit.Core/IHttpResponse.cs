// 
// IHttpResponse.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

#region

using System;
using System.IO;
using System.Threading.Tasks;

#endregion

namespace Ultz.Spirit.Core
{
    public interface IHttpResponse
    {
        /// <summary>
        ///     Gets the status line of this http response,
        ///     The first line that will be sent to the client.
        /// </summary>
        HttpResponseCode ResponseCode { get; }

        IHttpHeaders Headers { get; }
        [Obsolete("The connection is automatically closed unless it has been explicitly configured to be kept alive.")]
        bool CloseConnection { get; }
        Task WriteBody(StreamWriter writer);
    }
}
