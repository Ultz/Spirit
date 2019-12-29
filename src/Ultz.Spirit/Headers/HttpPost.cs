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
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ultz.Spirit.Core;

#endregion

namespace Ultz.Spirit.Headers
{
    public class HttpPost : IHttpPost
    {
        private readonly Lazy<IHttpHeaders> _parsed;
        public static Thread _thread;

        public HttpPost(byte[] raw)
        {
            Raw = raw;
            _parsed = new Lazy<IHttpHeaders>(Parse);
        }

        public static async Task<IHttpPost> Create(Stream stream, int postContentLength)
        {
            _thread = Thread.CurrentThread;
            var raw = new byte[postContentLength];

            var readBytes = 0;
            while (readBytes != postContentLength)
            {
                readBytes += await stream
                    .ReadAsync(raw, readBytes, Math.Min(postContentLength - readBytes, 1024))
                    .ConfigureAwait(false);
            }

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