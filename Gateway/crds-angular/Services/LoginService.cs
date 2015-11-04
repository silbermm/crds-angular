using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using crds_angular.Models.Crossroads;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using Microsoft.AspNet.Identity;
using MySql.Web.Security;
using MinistryPlatform;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.Services.Interfaces;
using Newtonsoft.Json;

namespace crds_angular.Services
{
    public class LoginService
    {
        private readonly IConfigurationWrapper _configurationWrapper;
        private readonly IContactService _contactService;
        private readonly IEmailCommunication _emailCommunication;
        private readonly IUserService _userService;

        public LoginService(IConfigurationWrapper configurationWrapper, IContactService contactService, IEmailCommunication emailCommunication, IUserService userService)
        {
            _configurationWrapper = configurationWrapper;
            _contactService = contactService;
            _emailCommunication = emailCommunication;
            _userService = userService;
        }

        public bool SetPasswordResetToken(string userName)
        {
            // use api user here because there won't be a user token available
            var apiUser = this._configurationWrapper.GetEnvironmentVarAsString("API_USER");
            var apiPassword = this._configurationWrapper.GetEnvironmentVarAsString("API_PASSWORD");
            var authData = AuthenticationService.authenticate(apiUser, apiPassword);
            var token = authData["token"].ToString();

            string resetToken = "sampletoken";

            Dictionary<string, object> userUpdateValues = new Dictionary<string, object>();
            userUpdateValues["User_Email"] = "test@test.com";
            userUpdateValues["__PasswordResetToken"] = "thisisatestoken"; // swap out for real implementation
            _userService.UpdateUser(token, userUpdateValues);

            // add the email here...
            var email = new EmailCommunicationDTO
            {
                FromContactId = 7, // church admin contact id
                FromUserId = 5, // church admin user id
                ToContactId = _contactService.GetContactIdByEmail(userName, token),
                TemplateId = 13356,
                MergeData = new Dictionary<string, object>
                    {
                        { "resetlink", resetToken }
                    }
            };

            try
            {
                _emailCommunication.SendEmail(email);
            }
            catch (Exception ex)
            {
                //_logger.Error(string.Format("Could not send email {0} for password reset", JsonConvert.SerializeObject(email, Formatting.Indented)), ex);
            }

            return true;

        }
    }
}