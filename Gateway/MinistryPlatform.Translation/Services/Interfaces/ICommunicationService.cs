﻿using System;
using System.Collections.Generic;
using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface ICommunicationService
    {
        CommunicationPreferences GetPreferences(String token, int userId);
        bool SetEmailSMSPreferences(String token, Dictionary<string, object> prefs);
        bool SetMailPreferences(string token, Dictionary<string, object> prefs);
        void SendMessage(Communication communication, Dictionary<string, object> mergeData);
        MessageTemplate GetTemplate(int templateId);    
        string ParseTemplateBody(string templateBody, Dictionary<string, object> record);
    }
}