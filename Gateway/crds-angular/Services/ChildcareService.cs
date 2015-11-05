using System;
using System.Collections.Generic;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.Services
{
    public class ChildcareService : IChildcareService
    {
        private readonly IEventParticipantService _eventParticipantService;
        private readonly ICommunicationService _communicationService;
        private readonly IConfigurationWrapper _configurationWrapper;
        private readonly IContactService _contactService;

        public ChildcareService(IEventParticipantService eventParticipantService,
                                ICommunicationService communicationService,
                                IConfigurationWrapper configurationWrapper,
                                IContactService contactService)
        {
            _eventParticipantService = eventParticipantService;
            _communicationService = communicationService;
            _configurationWrapper = configurationWrapper;
            _contactService = contactService;
        }

        public void SendRequestForRsvp()
        {
            var daysBeforeEvent = _configurationWrapper.GetConfigIntValue("NumberOfDaysBeforeEventToSend");
            var templateId = _configurationWrapper.GetConfigIntValue("ChildcareRequestTemplate");
            var authorUserId = _configurationWrapper.GetConfigIntValue("EmailAuthorId");

            var participants = _eventParticipantService.GetChildCareParticipants(daysBeforeEvent);
            var template = _communicationService.GetTemplate(templateId);

            var fromContact = _contactService.GetContactById(_configurationWrapper.GetConfigIntValue("UnassignedContact"));
            var replyToContact = fromContact;
            const int domainId = 1;

            foreach (var participant in participants)
            {
                var mergeData = SetMergeData(participant.GroupName, participant.EventStartDateTime, participant.EventId);

                var communication = new Communication
                {
                    AuthorUserId = authorUserId,
                    DomainId = domainId,
                    EmailBody = template.Body,
                    EmailSubject = template.Subject,
                    FromContactId = fromContact.Contact_ID,
                    FromEmailAddress = fromContact.Email_Address,
                    ReplyContactId = replyToContact.Contact_ID,
                    ReplyToEmailAddress = replyToContact.Email_Address,
                    ToContactId = participant.ContactId,
                    ToEmailAddress = participant.ParticipantEmail,
                    MergeData = mergeData
                };
                _communicationService.SendMessage(communication);
            }
        }

        private Dictionary<string, object> SetMergeData(string groupName, DateTime eventStartDateTime, int eventId)
        {
            var mergeData = new Dictionary<string, object>
            {
                {"GroupName", groupName},
                {"EventStartDate", eventStartDateTime.ToString("g")},
                {"EventId", eventId},
                {"BaseUrl", _configurationWrapper.GetConfigValue("BaseUrl")}
            };
            return mergeData;
        }
    }
}