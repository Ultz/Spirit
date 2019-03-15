// 
// ICompressor.cs
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
    /// <summary>
    ///     Represents an object that can compress <see cref="IHttpResponse" />s.
    /// </summary>
    public interface ICompressor
    {
        /// <summary>
        ///     The name of the compression algorithm
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     Compresses the given <paramref name="response" />
        /// </summary>
        /// <param name="response"></param>
        /// <returns>The compressed response</returns>
        Task<IHttpResponse> Compress(IHttpResponse response);
    }
}
