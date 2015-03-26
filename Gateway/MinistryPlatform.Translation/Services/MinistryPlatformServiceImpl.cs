using MinistryPlatform.Translation.Helpers;
using MinistryPlatform.Translation.PlatformService;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class MinistryPlatformServiceImpl : IMinistryPlatformService
    {
        private PlatformServiceClient platformServiceClient;

        public MinistryPlatformServiceImpl(PlatformServiceClient platformServiceClient)
        {
            this.platformServiceClient = platformServiceClient;
        }

        public List<Dictionary<string, object>> GetLookupRecords(int pageId, String token)
        {
            SelectQueryResult result = Call<SelectQueryResult>(token,
                platformClient => platformClient.GetPageLookupRecords(pageId, string.Empty, string.Empty, 0));
            return MPFormatConversion.MPFormatToList(result);
        }

        public Dictionary<string, object> GetLookupRecord(int pageId, string search, String token, int maxNumberOfRecordsToReturn = 100)
        {
            SelectQueryResult result = Call<SelectQueryResult>(token,
                platformClient => platformClient.GetPageLookupRecords(pageId, search, null, maxNumberOfRecordsToReturn));
            return MPFormatConversion.MPFormatToDictionary(result);
        }

        public SelectQueryResult GetRecords(int pageId, String token, String search = "", String sort = "")
        {
            return Call<SelectQueryResult>(token,
                platformClient => platformClient.GetPageRecords(pageId, search, sort, 0));
        }

        public JArray GetRecordsArr(int pageId, String token, String search = "", String sort = "")
        {
            return MPFormatConversion.MPFormatToJson(GetRecords(pageId, token, search, sort));
        }

        public List<Dictionary<string, object>> GetRecordsDict(int pageId, String token, String search = "", String sort = "")
        {
            return MPFormatConversion.MPFormatToList(GetRecords(pageId, token, search, sort));
        }

        public SelectQueryResult GetRecord(int pageId, int recordId, String token, bool quickadd = false)
        {
            return Call<SelectQueryResult>(token,
                platformClient => platformClient.GetPageRecord(pageId, recordId, quickadd));
        }

        public JArray GetRecordArr(int pageId, int recordId, String token, bool quickadd = false)
        {
            return MPFormatConversion.MPFormatToJson(GetRecord(pageId, recordId, token, quickadd));
        }

        public Dictionary<string, object> GetRecordDict(int pageId, int recordId, String token, bool quickadd = false)
        {
            return MPFormatConversion.MPFormatToDictionary(GetRecord(pageId, recordId, token, quickadd));
        }

        public List<Dictionary<string, object>> GetSubPageRecords(int subPageId, int recordId, String token)
        {
            SelectQueryResult result = Call<SelectQueryResult>(token,
                platformClient => platformClient.GetSubpageRecords(subPageId, recordId, string.Empty, string.Empty, 0));
            return MPFormatConversion.MPFormatToList(result);
        }

        public List<Dictionary<string, object>> GetSubpageViewRecords(int viewId, int parentRecordId,
           string token, string searchString = "", string sort = "", int top = 0)
        {
            var result = Call<SelectQueryResult>(token,
                platformClient => platformClient.GetSubpageViewRecords(viewId, parentRecordId, searchString, sort, top));
            return MPFormatConversion.MPFormatToList(result);
        }

        public List<Dictionary<string, object>> GetPageViewRecords(int viewId, string token, string searchString = "", string sort = "",
            int top = 0)
        {
            var result = Call<SelectQueryResult>(token,
                platformClient => platformClient.GetPageViewRecords(viewId, searchString, sort, top));
            return MPFormatConversion.MPFormatToList(result);
        }


        public int CreateRecord(int pageId, Dictionary<string, object> dictionary, String token,
            bool quickadd = false)
        {
            return Call<int>(token,
                platformClient => platformClient.CreatePageRecord(pageId, dictionary, quickadd));
        }

        public int CreateSubRecord(int subPageId, int parentRecordId, Dictionary<string, object> dictionary,
            String token, bool quickadd = false)
        {
            return Call<int>(token,
                platformClient => platformClient.CreateSubpageRecord(subPageId, parentRecordId, dictionary, quickadd));
        }

        public int DeleteRecord(int pageId, int recordId, DeleteOption[] deleteOptions, String token)
        {
            VoidCall(token,
                platfromClient => platfromClient.DeletePageRecord(pageId, recordId, deleteOptions));
            return recordId;
        }

        public void UpdateRecord(int pageId, Dictionary<string, object> dictionary, String token)
        {
            VoidCall(token, platfromClient => platfromClient.UpdatePageRecord(pageId, dictionary, false));
        }

        

        private T Call<T>(String token, Func<PlatformServiceClient, T> ministryPlatformFunc)
        {
            T result;
            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)platformServiceClient.InnerChannel))
            {
                if (System.ServiceModel.Web.WebOperationContext.Current != null)
                    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Authorization", "Bearer " + token);
                result = ministryPlatformFunc(platformServiceClient);
            }
            return result;
        }

        private void VoidCall(String token, Action<PlatformServiceClient> ministryPlatformFunc)
        {
            using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)platformServiceClient.InnerChannel))
            {
                if (System.ServiceModel.Web.WebOperationContext.Current != null)
                    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Authorization", "Bearer " + token);
                ministryPlatformFunc(platformServiceClient);
            }
        }
    }
}
