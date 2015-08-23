using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.WebPages;

namespace crds_angular.Util
{
    public static class CSV
    {
        private const string Quote = "\"";
        private const string EscapedQuote = "\"\"";
        private static readonly char[] EscapableCharacters = { '"', ',', '\r', '\n' };

        public static void ToCsv(List<Object> list, Stream stream, string delimiter)
        {
            var sw = new StreamWriter(stream, Encoding.UTF8);
            bool haveHeaders = false;

            foreach (var item in list)
            {
                if (!haveHeaders)
                {
                    haveHeaders = true;
                }
            }

            // Reset the stream position to the beginning
            stream.Seek(0, SeekOrigin.Begin);
        }

        private static Dictionary<PropertyInfo, string> GetProperties(Type type, string[] onlyFields)
        {
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                                    .Where(prop => IsSimpleOrNullableType(prop.PropertyType))
                                    .OrderBy(prop =>
                                    {
                                        var displayAttr = prop.GetCustomAttributes(typeof(DisplayAttribute), true).FirstOrDefault() as DisplayAttribute;
                                        return displayAttr != null ? displayAttr.Order : int.MaxValue;
                                    });

            // If the entity has MetadataTypeAttribute's, use them
            var metadata = (MetadataTypeAttribute[])type.GetCustomAttributes(typeof(MetadataTypeAttribute), true);

            var names = new Dictionary<PropertyInfo, string>();
            foreach (var property in properties)
            {
                if (onlyFields.Length == 0 || onlyFields.Contains(property.Name, StringComparer.InvariantCultureIgnoreCase))
                {
                    var text = GetDisplayName(property, metadata);

                    names.Add(property, text);
                }
            }

            return names;
        }

        private static string GetDisplayName(PropertyInfo property, IEnumerable<MetadataTypeAttribute> metadata)
        {
            // Extract the display name from the DisplayAttribute on the object or on any of the MetadataTypeAttribute's 
            // it may contain
            var displayText = metadata.Select(m => m.MetadataClassType.GetProperty(property.Name))
                    .Where(p => p != null)
                    .SelectMany(p => (DisplayAttribute[])p.GetCustomAttributes(typeof(DisplayAttribute), true))
                    .Concat((DisplayAttribute[])property.GetCustomAttributes(typeof(DisplayAttribute), true))
                    .Select(m => m.GetName())
                    .FirstOrDefault(n => !string.IsNullOrEmpty(n));

            // Return the display text if found, otherwise return the property name
            return displayText ?? property.Name;
        }

        private static bool IsSimpleOrNullableType(Type t)
        {
            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                t = Nullable.GetUnderlyingType(t);
            }

            return IsSimpleType(t);
        }

        private static bool IsSimpleType(Type t)
        {
            return t.IsPrimitive || t.IsEnum || t == typeof(string) || t == typeof(Decimal) || t == typeof(DateTime) || t == typeof(Guid);
        }

        private static void WriteRow(TextWriter sw, IEnumerable<string> values)
        {
            int index = 0;
            foreach (var value in values)
            {
                if (index > 0)
                {
                    sw.Write(",");
                }

                WriteValue(sw, value);
                index++;
            }

            sw.Write(Environment.NewLine);
            sw.Flush();
        }

        private static void WriteValue(TextWriter sw, string value)
        {
            bool needsEscaping = value.IndexOfAny(EscapableCharacters) >= 0;

            if (needsEscaping)
            {
                sw.Write(Quote);
                sw.Write(value.Replace(Quote, EscapedQuote));
                sw.Write(Quote);
            }
            else
            {
                sw.Write(value);
            }
        }
    }
}