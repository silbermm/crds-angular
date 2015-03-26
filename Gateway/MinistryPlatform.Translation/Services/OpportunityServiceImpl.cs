using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class OpportunityServiceImpl : IOpportunityService
    {
        public  List<Opportunity> GetOpportunitiesForGroup(int groupId, string token)
        {
            var subPageViewId = Convert.ToInt32(ConfigurationManager.AppSettings["GroupOpportunitiesEvents"]);
            var subPageRecords = MinistryPlatformService.GetSubpageViewRecords(subPageViewId, groupId, token);
            var opportunities = new List<Opportunity>();

            foreach (var record in subPageRecords)
            {
                var opportunity = new Opportunity
                {
                    OpportunityId = (int) record["dp_RecordID"],
                    OpportunityName = (string) record["Opportunity Title"],
                    EventType = (string) record["Event Type"], 
                    RoleTitle = (string) record["Role_Title"]
                };
                var cap = 0;
                Int32.TryParse(record["Maximum_Needed"] != null ? record["Maximum_Needed"].ToString() : "0", out cap);
                opportunity.Capacity = cap;
                //now get all events with type = event type id
                if (opportunity.EventType != null)
                {
                    var events = OpportunityService.GetEvents(opportunity.EventType, token);
                    //is this a good place to sort the events by date/time??? tm 
                    var sortedEvents = events.OrderBy(o => o.EventStartDate).ToList();
                    opportunity.Events = sortedEvents;
                }

                opportunities.Add(opportunity);
            }
            return opportunities;
        }

        public  int GetOpportunitySignupCount(int opportunityId, int eventId, string token)
        {
            var subPageViewId = Convert.ToInt32(ConfigurationManager.AppSettings["SignedupToServe"]);
            var search = ",,," + eventId;
            var records = MinistryPlatformService.GetSubpageViewRecords(subPageViewId, opportunityId, token, search);

            return records.Count();
        }
    }
}