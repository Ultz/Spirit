// 
// SslListener.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

#region

using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Ultz.Spirit.Core;

#endregion

namespace Ultz.Extensions.Spirit.Ssl
{
    public class SslListener : IHttpListener
    {
#if NETCOREAPP2_1 || NETSTANDARD2_1
        private readonly SslServerAuthenticationOptions _auth;
#else
        private readonly X509Certificate _certificate;
#endif
        private readonly IHttpListener _child;

        public SslListener(IHttpListener child, X509Certificate certificate)
        {
            _child = child;
#if NETCOREAPP2_1 || NETSTANDARD2_1
            _auth = new SslServerAuthenticationOptions()
            {
                ServerCertificate = new X509Certificate2(certificate.Export(X509ContentType.Pkcs12)),
                ClientCertificateRequired = false,
                EnabledSslProtocols = SslProtocols.Tls12
            };
#else
            _certificate = new X509Certificate2(certificate.Export(X509ContentType.Pkcs12));
#endif
        }

#if NETCOREAPP2_1 || NETSTANDARD2_1
        public SslListener(IHttpListener child, SslServerAuthenticationOptions auth)
        {
            _child = child;
            _auth = auth;
        }
#endif


        public async Task<IClient> GetClient()
        {
#if NETCOREAPP2_1 || NETSTANDARD2_1
            return new SslClient(await _child.GetClient().ConfigureAwait(false), _auth);
#else
            return new SslClient(await _child.GetClient().ConfigureAwait(false), _certificate);
#endif
        }

        public void Dispose()
        {
            _child.Dispose();
        }
    }
}
