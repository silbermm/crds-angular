using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Services.Interfaces;
using MinistryPlatform.Models;

namespace MinistryPlatform.Translation.Services
{
    public class ProgramService : BaseService,IProgramService
    {
        private readonly IMinistryPlatformService _ministryPlatformService;
        private readonly int _onlineGivingProgramsPageViewId;
        private readonly int _programsPageId;

        public ProgramService(IMinistryPlatformService ministryPlatformService, IAuthenticationService authenticationService, IConfigurationWrapper configurationWrapper)
            : base(authenticationService, configurationWrapper)
        {
            _ministryPlatformService = ministryPlatformService;
            _onlineGivingProgramsPageViewId = configurationWrapper.GetConfigIntValue("OnlineGivingProgramsPageViewId");
            _programsPageId = configurationWrapper.GetConfigIntValue("Programs");
        }

        public List<Program> GetAllPrograms(string token)
        {
            var records = _ministryPlatformService.GetPageViewRecords("AllProgramsList", token);
            var programs = new List<Program>();
            if (records == null || records.Count == 0)
            {
                return programs;
            }
            programs.AddRange(records.Select(Mapper.Map<Program>));

            return programs;
        }

        public List<Program> GetOnlineGivingPrograms(int? programType)
        {
            var searchString = programType == null ? null : string.Format(",,,{0}", programType);
            var programs =
                WithApiLogin(
                    apiToken => (_ministryPlatformService.GetPageViewRecords(_onlineGivingProgramsPageViewId, apiToken, searchString)));

            var programList = new List<Program>();
            if (programs == null || programs.Count == 0)
            {
                return programList;
            }
            programList.AddRange(programs.Select(Mapper.Map<Program>));

            return programList;
        }

        public Program GetProgramById(int programId)
        {
            return (WithApiLogin(token => (Mapper.Map<Program>(_ministryPlatformService.GetRecordDict(_programsPageId, programId, token)))));
        }
    }
}