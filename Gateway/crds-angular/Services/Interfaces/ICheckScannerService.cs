using System.Collections.Generic;
using crds_angular.Models.Crossroads.Stewardship;
using MinistryPlatform.Models;

namespace crds_angular.Services.Interfaces
{
    public interface ICheckScannerService
    {
        List<CheckScannerBatch> GetBatches(bool onlyOpenBatches = true);
        List<CheckScannerCheck> GetChecksForBatch(string batchName);
        CheckScannerBatch UpdateBatchStatus(string batchName, BatchStatus newStatus);
        CheckScannerBatch CreateDonationsForBatch(CheckScannerBatch batchDetails);
        EZScanDonorDetails GetContactDonorForCheck(string encryptedKey);
        ContactDonor CreateDonor(CheckScannerCheck batchDetails);
    }
}
