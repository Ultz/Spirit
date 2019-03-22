// 
// HttpPost.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

#region

using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Ultz.Spirit.Core;

#endregion

namespace Ultz.Spirit.Headers
{
    public class HttpPost : IHttpPost
    {
        private readonly Lazy<IHttpHeaders> _parsed;

        public HttpPost(byte[] raw)
        {
            Raw = raw;
            _parsed = new Lazy<IHttpHeaders>(Parse);
        }

        public static async Task<IHttpPost> Create(StreamReader reader, int postContentLength)
        {
            var rawEncoded = new char[postContentLength];

            var readBytes = await reader.ReadAsync(rawEncoded, 0, rawEncoded.Length).ConfigureAwait(false);

            var raw = Encoding.UTF8.GetBytes(rawEncoded, 0, readBytes);

            return new HttpPost(raw);
        }

        private IHttpHeaders Parse()
        {
            var body = Encoding.UTF8.GetString(Raw);
            var parsed = new QueryStringHttpHeaders(body);

            return parsed;
        }

        #region IHttpPost implementation

        public byte[] Raw { get; }

        public IHttpHeaders Parsed => _parsed.Value;

        #endregion
    }
}
