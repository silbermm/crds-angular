namespace crds_angular.Services.Interfaces
{
    public interface IChildcareService

    {
        void SendRequestForRsvp();
        object GetMyChildcareEvent(int parentEventId);
    }
}