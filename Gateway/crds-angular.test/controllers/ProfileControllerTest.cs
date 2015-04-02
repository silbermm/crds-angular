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

        [Test]
        public void GetFamilyServeSignUpTest()
        {
            const int contactId = 123456;
            var servingTeams = SetUpServingTeams();
            var servingDays = SetUpServingDays();

            _personServiceMock.Setup(mocked => mocked.GetServingTeams( It.IsAny<string>()))
                .Returns(servingTeams);
            _personServiceMock.Setup(mocked => mocked.GetServingDays( It.IsAny<string>()))
                .Returns(servingDays);

            IHttpActionResult result = _fixture.GetFamilyServeDays();

            Assert.NotNull(result);
            Assert.IsInstanceOf<OkNegotiatedContentResult<List<ServingDay>>>(result);
        }

        private List<ServingDay> SetUpServingDays()
        {
            var servingTeams = SetUpServingTeams();
            var servingDays = new List<ServingDay>
            {
                new ServingDay
                {
                    Day = "abc",
                    ServeTimes =
                    {
                        new ServingTime {Time = "08:30:00", ServingTeams = servingTeams},
                        new ServingTime {Time = "10:00:00", ServingTeams = servingTeams}
                    }
                }
            };

            return servingDays;
        }

        private List<ServingTeam> SetUpServingTeams()
        {
            var servingTeams = new List<ServingTeam>
            {
                new ServingTeam
                {
                    GroupId = 1,
                    Name = "team-1",
                    PrimaryContact = "me@example.com",
                    Members = new List<TeamMember>
                    {
                        new TeamMember
                        {
                            ContactId = 1,
                            Name = "memeber-1",
                            Roles =
                                new List<ServeRole>
                                {
                                    new ServeRole {Capacity = 100, Name = "Leader", SlotsTaken = 90},
                                    new ServeRole {Capacity = 50, Name = "Member", SlotsTaken = 1}
                                }
                        },
                        new TeamMember
                        {
                            ContactId = 2,
                            Name = "memeber-2",
                            Roles =
                                new List<ServeRole>
                                {
                                    new ServeRole {Capacity = 25, Name = "Admin", SlotsTaken = 0},
                                    new ServeRole {Capacity = 50, Name = "Member", SlotsTaken = 1}
                                }
                        }
                    }
                },
                new ServingTeam
                {
                    GroupId = 2,
                    Name = "team-2",
                    PrimaryContact = "me2@aol.com",
                    Members = new List<TeamMember>
                    {
                        new TeamMember
                        {
                            ContactId = 3,
                            Name = "memeber-3",
                            Roles =
                                new List<ServeRole>
                                {
                                    new ServeRole {Capacity = 100, Name = "Leader", SlotsTaken = 90},
                                    new ServeRole {Capacity = 50, Name = "Member", SlotsTaken = 1}
                                }
                        },
                        new TeamMember
                        {
                            ContactId = 2,
                            Name = "memeber-2",
                            Roles =
                                new List<ServeRole>
                                {
                                    new ServeRole {Capacity = 25, Name = "Admin", SlotsTaken = 0},
                                    new ServeRole {Capacity = 50, Name = "Member", SlotsTaken = 1}
                                }
                        }
                    }
                }
            };
            return servingTeams;
        }
    }
}