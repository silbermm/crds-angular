using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using log4net;
using MinistryPlatform.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MPInterfaces = MinistryPlatform.Translation.Services.Interfaces;
using RestSharp;

namespace crds_angular.Services
{
    public class BulkEmailSyncService : IBulkEmailSyncService
    {
        private readonly ILog logger = LogManager.GetLogger(typeof(GroupService));

        private readonly MPInterfaces.IBulkEmailRepository _bulkEmailRepository;
        private readonly MPInterfaces.IApiUserService _apiUserService;
        private readonly string _token;

        private string _apiKey;

        public BulkEmailSyncService(
            MPInterfaces.IBulkEmailRepository bulkEmailRepository,
            MPInterfaces.IApiUserService apiUserService,
            IConfigurationWrapper configWrapper)
        {
            _bulkEmailRepository = bulkEmailRepository;
            _apiUserService = apiUserService;
            _token = _apiUserService.GetToken();
            _apiKey = configWrapper.GetEnvironmentVarAsString("BULK_EMAIL_API_KEY");
        }


        public void RunService()
        {
            var token = _token;

            var publications = _bulkEmailRepository.GetPublications(token);
            Dictionary<string,string> listResponseIds = new Dictionary<string, string>();

            // Get Publications 
            // For Each Publication
            foreach (var publication in publications)
            {
                //   Get correpsonding Page Views
                var pageViewIds = _bulkEmailRepository.GetPageViewIds(token, publication.PublicationId);

                //     Get Page view records
                var subscribers = _bulkEmailRepository.GetSubscribers(token, publication.PublicationId, pageViewIds);

                // TODO: Implement for US2782
                //     If (ContactEmail != SubscriptionEmail)
                //       Update/Delete MailChimp record
                //     End if   

                var operationId = SendBatch(publication, subscribers);

                // add the publication and operation id in prep for polling 
                if (!String.IsNullOrEmpty(operationId))
                {
                    listResponseIds.Add(publication.ThirdPartyPublicationId, operationId);
                }

                // TODO: Update MP with 3rd Party Contact ID

                // TODO: Update MP with last sync date
            }

            LogUpdateStatuses(listResponseIds);
        }

        public string SendBatch(BulkEmailPublication publication, List<BulkEmailSubscriber> subscribers)
        {
            // needs to be a configvalue, not hardcoded url
            var client = new RestClient("https://us12.api.mailchimp.com/3.0/");

            client.Authenticator = new HttpBasicAuthenticator("noname", _apiKey);

            var request = new RestRequest("batches", Method.POST);
            request.AddHeader("Content-Type", "application/json");

            var operation = AddSubscribers(publication, subscribers);

            request.RequestFormat = DataFormat.Json;
            request.AddBody(operation);

            var responseContent = client.Execute(request).Content;
            var responseValues = DeserializeToDictionary(responseContent);

            // this needs to be returned, because we can't guarantee that the operation won't fail after it begins
            if (responseValues["status"].ToString() == "started" || 
                responseValues["status"].ToString() == "pending" || 
                responseValues["status"].ToString() == "finished")
            {
                return responseValues["id"].ToString();
            }
            else
            {
                // TODO: Add logging code here for failure
                logger.Error(string.Format("Bulk email sync failed for publication {0} Response detail: {1}", publication.PublicationId, responseContent));

                return null;
            }
        }

        private void LogUpdateStatuses(Dictionary<string, string> publicationOperationIds)
        {
            // poll mailchimp to see if batch was successful -- also need to confirm during dev
            // testing if the response is synchronous or asynchronous -- 99% sure it's synchronous
            var client = new RestClient("https://us12.api.mailchimp.com/3.0/");
            client.Authenticator = new HttpBasicAuthenticator("noname", _apiKey);

            do
            {
                // pause to allow the operations to complete -- consider switching this to async
                Thread.Sleep(5000);

                foreach (var idPair in publicationOperationIds)
                {
                    var request = new RestRequest("batches/" + idPair.Value, Method.GET);
                    request.AddHeader("Content-Type", "application/json");

                    var response = client.Execute(request).Content;
                    var responseValues = DeserializeToDictionary(response);

                    // this needs to be returned, because we can't guarantee that the operation won't fail after it begins
                    if (responseValues["status"].ToString() == "finished")
                    {
                        logger.Info(response);
                        publicationOperationIds.Remove(idPair.Key);
                    }
                    else if (responseValues["status"].ToString() == "started" || responseValues["status"].ToString() == "pending")
                    {
                        continue; // try again in another five seconds
                    }
                    else
                    {
                        // TODO: Add logging code here for failure
                        logger.Error(string.Format("Bulk email sync failed for publication {0} Response detail: {1}", idPair.Key, response));
                        publicationOperationIds.Remove(idPair.Key);
                    }
                }

            } while (publicationOperationIds.Any());
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

        // From http://stackoverflow.com/questions/1207731/how-can-i-deserialize-json-to-a-simple-dictionarystring-string-in-asp-net
        private Dictionary<string, object> DeserializeToDictionary(string jo)
        {
            var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(jo);
            var values2 = new Dictionary<string, object>();
            foreach (KeyValuePair<string, object> d in values)
            {
                // if (d.Value.GetType().FullName.Contains("Newtonsoft.Json.Linq.JObject"))
                if (d.Value is JObject)
                {
                    values2.Add(d.Key, DeserializeToDictionary(d.Value.ToString()));
                }
                else
                {
                    values2.Add(d.Key, d.Value);
                }
            }
            return values2;
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
