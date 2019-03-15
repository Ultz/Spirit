// 
// SslListener.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

#region

using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Ultz.Spirit.Core;

#endregion

namespace Ultz.Extensions.Spirit.Ssl
{
    public class SslListener : IHttpListener
    {
        private readonly X509Certificate _certificate;
        private readonly IHttpListener _child;

        public SslListener(IHttpListener child, X509Certificate certificate)
        {
            _child = child;
            _certificate = certificate;
        }

        public async Task<IClient> GetClient()
        {
            return new SslClient(await _child.GetClient().ConfigureAwait(false), _certificate);
        }

        public void Dispose()
        {
            _child.Dispose();
        }
    }
}
