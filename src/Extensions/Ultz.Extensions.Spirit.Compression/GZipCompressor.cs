// 
// GZipCompressor.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

#region

using System.Threading.Tasks;
using Ultz.Spirit.Core;

#endregion

namespace Ultz.Extensions.Spirit.Compression
{
    public class GZipCompressor : ICompressor
    {
        public static readonly ICompressor Default = new GZipCompressor();

        public string Name => "gzip";

        public Task<IHttpResponse> Compress(IHttpResponse response)
        {
            return CompressedResponse.CreateGZip(response);
        }
    }
}
