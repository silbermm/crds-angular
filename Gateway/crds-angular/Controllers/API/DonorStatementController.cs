using System;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Exceptions.Models;
using crds_angular.Models.Crossroads;
using crds_angular.Security;
using crds_angular.Services.Interfaces;
using MPInterfaces = MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.Controllers.API
{
    public class DonorStatementController : MPAuth
    {
        private readonly IDonorStatementService _donorStatementService;

        public DonorStatementController(IDonorStatementService donorStatementService)
        {
            _donorStatementService = donorStatementService;
        }


        [ResponseType(typeof(DonorStatementDTO))]
        [Route("api/donor-statement")]
        [HttpGet]
        public IHttpActionResult Get()
        {
            return (Authorized(token =>
            {
                try
                {
                    var donorStatement = _donorStatementService.GetDonorStatement(token);
                    return Ok(donorStatement);
                }                                
                catch (Exception ex)
                {
                    var apiError = new ApiErrorDto("Get Donor Statement", ex);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            }));
        }

        [Route("api/donor-statement")]
        [HttpPost]
        public IHttpActionResult Post(DonorStatementDTO donorStatement)
        {
            return (Authorized(token =>
            {
                try
                {
                    _donorStatementService.SaveDonorStatement(token, donorStatement);
                    return this.Ok();
                }
                catch (Exception ex)
                {
                    var apiError = new ApiErrorDto("Save Donor Statement", ex);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            }));
        }  
    }
}

