using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

namespace E.Deezer
{
    /// <summary>
    /// Task Extensions to stop tasks throwing exceptions straight away.
    /// </summary>
    public static class TaskExtensions
    {
        public static Task SuppressExceptions(this Task aTask)
        {
            aTask.ContinueWith((t) => { var ignored = t.Exception; },
                TaskContinuationOptions.OnlyOnFaulted |
                TaskContinuationOptions.ExecuteSynchronously);
            return aTask;
        }
        public static Task<T> SuppressExceptions<T>(this Task<T> aTask)
        {
            aTask.ContinueWith((t) => { var ignored = t.Exception; },
                TaskContinuationOptions.OnlyOnFaulted |
                TaskContinuationOptions.ExecuteSynchronously);
            return aTask;
        }
    }
}
