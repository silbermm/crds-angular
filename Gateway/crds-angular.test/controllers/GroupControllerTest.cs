using System;
using crds_angular.Controllers.API;
using crds_angular.Models.MP;
using MinistryPlatform.Translation.Services;
using NUnit.Framework;
using Rhino.Mocks;

namespace crds_angular.test.controllers
{

    [TestFixture]
    public class GroupControllerTest
    {
        private MockRepository mocks;
        private IGroupService _groupService;
        private GroupController controller;
        [SetUp]
        public void SetUp()
        {
            mocks = new MockRepository();
            _groupService = mocks.StrictMock<IGroupService>();
            controller = new GroupController(_groupService);
        }
        [Test]
        public void ShouldUseGroupService()
        {
            // Arrange
            var contact = new Contact();
            Expect.Call(_groupService.addContactToGroup());
            
            // Act
            controller.Post("1", "2", contact);
            
            // Assert
            mocks.VerifyAll();
        }
    }
}
