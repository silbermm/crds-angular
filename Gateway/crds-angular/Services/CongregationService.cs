using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MinistryPlatform.Translation.Services;

namespace crds_angular.Services
{
    public interface ICongregationService
    {
        void GetCongregationById(int id);
    }

    public class CongregationService : ICongregationService
    {
        private readonly ICongregationService _congregationService;

        public CongregationService(ICongregationService congregationService)
        {
            _congregationService = congregationService;
        }

        public void GetCongregationById(int id)
        {
            _congregationService.GetCongregationById(id);
        }
    }

    public class Congregation
    {
        int CongregationId { get; set; }
        string Name { get; set; }
        int LocationId { get; set; }
    }
}