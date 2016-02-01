using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Models.DTO;
using MinistryPlatform.Translation.Extensions;
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

        public MinistryPlatformUser GetUserByResetToken(string resetToken)
        {
            var searchString = string.Format(",,,,,\"{0}\"", resetToken);
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
                UserId = record["User_Name"] as string,
                UserEmail = record["User_Email"] as string,
                UserRecordId = Int32.Parse(record["dp_RecordID"].ToString())
            };

            return (user);
        }

        public List<RoleDto> GetUserRoles(int userId)
        {
            var records = _ministryPlatformService.GetSubpageViewRecords("User_Roles_With_ID", userId, ApiLogin());
            if (records == null || !records.Any())
            {
                return (null);
            }

            return records.Select(record => new RoleDto
            {
                Id = record.ToInt("Role_ID"), Name = record.ToString("Role_Name")
            }).ToList();
        }

        public void UpdateUser(Dictionary<string, object> userUpdateValues)
        {
            MinistryPlatformService.UpdateRecord(Convert.ToInt32(ConfigurationManager.AppSettings["Users"]), userUpdateValues, ApiLogin());
        }

        public int GetUserIdByUsername(string email)
        {
            var records = _ministryPlatformService.GetRecordsDict(Convert.ToInt32(ConfigurationManager.AppSettings["Users"]), ApiLogin(), ("," + email));
            if (records.Count != 1)
            {
                throw new ApplicationException("User email did not return exactly one user record");
            }

            var record = records[0];
            return record.ToInt("dp_RecordID");      
        }

        public int GetContactIdByUserId(int userId)
        {
            var records = _ministryPlatformService.GetPageViewRecords(2194, ApiLogin(), ("\"" + userId + "\",")); //
            if (records.Count != 1)
            {
                throw new Exception("User ID did not return exactly one user record");
            }

            var record = records[0];
            return record.ToInt("Contact ID");
        }

    }
}
