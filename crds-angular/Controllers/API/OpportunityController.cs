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
        [Route("api/opportunity")]
        public IHttpActionResult Post([FromBody] string stuff)
        {
            logger.Debug("opportunity response Post");
            var comments = string.Format("Test {0}", DateTime.Now.ToString(CultureInfo.CurrentCulture));

            return Authorized(token =>
            {
                OpportunityService.RespondToOpportunity(token, 113, comments);
                return this.Ok();
            });
        }
    }
}