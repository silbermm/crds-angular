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
        void addContactToGroup(string contactId, string groupId, string groupRoleId, DateTime startDateTime,
            DateTime endDateTime, bool employeeRole);
    }
}
