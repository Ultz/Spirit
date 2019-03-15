// 
// LimitedStream.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

#region

using System.IO;

#endregion

namespace Ultz.Spirit.Handling
{
    public class LimitedStream : Stream
    {
        private const string ExceptionMessageFormat = "The Stream has exceeded the {0} limit specified.";
        private readonly Stream _child;
        private long _readLimit;
        private long _writeLimit;

        public LimitedStream(Stream child, long readLimit = -1, long writeLimit = -1)
        {
            _child = child;
            _readLimit = readLimit;
            _writeLimit = writeLimit;
        }

        public override bool CanRead => _child.CanRead;

        public override bool CanSeek => _child.CanSeek;

        public override bool CanWrite => _child.CanWrite;

        public override long Length => _child.Length;

        public override long Position
        {
            get => _child.Position;
            set => _child.Position = value;
        }

        public override int ReadTimeout
        {
            get => _child.ReadTimeout;
            set => _child.ReadTimeout = value;
        }

        public override int WriteTimeout
        {
            get => _child.WriteTimeout;
            set => _child.WriteTimeout = value;
        }

        public override void Flush()
        {
            _child.Flush();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _child.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _child.SetLength(value);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var retVal = _child.Read(buffer, offset, count);

            AssertReadLimit(retVal);

            return retVal;
        }

        private void AssertReadLimit(int coefficient)
        {
            if (_readLimit == -1) return;

            _readLimit -= coefficient;

            if (_readLimit < 0) throw new IOException(string.Format(ExceptionMessageFormat, "read"));
        }

        private void AssertWriteLimit(int coefficient)
        {
            if (_writeLimit == -1) return;

            _writeLimit -= coefficient;

            if (_writeLimit < 0) throw new IOException(string.Format(ExceptionMessageFormat, "write"));
        }

        public override int ReadByte()
        {
            var retVal = _child.ReadByte();

            AssertReadLimit(1);

            return retVal;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _child.Write(buffer, offset, count);

            AssertWriteLimit(count);
        }

        public override void WriteByte(byte value)
        {
            _child.WriteByte(value);

            AssertWriteLimit(1);
        }
    }
}
