using System;
using System.Collections.Generic;
using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IOpportunityService
    {
        Opportunity GetOpportunityById(int opportunityId, string token);
        //List<Opportunity> GetOpportunitiesForGroup(int groupId, string token);
        int GetOpportunitySignupCount(int opportunityId, int eventId, string token);
        List<DateTime> GetAllOpportunityDates(int id, string token);
        DateTime GetLastOpportunityDate(int opportunityId, string token);
        int RespondToOpportunity(string token, int opportunityId, string comments);
        Response GetOpportunityResponse(int opportunityId, int eventId, Participant participant);
        List<Response> GetOpportunityResponses(int opportunityId, string token);
        int RespondToOpportunity(int participantId, int opportunityId, string comments, int eventId, bool response);
        Group GetGroupParticipantsForOpportunity(int id, string token);
    }
}