// 
// MiscellaneousExtensions.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

using Ultz.Spirit.Core;

namespace Ultz.Spirit.Extensions
{
    public static class MiscellaneousExtensions
    {
        public static string GetRawUrl(this IHttpRequest request)
        {
            return request.Uri.OriginalString;
        }
    }
}
