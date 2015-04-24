using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinistryPlatform.Translation.Services.Interfaces;
using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Services
{
    public class ProgramService : BaseService,IProgramService
    {
        private IMinistryPlatformService ministryPlatformService;
        private readonly int _onlineGivingProgramsPageViewId = Convert.ToInt32(AppSettings("OnlineGivingProgramsPageViewId"));

        public ProgramService(IMinistryPlatformService ministryPlatformService)
        {
            this.ministryPlatformService = ministryPlatformService;
        }

        public List<Program> GetOnlineGivingPrograms(int programType)
        {
            var programs =
                WithApiLogin<List<Dictionary<string,Object>>>(
                    apiToken =>
                    {
                        return (ministryPlatformService.GetPageViewRecords(_onlineGivingProgramsPageViewId, apiToken, ",,," + programType));
                    });

            var programList = new List<Program>();
            if (programs == null || programs.Count == 0)
            {
                return programList;
            }
            foreach (var p in programs)
            {
                var program = new Program();
                program.Name = (string)p["Program_Name"];
                program.ProgramId = (int) p["Program_ID"];
                programList.Add(program);
            }

            return programList;
        }
    }
}
