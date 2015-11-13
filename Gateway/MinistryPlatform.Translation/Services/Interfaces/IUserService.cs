using System.Collections.Generic;
using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IUserService
    {
        MinistryPlatformUser GetByUserId(string userId);
        MinistryPlatformUser GetByAuthenticationToken(string authToken);
        void UpdateUser(Dictionary<string, object> userUpdateValues);
        int GetUserIdByUsername(string username);
        int GetContactIdByUserId(int userId);
    }
}
