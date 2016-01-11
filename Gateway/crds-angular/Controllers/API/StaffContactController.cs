using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Exceptions.Models;
using crds_angular.Security;
using MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.Controllers.API
{
    public class StaffContactController : MPAuth
    {
        private readonly crds_angular.Services.Interfaces.IStaffContactService _contactService;

        public StaffContactController(crds_angular.Services.Interfaces.IStaffContactService contactService)
        {
            _contactService = contactService;
        }

        [ResponseType(typeof (List<Dictionary<string, object>>))]
        [Route("api/contact/{userRole}")]
        public IHttpActionResult GetByUserRole(string userRole)
        {
            return Authorized(t =>
            {
                try
            {
                var users = _contactService.GetContactsByRole(userRole, t);

                return Ok(users);
            }
                catch (Exception e)
                {
                    var msg = "Error getting users by role " + userRole;
                    logger.Error(msg, e);
                    var apiError = new ApiErrorDto(msg, e);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
            });
        }
    }
}