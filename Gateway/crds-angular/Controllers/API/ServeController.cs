using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Description;
using crds_angular.Exceptions.Models;
using crds_angular.Extenstions;
using crds_angular.Models.Crossroads;
using crds_angular.Models.Crossroads.Serve;
using crds_angular.Security;
using crds_angular.Services.Interfaces;
using log4net;
using MinistryPlatform.Translation.Services.Interfaces;
using Newtonsoft.Json;

namespace crds_angular.Controllers.API
{
    public class ServeController : MPAuth
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        //private IPersonService _personService;
        private IServeService _serveService;
        private IAuthenticationService _authenticationService;

        public ServeController(IServeService serveService, IAuthenticationService authenticationService)
        {
            this._serveService = serveService;
            this._authenticationService = authenticationService;
        }

        [ResponseType(typeof (List<ServingDay>))]
        [Route("api/serve/family-serve-days")]
        public IHttpActionResult GetFamilyServeDays()
        {
            return Authorized(token =>
            {
                try
                {
                    //var servingDays = _serveService.GetServingDays(token);
                    var servingDays = MockResponseForFrontEndTesting();
                    if (servingDays == null)
                    {
                        return Unauthorized();
                    }
                    return this.Ok(servingDays);
                }
                catch (Exception e)
                {
                    return this.BadRequest(e.Message);
                }
            });
        }

        [ResponseType(typeof (List<FamilyMember>))]
        [Route("api/serve/family")]
        public IHttpActionResult GetFamily()
        {
            return Authorized(token =>
            {
                var contactId = _authenticationService.GetContactId(token);
                var list = _serveService.GetMyImmediateFamily(contactId, token);
                if (list == null)
                {
                    return Unauthorized();
                }
                return this.Ok(list);
            });
        }

        [Route("api/serve/save-rsvp")]
        public IHttpActionResult SaveRsvp([FromBody] SaveRsvpDto saveRsvp)
        {
            //validate request
            if (saveRsvp.StartDateUnix <= 0)
            {
                var dateError = new ApiErrorDto("StartDate Invalid", new InvalidOperationException("Invalid Date"));
                throw new HttpResponseException(dateError.HttpResponseMessage);
            }

            return Authorized(token =>
            {
                try
                {
                    _serveService.SaveServeRsvp(token, saveRsvp.ContactId, saveRsvp.OpportunityId,
                        saveRsvp.EventTypeId, saveRsvp.StartDateUnix.FromUnixTime(),
                        saveRsvp.EndDateUnix.FromUnixTime(), saveRsvp.SignUp, saveRsvp.AlternateWeeks);
                }
                catch (Exception exception)
                {
                    var apiError = new ApiErrorDto("Save RSVP Failed", exception);
                    throw new HttpResponseException(apiError.HttpResponseMessage);
                }
                return this.Ok();
            });
        }



        //for testing only
        private List<ServingDay> MockResponseForFrontEndTesting()
        {
            var servingTeamsEightThirty = SetUpServingTeamsEightThirty();
            var servingTeamsTen = SetUpServingTeamsTen();
            var servingDays = new List<ServingDay>
            {
                new ServingDay
                {
                    Day = "4/27/2015",
                    Date = new DateTime(2015, 4, 27),
                    ServeTimes =
                    {
                        new ServingTime {Time = "08:30:00", ServingTeams = servingTeamsEightThirty},
                        new ServingTime {Time = "10:00:00", ServingTeams = servingTeamsTen}
                    }
                },
                new ServingDay
                {
                    Day = "4/28/2015",
                    Date = new DateTime(2015, 4, 28),
                    ServeTimes =
                    {
                        new ServingTime {Time = "08:30:00", ServingTeams = servingTeamsEightThirty},
                        new ServingTime {Time = "10:00:00", ServingTeams = servingTeamsTen}
                    }
                }
            };

            return servingDays;
        }



        private List<ServingTeam> SetUpServingTeamsEightThirty()
        {
            var servingTeams = new List<ServingTeam>
            {
                new ServingTeam
                {
                    EventType = "KC Nursery Oakley Sunday 8:30",
                    EventTypeId = 142,
                    GroupId = 6329,
                    Name = "KC Oakley Nursery MP",
                    PrimaryContact = "milacron@crossroads.net",
                    Members = new List<TeamMember>
                    {
                        TeamMember1(),
                        TeamMember2SignedUp(), TeamMember3()
                    }
                }
            };
            return servingTeams;
        }

        private List<ServingTeam> SetUpServingTeamsTen()
        {
            var servingTeams = new List<ServingTeam>
            {
                new ServingTeam
                {
                    EventType = "KC Nursery Oakley Sunday 8:30",
                    EventTypeId = 142,
                    GroupId = 6329,
                    Name = "KC Oakley Nursery MP",
                    PrimaryContact = "milacron@crossroads.net",
                    Members = new List<TeamMember>
                    {
                        TeamMember1(),
                        TeamMember2NotSignedUp(), TeamMember3()
                    }
                }
            };
            return servingTeams;
        }

        private TeamMember TeamMember3()
        {
            return new TeamMember
            {
                ContactId = 3,
                Name = "Brady",
                LastName = "Queenan", ServeRsvp = null,
                Roles =
                    new List<ServeRole>
                    {
                        new ServeRole
                        {
                            Name = "Nursery A - Sunday 8:30 Leader", RoleId = 145,
                            Capacity =
                                new Capacity
                                {
                                    Available = 0,
                                    BadgeType = null,
                                    Display = true,
                                    Maximum = 100,
                                    Message = null,
                                    Minimum = 1, Taken = 0
                                }
                        }
                    }
            };
        }

        private TeamMember TeamMember2SignedUp()
        {
            return new TeamMember
            {
                ContactId = 2,
                Name = "TJ",
                LastName = "Maddox",
                EmailAddress = "tmaddox33+mp@gmail.com", ServeRsvp = new ServeRsvp {RoleId = 145, Attending = true},
                Roles =
                    new List<ServeRole>
                    {
                        new ServeRole
                        {
                            Name = "Nursery A - Sunday 8:30 Leader",
                            RoleId = 145,
                            Capacity =
                                new Capacity
                                {
                                    Available = 0,
                                    BadgeType = null,
                                    Display = true,
                                    Maximum = 100,
                                    Message = null,
                                    Minimum = 1, Taken = 0
                                }
                        },
                        new ServeRole
                        {
                            Name = "Nursery B - Sunday 8:30 Member", RoleId = 106,
                            Capacity =
                                new Capacity
                                {
                                    Available = -1,
                                    BadgeType = "label-default",
                                    Display = true,
                                    Maximum = 0,
                                    Message = "Full",
                                    Minimum = 0, Taken = 1
                                }
                        }
                    }
            };
        }

        private TeamMember TeamMember2NotSignedUp()
        {
            return new TeamMember
            {
                ContactId = 2,
                Name = "TJ",
                LastName = "Maddox",
                EmailAddress = "tmaddox33+mp@gmail.com",
                ServeRsvp = new ServeRsvp{Attending = false, RoleId = 145},
                Roles =
                    new List<ServeRole>
                    {
                        new ServeRole
                        {
                            Name = "Nursery A - Sunday 8:30 Leader",
                            RoleId = 145,
                            Capacity =
                                new Capacity
                                {
                                    Available = 0,
                                    BadgeType = null,
                                    Display = true,
                                    Maximum = 100,
                                    Message = null,
                                    Minimum = 1, Taken = 0
                                }
                        },
                        new ServeRole
                        {
                            Name = "Nursery B - Sunday 8:30 Member", RoleId = 106,
                            Capacity =
                                new Capacity
                                {
                                    Available = -1,
                                    BadgeType = "label-default",
                                    Display = true,
                                    Maximum = 0,
                                    Message = "Full",
                                    Minimum = 0, Taken = 1
                                }
                        }
                    }
            };
        }

        private TeamMember TeamMember1()
        {
            return new TeamMember
            {
                ContactId = 1,
                Name = "Claire",
                LastName = "Maddox",
                EmailAddress = null, ServeRsvp = null,
                Roles =
                    new List<ServeRole>
                    {
                        new ServeRole
                        {
                            Name = "Nursery B - Sunday 8:30 Member", RoleId = 106,
                            Capacity =
                                new Capacity
                                {
                                    Available = 6,
                                    BadgeType = "label-info",
                                    Display = true,
                                    Maximum = 10,
                                    Message = "6 Needed",
                                    Minimum = 10, Taken = 4
                                }
                        },
                        new ServeRole
                        {
                            Name = "Nursery C - Sunday 8:30 Member", RoleId = 108,
                            Capacity =
                                new Capacity
                                {
                                    Available = -1,
                                    BadgeType = "label-default",
                                    Display = true,
                                    Maximum = 0,
                                    Message = "Full",
                                    Minimum = 0, Taken =1
                                }
                        }
                    }
            };
        }
    }
}