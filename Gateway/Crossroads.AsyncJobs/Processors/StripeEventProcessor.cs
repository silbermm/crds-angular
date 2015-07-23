using System;
using crds_angular.Models.Crossroads.Stewardship;
using crds_angular.Services;
using crds_angular.Services.Interfaces;
using Crossroads.AsyncJobs.Interfaces;
using Crossroads.AsyncJobs.Models;
using log4net;

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

            try
            {
                _logger.Debug(string.Format("Received event {0} at {1} (queued at {2})", stripeEvent.Type, details.RetrievedDateTime, details.EnqueuedDateTime));
                _stripeEventService.ProcessStripeEvent(stripeEvent);
            }
            catch (Exception e)
            {
                var msg = "Unexpected error processing Stripe Event " + stripeEvent.Type;
                _logger.Error(msg, e);

                _stripeEventService.RecordFailedEvent(stripeEvent, new StripeEventResponseDTO
                {
                    Exception = new ApplicationException("Problem processing Stripe event", e)
                });
            }
        }
    }
}