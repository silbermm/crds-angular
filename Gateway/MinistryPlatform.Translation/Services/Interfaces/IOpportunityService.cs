using System;
using System.Collections.Generic;
using MinistryPlatform.Models;
using MinistryPlatform.Models.DTO;
using MinistryPlatform.Translation.Models.Opportunities;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IOpportunityService
    {
        Opportunity GetOpportunityById(int opportunityId, string token);
        int GetOpportunitySignupCount(int opportunityId, int eventId, string token);
        List<DateTime> GetAllOpportunityDates(int id, string token);
        Group GetGroupParticipantsForOpportunity(int id, string token);
        DateTime GetLastOpportunityDate(int opportunityId, string token);
        int DeleteResponseToOpportunities(int participantId, int opportunityId, int eventId);
        int RespondToOpportunity(string token, int opportunityId, string comments);
        Response GetMyOpportunityResponses(int contactId, int opportunityId);
        Response GetOpportunityResponse(int contactId, int opportunityId);
        Response GetOpportunityResponse(int opportunityId, int eventId, Participant participant);
        List<MPResponse> SearchResponseByGroupAndEvent(String searchString);
        List<MPResponse> GetContactsOpportunityResponseByGroupAndEvent(int groupId, int eventId);
        List<Response> GetOpportunityResponses(int opportunityId, string token);
        void RespondToOpportunity(RespondToOpportunityDto opportunityResponse);
        int RespondToOpportunity(int participantId, int opportunityId, string comments, int eventId, bool response);
    }
}
