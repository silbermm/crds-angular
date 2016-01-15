using System.Collections.Generic;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class CongregationService : BaseService, ICongregationService
    {
        private readonly IMinistryPlatformService _ministryPlatformService;

        public CongregationService(IMinistryPlatformService ministryPlatformService, IAuthenticationService authenticationService, IConfigurationWrapper configuration)
            : base(authenticationService, configuration)
        {
            _ministryPlatformService = ministryPlatformService;
        }

        public Congregation GetCongregationById(int id)
        {
            var token = ApiLogin();
            var pageId = _configurationWrapper.GetConfigIntValue("CrossroadsLocations");
            Dictionary<string, object> recordDict;
            try
            {
                recordDict = _ministryPlatformService.GetRecordDict(pageId, id, token);
            }
            catch (System.ServiceModel.FaultException fault)
            {
                // this is terrible, but can't find another way to handle!!!!
                if (fault.Message.StartsWith("Record is not found"))
                {
                    return null;
                }
                throw;
            }

            var c = new Congregation();
            c.CongregationId = recordDict.ToInt("Congregation_ID");
            c.Name = recordDict.ToString("Congregation_Name");
            c.LocationId = recordDict.ToInt("Location_ID");
            return c;
        }
    }
}