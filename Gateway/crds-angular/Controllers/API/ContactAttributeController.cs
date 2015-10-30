using System.Web.Http;
using crds_angular.Models.Crossroads.Profile;
using crds_angular.Security;
using crds_angular.Services.Interfaces;

namespace crds_angular.Controllers.API
{
    public class ContactAttributeController : MPAuth
    {
        private readonly IContactAttributeService _contactAttributeService;

        public ContactAttributeController(IPersonService personService, IContactAttributeService contactAttributeService)
        {
            _contactAttributeService = contactAttributeService;
        }
        
        [Route("api/contact/attribute/{contactId}")]
        public IHttpActionResult Post(int contactId, [FromBody] ContactAttributeDTO contactAttribute)
        {
            return Authorized(token =>
            {                
                _contactAttributeService.SaveContactMultiAttribute(token , contactId, contactAttribute);                
                return this.Ok();
            });
        }
    }
}