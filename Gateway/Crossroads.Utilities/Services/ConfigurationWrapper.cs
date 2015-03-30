using System;
using System.Configuration;
using Crossroads.Utilities.Interfaces;

namespace Crossroads.Utilities.Services
{
    public class ConfigurationWrapper : IConfigurationWrapper
    {
        public int GetMinistryPlatformId(string mpKey)
        {
            int pageId;
            if (!int.TryParse(ConfigurationManager.AppSettings[mpKey], out pageId))
            {
                throw new InvalidOperationException(string.Format("Invalid Page Key: {0}", mpKey));
            }
            return pageId;
        }

        public string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key];

        }

        public string GetEnvironmentVarAsString(string variable)
        {
            var value = Environment.GetEnvironmentVariable(variable);
            if (value == null)
            {
                throw new ApplicationException(string.Format("Invalid Environment Variable: {0}", variable));
            }
            return value;
        }
    }
}