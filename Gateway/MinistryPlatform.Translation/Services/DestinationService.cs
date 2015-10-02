using System.Collections.Generic;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class DestinationService : BaseService, IDestinationService
    {
        private readonly IMinistryPlatformService _ministryPlatformService;

        public DestinationService(IMinistryPlatformService ministryPlatformService, IAuthenticationService authenticationService, IConfigurationWrapper configurationWrapper)
            : base(authenticationService, configurationWrapper)
        {
            _ministryPlatformService = ministryPlatformService;
        }

        public List<TripDocuments> DocumentsForDestination(int destinationId)
        {
            var token = ApiLogin();
            var searchString = string.Format(",{0}", destinationId);
            var records = _ministryPlatformService.GetPageViewRecords("TripDestinationDocuments", token, searchString);

            var documents = new List<TripDocuments>();
            foreach (var record in records)
            {
                var d = new TripDocuments();
                d.Description = record.ToString("Description");
                d.Document = record.ToString("Document");
                d.DocumentId = record.ToInt("Document_ID");
                documents.Add(d);
            }

            return documents;
        }
    }
}