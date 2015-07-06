using System;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Exceptions;
using crds_angular.Exceptions.Models;
using crds_angular.Models.Crossroads;
using crds_angular.Security;
using crds_angular.Services.Interfaces;
using MPInterfaces = MinistryPlatform.Translation.Services.Interfaces;
using MinistryPlatform.Models;
using System.Configuration;
using System.Collections.Generic;

namespace crds_angular.Controllers.API
{
    public class CommunicationController : MPAuth
    {
        private readonly MPInterfaces.ICommunicationService _mpCommunicationService;
        private readonly IPersonService _mpPersonService;


        public CommunicationController(MPInterfaces.ICommunicationService mpCommunicationService, IPersonService mpPersonService)
        {
            _mpCommunicationService = mpCommunicationService;
            _mpPersonService = mpPersonService;
        }

        [Route("api/sendemail")]
        public IHttpActionResult Post(Communication communication)
        {
            // TODO: Re-enable authorization when done with testing
            //return Authorized(token =>
            //{
                try
                {
                    communication.DomainId = 1;

                    // template id is set in client 
                    MessageTemplate template = _mpCommunicationService.GetTemplate(communication.TemplateId);
                    communication.EmailBody = template.Body;
                    communication.EmailSubject = template.Subject;

                    // populate the email fields for sender and receiver
                    var sender = _mpPersonService.GetPerson(communication.FromContactId);
                    communication.AuthorUserId = communication.FromContactId;
                    communication.FromEmailAddress = sender.EmailAddress;
                    communication.ReplyContactId = communication.FromContactId;
                    communication.ReplyToEmailAddress = sender.EmailAddress;

                    var receiver = _mpPersonService.GetPerson(communication.ToContactId);
                    communication.ToEmailAddress = receiver.EmailAddress;

                    _mpCommunicationService.SendMessage(communication);

                    return Ok();
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            //});
        }
    }
}
