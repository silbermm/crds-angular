using System;
using System.Collections.Generic;
using crds_angular.Models;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Serve;
using ServingTeam = crds_angular.Models.Crossroads.Serve.ServingTeam;

namespace crds_angular.Services.Interfaces
{
    public interface IPersonService
    {
        void SetProfile(String token, Person person);
        List<Skill> GetLoggedInUserSkills(int contactId, string token);
        Person GetLoggedInUserProfile(String token);
        List<FamilyMember> GetMyFamily(int contactId, string token);
        List<ServingTeam> GetMyFamiliesServingTeams(int contactId, string token);
        List<ServingDay> GetMyFamiliesServingDays(List<ServingTeam> teams, string token);
    }
}