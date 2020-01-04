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
using System.Linq;
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

        public HttpPost(byte[] raw)
        {
            Raw = raw;
            _parsed = new Lazy<IHttpHeaders>(Parse);
        }

        public static async Task<IHttpPost> Create(Stream stream, int postContentLength)
        {
            var buffer = new byte[postContentLength];
            int read = 0;
            while ((read += await stream.ReadAsync(buffer, read, buffer.Length - read)) > 0)
            {
                if (read == postContentLength)
                {
                    return new HttpPost(buffer);
                }
            }
            
            return new HttpPost(new ArraySegment<byte>(buffer, 0, read).ToArray());
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