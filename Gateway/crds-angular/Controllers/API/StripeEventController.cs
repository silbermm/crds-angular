using System;
using log4net;
using System.Web.Http;
using crds_angular.Services.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using crds_angular.Models.Crossroads;
using Crossroads.Utilities.Interfaces;

namespace crds_angular.Controllers.API
{
    public class StripeEventController : ApiController
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(StripeEventController));
        private readonly IPaymentService _paymentService;
        private readonly IDonationService _donationService;
        private readonly bool _liveMode;
        private readonly int _donationStatusDeposited;
        private readonly int _donationStatusSucceeded;

        public StripeEventController(IPaymentService paymentService, IDonationService donationService, IConfigurationWrapper configuration)
        {
            _paymentService = paymentService;
            _donationService = donationService;

            var b = configuration.GetConfigValue("StripeWebhookLiveMode");
            _liveMode = b != null && bool.Parse(b);

            _donationStatusDeposited = configuration.GetConfigIntValue("DonationStatusDeposited");
            _donationStatusSucceeded = configuration.GetConfigIntValue("DonationStatusSucceeded");
        }

        [Route("api/stripe-event")]
        [HttpPost]
        public IHttpActionResult ProcessStripeEvent([FromBody]StripeEvent stripeEvent)
        {
            if (stripeEvent == null || !ModelState.IsValid)
            {
                if (_logger.IsDebugEnabled)
                {
                    _logger.Debug("Received invalid Stripe event " + stripeEvent);
                }
                return (BadRequest(ModelState));
            }

            _logger.Debug("Received Stripe Event " + stripeEvent.Type);
            if (_liveMode != stripeEvent.LiveMode)
            {
                _logger.Debug("Dropping Stripe Event " + stripeEvent.Type + " because LiveMode was " + stripeEvent.LiveMode);
                return (Ok());
            }

            switch (stripeEvent.Type)
            {
                case "charge.succeeded":
                    ChargeSucceeded(stripeEvent.Created, ParseStripeEvent<StripeCharge>(stripeEvent.Data));
                    break;
                case "transfer.paid":
                    TransferPaid(stripeEvent.Created, ParseStripeEvent<StripeTransfer>(stripeEvent.Data));
                    break;
                default:
                    _logger.Debug("Ignoring event " + stripeEvent.Type);
                    break;
            }
            return (Ok());
        }

        private void ChargeSucceeded(DateTime? eventTimestamp, StripeCharge charge)
        {
            _logger.Debug("Processing charge.succeeded event for charge id " + charge.Id);
            _donationService.UpdateDonationStatus(charge.Id, _donationStatusSucceeded, eventTimestamp);
        }

        private void TransferPaid(DateTime? eventTimestamp, StripeTransfer transfer)
        {
            _logger.Debug("Processing transfer.paid event for transfer id " + transfer.Id);
            var charges = _paymentService.GetChargesForTransfer(transfer.Id);
            if (charges == null || charges.Count <= 0)
            {
                _logger.Debug("No charges found for transfer: " + transfer.Id);
                return;
            }

            _logger.Debug(string.Format("{0} charges to update for transfer {1}", charges.Count, transfer.Id));
            foreach (var charge in charges)
            {
                _logger.Debug("Updating charge id " + charge + " to Deposited status");
                _donationService.UpdateDonationStatus(charge, _donationStatusDeposited, eventTimestamp);
            }
        }

        private static T ParseStripeEvent<T>(StripeEventData data)
        {
            var jObject = data != null ? data.Object as JObject : null;
            return jObject != null ? JsonConvert.DeserializeObject<T>(jObject.ToString()) : (default(T));
        }
    }
}