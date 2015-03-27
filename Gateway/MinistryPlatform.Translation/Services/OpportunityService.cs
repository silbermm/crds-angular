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
    }
}