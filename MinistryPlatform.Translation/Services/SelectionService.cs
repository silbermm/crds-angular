using System.Collections.Generic;
using System.Linq;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class SelectionService : BaseService, ISelectionService
    {
        private readonly IMinistryPlatformService _ministryPlatformService;

        public SelectionService(IAuthenticationService authenticationService, IConfigurationWrapper configurationWrapper, IMinistryPlatformService ministryPlatformService) : base(authenticationService, configurationWrapper)
        {
            _ministryPlatformService = ministryPlatformService;
        }

        public IList<int> GetSelectionRecordIds(string authToken, int selectionId)
        {
            var result = _ministryPlatformService.GetSelectionsDict(selectionId, authToken);
            if (result == null || !result.Any())
            {
                return (null);
            }

            var selectedIds = result.Where(next => next.ContainsKey("dp_RecordID")).Select(next => next["dp_RecordID"] as int? ?? -1).ToList();
            return (selectedIds);
        }
    }
}
