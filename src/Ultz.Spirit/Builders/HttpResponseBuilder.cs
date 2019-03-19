// 
// HttpResponseBuilder.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Text;
using Ultz.Spirit.Core;

namespace Ultz.Spirit.Builders
{
    public class HttpResponseBuilder
    {
        private HttpResponseCode _code = HttpResponseCode.Ok;
        private List<KeyValuePair<string, string>> _headers = new List<KeyValuePair<string, string>>();
        private MemoryStream _content = new MemoryStream();
        private string _contentType = "text/html";
        private bool _keepAlive = true;

        public HttpResponseBuilder WithHeader(string name, string value)
        {
            _headers.Add(new KeyValuePair<string, string>(name, value));
            return this;
        }

        public HttpResponseBuilder WithStatusCode(HttpResponseCode code)
        {
            _code = code;
            return this;
        }

        public HttpResponseBuilder WithContentType(ContentType type)
        {
            _contentType = type.ToString();
            return this;
        }

        public HttpResponseBuilder WithContentType(string contentType)
        {
            _contentType = contentType;
            return this;
        }

        public HttpResponseBuilder WithContent(byte[] bytes)
        {
            _content = new MemoryStream(bytes);
            return this;
        }

        public HttpResponseBuilder WithContent(Action<StreamWriter> writer)
        {
            using (var sw = new StreamWriter(_content = new MemoryStream(), Encoding.UTF8, 1024, true))
            {
                writer(sw);
            }

            return this;
        }

        public HttpResponseBuilder WithKeepAlive(bool close = true)
        {
            _keepAlive = close;
            return this;
        }

        public IHttpResponse Build()
        {
            return new HttpResponse(_code, _contentType, _content, _keepAlive, _headers);
        }

        public HttpResponseBuilder WithContent(string content)
        {
            using (var sw = new StreamWriter(_content = new MemoryStream(), Encoding.UTF8, 1024, true))
            {
                sw.Write(content);
                sw.Flush();
            }

            return this;
        }
    }
}
