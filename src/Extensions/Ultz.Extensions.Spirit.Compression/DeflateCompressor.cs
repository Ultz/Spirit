// 
// DeflateCompressor.cs
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
    public class DeflateCompressor : ICompressor
    {
        public static readonly ICompressor Default = new DeflateCompressor();

        public string Name => "deflate";

        public Task<IHttpResponse> Compress(IHttpResponse response)
        {
            return CompressedResponse.CreateDeflate(response);
        }
    }
}
