using System;
using System.Collections.Generic;
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
    }
}
