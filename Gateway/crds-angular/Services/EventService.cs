using crds_angular.Services.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services.Interfaces;
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

        public Event getEvent(int eventId)
        {
            return _eventService.GetEvent(eventId);
        }

        public Event GetChildcareEvent(int parentEventId)
        {
            throw new System.NotImplementedException();
        }
    }
}