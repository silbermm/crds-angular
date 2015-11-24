using System;
using System.Web.Http;
using crds_angular.Models.Crossroads;
using crds_angular.Security;
using crds_angular.Services.Interfaces;
using MPInterfaces = MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.Controllers.API
{
    public class EmailCommunicationController : MPAuth
    {
        private readonly IEmailCommunication _emailCommunication;

        public EmailCommunicationController(IEmailCommunication emailCommunication)
        {
            _emailCommunication = emailCommunication;
        }

        [Route("api/sendemail")]
        public IHttpActionResult Post(EmailCommunicationDTO email)
        {
            return Authorized(token =>
            {
                try
                {
                    _emailCommunication.SendEmail(email, token);

                    return Ok();
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            });
        }

        [Route("api/sendgroupemail/")]
        public IHttpActionResult Post([FromBody] CommunicationDTO communication)
        {
            return Authorized(token =>
            {
                try
                {
                    _emailCommunication.SendEmail(communication);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            });
        }
    }
}
