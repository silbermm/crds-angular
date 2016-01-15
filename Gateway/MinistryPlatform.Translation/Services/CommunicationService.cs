using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Crossroads.Utilities.Interfaces;
using log4net;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Exceptions;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class CommunicationService : BaseService, ICommunicationService

    {
        private readonly int _messagePageId = Convert.ToInt32(AppSettings("MessagesPageId"));
        private readonly int _recipientsSubPageId = Convert.ToInt32(AppSettings("RecipientsSubpageId"));
        private readonly int _communicationStatusId = Convert.ToInt32(AppSettings("CommunicationStatusId"));
        private readonly int _actionStatusId = Convert.ToInt32(AppSettings("ActionStatusId"));
        private readonly int _contactPageId = Convert.ToInt32(AppSettings("Contacts"));

        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IMinistryPlatformService _ministryPlatformService;

        public CommunicationService(IMinistryPlatformService ministryPlatformService, IAuthenticationService authenticationService,IConfigurationWrapper configurationWrapper)
            : base(authenticationService, configurationWrapper)
        {
            _ministryPlatformService = ministryPlatformService;
        }
		
        public int GetUserIdFromContactId(string token, int contactId)
        {
            int pNum = Convert.ToInt32(ConfigurationManager.AppSettings["MyContact"]);
            var profile = MinistryPlatformService.GetRecordDict(pNum, contactId, token);

            return (int) profile["User_Account"];
        }

        public int GetUserIdFromContactId(int contactId)
        {
            var profile = MinistryPlatformService.GetRecordDict(_contactPageId, contactId, ApiLogin());

            return (int)profile["User_Account"];
        }

        public string GetEmailFromContactId(int contactId)
        {
            var contact = _ministryPlatformService.GetRecordDict(_contactPageId, contactId, ApiLogin());
            return contact["Email_Address"].ToString();
        }

        public CommunicationPreferences GetPreferences(String token, int userId)
        {
            int pNum = Convert.ToInt32( ConfigurationManager.AppSettings["MyContact"]);
            int hNum = Convert.ToInt32(ConfigurationManager.AppSettings["MyHousehold"]);
            var profile = _ministryPlatformService.GetRecordDict(pNum, userId, token);
            var household = _ministryPlatformService.GetRecordDict(hNum, (int)profile["Household_ID"], token);
            return new CommunicationPreferences
            {
                Bulk_Email_Opt_Out = (bool)profile["Bulk_Email_Opt_Out"],
                Bulk_Mail_Opt_Out = (bool)household["Bulk_Mail_Opt_Out"],
                Bulk_SMS_Opt_Out = (bool)profile["Bulk_SMS_Opt_Out"]
            };
        }

        public bool SetEmailSMSPreferences(String token, Dictionary<string,object> prefs){
            int pId = Convert.ToInt32(ConfigurationManager.AppSettings["MyContact"]);
            _ministryPlatformService.UpdateRecord(pId, prefs, token);
            return true;
        }

        public bool SetMailPreferences(string token, Dictionary<string,object> prefs){
            int pId = Convert.ToInt32(ConfigurationManager.AppSettings["MyHousehold"]);
            _ministryPlatformService.UpdateRecord(pId, prefs, token);
            return true;
        }

        /// <summary>
        /// Creates the correct record in MP so that the mail service can pick it up and send 
        /// it during the scheduled run
        /// </summary>
        /// <param name="communication">The message properties </param>        
        public void SendMessage(Communication communication)
        {
            var token = ApiLogin();

            var communicationId = AddCommunication(communication, token);
            AddCommunicationMessages(communication, communicationId, token);
        }

        private int AddCommunication(Communication communication, string token)
        {
            var dictionary = new Dictionary<string, object>
            {
                {"Subject", communication.EmailSubject},
                {"Body", communication.EmailBody},
                {"Author_User_Id", communication.AuthorUserId},
                {"Start_Date", DateTime.Now},
                {"From_Contact", communication.FromContact.ContactId},
                {"Reply_to_Contact", communication.ReplyToContact.ContactId},
                {"Communication_Status_ID", _communicationStatusId}
            };
            var communicationId = _ministryPlatformService.CreateRecord(_messagePageId, dictionary, token);
            return communicationId;
        }

        private void AddCommunicationMessages(Communication communication, int communicationId, string token)
        {
            foreach (Contact contact in communication.ToContacts)
            {
                var dictionary = new Dictionary<string, object>
                {
                    {"Action_Status_ID", _actionStatusId},
                    {"Action_Status_Time", DateTime.Now},
                    {"Contact_ID", contact.ContactId},
                    {"From", communication.FromContact.EmailAddress},
                    {"To", contact.EmailAddress},
                    {"Reply_To", communication.ReplyToContact.EmailAddress},
                    {"Subject", ParseTemplateBody(communication.EmailSubject, communication.MergeData)},
                    {"Body", ParseTemplateBody(communication.EmailBody, communication.MergeData)}
                };
                _ministryPlatformService.CreateSubRecord(_recipientsSubPageId, communicationId, dictionary, token);
            }
        }

        public MessageTemplate GetTemplate(int templateId)
        {
            var pageRecords = _ministryPlatformService.GetRecordDict(_messagePageId, templateId, ApiLogin());

            if (pageRecords == null)
            {
                throw new InvalidOperationException("Couldn't find message template.");
            }

            var template = new MessageTemplate
            {
                Body = pageRecords["Body"].ToString(),
                Subject = pageRecords["Subject"].ToString()
            };

            return template;
        }

        public Communication GetTemplateAsCommunication(int templateId, int fromContactId, string fromEmailAddress, int replyContactId, string replyEmailAddress, int toContactId, string toEmailAddress, Dictionary<string, object> mergeData = null)
        {
            var template = GetTemplate(templateId);
            return new Communication
            {
                AuthorUserId = _configurationWrapper.GetConfigIntValue("DefaultAuthorUser"),
                DomainId = 1,
                EmailBody = template.Body,
                EmailSubject = template.Subject,
                FromContact = new Contact {ContactId = fromContactId, EmailAddress = fromEmailAddress},
                ReplyToContact = new Contact {ContactId = replyContactId, EmailAddress = replyEmailAddress},
                ToContacts = new List<Contact>{ new Contact{ContactId = toContactId, EmailAddress = toEmailAddress}},
                TemplateId = templateId,
                MergeData = mergeData
            };
        }

        public string ParseTemplateBody(string templateBody, Dictionary<string, object> record)
        {
            try
            {
                if (record == null)
                {
                    return templateBody;
                }
                return record.Aggregate(templateBody,
                    (current, field) => current.Replace("[" + field.Key + "]", field.Value.ToString()));
            }
            catch (Exception ex)
            {
                _logger.Warn("Failed to parse the template", ex);
                throw new TemplateParseException("Failed to parse the template", ex);
            }
        }
    }
}