// 
// TcpListenerAdapter.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

#region

using System.Net.Sockets;
using System.Threading.Tasks;

#endregion

namespace Ultz.Spirit.Core
{
    public class TcpListenerAdapter : IHttpListener
    {
        private readonly TcpListener _listener;

        public TcpListenerAdapter(TcpListener listener)
        {
            _listener = listener;
            _listener.Start();
        }

        public async Task<IClient> GetClient()
        {
            return new TcpClientAdapter(await _listener.AcceptTcpClientAsync().ConfigureAwait(false));
        }

        public void Dispose()
        {
            _listener.Stop();
        }
    }
}
