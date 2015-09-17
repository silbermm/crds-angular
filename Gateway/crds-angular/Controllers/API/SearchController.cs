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
        [Route("api/search/{searchCriteria}")]
        //[EnableCors("*", "*", "*")]
        public IHttpActionResult GetSearchResults(string searchCriteria)
        {
            try
            {
                return Ok(_searchService.GetSearchResults(searchCriteria));
            }
            catch (Exception ex)
            {
                var apiError = new ApiErrorDto("Get Search Results Failed", ex);
                throw new HttpResponseException(apiError.HttpResponseMessage);
            }
        }
    }
}