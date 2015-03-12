using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Models;
using crds_angular.Security;
using crds_angular.Services;

namespace crds_angular.Controllers.API
{
    public class ProfileController : MPAuth
    {
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