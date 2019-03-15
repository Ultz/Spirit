// 
// IHttpRequestHandler.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

#region

using System;
using System.Threading.Tasks;

#endregion

namespace Ultz.Spirit.Core
{
    public interface IHttpRequestHandler
    {
        Task Handle(IHttpContext context, Func<Task> next);
    }
}
