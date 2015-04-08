using System;
using System.Globalization;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Security;
using crds_angular.Services;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.Controllers.API
{
    public class OpportunityController : MPAuth
    {

        private IOpportunityService _opportunityService;

        public OpportunityController(IOpportunityService opportunityService)
        {
            this._opportunityService = opportunityService;
        }

        [ResponseType(typeof (int))]
        [Route("api/opportunity/{id}")]
        public IHttpActionResult Post(int id, [FromBody] string stuff)
        {            
            var comments = string.Format("Request on {0}", DateTime.Now.ToString(CultureInfo.CurrentCulture));

            return Authorized(token =>
            {
                try
                {
                    var opportunityId = OpportunityService.RespondToOpportunity(token, id, comments);
                    return this.Ok(opportunityId);
                }
                catch (Exception ex)
                {
                    return this.InternalServerError(ex);
                }

            });
        }

        [ResponseType(typeof (DateTime))]
        [Route("api/opportunity/getLastOpportunityDate/{id}")]
        public IHttpActionResult GetLastOpportunityDate(int opportunityId)
        {
            return Authorized(token => this.Ok(_opportunityService.GetLastOpportunityDate(opportunityId, token)));
        }
    }
}