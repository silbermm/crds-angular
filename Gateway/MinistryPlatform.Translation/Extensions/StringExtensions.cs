using System;
using System.Collections.Generic;

namespace MinistryPlatform.Translation.Extensions
{
    public static class StringExtensions
    {

        public static string GetValue<TKey>(this Dictionary<TKey, int> dict, TKey key, int value)
        {
            return "Sandi";
        }

        public static string ToString(this Dictionary<string, object> input, string key)
        {
            return input[key]==null ? string.Empty : input[key].ToString();
        }

        public static int ToInt(this Dictionary<string, object> input, string key, bool throwExceptionIfFailed = false)
        {
            if (input[key] == null)
            {
                return 0;
            }

            int result;
            var valid = int.TryParse(input[key].ToString(), out result);
            if (valid) return result;

            if (throwExceptionIfFailed)
                throw new FormatException(string.Format("'{0}' cannot be converted as int", key));
            return result;
        }

        public static int? ToNullableInt(this Dictionary<string, object> input, string key, bool throwExceptionIfFailed = false)
        {
            if (input[key] == null)
            {
                return null;
            }

            int result;
            var valid = int.TryParse(input[key].ToString(), out result);
            if (valid) return result;

            if (throwExceptionIfFailed)
                throw new FormatException(string.Format("'{0}' cannot be converted as int", key));
            return result;
        }

        public static string ToDateAsString(this Dictionary<string, object> input, string key, bool throwExceptionIfFailed = false)
        {
            if (input[key] == null)
            {
                return string.Empty;
            }

            DateTime result;
            var valid = DateTime.TryParse(input[key].ToString(), out result);
            if (valid)
            {
                return result.ToString("d");
            }

            if (throwExceptionIfFailed)
                throw new FormatException(string.Format("'{0}' cannot be converted as DateTime", key));
            return string.Empty;
        }

        public static bool ToBool(this Dictionary<string, object> input, string key, bool throwExceptionIfFailed = false)
        {
            bool result;
            var valid = bool.TryParse(input[key].ToString(), out result);
            if (valid) return result;

            if (throwExceptionIfFailed)
                throw new FormatException(string.Format("'{0}' cannot be converted as bool", key));
            return result;
        }

        //public static int ToInt(this string input, bool throwExceptionIfFailed = false)
        //{
        //    int result;
        //    var valid = int.TryParse(input, out result);
        //    if (valid) return result;

        //    if (throwExceptionIfFailed)
        //        throw new FormatException(string.Format("'{0}' cannot be converted as int", input));
        //    return result;
        //}

        //public static int? ToNullableInt(this string input, bool throwExceptionIfFailed = false)
        //{
        //    int result;

        //    if (input == null) return null;

        //    var valid = int.TryParse(input, out result);
        //    if (valid) return result;

        //    if (throwExceptionIfFailed)
        //        throw new FormatException(string.Format("'{0}' cannot be converted as int", input));
        //    return result;
        //}

        //public static bool ToBool(this string input, bool throwExceptionIfFailed = false)
        //{
        //    bool result;
        //    var valid = bool.TryParse(input, out result);
        //    if (valid) return result;

        //    if (throwExceptionIfFailed)
        //        throw new FormatException(string.Format("'{0}' cannot be converted as bool", input));
        //    return result;

        //}

        //public static string DateToString(this string input, bool throwExceptionIfFailed = false)
        //{
        //    DateTime result;
        //    var valid = DateTime.TryParse(input, out result);
        //    if (valid)
        //    {
        //        return result.ToString("d");
        //    }

        //    if (throwExceptionIfFailed)
        //        throw new FormatException(string.Format("'{0}' cannot be converted as DateTime", input));
        //    return string.Empty;

        //}
    }
}
