using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public interface ICongregationService
    {
        void GetCongregationById(int id);
    }

    public class CongregationService : BaseService, ICongregationService
    {
        private readonly IMinistryPlatformService _ministryPlatformService;

        public CongregationService(IMinistryPlatformService ministryPlatformService, IAuthenticationService authenticationService, IConfigurationWrapper configuration)
            : base(authenticationService, configuration)
        {
            _ministryPlatformService = ministryPlatformService;
        }

        public void GetCongregationById(int id)
        {
            var token = ApiLogin();
            var pageId = 466;
            var tmp = _ministryPlatformService.GetRecord(pageId, id, token);
        }
    }
}