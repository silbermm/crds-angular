using System;
using crds_angular.Models.Crossroads;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services.Interfaces;
using IPersonService = crds_angular.Services.Interfaces.IPersonService;

namespace crds_angular.Services
{
    public class EmailCommunication : IEmailCommunication
    {
        private readonly ICommunicationService _communicationService;
        private readonly IPersonService _personService;

        public EmailCommunication(ICommunicationService communicationService, IPersonService personService)
        {
            _communicationService = communicationService;
            _personService = personService;
        }

        public void SendEmail(EmailCommunicationDTO email, string token)
        {
            var communication = new Communication();
            communication.DomainId = 1;

            if (token == null && email.FromUserId == null)
            {
                throw (new InvalidOperationException("Must provide either email.FromUserId or an authentication token."));
            }

            communication.AuthorUserId = email.FromUserId ?? _communicationService.GetUserIdFromContactId(token, email.FromContactId);

            var sender = _personService.GetPerson(email.FromContactId);
            communication.FromContactId = sender.ContactId;
            communication.FromEmailAddress = sender.EmailAddress;
            communication.ReplyContactId = sender.ContactId;
            communication.ReplyToEmailAddress = sender.EmailAddress;

            var receiver = _personService.GetPerson(email.ToContactId);
            communication.ToContactId = receiver.ContactId;
            communication.ToEmailAddress = receiver.EmailAddress;

            var template = _communicationService.GetTemplate(email.TemplateId);
            communication.TemplateId = email.TemplateId;
            communication.EmailBody = template.Body;
            communication.EmailSubject = template.Subject;

            communication.MergeData = email.MergeData;

            _communicationService.SendMessage(communication);
        }
    }
}