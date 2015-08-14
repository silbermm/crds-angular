using System.Web.Http;
using crds_angular.Security;
using crds_angular.Services.Interfaces;

namespace crds_angular.Controllers.API
{
    public class CheckScannerController : MPAuth
    {
        private readonly ICheckScannerService _checkScannerService;

        public CheckScannerController(ICheckScannerService checkScannerService)
        {
            _checkScannerService = checkScannerService;
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
    }
}
