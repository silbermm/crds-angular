
namespace crds_angular.Services.Interfaces
{
    public interface ILoginService
    {
        bool PasswordResetRequest(string email);
        bool ResetPassword(string password, string token);
        bool ClearResetToken(string email);
        string VerifyResetToken(string token);
    }
}
