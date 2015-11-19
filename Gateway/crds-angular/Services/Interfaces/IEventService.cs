using System;
using crds_angular.Models.Crossroads.Events;
using Event = MinistryPlatform.Models.Event;
using System.Collections.Generic;

namespace crds_angular.Services.Interfaces
{
    public interface IEventService
    {
        Event GetEvent(int eventId);
        void RegisterForEvent(List<EventRsvpDTO> eventDto);
    }
}