using System;
using System.Collections.Generic;
using System.Text;

using System.Threading.Tasks;

namespace E.Deezer.Util
{
    internal static class TaskExtensions
    {
        public static void ThrowIfFaulted<T>(this Task<T> task)
        {
            if (task.IsFaulted)
            {
                throw task.Exception
                          .GetBaseException();
            }
        }
    }
}
