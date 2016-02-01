﻿using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Crossroads.Utilities.Services
{
    /// <summary>
    /// Uses the EnumMember attribute to parse an enum from a string, or return a string value for a given enum.
    /// </summary>
    public class EnumMemberSerializationUtils
    {
        /// <summary>
        /// Converts the given Enum to a string value.  The value returned will be the Value on the EnumMember attribute.
        /// </summary>
        /// <typeparam name="T">The Enum Type to convert</typeparam>
        /// <param name="type">The enum value to convert to a string</param>
        /// <returns>The string Value on the EnumMember attribute</returns>
        public static string ToEnumString<T>(T type)
        {
            var enumType = typeof(T);
            var name = Enum.GetName(enumType, type);
            var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();
            return enumMemberAttribute.Value;
        }

        /// <summary>
        /// Convert a string representation of an enum to the Enum member.  This looks for an EnumMember attribute matching the given string.
        /// </summary>
        /// <typeparam name="T">The Enum Type to convert</typeparam>
        /// <param name="str">The string Value on the EnumMember attribute</param>
        /// <returns>The enum value from the given string representation, or the Enum's default value if not found</returns>
        public static T ToEnum<T>(string str)
        {
            var enumType = typeof(T);
            foreach (var name in Enum.GetNames(enumType))
            {
                var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();
                if (enumMemberAttribute.Value == str) return (T)Enum.Parse(enumType, name);
            }
            return default(T);
        }
    }
}
