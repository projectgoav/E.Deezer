using System;
using System.Collections.Generic;
using System.Text;

using System.Threading;

namespace E.Deezer.Util
{
    internal static class CancellationTokenSourceExtensions
    {
        internal static readonly CancellationToken PRE_CANCELLED_TOKEN = new CancellationToken(canceled: true);


        public static CancellationToken GetCancellationTokenSafe(this CancellationTokenSource source)
        {
            if (source == null)
                return PRE_CANCELLED_TOKEN;

            return source.IsCancellationRequested ? PRE_CANCELLED_TOKEN
                                                  : source.Token;
        }
    }
}
