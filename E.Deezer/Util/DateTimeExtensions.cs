using System;

using System.Globalization;

namespace E.Deezer.Util
{
    public static class DateTimeExtensions
    {
        public const string API_DATE_FORMAT = "yyyy-MM-dd";
        public const string API_DATE_TIME_FORMAT = "yyyy-MM-dd HH:mm:ss";
        public const string DEFAULT_DATE = "0000-00-00";

        public static DateTime? ParseApiDate(string dateString)
        {
            bool isEmpty = string.IsNullOrEmpty(dateString);
            bool isDefault = dateString == DEFAULT_DATE;

            if (isEmpty || isDefault)
                return null;

            return DateTime.ParseExact(dateString, API_DATE_FORMAT, CultureInfo.InvariantCulture);
        }

        public static DateTime? ParseApiDateTime(string dateTimeString)
        {
            bool isEmpty = string.IsNullOrEmpty(dateTimeString);
            bool isDefault = dateTimeString == DEFAULT_DATE;

            if (isEmpty || isDefault)
                return null;

            return DateTime.ParseExact(dateTimeString, API_DATE_TIME_FORMAT, CultureInfo.InvariantCulture);
        }

        public static DateTime? ParseUnixTimeFromSeconds(uint seconds)
        {
            DateTime? parsed = null;

            try
            {
                parsed = DateTimeOffset.FromUnixTimeSeconds(seconds)
                                       .UtcDateTime;
            }
            catch
            {
                // This is a bit nasty, but if we're failing to parse we'll
                // simply let this return null.

                // In the future we should include some logging or a way to incidate
                // this portion of the code has been hit.
            }

            return parsed;
        }


    }
}
