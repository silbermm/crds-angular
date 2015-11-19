using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using crds_angular.Models.Crossroads.Events;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Translation.Services.Interfaces;
using Event = MinistryPlatform.Models.Event;
using IEventService = crds_angular.Services.Interfaces.IEventService;
using TranslationEventService = MinistryPlatform.Translation.Services.Interfaces.IEventService;

namespace crds_angular.Services
{
    public class EventService : MinistryPlatformBaseService, IEventService
    {

        private readonly TranslationEventService _eventService;

        public EventService(TranslationEventService eventService)
        {
            this._eventService = eventService;
        }

        public Event GetEvent(int eventId)
        {
            return _eventService.GetEvent(eventId);
        }

        public void RegisterForEvent(List<EventRsvpDTO> eventDto)
        {
            try
            {
                eventDto.ForEach(dto => _eventService.RegisterParticipantForEvent(dto.ParticipantId, dto.EventId, dto.GroupId));
            }
            catch (Exception e)
            {
                throw new ApplicationException("Unable to add event participant: " + e.Message);
            }
        }
    }
}