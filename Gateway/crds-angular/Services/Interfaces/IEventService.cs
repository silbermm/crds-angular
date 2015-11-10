using MinistryPlatform.Models;

namespace crds_angular.Services.Interfaces
{
    public interface IEventService
    {
        Event getEvent(int eventId);
        Event GetChildcareEvent(int parentEventId);
    }
}