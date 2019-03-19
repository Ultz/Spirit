// 
// H2Stream.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

using System;
using System.IO;
using Http2;

namespace Ultz.Spirit.Http.Two
{
    public class H2Stream : Stream
    {
        private IStream _wrap;
        public H2Stream(H2Client wrap)
        {
            _wrap = wrap.Base;
        }
            
        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _wrap.ReadAsync(new ArraySegment<byte>(buffer, offset, count)).ConfigureAwait(false).GetAwaiter().GetResult().BytesRead;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new System.NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new System.NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _wrap.WriteAsync(new ArraySegment<byte>(buffer, offset, count)).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public override void Close()
        {
            _wrap.CloseAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            base.Close();
        }

        public override bool CanRead { get; } = true;
        public override bool CanSeek { get; } = false;
        public override bool CanWrite { get; } = true;
        public override long Length => throw new NotSupportedException();
        public override long Position {get => throw new NotSupportedException();set => throw new NotSupportedException();}
    }
}