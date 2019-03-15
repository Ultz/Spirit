// 
// TaskFactoryExtensions.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

#region

using System.Threading.Tasks;

#endregion

namespace Ultz.Spirit.Extensions
{
    public static class TaskFactoryExtensions
    {
        private static readonly Task CompletedTask = Task.FromResult<object>(null);

        public static Task GetCompleted(this TaskFactory factory)
        {
            return CompletedTask;
        }
    }
}
