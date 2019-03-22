# Spirit

Project Spirit is a very lightweight, simple, embedded HTTP server written in C# for .NET Standard. Spirit is heavily
based on µHttpSharp and is made in-house and supported by Ultz as part of their projects. This project supersedes
SimpleServer - another server library written by Ultz.

## Usage

A sample for usage : 

	using (var httpServer = new HttpServer(new HttpRequestProvider()))
	{
		// Normal port 80 :
		httpServer.Use(new TcpListenerAdapter(new TcpListener(IPAddress.Loopback, 80)));
        
		// Ssl Support :
		var serverCertificate = X509Certificate.CreateFromCertFile(@"TempCert.cer");
		httpServer.Use(new SslListener(new TcpListenerAdapter(new TcpListener(IPAddress.Loopback, 443)), serverCertificate));

		// Request handling : 
		httpServer.Use((context, next) => {
			Console.WriteLine("Got Request!");
			return next();
		});

		// Handler classes : 
		httpServer.Use(new TimingHandler());
		httpServer.Use(new HttpRouter().With(string.Empty, new IndexHandler())
										.With("about", new AboutHandler()));
		httpServer.Use(new FileHandler());
		httpServer.Use(new ErrorHandler());
		
		httpServer.Start();
		
		Console.ReadLine();
	}
	

## Features

Spirit is a simple HTTP server based on [µHttpSharp](https://github.com/bonesoul/uhttpsharp) and inspired by
[koa](http://koajs.com), and has the following features :

* [RESTful](http://en.wikipedia.org/wiki/Representational_state_transfer) controllers (coming soon)
* SSL/TLS Support
* Easy Chain-Of-Responsibility architecture
* Modularity
* .NET Standard support
* HTTP/2 over HTTPS

## Performance

```
Server Software:        Spirit/1.0
Document Path:          /
Document Length:        21 bytes
Requests per second:    5942.10 [#/sec] (mean)
Time per request:       0.168 [ms] (mean, across all concurrent requests)

Percentage of the requests served within a certain time (ms)
  50%      1
  66%      1
  75%      1
  80%      1
  90%      2
  95%      2
  98%      2
  99%      3
 100%      4 (longest request)
 ```

## How To Contribute?

Spirit uses and encourages [Early Pull Requests](https://medium.com/practical-blend/pull-request-first-f6bb667a9b6). Please don't wait until you're done to open a PR!

1. Install [Git](https://git-scm.com/downloads) and the [.NET Core SDK](https://www.microsoft.com/net/download)
2. [Fork Spirit](https://github.com/Ultz/Spirit/fork)
3. Create a branch on your fork.
4. Add an empty commit to start your work off (and let you open a PR): `git commit --allow-empty -m "start of [thing you're working on]"`
5. Open a **draft pull request** with `[WIP]` in the title. Do this **before** you actually start working.
6. Make your commits in small, incremental steps with clear descriptions.
7. Tag a maintainer when you're done and ask for a review!

*In memory of the Spirit and Opportunity rovers, providing 6 and 15 years of service respectively, after landing in
2004.*
