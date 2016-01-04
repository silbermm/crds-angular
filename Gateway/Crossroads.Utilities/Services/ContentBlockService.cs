using System.Collections.Generic;
using System.IO;
using Crossroads.Utilities.Interfaces;
using Crossroads.Utilities.Models;
using log4net;
using RestSharp;

namespace Crossroads.Utilities.Services
{
    public class ContentBlockService : Dictionary<string, ContentBlock>, IContentBlockService
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(ContentBlockService));
        public ContentBlockService(IRestClient cmsRestClient)
        {
            var blocks = cmsRestClient.Execute<ContentBlocks>(new RestRequest("/api/ContentBlock", Method.GET));
            if (blocks.Data == null)
            {
                _logger.Fatal(string.Format("Unable to get the content blocks from the CMS! {0}", blocks.ErrorException.Message));
                return;
            }
            foreach (var b in blocks.Data.contentBlocks)
            {
                Add(b.Title, b);
            }            
        }
    }
}