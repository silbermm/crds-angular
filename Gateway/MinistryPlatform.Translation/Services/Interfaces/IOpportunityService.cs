using System;
using System.Collections.Generic;
using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IOpportunityService
    {
        List<Opportunity> GetOpportunitiesForGroup(int groupId, string token);
        int GetOpportunitySignupCount(int opportunityId, int eventId, string token);
        DateTime GetLastOpportunityDate(int opportunityId, string token);
        int RespondToOpportunity(string token, int opportunityId, string comments);
        Response GetOpportunityResponse(int opportunityId, int eventId, Participant participant);
        int RespondToOpportunity(int participantId, int opportunityId, string comments, int eventId, bool response);
    }
}
