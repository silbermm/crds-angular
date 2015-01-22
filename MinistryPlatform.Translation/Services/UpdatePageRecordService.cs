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
    public class UpdatePageRecordService
    {
        public static void UpdateRecord(int pageId, Dictionary<string, object> dictionary, String token)
        {
            try
            {
                var platformServiceClient = new PlatformService.PlatformServiceClient();

                using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)platformServiceClient.InnerChannel))
                {
                    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Authorization", "Bearer " + token);
                    platformServiceClient.UpdatePageRecord(pageId, dictionary, false);                    
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}