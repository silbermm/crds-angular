using Amazon.CloudSearchDomain.Model;
using Newtonsoft.Json.Linq;

namespace crds_angular.Services.Interfaces
{
    public interface ISearchService
    {
        JArray GetSearchResults(SearchRequest searchRequest);
    }
}