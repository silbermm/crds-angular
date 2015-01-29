using System;
using System.Globalization;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Security;
using MinistryPlatform.Translation.Services;

namespace crds_angular.Controllers.API
{
    public class OpportunityController : CookieAuth
    {
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
    }
}