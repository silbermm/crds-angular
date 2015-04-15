using System;
using System.Collections.Generic;
using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IEventService
    {
        int registerParticipantForEvent(int participantId, int eventId);
        List<Event> GetEvents(string eventType, string token);
        List<Event> GetEventsByTypeForRange(int eventTypeId, DateTime startDate, DateTime endDate, string token);
    }
}
