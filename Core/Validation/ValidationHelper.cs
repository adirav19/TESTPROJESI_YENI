using System.ComponentModel.DataAnnotations;

namespace TESTPROJESI.Core.Validation
{
    /// <summary>
    /// ✅ Validation helper metodları
    /// </summary>
    public static class ValidationHelper
    {
        public static (bool IsValid, List<string> Errors) Validate<T>(T obj) where T : class
        {
            var context = new ValidationContext(obj);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(obj, context, results, true);

            var errors = results
                .Where(r => r.ErrorMessage != null)
                .Select(r => r.ErrorMessage!)
                .ToList();

            return (isValid, errors);
        }

        public static void ValidateAndThrow<T>(T obj) where T : class
        {
            var (isValid, errors) = Validate(obj);

            if (!isValid)
            {
                throw new ValidationException(string.Join(", ", errors));
            }
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            var emailAttribute = new EmailAddressAttribute();
            return emailAttribute.IsValid(email);
        }

        public static bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            var digits = new string(phone.Where(char.IsDigit).ToArray());
            return digits.Length == 10 || digits.Length == 11;
        }

        public static bool IsValidTcKimlikNo(string tcKimlikNo)
        {
            if (string.IsNullOrWhiteSpace(tcKimlikNo) || tcKimlikNo.Length != 11)
                return false;

            if (!tcKimlikNo.All(char.IsDigit))
                return false;

            if (tcKimlikNo[0] == '0')
                return false;

            var digits = tcKimlikNo.Select(c => int.Parse(c.ToString())).ToArray();

            var sum1 = digits[0] + digits[2] + digits[4] + digits[6] + digits[8];
            var sum2 = digits[1] + digits[3] + digits[5] + digits[7];
            var digit10 = ((sum1 * 7) - sum2) % 10;

            if (digit10 != digits[9])
                return false;

            var sum3 = digits.Take(10).Sum();
            var digit11 = sum3 % 10;

            return digit11 == digits[10];
        }

        public static bool IsValidIban(string iban)
        {
            if (string.IsNullOrWhiteSpace(iban))
                return false;

            iban = iban.Replace(" ", "").ToUpper();

            if (!iban.StartsWith("TR") || iban.Length != 26)
                return false;

            return iban.Skip(2).All(char.IsDigit);
        }

        public static bool IsValidVergiNo(string vergiNo)
        {
            if (string.IsNullOrWhiteSpace(vergiNo) || vergiNo.Length != 10)
                return false;

            return vergiNo.All(char.IsDigit);
        }

        public static bool IsValidUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return false;

            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult) &&
                   (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        public static bool IsValidDateRange(DateTime startDate, DateTime endDate)
        {
            return startDate <= endDate;
        }

        public static bool IsRequired(string? value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        public static bool IsValidLength(string? value, int minLength, int maxLength)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            return value.Length >= minLength && value.Length <= maxLength;
        }

        public static bool IsInRange(decimal value, decimal min, decimal max)
        {
            return value >= min && value <= max;
        }

        public static bool IsInRange(int value, int min, int max)
        {
            return value >= min && value <= max;
        }
    }
}
