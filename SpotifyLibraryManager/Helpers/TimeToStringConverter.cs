using System;
using System.Globalization;

namespace SpotifyLibraryManager.Helpers
{
    public static class TimeToStringConverter
    {
        private static CultureInfo _lang = new("en-US");
        public static string FromMiliseconds(int miliseconds)
        {
            TimeSpan totalDuration = TimeSpan.FromMilliseconds(miliseconds);

            if (totalDuration.Hours > 0)
                return totalDuration.Hours + " h " + totalDuration.Minutes + " min";
            else
                return totalDuration.Minutes + " min " + totalDuration.Seconds + " s";
        }

        public static string ToLongDateString(string someDate, string prefix)
        {
            if (DateTime.TryParse(someDate, out DateTime parsedReleaseDate))
            {
                return prefix + " at " + parsedReleaseDate.ToString(_lang.DateTimeFormat.LongDatePattern, _lang);
            }
            else { return prefix + " in " + someDate; }
        }
    }
}
