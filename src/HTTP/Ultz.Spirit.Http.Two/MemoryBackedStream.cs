// 
// DualSourceStream.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

using System;
using System.IO;

namespace Ultz.Spirit.Http.Two
{
    public class MemoryBackedStream : Stream
    {
        public MemoryStream Memory { get; }
        public Stream Stream { get; }

        public MemoryBackedStream(MemoryStream memory, Stream stream)
        {
            Memory = memory;
            Stream = stream;
        }
        
        public override void Flush()
        {
            Stream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var o = offset;
            var c = count;
            var memoryBacking = Memory.Read(buffer, offset, count);
            c -= memoryBacking;
            o += memoryBacking;
            var actual = Stream.Read(buffer, o, c);
            return memoryBacking + actual;
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
            Stream.Write(buffer, offset, count);
        }

        public void Back(byte[] buffer, int offset, int count)
        {
            Memory.Write(buffer, offset, count);
        }

        public override bool CanRead { get; } = true;
        public override bool CanSeek { get; } = false;
        public override bool CanWrite { get; } = true;
        public override long Length => throw new NotSupportedException();

        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }
    }
}
