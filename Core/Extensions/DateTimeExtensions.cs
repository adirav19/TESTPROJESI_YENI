using TESTPROJESI.Core.Constants;

namespace TESTPROJESI.Core.Extensions
{
    /// <summary>
    /// 📅 DateTime işlemleri için extension metodlar
    /// </summary>
    public static class DateTimeExtensions
    {
        public static string ToApiFormat(this DateTime date)
        {
            return date.ToString(AppConstants.DateFormats.ApiFormat);
        }

        public static string ToDateOnlyFormat(this DateTime date)
        {
            return date.ToString(AppConstants.DateFormats.DateOnly);
        }

        public static string ToDisplayFormat(this DateTime date)
        {
            return date.ToString(AppConstants.DateFormats.DisplayFormat);
        }

        public static string ToDisplayFormatWithTime(this DateTime date)
        {
            return date.ToString(AppConstants.DateFormats.DisplayFormatWithTime);
        }

        public static DateTime? ToDateTimeSafe(this string? dateString)
        {
            if (string.IsNullOrWhiteSpace(dateString))
                return null;

            return DateTime.TryParse(dateString, out var result) ? result : null;
        }

        public static bool IsToday(this DateTime date)
        {
            return date.Date == DateTime.Today;
        }

        public static bool IsPast(this DateTime date)
        {
            return date < DateTime.Now;
        }

        public static bool IsFuture(this DateTime date)
        {
            return date > DateTime.Now;
        }

        public static int DaysDifference(this DateTime startDate, DateTime endDate)
        {
            return (endDate.Date - startDate.Date).Days;
        }

        public static DateTime StartOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        public static DateTime EndOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
        }

        public static DateTime StartOfYear(this DateTime date)
        {
            return new DateTime(date.Year, 1, 1);
        }

        public static DateTime EndOfYear(this DateTime date)
        {
            return new DateTime(date.Year, 12, 31);
        }

        public static DateTime StartOfWeek(this DateTime date)
        {
            var diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
            return date.AddDays(-1 * diff).Date;
        }

        public static DateTime EndOfWeek(this DateTime date)
        {
            return date.StartOfWeek().AddDays(6);
        }

        public static bool IsWeekday(this DateTime date)
        {
            return date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday;
        }

        public static bool IsWeekend(this DateTime date)
        {
            return !date.IsWeekday();
        }

        public static int CalculateAge(this DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;

            if (birthDate.Date > today.AddYears(-age))
                age--;

            return age;
        }

        public static long ToUnixTimestamp(this DateTime date)
        {
            return ((DateTimeOffset)date).ToUnixTimeSeconds();
        }

        public static DateTime FromUnixTimestamp(this long timestamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime;
        }
    }
}
