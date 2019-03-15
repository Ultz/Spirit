// 
// IHttpRequest.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

#region

using System;

#endregion

namespace Ultz.Spirit.Core
{
    public interface IHttpRequest
    {
        IHttpHeaders Headers { get; }

        HttpMethods Method { get; }

        string Protocol { get; }

        Uri Uri { get; }

        string[] RequestParameters { get; }

        IHttpPost Post { get; }

        IHttpHeaders QueryString { get; }
    }
}
