using System;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Results;
using Amazon.CloudSearchDomain.Model;
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
            [FromUri(Name = "q.options")]string queryOptions = null,
            [FromUri(Name = "cursor")]string cursor = null,
            [FromUri(Name = "expr")]string expr = null,
            [FromUri(Name = "facet")]string facet = null,
            [FromUri(Name = "highlight")]string highlight = null,
            [FromUri(Name = "partial")]bool partial = false,
            [FromUri(Name = "return")]string returnParameter = null,
            [FromUri(Name = "size")]long size = 10,
            [FromUri(Name = "sort")]string sort = null,
            [FromUri(Name = "start")]long start = 0
            )
        {
            try
            {
                SearchRequest request = new SearchRequest();
                request.Query = query;
                request.FilterQuery = filterQuery;
                request.QueryParser = queryParser;
                request.QueryOptions = queryOptions;
                request.Cursor = cursor;
                request.Expr = expr;
                request.Facet = facet;
                request.Highlight = highlight;
                request.Partial = partial;
                request.Return = returnParameter;
                request.Size = size;
                request.Sort = sort;
                request.Start = start;

                return Ok(_searchService.GetSearchResults(request));
            }
            catch (Exception ex)
            {
                var apiError = new ApiErrorDto("Get Search Results Failed", ex);
                throw new HttpResponseException(apiError.HttpResponseMessage);
            }
        }
    }
}