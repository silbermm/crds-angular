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
        private readonly int _publicationPageViewSubPageId = Convert.ToInt32(AppSettings("PublicationPageViewSubPageId"));
        private readonly int _segmentationBasePageViewId = Convert.ToInt32(AppSettings("SegmentationBasePageViewId"));
        private readonly int _subscribersBasePageViewId = Convert.ToInt32(AppSettings("Subscribers"));

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

        public void UpdateLastSyncDate(string token, BulkEmailPublication publication)
        {
            var publicationDictionary = new Dictionary<string, object>
            {
                {"Publication_ID", publication.PublicationId},
                {"Last_Successful_Sync", publication.LastSuccessfulSync}
            };
            _ministryPlatformService.UpdateRecord(Convert.ToInt32(AppSettings("Publications")), publicationDictionary, token);
        }

        public List<int> GetPageViewIds(string token, int publicationId)
        {
            var records = _ministryPlatformService.GetSubPageRecords(_publicationPageViewSubPageId, publicationId, token);

            var publications = records.Select(record => record.ToInt("Page_View_ID")).ToList();

            return publications;
        }

        public List<BulkEmailSubscriber> GetSubscribers(string token, int publicationId, List<int> pageViewIds)
        {
            var publcationFilter = string.Format(",\"{0}\"", publicationId);
            var subscribers = GetBaseSubscribers(token, publcationFilter);

            foreach (var pageViewId in pageViewIds)
            {
                AddAdditionalFields(token, subscribers, publcationFilter, pageViewId);
            }

            return subscribers.Values.ToList();
        }

        public void UpdateSubscriber(string token, BulkEmailSubscriber subscriber)
        {
            var subscriberDictionary = new Dictionary<string, object>
            {
                {"Contact_Publication_ID", subscriber.ContactPublicationId},
                {"Unsubscribed", !subscriber.Subscribed},
                {"Third_Party_Contact_ID", subscriber.ThirdPartyContactId}

            };
            _ministryPlatformService.UpdateRecord(Convert.ToInt32(AppSettings("Subscribers")), subscriberDictionary, token);
        }

        private Dictionary<int, BulkEmailSubscriber> GetBaseSubscribers(string token, string publicationFilter)
        {
            var records = _ministryPlatformService.GetPageViewRecords(_segmentationBasePageViewId, token, publicationFilter);
            var subscribers = new Dictionary<int, BulkEmailSubscriber>();

            foreach (var record in records)
            {
                var subscriber = new BulkEmailSubscriber();
                subscriber.ContactPublicationId = record.ToInt("Contact_Publication_ID");
                subscriber.ContactId = record.ToInt("Contact_ID");
                subscriber.EmailAddress = record.ToString("Email_Address");
                subscriber.ThirdPartyContactId = record.ToString("Third_Party_Contact_ID");
                subscriber.Subscribed = !record.ToBool("Unsubscribed");

                AddMergeFields(subscriber, record);

                subscribers.Add(subscriber.ContactPublicationId, subscriber);
            }
            return subscribers;
        }

        private void AddAdditionalFields(string token, Dictionary<int, BulkEmailSubscriber> subscribers, string publicationFilter, int pageViewId)
        {            
            var records = _ministryPlatformService.GetPageViewRecords(pageViewId, token, publicationFilter);

            foreach (var record in records)
            {
                var contactPublicationId = record.ToInt("Contact_Publication_ID");

                if (!subscribers.ContainsKey(contactPublicationId))
                {
                    continue;
                }

                var subscriber = subscribers[contactPublicationId];

                AddMergeFields(subscriber, record);
            }
        }

        private void AddMergeFields(BulkEmailSubscriber subscriber, Dictionary<string, object> record)
        {
            var columnsToSkip = new List<string>()
            {
                "dp_RecordID", 
                "dp_RecordName",
                "dp_Selected",
                "dp_FileID",
                "dp_RecordStatus",
                "Contact_Publication_ID",
                "Publication_ID",
                "Title",
                "Third_Party_Publication_ID",
                "Last_Successful_Sync",
                "Unsubscribed",
                "Third_Party_Contact_ID",
                "Contact_ID",
                "Email_Address"
            };

                  
            foreach (var column in record)
            {
                if (columnsToSkip.Contains(column.Key))
                {
                    continue;
                }

                if (subscriber.MergeFields.Keys.Contains(column.Key))
                {
                    continue;
                }

                if (column.Value == null)
                {
                    continue;
                }
                
                subscriber.MergeFields.Add(column.Key, column.Value.ToString());
            }
        }

        public void SetSubscriberSyncs(string token, List<BulkEmailSubscriberOpt> subscriberOpts)
        {
            foreach (var subscriberOpt in subscriberOpts)
            {
                UpdateContactPublication(token, subscriberOpt);
            }
        }

        public void UpdateContactPublication(string token, BulkEmailSubscriberOpt subscriberOpt)
        {
            var searchString = string.Format(",\"{0}\",,,,,,,\"{1}\"", subscriberOpt.PublicationID, subscriberOpt.EmailAddress);
            var contactPublications = _ministryPlatformService.GetPageViewRecords(_segmentationBasePageViewId, token, searchString);

            // do not update if there is no corresponding subscriber -- this may be handled in a future story
            if (contactPublications.Count == 0)
            {
                return;
            }

            var contactPublication = contactPublications.SingleOrDefault();
            var contactPublicationID = contactPublication.ToString("Contact_Publication_ID");

            Dictionary<string, object> subscriberOptDict = new Dictionary<string, object>
            {
                {"Contact_Publication_ID", contactPublicationID},
                {"Unsubscribed", (subscriberOpt.Status == "subscribed" ? false : true)}
            };

            _ministryPlatformService.UpdateRecord(_subscribersBasePageViewId, subscriberOptDict, token);         
        }
    }
}
