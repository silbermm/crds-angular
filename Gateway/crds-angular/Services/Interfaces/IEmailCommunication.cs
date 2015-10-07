using crds_angular.Models.Crossroads;

namespace crds_angular.Services.Interfaces
{
    public interface IEmailCommunication
    {
        void SendEmail(EmailCommunicationDTO email, string token = null);        
    }
}