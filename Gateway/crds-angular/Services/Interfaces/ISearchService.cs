using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;

namespace crds_angular.Services.Interfaces
{
    public interface ISearchService
    {
        JArray GetSearchResults(string searchCriteria);
    }
}