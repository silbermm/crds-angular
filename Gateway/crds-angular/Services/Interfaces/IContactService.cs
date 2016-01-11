using System.Collections.Generic;

namespace crds_angular.Services.Interfaces
{
    public interface IStaffContactService
    {
        List<Dictionary<string, object>> GetContactsByRole(string userRole, string token);
    }
}