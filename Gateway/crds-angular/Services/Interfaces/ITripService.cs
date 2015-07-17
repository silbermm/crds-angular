using System.Collections.Generic;
using crds_angular.Models.Crossroads.Trip;

namespace crds_angular.Services.Interfaces
{
    public interface ITripService
    {
        List<Particpant> Search(string search, string token);
    }
}
