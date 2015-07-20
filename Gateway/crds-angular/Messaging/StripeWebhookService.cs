using System;
using System.ServiceModel;
using crds_angular.Controllers.API;
using crds_angular.Messaging.Contracts;
using crds_angular.Models.Crossroads.Stewardship;
using crds_angular.Services.Interfaces;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace crds_angular.Messaging
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class StripeWebhookService : IStripeWebhookService
    {
        private readonly IStripeEventService _stripeEventService;
        private readonly ILog _logger = LogManager.GetLogger(typeof(StripeWebhookService));

        public StripeWebhookService(IStripeEventService stripeEventService)
        {
            _stripeEventService = stripeEventService;
        }

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        public StripeEventResponseDTO OnWebhookReceived(StripeEvent stripeEvent)
        {
            StripeEventResponseDTO response = null;
            try
            {
                switch (stripeEvent.Type)
                {
                    case "charge.succeeded":
                        _stripeEventService.ChargeSucceeded(stripeEvent.Created, ParseStripeEvent<StripeCharge>(stripeEvent.Data));
                        break;
                    case "charge.failed":
                        _stripeEventService.ChargeFailed(stripeEvent.Created, ParseStripeEvent<StripeCharge>(stripeEvent.Data));
                        break;
                    case "transfer.paid":
                        response = _stripeEventService.TransferPaid(stripeEvent.Created, ParseStripeEvent<StripeTransfer>(stripeEvent.Data));
                        break;
                    default:
                        _logger.Debug("Ignoring event " + stripeEvent.Type);
                        break;
                }
            }
            catch (Exception e)
            {
                var msg = "Unexpected error processing Stripe Event " + stripeEvent.Type;
                _logger.Error(msg, e);
                var responseDto = new StripeEventResponseDTO()
                {
                    Exception = new ApplicationException(msg, e),
                    Message = msg
                };
                return (responseDto);
            }

            return (response ?? new StripeEventResponseDTO { });
        }

        private static T ParseStripeEvent<T>(StripeEventData data)
        {
            var jObject = data != null && data.Object != null ? data.Object as JObject : null;
            return jObject != null ? JsonConvert.DeserializeObject<T>(jObject.ToString()) : (default(T));
        }
    }
}