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
    [Route("api/profile/stuff/{contactId}")]
        public IHttpActionResult GetSomeStuff(int contactId)
        {
            //var personService = new PersonService();
            //var stuff = personService.GetMeMyFamilysServingStuff(token);

            //var token = TranslationService.Login("tmaddox", "crds1234");
            //var token =
            //        "AAEAAHQzsyBskUp5lt-v1AssHzc_THBNL9JtRBGxf7pb-nDwFL-bx6zjOZ-XesOI-8gTIv7ZlDRjYZEWVK_q91TNeAoaP3qaKc_MGlnEz5GmHzAsM0v4mUdgLs7H-As7lBB-hMgit-xc57ofSXU4s9KblBQ5ZcKgbezaFYNcC0DMMetN-pouQ8XgqpTeYxAQWg8FcDa9EvQQwR6zMgCOhSTYNiPMnvefshJTqmJK3LtWi7kgoPrQg-6FKe5SC0kLGb1blbm1Mfsz4QhU9WqvyAIG4-jyN2ZIUJjrqYW52fCSqAY-_zmDAiJNXZqUhFdddV2vi0353pRSc7uzeLSOgyIeSHTIAAAATGlmZXRpbWU9MTgwMCZDbGllbnRJZGVudGlmaWVyPWNsaWVudCZVc2VyPWFiYzRkMDMwLTk3YjktNGY2Yi04NmI3LTMwY2QxMWQxODM3OCZTY29wZT1odHRwJTNBJTJGJTJGd3d3LnRoaW5rbWluaXN0cnkuY29tJTJGZGF0YXBsYXRmb3JtJTJGc2NvcGVzJTJGYWxsJnRzPTE0MjY2OTQ4NTUmdD1Eb3ROZXRPcGVuQXV0aC5PQXV0aDIuQWNjZXNzVG9rZW4";
            return Authorized(token =>
            {
                var personService = new PersonService();
                var stuff = personService.GetMeMyFamilysServingStuff(contactId, token);
                var list = personService.GetEventsStuff(stuff, token);
                if (list == null)
                {
                    return Unauthorized();
                }
                return this.Ok(list);
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