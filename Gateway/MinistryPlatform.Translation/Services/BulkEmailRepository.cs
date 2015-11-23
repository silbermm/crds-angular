using System;
using System.Collections.Generic;
using System.Linq;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class BulkEmailRepository : BaseService, IBulkEmailRepository
    {
        private readonly IMinistryPlatformService _ministryPlatformService;
        private readonly int _bulkEmailPublicationPageViewId = Convert.ToInt32(AppSettings("BulkEmailPublicationsPageView"));


        public BulkEmailRepository(IAuthenticationService authenticationService, IConfigurationWrapper configurationWrapper, IMinistryPlatformService ministryPlatformService) :
            base(authenticationService, configurationWrapper)
        {
            _ministryPlatformService = ministryPlatformService;
        }

        public List<BulkEmailPublication> GetPublications(string token)
        {
            var records = _ministryPlatformService.GetPageViewRecords(_bulkEmailPublicationPageViewId, token);            

            var publications = records.Select(record => new BulkEmailPublication
            {
                PublicationId = record.ToInt("Publication_ID"),
                Title = record.ToString("Title"),
                Description = record.ToString("Description"),
                ThirdPartyPublicationId = record.ToString("Third_Party_Publication_ID"),
                LastSuccessfulSync = record.ToDate("Last_Successful_Sync"),
            }).ToList();

            return publications;
        }
    }
}
