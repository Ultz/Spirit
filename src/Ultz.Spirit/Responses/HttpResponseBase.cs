// 
// HttpResponseBase.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

#region

using System;
using System.IO;
using System.Threading.Tasks;
using Ultz.Spirit.Core;

#endregion

namespace Ultz.Spirit.Responses
{
    public abstract class HttpResponseBase : IHttpResponse
    {
        protected HttpResponseBase(HttpResponseCode code, IHttpHeaders headers)
        {
            ResponseCode = code;
            Headers = headers;
        }

        public abstract Task WriteBody(StreamWriter writer);

        public HttpResponseCode ResponseCode { get; }

        public IHttpHeaders Headers { get; }

        public bool CloseConnection
        {
            get
            {
                string value;
                return !(Headers.TryGetByName("Connection", out value) &&
                         value.Equals("Keep-Alive", StringComparison.InvariantCultureIgnoreCase));
            }
        }
    }
}
