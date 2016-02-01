﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using crds_angular.Models.Crossroads;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using log4net;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services.Interfaces;
using Newtonsoft.Json;

//using WebMatrix.WebData;

namespace crds_angular.Services
{
    public class LoginService : ILoginService
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(DonationService));

        private readonly IConfigurationWrapper _configurationWrapper;
        private readonly IContactService _contactService;
        private readonly IEmailCommunication _emailCommunication;
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;

        public LoginService(IAuthenticationService authenticationService, IConfigurationWrapper configurationWrapper, IContactService contactService, IEmailCommunication emailCommunication, IUserService userService)
        {
            _configurationWrapper = configurationWrapper;
            _contactService = contactService;
            _emailCommunication = emailCommunication;
            _userService = userService;
            _authenticationService = authenticationService;
        }

        public bool PasswordResetRequest(string username)
        {
            int user_ID = 0;
            int contact_Id = 0;

            // validate the email on the server side to avoid erroneous or malicious requests
            try
            {
                user_ID = _userService.GetUserIdByUsername(username);
                contact_Id = _userService.GetContactIdByUserId(user_ID);
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("Could not find email {0} for password reset", JsonConvert.SerializeObject(username, Formatting.Indented)), ex);
                return false;
            }

            // create a token -- see http://stackoverflow.com/questions/664673/how-to-implement-password-resets
            var resetArray = Encoding.UTF8.GetBytes(Guid.NewGuid() + username + System.DateTime.Now);
            RNGCryptoServiceProvider prov = new RNGCryptoServiceProvider();
            prov.GetBytes(resetArray);
            var resetToken = Encoding.UTF8.GetString(resetArray);
            string cleanToken = Regex.Replace(resetToken, "[^A-Za-z0-9]", "");

            Dictionary<string, object> userUpdateValues = new Dictionary<string, object>();
            userUpdateValues["User_ID"] = user_ID;
            userUpdateValues["PasswordResetToken"] = cleanToken; // swap out for real implementation
            _userService.UpdateUser(userUpdateValues);

            string baseURL = _configurationWrapper.GetConfigValue("BaseURL");
            string resetLink = (@"https://" + baseURL + "/reset-password?token=" + cleanToken);

            // add the email here...
            var emailCommunication = new EmailCommunicationDTO
            {
                FromContactId = 7, // church admin contact id
                FromUserId = 5, // church admin user id
                ToContactId = contact_Id,
                TemplateId = 13356,
                MergeData = new Dictionary<string, object>
                    {
                        { "resetlink", resetLink }
                    }
            };

            try
            {
                _emailCommunication.SendEmail(emailCommunication);
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("Could not send email {0} for password reset", JsonConvert.SerializeObject(username, Formatting.Indented)), ex);
                return false;
            }
        }

        public bool ResetPassword(string password, string token)
        {
            var user = _userService.GetUserByResetToken(token);

            Dictionary<string, object> userUpdateValues = new Dictionary<string, object>();
            userUpdateValues["User_ID"] = user.UserRecordId;
            userUpdateValues["PasswordResetToken"] = null;
            userUpdateValues["Password"] = password;
            _userService.UpdateUser(userUpdateValues);

            return true;
        }

        public bool ClearResetToken(string username)
        {
            int user_ID = _userService.GetUserIdByUsername(username);

            Dictionary<string, object> userUpdateValues = new Dictionary<string, object>();
            userUpdateValues["User_ID"] = user_ID;
            userUpdateValues["ResetToken"] = null; // swap out for real implementation
            _userService.UpdateUser(userUpdateValues);

            return true;
        }

        public bool VerifyResetToken(string token)
        {
            var user = _userService.GetUserByResetToken(token);

            if (user != null)
            {
                return true;
            }

            return false;
        }
    }
}