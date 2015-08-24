using System;
using System.Collections.Generic;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class LookupService : BaseService
    {
        public LookupService(IAuthenticationService authenticationService, IConfigurationWrapper configurationWrapper)
            : base(authenticationService, configurationWrapper)
        {
        }

        public static Dictionary<string, object> EmailSearch(String email, string token)
        {
            return MinistryPlatformService.GetLookupRecord(AppSettings("Emails"), email, token);
        }

        public static List<Dictionary<string, object>> Genders(string token)
        {
            return MinistryPlatformService.GetLookupRecords(AppSettings("Genders"), token);
        }

        public static List<Dictionary<string, object>> MaritalStatus(string token)
        {
            return MinistryPlatformService.GetLookupRecords(AppSettings("MaritalStatus"), token);
        }

        public static List<Dictionary<string, object>> ServiceProviders(string token)
        {
            return MinistryPlatformService.GetLookupRecords(AppSettings("ServiceProviders"), token);
        }

        public static List<Dictionary<string, object>> States(string token)
        {
            return MinistryPlatformService.GetLookupRecords(AppSettings("States"), token);
        }

        public static List<Dictionary<string, object>> Countries(string token)
        {
            return MinistryPlatformService.GetLookupRecords(AppSettings("Countries"), token);
        }

        public static List<Dictionary<string, object>> CrossroadsLocations(string token)
        {
            return MinistryPlatformService.GetLookupRecords(AppSettings("CrossroadsLocations"), token);
        }

        public static List<Dictionary<string, object>> WorkTeams(string token)
        {
            return MinistryPlatformService.GetLookupRecords(AppSettings("WorkTeams"), token);
        }
    }
}