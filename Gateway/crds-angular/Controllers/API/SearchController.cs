using System;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Results;
using crds_angular.Exceptions.Models;
using crds_angular.Security;
using crds_angular.Services.Interfaces;

namespace crds_angular.Controllers.API
{
    public class SearchController : MPAuth
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet]        
        [Route("api/search")]
        public IHttpActionResult GetSearchResults(
            [FromUri(Name = "q")]string query = null, 
            [FromUri(Name = "fq")]string filterQuery = null,
            [FromUri(Name = "q.parser")]string queryParser = null,
            [FromUri(Name = "q.options")]string queryOptions = null)
        {
            try
            {
                return Ok(_searchService.GetSearchResults(query, filterQuery, queryParser, queryOptions));
            }
            catch (Exception ex)
            {
                var apiError = new ApiErrorDto("Get Search Results Failed", ex);
                throw new HttpResponseException(apiError.HttpResponseMessage);
            }
        }
    }
}