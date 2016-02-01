
using crds_angular.Models.Crossroads.VolunteerApplication;
﻿using System.Collections.Generic;
using crds_angular.Models.Crossroads.Serve;

namespace crds_angular.Services.Interfaces
{
    public interface IVolunteerApplicationService
    {
        bool SaveStudent(StudentApplicationDto application);
        bool SaveAdult(AdultApplicationDto application);
        List<FamilyMember> FamilyThatUserCanSubmitFor(string token);
    }
}
