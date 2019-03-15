// 
// HttpRequest.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

#region

using System;
using System.Diagnostics;
using Ultz.Spirit.Core;
using Ultz.Spirit.Headers;

#endregion

namespace Ultz.Spirit
{
    [DebuggerDisplay("{Method} {OriginalUri,nq}")]
    public class HttpRequest : IHttpRequest
    {
        public HttpRequest
        (
            IHttpHeaders headers,
            HttpMethods method,
            string protocol,
            Uri uri,
            string[] requestParameters,
            IHttpHeaders queryString,
            IHttpPost post
        )
        {
            Headers = headers;
            Method = method;
            Protocol = protocol;
            Uri = uri;
            RequestParameters = requestParameters;
            QueryString = queryString;
            Post = post;
        }

        internal string OriginalUri
        {
            get
            {
                if (QueryString == null) return Uri.OriginalString;

                return Uri.OriginalString + "?" + QueryString.ToUriData();
            }
        }

        public IHttpHeaders Headers { get; }

        public HttpMethods Method { get; }

        public string Protocol { get; }

        public Uri Uri { get; }

        public string[] RequestParameters { get; }

        public IHttpPost Post { get; }

        public IHttpHeaders QueryString { get; }
    }
}
