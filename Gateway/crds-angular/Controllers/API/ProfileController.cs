using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Exceptions.Models;
using crds_angular.Models;
using crds_angular.Security;
using crds_angular.Services.Interfaces;
using log4net;
using IPersonService = crds_angular.Services.Interfaces.IPersonService;
using IDonorService = crds_angular.Services.Interfaces.IDonorService;

namespace crds_angular.Controllers.API
{
    public class ProfileController : MPAuth
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IPersonService _personService;
        private readonly IServeService _serveService;
        private readonly IDonorService _donorService;
        private readonly IUserImpersonationService _impersonationService;

        public ProfileController(IPersonService personService, IServeService serveService, IUserImpersonationService impersonationService, IDonorService donorService)
        {
            _personService = personService;
            _serveService = serveService;
            _impersonationService = impersonationService;
            _donorService = donorService;
        }

        [ResponseType(typeof (Person))]
        [Route("api/profile")]
        public IHttpActionResult GetProfile([FromUri(Name = "impersonateDonorId")]int? impersonateDonorId = null)
        {
            return Authorized(token =>
            {
                var impersonateUserId = impersonateDonorId == null ? string.Empty : _donorService.GetContactDonorForDonorId(impersonateDonorId.Value).Email;
                var person = string.IsNullOrWhiteSpace(impersonateUserId)
                    ? _personService.GetLoggedInUserProfile(token)
                    : _impersonationService.WithImpersonation(token, impersonateUserId, () => _personService.GetLoggedInUserProfile(token));
                if (person == null)
                {
                    return Unauthorized();
                }
                return Ok(person);
            });
        }

        [ResponseType(typeof(Person))]
        [Route("api/profile/{contactId}")]
        public IHttpActionResult GetProfile(int contactId)
        {
            return Authorized(token =>
            {
                // does the logged in user have permission to view this contact?
                var loggedInUser = _personService.GetLoggedInUserProfile(token);
                var family = _serveService.GetImmediateFamilyParticipants(loggedInUser.ContactId, token);
                Person person = null;
                if (family.Where(f => f.ContactId == contactId).ToList().Count > 0)
                {
                    person = _personService.GetPerson(contactId);
                }
                if (person == null)
                {
                    return Unauthorized();
                }
                return this.Ok(person);
            });
        }

        [Route("api/profile")]
        public IHttpActionResult Post([FromBody] Person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Authorized(t =>
            {
                // does the logged in user have permission to view this contact?
                var loggedInUser = _personService.GetLoggedInUserProfile(t);
                var family = _serveService.GetImmediateFamilyParticipants(loggedInUser.ContactId, t);

                if (family.Where(f => f.ContactId == person.ContactId).ToList().Count > 0)
                {
                    try
                    {
                        _personService.SetProfile(t, person);
                        return this.Ok();
                    }
                    catch(Exception ex)
                    {
                        var apiError = new ApiErrorDto("GP Export File Creation Failed", ex);
                        throw new HttpResponseException(apiError.HttpResponseMessage);   
                    }
                }
                else
                {
                    return this.Unauthorized();
                }
               
            });
        }
    }
}
