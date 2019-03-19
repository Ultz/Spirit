// 
// H2RequestProvider.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Http2;
using Http2.Hpack;
using Microsoft.Extensions.Logging;
using Ultz.Spirit.Core;
using Ultz.Spirit.Http.One;

namespace Ultz.Spirit.Http.Two
{
    public class H2RequestProvider : IHttpRequestProvider
    {
        private H2Mode _mode;
        private HttpRequestProvider _h1;

        public H2RequestProvider(H2Mode mode = H2Mode.AlwaysUpgrade)
        {
            _mode = mode;
            _h1 = new HttpRequestProvider();
        }
        
        public async Task<bool> Provide(StreamReader streamReader, Action<IHttpRequest> onRequestReceived, ILogger logger)
        {
            var bytes = new byte[ClientPreface.Length];
            await streamReader.BaseStream.ReadAsync(bytes, 0, bytes.Length);
            if (!bytes.SequenceEqual(ClientPreface.Bytes))
            {
                // HTTP/1
                if (_mode == H2Mode.NeverUpgrade)
                {
                    return await _h1.Provide
                    (
                        new StreamReader(new MemoryBackedStream(new MemoryStream(bytes), streamReader.BaseStream)),
                        onRequestReceived, logger
                    );
                }
                else
                {
                    // TODO: TryHttp2Onboarding
                }
            }
            else
            {
                // HTTP/2
                var wrap = streamReader.BaseStream.CreateStreams();
                var connection = new Connection(new ConnectionConfigurationBuilder(true)
                    .UseStreamListener(stream =>
                        {
                            var headers = stream.ReadHeadersAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                            // TODO: Continue H2 Implementation...
                            return false;
                        })
                    .UseSettings(Settings.Max)
                    .UseHuffmanStrategy(HuffmanStrategy.IfSmaller)
                    .Build(), wrap.ReadableStream, wrap.WriteableStream, new Connection.Options(){Logger = logger});
            }

            return false; // todo
        }

        public async Task<bool> TryHttp2Onboarding()
        {
            // TODO: H2 Upgrade logic
            return false;
        }

        public HttpClientHandlerBase Handle(IClient client, Func<IHttpContext, Task> requestHandler, IHttpRequestProvider requestProvider, ILogger logger)
        {
            return new H2ClientHandler(client, requestHandler, requestProvider, logger);
        }
    }
}
