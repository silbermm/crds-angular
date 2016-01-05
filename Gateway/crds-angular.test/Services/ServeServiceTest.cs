using System;
using System.Collections.Generic;
using crds_angular.App_Start;
using crds_angular.Models;
using crds_angular.Models.Crossroads.Profile;
using crds_angular.Models.Crossroads.Serve;
using crds_angular.Services;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Extensions;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Models;
using MinistryPlatform.Translation.Services.Interfaces;
using Moq;
using NUnit.Framework;
using IGroupService = MinistryPlatform.Translation.Services.Interfaces.IGroupService;
using IEventService = MinistryPlatform.Translation.Services.Interfaces.IEventService;
using Participant = MinistryPlatform.Models.Participant;

namespace crds_angular.test.Services
{
    [TestFixture]
    public class ServeServiceTest
    {
        private Mock<IContactRelationshipService> _contactRelationshipService;
        private Mock<IContactService> _contactService;
        private Mock<IOpportunityService> _opportunityService;
        private Mock<IAuthenticationService> _authenticationService;
        private Mock<IPersonService> _personService;
        private Mock<IServeService> _serveService;
        private Mock<IEventService> _eventService;
        private Mock<IParticipantService> _participantService;
        private Mock<IGroupParticipantService> _groupParticipantService;
        private Mock<IGroupService> _groupService;
        private Mock<ICommunicationService> _communicationService;
        private Mock<IConfigurationWrapper> _configurationWrapper;
        private Mock<IApiUserService> _apiUserService;
        private Mock<IResponseService> _responseService;

        private ServeService _fixture;

        private MessageTemplate mockRsvpChangedTemplate = new MessageTemplate
        {
            Body =
                "This message is to confirm that you have changed your rsvp from [Previous_Opportunity_Name] to [Opportunity_Name]." +
                "If you have any questions, contact [Group_Contact] by replying to this email." +
                "int.crossroads.net/serve-signup",
            Subject = "Serving RSVP Confirmation"
        };

        private MessageTemplate mockRsvpNoTemplate = new MessageTemplate
        {
            Body =
                "Thank you for notifying us that you cannot serve with [Group_Name] from [Start_Date] to [End_Date]." +
                "If you have any questions, contact [Group_Contact] by replying to this email." +
                "int.crossroads.net/serve-signup",
            Subject = "Serving RSVP Confirmation"
        };

        private MessageTemplate mockRsvpYesTemplate = new MessageTemplate
        {
            Body = "Thank you for signing up to serve with [Opportunity_Name] from [Start_Date] to [End_Date]!" +
                   "On the day you are serving, please report to [Room] at [Shift_Start] and plan on staying until [Shift_End]." +
                   "If you have any questions, contact [Group_Contact] by replying to this email." +
                   "int.crossroads.net/serve-signup",
            Subject = "Serving RSVP Confirmation"
        };

        private readonly int rsvpYesId = 11366;
        private readonly int rsvpNoId = 11299;
        private readonly int rsvpChangeId = 11366;

        private Opportunity fakeOpportunity = new Opportunity();
        private MyContact fakeGroupContact = new MyContact();
        private MyContact fakeMyContact = new MyContact();

        [SetUp]
        public void SetUp()
        {
            _contactRelationshipService = new Mock<IContactRelationshipService>();
            _contactService = new Mock<IContactService>();
            _opportunityService = new Mock<IOpportunityService>();
            _authenticationService = new Mock<IAuthenticationService>();
            _personService = new Mock<crds_angular.Services.Interfaces.IPersonService>();
            _eventService = new Mock<IEventService>();
            _serveService = new Mock<IServeService>();
            _participantService = new Mock<IParticipantService>();
            _groupParticipantService = new Mock<IGroupParticipantService>();
            _groupService = new Mock<IGroupService>();
            _communicationService = new Mock<ICommunicationService>();
            _configurationWrapper = new Mock<IConfigurationWrapper>();
            _apiUserService = new Mock<IApiUserService>();
            _responseService = new Mock<IResponseService>();

            fakeOpportunity.EventTypeId = 3;
            fakeOpportunity.GroupContactId = 23;
            fakeOpportunity.GroupContactName = "Harold";
            fakeOpportunity.GroupName = "Mighty Ducks";
            fakeOpportunity.OpportunityId = 12;
            fakeOpportunity.OpportunityName = "Goalie";

            fakeGroupContact.Contact_ID = 23;
            fakeGroupContact.Email_Address = "fakeEmail@fake.com";
            fakeGroupContact.Nickname = "fakeNick";
            fakeGroupContact.Last_Name = "Name";

            fakeMyContact.Contact_ID = 8;
            fakeMyContact.Email_Address = "fakeUser@fake.com";

            _contactService.Setup(m => m.GetContactById(1)).Returns(fakeGroupContact);
            _contactService.Setup(m => m.GetContactById(fakeOpportunity.GroupContactId)).Returns(fakeGroupContact);
            _contactService.Setup(m => m.GetContactById(8)).Returns(fakeMyContact);

            _communicationService.Setup(m => m.GetTemplate(rsvpYesId)).Returns(mockRsvpYesTemplate);
            _communicationService.Setup(m => m.GetTemplate(rsvpNoId)).Returns(mockRsvpNoTemplate);
            _communicationService.Setup(m => m.GetTemplate(rsvpChangeId)).Returns(mockRsvpChangedTemplate);


            _authenticationService.Setup(mocked => mocked.GetContactId(It.IsAny<string>())).Returns(123456);
            var myContact = new MyContact
            {
                Contact_ID = 123456,
                Email_Address = "contact@email.com",
                Last_Name = "last-name",
                Nickname = "nickname",
                First_Name = "first-name",
                Middle_Name = "middle-name",
                Maiden_Name = "maiden-name",
                Mobile_Phone = "mobile-phone",
                Mobile_Carrier = 999,
                Date_Of_Birth = "date-of-birth",
                Marital_Status_ID = 5,
                Gender_ID = 2,
                Employer_Name = "employer-name",
                Address_Line_1 = "address-line-1",
                Address_Line_2 = "address-line-2",
                City = "city",
                State = "state",
                Postal_Code = "postal-code",                
                Foreign_Country = "foreign-country",
                Home_Phone = "home-phone",
                Congregation_ID = 8,
                Household_ID = 7,
                Address_ID = 6
            };
            _contactService.Setup(mocked => mocked.GetMyProfile(It.IsAny<string>())).Returns(myContact);

            var person = new Person();
            person.ContactId = myContact.Contact_ID;
            person.EmailAddress = myContact.Email_Address;
            person.LastName = myContact.Last_Name;
            person.NickName = myContact.Nickname;

            _personService.Setup(m => m.GetLoggedInUserProfile(It.IsAny<string>())).Returns(person);

            _fixture = new ServeService(_contactService.Object, _contactRelationshipService.Object,
                _opportunityService.Object, _eventService.Object,
                _participantService.Object, _groupParticipantService.Object, _groupService.Object,
                _communicationService.Object, _authenticationService.Object, _configurationWrapper.Object, _apiUserService.Object, _responseService.Object);

            //force AutoMapper to register
            AutoMapperConfig.RegisterMappings();
        }

        [Test]
        public void ShouldSendReminderEmails()
        {
            const int pageId = 2203;
            const string apiToken = "1234";
            const int defaultEmailTemplate = 14567;

            var now = DateTime.Now;

            var fakeServeReminder = new ServeReminder()
            {
                OpportunityTitle = "Some Title",
                EventEndDate = now,
                EventStartDate = now,
                EventTitle = "Whatever",
                OpportunityContactId = fakeGroupContact.Contact_ID,
                OpportunityEmailAddress = fakeGroupContact.Email_Address,
                ShiftEnd = new TimeSpan(0, 7, 0, 0),
                ShiftStart = new TimeSpan(0, 9, 0, 0),
                SignedupContactId = fakeMyContact.Contact_ID,
                SignedupEmailAddress = fakeMyContact.Email_Address
            };

            var fakePageView = new MPServeReminders()
            {
                Opportunity_Title = fakeServeReminder.OpportunityTitle,               
                Opportunity_Contact_Id = fakeServeReminder.OpportunityContactId,
                Opportunity_Email_Address = fakeServeReminder.OpportunityEmailAddress,
                Event_End_Date = now,
                Event_Start_Date = now,
                Event_Title = fakeServeReminder.EventTitle,
                Signedup_Contact_Id = fakeMyContact.Contact_ID,
                Signedup_Email_Address = fakeMyContact.Email_Address,
                Template_Id = null,
                Shift_Start = fakeServeReminder.ShiftStart,
                Shift_End = fakeServeReminder.ShiftEnd
            };

            var fakeList = new List<MPServeReminders> ()
            {
                fakePageView
            };

            const int defaultContactEmailId = 1519180;
            

            var token = _apiUserService.Setup(m => m.GetToken()).Returns(apiToken);
            _responseService.Setup(m => m.GetServeReminders(apiToken)).Returns(fakeList);
            _contactService.Setup(m => m.GetContactById(defaultContactEmailId)).Returns(fakeGroupContact);

            fakeList.ForEach(f =>
            {
                var mergeData = new Dictionary<string, object>(){
                    {"Opportunity_Title", fakeServeReminder.OpportunityTitle},
                    {"Nickname", fakeMyContact.Nickname},
                    {"Event_Start_Date", fakeServeReminder.EventStartDate.ToShortDateString()},
                    {"Event_End_Date", fakeServeReminder.EventEndDate.ToShortDateString()},
                    {"Shift_Start", fakeServeReminder.ShiftStart},
                    {"Shift_End", fakeServeReminder.ShiftEnd}
                 };

                var contact = new Contact() {ContactId = fakeGroupContact.Contact_ID, EmailAddress = fakeGroupContact.Email_Address};
                var toContact = new Contact() {ContactId = fakeMyContact.Contact_ID, EmailAddress = fakeMyContact.Email_Address};
                var fakeCommunication = new Communication()
                {
                    AuthorUserId = fakeGroupContact.Contact_ID,
                    DomainId = 1,
                    EmailBody = "Some Email Body",
                    EmailSubject = "Whatever",
                    FromContact = contact,
                    MergeData = mergeData,
                    ReplyToContact = contact,
                    TemplateId = defaultEmailTemplate,
                    ToContacts = new List<Contact>() {toContact}
                };

                _contactService.Setup(m => m.GetContactById(fakeServeReminder.SignedupContactId)).Returns(fakeMyContact);
                _communicationService.Setup(m => m.GetTemplateAsCommunication(defaultEmailTemplate,
                                                                              fakeGroupContact.Contact_ID,
                                                                              fakeGroupContact.Email_Address,
                                                                              fakeServeReminder.OpportunityContactId,
                                                                              fakeServeReminder.OpportunityEmailAddress,
                                                                              fakeMyContact.Contact_ID,
                                                                              fakeMyContact.Email_Address,
                                                                              mergeData)).Returns(fakeCommunication);
                _communicationService.Setup(m => m.SendMessage(fakeCommunication));
                _communicationService.Verify();

            });
            _responseService.Verify();
        }

        [Test]
        public void GetMyFamiliesServingEventsTest()
        {
            var contactId = 123456;

            _contactRelationshipService.Setup(m => m.GetMyImmediateFamilyRelationships(contactId, It.IsAny<string>()))
                .Returns(MockContactRelationships());

            _participantService.Setup(m => m.GetParticipant(It.IsAny<int>()))
                .Returns(new Participant {ParticipantId = 1});

            _groupParticipantService.Setup(g => g.GetServingParticipants(It.IsAny<List<int>>(), It.IsAny<long>(), It.IsAny<long>(), contactId)).Returns(MockGroupServingParticipants());

            var servingDays = _fixture.GetServingDays(It.IsAny<string>(), contactId, It.IsAny<long>(), It.IsAny<long>());
            _contactRelationshipService.VerifyAll();
            _groupParticipantService.Verify();
            _serveService.VerifyAll();
            _participantService.VerifyAll();

            Assert.IsNotNull(servingDays);
            Assert.AreEqual(2, servingDays.Count);
            var servingDay = servingDays[0];
            Assert.AreEqual(2, servingDay.ServeTimes.Count);

            var servingTime = servingDay.ServeTimes[0];
            Assert.AreEqual(1, servingTime.ServingTeams.Count);

            servingTime = servingDay.ServeTimes[1];
            Assert.AreEqual(1, servingTime.ServingTeams.Count);
        }

        private static List<GroupServingParticipant> MockGroupServingParticipants()
        {
            var startDate = DateTime.Today;
            var servingParticipants = new List<GroupServingParticipant>
            {
                new GroupServingParticipant
                {
                    ContactId = 2,
                    DomainId = 1,
                    EventId = 3,
                    EventStartDateTime = startDate,
                    EventTitle = "Serving Event",
                    EventType = "Event Type",
                    EventTypeId = 4,
                    GroupId = 5,
                    GroupName = "Group",
                    GroupPrimaryContactEmail = "group@leader.com",
                    GroupRoleId = 6,
                    OpportunityId = 7,
                    OpportunityMaximumNeeded = 10,
                    OpportunityMinimumNeeded = 5,
                    OpportunityRoleTitle = "Member",
                    OpportunityShiftEnd = TimeSpan.Parse("8:30"),
                    OpportunityShiftStart = TimeSpan.Parse("10:30"),
                    OpportunitySignUpDeadline = 7,
                    OpportunityTitle = "Serving",
                    ParticipantEmail = "partici@pants.com",
                    ParticipantId = 8,
                    ParticipantLastName = "McServer",
                    ParticipantNickname = "Servy",
                    Rsvp = true
                },
                new GroupServingParticipant
                {
                    ContactId = 2,
                    DomainId = 1,
                    EventId = 3,
                    EventStartDateTime = startDate.AddHours(4),
                    EventTitle = "Serving Event",
                    EventType = "Event Type",
                    EventTypeId = 4,
                    GroupId = 5,
                    GroupName = "Group",
                    GroupPrimaryContactEmail = "group@leader.com",
                    GroupRoleId = 6,
                    OpportunityId = 7,
                    OpportunityMaximumNeeded = 10,
                    OpportunityMinimumNeeded = 5,
                    OpportunityRoleTitle = "Member",
                    OpportunityShiftEnd = TimeSpan.Parse("8:30"),
                    OpportunityShiftStart = TimeSpan.Parse("10:30"),
                    OpportunitySignUpDeadline = 7,
                    OpportunityTitle = "Serving",
                    ParticipantEmail = "partici@pants.com",
                    ParticipantId = 8,
                    ParticipantLastName = "McServer",
                    ParticipantNickname = "Servy",
                    Rsvp = true
                },
                new GroupServingParticipant
                {
                    ContactId = 2,
                    DomainId = 1,
                    EventId = 3,
                    EventStartDateTime = startDate.AddDays(1),
                    EventTitle = "Serving Event",
                    EventType = "Event Type",
                    EventTypeId = 4,
                    GroupId = 5,
                    GroupName = "Group",
                    GroupPrimaryContactEmail = "group@leader.com",
                    GroupRoleId = 6,
                    OpportunityId = 7,
                    OpportunityMaximumNeeded = 10,
                    OpportunityMinimumNeeded = 5,
                    OpportunityRoleTitle = "Member",
                    OpportunityShiftEnd = TimeSpan.Parse("8:30"),
                    OpportunityShiftStart = TimeSpan.Parse("10:30"),
                    OpportunitySignUpDeadline = 7,
                    OpportunityTitle = "Serving",
                    ParticipantEmail = "partici@pants.com",
                    ParticipantId = 8,
                    ParticipantLastName = "McServer",
                    ParticipantNickname = "Servy",
                    Rsvp = true
                },
                new GroupServingParticipant
                {
                    ContactId = 2,
                    DomainId = 1,
                    EventId = 3,
                    EventStartDateTime = startDate.AddDays(1).AddHours(4),
                    EventTitle = "Serving Event",
                    EventType = "Event Type",
                    EventTypeId = 4,
                    GroupId = 5,
                    GroupName = "Group",
                    GroupPrimaryContactEmail = "group@leader.com",
                    GroupRoleId = 6,
                    OpportunityId = 7,
                    OpportunityMaximumNeeded = 10,
                    OpportunityMinimumNeeded = 5,
                    OpportunityRoleTitle = "Member",
                    OpportunityShiftEnd = TimeSpan.Parse("8:30"),
                    OpportunityShiftStart = TimeSpan.Parse("10:30"),
                    OpportunitySignUpDeadline = 7,
                    OpportunityTitle = "Serving",
                    ParticipantEmail = "partici@pants.com",
                    ParticipantId = 8,
                    ParticipantLastName = "McServer",
                    ParticipantNickname = "Servy",
                    Rsvp = true
                }
            };
            return servingParticipants;
        }

        private static List<ContactRelationship> MockContactRelationships()
        {
            var mockRelationships = new List<ContactRelationship>();
            var mockRelationship1 = new ContactRelationship();
            mockRelationship1.Contact_Id = 1111111;
            mockRelationship1.Participant_Id = 1;
            var mockRelationship2 = new ContactRelationship();
            mockRelationship2.Contact_Id = 123456;
            mockRelationship2.Participant_Id = 2;
            mockRelationships.Add(mockRelationship1);
            mockRelationships.Add(mockRelationship2);
            return mockRelationships;
        }

        [Test, TestCaseSource("OpportunityCapacityCases")]
        public void OpportunityCapacityHasMinHasMax(int? min, int? max, List<Response> mockResponses,
            Capacity expectedCapacity)
        {
            const int opportunityId = 9999;
            const int eventId = 1000;

            var opportunity = new Opportunity();
            opportunity.MaximumNeeded = max;
            opportunity.MinimumNeeded = min;
            opportunity.OpportunityId = opportunityId;
            opportunity.Responses = mockResponses;

            _opportunityService.Setup(m => m.GetOpportunityResponses(opportunityId, It.IsAny<string>()))
                .Returns(opportunity.Responses);

            var capacity = _fixture.OpportunityCapacity(opportunityId, eventId, min, max);

            Assert.IsNotNull(capacity);
            Assert.AreEqual(capacity.Available, expectedCapacity.Available);
            Assert.AreEqual(capacity.BadgeType, expectedCapacity.BadgeType);
            Assert.AreEqual(capacity.Display, expectedCapacity.Display);
            Assert.AreEqual(capacity.Maximum, expectedCapacity.Maximum);
            Assert.AreEqual(capacity.Message, expectedCapacity.Message);
            Assert.AreEqual(capacity.Minimum, expectedCapacity.Minimum);
            Assert.AreEqual(capacity.Taken, expectedCapacity.Taken);
        }

        private static readonly object[] OpportunityCapacityCases =
        {
            new object[]
            {
                10, 20, new List<Response>(),
                new Capacity
                {
                    Available = 10,
                    BadgeType = "label-info",
                    Display = true,
                    Maximum = 20,
                    Message = "10 Needed",
                    Minimum = 10,
                    Taken = 0
                }
            },
            new object[]
            {
                10, null, new List<Response>(),
                new Capacity
                {
                    Available = 10,
                    BadgeType = "label-info",
                    Display = true,
                    Maximum = 10,
                    Message = "10 Needed",
                    Minimum = 10,
                    Taken = 0
                }
            },
            new object[]
            {
                null, 20, new List<Response>(),
                new Capacity
                {
                    Available = 20,
                    BadgeType = "label-info",
                    Display = true,
                    Maximum = 20,
                    Message = "20 Needed",
                    Minimum = 20,
                    Taken = 0
                }
            },
            new object[]
            {
                10, 20, MockFifteenResponses(),
                new Capacity
                {
                    Display = true,
                    Maximum = 20,
                    Minimum = 10,
                }
            },
            new object[]
            {
                10, 20, MockTwentyResponses(),
                new Capacity
                {
                    Available = -10,
                    BadgeType = "label-default",
                    Display = true,
                    Maximum = 20,
                    Message = "Full",
                    Minimum = 10,
                    Taken = 20
                }
            }
        };

        [Test]
        public void OpportunityCapacityMinAndMaxNull()
        {
            const int opportunityId = 9999;
            const int eventId = 1000;

            var opportunity = new Opportunity();
            opportunity.MaximumNeeded = null;
            opportunity.MinimumNeeded = null;
            opportunity.OpportunityId = opportunityId;
            opportunity.Responses = new List<Response>();

            _opportunityService.Setup(m => m.GetOpportunityResponses(opportunityId, It.IsAny<string>()))
                .Returns(opportunity.Responses);

            var capacity = _fixture.OpportunityCapacity(opportunityId, eventId, opportunity.MinimumNeeded,
                opportunity.MaximumNeeded);

            Assert.IsNotNull(capacity);
            Assert.AreEqual(capacity.Display, false);
        }

        [Test]
        public void ChangeResponseFromNoToYes()
        {
            int contactId = fakeMyContact.Contact_ID;
            int opportunityId = fakeOpportunity.OpportunityId;
            int eventTypeId = fakeOpportunity.EventTypeId;
            const bool signUp = true;
            const bool alternateWeeks = false;
            var oppIds = new List<int>() {1, 2, 3, 4, 5};

            SetUpRSVPMocks(contactId, eventTypeId, opportunityId, signUp, SetupMockEvents());

            // The current Opportunity
            _opportunityService.Setup(m => m.GetOpportunityById(opportunityId, It.IsAny<string>()))
                .Returns(fakeOpportunity);
            _opportunityService.Setup(m => m.DeleteResponseToOpportunities(47, 1, 1)).Returns(1);

            // The previous Opportunity
            _opportunityService.Setup(m => m.GetOpportunityById(1, It.IsAny<string>())).Returns(new Opportunity()
            {
                OpportunityId = 1,
                OpportunityName = "Previous Opportunity"
            });

            _configurationWrapper.Setup(m => m.GetConfigIntValue("DefaultContactEmailId")).Returns(1234);
            _contactService.Setup(m => m.GetContactById(1234)).Returns(new MyContact()
            {
                Email_Address = "gmail@google.com",
                Contact_ID = 1234567890
            });
            
            SaveRsvpDto dto = new SaveRsvpDto
            {
                ContactId = contactId,
                OpportunityId = opportunityId,
                OpportunityIds = oppIds,
                EventTypeId = eventTypeId,
                AlternateWeeks = alternateWeeks,
                StartDateUnix = new DateTime(2015, 1, 1).ToUnixTime(),
                EndDateUnix = new DateTime(2016, 1, 1).ToUnixTime(),
                SignUp = signUp
            };

            _fixture.SaveServeRsvp("1234567", dto);

            _participantService.VerifyAll();
            _eventService.Verify(
                m =>
                    m.GetEventsByTypeForRange(eventTypeId, It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                        It.IsAny<string>()), Times.Exactly(1));
            _eventService.Verify(m => m.RegisterParticipantForEvent(47, It.IsAny<int>(), 0, 0), Times.Exactly(5));
            _opportunityService.Verify(
                (m => m.RespondToOpportunity(47, opportunityId, It.IsAny<string>(), It.IsAny<int>(), signUp)),
                Times.Exactly(5));

            _communicationService.Verify(m => m.GetTemplate(rsvpChangeId));

            var comm = new Communication
            {
                AuthorUserId = 5,
                DomainId = 1,
                EmailBody = mockRsvpChangedTemplate.Body,
                EmailSubject = mockRsvpChangedTemplate.Subject,
                FromContact = new Contact {ContactId = fakeGroupContact.Contact_ID, EmailAddress = fakeGroupContact.Email_Address},
                ReplyToContact = new Contact { ContactId = fakeGroupContact.Contact_ID, EmailAddress = fakeGroupContact.Email_Address },
                ToContacts = new List<Contact> {new Contact{ContactId = fakeGroupContact.Contact_ID, EmailAddress = fakeMyContact.Email_Address}}
            };

            var mergeData = new Dictionary<string, object>
            {
                {"Opportunity_Name", It.IsAny<string>()},
                {"Start_Date", It.IsAny<string>()},
                {"End_Date", It.IsAny<string>()},
                {"Shift_Start", It.IsAny<string>()},
                {"Shift_End", It.IsAny<string>()},
                {"Room", It.IsAny<string>()},
                {"Group_Contact", It.IsAny<string>()},
                {"Group_Name", It.IsAny<string>()},
                {"Volunteer_Name", It.IsAny<string>()},
                {"Previous_Opportunity_Name", It.IsAny<string>()}
            };

            _communicationService.Setup(
                m => m.SendMessage(It.IsAny<Communication>()))
                .Callback((Communication communication) => { }).Verifiable();
            _communicationService.Verify(
                m => m.SendMessage(It.IsAny<Communication>()));
        }

        [Test]
        public void RespondToServeOpportunityYesEveryWeek()
        {
            const int contactId = 8;
            const int opportunityId = 12;
            const int eventTypeId = 3;
            const bool signUp = true;
            const bool alternateWeeks = false;
            var oppIds = new List<int>() {1, 2, 3, 4, 5};

            SetUpRSVPMocks(contactId, eventTypeId, opportunityId, signUp, SetupMockEvents());

            _configurationWrapper.Setup(m => m.GetConfigIntValue("DefaultContactEmailId")).Returns(1234);
            _contactService.Setup(m => m.GetContactById(1234)).Returns(new MyContact()
            {
                Email_Address = "gmail@google.com",
                Contact_ID = 1234567890
            });

            // The current Opportunity
            _opportunityService.Setup(m => m.GetOpportunityById(opportunityId, It.IsAny<string>()))
                .Returns(fakeOpportunity);

            SaveRsvpDto dto = new SaveRsvpDto
            {
                ContactId = contactId,
                OpportunityId = opportunityId,
                OpportunityIds = oppIds,
                EventTypeId = eventTypeId,
                AlternateWeeks = alternateWeeks,
                StartDateUnix = new DateTime(2015, 1, 1).ToUnixTime(),
                EndDateUnix = new DateTime(2016, 1, 1).ToUnixTime(),
                SignUp = signUp
            };

            _fixture.SaveServeRsvp("1234567", dto);

            _participantService.VerifyAll();
            _eventService.Verify(
                m =>
                    m.GetEventsByTypeForRange(eventTypeId, It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                        It.IsAny<string>()), Times.Exactly(1));
            _eventService.Verify(m => m.RegisterParticipantForEvent(47, It.IsAny<int>(), 0, 0), Times.Exactly(5));
            _opportunityService.Verify(
                (m => m.RespondToOpportunity(47, opportunityId, It.IsAny<string>(), It.IsAny<int>(), signUp)),
                Times.Exactly(5));

            Opportunity o = new Opportunity();
            o.OpportunityName = "Whatever";
            o.OpportunityId = opportunityId;
            o.ShiftStart = new TimeSpan();
            o.ShiftEnd = new TimeSpan();
            o.Room = "123";

            _opportunityService.Setup(m => m.GetOpportunityById(opportunityId, It.IsAny<string>())).Returns(o);
        }

        [Test, TestCaseSource("AllMockEvents")]
        public void RespondToServeOpportunityYesForEveryOtherWeek(List<Event> mockEvents)
        {
            const int contactId = 8;
            const int opportunityId = 12;
            const int eventTypeId = 3;
            const bool signUp = true;
            const bool alternateWeeks = true;
            var expectedEventIds = new List<int> {1, 3, 5};
            var oppIds = new List<int>() {1, 2, 3, 4, 5};

            SetUpRSVPMocks(contactId, eventTypeId, opportunityId, signUp, mockEvents);

            // The current Opportunity
            _opportunityService.Setup(m => m.GetOpportunityById(opportunityId, It.IsAny<string>()))
                .Returns(fakeOpportunity);

            _opportunityService.Setup(m => m.GetOpportunityById(1, It.IsAny<string>())).Returns(new Opportunity()
            {
                OpportunityId = 1,
                OpportunityName = "Previous Opportunity",
                GroupContactId = fakeOpportunity.GroupContactId
            });

            _configurationWrapper.Setup(m => m.GetConfigIntValue("DefaultContactEmailId")).Returns(1234);
            _contactService.Setup(m => m.GetContactById(1234)).Returns(new MyContact()
            {
                Email_Address = "gmail@google.com",
                Contact_ID = 1234567890
            });

            SaveRsvpDto dto = new SaveRsvpDto
            {
                ContactId = contactId,
                OpportunityId = opportunityId,
                OpportunityIds = oppIds,
                EventTypeId = eventTypeId,
                AlternateWeeks = alternateWeeks,
                StartDateUnix = new DateTime(2015, 1, 1).ToUnixTime(),
                EndDateUnix = new DateTime(2016, 1, 1).ToUnixTime(),
                SignUp = signUp
            };

            _fixture.SaveServeRsvp(It.IsAny<string>(), dto);

            // The current Opportunity
            _opportunityService.Setup(m => m.GetOpportunityById(opportunityId, It.IsAny<string>()))
                .Returns(fakeOpportunity);

            _participantService.VerifyAll();
            _eventService.Verify(
                m =>
                    m.GetEventsByTypeForRange(eventTypeId, It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                        It.IsAny<string>()), Times.Exactly(1));

            _eventService.Verify(m => m.RegisterParticipantForEvent(47, It.IsIn<int>(expectedEventIds), 0, 0),
                Times.Exactly(3));

            _opportunityService.Verify(
                (m =>
                    m.RespondToOpportunity(47, opportunityId, It.IsAny<string>(), It.IsIn<int>(expectedEventIds), signUp)),
                Times.Exactly(3));
        }

        private static readonly object[] AllMockEvents =
        {
            new[] {SetupMockEvents()},
            new[] {SetupWeekMissingInMySeriesMockEvents()},
            new[] {SetupWeekMissingNotInMySeriesMockEvents()},
            new[] {SetupWeekMutipleMissingInMySeriesMockEvents()},
            new[] {SetupWeekNotInSequentialOrderMockEvents()}
        };

        private static List<Event> SetupMockEvents()
        {
            return new List<Event>
            {
                new Event
                {
                    EventId = 1,
                    EventStartDate = new DateTime(2015, 1, 1)
                },
                new Event
                {
                    EventId = 2,
                    EventStartDate = new DateTime(2015, 1, 8)
                },
                new Event
                {
                    EventId = 3,
                    EventStartDate = new DateTime(2015, 1, 15)
                },
                new Event
                {
                    EventId = 4,
                    EventStartDate = new DateTime(2015, 1, 22)
                },
                new Event
                {
                    EventId = 5,
                    EventStartDate = new DateTime(2015, 1, 29)
                }
            };
        }

        private static List<Event> SetupWeekMissingInMySeriesMockEvents()
        {
            return new List<Event>
            {
                new Event
                {
                    EventId = 1,
                    EventStartDate = new DateTime(2015, 1, 1)
                },
                new Event
                {
                    EventId = 2,
                    EventStartDate = new DateTime(2015, 1, 8)
                },
                new Event
                {
                    EventId = 4,
                    EventStartDate = new DateTime(2015, 1, 22)
                },
                new Event
                {
                    EventId = 3,
                    EventStartDate = new DateTime(2015, 1, 29)
                },
                new Event
                {
                    EventId = 6,
                    EventStartDate = new DateTime(2015, 2, 5)
                },
                new Event
                {
                    EventId = 5,
                    EventStartDate = new DateTime(2015, 2, 12)
                }
            };
        }

        private static List<Event> SetupWeekMissingNotInMySeriesMockEvents()
        {
            return new List<Event>
            {
                new Event
                {
                    EventId = 1,
                    EventStartDate = new DateTime(2015, 1, 1)
                },
                new Event
                {
                    EventId = 3,
                    EventStartDate = new DateTime(2015, 1, 15)
                },
                new Event
                {
                    EventId = 4,
                    EventStartDate = new DateTime(2015, 1, 22)
                },
                new Event
                {
                    EventId = 5,
                    EventStartDate = new DateTime(2015, 1, 29)
                }
            };
        }

        private static List<Event> SetupWeekMutipleMissingInMySeriesMockEvents()
        {
            return new List<Event>
            {
                new Event
                {
                    EventId = 1,
                    EventStartDate = new DateTime(2015, 1, 1)
                },
                new Event
                {
                    EventId = 3,
                    EventStartDate = new DateTime(2015, 1, 15)
                },
                new Event
                {
                    EventId = 4,
                    EventStartDate = new DateTime(2015, 1, 22)
                },
                new Event
                {
                    EventId = 6,
                    EventStartDate = new DateTime(2015, 2, 5)
                },
                new Event
                {
                    EventId = 5,
                    EventStartDate = new DateTime(2015, 2, 12)
                }
            };
        }

        private static List<Event> SetupWeekNotInSequentialOrderMockEvents()
        {
            return new List<Event>
            {
                new Event
                {
                    EventId = 2,
                    EventStartDate = new DateTime(2015, 1, 8)
                },
                new Event
                {
                    EventId = 5,
                    EventStartDate = new DateTime(2015, 1, 29)
                },
                new Event
                {
                    EventId = 4,
                    EventStartDate = new DateTime(2015, 1, 22)
                },
                new Event
                {
                    EventId = 1,
                    EventStartDate = new DateTime(2015, 1, 1)
                },
                new Event
                {
                    EventId = 3,
                    EventStartDate = new DateTime(2015, 1, 15)
                }
            };
        }

        private void SetUpRSVPMocks(int contactId, int eventTypeId, int opportunityId, bool signUp,
            List<Event> mockEvents)
        {
            var mockParticipant = new Participant
            {
                ParticipantId = 47
            };
            
            //mock it up
            _participantService.Setup(m => m.GetParticipant(contactId)).Returns(mockParticipant);
            _eventService.Setup(
                m =>
                    m.GetEventsByTypeForRange(eventTypeId, It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                        It.IsAny<string>())).Returns(mockEvents);

            foreach (var mockEvent in mockEvents)
            {
                _eventService.Setup(m => m.RegisterParticipantForEvent(mockParticipant.ParticipantId, mockEvent.EventId, 0, 0));
                _opportunityService.Setup(
                    m =>
                        m.RespondToOpportunity(mockParticipant.ParticipantId, opportunityId, It.IsAny<string>(),
                            mockEvent.EventId, signUp));
            }
        }
        
        private static List<Response> MockTwentyResponses()
        {
            var responses = new List<Response>();
            for (var i = 0; i < 20; i++)
            {
                responses.Add(new Response {Event_ID = 1000});
            }
            return responses;
        }

        private static List<Response> MockFifteenResponses()
        {
            var responses = new List<Response>();
            for (var i = 0; i < 15; i++)
            {
                responses.Add(new Response {Event_ID = 1000});
            }
            return responses;
        }
    }
}
