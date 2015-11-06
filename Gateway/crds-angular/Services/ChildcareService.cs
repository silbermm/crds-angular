using System;
using System.Collections.Generic;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using log4net;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.Services
{
    public class ChildcareService : IChildcareService
    {
        private readonly ICommunicationService _communicationService;
        private readonly IConfigurationWrapper _configurationWrapper;
        private readonly IContactService _contactService;
        private readonly IEventParticipantService _eventParticipantService;
        private readonly IEventService _eventService;
        private readonly ILog _logger = LogManager.GetLogger(typeof (ChildcareService));

        public ChildcareService(IEventParticipantService eventParticipantService,
                                ICommunicationService communicationService,
                                IConfigurationWrapper configurationWrapper,
                                IContactService contactService,
            IEventService eventService)
        {
            _eventParticipantService = eventParticipantService;
            _communicationService = communicationService;
            _configurationWrapper = configurationWrapper;
            _contactService = contactService;
            _eventService = eventService;
        }

        public void SendRequestForRsvp()
        {
            var daysBeforeEvent = _configurationWrapper.GetConfigIntValue("NumberOfDaysBeforeEventToSend");
            var templateId = _configurationWrapper.GetConfigIntValue("ChildcareRequestTemplate");
            var authorUserId = _configurationWrapper.GetConfigIntValue("EmailAuthorId");
            var template = _communicationService.GetTemplate(templateId);
            var fromContact = _contactService.GetContactById(_configurationWrapper.GetConfigIntValue("UnassignedContact"));
            const int domainId = 1;

            var participants = _eventParticipantService.GetChildCareParticipants(daysBeforeEvent);
            foreach (var participant in participants)
            {
                var childEvent = _eventService.GetEventByParentEventId(participant.EventId);

                if (childEvent == null)
                {
                    //childcare event does not exist, don't send a message
                    continue;
                }
                if (childEvent.EventType != "Childcare")
                {
                    //childcare event does not exist, don't send a message
                    continue;
                }
                var mergeData = SetMergeData(participant.GroupName, participant.EventStartDateTime, participant.EventId);
                var replyToContact = ReplyToContact(childEvent);
                var communication = FormatCommunication(authorUserId, domainId, template, fromContact, replyToContact, participant, mergeData);
                try
                {
                    _communicationService.SendMessage(communication);
                }
                catch (Exception ex)
                {
                    LogError(participant, ex);
                }
            }
        }

        private static MyContact ReplyToContact(Event childEvent)
        {
            var contact = childEvent.PrimaryContact;
            var replyToContact = new MyContact();
            replyToContact.Contact_ID = contact.ContactId;
            replyToContact.Email_Address = contact.EmailAddress;
            return replyToContact;
        }

        private static Communication FormatCommunication(int authorUserId,
                                                         int domainId,
                                                         MessageTemplate template,
                                                         MyContact fromContact,
                                                         MyContact replyToContact,
                                                         EventParticipant participant,
                                                         Dictionary<string, object> mergeData)
        {
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
            return communication;
        }

        private void LogError(EventParticipant participant, Exception ex)
        {
            var participantId = participant.ParticipantId;
            var groupId = participant.GroupId;
            var eventId = participant.EventId;
            _logger.Error(string.Format("Send Childcare RSVP email failed. Participant: {0}, Group: {1}, Event: {2}", participantId, groupId, eventId), ex);
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