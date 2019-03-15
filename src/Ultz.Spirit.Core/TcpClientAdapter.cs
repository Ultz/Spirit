// 
// TcpClientAdapter.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

#region

using System.IO;
using System.Net;
using System.Net.Sockets;

#endregion

namespace Ultz.Spirit.Core
{
    public class TcpClientAdapter : IClient
    {
        private readonly TcpClient _client;

        public TcpClientAdapter(TcpClient client)
        {
            _client = client;
        }

        public Stream Stream => _client.GetStream();

        public bool Connected => _client.Connected;

        public void Close()
        {
            _client.Close();
        }


        public EndPoint RemoteEndPoint => _client.Client.RemoteEndPoint;
    }
}
