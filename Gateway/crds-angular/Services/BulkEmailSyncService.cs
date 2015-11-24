using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Models;
using Newtonsoft.Json;
using MPInterfaces = MinistryPlatform.Translation.Services.Interfaces;
using RestSharp;

namespace crds_angular.Services
{
    public class BulkEmailSyncService : IBulkEmailSyncService
    {
        private readonly MPInterfaces.IBulkEmailRepository _bulkEmailRepository;
        private readonly MPInterfaces.IApiUserService _apiUserService;
        private readonly string _token;

        public BulkEmailSyncService(
            MPInterfaces.IBulkEmailRepository bulkEmailRepository,
            MPInterfaces.IApiUserService apiUserService)
        {
            _bulkEmailRepository = bulkEmailRepository;
            _apiUserService = apiUserService;
            _token = _apiUserService.GetToken();
        }


        public void RunService()
        {
            var token = _token;

            var publications = _bulkEmailRepository.GetPublications(token);

            // Get Publications 
            // For Each Publication

            foreach (var publication in publications)
            {
                //   Get correpsonding Page Views
                var pageViewIds = _bulkEmailRepository.GetPageViewIds(token, publication.PublicationId);

                //     Get Page view records
                var subscribers = _bulkEmailRepository.GetSubscribers(token, publication.PublicationId, pageViewIds);

                //     If (ContactEmail != SubscriptionEmail)
                //       Update/Delete MailChimp record
                //     End if   

                SendBatch(publication, subscribers);

                // TODO: Query MailChimp to see if batch was successfull

                // TODO: Update MP with last sync date
            }
        }

        public void SendBatch(BulkEmailPublication publication, List<BulkEmailSubscriber> subscribers)
        {
            // needs to be a configvalue, not hardcoded url
            var client = new RestClient("https://us12.api.mailchimp.com/3.0/");

            // TODO: Since this password was in public GitHub it needs to be invalidated and regenerated
            // needs to be a configvalue, not hardcoded url
            var password = "65ec517435aa07e010261c5a6692c7c7-us12";

            client.Authenticator = new HttpBasicAuthenticator("noname", password);

            var request = new RestRequest("batches", Method.POST);
            request.AddHeader("Content-Type", "application/json");

            var operation = AddSubscribers(publication, subscribers);

            request.RequestFormat = DataFormat.Json;
            request.AddBody(operation);

            var response = client.Execute(request);

            // TODO: Add code to Validate response
        }

        private SubscriberOperation AddSubscribers(BulkEmailPublication publication, List<BulkEmailSubscriber> subscribers)
        {
            SubscriberOperation operation = new SubscriberOperation();
            operation.operations = new List<Subscriber>();

            foreach (var subscriber in subscribers)
            {
                // TODO: Update MP with 3rd Party Contact ID
                if (subscriber.EmailAddress != subscriber.ThirdPartyContactId)
                {//Assuming success here, update the ID to match the emailaddress
                    //TODO: US2782 - need to also recongnize this is an email change and handle this acordingly
                    subscriber.ThirdPartyContactId = subscriber.EmailAddress;
                    _bulkEmailRepository.SetBaseSubscriber(_token, subscriber);
                }

                var mailChimpSubscriber = new Subscriber();
                mailChimpSubscriber.method = "POST";

                //TODO: Determine how to populate the ThirdPartyPublicationID? For now you may just write SQL to update table directly
                mailChimpSubscriber.path = string.Format("lists/{0}/members", publication.ThirdPartyPublicationId);

                // TODO: Do we need to store this somewhere to verify batch processed successfully
                mailChimpSubscriber.operation_id = Guid.NewGuid().ToString();

                string mergeFields = JsonConvert.SerializeObject(subscriber.MergeFields);
                // TODO: Use JSON.NET to serialize this rather than a string
                mailChimpSubscriber.body = String.Format("{{ \"status\":\"{0}\", \"email_address\":{1}, \"merge_fields\":{2} }}",
                    // TODO: Determine if these are the right values
                    subscriber.Subscribed ? "subscribed" : "unsubscribed", 
                    subscriber.EmailAddress, 
                    mergeFields);

                // add to list here
                operation.operations.Add(mailChimpSubscriber);
            }
            
            return operation;
        }
    }

    public class SubscriberOperation
    {
        public List<Subscriber> operations { get; set; }
    }

    public class Subscriber
    {
        public string method { get; set; }
        public string path { get; set; }
        public string operation_id { get; set; }
        public string body { get; set; }
    }
}