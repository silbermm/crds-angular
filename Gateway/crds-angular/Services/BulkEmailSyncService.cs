using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using crds_angular.Services.Interfaces;
using MPInterfaces = MinistryPlatform.Translation.Services.Interfaces;
using RestSharp;

namespace crds_angular.Services
{
    public class BulkEmailSyncService : IBulkEmailSyncService
    {
        private readonly MPInterfaces.IBulkEmailRepository _bulkEmailRepository;
        private readonly MPInterfaces.IApiUserService _apiUserService;

        public BulkEmailSyncService(
            MPInterfaces.IBulkEmailRepository bulkEmailRepository,  
            MPInterfaces.IApiUserService apiUserService)
        {
            _bulkEmailRepository = bulkEmailRepository;
            _apiUserService = apiUserService;
        }


        public void RunService()
        {
            var token = _apiUserService.GetToken();

            var publications = _bulkEmailRepository.GetPublications(token);

            // Get Publications 
            // For Each Publication
            
            foreach (var publication in publications)
            {
                Console.WriteLine("{0} - {1}", publication.Title, publication.Description);

                //   Get correpsonding Page Views
                var pageViewIds = _bulkEmailRepository.GetPageViewIds(token, publication.PublicationId);

                //     Get Page view records
                var subscribers = _bulkEmailRepository.GetSubscribers(token, publication.PublicationId, pageViewIds);

                //   For each Page View
                foreach (var pageViewId in pageViewIds)
                {
                    

                    //     If (ContactEmail != SubscriptionEmail)
                    //       Update/Delete MailChimp record
                    //     End if
                    //
                    //     Translate fot MailChimp Format
                    //     Add to batch

                    Console.WriteLine("\t{0}", pageViewId);
                }

                //   End For
                //
                //   ?? Do we need to merge PageView pages?
                //
                //   Save Batch
            }
        }

        public void RunService_Sample()
        {
            // needs to be a configvalue, not hardcoded url
            var client = new RestSharp.RestClient("https://us12.api.mailchimp.com/3.0/");

            // needs to be a configvalue, not hardcoded url
            var password = "65ec517435aa07e010261c5a6692c7c7-us12";

            client.Authenticator = new HttpBasicAuthenticator("noname", password); 

            var request = new RestRequest("batches", Method.POST);
            request.AddHeader("Content-Type", "application/json");

            SubscriberOperation operation = new SubscriberOperation();
            operation.operations = new List<Subscriber>();

            // for loop starts here...

            // subscribers info needs to be pulled from the DB -- merge fields should be serializeable
            Subscriber s1 = new Subscriber();
            s1.method = "POST";

            // TODO: Use Third Party Publication ID here
            var thirdPartyPublicationId = "67db650475";

            s1.path = string.Format("lists/{0}/members", thirdPartyPublicationId);
            // TODO: Do we need to store this somewhere to verify batch processed successfully
            s1.operation_id = Guid.NewGuid().ToString();

            string emailaddy = "\"me@me.com\"";
            string mergeFields = "";
            s1.body = String.Format("{{ \"status\":\"subscribed\", \"email_address\":{0}, \"merge_fields\":{1} }}", emailaddy, mergeFields);

            // add to list here
            operation.operations.Add(s1);

            // for loop ends here...

            request.RequestFormat = DataFormat.Json;
            request.AddBody(operation);

            var response = client.Execute(request);

            //Console.WriteLine(response.Content);
            //Console.ReadLine();

            // handle logging down here
        }
    }

    class SubscriberOperation
    {
        public List<Subscriber> operations { get; set; }
    }

    class Subscriber
    {
        public string method { get; set; }
        public string path { get; set; }
        public string operation_id { get; set; }
        public string body { get; set; }
    }
}