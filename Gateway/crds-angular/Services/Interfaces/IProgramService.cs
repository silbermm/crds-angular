using System.Collections.Generic;
using crds_angular.Models.Crossroads;

namespace crds_angular.Services.Interfaces
{
    public interface IProgramService
    {
        List<ProgramDTO> GetOnlineGivingPrograms(int? programType = null);
        ProgramDTO GetProgramById(int programId);
        List<ProgramDTO> GetAllProgramsForReal(string token);
    }
}
