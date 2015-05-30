using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using MinistryPlatform.Models;
using Newtonsoft.Json;

namespace MinistryPlatform.Translation.Services
{
    public class CommunicationService : BaseService
    {
        private static readonly int MessagePageId = Convert.ToInt32(AppSettings("MessagesPageId"));
        private static readonly int RecipientsSubPageId = Convert.ToInt32(AppSettings("RecipientsSubpageId"));
        private static readonly int CommunicationStatusId = Convert.ToInt32(AppSettings("CommunicationStatusId"));
        private static readonly int ActionStatusId = Convert.ToInt32(AppSettings("ActionStatusId"));

        public static CommunicationPreferences GetPreferences(String token, int userId)
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

        public static bool SetEmailSMSPreferences(String token, Dictionary<string,object> prefs){
            try
            {
                int pId = Convert.ToInt32(ConfigurationManager.AppSettings["MyContact"]);
                MinistryPlatformService.UpdateRecord(pId, prefs, token);
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
            
        }

        public static bool SetMailPreferences(string token, Dictionary<string,object> prefs){
            try
            {
                int pId = Convert.ToInt32(ConfigurationManager.AppSettings["MyHousehold"]);
                MinistryPlatformService.UpdateRecord(pId, prefs, token);
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static void SendMessage(Communication communication, Dictionary<string, object> mergeData, string token)
        {
            var communicationId = AddCommunication(communication, token);
            AddCommunicationMessage(communication, communicationId, mergeData, token);
        }

        private static int AddCommunication(Communication communication, string token)
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

        private static void AddCommunicationMessage(Communication communication, int communicationId, Dictionary<string, object> mergeData, string token)
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
                {"Body", ParseTemplateBody(communication.EmailBody, mergeData)}
            };
            MinistryPlatformService.CreateSubRecord(RecipientsSubPageId, communicationId, dictionary, token);
        }

        public static MessageTemplate GetTemplate(int templateId, string token)
        {
            var pageRecords = MinistryPlatformService.GetRecordDict(MessagePageId, templateId, token);

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

        public static string ParseTemplateBody(string templateBody, Dictionary<string, object> record)
        {
            return record.Aggregate(templateBody, (current, field) => current.Replace("[" + field.Key + "]", field.Value.ToString()));
        }
    }
}