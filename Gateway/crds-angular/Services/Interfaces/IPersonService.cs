using System;
using System.Collections.Generic;
using crds_angular.Models;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Profile;
using crds_angular.Models.MP;
using MinistryPlatform.Models.DTO;

namespace crds_angular.Services.Interfaces
{
    public interface IPersonService
    {
        void SetProfile(String token, Person person);        
        Person GetLoggedInUserProfile(String token);
        Person GetPerson(int contactId);
        List<RoleDto> GetLoggedInUserRoles(string token);
    }
}