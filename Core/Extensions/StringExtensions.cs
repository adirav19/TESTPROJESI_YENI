namespace TESTPROJESI.Core.Extensions
{
    /// <summary>
    /// 📝 String işlemleri için extension metodlar
    /// </summary>
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string? value)
            => string.IsNullOrEmpty(value);

        public static bool IsNullOrWhiteSpace(this string? value)
            => string.IsNullOrWhiteSpace(value);

        public static string SafeTrim(this string? value)
            => value?.Trim() ?? string.Empty;

        public static string Truncate(this string value, int maxLength, string suffix = "...")
        {
            if (string.IsNullOrEmpty(value) || value.Length <= maxLength)
                return value;

            return value.Substring(0, maxLength - suffix.Length) + suffix;
        }

        public static string ToTitleCase(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
        }

        public static bool IsNumeric(this string value)
            => !string.IsNullOrEmpty(value) && value.All(char.IsDigit);

        public static string TruncateAfter(this string value, char delimiter)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            int index = value.IndexOf(delimiter);
            return index >= 0 ? value.Substring(0, index) : value;
        }
    }
}
