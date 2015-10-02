using System;
using System.Collections.Generic;
using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface ICommunicationService
    {
        CommunicationPreferences GetPreferences(String token, int userId);
        bool SetEmailSMSPreferences(String token, Dictionary<string, object> prefs);
        bool SetMailPreferences(string token, Dictionary<string, object> prefs);
        void SendMessage(Communication communication);
        MessageTemplate GetTemplate(int templateId);    
        string ParseTemplateBody(string templateBody, Dictionary<string, object> record);
        int GetUserIdFromContactId(string token, int contactId);
        int GetUserIdFromContactId(int contactId);
        string GetEmailFromContactId(int contactId);
    }
}