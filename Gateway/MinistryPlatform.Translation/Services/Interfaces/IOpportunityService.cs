using System;
using System.Collections.Generic;
using MinistryPlatform.Models;
using MinistryPlatform.Models.DTO;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IOpportunityService
    {
        List<DateTime> GetAllOpportunityDates(int id, string token);
        Group GetGroupParticipantsForOpportunity(int id, string token);
        DateTime GetLastOpportunityDate(int opportunityId, string token);
        Response GetMyOpportunityResponses(int contactId, int opportunityId, string token);
        List<Opportunity> GetOpportunitiesForGroup(int groupId, string token);
        Response GetOpportunityResponse(int opportunityId, int eventId, Participant participant);
        List<Response> GetOpportunityResponses(int opportunityId, string token);
        int GetOpportunitySignupCount(int opportunityId, int eventId, string token);
        void RespondToOpportunity(RespondToOpportunityDto opportunityResponse);
        int RespondToOpportunity(int participantId, int opportunityId, string comments, int eventId, bool response);
        int RespondToOpportunity(string token, int opportunityId, string comments);
    }
}