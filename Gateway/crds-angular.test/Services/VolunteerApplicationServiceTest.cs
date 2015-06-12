using System.Collections.Generic;
using crds_angular.Models.Crossroads.Serve;
using crds_angular.Services;
using crds_angular.Services.Interfaces;
using Moq;
using NUnit.Framework;

namespace crds_angular.test.Services
{
    [TestFixture]
    public class VolunteerApplicationServiceTest
    {
        private Mock<IServeService> _serveService;
        private VolunteerApplicationService _fixture;

        [SetUp]
        public void SetUp()
        {
            _serveService = new Mock<IServeService>();

            _fixture = new VolunteerApplicationService(_serveService.Object);
        }

        [Test]
        public void FamilyThatUserCanSubmitFor()
        {
            const int contactId = 987654;
            var mockFamily = MockFamily();

            _serveService.Setup(m => m.GetImmediateFamilyParticipants(contactId, It.IsAny<string>()))
                .Returns(mockFamily);

            var family = _fixture.FamilyThatUserCanSubmitFor(contactId, It.IsAny<string>());

            _serveService.VerifyAll();
            Assert.AreEqual(mockFamily.Count - 1, family.Count);
        }

        private List<FamilyMember> MockFamily()
        {
            return new List<FamilyMember>
            {
                new FamilyMember
                {
                    ContactId = 768379,
                    ParticipantId = 994377,
                    PreferredName = "Tony",
                    LastName = null,
                    LoggedInUser = true,
                    Email = "tmaddox33mp1@gmail.com",
                    RelationshipId = 0
                },
                new FamilyMember
                {
                    ContactId = 1519134,
                    ParticipantId = 1446324,
                    PreferredName = "Brady",
                    LastName = "Bunch",
                    LoggedInUser = false,
                    Email = null,
                    RelationshipId = 6
                },
                new FamilyMember
                {
                    ContactId = 333,
                    ParticipantId = 333,
                    PreferredName = "wife",
                    LastName = "spouse",
                    LoggedInUser = false,
                    Email = "wife@spouse.net",
                    RelationshipId = 1
                }
            };
        }
    }
}