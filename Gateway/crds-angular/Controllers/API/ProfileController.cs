using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Models;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Serve;
using crds_angular.Security;
using crds_angular.Services;
using MinistryPlatform.Translation.Services;
using ServingTeam = crds_angular.Models.Crossroads.ServingTeam;

namespace crds_angular.Controllers.API
{
    public class ProfileController : MPAuth
    {
        [ResponseType(typeof (List<ServingDay>))]
        [Route("api/profile/servesignup")]
        public IHttpActionResult GetFamilyServeDays()
        {
            return Authorized(token =>
            {
                try
                {
                    var contactId = AuthenticationService.GetContactId(token);
                    var personService = new PersonService();
                    var stuff = personService.GetMyFamiliesServingTeams(contactId, token);
                    var list = personService.GetMyFamiliesServingEvents(stuff, token);
                    if (list == null)
                    {
                        return Unauthorized();
                    }
                    return this.Ok(list);
                }
                catch (Exception e)
                {
                    return this.BadRequest(e.Message);
                }
            });
        }

        [ResponseType(typeof (List<FamilyMember>))]
        [Route("api/profile/family")]
        public IHttpActionResult GetFamily()
        {
            return Authorized(token =>
            {
                var contactId = AuthenticationService.GetContactId(token);
                var personService = new PersonService();
                var list = personService.GetMyFamily(contactId, token);
                if (list == null)
                {
                    return Unauthorized();
                }
                return this.Ok(list);
            });
        }

        [ResponseType(typeof (Person))]
        [Route("api/profile")]
        public IHttpActionResult GetProfile()
        {
            return Authorized(token =>
            {
                var personService = new PersonService();
                var person = personService.getLoggedInUserProfile(token);
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
                var personService = new PersonService();
                personService.setProfile(t, person);
                return this.Ok();
            });
        }
    }
}