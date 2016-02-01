using System;
using System.Collections.Generic;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class LookupService : BaseService, ILookupService
    {
        private readonly IMinistryPlatformService _ministryPlatformServiceImpl;

        public LookupService(IAuthenticationService authenticationService, IConfigurationWrapper configurationWrapper, IMinistryPlatformService ministryPlatformServiceImpl)
            : base(authenticationService, configurationWrapper)
        {
            _ministryPlatformServiceImpl = ministryPlatformServiceImpl;
        }

        public Dictionary<string, object> EmailSearch(string email, string token)
        {
            return _ministryPlatformServiceImpl.GetLookupRecord(AppSettings("Emails"), email, token);
        }

        public List<Dictionary<string, object>> EventTypes(string token)
        {
            return _ministryPlatformServiceImpl.GetLookupRecords(AppSettings("EventTypesLookup"), token);
        }

        public List<Dictionary<string, object>> Genders(string token)
        {
            return _ministryPlatformServiceImpl.GetLookupRecords(AppSettings("Genders"), token);
        }

        public List<Dictionary<string, object>> MaritalStatus(string token)
        {
            return _ministryPlatformServiceImpl.GetLookupRecords(AppSettings("MaritalStatus"), token);
        }

        public List<Dictionary<string, object>> ServiceProviders(string token)
        {
            return _ministryPlatformServiceImpl.GetLookupRecords(AppSettings("ServiceProviders"), token);
        }

        public List<Dictionary<string, object>> States(string token)
        {
            return _ministryPlatformServiceImpl.GetLookupRecords(AppSettings("States"), token);
        }

        public List<Dictionary<string, object>> Countries(string token)
        {
            return _ministryPlatformServiceImpl.GetLookupRecords(AppSettings("Countries"), token);
        }

        public List<Dictionary<string, object>> CrossroadsLocations(string token)
        {
            return _ministryPlatformServiceImpl.GetLookupRecords(AppSettings("CrossroadsLocations"), token);
        }

        public List<Dictionary<string, object>> ReminderDays(string token)
        {
            return _ministryPlatformServiceImpl.GetLookupRecords(AppSettings("ReminderDaysLookup"), token);
        }

        public List<Dictionary<string, object>> WorkTeams(string token)
        {
            return _ministryPlatformServiceImpl.GetLookupRecords(AppSettings("WorkTeams"), token);
        }
    }
}