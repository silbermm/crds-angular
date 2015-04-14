using System;
using System.Collections.Generic;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Serve;

namespace crds_angular.Services.Interfaces
{
    public interface IServeService
    {
        List<FamilyMember> GetMyImmediateFamily(int contactId, string token);
        List<ServingTeam> GetServingTeams(string token);
        List<ServingDay> GetServingDays(string token);

        bool SaveServeResponse(string token, int contactid, int opportunityId, int eventTypeId, DateTime startDate,
            DateTime endDate, bool signUp);
    }
}