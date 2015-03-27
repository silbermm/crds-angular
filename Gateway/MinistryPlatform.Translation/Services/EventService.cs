﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class EventService : BaseService, IEventService
    {
        readonly private log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly int EventParticipantPageId = Convert.ToInt32(AppSettings("EventsParticipants"));
        private readonly int EventParticipantStatusDefaultID = Convert.ToInt32(AppSettings("Event_Participant_Status_Default_ID"));
        private IMinistryPlatformService ministryPlatformService;

        public EventService(IMinistryPlatformService ministryPlatformService)
        {
            this.ministryPlatformService = ministryPlatformService;
        }

        public int registerParticipantForEvent(int participantId, int eventId)
        {
            logger.Debug("Adding participant " + participantId + " to event " + eventId);
            var values = new Dictionary<string, object>
            {
                { "Participant_ID", participantId },
                { "Event_ID", eventId },
                { "Participation_Status_ID", EventParticipantStatusDefaultID },
            };

            int eventParticipantId = WithApiLogin<int>(apiToken =>
            {
                return (ministryPlatformService.CreateSubRecord(EventParticipantPageId, eventId, values, apiToken, true));
            });

            logger.Debug("Added participant " + participantId + " to event " + eventId + ": record id: " + eventParticipantId);
            return (eventParticipantId);
        }

        public List<Event> GetEvents(string eventType, string token)
        {
            //this is using the basic Events page, any concern there?
            var pageId = Convert.ToInt32(ConfigurationManager.AppSettings["Events"]);
            var search = ",," + eventType;
            var records = ministryPlatformService.GetRecordsDict(pageId, token, search);

            return records.Select(record => new Event
            {
                EventTitle = (string)record["Event_Title"],
                EventType = (string)record["Event_Type"],
                EventStartDate = (DateTime)record["Event_Start_Date"],
                EventEndDate = (DateTime)record["Event_End_Date"],
                EventId = (int)record["dp_RecordID"]
            }).ToList();
        }
    }
}
