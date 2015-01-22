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
    public class DeletePageRecordService
    {
        public static int DeleteRecord(int pageId, int recordId, PlatformService.DeleteOption[] deleteOptions, String token)
        {
            try
            {
                var platformServiceClient = new PlatformService.PlatformServiceClient();
                using (new System.ServiceModel.OperationContextScope((System.ServiceModel.IClientChannel)platformServiceClient.InnerChannel))
                {
                    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Authorization", "Bearer " + token);
                    platformServiceClient.DeletePageRecord(pageId, recordId, deleteOptions);
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
