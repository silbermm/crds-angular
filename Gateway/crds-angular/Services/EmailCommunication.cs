using System;
using System.Collections.Generic;
using crds_angular.Models.Crossroads;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services.Interfaces;
using IPersonService = crds_angular.Services.Interfaces.IPersonService;

namespace crds_angular.Services
{
    public class EmailCommunication : IEmailCommunication
    {
        private readonly ICommunicationService _communicationService;
        private readonly IPersonService _personService;
        private readonly IContactService _contactService;
        private readonly IConfigurationWrapper _configurationWrapper;
        private readonly int DefaultContactEmailId;

        public EmailCommunication(ICommunicationService communicationService, 
            IPersonService personService, 
            IContactService contactService,
            IConfigurationWrapper configurationWrapper)
        {
            _communicationService = communicationService;
            _personService = personService;
            _contactService = contactService;
            _configurationWrapper = configurationWrapper;
            DefaultContactEmailId = _configurationWrapper.GetConfigIntValue("DefaultContactEmailId");
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

            var sender = _personService.GetPerson(DefaultContactEmailId);
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
            var sender = _personService.GetPerson(DefaultContactEmailId);
            var from = new Contact {ContactId = DefaultContactEmailId, EmailAddress = sender.EmailAddress};
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