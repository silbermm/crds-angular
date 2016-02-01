using crds_angular.Models.Crossroads;

namespace crds_angular.Services.Interfaces
{
    public interface IDonorStatementService
    {
        DonorStatementDTO GetDonorStatement(string token);
        void SaveDonorStatement(string token, DonorStatementDTO donorStatement);
    }
}
