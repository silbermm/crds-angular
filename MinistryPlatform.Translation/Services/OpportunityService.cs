using System;
using System.Collections.Generic;
using System.Configuration;

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

                var recordId = CreatePageRecordService.CreateRecord(pageId, values, token, true);
                return recordId;
        }
    }
}