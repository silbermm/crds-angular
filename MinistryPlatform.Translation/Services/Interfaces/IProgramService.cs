using System.Collections.Generic;
using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IProgramService
    {
        List<Program> GetOnlineGivingPrograms(int? programType);
        Program GetProgramById(int programId);
        List<Program> GetAllPrograms();
    }
}