using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Services.Interfaces
{
    public interface IProgramService
    {
        List<Program> GetOnlineGivingPrograms(int? programType);
        Program GetProgramById(int programId);
        List<Program> GetAllPrograms(string token);
    }
}
