using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using crds_angular.Models.Json;

namespace crds_angular.Services.Interfaces
{
    public interface ILoginService
    {
        bool PasswordResetRequest(string email);
        bool AcceptPasswordResetRequest(string email, string token, string password);
    }
}
