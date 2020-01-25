using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using System.Threading;

namespace E.Deezer.Util
{
    internal static class Compat
    {
        public static class Task
        {
#if NET45 || NETSTANDARD11
            private static readonly System.Threading.Tasks.Task COMPLETED_TASk = System.Threading.Tasks.Task.Run(() =>{ });
#endif


            public static System.Threading.Tasks.Task CompletedTask()
            {
#if NET45 || NETSTANDARD11
                return COMPLETED_TASk;
#else
                return System.Threading.Tasks.Task.CompletedTask;
#endif
            }


            public static System.Threading.Tasks.Task<T> FromCanceled<T>()
            {
#if NET45 || NETSTANDARD11
                var tcs = new TaskCompletionSource<T>();
                tcs.SetCanceled();

                return tcs.Task;
#else
                return System.Threading.Tasks.Task.FromCanceled<T>(CancellationTokenSourceExtensions.PRE_CANCELLED_TOKEN);
#endif
            }
        }

    }
}
