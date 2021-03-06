﻿// 
// Program.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

#region

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.Extensions.Logging.Console;
using Ultz.Extensions.Spirit.Handlers;
using Ultz.Extensions.Spirit.Routing;
using Ultz.Extensions.Spirit.Ssl;
using Ultz.Spirit;
using Ultz.Spirit.Core;
using Ultz.Spirit.Extensions;
using Ultz.Spirit.Headers;
using Ultz.Spirit.Http.One;
using Ultz.Spirit.Http.Two;
using Ultz.Spirit.Responses;

#endregion

namespace DemoOne
{
    internal class Program
    {
        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
            using (var httpServer = new HttpServer(new H2RequestProvider()))
            {
                // Normal port
                httpServer.Use(new TcpListenerAdapter(new TcpListener(IPAddress.Loopback, 81)));

                // Ssl Support :
                var serverCertificate = TempCer.Get();
                var loggerProvider = new ConsoleLoggerProvider((s, level) => true, false);
                httpServer.Use
                (
                    new SslListenerBuilder().WithH2()
                        .WithTls12()
                        .WithServerCertificate(serverCertificate)
                        .WithChild(new TcpListenerAdapter(new TcpListener(IPAddress.Loopback, 444)))
                        .Build()
                );

                // Request handling : 
               // httpServer.Use
               // (
               //     (context, next) =>
               //     {
               //         Console.WriteLine("Got Request!");
               //         return next();
               //     }
               // );

                // Handler classes : 
                //httpServer.Use(new TimingHandler());
                httpServer.Use(new PostTest());
                httpServer.Use(loggerProvider);
                httpServer.Use
                (
                    new HttpRouter().With(string.Empty, new IndexHandler())
                        .With("about", new AboutHandler())
                );
                httpServer.Use(new FileHandler());
                httpServer.Use(new ErrorHandler());

                httpServer.Start();

                Console.ReadLine();
            }
        }
    }
}
