using System;
using crds_angular.Models.Crossroads.Events;
using Event = MinistryPlatform.Models.Event;
using MinistryPlatform.Translation.Models.People;
using System.Collections.Generic;
using System.Web.Razor.Tokenizer;

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
    }
}