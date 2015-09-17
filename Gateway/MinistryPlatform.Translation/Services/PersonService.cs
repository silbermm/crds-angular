using System;
using System.Collections.Generic;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class PersonService : BaseService, IPersonService
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IConfigurationWrapper _configurationWrapper;
        private readonly IMinistryPlatformService _ministryPlatformService;

        public PersonService(IAuthenticationService authenticationService, IConfigurationWrapper configurationWrapper, IMinistryPlatformService ministryPlatformService) 
            : base(authenticationService, configurationWrapper)
        {
            this._authenticationService = authenticationService;
            this._configurationWrapper = configurationWrapper;
            this._ministryPlatformService = ministryPlatformService;
        }

        public void UpdateProfile(int contactId, Dictionary<string, object> profileDictionary, Dictionary<string,object> householdDictionary, Dictionary<string, object> addressDictionary  )
        {

            WithApiLogin<int>(token =>
            {
                try
                {
                    _ministryPlatformService.UpdateRecord(_configurationWrapper.GetConfigIntValue("Contacts"), profileDictionary, token);
                    if (addressDictionary["Address_ID"] != null)
                    {
                        //address exists, update it
                        _ministryPlatformService.UpdateRecord(_configurationWrapper.GetConfigIntValue("Addresses"), addressDictionary, token);
                    }
                    else
                    {
                        //address does not exist, create it, then attach to household
                        var addressId = _ministryPlatformService.CreateRecord(_configurationWrapper.GetConfigIntValue("Addresses"), addressDictionary, token);
                        householdDictionary.Add("Address_ID", addressId);
                    }
                    _ministryPlatformService.UpdateRecord(_configurationWrapper.GetConfigIntValue("Households"), householdDictionary, token);
                    return 1;
                }
                catch (Exception e)
                {
                    return 0;
                }
                
            });
        }
    }
}