using System;
using System.Collections.Generic;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Serve;

namespace crds_angular.Services.Interfaces
{
    public interface IServeService
    {
        List<FamilyMemberDto> GetImmediateFamilyParticipants(int contactId, string token);
        List<FamilyMember> GetMyImmediateFamily(int contactId, string token);
        DateTime GetLastServingDate(int opportunityId, string token);
        List<ServingTeam> GetServingTeams(string token);
        List<ServingDay> GetServingDays(string token);

        List<ServingDay> GetServingDaysFaster(string token);
        Capacity OpportunityCapacity(int opportunityId, int eventId, int? minNeeded, int? maxNeeded, string token);

        bool SaveServeRsvp(string token, int contactid, int opportunityId, int eventTypeId, DateTime startDate,
            DateTime endDate, bool signUp, bool alternateWeeks);
    }
}