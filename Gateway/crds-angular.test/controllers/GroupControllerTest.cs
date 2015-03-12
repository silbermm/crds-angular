using System;
using crds_angular.Controllers.API;
using crds_angular.Models.MP;
using MinistryPlatform.Translation.Services;
using NUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;

namespace crds_angular.test.controllers
{
    [TestFixture]
    public class GroupControllerTest
    {
        //private MockRepository mocks;
        private IGroupService _groupService;
        private GroupController controller;

        [SetUp]
        public void SetUp()
        {
            //mocks = new MockRepository();
            _groupService = MockRepository.GenerateMock<IGroupService>();
            controller = new GroupController(_groupService);
        }

        [Test]
        public void ShouldUseGroupService()
        {
            // Arrange
            var contact = new ContactDTO();
            _groupService.Expect(
                e =>
                    e.addContactToGroup(Arg<string>.Is.Equal("1"), Arg<string>.Is.Equal("2"), Arg<string>.Is.Anything,
                        Arg<DateTime>.Is.Anything, Arg<DateTime>.Is.Anything, Arg<bool>.Is.Anything));

            // Act
            controller.Post("1", "2", contact);

            // Assert
            _groupService.Replay();
            _groupService.VerifyAllExpectations();
        }
    }
}