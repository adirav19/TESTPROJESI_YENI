using System.Text.Json;

namespace TESTPROJESI.Core.Extensions
{
    /// <summary>
    /// 🔧 JSON işlemleri için extension metodlar
    /// </summary>
    public static class JsonExtensions
    {
        private static readonly JsonSerializerOptions DefaultOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = false
        };

        public static string GetStringSafe(this JsonElement element, string propertyName, string defaultValue = "")
        {
            if (!element.TryGetProperty(propertyName, out var prop))
                return defaultValue;

            return prop.ValueKind switch
            {
                JsonValueKind.String => prop.GetString() ?? defaultValue,
                JsonValueKind.Number => prop.GetRawText(),
                JsonValueKind.Null => defaultValue,
                _ => defaultValue
            };
        }

        public static decimal GetDecimalSafe(this JsonElement element, string propertyName, decimal defaultValue = 0)
        {
            if (!element.TryGetProperty(propertyName, out var prop))
                return defaultValue;

            if (prop.ValueKind == JsonValueKind.Number && prop.TryGetDecimal(out var num))
                return num;

            if (prop.ValueKind == JsonValueKind.String && decimal.TryParse(prop.GetString(), out var strNum))
                return strNum;

            return defaultValue;
        }

        public static int GetIntSafe(this JsonElement element, string propertyName, int defaultValue = 0)
        {
            if (!element.TryGetProperty(propertyName, out var prop))
                return defaultValue;

            if (prop.ValueKind == JsonValueKind.Number && prop.TryGetInt32(out var num))
                return num;

            if (prop.ValueKind == JsonValueKind.String && int.TryParse(prop.GetString(), out var strNum))
                return strNum;

            return defaultValue;
        }

        public static bool GetBoolSafe(this JsonElement element, string propertyName, bool defaultValue = false)
        {
            if (!element.TryGetProperty(propertyName, out var prop))
                return defaultValue;

            return prop.ValueKind switch
            {
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                JsonValueKind.String when bool.TryParse(prop.GetString(), out var boolVal) => boolVal,
                _ => defaultValue
            };
        }

        public static DateTime? GetDateTimeSafe(this JsonElement element, string propertyName)
        {
            if (!element.TryGetProperty(propertyName, out var prop))
                return null;

            if (prop.ValueKind == JsonValueKind.String && DateTime.TryParse(prop.GetString(), out var date))
                return date;

            return null;
        }

        public static string ToJson<T>(this T obj, bool indented = false)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = indented
            };

            return JsonSerializer.Serialize(obj, options);
        }

        public static T? FromJson<T>(this string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return default;

            return JsonSerializer.Deserialize<T>(json, DefaultOptions);
        }

        public static JsonElement UnwrapData(this JsonElement element)
        {
            if (element.ValueKind == JsonValueKind.Object &&
                element.TryGetProperty("Data", out var data))
            {
                return data;
            }

            return element;
        }

        public static List<T> ToList<T>(this JsonElement element)
        {
            var list = new List<T>();

            if (element.ValueKind != JsonValueKind.Array)
                return list;

            foreach (var item in element.EnumerateArray())
            {
                var obj = JsonSerializer.Deserialize<T>(item.GetRawText(), DefaultOptions);
                if (obj != null)
                    list.Add(obj);
            }

            return list;
        }
    }
}
