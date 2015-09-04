using System.Web.Http;
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
