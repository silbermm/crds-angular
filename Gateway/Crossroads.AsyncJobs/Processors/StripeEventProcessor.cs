using System;
using crds_angular.Models.Crossroads.Stewardship;
using crds_angular.Services.Interfaces;
using Crossroads.AsyncJobs.Interfaces;
using Crossroads.AsyncJobs.Models;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Crossroads.AsyncJobs.Processors
{
    public class StripeEventProcessor : IJobExecutor<StripeEvent>
    {
        private readonly IStripeEventService _stripeEventService;
        private readonly ILog _logger = LogManager.GetLogger(typeof(StripeEventProcessor));

        public StripeEventProcessor(IStripeEventService stripeEventService)
        {
            _stripeEventService = stripeEventService;
        }

        public void Execute(JobDetails<StripeEvent> details)
        {
            var stripeEvent = details.Data;

            _logger.Debug(string.Format("Received event {0} at {1} (queued at {2})", stripeEvent.Type, details.RetrievedDateTime, details.EnqueuedDateTime));

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
                        _stripeEventService.TransferPaid(stripeEvent.Created, ParseStripeEvent<StripeTransfer>(stripeEvent.Data));
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
            }
        }

        private static T ParseStripeEvent<T>(StripeEventData data)
        {
            var jObject = data != null && data.Object != null ? data.Object as JObject : null;
            return jObject != null ? JsonConvert.DeserializeObject<T>(jObject.ToString()) : (default(T));
        }
    }
}