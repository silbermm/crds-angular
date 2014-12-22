using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using crds_angular.Models;

namespace crds_angular.Services
{
    public class HouseholdService : MinistryPlatformBaseService
    {
        //public void setProfile(String token, Person person)
        //{
        //    var dictionary = getDictionary(person);

        //    MinistryPlatform.Translation.Services.UpdatePageRecordService.UpdateRecord(474, dictionary, token);
        //    //MinistryPlatform.Translation.Services.UpdatePageRecordService.UpdateRecord(465, getDictionary(household), token);       
        //}
        
        
        public Models.Household getLoggedinUserHousehold(string token)
        {
            var contactId = MinistryPlatform.Translation.AuthenticationService.GetContactId(token);
            JArray household = MinistryPlatform.Translation.Services.GetPageRecordService.GetRecord(474, contactId, token);
            var householdJson = TranslationService.DecodeJson(household.ToString());
            return householdJson;
        }

    }
}