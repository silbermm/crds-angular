using Amazon.CloudSearchDomain;
using Amazon.CloudSearchDomain.Model;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using Newtonsoft.Json.Linq;

namespace crds_angular.Services
{
    public class SearchService : ISearchService
    {
        private readonly AmazonCloudSearchDomainClient _client;

        public SearchService(IConfigurationWrapper configurationWrapper)
        {
            var endpoint = configurationWrapper.GetEnvironmentVarAsString("AMAZON_SEARCH_ENDPOINT");
            var apiKey = configurationWrapper.GetEnvironmentVarAsString("AMAZON_API_KEY");
            var apiSecret = configurationWrapper.GetEnvironmentVarAsString("AMAZON_API_SECRET");

            AmazonCloudSearchDomainConfig config = new AmazonCloudSearchDomainConfig();

            config.ServiceURL = endpoint;
            _client = new AmazonCloudSearchDomainClient(apiKey, apiSecret, config);
        }

        public SearchResponse GetSearchResults(SearchRequest searchRequest)
        {            
            var searchResponse = _client.Search(searchRequest);

            return searchResponse;
        }
    }
}