using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using crds_angular.Models.Crossroads;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using log4net;
using Microsoft.AspNet.Identity;
using MySql.Web.Security;
using MinistryPlatform;
using MinistryPlatform.Translation.Services;
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

        public LoginService(IConfigurationWrapper configurationWrapper, IContactService contactService, IEmailCommunication emailCommunication, IUserService userService)
        {
            _configurationWrapper = configurationWrapper;
            _contactService = contactService;
            _emailCommunication = emailCommunication;
            _userService = userService;
        }

        public bool PasswordResetRequest(string email)
        {
            // use service to get apiuser!!
            // use api user here because there won't be a user token available
            var apiUser = this._configurationWrapper.GetEnvironmentVarAsString("API_USER");
            var apiPassword = this._configurationWrapper.GetEnvironmentVarAsString("API_PASSWORD");
            var authData = AuthenticationService.authenticate(apiUser, apiPassword);
            var token = authData["token"].ToString();

            // this is really getting the user id -- we need a function to get the contact id by the user id
            int user_ID = _userService.GetUserIdByEmail(email);
            int contactId = _contactService.GetContactIdByEmail(email);

            // create a token -- see http://stackoverflow.com/questions/664673/how-to-implement-password-resets
            var resetArray = Encoding.UTF8.GetBytes(Guid.NewGuid() + email + System.DateTime.Now);
            RNGCryptoServiceProvider prov = new RNGCryptoServiceProvider();
            prov.GetBytes(resetArray);
            var resetToken = Encoding.UTF8.GetString(resetArray);
            string cleanToken = Regex.Replace(resetToken, "[^A-Za-z0-9 _]", "");

            Dictionary<string, object> userUpdateValues = new Dictionary<string, object>();
            userUpdateValues["User_ID"] = user_ID;
            userUpdateValues["ResetToken"] = cleanToken; // swap out for real implementation
            _userService.UpdateUser(token, userUpdateValues);

            // add the email here...
            var emailCommunication = new EmailCommunicationDTO
            {
                FromContactId = 7, // church admin contact id
                FromUserId = 5, // church admin user id
                ToContactId = contactId,
                TemplateId = 13356,
                MergeData = new Dictionary<string, object>
                    {
                        { "resetlink", cleanToken }
                    }
            };

            try
            {
                _emailCommunication.SendEmail(emailCommunication);
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("Could not send email {0} for password reset", JsonConvert.SerializeObject(email, Formatting.Indented)), ex);
            }

            return true;
        }

        public bool AcceptPasswordResetRequest(string email, string token, string password)
        {
            throw new Exception("Not Implemented Yet");
        }
    }
}