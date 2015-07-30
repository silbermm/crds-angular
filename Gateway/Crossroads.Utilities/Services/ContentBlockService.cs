using System.Collections.Generic;
using Crossroads.Utilities.Interfaces;
using Crossroads.Utilities.Models;
using RestSharp;

namespace Crossroads.Utilities.Services
{
    public class ContentBlockService : Dictionary<string, ContentBlock>, IContentBlockService
    {
        public ContentBlockService(IRestClient cmsRestClient)
        {
            var blocks = cmsRestClient.Execute<List<ContentBlock>>(new RestRequest("/api/ContentBlock", Method.GET));

            foreach (var b in blocks.Data)
            {
                Add(b.Title, b);
            }
        }
    }
}