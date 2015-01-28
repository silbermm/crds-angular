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
        [ResponseType(typeof (bool))]
        [Route("api/opportunity/{id}")]
        public IHttpActionResult Post(int id, [FromBody] string stuff)
        {            
            var comments = string.Format("Request on {0}", DateTime.Now.ToString(CultureInfo.CurrentCulture));

            return Authorized(token =>
            {
                var opportunity = OpportunityService.RespondToOpportunity(token, id, comments);
                return opportunity ? (IHttpActionResult)this.Ok() : this.InternalServerError();

            });
        }
    }
}