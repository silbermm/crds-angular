using System.Collections.Generic;
using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IUserService
    {
        MinistryPlatformUser GetByUserId(string userId);
        MinistryPlatformUser GetByAuthenticationToken(string authToken);
        void UpdateUser(Dictionary<string, object> userUpdateValues);
        int GetUserIdByEmail(string email);
        int GetContactIdByUserId(int userId);
    }
}
