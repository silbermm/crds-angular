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
        private readonly bool _liveMode;

        public StripeEventController(IPaymentService paymentService, IConfigurationWrapper configuration)
        {
            _paymentService = paymentService;

            var b = configuration.GetConfigValue("StripeWebhookLiveMode");
            _liveMode = b != null && bool.Parse(b);
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
                    ChargeSucceeded(ParseStripeEvent<StripeCharge>(stripeEvent.Data));
                    break;
                case "transfer.paid":
                    TransferPaid(ParseStripeEvent<StripeTransfer>(stripeEvent.Data));
                    break;
                default:
                    _logger.Debug("Ignoring event " + stripeEvent.Type);
                    break;
            }
            return (Ok());
        }

        private void ChargeSucceeded(StripeCharge charge)
        {
            if (charge == null)
            {
                return;
            }

            _logger.Debug("Processing charge.succeeded event for charge id " + charge.Id);
            // TODO Call service to update charge to "Succeeded" status
        }

        private void TransferPaid(StripeTransfer transfer)
        {
            if (transfer == null)
            {
                return;
            }

            _logger.Debug("Processing transfer.paid event for transfer id " + transfer.Id);
            var charges = _paymentService.GetChargesForTransfer(transfer.Id);
            if (charges == null || charges.Count <= 0)
            {
                return;
            }

            foreach (var charge in charges)
            {
                _logger.Debug("Updating charge id " + charge + " to Deposited status");
                // TODO Call service to update charge to "Deposited" status
            }
        }

        private static T ParseStripeEvent<T>(StripeEventData data)
        {
            var jObject = data != null ? data.Object as JObject : null;
            return jObject != null ? JsonConvert.DeserializeObject<T>(jObject.ToString()) : (default(T));
        }
    }
}