using System;
using System.Collections.Generic;
using MinistryPlatform.Models;
using Participant = MinistryPlatform.Translation.Models.People.Participant;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IEventService
    {
        int SafeRegisterParticipant(int participantId, int eventId, int groupId = 0, int groupParticipantId = 0);
        int RegisterParticipantForEvent(int participantId, int eventId, int groupId = 0, int groupParticipantId = 0);
        int UnregisterParticipantForEvent(int participantId, int eventId);
        List<Event> GetEvents(string eventType, string token);
        List<Event> GetEventsByTypeForRange(int eventTypeId, DateTime startDate, DateTime endDate, string token);
        List<Group> GetGroupsForEvent(int eventId);
        bool EventHasParticipant(int eventId, int participantId);
        Event GetEvent(int eventId);
        List<Event> GetEventsByParentEventId(int parentEventId);
        IEnumerable<Event> EventsByPageId(string token, int pageViewId);
        IEnumerable<Participant> EventParticipants(string token, int eventId);
    }
}