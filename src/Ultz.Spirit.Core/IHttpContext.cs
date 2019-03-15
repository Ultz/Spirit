// 
// IHttpContext.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

#region

using System.Net;

#endregion

namespace Ultz.Spirit.Core
{
    public interface IHttpContext
    {
        IHttpRequest Request { get; }

        IHttpResponse Response { get; set; }

        ICookiesStorage Cookies { get; }

        dynamic State { get; }

        EndPoint RemoteEndPoint { get; }
    }
}
