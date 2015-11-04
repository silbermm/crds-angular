using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Results;
using crds_angular.Controllers.API;
using crds_angular.Models;
using crds_angular.Models.Crossroads.Profile;
using crds_angular.Models.Crossroads.Serve;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Translation.Services.Interfaces;
using Moq;
using NUnit.Framework;
using IDonorService = crds_angular.Services.Interfaces.IDonorService;

namespace crds_angular.test.controllers
{
    [TestFixture]
    public class ProfileControllerTest
    {
        private ProfileController _fixture;

        private Mock<crds_angular.Services.Interfaces.IPersonService> _personServiceMock;
        private Mock<IServeService> _serveServiceMock;
        private Mock<IAuthenticationService> _authenticationServiceMock;
        private Mock<crds_angular.Services.Interfaces.IDonorService> _donorService;
        private Mock<IUserImpersonationService> _impersonationService;
        private Mock<IAuthenticationService> _authenticationService;

        private string _authType;
        private string _authToken;

        private int myContactId = 123456;

        [SetUp]
        public void SetUp()
        {
            _personServiceMock = new Mock<crds_angular.Services.Interfaces.IPersonService>();
            _serveServiceMock = new Mock<IServeService>();
            _donorService = new Mock<IDonorService>();
            _impersonationService = new Mock<IUserImpersonationService>();
            _authenticationService = new Mock<IAuthenticationService>();

            _fixture = new ProfileController(_personServiceMock.Object, _serveServiceMock.Object, _impersonationService.Object, _donorService.Object, _authenticationService.Object);
            _authenticationServiceMock = new Mock<IAuthenticationService>();

            _authType = "auth_type";
            _authToken = "auth_token";
            _fixture.Request = new HttpRequestMessage();
            _fixture.Request.Headers.Authorization = new AuthenticationHeaderValue(_authType, _authToken);
            _fixture.RequestContext = new HttpRequestContext();

            _authenticationServiceMock.Setup(mocked => mocked.GetContactId(_authType + " " + _authToken)).Returns(myContactId);

        }

        [Test]
        public void shouldGetProfileForFamilyMember()
        {
            
            Person me = new Person()
            {
                ContactId = myContactId
            };

            Person brady = new Person()
            {
                ContactId = 13579,
                NickName = "Brady"
            };

            var familyList = new List<FamilyMember>
            {
                new FamilyMember()
                {
                    Age = 52,
                    ContactId = myContactId,
                    Email = "test@mail.com",
                    LastName = "Maddox",
                    LoggedInUser = true,
                    ParticipantId = 123456,
                    PreferredName = "Tony",
                    RelationshipId = 1
                },
                new FamilyMember()
                {
                    Age = 12,
                    ContactId = 13579,
                    LastName = "Maddox",
                    PreferredName = "Brady"
                }
            };

            _serveServiceMock.Setup(x => x.GetImmediateFamilyParticipants(_authType + " " + _authToken)).Returns(familyList);
            _personServiceMock.Setup(x => x.GetLoggedInUserProfile(_authType + " " + _authToken)).Returns(me);
            _personServiceMock.Setup(x => x.GetPerson(13579)).Returns(brady);
            
            IHttpActionResult result = _fixture.GetProfile(13579);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkNegotiatedContentResult<Person>>(result);
            var r = (OkNegotiatedContentResult<Person>)result;
            Assert.IsNotNull(r.Content);
            Assert.AreEqual(r.Content.ContactId, brady.ContactId);

        }



        
    }
}