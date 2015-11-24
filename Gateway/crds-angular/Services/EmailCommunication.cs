using System;
using System.Collections.Generic;
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
            var from = new Contact { ContactId = sender.ContactId, EmailAddress = sender.EmailAddress };
            communication.FromContact = from;
            communication.ReplyToContact = from;

            var receiver = _personService.GetPerson(email.ToContactId);
            var recipient = new Contact {ContactId = receiver.ContactId, EmailAddress = receiver.EmailAddress};
            communication.ToContacts.Add(recipient);

            var template = _communicationService.GetTemplate(email.TemplateId);
            communication.TemplateId = email.TemplateId;
            communication.EmailBody = template.Body;
            communication.EmailSubject = template.Subject;

            communication.MergeData = email.MergeData;

            _communicationService.SendMessage(communication);
        }

        public void SendEmail(CommunicationDTO emailData)
        {
            var from = new Contact {ContactId = emailData.FromContactId, EmailAddress = _communicationService.GetEmailFromContactId(emailData.FromContactId)};
            var comm = new Communication
            {
                AuthorUserId = 1,
                DomainId = 1,
                EmailBody = emailData.Body,
                EmailSubject = emailData.Subject,
                FromContact = from,
                ReplyToContact = from,
                MergeData = new Dictionary<string, object>(),
                ToContacts = new List<Contact>()
            };
            foreach (var to in emailData.ToContactIds)
            {
                var contact = new Contact();
                contact.ContactId = to;
                contact.EmailAddress = _communicationService.GetEmailFromContactId(to);
                comm.ToContacts.Add(contact);
            }
            _communicationService.SendMessage(comm);
        }
    }
}