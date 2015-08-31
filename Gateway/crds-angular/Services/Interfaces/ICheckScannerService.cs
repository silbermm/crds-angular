using System.Collections.Generic;
using crds_angular.Models.Crossroads.Stewardship;

namespace crds_angular.Services.Interfaces
{
    public interface ICheckScannerService
    {
        List<CheckScannerBatch> GetOpenBatches();
        List<CheckScannerBatch> GetAllBatches();
        List<CheckScannerCheck> GetChecksForBatch(string batchName);
        CheckScannerBatch UpdateBatchStatus(string batchName, BatchStatus newStatus);
        CheckScannerBatch CreateDonationsForBatch(CheckScannerBatch batchDetails);
    }
}
