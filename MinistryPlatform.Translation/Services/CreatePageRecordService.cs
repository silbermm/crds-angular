using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MinistryPlatform.Translation.Services
{

    //TODO Refactor boiler plate logic that seems to exist in each service
    public class CreatePageRecordService
    {
        public static int CreateRecord(int pageId, Dictionary<string, object> dictionary, String token) {
            int recordId = 0;

            try
            {
                var platformServiceClient = new PlatformService.PlatformServiceClient();
  

                using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)platformServiceClient.InnerChannel))
                {
                    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Authorization", "Bearer " + token);
                    recordId = platformServiceClient.CreatePageRecord(pageId, dictionary, false);
                }

            }
            catch (Exception e)
            {
                throw e;
            }

            return recordId;
        }

        public static int CreateSubRecord(int subPageId, int parentRecordId, Dictionary<string, object> dictionary, String token) {

            int recordId = 0;

            try
            {
                var platformServiceClient = new PlatformService.PlatformServiceClient();


                using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)platformServiceClient.InnerChannel))
                {
                    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Authorization", "Bearer " + token);
                    recordId = platformServiceClient.CreateSubpageRecord(subPageId, parentRecordId, dictionary, false);
                }

            }
            catch (Exception e)
            {
                throw e;
            }

            return recordId;
        }
    }
}
