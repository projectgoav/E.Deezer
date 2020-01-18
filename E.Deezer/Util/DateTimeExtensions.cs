using System;
using System.Collections.Generic;
using System.Text;

using System.Globalization;

namespace E.Deezer.Util
{
    public static class DateTimeExtensions
    {
#if NET45 || NETSTANDARD11
        private static readonly DateTime UNIX_EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
#endif

        public const string API_DATE_FORMAT = "yyyy-MM-dd";
        public const string DEFAULT_DATE = "0000-00-00";

        public static DateTime? ParseApiDateTime(string dateString)
        {
            bool isEmpty = string.IsNullOrEmpty(dateString);
            bool isDefault = dateString == DEFAULT_DATE;

            if (isEmpty || isDefault)
                return null;

            return DateTime.ParseExact(dateString, API_DATE_FORMAT, CultureInfo.InvariantCulture);
        }


        public static DateTime? ParseUnixTimeFromSeconds(uint seconds)
        {
            DateTime? parsed = null;

            try
            {
#if NETSTANDARD20
                parsed = DateTimeOffset.FromUnixTimeSeconds(seconds)
                                       .UtcDateTime;
#else
                parsed = UNIX_EPOCH.AddSeconds(seconds);
#endif
            }
            catch
            {
                //TODO: Log something here?
                //TODO: Do something better here about parsing.
            }

            return parsed;
        }


    }
}
