using System.Collections.Generic;
using crds_angular.Models.Crossroads.Events;
using MinistryPlatform.Translation.Models.People;
using Event = MinistryPlatform.Models.Event;

namespace crds_angular.Services.Interfaces
{
    public interface IEventService
    {
        Event GetEvent(int eventId);
        void RegisterForEvent(List<EventRsvpDTO> eventDto, string token);
        IList<Models.Crossroads.Events.Event> EventsReadyForReminder(string token);
        IList<Participant> EventPaticpants(int eventId, string token);
        void SendReminderEmails();
        List<Participant> MyChildrenParticipants(int contactId, IList<Participant> children, string token);
        Event GetMyChildcareEvent(int parentEventId, string token);
        Event GetChildcareEvent(int parentEventId);
    }
}