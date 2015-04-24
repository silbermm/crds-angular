using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Models.Crossroads;
using crds_angular.Security;
using MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.Controllers.API
{
    public class ProgramController : MPAuth
    {
        private IProgramService programService;

        public ProgramController(IProgramService programService)
        {
            this.programService = programService;
        }

        [Route("api/programs/{programType}")]
        public IHttpActionResult Get(int programType)
        {
                var programs = programService.GetOnlineGivingPrograms(programType);

                var programList = new List<ProgramDTO>();

                foreach (var program in programs)
                {
                    var programDTO = new ProgramDTO();
                    programDTO.Name = program.Name;
                    programDTO.ProgramId = program.ProgramId;
                    programList.Add(programDTO);
                }
                return Ok(programList);
        }

    }
}
