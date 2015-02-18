﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinistryPlatform.Translation.Services;

namespace MinistryPlatform.Translation.Test
{
    [TestFixture]
    public class LookupTest
    {

        private const string USERNAME = "testme";
        private const string PASSWORD = "changeme";
        private const string EMAIL = "testme@test.com";

        [Test]
        public void FindAnAttribute([Values("Dentist", "Social media wizard")] string attributeName)
        {
            var token = AuthenticationService.authenticate(USERNAME, PASSWORD);
            Assert.IsNotNull(token);

            var pageId = Convert.ToInt32(ConfigurationManager.AppSettings["Attributes"]);

            var attribute = MinistryPlatformService.GetLookupRecord(pageId, attributeName, token, 1);
            Assert.IsNotNull(attribute);
        }

        [Test]
        public void ShouldReturnAValidObjectWithUserIdAndEmailAddress()
        {
            var token = AuthenticationService.authenticate(USERNAME, PASSWORD);
            Assert.IsNotNull(token);
            var contactId = AuthenticationService.GetContactId(token);
            Assert.IsNotNull(contactId);
            var emails = LookupService.EmailSearch(EMAIL, token);
            Assert.IsNotEmpty(emails);
        }

        [Test]
        public void ShouldReturnValidObjectForUpperCaseEmailAddress()
        {
            var token = AuthenticationService.authenticate(USERNAME, PASSWORD);
            Assert.IsNotNull(token);
            var contactId = AuthenticationService.GetContactId(token);
            Assert.IsNotNull(contactId);
            var emails = LookupService.EmailSearch(EMAIL.ToUpper(), token);
            Assert.IsNotEmpty(emails);
        }

        [Test]
        public void ShouldBeEmpty()
        {
            var token = AuthenticationService.authenticate(USERNAME, PASSWORD);
            Assert.IsNotNull(token);
            var contactId = AuthenticationService.GetContactId(token);
            Assert.IsNotNull(contactId);
            var emails = LookupService.EmailSearch("CRAP@CRAP.com", token);
            Assert.IsEmpty(emails);
        }

        [Test]
        public void ShouldFindListOfGenders()
        {
            var token = AuthenticationService.authenticate(USERNAME, PASSWORD);
            Assert.IsNotNull(token);
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
            var token = AuthenticationService.authenticate(USERNAME, PASSWORD);
            Assert.IsNotNull(token);
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
            var token = AuthenticationService.authenticate(USERNAME, PASSWORD);
            Assert.IsNotNull(token);
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
            var token = AuthenticationService.authenticate(USERNAME, PASSWORD);
            Assert.IsNotNull(token);
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
            var token = AuthenticationService.authenticate(USERNAME, PASSWORD);
            Assert.IsNotNull(token);
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
            var token = AuthenticationService.authenticate(USERNAME, PASSWORD);
            Assert.IsNotNull(token);
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