using MinistryPlatform.Models;

namespace crds_angular.Services.Interfaces
{
    public interface IChildcareService

    {
        void SendRequestForRsvp();
        Event GetMyChildcareEvent(int parentEventId, string token);
    }
}