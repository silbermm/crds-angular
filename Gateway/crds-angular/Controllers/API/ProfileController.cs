using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Models;
using crds_angular.Security;
using crds_angular.Services;

namespace crds_angular.Controllers.API
{
    public class ProfileController : MPAuth
    {
    [ResponseType(typeof(List<tmServingTeam>))]
    [Route("api/profile/stuff/{contactId}")]
        public IHttpActionResult GetSomeStuff(int contactId)
        {
            //var personService = new PersonService();
            //var stuff = personService.GetMeMyFamilysServingStuff(token);

            //var token = TranslationService.Login("tmaddox", "crds1234");
            var token =
                    "AAEAAAtXCHDSb1YcFWSncg_OY1SlC5e1Kg6eO7Y2MQWFfXklGtlmdCDkMiuEK4kNJsVEKqpg1fqCd-BPLEs10xtAqodvx0CsC94p875FYaJE0e5-opQMzQmQIj9Yle3OUP2ygMNGfZzUoh0flEC4i5dOKJlAhayi-2RRTuVnkiSN_9py8e8rz5zZO1xJgCcNCPm4unN4n78pQJuymoL4WJCxcn_JxSRmWTtF1IIdGV8HOrKyTlaqQ0gGrxFh-E9SH7SSnGXRdBqsZ0JCYGUcyPpYDzyBz5S5VdFNtzIM6-NEOED_QMudHlAyWXoI397jrReWtIr62qFVZNZuBet25uinlBXIAAAATGlmZXRpbWU9MTgwMCZDbGllbnRJZGVudGlmaWVyPWNsaWVudCZVc2VyPWFiYzRkMDMwLTk3YjktNGY2Yi04NmI3LTMwY2QxMWQxODM3OCZTY29wZT1odHRwJTNBJTJGJTJGd3d3LnRoaW5rbWluaXN0cnkuY29tJTJGZGF0YXBsYXRmb3JtJTJGc2NvcGVzJTJGYWxsJnRzPTE0MjY2OTMwMTUmdD1Eb3ROZXRPcGVuQXV0aC5PQXV0aDIuQWNjZXNzVG9rZW4";
            //return Authorized(token =>
            //{
                var personService = new PersonService();
                var stuff = personService.GetMeMyFamilysServingStuff(contactId, token);
                var list = personService.GetEventsStuff(stuff, token);
                if (list == null)
                {
                    return Unauthorized();
                }
                return this.Ok(list);
            //});

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