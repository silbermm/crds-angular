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
        [Route("api/profile/stuff")]
        public IHttpActionResult GetSomeStuff()
        {
            //var personService = new PersonService();
            //var stuff = personService.GetMeMyFamilysServingStuff(token);

        //var token =
        //    "AAEAADfUoZ5uMrc-M2oAr-hHZnYr-qkog7TmaQ-u0p9o50GHcfK92ief0psjjj-zp9NlpVIAy2Sq18tVc0iPwBROjFSXXjXETUaXH72QNw3SKzm2OGDla3dcymg0jEOIVbpECZbmV1EdBVtXQGSsMzvhXzoWP6a70y2lXKV1CUdzvFDAPX5ORxxgJwoq5Q67BI5BPBHnuLmb1TXDNkUrLoui5GRxD-rbJ9XRfOQT_cslQaeUgsgm8bfrdoVWo1vyXiQGabcPKpTZegQTBfAfrBakB1bFKPI9AohDEuEFrorc2jtbFhzwTBnHSgcGMpBhwA_I1vZbQbrYSLLCYaRtPAI3Ld3IAAAATGlmZXRpbWU9MTgwMCZDbGllbnRJZGVudGlmaWVyPWNsaWVudCZVc2VyPWFiYzRkMDMwLTk3YjktNGY2Yi04NmI3LTMwY2QxMWQxODM3OCZTY29wZT1odHRwJTNBJTJGJTJGd3d3LnRoaW5rbWluaXN0cnkuY29tJTJGZGF0YXBsYXRmb3JtJTJGc2NvcGVzJTJGYWxsJnRzPTE0MjY2ODM1MzImdD1Eb3ROZXRPcGVuQXV0aC5PQXV0aDIuQWNjZXNzVG9rZW4";
            return Authorized(token =>
            {
                var personService = new PersonService();
                var stuff = personService.GetMeMyFamilysServingStuff( token);
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