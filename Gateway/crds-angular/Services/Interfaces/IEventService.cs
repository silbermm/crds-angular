using MinistryPlatform.Models;

namespace crds_angular.Services.Interfaces
{
    public interface IEventService
    {
        Event GetEvent(int eventId);
    }
}