// 
// IClient.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

#region

using System.IO;
using System.Net;

#endregion

namespace Ultz.Spirit.Core
{
    public interface IClient
    {
        Stream Stream { get; }

        bool Connected { get; }

        EndPoint RemoteEndPoint { get; }

        void Close();
    }
}
