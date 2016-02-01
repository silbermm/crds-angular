using System.Collections.Generic;
using crds_angular.Models.Crossroads.Stewardship;

namespace crds_angular.DataAccess.Interfaces
{
    public interface ICheckScannerDao
    {
        List<CheckScannerBatch> GetBatches(bool onlyOpenBatches = true);
        List<CheckScannerCheck> GetChecksForBatch(string batchName);
        CheckScannerBatch UpdateBatchStatus(string batchName, BatchStatus newStatus);
        void UpdateCheckStatus(int checkId, bool exported, string errorMessage = null);
    }
}
