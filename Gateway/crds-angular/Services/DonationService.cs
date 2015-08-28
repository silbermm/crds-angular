using System;
using System.Collections.Generic;
using System.IO;
using AutoMapper;
using crds_angular.Models.Crossroads.Stewardship;
using MPServices=MinistryPlatform.Translation.Services.Interfaces;
using crds_angular.Services.Interfaces;
using crds_angular.Util;
using MinistryPlatform.Models;
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
            var batchId = _mpDonationService.CreateDonationBatch(batch.BatchName, batch.SetupDateTime, batch.BatchTotalAmount,batch.ItemCount, batch.BatchEntryType, batch.DepositId, batch.FinalizedDateTime, batch.ProcessorTransferId);

            batch.Id = batchId;

            foreach (var donation in batch.Donations)
            {
                _mpDonationService.AddDonationToBatch(batchId, int.Parse(donation.donation_id));
            }

            return (batch);
        }

        public DonationBatchDTO GetDonationBatchByProcessorTransferId(string processorTransferId)
        {
            return (Mapper.Map<DonationBatch, DonationBatchDTO>(_mpDonationService.GetDonationBatchByProcessorTransferId(processorTransferId)));
        }

        public DonationBatchDTO GetDonationBatch(int batchId)
        {
            return (Mapper.Map<DonationBatch, DonationBatchDTO>(_mpDonationService.GetDonationBatch(batchId)));
        }

        public DonationBatchDTO GetDonationBatchByDepositId(int depositId)
        {
            return (Mapper.Map<DonationBatch, DonationBatchDTO>(_mpDonationService.GetDonationBatchByDepositId(depositId)));
        }

        public List<DepositDTO> GetSelectedDonationBatches(int selectionId, string token)
        {
            var selectedDeposits = _mpDonationService.GetSelectedDonationBatches(selectionId, token);
            var deposits = new List<DepositDTO>();

            foreach (var deposit in selectedDeposits)
            {
                deposits.Add(Mapper.Map<Deposit, DepositDTO>(deposit));
            }

            return deposits;
        }

        public void ProcessDeclineEmail(string processorPaymentId)
        {
            _mpDonationService.ProcessDeclineEmail(processorPaymentId);
        }

        public DepositDTO CreateDeposit(DepositDTO deposit)
        {
            deposit.Id = _mpDonationService.CreateDeposit(deposit.DepositName, deposit.DepositTotalAmount, deposit.DepositAmount, deposit.ProcessorFeeTotal, deposit.DepositDateTime,
                deposit.AccountNumber, deposit.BatchCount, deposit.Exported, deposit.Notes, deposit.ProcessorTransferId);
            
            return (deposit);

        }

        public void CreatePaymentProcessorEventError(StripeEvent stripeEvent, StripeEventResponseDTO stripeEventResponse)
        {
            _mpDonationService.CreatePaymentProcessorEventError(stripeEvent.Created, stripeEvent.Id, stripeEvent.Type, JsonConvert.SerializeObject(stripeEvent, Formatting.Indented), JsonConvert.SerializeObject(stripeEventResponse, Formatting.Indented));
        }

        public MemoryStream CreateGPExport(int depositId, string token)
        {
            var gpExport = _mpDonationService.CreateGPExport(depositId, token);
            var stream = new MemoryStream();
            CSV.Create(gpExport, GPExportDatum.Headers, stream, "\t");

            return stream;
        }
        public void UpdateDepositToExported(int depositId)
        {
            _mpDonationService.UpdateDepositToExported(depositId);
        }

        public List<DepositDTO> GenerateGPExportFileNames(int selectionId, string token)
        {
            var deposits = GetSelectedDonationBatches(selectionId, token);

            foreach (var deposit in deposits)
            {
                deposit.ExportFileName = GPExportFileName(deposit.Id);
            }

            return deposits;
        }

        public string GPExportFileName(int depositId)
        {
            var batch = GetDonationBatchByDepositId(depositId);

            var date = DateTime.Today.ToString("MMyy");
            return string.Format("{0}_{1}.csv", batch.BatchName, date);
        }
    }
}