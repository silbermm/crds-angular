using crds_angular.App_Start;
using crds_angular.Services;
using MinistryPlatform.Translation.Services;
using NUnit.Framework;

namespace crds_angular.test.Services
{
    class PersonServiceTest
    {
        private PersonService _personService = new PersonService();

        //private const string USERNAME = "testme";
        //private const string PASSWORD = "changeme";
        private const string USERNAME = "tmaddox@aol.com";
        private const string PASSWORD = "crds1234";

        [Test]
        public void GetServingTeamsForContact()
        {
            //force AutoMapper to register
            AutoMapperConfig.RegisterMappings();

            // First we need to get a session
            var token = TranslationService.Login(USERNAME, PASSWORD);
            Assert.IsNotNull(token, "Token should be valid");

            //Need Contact Id for token's Contact
            var contactId = AuthenticationService.GetContactId(token);

            // Make the call...
            var result = _personService.GetServingOpportunities(contactId, token);
            Assert.IsNotNull(result);
        }
    }
}
