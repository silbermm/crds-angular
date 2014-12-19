using crds_angular.Models;
using crds_angular.Models.Crossroads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace crds_angular.Services
{
    public static class AccountService
    {

        public static AccountInfo getAccountInfo(string token)
        {
            var contactId = MinistryPlatform.Translation.AuthenticationService.GetContactId(token);
            Dictionary<string, object> contact = MinistryPlatform.Translation.Services.GetPageRecordService.GetRecordDict(455, contactId, token);            
            //Dictionary<string, object> household = MinistryPlatform.Translation.Services.GetPageRecordService.GetRecordDict(466, (int)contact["Household_ID"], token);

            var accountInfo = AutoMapper.Mapper.Map<AccountInfo>(contact);

            return accountInfo;


       }

    }
}