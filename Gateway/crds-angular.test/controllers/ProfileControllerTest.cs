using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Results;
using crds_angular.Controllers.API;
using crds_angular.Models.Crossroads.Serve;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Translation.Services.Interfaces;
using Moq;
using NUnit.Framework;

namespace crds_angular.test.controllers
{
    [TestFixture]
    public class ProfileControllerTest
    {
        private ProfileController _fixture;

        private Mock<IPersonService> _personServiceMock;

        private string _authType;
        private string _authToken;

        [SetUp]
        public void SetUp()
        {
            _personServiceMock = new Mock<IPersonService>();

            _fixture = new ProfileController(_personServiceMock.Object);

            _authType = "auth_type";
            _authToken = "auth_token";
            _fixture.Request = new HttpRequestMessage();
            _fixture.Request.Headers.Authorization = new AuthenticationHeaderValue(_authType, _authToken);
            _fixture.RequestContext = new HttpRequestContext();
        }

        
    }
}