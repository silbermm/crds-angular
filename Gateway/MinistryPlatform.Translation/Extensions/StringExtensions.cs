using System;

namespace MinistryPlatform.Translation.Extensions
{
    public static class StringExtensions
    {
        public static int ToInt(this string input, bool throwExceptionIfFailed = false)
        {
            int result;
            var valid = int.TryParse(input, out result);
            if (valid) return result;

            if (throwExceptionIfFailed)
                throw new FormatException(string.Format("'{0}' cannot be converted as int", input));
            return result;
        }

        public static int? ToNullableInt(this string input, bool throwExceptionIfFailed = false)
        {
            int result;

            if (input == null) return null;

            var valid = int.TryParse(input, out result);
            if (valid) return result;

            if (throwExceptionIfFailed)
                throw new FormatException(string.Format("'{0}' cannot be converted as int", input));
            return result;
        }

        public static bool ToBool(this string input, bool throwExceptionIfFailed = false)
        {
            bool result;
            var valid = bool.TryParse(input, out result);
            if (valid) return result;

            if (throwExceptionIfFailed)
                throw new FormatException(string.Format("'{0}' cannot be converted as bool", input));
            return result;

        }

        public static string DateToString(this string input, bool throwExceptionIfFailed = false)
        {
            DateTime result;
            var valid = DateTime.TryParse(input, out result);
            if (valid)
            {
                return result.ToString("d");
            }

            if (throwExceptionIfFailed)
                throw new FormatException(string.Format("'{0}' cannot be converted as DateTime", input));
            return string.Empty;

        }
    }
}
