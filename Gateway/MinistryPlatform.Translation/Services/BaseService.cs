using System;
using System.Configuration;
using Crossroads.Utilities.Services;

namespace MinistryPlatform.Translation.Services
{
    public class BaseService
    {
        protected static int AppSettings(string pageKey)
        {
            int pageId;
            if (!int.TryParse(ConfigurationManager.AppSettings[pageKey], out pageId))
            {
                throw new InvalidOperationException(string.Format("Invalid Page Key: {0}", pageKey));
            }
            return pageId;
        }

        protected static T          WithApiLogin<T>(Func<string, T> doIt)
        {
            return (doIt(apiLogin()));
        }

        protected static string apiLogin()
        {
            var configWrapper = new ConfigurationWrapper();
            var apiUser = configWrapper.GetEnvironmentVarAsString("API_USER");
            var apiPasword = configWrapper.GetEnvironmentVarAsString("API_PASSWORD");
            return (AuthenticationService.authenticate(apiUser, apiPasword));
        }

        protected static int AppSetting(string key)
        {
            int value;
            if (!int.TryParse(ConfigurationManager.AppSettings[key], out value))
            {
                throw new InvalidOperationException(string.Format("Invalid Page Key: {0}", key));
            }
            return value;
        }
    }
}
