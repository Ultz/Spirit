using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Ultz.Spirit.Http.One
{
    public static class StreamExtensions
    {
        public static async Task<string> ReadLineAsync(this Stream stream)
        {
            var bytes = new List<byte>();
            var buffer = new byte[1];
            var cr = false;
            var lf = false;
            while (!cr || !lf)
            {
                var r = await stream.ReadAsync(buffer, 0, 1);
                if (r == -1)
                {
                    throw new EndOfStreamException();
                }

                var b = buffer[0];

                if (b == '\r')
                {
                    cr = true;
                    continue;
                }

                if (cr && b != '\n')
                {
                    cr = false;
                    bytes.Add((byte) '\r');
                    continue;
                }

                if (b == '\n')
                {
                    lf = true;
                    continue;
                }

                bytes.Add(b);
            }

            return Encoding.UTF8.GetString(bytes.ToArray());
        }
    }
}