using System;
using System.Collections.Generic;
using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IEventService
    {
        int registerParticipantForEvent(int participantId, int eventId);
        int unRegisterParticipantForEvent(int participantId, int eventId);
        List<Event> GetEvents(string eventType, string token);
        List<Event> GetEventsByTypeForRange(int eventTypeId, DateTime startDate, DateTime endDate, string token);
        List<Group> GetGroupsForEvent(int eventId);
        bool EventHasParticipant(int eventId, int participantId);
        Event GetEvent(int eventId);
        List<Event> GetEventsByParentEventId(int parentEventId);
    }
}
