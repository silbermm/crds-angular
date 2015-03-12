using crds_angular.Controllers.API;
using crds_angular.Models.MP;
using MinistryPlatform.Translation.Services;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Web.Http;
using System.Web.Http.Results;

namespace crds_angular.test.controllers
{
    [TestFixture]
    class GroupControllerTestNSubstitute
    {
        private GroupController fixture;
        private IGroupService groupService;

        [SetUp]
        public void SetUp()
        {
            groupService = Substitute.For<IGroupService>();
            fixture = new GroupController(groupService);
        }

        [Test]
        public void testCallGroupService()
        {
            ContactDTO contact = new ContactDTO();
            IHttpActionResult result = fixture.Post("3", "4", contact);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(OkResult), result);
            groupService.Received().addContactToGroup("3", "4", "1", Arg.Any<DateTime>(), null, false);
        }

    }
}
