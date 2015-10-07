using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Security;
using Crossroads.Utilities.Services;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.Controllers.API
{
    public class RoleController : MPAuth
    {

        private readonly IContactService _contactSerivce;

        public RoleController(IContactService contactSerivce)
        {
            _contactSerivce = contactSerivce;
        }

        [ResponseType(typeof(IList<int>))]
        public IHttpActionResult Get(int id)
        {
            return Authorized(token =>
            {
                try
                {
                    var contactIds = _contactSerivce.GetContactIdByRoleId(id, token);
                    return this.Ok(contactIds);
                }
                catch (Exception )
                {
                    return BadRequest();
                }
            });
        }
    }
}