// 
// H2StreamWrapper.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

using System.IO;
using System.Net;
using Http2;
using Ultz.Spirit.Core;

namespace Ultz.Spirit.Http.Two
{
    public class H2Client : IClient
    {
        public H2Client(IStream stream, IClient @base)
        {
            Client = @base;
            Base = stream;
            Stream = new H2Stream(this);
        }

        public IClient Client { get; }
        public IStream Base { get; }
        public Stream Stream { get; }
        public bool Connected { get; }
        public EndPoint RemoteEndPoint { get; }
        public void Close()
        {
            Base.CloseAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }
        
    }
}
