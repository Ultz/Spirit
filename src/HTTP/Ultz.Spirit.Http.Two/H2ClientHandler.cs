using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Http2.Hpack;
using Microsoft.Extensions.Logging;
using Ultz.Spirit.Core;
using Ultz.Spirit.Handling;
using Ultz.Spirit.Headers;
using Ultz.Spirit.Http.One;

namespace Ultz.Spirit.Http.Two
{
    public class H2ClientHandler : HttpClientHandlerBase
    {
        public H2ClientHandler
        (
            IClient client,
            Func<IHttpContext, Task> requestHandler,
            IHttpRequestProvider requestProvider,
            ILogger logger
        ) : base(client, requestHandler, requestProvider, logger)
        {
        }

        protected override async void Process()
        {
            try
            {
                while (Client.Connected)
                {
                    var notFlushingStream = new NotFlushingStream(Stream);
                    var streamReader = new StreamReader(notFlushingStream);

                    var result = await RequestProvider.Provide(streamReader, async request =>
                    {
                        var context = new HttpContext(request, Client.RemoteEndPoint, request is H2RequestDecorator req ? new H2Client(req.Stream, Client, req.Connection) : Client);

                        Logger?.LogInformation("{1} : Got request {0}", request.Uri, Client.RemoteEndPoint);

                        await RequestHandler(context).ConfigureAwait(false);

                        if (context.Response != null)
                        {
                            if (request is H2RequestDecorator h2Request)
                            {
                                await WriteResponse(h2Request, context);
                            }
                            else
                            {
                                var streamWriter = new StreamWriter(new MemoryStream());
                                await HttpClientHandler.WriteResponse(context, streamWriter).ConfigureAwait(false);
                                var bytes = ((MemoryStream) streamWriter.BaseStream).ToArray();
                                await notFlushingStream.WriteAsync(bytes, 0, bytes.Length);
                                await notFlushingStream.ExplicitFlushAsync().ConfigureAwait(false);

                                if (!request.Headers.KeepAliveConnection() || context.Response.CloseConnection)
                                    Client.Close();
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
                Logger?.LogWarning($"Error while serving : {RemoteEndPoint}", e);
                try
                {
                    Client.Close();
                }
                catch
                {
                    // connection's probably aborted by remote, ignore exception.
                }
            }

            Logger?.LogInformation("Lost Client {0}", RemoteEndPoint);
        }

        private static async Task WriteResponse(H2RequestDecorator h2Request, IHttpContext context)
        {
            var stream = h2Request.Stream;
            var sw = new StreamWriter(new MemoryStream());
            await context.Response.WriteBody(sw);
            var payload = ((MemoryStream) sw.BaseStream).ToArray();
            var hasPayload = payload.Length != 0;
            await stream.WriteHeadersAsync
            (
                context.Response.Headers.Where
                        (x => !x.Key.StartsWith(":"))
                    .Select(x => new HeaderField() {Name = x.Key, Value = x.Value})
                    .Concat
                    (
                        new[]
                        {
                            new HeaderField()
                                {Name = ":status", Value = ((int) context.Response.ResponseCode).ToString()}
                        }
                    ), !hasPayload
            );
            if (hasPayload)
            {
                await stream.WriteAsync(new ArraySegment<byte>(payload), true);
            }
        }
    }
}
