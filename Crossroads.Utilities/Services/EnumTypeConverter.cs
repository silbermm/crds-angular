using System;
using System.ComponentModel;
using System.Globalization;

namespace Crossroads.Utilities.Services
{
    public class EnumTypeConverter<T> : TypeConverter where T : struct
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            T result;

            if (value == null || !Enum.TryParse(value.ToString(), out result))
            {
                throw new InvalidEnumArgumentException(string.Format("Could not locate enum value '{0}' for enum type {1}", value != null ? value.ToString() : "null", typeof(T)));
            }

            return result;
        }
    }
}
