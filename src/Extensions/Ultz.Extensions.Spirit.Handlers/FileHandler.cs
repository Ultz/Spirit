// 
// Class1.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Ultz.Spirit;
using Ultz.Spirit.Core;
using Ultz.Spirit.Headers;

#endregion

namespace Ultz.Extensions.Spirit.Handlers
{
    public class FileHandler : IHttpRequestHandler
    {
        static FileHandler()
        {
            DefaultMimeType = "text/plain";
            MimeTypes = new Dictionary<string, string>
            {
                {".css", "text/css"},
                {".gif", "image/gif"},
                {".htm", "text/html"},
                {".html", "text/html"},
                {".jpg", "image/jpeg"},
                {".js", "application/javascript"},
                {".png", "image/png"},
                {".xml", "application/xml"}
            };
        }

        public static string DefaultMimeType { get; set; }
        public static string HttpRootDirectory { get; set; }
        public static IDictionary<string, string> MimeTypes { get; }

        public async Task Handle(IHttpContext context, Func<Task> next)
        {
            var requestPath = context.Request.Uri.OriginalString.TrimStart('/');

            var httpRoot = Path.GetFullPath(HttpRootDirectory ?? ".");
            var path = Path.GetFullPath(Path.Combine(httpRoot, requestPath));

            if (!File.Exists(path))
            {
                await next().ConfigureAwait(false);

                return;
            }

            context.Response = new HttpResponse
                (GetContentType(path), File.OpenRead(path), context.Request.Headers.KeepAliveConnection());
        }

        private string GetContentType(string path)
        {
            var extension = Path.GetExtension(path) ?? string.Empty;
            return MimeTypes.ContainsKey(extension) ? MimeTypes[extension] : DefaultMimeType;
        }
    }
}
