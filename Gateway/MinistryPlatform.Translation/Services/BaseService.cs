using System;
using System.Configuration;

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

        protected static T WithApiLogin<T>(Func<string, T> doIt)
        {
            return (doIt(apiLogin()));
        }

        protected static string apiLogin()
        {
            return (AuthenticationService.authenticate(ConfigurationManager.AppSettings["ApiUser"], ConfigurationManager.AppSettings["ApiPass"]));
        }
    }
}
