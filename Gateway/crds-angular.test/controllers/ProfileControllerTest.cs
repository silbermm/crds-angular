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
        private Mock<IAuthenticationService> _authenticationService;

        private string authType;
        private string authToken;

        [SetUp]
        public void SetUp()
        {
            _personServiceMock = new Mock<IPersonService>();
            _authenticationService = new Mock<IAuthenticationService>();

            _fixture = new ProfileController(_personServiceMock.Object, _authenticationService.Object);

            authType = "auth_type";
            authToken = "auth_token";
            _fixture.Request = new HttpRequestMessage();
            _fixture.Request.Headers.Authorization = new AuthenticationHeaderValue(authType, authToken);
            _fixture.RequestContext = new HttpRequestContext();
        }

        [Test]
        public void GetFamilyServeSignUpTest()
        {
            const int contactId = 123456;
            var servingTeams = SetUpServingTeams();
            var servingDays = SetUpServingDays();

            _authenticationService.Setup(mocked => mocked.GetContactId(It.IsAny<string>())).Returns(contactId);

            _personServiceMock.Setup(mocked => mocked.GetServingTeams(contactId, It.IsAny<string>()))
                .Returns(servingTeams);
            _personServiceMock.Setup(mocked => mocked.GetServingDays(servingTeams, It.IsAny<string>()))
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