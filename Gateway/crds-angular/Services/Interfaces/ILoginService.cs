
namespace crds_angular.Services.Interfaces
{
    public interface ILoginService
    {
        bool PasswordResetRequest(string email);
        bool AcceptPasswordResetRequest(string email, string token, string password);

        bool ClearResetToken(string email);
        string VerifyResetToken(string token);
    }
}
