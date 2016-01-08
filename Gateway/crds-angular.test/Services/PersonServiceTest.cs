using System;
using System.Collections.Generic;
using crds_angular.App_Start;
using crds_angular.Models.Crossroads.Profile;
using crds_angular.Services;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Models;
using MPInterfaces = MinistryPlatform.Translation.Services.Interfaces;
using Moq;
using NUnit.Framework;

namespace crds_angular.test.Services
{
    internal class PersonServiceTest_opportunityService
    {
        private Mock<IContactAttributeService> _contactAttributeService;
        private Mock<MPInterfaces.IContactService> _contactService;
        private Mock<MPInterfaces.IAuthenticationService> _authenticationService;
        private Mock<MPInterfaces.IApiUserService> _apiUserService;
        private Mock<MPInterfaces.IParticipantService> _participantService;

        private PersonService _fixture;
        private MyContact _myContact;
        private List<HouseholdMember> _householdMembers;

        private readonly DateTime startDate = new DateTime(2015, 2, 21);

        [SetUp]
        public void SetUp()
        {
            _contactAttributeService = new Mock<IContactAttributeService>();
            var contactAllAttributesDto = new ContactAllAttributesDTO();
            _contactAttributeService.Setup(mocked => mocked.GetContactAttributes(It.IsAny<string>(), It.IsAny<int>())).Returns(contactAllAttributesDto);
            _contactService = new Mock<MPInterfaces.IContactService>();
            _authenticationService = new Mock<MPInterfaces.IAuthenticationService>();
            _participantService = new Mock<MPInterfaces.IParticipantService>();

            _apiUserService = new Mock<MPInterfaces.IApiUserService>();
            _apiUserService.Setup(m => m.GetToken()).Returns("something");

            _authenticationService.Setup(mocked => mocked.GetContactId(It.IsAny<string>())).Returns(123456);
            _myContact = new MyContact
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
                Household_Name = "hh name",
                Address_ID = 6,
                Attendance_Start_Date = startDate
            };
            _householdMembers = new List<HouseholdMember>();

            _fixture = new PersonService(_contactService.Object, _contactAttributeService.Object, _apiUserService.Object, _participantService.Object);

            //force AutoMapper to register
            AutoMapperConfig.RegisterMappings();
        }

        [Test]
        public void TestGetProfileForContactId()
        {
            const int contactId = 123456;

            _contactService.Setup(mocked => mocked.GetContactById(contactId)).Returns(_myContact);
            _contactService.Setup(mocked => mocked.GetHouseholdFamilyMembers(7)).Returns(_householdMembers);

            var person = _fixture.GetPerson(contactId);
            _contactService.VerifyAll();

            Assert.IsNotNull(person);

            Assert.AreEqual(123456, person.ContactId);
            Assert.AreEqual("contact@email.com", person.EmailAddress);
            Assert.AreEqual("nickname", person.NickName);
            Assert.AreEqual("first-name", person.FirstName);
            Assert.AreEqual("middle-name", person.MiddleName);
            Assert.AreEqual("last-name", person.LastName);
            Assert.AreEqual("maiden-name", person.MaidenName);
            Assert.AreEqual("mobile-phone", person.MobilePhone);
            Assert.AreEqual(999, person.MobileCarrierId);
            Assert.AreEqual("date-of-birth", person.DateOfBirth);
            Assert.AreEqual(5, person.MaritalStatusId);
            Assert.AreEqual(2, person.GenderId);
            Assert.AreEqual("employer-name", person.EmployerName);
            Assert.AreEqual("address-line-1", person.AddressLine1);
            Assert.AreEqual("address-line-2", person.AddressLine2);
            Assert.AreEqual("city", person.City);
            Assert.AreEqual("state", person.State);
            Assert.AreEqual("postal-code", person.PostalCode);
            Assert.AreEqual(startDate, person.AttendanceStartDate);
            Assert.AreEqual("foreign-country", person.ForeignCountry);
            Assert.AreEqual("home-phone", person.HomePhone);
            Assert.AreEqual(8, person.CongregationId);
            Assert.AreEqual(7, person.HouseholdId);
            Assert.AreEqual("hh name", person.HouseholdName);
            Assert.AreEqual(6, person.AddressId);
            Assert.AreSame(_householdMembers, person.HouseholdMembers);
        }

        [Test]
        public void GetLoggedInUserProfileTest()
        {
            const string token = "some-string";

            _contactService.Setup(mocked => mocked.GetMyProfile(token)).Returns(_myContact);
            _contactService.Setup(mocked => mocked.GetHouseholdFamilyMembers(7)).Returns(_householdMembers);

            var person = _fixture.GetLoggedInUserProfile(token);
            _contactService.VerifyAll();

            Assert.IsNotNull(person);

            Assert.AreEqual(123456, person.ContactId);
            Assert.AreEqual("contact@email.com", person.EmailAddress);
            Assert.AreEqual("nickname", person.NickName);
            Assert.AreEqual("first-name", person.FirstName);
            Assert.AreEqual("middle-name", person.MiddleName);
            Assert.AreEqual("last-name", person.LastName);
            Assert.AreEqual("maiden-name", person.MaidenName);
            Assert.AreEqual("mobile-phone", person.MobilePhone);
            Assert.AreEqual(999, person.MobileCarrierId);
            Assert.AreEqual("date-of-birth", person.DateOfBirth);
            Assert.AreEqual(5, person.MaritalStatusId);
            Assert.AreEqual(2, person.GenderId);
            Assert.AreEqual("employer-name", person.EmployerName);
            Assert.AreEqual("address-line-1", person.AddressLine1);
            Assert.AreEqual("address-line-2", person.AddressLine2);
            Assert.AreEqual("city", person.City);
            Assert.AreEqual("state", person.State);
            Assert.AreEqual("postal-code", person.PostalCode);
            Assert.AreEqual(startDate, person.AttendanceStartDate);
            Assert.AreEqual("foreign-country", person.ForeignCountry);
            Assert.AreEqual("home-phone", person.HomePhone);
            Assert.AreEqual(8, person.CongregationId);
            Assert.AreEqual(7, person.HouseholdId);
            Assert.AreEqual("hh name", person.HouseholdName);
            Assert.AreEqual(6, person.AddressId);
            Assert.AreSame(_householdMembers, person.HouseholdMembers);
        }
    }
}
