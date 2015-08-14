using System.Messaging;
using System.Web.Http;
using crds_angular.Models.Crossroads.Stewardship;
using crds_angular.Security;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using Crossroads.Utilities.Messaging.Interfaces;
using log4net;

namespace crds_angular.Controllers.API
{
    public class CheckScannerController : MPAuth
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(CheckScannerController));

        private readonly ICheckScannerService _checkScannerService;

        private readonly MessageQueue _donationsQueue;
        private readonly IMessageFactory _messageFactory;
        private readonly bool _asynchronous;

        public CheckScannerController(IConfigurationWrapper configuration, ICheckScannerService checkScannerService, IMessageQueueFactory messageQueueFactory = null, IMessageFactory messageFactory = null)
        {
            _checkScannerService = checkScannerService;

            var b = configuration.GetConfigValue("CheckScannerDonationsAsynchronousProcessingMode");
            _asynchronous = b != null && bool.Parse(b);
            if (_asynchronous)
            {
                var donationsQueueName = configuration.GetConfigValue("CheckScannerDonationsQueueName");
                // ReSharper disable once PossibleNullReferenceException
                _donationsQueue = messageQueueFactory.CreateQueue(donationsQueueName, QueueAccessMode.Send);
                _messageFactory = messageFactory;
            }
        }

        [Route("api/checkscanner/batches")]
        public IHttpActionResult GetOpenBatches()
        {
            // TODO Uncomment this to make the endpoint require authentication
            //return (Authorized(token =>
            //{
                var batches = _checkScannerService.GetOpenBatches();
                return (Ok(batches));
            //}));
        }

        [Route("api/checkscanner/batches/{batchName}/checks")]
        public IHttpActionResult GetChecksForBatch(string batchName)
        {
            // TODO Uncomment this to make the endpoint require authentication
            //return (Authorized(token =>
            //{
                var checks = _checkScannerService.GetChecksForBatch(batchName);
                return (Ok(checks));
            //}));
        }

        [Route("api/checkscanner/batches"), HttpPost]
        public IHttpActionResult CreateDonationsForBatch([FromBody] CheckScannerBatch batch)
        {
            // TODO Uncomment this to make the endpoint require authentication
            //return (Authorized(token =>
            //{
                if (!_asynchronous) return (Ok(_checkScannerService.CreateDonationsForBatch(batch)));

                var message = _messageFactory.CreateMessage(batch);
                _donationsQueue.Send(message);
                return (Ok(batch));
            //}));
        }
    }
}
