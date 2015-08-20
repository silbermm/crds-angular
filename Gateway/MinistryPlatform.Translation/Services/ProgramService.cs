using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Services.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Extensions;

namespace MinistryPlatform.Translation.Services
{
    public class ProgramService : BaseService,IProgramService
    {
        private IMinistryPlatformService ministryPlatformService;
        private readonly int _onlineGivingProgramsPageViewId = Convert.ToInt32(AppSettings("OnlineGivingProgramsPageViewId"));
        private readonly int programsPageId = AppSettings("Programs");

        public ProgramService(IMinistryPlatformService ministryPlatformService, IAuthenticationService authenticationService, IConfigurationWrapper configurationWrapper)
            : base(authenticationService, configurationWrapper)
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

        public Program GetProgramById(int programId)
        {
            var recordsDict = ministryPlatformService.GetRecordDict(programsPageId, programId, ApiLogin());

            var program = new Program
            {
                CommunicationTemplateId = recordsDict.ToInt("Communication_ID"),
                ProgramId = recordsDict.ToInt("Program_ID"),
                Name = recordsDict.ToString("Program_Name")
            };
            return program;
        }
    }
}
