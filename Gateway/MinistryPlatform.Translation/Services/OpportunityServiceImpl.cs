using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Extensions;
using MinistryPlatform.Translation.Services.Interfaces;

namespace MinistryPlatform.Translation.Services
{
    public class OpportunityServiceImpl : BaseService, IOpportunityService
    {
        private IMinistryPlatformService _ministryPlatformService;
        private IEventService _eventService;
        private IAuthenticationService _authenticationService;

        private readonly int _groupOpportunitiesEventsPageViewId =
            Convert.ToInt32(AppSettings("GroupOpportunitiesEvents"));

        private readonly int _signedupToServeSubPageViewId = Convert.ToInt32(AppSettings("SignedupToServe"));
        private readonly int _opportunityPage = Convert.ToInt32(AppSettings("OpportunityPage"));
        private readonly int _eventPage = Convert.ToInt32(AppSettings("Events"));

        public OpportunityServiceImpl(IMinistryPlatformService ministryPlatformService, IEventService eventService,
            IAuthenticationService authenticationService)
        {
            this._ministryPlatformService = ministryPlatformService;
            this._eventService = eventService;
            this._authenticationService = authenticationService;
        }

        public List<Opportunity> GetOpportunitiesForGroup(int groupId, string token)
        {
            var subPageRecords = _ministryPlatformService.GetSubpageViewRecords(_groupOpportunitiesEventsPageViewId,
                groupId, token);
            var opportunities = new List<Opportunity>();

            foreach (var record in subPageRecords)
            {
                var opportunity = new Opportunity
                {
                    OpportunityId = record.ToInt("dp_RecordID"),
                    OpportunityName = record.ToString("Opportunity Title"),
                    EventType = record.ToString("Event Type"),
                    EventTypeId = record.ToInt("Event Type ID"),
                    RoleTitle = record.ToString("Role_Title"),
                    MaximumNeeded = record.ToNullableInt("Maximum_Needed"),
                    MinimumNeeded = record.ToNullableInt("Minimum_Needed")
                };
                //now get all events with type = event type id
                if (opportunity.EventType != null)
                {
                    var events = _eventService.GetEvents(opportunity.EventType, token);
                    var sortedEvents = events.OrderBy(o => o.EventStartDate).ToList();
                    opportunity.Events = sortedEvents;
                }

                opportunities.Add(opportunity);
            }
            return opportunities;
        }

        public Response GetOpportunityResponse(int opportunityId, int eventId, Participant participant)
        {
            var searchString = string.Format(",{0},{1},{2}", opportunityId, eventId, participant.ParticipantId);
            List<Dictionary<string, object>> dictionaryList;
            try
            {
                dictionaryList =
                    WithApiLogin<List<Dictionary<string, object>>>(
                        apiToken =>
                            (_ministryPlatformService.GetPageViewRecords("ResponseByOpportunityAndEvent", apiToken,
                                searchString, "", 0)));
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    string.Format(
                        "GetOpportunityResponse failed.  Participant Id: {0}, Opportunity Id: {1}, Event Id: {2}",
                        participant, opportunityId, eventId), ex.InnerException);
            }

            if (dictionaryList.Count == 0)
            {
                return new Response();
            }

            var response = new Response();
            try
            {
                var dictionary = dictionaryList.First();
                response.Opportunity_ID = dictionary.ToInt("Opportunity_ID");
                response.Participant_ID = dictionary.ToInt("Participant_ID");
                response.Response_Result_ID = dictionary.ToInt("Response_Result_ID");
            }
            catch (InvalidOperationException ex)
            {
                throw new ApplicationException(
                    string.Format("RespondToOpportunity failed.  Participant Id: {0}, Opportunity Id: {1}",
                        participant, opportunityId), ex.InnerException);
            }


            return response;
        }

        public int GetOpportunitySignupCount(int opportunityId, int eventId, string token)
        {
            var search = ",,," + eventId;
            var records = _ministryPlatformService.GetSubpageViewRecords(_signedupToServeSubPageViewId, opportunityId,
                token, search);

            return records.Count();
        }

        public DateTime GetLastOpportunityDate(int opportunityId, string token)
        {
            //First get the event type
            var opp = _ministryPlatformService.GetRecordDict(_opportunityPage, opportunityId, token);
            var eventType = opp["Event_Type_ID_Text"];

            //Now get all the events for this type
            var searchString = ",," + eventType;
            var sort = "0";
            var events = _ministryPlatformService.GetRecordsDict(_eventPage, token, searchString, sort);

            //grab the last one
            try
            {
                var lastEvent = events.Last();
                var lastEventDate = DateTime.Parse(lastEvent["Event_Start_Date"].ToString());

                return lastEventDate;
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception("No events found. Cannot return the last event date.");
            }
        }

        public int RespondToOpportunity(string token, int opportunityId, string comments)
        {
            var participant = _authenticationService.GetParticipantRecord(token);
            var participantId = participant.ParticipantId;

            var values = new Dictionary<string, object>
            {
                {"Response_Date", DateTime.Now},
                {"Opportunity_ID", opportunityId},
                {"Participant_ID", participantId},
                {"Closed", false},
                {"Comments", comments}
            };

            var recordId = _ministryPlatformService.CreateRecord("OpportunityResponses", values, token, true);
            return recordId;
        }

        public int RespondToOpportunity(int participantId, int opportunityId, string comments, int eventId)
        {
            var values = new Dictionary<string, object>
            {
                {"Response_Date", DateTime.Now},
                {"Opportunity_ID", opportunityId},
                {"Participant_ID", participantId},
                {"Closed", false},
                {"Comments", comments},
                {"Event_ID", eventId},
                {"Response_Result_ID", 1}
            };

            int recordId;
            try
            {
                recordId =
                    WithApiLogin<int>(
                        apiToken =>
                            (_ministryPlatformService.CreateRecord("OpportunityResponses", values, apiToken, true)));
            }
            catch (Exception ex)
            {
                throw new ApplicationException(
                    string.Format("RespondToOpportunity failed.  Participant Id: {0}, Opportunity Id: {1}",
                        participantId, opportunityId), ex.InnerException);
            }
            return recordId;
        }

        
    }
}