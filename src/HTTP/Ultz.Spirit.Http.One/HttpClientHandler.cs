// 
// HttpClientHandler.cs
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
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Ultz.Spirit.Core;
using Ultz.Spirit.Handling;
using Ultz.Spirit.Headers;

#endregion

namespace Ultz.Spirit.Http.One
{
    internal sealed class HttpClientHandler : HttpClientHandlerBase
    {
        public static readonly string Version = "Spirit/" + typeof(HttpServer).Assembly.GetName().Version.ToString(2);

        public HttpClientHandler
        (
            IClient client,
            Func<IHttpContext, Task> requestHandler,
            IHttpRequestProvider requestProvider,
            ILogger logger
        ) : base(client, requestHandler, requestProvider, logger)
        {
            UpdateLastOperationTime();
        }

        protected override async void Process()
        {
            try
            {
                while (Client.Connected)
                {
                    // TODO : Configuration.
                    var limitedStream = new NotFlushingStream(new LimitedStream(Stream, 1024 * 1024, 1024 * 1024));
                    var streamReader = new StreamReader(limitedStream);

                    var request = await RequestProvider.Provide(streamReader).ConfigureAwait(false);

                    if (request != null)
                    {
                        UpdateLastOperationTime();

                        var context = new HttpContext(request, Client.RemoteEndPoint);

                        Logger.LogInformation("{1} : Got request {0}", request.Uri, Client.RemoteEndPoint);

                        await RequestHandler(context).ConfigureAwait(false);

                        if (context.Response != null)
                        {
                            var streamWriter = new StreamWriter(new MemoryStream());
                            await WriteResponse(context, streamWriter).ConfigureAwait(false);
                            var bytes = ((MemoryStream) streamWriter.BaseStream).ToArray();
                            await limitedStream.WriteAsync(bytes, 0, bytes.Length);
                            await limitedStream.ExplicitFlushAsync().ConfigureAwait(false);

                            if (!request.Headers.KeepAliveConnection() || context.Response.CloseConnection)
                                Client.Close();
                            
                            Logger.LogDebug("Completed");
                        }

                        UpdateLastOperationTime();
                    }
                    else
                    {
                        Client.Close();
                    }
                }
            }
            catch (Exception e)
            {
                // Hate people who make bad calls.
                Logger.LogWarning($"Error while serving : {RemoteEndPoint}", e);
                Client.Close();
            }

            Logger.LogInformation("Lost Client {0}", RemoteEndPoint);
        }

        private async Task WriteResponse(HttpContext context, StreamWriter writer)
        {
            var response = context.Response;

            // Headers
            await writer.WriteLineAsync
                (
                    $"HTTP/1.1 {(int) response.ResponseCode} {response.ResponseCode}"
                )
                .ConfigureAwait(false);

            foreach (var header in response.Headers.Where(x => x.Key != "server"))
                await writer.WriteLineAsync($"{header.Key}: {header.Value}").ConfigureAwait(false);
            if (response.Headers.TryGetByName("Server", out var val))
                await writer.WriteLineAsync("Server: "+Version + "; " + val);
            else
                await writer.WriteLineAsync("Server: "+Version);

            // Cookies
            if (context.Cookies.Touched)
                await writer.WriteAsync(context.Cookies.ToCookieData())
                    .ConfigureAwait(false);

            // Empty Line
            await writer.WriteLineAsync().ConfigureAwait(false);

            var sw = new StreamWriter(new MemoryStream());
            // Body
            await response.WriteBody(sw).ConfigureAwait(false);
            var bytes = ((MemoryStream) sw.BaseStream).ToArray();
            await writer.FlushAsync().ConfigureAwait(false);
            await writer.BaseStream.WriteAsync(bytes, 0, bytes.Length);
        }

        private void UpdateLastOperationTime()
        {
            // _lastOperationTime = DateTime.Now;
        }
    }
}
