using System;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Exceptions.Models;
using crds_angular.Models;
using crds_angular.Models.Crossroads;
using crds_angular.Security;
using crds_angular.Services;
using crds_angular.Services.Interfaces;

namespace crds_angular.Controllers.API
{
    public class CongregationController : MPAuth
    {
        private readonly ICongregationService _congregationService;

        public CongregationController(ICongregationService congregationService)
        {
            _congregationService = congregationService;
        }

        [ResponseType(typeof(Congregation))]
        [Route("api/congregation/{id}")]
        public IHttpActionResult Get(int id)
        {
            return Authorized(t =>
            {
                try
                {
                    var congregation = _congregationService.GetCongregationById(id);

                    return Ok(congregation);
                }
                catch (Exception e)
                {
                    var msg = "Error getting Congregation by id " + id;
                    logger.Error(msg, e);
                    var apiError = new ApiErrorDto(msg, e);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }
    }
}