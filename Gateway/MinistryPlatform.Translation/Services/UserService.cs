using System.Linq;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly IMinistryPlatformService _ministryPlatformService;
        private readonly int _usersApiLookupPageViewId;

        public UserService(IAuthenticationService authenticationService, IConfigurationWrapper configurationWrapper, IMinistryPlatformService ministryPlatformService) : base(authenticationService, configurationWrapper)
        {
            _ministryPlatformService = ministryPlatformService;
            _usersApiLookupPageViewId = _configurationWrapper.GetConfigIntValue("UsersApiLookupPageView");
        }

        public MinistryPlatformUser GetByUserId(string userId)
        {
            var searchString = string.Format("\"{0}\",", userId);
            return (GetUser(searchString));
        }

        public MinistryPlatformUser GetByAuthenticationToken(string authToken)
        {
            var contactId = _authenticationService.GetContactId(authToken);

            var searchString = string.Format(",\"{0}\"", contactId);
            return (GetUser(searchString));
        }

        private MinistryPlatformUser GetUser(string searchString)
        {
            var records = _ministryPlatformService.GetPageViewRecords(_usersApiLookupPageViewId, ApiLogin(), searchString);
            if (records == null || !records.Any())
            {
                return (null);
            }

            var record = records.First();
            var user = new MinistryPlatformUser
            {
                CanImpersonate = record["Can_Impersonate"] as bool? ?? false,
                Guid = record.ContainsKey("User_GUID") ? record["User_GUID"].ToString() : null,
                UserId = record["User_Name"] as string
            };

            return (user);
        }
    }
}
