using System;
using System.Collections.Generic;
using MinistryPlatform.Translation.PlatformService;
using MinistryPlatform.Translation.Utils;
using MinistryPlatform.Translation.Helpers;
using Newtonsoft.Json.Linq;

namespace MinistryPlatform.Translation.Services
{
    public class MinistryPlatformService
    {
        public static SelectQueryResult GetRecords(int pageId, String token, String search = "", String sort = "")
        {
            return PlatformUtils.Call<SelectQueryResult>(token,
                platformClient => platformClient.GetPageRecords(pageId, search, sort, 0));
        }

        public static List<Dictionary<string, object>> GetRecordsDict(int pageId, String token, String search = "",
            String sort = "")
        {
            return MPFormatConversion.MPFormatToList(GetRecords(pageId, token, search, sort));
        }

        public static SelectQueryResult GetRecord(int pageId, int recordId, String token, bool quickadd = false)
        {
            return PlatformUtils.Call<SelectQueryResult>(token,
                platformClient => platformClient.GetPageRecord(pageId, recordId, quickadd));
        }

        public static Dictionary<string, object> GetRecordDict(int pageId, int recordId, String token,
            bool quickadd = false)
        {
            return MPFormatConversion.MPFormatToDictionary(GetRecord(pageId, recordId, token, quickadd));
        }

        public static List<Dictionary<string, object>> GetSubpageViewRecords(int viewId, int parentRecordId,
            string token, string searchString = "", string sort = "", int top = 0)
        {
            var result = PlatformUtils.Call<SelectQueryResult>(token,
                platformClient => platformClient.GetSubpageViewRecords(viewId, parentRecordId, searchString, sort, top));
            return MPFormatConversion.MPFormatToList(result);
        }

        public static int CreateRecord(int pageId, Dictionary<string, object> dictionary, String token,
            bool quickadd = false)
        {
            return PlatformUtils.Call<int>(token,
                platformClient => platformClient.CreatePageRecord(pageId, dictionary, quickadd));
        }

        public static int CreateSubRecord(int subPageId, int parentRecordId, Dictionary<string, object> dictionary,
            String token, bool quickadd = false)
        {
            return PlatformUtils.Call<int>(token,
                platformClient => platformClient.CreateSubpageRecord(subPageId, parentRecordId, dictionary, quickadd));
        }

        public static void UpdateRecord(int pageId, Dictionary<string, object> dictionary, String token)
        {
            PlatformUtils.VoidCall(token, platfromClient => platfromClient.UpdatePageRecord(pageId, dictionary, false));
        }
    }
}