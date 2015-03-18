﻿using System;
using System.Collections.Generic;
using MinistryPlatform.Translation.PlatformService;
using MinistryPlatform.Translation.Utils;
using MinistryPlatform.Translation.Helpers;
using Newtonsoft.Json.Linq;

namespace MinistryPlatform.Translation.Services
{
    public interface IMinistryPlatformService
    {
        List<Dictionary<string, object>> GetLookupRecords(int pageId, String token);
        
        Dictionary<string, object> GetLookupRecord(int pageId, string search, String token, int maxNumberOfRecordsToReturn = 100);
        
        SelectQueryResult GetRecords(int pageId, String token, String search = "", String sort = "");
        
        JArray GetRecordsArr(int pageId, String token, String search = "", String sort = "");
        
        List<Dictionary<string, object>> GetRecordsDict(int pageId, String token, String search = "", String sort = "");
        
        SelectQueryResult GetRecord(int pageId, int recordId, String token, bool quickadd = false);
        
        JArray GetRecordArr(int pageId, int recordId, String token, bool quickadd = false);
        
        Dictionary<string, object> GetRecordDict(int pageId, int recordId, String token, bool quickadd = false);
        
        List<Dictionary<string, object>> GetSubPageRecords(int subPageId, int recordId, String token);
        
        int CreateRecord(int pageId, Dictionary<string, object> dictionary, String token,
            bool quickadd = false);
        
        int CreateSubRecord(int subPageId, int parentRecordId, Dictionary<string, object> dictionary,
            String token, bool quickadd = false);
        
        int DeleteRecord(int pageId, int recordId, DeleteOption[] deleteOptions, String token);

        void UpdateRecord(int pageId, Dictionary<string, object> dictionary, String token);
    }
}