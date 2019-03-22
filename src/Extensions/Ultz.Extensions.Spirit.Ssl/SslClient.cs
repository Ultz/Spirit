// 
// SslClient.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

#region

using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Ultz.Spirit.Core;

#endregion

namespace Ultz.Extensions.Spirit.Ssl
{
    public class SslClient : IClient
    {
        private readonly IClient _child;
        private readonly SslStream _sslStream;

        #if NETCOREAPP2_1 || NETSTANDARD2_1
        public SslClient(IClient child, SslServerAuthenticationOptions certificate)
        {
            _child = child;
            _sslStream = new SslStream(_child.Stream);
            _sslStream.AuthenticateAsServerAsync(certificate, CancellationToken.None).GetAwaiter().GetResult();
        }
        #else
        public SslClient(IClient child, X509Certificate certificate)
        {
            _child = child;
            _sslStream = new SslStream(_child.Stream);
            _sslStream.AuthenticateAsServer(certificate, false, SslProtocols.Tls, true);
        }
        #endif

        public Stream Stream => _sslStream;

        public bool Connected => _child.Connected;

        public void Close()
        {
            _child.Close();
        }

        public EndPoint RemoteEndPoint => _child.RemoteEndPoint;
    }
}
