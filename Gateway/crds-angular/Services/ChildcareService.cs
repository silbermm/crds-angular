using System;
using System.Collections.Generic;
using System.Linq;
using crds_angular.Models.Crossroads.Childcare;
using crds_angular.Models.Crossroads.Serve;
using crds_angular.Services.Interfaces;
using crds_angular.Util.Interfaces;
using Crossroads.Utilities.Interfaces;
using log4net;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services.Interfaces;
using IEventService = MinistryPlatform.Translation.Services.Interfaces.IEventService;

namespace crds_angular.Services
{
    public class ChildcareService : IChildcareService
    {
        private readonly ICommunicationService _communicationService;
        private readonly IConfigurationWrapper _configurationWrapper;
        private readonly IContactService _contactService;
        private readonly IEventParticipantService _eventParticipantService;
        private readonly IEventService _eventService;
        private readonly IParticipantService _participantService;
        private readonly IServeService _serveService;
        private readonly IDateTime _dateTimeWrapper;

        private readonly ILog _logger = LogManager.GetLogger(typeof (ChildcareService));

        public ChildcareService(IEventParticipantService eventParticipantService,
                                ICommunicationService communicationService,
                                IConfigurationWrapper configurationWrapper,
                                IContactService contactService,
                                IEventService eventService,
                                IParticipantService participantService,
                                IServeService serveService,
                                IDateTime dateTimeWrapper)
        {
            _eventParticipantService = eventParticipantService;
            _communicationService = communicationService;
            _configurationWrapper = configurationWrapper;
            _contactService = contactService;
            _eventService = eventService;
            _participantService = participantService;
            _serveService = serveService;
            _dateTimeWrapper = dateTimeWrapper;
        }

        public List<FamilyMember> MyChildren(string token)
        {
            var family = _serveService.GetImmediateFamilyParticipants(token);
            var myChildren = new List<FamilyMember>();

            foreach (var member in family)
            {
                var schoolGrade = SchoolGrade(member.HighSchoolGraduationYear);
                var maxAgeWithoutGrade = _configurationWrapper.GetConfigIntValue("MaxAgeWithoutGrade");
                var maxGradeForChildcare = _configurationWrapper.GetConfigIntValue("MaxGradeForChildcare");
                if (schoolGrade == 0 && member.Age <= maxAgeWithoutGrade)
                {
                    myChildren.Add(member);
                }
                else if (schoolGrade > 0 && schoolGrade <= maxGradeForChildcare)
                {
                    myChildren.Add(member);
                }
            }
            return myChildren;
        }

        public void SaveRsvp(ChildcareRsvpDto saveRsvp)
        {
            var participantId = 0;

            try
            {
                foreach (var p in saveRsvp.Participants)
                {
                    participantId = p;
                    _eventService.RegisterParticipantIfNotRegistered(saveRsvp.EventId, participantId);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("Save RSVP failed for event ({0}), participant ({1})", saveRsvp.EventId, participantId), ex);
            }
        }

        public int SchoolGrade(int graduationYear)
        {
            if (graduationYear == 0)
            {
                return 0;
            }
            var today = _dateTimeWrapper.Today;
            var todayMonth = today.Month;
            var yearForCalc = today.Year;
            if (todayMonth > 7)
            {
                yearForCalc = today.Year + 1;
            }

            var grade = 12 - (graduationYear - yearForCalc);
            if (grade <= 12 && grade >= 0)
            {
                return grade;
            }
            return 0;
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
                var childEvent = GetChildcareEvent(participant.EventId);
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

        public Event GetMyChildcareEvent(int parentEventId, string token)
        {
            var participantRecord = _participantService.GetParticipantRecord(token);
            if (!_eventService.EventHasParticipant(parentEventId, participantRecord.ParticipantId))
            {
                return null;
            }
            // token user is part of parent event, retrieve childcare event
            var childcareEvent = GetChildcareEvent(parentEventId);
            return childcareEvent;
        }

        private Event GetChildcareEvent(int parentEventId)
        {
            var childEvents = _eventService.GetEventsByParentEventId(parentEventId);
            var childcareEvents = childEvents.Where(childEvent => childEvent.EventType == "Childcare").ToList();

            if (childcareEvents.Count == 0)
            {
                return null;
            }
            if (childcareEvents.Count > 1)
            {
                throw new ApplicationException(string.Format("Mulitple Childcare Events Exist, parent event id: {0}", parentEventId));
            }
            return childcareEvents.First();
        }

        private static MyContact ReplyToContact(Event childEvent)
        {
            var contact = childEvent.PrimaryContact;
            var replyToContact = new MyContact
            {
                Contact_ID = contact.ContactId,
                Email_Address = contact.EmailAddress
            };
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