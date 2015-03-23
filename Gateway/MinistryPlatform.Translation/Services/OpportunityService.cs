using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Services
{
    public class OpportunityService
    {
        
        public static int RespondToOpportunity(string token, int opportunityId, string comments)
        {
            var participant = AuthenticationService.GetParticipantRecord(token);
            var participantId = participant.ParticipantId;
            var pageId = Convert.ToInt32(ConfigurationManager.AppSettings["OpportunityResponses"]);

            var values = new Dictionary<string, object>
            {
                {"Response_Date", DateTime.Now},
                {"Opportunity_ID", opportunityId},
                {"Participant_ID", participantId},
                {"Closed", false},
                {"Comments", comments}
            };

            var recordId = MinistryPlatformService.CreateRecord(pageId, values, token, true);
            return recordId;
        }

        public static List<Opportunity> GetOpportunitiesForGroup(int groupId, string token)
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
                int.TryParse(record["Maximum_Needed"] != null ? record["Maximum_Needed"].ToString() : "0", out cap);
                opportunity.Capacity = cap;
                //now get all events with type = event type id
                if (opportunity.EventType != null)
                {
                    var events = GetEvents(opportunity.EventType, token);
                    //is this a good place to sort the events by date/time??? tm 
                    var sortedEvents = events.OrderBy(o => o.EventStartDate).ToList();
                    opportunity.Events = sortedEvents;
                }

                opportunities.Add(opportunity);
            }
            return opportunities;
        }

        //public for testing;a better way?
        //should some of this be moved to Event Service?  probably
        //suggestion: make event service to return events.  make this method search for specific type of event, ???
        public static List<Event> GetEvents(string eventType, string token)
        {
            //this is using the basic Events page, any concern there?
            var pageId = Convert.ToInt32(ConfigurationManager.AppSettings["Events"]);
            var search = ",," + eventType;
            var records = MinistryPlatformService.GetRecordsDict(pageId, token, search);

            return records.Select(record => new Event
            {
                EventTitle = (string) record["Event_Title"],
                EventType = (string) record["Event_Type"],
                EventStartDate = (DateTime) record["Event_Start_Date"],
                EventEndDate = (DateTime) record["Event_End_Date"], 
                EventId = (int)record["dp_RecordID"]
            }).ToList();
        }

        public static Response GetMyOpportunityResponses(int contactId, int opportunityId, string token)
        {
            var subPageViewId = Convert.ToInt32(ConfigurationManager.AppSettings["ContactOpportunityResponses"]);
            var subpageViewRecords = MinistryPlatformService.GetSubpageViewRecords(subPageViewId, opportunityId, token,
                ",,,," + contactId);
            var list = subpageViewRecords.ToList();
            var s = list.SingleOrDefault();
            if (s == null) return null;
            var response = new Response
            {
                Opportunity_ID = (int) s["Opportunity ID"],
                Participant_ID = (int) s["Participant ID"],
                Response_Date = (DateTime) s["Response Date"],
                Response_Result_ID = (int?) s["Response Result ID"]
            };
            return response;
        }

        public static int GetOpportunitySignupCount(int opportunityId, int eventId, string token)
        {
            var subPageViewId = Convert.ToInt32(ConfigurationManager.AppSettings["SignedupToServe"]);
            var search = ",,," + eventId;
            var records = MinistryPlatformService.GetSubpageViewRecords(subPageViewId, opportunityId, token, search);

            return records.Count();
        }
    }
}