using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using crds_angular.Models.Json;
using crds_angular.Security;
using MinistryPlatform.Translation.Services.Interfaces;
using Newtonsoft.Json;

namespace crds_angular.Controllers.API
{
    public class MinistryPlatformToolsController: MPAuth
    {
        private readonly ISelectionService _selectionService;

        public MinistryPlatformToolsController(ISelectionService selectionService)
        {
            _selectionService = selectionService;
        }

        [HttpGet]
        [Route("api/mptools/selection/{selectionId:regex(\\d+)}")]
        public IHttpActionResult GetPageSelectionRecordIds(int selectionId)
        {
            var response = Authorized(token =>
            {
                var selected = _selectionService.GetSelectionRecordIds(token, selectionId);
                if (selected == null || !selected.Any())
                {
                    return (RestHttpActionResult<SelectedRecords>.WithStatus(HttpStatusCode.NotFound, new SelectedRecords()));
                }
                var selectedRecords = new SelectedRecords
                {
                    RecordIds = selected
                };

                return (Ok(selectedRecords));
            });

            return (response);
        }
    }

    public class SelectedRecords
    {
        [JsonProperty("RecordIds", NullValueHandling = NullValueHandling.Ignore)]
        public IList<int> RecordIds { get; set; }
    }
}