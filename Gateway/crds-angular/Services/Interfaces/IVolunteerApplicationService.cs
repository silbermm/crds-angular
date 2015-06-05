using System;
using System.Collections.Generic;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Serve;
using crds_angular.Models.Crossroads.VolunteerApplication;

namespace crds_angular.Services.Interfaces
{
    public interface IVolunteerApplicationService
    {
        bool Save(VolunteerApplicationDto application);
    }
}