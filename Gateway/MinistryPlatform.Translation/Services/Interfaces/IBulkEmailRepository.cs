using System.Collections.Generic;
using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IBulkEmailRepository
    {
        List<BulkEmailPublication> GetPublications(string token);
        void UpdateLastSyncDate(string token, BulkEmailPublication publication);
        List<int> GetPageViewIds(string token, int publicationId);
        List<BulkEmailSubscriber> GetSubscribers(string token, int publicationId, List<int> pageViewIds);
        void UpdateSubscriber(string token, BulkEmailSubscriber subscriber);
        void SetSubscriberSyncs(string token, List<BulkEmailSubscriberOpt> subscriberOpts);

    }
}
