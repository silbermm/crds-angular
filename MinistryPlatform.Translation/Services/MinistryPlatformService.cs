using System;
using System.Collections.Generic;
using MinistryPlatform.Translation.PlatformService;
using MinistryPlatform.Translation.Utils;

namespace MinistryPlatform.Translation.Services
{
    public class MinistryPlatformService
    {
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

        public static int DeleteRecord(int pageId, int recordId, DeleteOption[] deleteOptions, String token)
        {
            PlatformUtils.VoidCall(token,
                platfromClient => platfromClient.DeletePageRecord(pageId, recordId, deleteOptions));
            return recordId;
        }

        public static void UpdateRecord(int pageId, Dictionary<string, object> dictionary, String token)
        {
            PlatformUtils.VoidCall(token, platfromClient => platfromClient.UpdatePageRecord(pageId, dictionary, false));
        }
    }
}