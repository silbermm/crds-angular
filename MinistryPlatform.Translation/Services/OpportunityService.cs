using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Web;
using MinistryPlatform.Translation.Exceptions;
using MinistryPlatform.Translation.PlatformService;

namespace MinistryPlatform.Translation.Services
{
    public class OpportunityService
    {
        public static void RespondToOpportunity(string token, int opportunityId, string comments)
        {
            try
            {
                var platformServiceClient = new PlatformServiceClient();

                var participant = AuthenticationService.GetParticipantRecord(token);
                var participantId = participant.ParticipantId;

                var values = new Dictionary<string, object>
                {
                    {"Response_Date", DateTime.Now},
                    {"Opportunity_ID", opportunityId},
                    {"Participant_ID", participantId},
                    {"Closed", false},
                    {"Comments", comments}
                };

                using (new OperationContextScope(platformServiceClient.InnerChannel))
                {
                    if (WebOperationContext.Current != null)
                        WebOperationContext.Current.OutgoingRequest.Headers.Add("Authorization", "Bearer " + token);
                    platformServiceClient.CreatePageRecord(
                        Convert.ToInt32(ConfigurationManager.AppSettings["OpportunityResponses"]), values, true);
                }
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message == "Sequence contains more than one element")
                {
                    throw new MultipleRecordsException("Multiple Participant records found! Only one participant allowed per Contact.");
                }

            }
        }
    }
}
