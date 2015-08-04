﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinistryPlatform.Translation.PlatformService;
using MinistryPlatform.Translation.Services;
using MinistryPlatform.Translation.Services.Interfaces;
using Moq;

namespace MinistryPlatform.Translation.Test
{
    [TestFixture]
    [Category("IntegrationTests")]
    public class LookupTest
    {

        private const string USERNAME = "testme";
        private const string PASSWORD = "changeme";
        private const string EMAIL = "donotreply+testme@crossroads.net";
    
        private AuthenticationServiceImpl _fixture;
        private PlatformServiceClient _platformService;
        private Mock<IMinistryPlatformService> _ministryPlatformService;

       
        [SetUp]
        public void SetUp()
        {
            _ministryPlatformService = new Mock<IMinistryPlatformService>();
            _platformService = new PlatformServiceClient();
            _fixture = new AuthenticationServiceImpl(_platformService, _ministryPlatformService.Object);
        }
       
        [Test]
        public void FindAnAttribute([Values("Dentist", "Social media wizard")] string attributeName)
        {
            var authData = AuthenticationService.authenticate(USERNAME, PASSWORD);
            Assert.IsNotNull(authData);
            var token = authData["token"].ToString();

            var pageId = Convert.ToInt32(ConfigurationManager.AppSettings["Attributes"]);

            var attribute = MinistryPlatformService.GetLookupRecord(pageId, attributeName, token, 1);
            Assert.IsNotNull(attribute);
        }

        [Test]
        public void ShouldReturnAValidObjectWithUserIdAndEmailAddress()
        {
            var authData = AuthenticationService.authenticate(USERNAME, PASSWORD);
            Assert.IsNotNull(authData);
            var token = authData["token"].ToString();
            var contactId = _fixture.GetContactId(token);
            Assert.IsNotNull(contactId);
            var emails = LookupService.EmailSearch(EMAIL, token);
            Assert.IsNotEmpty(emails);
        }

        [Test]
        public void ShouldReturnValidObjectForUpperCaseEmailAddress()
        {
            var authData = AuthenticationService.authenticate(USERNAME, PASSWORD);
            Assert.IsNotNull(authData);
            var token = authData["token"].ToString();
            var contactId = _fixture.GetContactId(token);
            Assert.IsNotNull(contactId);
            var emails = LookupService.EmailSearch(EMAIL.ToUpper(), token);
            Assert.IsNotEmpty(emails);
        }

        [Test]
        public void ShouldBeEmpty()
        {
            var authData = AuthenticationService.authenticate(USERNAME, PASSWORD);
            Assert.IsNotNull(authData);
            var token = authData["token"].ToString();
            var contactId = _fixture.GetContactId(token);
            Assert.IsNotNull(contactId);
            var emails = LookupService.EmailSearch("CRAP@CRAP.com", token);
            Assert.IsEmpty(emails);
        }

        [Test]
        public void ShouldFindListOfGenders()
        {
            var authData = AuthenticationService.authenticate(USERNAME, PASSWORD);
            Assert.IsNotNull(authData);
            var token = authData["token"].ToString();
            List<Dictionary<string,object>> genders = LookupService.Genders(token);
            Assert.IsNotEmpty(genders);
            genders.ForEach(x =>
            {
                Assert.IsInstanceOf<Dictionary<string,object>>(x);
            });
        }

        [Test]
        public void ShouldFindListOfMaritalStatus()
        {
            var authData = AuthenticationService.authenticate(USERNAME, PASSWORD);
            Assert.IsNotNull(authData);
            var token = authData["token"].ToString();
            List<Dictionary<string, object>> maritalStatus = LookupService.MaritalStatus(token);
            Assert.IsNotEmpty(maritalStatus);
            maritalStatus.ForEach(x =>
            {
                Assert.IsInstanceOf<Dictionary<string, object>>(x);
            });
        }

        [Test]
        public void ShouldFindListOfServiceProviders()
        {
            var authData = AuthenticationService.authenticate(USERNAME, PASSWORD);
            Assert.IsNotNull(authData);
            var token = authData["token"].ToString();
            List<Dictionary<string, object>> ServiceProviders = LookupService.ServiceProviders(token);
            Assert.IsNotEmpty(ServiceProviders);
            ServiceProviders.ForEach(x =>
            {
                Assert.IsInstanceOf<Dictionary<string, object>>(x);
            });
        }

        [Test]
        public void ShouldFindListOfStates()
        {
            var authData = AuthenticationService.authenticate(USERNAME, PASSWORD);
            Assert.IsNotNull(authData);
            var token = authData["token"].ToString();
            List<Dictionary<string, object>> States = LookupService.States(token);
            Assert.IsNotEmpty(States);
            States.ForEach(x =>
            {
                Assert.IsInstanceOf<Dictionary<string, object>>(x);
            });
        }

        [Test]
        public void ShouldFindListOfCountries()
        {
            var authData = AuthenticationService.authenticate(USERNAME, PASSWORD);
            Assert.IsNotNull(authData);
            var token = authData["token"].ToString();
            List<Dictionary<string, object>> Countries = LookupService.Countries(token);
            Assert.IsNotEmpty(Countries);
            Countries.ForEach(x =>
            {
                Assert.IsInstanceOf<Dictionary<string, object>>(x);
            });
        }

       [Test]
        public void ShouldFindListOfCrossroadsLocations()
        {
            var authData = AuthenticationService.authenticate(USERNAME, PASSWORD);
            Assert.IsNotNull(authData);
            var token = authData["token"].ToString();;
            List<Dictionary<string, object>> CrossroadsLocations = LookupService.CrossroadsLocations(token);
            Assert.IsNotEmpty(CrossroadsLocations);
            var clifton = new Dictionary<string, object>();
            clifton.Add("dp_RecordID", 11);
            clifton.Add("dp_RecordName", "Clifton");

           ////var list = CrossroadsLocations.ToList();
            Assert.Contains(clifton, CrossroadsLocations);
            CrossroadsLocations.ForEach(x =>
            {
                Assert.IsInstanceOf<Dictionary<string, object>>(x);
            });
        }

    }
}
