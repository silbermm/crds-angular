using System;
using System.Collections.Generic;

namespace MinistryPlatform.Translation.Services
{
    public class EventService : BaseService, IEventService
    {
        readonly private log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly int EventParticipantPageId = Convert.ToInt32(AppSettings("EventsParticipants"));
        private readonly int EventParticipantStatusDefaultID = Convert.ToInt32(AppSettings("Event_Participant_Status_Default_ID"));

        public int registerParticipantForEvent(int participantId, int eventId)
        {
            logger.Debug("Adding participant " + participantId + " to event " + eventId);
            var values = new Dictionary<string, object>
            {
                { "Participant_ID", participantId },
                { "Event_ID", eventId },
                { "Participation_Status_ID", EventParticipantStatusDefaultID },
            };

            int eventParticipantId = createEventParticipant(eventId, values);

            logger.Debug("Added participant " + participantId + " to event " + eventId + ": record id: " + eventParticipantId);
            return (eventParticipantId);
        }

        /**
         * This method exists solely to wrap the static call to the MinistryPlatformService, to enable unit testing
         * TODO Refactor the MinistryPlatformService to have dependencies injected, and get rid of static calls
         */
        protected virtual int createEventParticipant(int eventId, Dictionary<string, object> values)
        {
            int eventParticipantId = WithApiLogin<int>(apiToken =>
            {
                return (MinistryPlatformService.CreateSubRecord(EventParticipantPageId, eventId, values, apiToken));
            });
            return (eventParticipantId);
        }
    }
}
