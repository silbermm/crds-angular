using System.Collections.Generic;
using crds_angular.Services.Interfaces;
using MPInterfaces = MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.Services
{
    public class SubscriptionsService : MinistryPlatformBaseService, ISubscriptionsService
    {
        private readonly MPInterfaces.IMinistryPlatformService _ministryPlatformService;
        public SubscriptionsService(MPInterfaces.IMinistryPlatformService ministryPlatformService)
        {
            _ministryPlatformService = ministryPlatformService;
        }
        public List<Dictionary<string, object>> GetSubscriptions(int contactId, string token)
        {
            var publications = _ministryPlatformService.GetRecordsDict("Publications", token, ",,,,True", "8 asc");
            var subscriptions = _ministryPlatformService.GetSubPageRecords("SubscriptionsSubPage", contactId, token);
            foreach (var publication in publications)
            {
                object unSub = true;
                foreach (var subscription in subscriptions)
                {
                    object spID;
                    object pID;
                    subscription.TryGetValue("Publication_ID", out spID);
                    publication.TryGetValue("ID", out pID);
                    if ((int)pID == (int)spID)
                    {
                        subscription.TryGetValue("Unsubscribed", out unSub);
                        break;
                    }
                }
                publication.Add("Subscribed", !(bool)unSub);
            }
            return (publications);
        }
    }
}