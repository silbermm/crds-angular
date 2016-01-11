using System.Collections.Generic;

namespace crds_angular.Services.Interfaces
{
    public interface IStaffContactService
    {
        List<Dictionary<string, object>> GetStaffContacts(string token);
    }
}