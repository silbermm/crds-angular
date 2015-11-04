using System.Collections.Generic;
using crds_angular.Models.Crossroads.Serve;
using crds_angular.Services;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Translation.Services.Interfaces;
using Moq;
using NUnit.Framework;

namespace crds_angular.test.Services
{
    [TestFixture]
    public class VolunteerApplicationServiceTest
    {
        private Mock<IFormSubmissionService> _formSubmissionService;
        private Mock<IConfigurationWrapper> _configWrapper;
        private Mock<IServeService> _serveService;
        private VolunteerApplicationService _fixture;

        [SetUp]
        public void SetUp()
        {
            _formSubmissionService = new Mock<IFormSubmissionService>();
            _configWrapper = new Mock<IConfigurationWrapper>();
            _serveService = new Mock<IServeService>();

            _fixture = new VolunteerApplicationService(_formSubmissionService.Object, _configWrapper.Object,
                _serveService.Object);
        }

        [Test]
        public void FamilyThatUserCanSubmitFor()
        {
            var mockFamily = MockFamily();

            _serveService.Setup(m => m.GetImmediateFamilyParticipants(It.IsAny<string>()))
                .Returns(mockFamily);

            var family = _fixture.FamilyThatUserCanSubmitFor(It.IsAny<string>());

            _serveService.VerifyAll();
            Assert.AreEqual(mockFamily.Count - 1, family.Count);
        }

        [Test]
        //Foster Relationship, age 10,11,12,13
        public void FosterThatICanSubmitFor()
        {
            var mockFamily = MockFamilyFosterAgeTen();

            _serveService.Setup(m => m.GetImmediateFamilyParticipants(It.IsAny<string>()))
                .Returns(mockFamily);

            var family = _fixture.FamilyThatUserCanSubmitFor(It.IsAny<string>());
            _serveService.VerifyAll();
            //only remove the spouse
            Assert.AreEqual(mockFamily.Count - 1, family.Count);
        }

        [Test]
        //Foster Relationship 14+
        public void FosterThatICanNotSubmitFor()
        {
            var mockFamily = MockFamilyFosterAge14();

            _serveService.Setup(m => m.GetImmediateFamilyParticipants(It.IsAny<string>()))
                .Returns(mockFamily);

            var family = _fixture.FamilyThatUserCanSubmitFor(It.IsAny<string>());
            _serveService.VerifyAll();
            //remove the spouse and foster age 14
            Assert.AreEqual(mockFamily.Count - 2, family.Count);
        }

        [Test]
        //Foster Relationship Under 10
        public void FosterUnderTen()
        {
            var mockFamily = MockFamilyFosterAge9();

            _serveService.Setup(m => m.GetImmediateFamilyParticipants(It.IsAny<string>()))
                .Returns(mockFamily);

            var family = _fixture.FamilyThatUserCanSubmitFor(It.IsAny<string>());
            _serveService.VerifyAll();
            //remove the spouse and foster under 10
            Assert.AreEqual(mockFamily.Count - 2, family.Count);
        }

        [Test]
        //Child Relationship, age 10,11,12,13
        public void ChildICanSubmitFor()
        {
            var mockFamily = MockFamilyChildAgeTen();

            _serveService.Setup(m => m.GetImmediateFamilyParticipants(It.IsAny<string>()))
                .Returns(mockFamily);

            var family = _fixture.FamilyThatUserCanSubmitFor(It.IsAny<string>());
            _serveService.VerifyAll();
            //only remove the spouse
            Assert.AreEqual(mockFamily.Count - 1, family.Count);
        }

        [Test]
        //Child Relationship, age 14+
        public void ChildICanNotSubmitFor()
        {
            var mockFamily = MockFamilyChildAge14();

            _serveService.Setup(m => m.GetImmediateFamilyParticipants(It.IsAny<string>()))
                .Returns(mockFamily);

            var family = _fixture.FamilyThatUserCanSubmitFor(It.IsAny<string>());
            _serveService.VerifyAll();
            //remove the spouse and child age 14
            Assert.AreEqual(mockFamily.Count - 2, family.Count);
        }

        [Test]
        //Child Relationship Under 10
        public void ChildUnderTen()
        {
            var mockFamily = MockFamilyChildAge9();

            _serveService.Setup(m => m.GetImmediateFamilyParticipants(It.IsAny<string>()))
                .Returns(mockFamily);

            var family = _fixture.FamilyThatUserCanSubmitFor(It.IsAny<string>());
            _serveService.VerifyAll();
            //remove the spouse and child under 10
            Assert.AreEqual(mockFamily.Count - 2, family.Count);
        }

        [Test]
        //Legal ward 10+
        public void LegalWardOver14ThatICanSubmitFor()
        {
            const int contactId = 1111;
            var mockFamily = MockFamilyLegalWard();

            _serveService.Setup(m => m.GetImmediateFamilyParticipants(It.IsAny<string>()))
                .Returns(mockFamily);

            var family = _fixture.FamilyThatUserCanSubmitFor(It.IsAny<string>());
            _serveService.VerifyAll();
            //remove the spouse only
            Assert.AreEqual(mockFamily.Count - 1, family.Count);
        }

        private static List<FamilyMember> MockFamily()
        {
            return new List<FamilyMember>
            {
                new FamilyMember
                {
                    ContactId = 1111,
                    ParticipantId = 1111,
                    PreferredName = "Husband",
                    LastName = null,
                    LoggedInUser = true,
                    Email = "tmaddox33mp1@gmail.com",
                    RelationshipId = 0,
                    Age = 40
                },
                new FamilyMember
                {
                    ContactId = 2222,
                    ParticipantId = 2222,
                    PreferredName = "Child",
                    LastName = "Bunch",
                    LoggedInUser = false,
                    Email = null,
                    RelationshipId = 6,
                    Age = 12
                },
                new FamilyMember
                {
                    ContactId = 333,
                    ParticipantId = 333,
                    PreferredName = "wife",
                    LastName = "spouse",
                    LoggedInUser = false,
                    Email = "wife@spouse.net",
                    RelationshipId = 1,
                    Age = 23
                }
            };
        }

        private static List<FamilyMember> MockFamilyFosterAgeTen()
        {
            return new List<FamilyMember>
            {
                new FamilyMember
                {
                    ContactId = 1111,
                    ParticipantId = 1111,
                    PreferredName = "Parent",
                    LastName = null,
                    LoggedInUser = true,
                    Email = "parent@crossroads.com",
                    RelationshipId = 0,
                    Age = 40
                },
                new FamilyMember
                {
                    ContactId = 2222,
                    ParticipantId = 2222,
                    PreferredName = "Foster",
                    LastName = "Child",
                    LoggedInUser = false,
                    Email = null,
                    RelationshipId = 21,
                    Age = 10
                },
                new FamilyMember
                {
                    ContactId = 333,
                    ParticipantId = 333,
                    PreferredName = "wife",
                    LastName = "spouse",
                    LoggedInUser = false,
                    Email = "wife@spouse.net",
                    RelationshipId = 1,
                    Age = 30
                }
            };
        }

        private static List<FamilyMember> MockFamilyFosterAge14()
        {
            return new List<FamilyMember>
            {
                new FamilyMember
                {
                    ContactId = 1111,
                    ParticipantId = 1111,
                    PreferredName = "Parent",
                    LastName = null,
                    LoggedInUser = true,
                    Email = "parent@crossroads.com",
                    RelationshipId = 0,
                    Age = 40
                },
                new FamilyMember
                {
                    ContactId = 2222,
                    ParticipantId = 2222,
                    PreferredName = "Foster",
                    LastName = "Child",
                    LoggedInUser = false,
                    Email = null,
                    RelationshipId = 21,
                    Age = 14
                },
                new FamilyMember
                {
                    ContactId = 333,
                    ParticipantId = 333,
                    PreferredName = "wife",
                    LastName = "spouse",
                    LoggedInUser = false,
                    Email = "wife@spouse.net",
                    RelationshipId = 1,
                    Age = 30
                }
            };
        }

        private static List<FamilyMember> MockFamilyFosterAge9()
        {
            return new List<FamilyMember>
            {
                new FamilyMember
                {
                    ContactId = 1111,
                    ParticipantId = 1111,
                    PreferredName = "Parent",
                    LastName = null,
                    LoggedInUser = true,
                    Email = "parent@crossroads.com",
                    RelationshipId = 0,
                    Age = 40
                },
                new FamilyMember
                {
                    ContactId = 2222,
                    ParticipantId = 2222,
                    PreferredName = "Foster",
                    LastName = "Child",
                    LoggedInUser = false,
                    Email = null,
                    RelationshipId = 21,
                    Age = 9
                },
                new FamilyMember
                {
                    ContactId = 333,
                    ParticipantId = 333,
                    PreferredName = "wife",
                    LastName = "spouse",
                    LoggedInUser = false,
                    Email = "wife@spouse.net",
                    RelationshipId = 1,
                    Age = 30
                }
            };
        }

        private static List<FamilyMember> MockFamilyChildAgeTen()
        {
            return new List<FamilyMember>
            {
                new FamilyMember
                {
                    ContactId = 1111,
                    ParticipantId = 1111,
                    PreferredName = "Parent",
                    LastName = null,
                    LoggedInUser = true,
                    Email = "parent@crossroads.com",
                    RelationshipId = 0,
                    Age = 40
                },
                new FamilyMember
                {
                    ContactId = 2222,
                    ParticipantId = 2222,
                    PreferredName = "Foster",
                    LastName = "Child",
                    LoggedInUser = false,
                    Email = null,
                    RelationshipId = 6,
                    Age = 10
                },
                new FamilyMember
                {
                    ContactId = 333,
                    ParticipantId = 333,
                    PreferredName = "wife",
                    LastName = "spouse",
                    LoggedInUser = false,
                    Email = "wife@spouse.net",
                    RelationshipId = 1,
                    Age = 30
                }
            };
        }

        private static List<FamilyMember> MockFamilyChildAge14()
        {
            return new List<FamilyMember>
            {
                new FamilyMember
                {
                    ContactId = 1111,
                    ParticipantId = 1111,
                    PreferredName = "Parent",
                    LastName = null,
                    LoggedInUser = true,
                    Email = "parent@crossroads.com",
                    RelationshipId = 0,
                    Age = 40
                },
                new FamilyMember
                {
                    ContactId = 2222,
                    ParticipantId = 2222,
                    PreferredName = "Foster",
                    LastName = "Child",
                    LoggedInUser = false,
                    Email = null,
                    RelationshipId = 6,
                    Age = 14
                },
                new FamilyMember
                {
                    ContactId = 333,
                    ParticipantId = 333,
                    PreferredName = "wife",
                    LastName = "spouse",
                    LoggedInUser = false,
                    Email = "wife@spouse.net",
                    RelationshipId = 1,
                    Age = 30
                }
            };
        }

        private static List<FamilyMember> MockFamilyChildAge9()
        {
            return new List<FamilyMember>
            {
                new FamilyMember
                {
                    ContactId = 1111,
                    ParticipantId = 1111,
                    PreferredName = "Parent",
                    LastName = null,
                    LoggedInUser = true,
                    Email = "parent@crossroads.com",
                    RelationshipId = 0,
                    Age = 40
                },
                new FamilyMember
                {
                    ContactId = 2222,
                    ParticipantId = 2222,
                    PreferredName = "My",
                    LastName = "Child",
                    LoggedInUser = false,
                    Email = null,
                    RelationshipId = 6,
                    Age = 9
                },
                new FamilyMember
                {
                    ContactId = 333,
                    ParticipantId = 333,
                    PreferredName = "wife",
                    LastName = "spouse",
                    LoggedInUser = false,
                    Email = "wife@spouse.net",
                    RelationshipId = 1,
                    Age = 30
                }
            };
        }

        private static List<FamilyMember> MockFamilyLegalWard()
        {
            return new List<FamilyMember>
            {
                new FamilyMember
                {
                    ContactId = 1111,
                    ParticipantId = 1111,
                    PreferredName = "Parent",
                    LastName = null,
                    LoggedInUser = true,
                    Email = "parent@crossroads.com",
                    RelationshipId = 0,
                    Age = 40
                },
                new FamilyMember
                {
                    ContactId = 2222,
                    ParticipantId = 2222,
                    PreferredName = "Legal",
                    LastName = "Ward",
                    LoggedInUser = false,
                    Email = null,
                    RelationshipId = 29,
                    Age = 14
                },
                new FamilyMember
                {
                    ContactId = 333,
                    ParticipantId = 333,
                    PreferredName = "wife",
                    LastName = "spouse",
                    LoggedInUser = false,
                    Email = "wife@spouse.net",
                    RelationshipId = 1,
                    Age = 30
                }
            };
        }
    }
}