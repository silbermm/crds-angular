using crds_angular.Models;
using crds_angular.Models.Crossroads;
using crds_angular.Services.Interfaces;

namespace crds_angular.Services
{
    public class CongregationService : ICongregationService
    {
        private readonly MinistryPlatform.Translation.Services.Interfaces.ICongregationService _congregationService;

        public CongregationService(MinistryPlatform.Translation.Services.Interfaces.ICongregationService congregationService)
        {
            _congregationService = congregationService;
        }

        public Congregation GetCongregationById(int id)
        {
            var tmp = _congregationService.GetCongregationById(id);
            var c = new Congregation();
            c.CongregationId = tmp.CongregationId;
            c.LocationId = tmp.LocationId;
            c.Name = tmp.Name;

            return c;
        }
    }
}