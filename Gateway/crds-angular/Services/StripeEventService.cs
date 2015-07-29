﻿using System;
using System.Collections.Generic;
using crds_angular.Controllers.API;
using crds_angular.Models.Crossroads.Stewardship;
using crds_angular.Services.Interfaces;
using log4net;
using Crossroads.Utilities.Interfaces;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace crds_angular.Services
{

    public class StripeEventService : IStripeEventService
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(StripeEventController));
        private readonly IPaymentService _paymentService;
        private readonly IDonationService _donationService;
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
            _donationService.ProcessDeclineEmail(charge.Id);
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

            if (response.FailedUpdates.Count > 0)
            {
                response.Exception = new ApplicationException("Could not update all charges to 'deposited' status, see message for details");
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

        public StripeEventResponseDTO ProcessStripeEvent(StripeEvent stripeEvent)
        {
            StripeEventResponseDTO response = null;
            try
            {
                switch (stripeEvent.Type)
                {
                    case "charge.succeeded":
                        ChargeSucceeded(stripeEvent.Created, ParseStripeEvent<StripeCharge>(stripeEvent.Data));
                        break;
                    case "charge.failed":
                        ChargeFailed(stripeEvent.Created, ParseStripeEvent<StripeCharge>(stripeEvent.Data));
                        break;
                    case "transfer.paid":
                        response = TransferPaid(stripeEvent.Created, ParseStripeEvent<StripeTransfer>(stripeEvent.Data));
                        break;
                    default:
                        _logger.Debug("Ignoring event " + stripeEvent.Type);
                        break;
                }
                if (response != null && response.Exception != null)
                {
                    RecordFailedEvent(stripeEvent, response);
                }
            }
            catch (Exception e)
            {
                response = new StripeEventResponseDTO
                {
                    Exception = new ApplicationException("Problem processing Stripe event", e)
                };
                RecordFailedEvent(stripeEvent, response);
                throw;
            }
            return (response);
        }

        public void RecordFailedEvent(StripeEvent stripeEvent, StripeEventResponseDTO stripeEventResponse)
        {
            try
            {
                _donationService.CreatePaymentProcessorEventError(stripeEvent, stripeEventResponse);
            }
            catch (Exception e)
            {
                _logger.Error("Error writing event to failure log", e);
            }
        }

        private static T ParseStripeEvent<T>(StripeEventData data)
        {
            var jObject = data != null && data.Object != null ? data.Object as JObject : null;
            return jObject != null ? JsonConvert.DeserializeObject<T>(jObject.ToString()) : (default(T));
        }
    }

    // ReSharper disable once InconsistentNaming
    public class StripeEventResponseDTO
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("exception")]
        public ApplicationException Exception { get; set; }
    }

    // ReSharper disable once InconsistentNaming
    public class TransferPaidResponseDTO : StripeEventResponseDTO
    {
        [JsonProperty("transaction_count")]
        public int TotalTransactionCount { get; set; }

        [JsonProperty("successful_updates")]
        public List<string> SuccessfulUpdates { get { return (_successfulUpdates); } }
        private readonly List<string> _successfulUpdates = new List<string>();

        [JsonProperty("failed_updates")]
        public List<KeyValuePair<string, string>> FailedUpdates { get { return (_failedUpdates); } }
        private readonly List<KeyValuePair<string, string>> _failedUpdates = new List<KeyValuePair<string, string>>();

        [JsonProperty("donation_batch")]
        public DonationBatchDTO Batch;

        [JsonProperty("deposit")]
        public DepositDTO Deposit;
    }
}