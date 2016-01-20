using System.Web.Http;
using crds_angular.Exceptions;
using crds_angular.Models.Crossroads;
using crds_angular.Services.Interfaces;
using crds_angular.Exceptions.Models;

namespace crds_angular.Controllers.API
{
    public class UserController : ApiController
    {

        private readonly IAccountService _accountService;

        public UserController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public IHttpActionResult Post([FromBody] User user)
        {
            try
            {
                var returnvalue = _accountService.RegisterPerson(user);
                return Ok(returnvalue);
            }
            catch (DuplicateUserException e)
            {
                var apiError = new ApiErrorDto("Duplicate User", e);
                throw new HttpResponseException(apiError.HttpResponseMessage);                
            }
        }
    }
}