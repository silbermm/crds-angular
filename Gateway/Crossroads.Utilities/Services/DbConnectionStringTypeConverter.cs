using System;
using System.ComponentModel;
using System.Configuration;
using System.Data.Common;

namespace Crossroads.Utilities.Services
{
    public class DbConnectionNameTypeConverter : TypeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            var connectionStringName = (string)value;
            var connectionStringSettings = ConfigurationManager.ConnectionStrings[connectionStringName];

            var providerFactory = DbProviderFactories.GetFactory(connectionStringSettings.ProviderName);
            var connection = providerFactory.CreateConnection();
            connection.ConnectionString = Environment.ExpandEnvironmentVariables(connectionStringSettings.ConnectionString);
            return connection;
        }
    }
}
