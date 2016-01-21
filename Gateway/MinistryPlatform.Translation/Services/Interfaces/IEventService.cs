using System;
using System.Collections.Generic;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Models.EventReservations;
using Participant = MinistryPlatform.Translation.Models.People.Participant;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IEventService
    {
        int CreateEvent(EventReservationDto eventReservationReservation, string token);
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
        IEnumerable<Event> EventsByPageViewId(string token, int pageViewId, string search);
        IEnumerable<Participant> EventParticipants(string token, int eventId);
        void SetReminderFlag(int eventId, string token);
    }
}