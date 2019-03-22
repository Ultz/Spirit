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
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Http2.Hpack;
using Microsoft.Extensions.Logging;
using Ultz.Spirit.Core;
using Ultz.Spirit.Headers;
using Ultz.Spirit.Http.One;

#endregion

namespace Ultz.Spirit.Http.Two
{
    public sealed class H2ClientHandler : HttpClientHandlerBase
    {
        public static readonly string Version = "Spirit/" + typeof(HttpServer).Assembly.GetName().Version.ToString(2);

        public H2ClientHandler
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
                    var streamReader = new StreamReader(Stream);

                    var result = await RequestProvider.Provide(streamReader, async request =>
                    {
                        try
                        {
                            UpdateLastOperationTime();

                            var context = new HttpContext
                            (
                                request, Client.RemoteEndPoint,
                                request is H2RequestDecorator req ? new H2Client(req.Stream, Client) : Client
                            );

                            Logger?.LogInformation("{1} : Got request {0}", request.Uri, Client.RemoteEndPoint);

                            await RequestHandler(context).ConfigureAwait(false);

                            if (context.Response != null)
                            {
                                if (context.Client is H2Client)
                                {
                                    await ProcessHttp2(context);
                                }
                                else
                                {
                                    await ProcessHttp1(context);
                                }
                            }

                            UpdateLastOperationTime();
                        }
                        catch (Exception ex)
                        {
                            // Hate people who make bad calls.
                            Logger?.LogWarning($"Error while serving : {RemoteEndPoint} - {ex}");
                            try
                            {
                                Client.Close();
                            }
                            catch
                            {
                                // ignored
                            }
                        }
                    }, Logger).ConfigureAwait(false);

                    if (!result)
                    {
                        Client.Close();
                    }
                }
            }
            catch (Exception e)
            {
                // Hate people who make bad calls.
                Logger?.LogWarning($"Error while serving : {RemoteEndPoint} - {e}");
                try
                {
                    Client.Close();
                }
                catch
                {
                    // ignored
                }
            }

            GC.Collect();
            Logger?.LogInformation("Lost Client {0}", RemoteEndPoint);
        }

        private async Task ProcessHttp1(HttpContext context)
        {
            var streamWriter = new StreamWriter(new MemoryStream());
            await HttpClientHandler.WriteResponse(context, streamWriter).ConfigureAwait(false);
            var bytes = ((MemoryStream) streamWriter.BaseStream).ToArray();
            await Stream.WriteAsync(bytes, 0, bytes.Length).ConfigureAwait(false);

            if (!(context.Request.Headers.KeepAliveConnection() && context.Response.Headers.KeepAliveConnection()))
                Client.Close();
        }

        private async Task ProcessHttp2(HttpContext context)
        {
            var h2Client = (H2Client) context.Client;
            var payloadStream = new MemoryStream();
            await context.Response.WriteBody(new StreamWriter(payloadStream));
            var payload = new ArraySegment<byte>(payloadStream.ToArray());
            await h2Client.Base.WriteHeadersAsync
            (
                context.Response.Headers.Select(x => new HeaderField() {Name = x.Key, Value = x.Value}).Concat
                (
                    new[]
                    {
                        new HeaderField()
                            {Name = ":status", Value = ((int) context.Response.ResponseCode).ToString()}
                    }
                ), payload.Count == 0
            ).ConfigureAwait(false);
            if (payload.Count != 0)
            {
                await h2Client.Base.WriteAsync(payload, true).ConfigureAwait(false);
            }
        }

        private void UpdateLastOperationTime()
        {
            // _lastOperationTime = DateTime.Now;
        }
    }
}
