using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Exceptions.Models;
using crds_angular.Models.Crossroads;
using crds_angular.Security;
using crds_angular.Services.Interfaces;

namespace crds_angular.Controllers.API
{
    public class ProgramController : MPAuth
    {
        private readonly IProgramService _programService;

        public ProgramController(IProgramService programService)
        {
            _programService = programService;
        }

        [Route("api/all-programs")]
        [ResponseType(typeof (IList<ProgramDTO>))]
        public IHttpActionResult Get()
        {
            return Authorized(token =>
            {
                try
                {
                    var programs = _programService.GetAllProgramsForReal(token);
                    return this.Ok(programs);
                }
                catch (Exception exception)
                {
                    var apiError = new ApiErrorDto("Get All Programs", exception);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }

        [Route("api/programs")]
        [HttpGet]
        public IHttpActionResult GetAllPrograms([FromUri(Name = "excludeTypes")] int[] excludeTypes = null)
        {
            var result = _programService.GetOnlineGivingPrograms();
            if (excludeTypes == null || excludeTypes.Length == 0)
            {
                return (Ok(result));
            }

            foreach (var t in excludeTypes)
            {
                result.RemoveAll(p => p.ProgramType == t);
            }

            return Ok(result);
        }

        [Route("api/programs/{programType}")]
        [HttpGet]
        public IHttpActionResult GetProgramsByType(int programType)
        {
            return Ok(_programService.GetOnlineGivingPrograms(programType));
        }
    }
}