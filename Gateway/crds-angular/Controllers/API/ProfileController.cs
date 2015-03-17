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

        var token =
            "AAEAAHBzNn0LlVsTd9Oi3iPOl2ljh7FTkzv11UW-qHxc2jsId5mEzHFxelqTSGvcLK-PqDYgj7DAohjDGDvvzVz_04MiBBlAc3qlGShY5ztmKj6YMX82fn6ao3cNFtat6LQaPQS5itvHAXf1AABcMxCRInRD0fmBZxrdi6FNab02vG5ww36VPQGMK4lsEd072bhxMD1eOe9eAEfUCBVi1NMjevHRpy-JjaJO4MQ9_UsS6ht-wZcvKVZndVbugy67eWHx8VzaKitx_2oQBpJNQvKW8EA7c_ep8FnZR4UpoxA-I7iz_I3j3wiAV7AKy9uO0wMn1HYtNdH_CsHi8t0KyxsAgSjIAAAATGlmZXRpbWU9MTgwMCZDbGllbnRJZGVudGlmaWVyPWNsaWVudCZVc2VyPWFiYzRkMDMwLTk3YjktNGY2Yi04NmI3LTMwY2QxMWQxODM3OCZTY29wZT1odHRwJTNBJTJGJTJGd3d3LnRoaW5rbWluaXN0cnkuY29tJTJGZGF0YXBsYXRmb3JtJTJGc2NvcGVzJTJGYWxsJnRzPTE0MjY2MDkxMzMmdD1Eb3ROZXRPcGVuQXV0aC5PQXV0aDIuQWNjZXNzVG9rZW4";
            //return Authorized(token =>
            //{
                var personService = new PersonService();
                var stuff = personService.GetMeMyFamilysServingStuff( token);
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