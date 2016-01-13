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
            var pageId = 466;
            var recordDict = _ministryPlatformService.GetRecordDict(pageId, id, token);
            //var tmp = _ministryPlatformService.get
            var c = new Congregation();
            c.CongregationId = recordDict.ToInt("Congregation_ID");
            c.Name = recordDict.ToString("Congregation_Name");
            c.LocationId = recordDict.ToInt("Location_ID");
            return c;
        }
    }
}