using System;
using System.Collections.Generic;

namespace MinistryPlatform.Translation.Extensions
{
    public static class DictionaryExtensions
    {
        public static string ToString(this Dictionary<string, object> input, string key)
        {
            return input[key]==null ? null : input[key].ToString();
        }

        public static int ToInt(this Dictionary<string, object> input, string key, bool throwExceptionIfFailed = false)
        {
            // TODO: catch  when key does not exist in "input"
            if (input[key] == null)
            {
                if (throwExceptionIfFailed)
                {
                    throw new FormatException(string.Format("'{0}' cannot be converted as int", key));
                }
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
                if (throwExceptionIfFailed)
                {
                    throw new FormatException(string.Format("'{0}' cannot be converted as DateTime - null value", key));
                }
                return string.Empty;
            }

            DateTime result;
            var valid = DateTime.TryParse(input[key].ToString(), out result);
            if (valid)
            {
                return result.ToString("MM/dd/yyyy");
            }

            if (throwExceptionIfFailed)
                throw new FormatException(string.Format("'{0}' cannot be converted as DateTime", key));
            return string.Empty;
        }

        public static bool ToBool(this Dictionary<string, object> input, string key, bool throwExceptionIfFailed = false)
        {
            if (input[key] == null)
            {
                if (throwExceptionIfFailed)
                {
                    throw new FormatException(string.Format("'{0}' cannot be converted as Bool - null value", key));
                }
                return false;
            }

            bool result;
            var valid = bool.TryParse(input[key].ToString(), out result);
            if (valid) return result;

            if (throwExceptionIfFailed)
                throw new FormatException(string.Format("'{0}' cannot be converted as bool", key));
            return result;
        }

    }
}
