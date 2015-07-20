using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using crds_angular.Controllers.API;
using crds_angular.Models.Crossroads.Stewardship;
using crds_angular.Services.Interfaces;
using log4net;
using Newtonsoft.Json;
using Crossroads.Utilities.Interfaces;
using System.Text;

namespace crds_angular.Services
{

    public class StripeEventService
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(StripeEventController));
        private readonly IPaymentService _paymentService;
        private readonly IDonationService _donationService;
        private readonly bool _liveMode;
        private readonly int _donationStatusDeclined;
        private readonly int _donationStatusDeposited;
        private readonly int _donationStatusSucceeded;
        private readonly int _batchEntryTypePaymentProcessor;

        // This value is used when creating the batch name for exporting to GP.  It must be 15 characters or less.
        private const string BatchNameDateFormat = @"\M\PyyyyMMddHHmm";

        public StripeEventService(IPaymentService paymentService, IDonationService donationService, IConfigurationWrapper configuration)
        {
            _paymentService = paymentService;
            _donationService = donationService;

            var b = configuration.GetConfigValue("StripeWebhookLiveMode");
            _liveMode = b != null && bool.Parse(b);

            _donationStatusDeclined = configuration.GetConfigIntValue("DonationStatusDeclined");
            _donationStatusDeposited = configuration.GetConfigIntValue("DonationStatusDeposited");
            _donationStatusSucceeded = configuration.GetConfigIntValue("DonationStatusSucceeded");
            _batchEntryTypePaymentProcessor = configuration.GetConfigIntValue("BatchEntryTypePaymentProcessor");
        }

        public void ChargeSucceeded(DateTime? eventTimestamp, StripeCharge charge)
        {
            _logger.Debug("Processing charge.succeeded event for charge id " + charge.Id);
            _donationService.UpdateDonationStatus(charge.Id, _donationStatusSucceeded, eventTimestamp);
        }

        public void ChargeFailed(DateTime? eventTimestamp, StripeCharge charge)
        {
            _logger.Debug("Processing charge.failed event for charge id " + charge.Id);
            var notes = new StringBuilder();
            notes.Append(charge.FailureCode ?? "No Stripe Failure Code")
                .Append(": ")
                .Append(charge.FailureMessage ?? "No Stripe Failure Message");
            _donationService.UpdateDonationStatus(charge.Id, _donationStatusDeclined, eventTimestamp, notes.ToString());
        }

        public TransferPaidResponseDTO TransferPaid(DateTime? eventTimestamp, StripeTransfer transfer)
        {
            _logger.Debug("Processing transfer.paid event for transfer id " + transfer.Id);
            var response = new TransferPaidResponseDTO();
            var charges = _paymentService.GetChargesForTransfer(transfer.Id);
            if (charges == null || charges.Count <= 0)
            {
                _logger.Debug("No charges found for transfer: " + transfer.Id);
                response.TotalTransactionCount = 0;
                return (response);
            }

            var now = DateTime.Now;
            var batchName = now.ToString(BatchNameDateFormat);

            var batch = new DonationBatchDTO()
            {
                BatchName = batchName,
                SetupDateTime = now,
                BatchTotalAmount = 0,
                ItemCount = 0,
                BatchEntryType = _batchEntryTypePaymentProcessor,
                FinalizedDateTime = now,
                DepositId = null,
                ProcessorTransferId = transfer.Id
            };

            response.TotalTransactionCount = charges.Count;
            _logger.Debug(string.Format("{0} charges to update for transfer {1}", charges.Count, transfer.Id));
            foreach (var charge in charges)
            {
                _logger.Debug("Updating charge id " + charge + " to Deposited status");
                try
                {
                    var donationId = _donationService.UpdateDonationStatus(charge.Id, _donationStatusDeposited, eventTimestamp);
                    response.SuccessfulUpdates.Add(charge.Id);
                    batch.ItemCount++;
                    batch.BatchTotalAmount += (charge.Amount / 100M);
                    batch.Donations.Add(new DonationDTO { donation_id = "" + donationId, amount = charge.Amount });
                }
                catch (Exception e)
                {
                    _logger.Warn("Error updating charge " + charge, e);
                    response.FailedUpdates.Add(new KeyValuePair<string, string>(charge.Id, e.Message));
                }
            }

            // Create the deposit
            var deposit = new DepositDTO
            {
                // Account number must be non-null, and non-empty; using a single space to fulfill this requirement
                AccountNumber = " ",
                BatchCount = 1,
                DepositDateTime = now,
                DepositName = batchName,
                // This is the amount from Stripe - will show out of balance if does not match batch total above
                DepositTotalAmount = transfer.Amount / 100M,
                Exported = false,
                Notes = null,
                ProcessorTransferId = transfer.Id
            };
            try
            {
                response.Deposit = _donationService.CreateDeposit(deposit);
            }
            catch (Exception e)
            {
                _logger.Error("Failed to create batch deposit", e);
                throw;
            }

            // Create the batch, with the above deposit id
            batch.DepositId = response.Deposit.Id;
            try
            {
                response.Batch = _donationService.CreateDonationBatch(batch);
            }
            catch (Exception e)
            {
                _logger.Error("Failed to create donation batch", e);
                throw;
            }

            return (response);
        }
    }
}