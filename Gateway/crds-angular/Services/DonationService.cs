using System;
using crds_angular.Models.Crossroads.Stewardship;
using MPServices=MinistryPlatform.Translation.Services.Interfaces;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Models.DTO;
using Newtonsoft.Json;

namespace crds_angular.Services
{
    public class DonationService: IDonationService
    {
        private readonly MPServices.IDonationService _mpDonationService;

        public DonationService(MPServices.IDonationService mpDonationService)
        {
            _mpDonationService = mpDonationService;
        }

        public DonationDTO GetDonationByProcessorPaymentId(string processorPaymentId)
        {
            var d = _mpDonationService.GetDonationByProcessorPaymentId(processorPaymentId);
            if (d == null)
            {
                return (null);
            }

            var donation = new DonationDTO
            {
                amount = d.donationAmt,
                donation_id = d.donationId + "",
                batch_id = d.batchId
            };
            return (donation);
        }

        public int UpdateDonationStatus(int donationId, int statusId, DateTime? statusDate, string statusNote = null)
        {
            return(_mpDonationService.UpdateDonationStatus(donationId, statusId, statusDate ?? DateTime.Now, statusNote));
        }

        public int UpdateDonationStatus(string processorPaymentId, int statusId, DateTime? statusDate, string statusNote = null)
        {
            return(_mpDonationService.UpdateDonationStatus(processorPaymentId, statusId, statusDate ?? DateTime.Now, statusNote));
        }

        public DonationBatchDTO CreateDonationBatch(DonationBatchDTO batch)
        {
            var batchId = _mpDonationService.CreateDonationBatch(batch.BatchName, batch.SetupDateTime,
                batch.BatchTotalAmount, batch.ItemCount, batch.BatchEntryType, batch.DepositId, batch.FinalizedDateTime, batch.ProcessorTransferId);

            batch.Id = batchId;

            foreach (var donation in batch.Donations)
            {
                _mpDonationService.AddDonationToBatch(batchId, int.Parse(donation.donation_id));
            }

            return (batch);
        }

        public DonationBatchDTO GetDonationBatchByProcessorTransferId(string processorTransferId)
        {
            return(MapDonationBatchDto(_mpDonationService.GetDonationBatchByProcessorTransferId(processorTransferId)));
        }

        public DonationBatchDTO GetDonationBatch(int batchId)
        {
            return (MapDonationBatchDto(_mpDonationService.GetDonationBatch(batchId)));
        }

        private static DonationBatchDTO MapDonationBatchDto(DonationBatch b)
        {
            if (b == null)
            {
                return (null);
            }

            var batch = new DonationBatchDTO
            {
                Id = b.Id,
                DepositId = b.DepositId,
                ProcessorTransferId = b.ProcessorTransferId
            };
            return (batch);
        }

        public void ProcessDeclineEmail(string processorPaymentId)
        {
            _mpDonationService.ProcessDeclineEmail(processorPaymentId);
        }

        public DepositDTO CreateDeposit(DepositDTO deposit)
        {
            deposit.Id = _mpDonationService.CreateDeposit(deposit.DepositName, deposit.DepositTotalAmount, deposit.DepositDateTime,
                deposit.AccountNumber, deposit.BatchCount, deposit.Exported, deposit.Notes, deposit.ProcessorTransferId);
            
            return (deposit);

        }

        public void CreatePaymentProcessorEventError(StripeEvent stripeEvent, StripeEventResponseDTO stripeEventResponse)
        {
            _mpDonationService.CreatePaymentProcessorEventError(stripeEvent.Created, stripeEvent.Id, stripeEvent.Type, JsonConvert.SerializeObject(stripeEvent, Formatting.Indented), JsonConvert.SerializeObject(stripeEventResponse, Formatting.Indented));
        }
    }
}