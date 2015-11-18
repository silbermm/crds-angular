using AutoMapper;
using crds_angular.Models.Crossroads;
using crds_angular.Services.Interfaces;
using log4net;
using MinistryPlatform.Models;
using IDonorService = MinistryPlatform.Translation.Services.Interfaces.IDonorService;

namespace crds_angular.Services
{
    public class DonorStatementService : IDonorStatementService
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof (DonorService));
        private readonly IDonorService _mpDonorService;


        public DonorStatementService(IDonorService mpDonorService)
        {
            _mpDonorService = mpDonorService;           
        }

        public DonorStatementDTO GetDonorStatement(string token)
        {            
            var mpDonorStatement = _mpDonorService.GetDonorStatement(token);            
            var donorStatement = Mapper.Map<DonorStatement, DonorStatementDTO>(mpDonorStatement);
            return donorStatement;
        }

        public void SaveDonorStatement(string token, DonorStatementDTO donorStatement)
        {
            var mpDonorStatement = Mapper.Map<DonorStatementDTO, DonorStatement>(donorStatement);
            _mpDonorService.UpdateDonorStatement(token, mpDonorStatement);
        }
    }
}