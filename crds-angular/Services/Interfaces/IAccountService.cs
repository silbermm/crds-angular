using System.Collections.Generic;
using crds_angular.Models.Crossroads;

namespace crds_angular.Services.Interfaces
{
    public interface IAccountService
    {
        bool ChangePassword(string token, string newPassword);
        bool SaveCommunicationPrefs(string token, AccountInfo accountInfo);
        AccountInfo getAccountInfo(string token);
        Dictionary<string, string> RegisterPerson(User newUserData);
    }
}