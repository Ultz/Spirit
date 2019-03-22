// 
// SslListenerBuilder.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

using System.Collections.Generic;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using Ultz.Spirit.Core;

#if NETSTANDARD2_1 || NETCOREAPP2_1
namespace Ultz.Extensions.Spirit.Ssl
{
    public class SslListenerBuilder
    {
        private SslServerAuthenticationOptions _options = new SslServerAuthenticationOptions();
        private IHttpListener _listener;

        public SslListenerBuilder WithH2()
        {
            return WithAlpnProtocol(SslApplicationProtocol.Http2);
        }

        public SslListenerBuilder WithHttp11()
        {
            return WithAlpnProtocol(SslApplicationProtocol.Http11);
        }

        public SslListenerBuilder WithAlpnProtocol(SslApplicationProtocol proto)
        {
            (_options.ApplicationProtocols ?? (_options.ApplicationProtocols = new List<SslApplicationProtocol>())).Add
                (proto);
            return this;
        }

        public SslListenerBuilder WithServerCertificate(X509Certificate cert)
        {
            _options.ServerCertificate = cert;
            return this;
        }

        public SslListenerBuilder WithEncryptionPolicy(EncryptionPolicy policy)
        {
            _options.EncryptionPolicy = policy;
            return this;
        }

        public SslListenerBuilder WithNoEncryption()
        {
            return WithEncryptionPolicy(EncryptionPolicy.NoEncryption);
        }

        public SslListenerBuilder WithNoEncryptionAllowed()
        {
            return WithEncryptionPolicy(EncryptionPolicy.AllowNoEncryption);
        }

        public SslListenerBuilder WithEncryptionRequired()
        {
            return WithEncryptionPolicy(EncryptionPolicy.RequireEncryption);
        }

        public SslListenerBuilder WithRenegotiationAllowed(bool allowed = true)
        {
            _options.AllowRenegotiation = allowed;
            return this;
        }

        public SslListenerBuilder WithSslProtocol(SslProtocols protocol)
        {
            _options.EnabledSslProtocols = protocol;
            return this;
        }

        public SslListenerBuilder WithTls10()
        {
            return WithSslProtocol(SslProtocols.Tls);
        }

        public SslListenerBuilder WithTls11()
        {
            return WithSslProtocol(SslProtocols.Tls11);
        }

        public SslListenerBuilder WithTls12()
        {
            return WithSslProtocol(SslProtocols.Tls12);
        }

        public SslListenerBuilder WithClientCertificate(bool required = true)
        {
            _options.ClientCertificateRequired = required;
            return this;
        }

        public SslListenerBuilder WithRemoteValidationCallback(RemoteCertificateValidationCallback callback)
        {
            _options.RemoteCertificateValidationCallback = callback;
            return this;
        }

        public SslListenerBuilder WithServerCertificateSelectionCallback(ServerCertificateSelectionCallback callback)
        {
            _options.ServerCertificateSelectionCallback = callback;
            return this;
        }

        public SslListenerBuilder WithRevocationCheckMode(X509RevocationMode mode)
        {
            _options.CertificateRevocationCheckMode = mode;
            return this;
        }

        public SslListenerBuilder WithOfflineMode()
        {
            return WithRevocationCheckMode(X509RevocationMode.Offline);
        }

        public SslListenerBuilder WithOnlineMode()
        {
            return WithRevocationCheckMode(X509RevocationMode.Online);
        }

        public SslListenerBuilder WithNoChecks()
        {
            return WithRevocationCheckMode(X509RevocationMode.NoCheck);
        }

        public SslListenerBuilder WithChild(IHttpListener listener)
        {
            _listener = listener;
            return this;
        }

        public SslListener Build()
        {
            return new SslListener(_listener, _options);
        }
    }
}
#endif
