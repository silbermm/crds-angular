using System.Messaging;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Models.Crossroads.Stewardship;
using crds_angular.Security;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using Crossroads.Utilities.Messaging.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.Controllers.API
{
    public class CheckScannerController : MPAuth
    {
        private readonly bool _asynchronous;
        private readonly IAuthenticationService _authenticationService;
        private readonly ICheckScannerService _checkScannerService;
        private readonly ICommunicationService _communicationService;
        private readonly MessageQueue _donationsQueue;
        private readonly IMessageFactory _messageFactory;

        public CheckScannerController(IConfigurationWrapper configuration,
                                      ICheckScannerService checkScannerService,
                                      IAuthenticationService authenticationService,
                                      ICommunicationService communicationService,
                                      IMessageQueueFactory messageQueueFactory = null,
                                      IMessageFactory messageFactory = null)
        {
            _checkScannerService = checkScannerService;
            _authenticationService = authenticationService;
            _communicationService = communicationService;

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

        [RequiresAuthorization]
        [Route("api/checkscanner/batches")]
        public IHttpActionResult GetBatches([FromUri(Name = "onlyOpen")] bool onlyOpen = true)
        {
            return (Authorized(token =>
            {
                var batches = _checkScannerService.GetBatches(onlyOpen);
                return (Ok(batches));
            }));
        }

        [RequiresAuthorization]
        [Route("api/checkscanner/batches/{batchName}/checks")]
        public IHttpActionResult GetChecksForBatch(string batchName)
        {
            return (Authorized(token =>
            {
                var checks = _checkScannerService.GetChecksForBatch(batchName);
                return (Ok(checks));
            }));
        }

        [RequiresAuthorization]
        [Route("api/checkscanner/batches"), HttpPost]
        public IHttpActionResult CreateDonationsForBatch([FromBody] CheckScannerBatch batch)
        {
            return (Authorized(token =>
            {
                if (!_asynchronous)
                {
                    return (Ok(_checkScannerService.CreateDonationsForBatch(batch)));
                }

                batch.MinistryPlatformContactId = _authenticationService.GetContactId(token);
                batch.MinistryPlatformUserId = _communicationService.GetUserIdFromContactId(token, batch.MinistryPlatformContactId.Value);

                var message = _messageFactory.CreateMessage(batch);
                _donationsQueue.Send(message);
                return (Ok(batch));
            }));
        }
        /// <summary>
        /// Takes in the encrypted account and routing number which is then used to locate an existing donor.
        /// If an existing donor is found, then the address data is returned.
        /// If an existing donor is not found, then a 404 will be returned
        /// </summary>
        /// <param name="checkAccount">This is the encrypted account and routing number from EZ Scan.</param>
        /// <returns>The created or updated donor record.</returns>
        [RequiresAuthorization]
        [ResponseType(typeof(EZScanDonorDetails))]
        [Route("api/checkscanner/getdonor"), HttpPost]
        public IHttpActionResult GetDonorForCheck([FromBody] CheckAccount checkAccount)
        {
            return (Authorized(token =>
            {
                var donorDetail = _checkScannerService.GetContactDonorForCheck(checkAccount.AccountNumber, checkAccount.RoutingNumber);
                if (donorDetail == null)
                {
                    return NotFound();
                }
                return (Ok(donorDetail)); 
            }));
        }
        
        /// <summary>
        /// Creates a donor record in Ministry Platform based off of the check details passed in.
        /// </summary>
        /// <param name="checkDetails" type="FromBody">The Check Details from the check that was scanned.</param>
        /// <returns>The created donor record.</returns>
        [RequiresAuthorization]
        [ResponseType(typeof(ContactDonor))]
        [Route("api/checkscanner/donor"), HttpPost]
        public IHttpActionResult CreateDonor([FromBody] CheckScannerCheck checkDetails)
        {
            return (Authorized(token =>
            {
                var result = _checkScannerService.CreateDonor(checkDetails);
                return (Ok(result));

            }));
        }
    }
}
