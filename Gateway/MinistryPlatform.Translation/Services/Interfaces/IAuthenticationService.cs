using System;
using System.Collections.Generic;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Dictionary<string, object> Authenticate(string username, string password);

        Boolean ChangePassword(string token, string emailAddress, string firstName, string lastName, string password, string mobilephone);

        /// <summary>
        /// Change a users password
        /// </summary>
        /// <param name="token"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        Boolean ChangePassword(string token, string newPassword);

        //Get ID of currently logged in user
        int GetContactId(string token);
    }
}