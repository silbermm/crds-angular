using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace crds_angular.Services
{
    public class HouseholdService
    {
        public static Models.Household getLoggedinUserHousehold(string token)
        {
            var contactId = MinistryPlatform.Translation.AuthenticationService.GetContactId(token);
            JArray household = MinistryPlatform.Translation.Services.GetPageRecordService.GetRecord(474, contactId, token);
            var householdJson = TranslationService.DecodeJson(household.ToString());
            return householdJson;
        }

    }
}