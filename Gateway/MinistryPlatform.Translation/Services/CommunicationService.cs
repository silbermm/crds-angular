using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Threading;
using log4net;
using log4net.Repository.Hierarchy;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Exceptions;
using MinistryPlatform.Translation.Services.Interfaces;
using Newtonsoft.Json;

namespace MinistryPlatform.Translation.Services
{
    public class CommunicationService : BaseService, ICommunicationService

    {
        private readonly int MessagePageId = Convert.ToInt32(AppSettings("MessagesPageId"));
        private readonly int RecipientsSubPageId = Convert.ToInt32(AppSettings("RecipientsSubpageId"));
        private readonly int CommunicationStatusId = Convert.ToInt32(AppSettings("CommunicationStatusId"));
        private readonly int ActionStatusId = Convert.ToInt32(AppSettings("ActionStatusId"));
        private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public int GetUserIdFromContactId(string token, int contactId)
        {
            int pNum = Convert.ToInt32(ConfigurationManager.AppSettings["MyContact"]);
            var profile = MinistryPlatformService.GetRecordDict(pNum, contactId, token);

            return (int) profile["User_Account"];
        }

        public CommunicationPreferences GetPreferences(String token, int userId)
        {
            int pNum = Convert.ToInt32( ConfigurationManager.AppSettings["MyContact"]);
            int hNum = Convert.ToInt32(ConfigurationManager.AppSettings["MyHousehold"]);
            var profile = MinistryPlatformService.GetRecordDict(pNum, userId, token);
            var household = MinistryPlatformService.GetRecordDict(hNum, (int)profile["Household_ID"], token);
            return new CommunicationPreferences
            {
                Bulk_Email_Opt_Out = (bool)profile["Bulk_Email_Opt_Out"],
                Bulk_Mail_Opt_Out = (bool)household["Bulk_Mail_Opt_Out"],
                Bulk_SMS_Opt_Out = (bool)profile["Bulk_SMS_Opt_Out"]
            };
            //return MinistryPlatformService.GetRecordsDict(Convert.ToInt32(pageNumber), token);
        }

        public bool SetEmailSMSPreferences(String token, Dictionary<string,object> prefs){
            try
            {
                int pId = Convert.ToInt32(ConfigurationManager.AppSettings["MyContact"]);
                MinistryPlatformService.UpdateRecord(pId, prefs, token);
                return true;
            }
            catch (Exception e)
            {
                throw;
            }
            
        }

        public bool SetMailPreferences(string token, Dictionary<string,object> prefs){
            try
            {
                int pId = Convert.ToInt32(ConfigurationManager.AppSettings["MyHousehold"]);
                MinistryPlatformService.UpdateRecord(pId, prefs, token);
                return true;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// Creates the correct record in MP so that the mail service can pick it up and send 
        /// it during the scheduled run
        /// </summary>
        /// <param name="communication">The message properties </param>        
        public void SendMessage(Communication communication)
        {
            var token = apiLogin();

            var communicationId = AddCommunication(communication, token);
            AddCommunicationMessage(communication, communicationId, token);
        }

        private int AddCommunication(Communication communication, string token)
        {
            var dictionary = new Dictionary<string, object>
            {
                {"Subject", communication.EmailSubject},
                {"Body", communication.EmailBody},
                {"Author_User_Id", communication.AuthorUserId},
                {"Start_Date", DateTime.Now},
                {"From_Contact", communication.FromContactId},
                {"Reply_to_Contact", communication.ReplyContactId},
                {"Communication_Status_ID", CommunicationStatusId}
            };
            var communicationId = MinistryPlatformService.CreateRecord(MessagePageId, dictionary, token);
            return communicationId;
        }

        private void AddCommunicationMessage(Communication communication, int communicationId, string token)
        {
            var dictionary = new Dictionary<string, object>
            {
                {"Action_Status_ID", ActionStatusId},
                {"Action_Status_Time", DateTime.Now},
                {"Contact_ID", communication.ToContactId},
                {"From", communication.FromEmailAddress},
                {"To", communication.ToEmailAddress},
                {"Reply_To", communication.ReplyToEmailAddress},
                {"Subject", communication.EmailSubject},
                {"Body", ParseTemplateBody(communication.EmailBody, communication.MergeData)}
            };
            MinistryPlatformService.CreateSubRecord(RecipientsSubPageId, communicationId, dictionary, token);
        }

        public MessageTemplate GetTemplate(int templateId)
        {
            var pageRecords = MinistryPlatformService.GetRecordDict(MessagePageId, templateId, apiLogin());

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

        public string ParseTemplateBody(string templateBody, Dictionary<string, object> record)
        {
            try
            {
                return record.Aggregate(templateBody,
                    (current, field) => current.Replace("[" + field.Key + "]", field.Value.ToString()));
            }
            catch (Exception ex)
            {
                logger.Debug(string.Format("Failed to parse the template"));
                throw new TemplateParseException("Failed to parse the template", ex);
            }
        }
    }
}