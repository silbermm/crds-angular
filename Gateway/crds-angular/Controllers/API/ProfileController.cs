using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Models;
using crds_angular.Models.Crossroads.Serve;
using crds_angular.Security;
using crds_angular.Services;

namespace crds_angular.Controllers.API
{
    public class ProfileController : MPAuth
    {
    [ResponseType(typeof(List<ServingDay>))]
    [Route("api/profile/familyserve/{contactId}")]
        public IHttpActionResult GetFamilyServeDays(int contactId)
        {
            return Authorized(token =>
            {
                try
                {
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
                    //var ex = new Exception("Hi Matt");
                    return this.BadRequest(e.Message);
                    //return this.InternalServerError(ex);
                    //return new CustomErrorIHttpActionResult(e.Message, Request);
                }
            });

        }

        [ResponseType(typeof (Person))]
        [Route("api/profile/family/{contactId}")]
        public IHttpActionResult GetFamily(int contactId)
        {
            return Authorized(token =>
            {
                var personService = new PersonService();
                var list = personService.GetMyFamily(contactId, token);
                if (list == null)
                {
                    return Unauthorized();
                }
                return this.Ok(list);
            });
        }

        [ResponseType(typeof(List<Models.Crossroads.ServingTeam>))]
        [Route("api/profile/serving/{contactId}")]
        public IHttpActionResult GetServingTeams(int contactId)
        {
            return Authorized(token =>
            {
                var personService = new PersonService();
                var list = personService.GetServingOpportunities(contactId, token);
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