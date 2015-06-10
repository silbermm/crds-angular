using crds_angular.Models.Crossroads.VolunteerApplication;

namespace crds_angular.Services.Interfaces
{
    public interface IVolunteerApplicationService
    {
        bool SaveStudent(StudentApplicationDto application);
        bool SaveAdult(AdultApplicationDto application);
    }
}