using System;
using System.Collections.Generic;

namespace MinistryPlatform.Translation.Extensions
{
    public static class DictionaryExtensions
    {
        public static string ToString(this Dictionary<string, object> input, string key)
        {
            var dictVal = DictVal(input, key);
            return dictVal == null ? null : dictVal.ToString();
        }

        public static int ToInt(this Dictionary<string, object> input, string key, bool throwExceptionIfFailed = false)
        {
            var dictVal = DictVal(input, key);
            if (dictVal == null)
            {
                if (throwExceptionIfFailed)
                {
                    throw new FormatException(string.Format("'{0}' cannot be converted as int", key));
                }
                return 0;
            }

            int result;
            var valid = int.TryParse(dictVal.ToString(), out result);
            if (valid)
            {
                return result;
            }

            if (throwExceptionIfFailed)
            {
                throw new FormatException(string.Format("'{0}' cannot be converted as int", key));
            }
            return result;
        }

        public static T ToNullableObject<T>(this Dictionary<string, object> input,
                                            string key,
                                            bool throwExceptionIfFailed = false) where T : class
        {
            var dictVal = DictVal(input, key);
            if (dictVal == null)
            {
                return null;
            }

            var val = dictVal as T;
            if (val != null)
            {
                return val;
            }
            else
            {
                if (throwExceptionIfFailed)
                {
                    throw new FormatException(string.Format("'{0}' cannot be converted to Opportunity", key));
                }
            }
            return null;
        }

        public static int? ToNullableInt(this Dictionary<string, object> input, string key, bool throwExceptionIfFailed = false)
        {
            var dictVal = DictVal(input, key);
            if (dictVal == null)
            {
                return null;
            }

            int result;
            var valid = int.TryParse(dictVal.ToString(), out result);
            if (valid)
            {
                return result;
            }

            if (throwExceptionIfFailed)
            {
                throw new FormatException(string.Format("'{0}' cannot be converted as int", key));
            }
            return result;
        }

        public static TimeSpan? ToNullableTimeSpan(this Dictionary<string, object> input, string key, bool throwExceptionIfFailed = false)
        {
            var dictVal = DictVal(input, key);
            if (dictVal == null)
            {
                return null;
            }

            TimeSpan result;
            var valid = TimeSpan.TryParse(dictVal.ToString(), out result);
            if (valid)
            {
                return result;
            }
            if (throwExceptionIfFailed)
            {
                throw new FormatException(string.Format("'{0}' cannot be converted to TimeSpan", key));
            }
            return result;
        }

        public static DateTime ToDate(this Dictionary<string, object> input, string key, bool throwExceptionIfFailed = false)
        {
            var dictVal = DictVal(input, key);
            if (dictVal == null)
            {
                if (throwExceptionIfFailed)
                {
                    throw new FormatException(string.Format("'{0}' cannot be converted as DateTime - null value", key));
                }
                return new DateTime();
            }

            DateTime result;
            var valid = DateTime.TryParse(dictVal.ToString(), out result);
            if (valid)
            {
                return result;
            }

            if (throwExceptionIfFailed)
            {
                throw new FormatException(string.Format("'{0}' cannot be converted as DateTime", key));
            }
            return result;
        }

        public static DateTime? ToNullableDate(this Dictionary<string, object> input, string key, bool throwExceptionIfFailed = false)
        {
            var dictVal = DictVal(input, key);
            if (dictVal == null)
            {
                return null;
            }

            DateTime result;
            var valid = DateTime.TryParse(dictVal.ToString(), out result);
            if (valid)
            {
                return result;
            }

            if (throwExceptionIfFailed)
            {
                throw new FormatException(string.Format("'{0}' cannot be converted as DateTime", key));
            }
            return result;
        }

        public static string ToDateAsString(this Dictionary<string, object> input, string key, bool throwExceptionIfFailed = false)
        {
            var dictVal = DictVal(input, key);
            if (dictVal == null)
            {
                if (throwExceptionIfFailed)
                {
                    throw new FormatException(string.Format("'{0}' cannot be converted as DateTime - null value", key));
                }
                return string.Empty;
            }

            DateTime result;
            var valid = DateTime.TryParse(dictVal.ToString(), out result);
            if (valid)
            {
                return result.ToString("MM/dd/yyyy");
            }

            if (throwExceptionIfFailed)
            {
                throw new FormatException(string.Format("'{0}' cannot be converted as DateTime", key));
            }
            return string.Empty;
        }

        public static bool ToBool(this Dictionary<string, object> input, string key, bool throwExceptionIfFailed = false)
        {
            var dictVal = DictVal(input, key);
            if (dictVal == null)
            {
                if (throwExceptionIfFailed)
                {
                    throw new FormatException(string.Format("'{0}' cannot be converted as Bool - null value", key));
                }
                return false;
            }

            bool result;
            var valid = bool.TryParse(dictVal.ToString(), out result);
            if (valid)
            {
                return result;
            }

            if (throwExceptionIfFailed)
            {
                throw new FormatException(string.Format("'{0}' cannot be converted as bool", key));
            }
            return result;
        }

        private static object DictVal(Dictionary<string, object> input, string key)
        {
            object dictVal;
            try
            {
                dictVal = input[key];
            }
            catch (KeyNotFoundException)
            {
                throw new KeyNotFoundException(string.Format("'{0}' key not found", key));
            }
            return dictVal;
        }
    }
}