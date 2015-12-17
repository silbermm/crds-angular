using System.Collections.Generic;
using crds_angular.Models.Crossroads.Events;
using MinistryPlatform.Translation.Models.People;
using Event = MinistryPlatform.Models.Event;

namespace crds_angular.Services.Interfaces
{
    public interface IEventService
    {
        Event GetEvent(int eventId);
        void RegisterForEvent(EventRsvpDto eventDto, string token);
        IList<Models.Crossroads.Events.Event> EventsReadyForReminder(string token);
        IList<Participant> EventPaticpants(int eventId, string token);
        void SendReminderEmails();
    }
}