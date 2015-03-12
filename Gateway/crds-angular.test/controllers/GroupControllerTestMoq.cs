using crds_angular.Controllers.API;
using crds_angular.Models.MP;
using MinistryPlatform.Translation.Services;
using Moq;
using Moq.Matchers;
using NUnit.Framework;
using System;
using System.Web.Http;
using System.Web.Http.Results;

namespace crds_angular.test.controllers
{
    [TestFixture]
    class GroupControllerTestMoq
    {
        private GroupController fixture;
        private Mock<IGroupService> groupServiceMock;

        [SetUp]
        public void SetUp()
        {
            groupServiceMock = new Mock<IGroupService>();
            fixture = new GroupController(groupServiceMock.Object);
        }

        [Test]
        public void testCallGroupService()
        {
            ContactDTO contact = new ContactDTO();
            IHttpActionResult result = fixture.Post("3", "4", contact);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(OkResult), result);
            groupServiceMock.Verify(mocked => mocked.addContactToGroup("3", "4", "1", It.IsAny<DateTime>(), null, false));
        }

    }
}
