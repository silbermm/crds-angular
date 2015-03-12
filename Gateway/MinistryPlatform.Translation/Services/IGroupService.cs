using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MinistryPlatform.Translation.Services
{
    public interface IGroupService
    {
        int addUserToGroup(String userToken, String groupId, String groupRoleId, DateTime startDate,
            DateTime? endDate = null, Boolean? employeeRole = false);
    }
}
