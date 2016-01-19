using System.Collections.Generic;
using crds_angular.Models.Crossroads.Events;
using MinistryPlatform.Translation.Models.People;
using Event = MinistryPlatform.Models.Event;

namespace crds_angular.Services.Interfaces
{
    public interface IEventService
    {
        bool CreateEventReservation(EventToolDto eventTool);
        EventToolDto GetEventReservation(int eventId);
        Event GetEvent(int eventId);
        void RegisterForEvent(EventRsvpDto eventDto, string token);
        IList<Models.Crossroads.Events.Event> EventsReadyForPrimaryContactReminder(string token);
        IList<Models.Crossroads.Events.Event> EventsReadyForReminder(string token);
        IList<Participant> EventParticpants(int eventId, string token);
        void SendReminderEmails();
        void SendPrimaryContactReminderEmails();
        List<Participant> MyChildrenParticipants(int contactId, IList<Participant> children, string token);
        Event GetMyChildcareEvent(int parentEventId, string token);
        Event GetChildcareEvent(int parentEventId);
        bool UpdateEventReservation(EventToolDto eventReservation, int eventId);
    }
}