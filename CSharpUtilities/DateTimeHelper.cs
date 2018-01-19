using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpUtilities
{
    public static class DateTimeHelper
    {
        /// <summary>
        /// Convert Unixtimestap to DateTime
        /// </summary>
        /// <param name="unixTimeStamp"></param>
        /// <returns></returns>
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static DateTime? GetLatestDate(List<DateTime> dates)
        {
            DateTime max = DateTime.MinValue; // Start with the lowest value possible...
            foreach (DateTime date in dates)
            {
                if (DateTime.Compare(date, max) == 1)
                    max = date;
            }
            if (max == DateTime.MinValue) return null;
            return max;
        }
    }
}
