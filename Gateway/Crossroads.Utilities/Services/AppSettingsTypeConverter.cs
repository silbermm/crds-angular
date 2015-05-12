using Crossroads.Utilities.Interfaces;
using System;
using System.ComponentModel;
using System.Globalization;

namespace Crossroads.Utilities.Services
{
    /// <summary>
    /// A TypeConverter to expose appSettings configuration entries to unity configuration.
    /// </summary>
    public class AppSettingsTypeConverter : TypeConverter
    {
        private IConfigurationWrapper configurationWrapper;

        public AppSettingsTypeConverter()
        {
            configurationWrapper = new ConfigurationWrapper();
        }

        /// <summary>
        /// Construct a new AppSettingsTypeConverter with the given IConfigurationWrapper.
        /// This is intended primarily for unit testing, as a TypeConverter requires a 
        /// zero-arg constructor to be used by Unity.
        /// </summary>
        /// <param name="configurationWrapper"></param>
        public AppSettingsTypeConverter(IConfigurationWrapper configurationWrapper)
        {
            this.configurationWrapper = configurationWrapper;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return (sourceType == typeof(string));
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return (configurationWrapper.GetConfigValue((string)value));
        }
    }
}
