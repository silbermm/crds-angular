﻿using crds_angular.Controllers.API;
using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Routing;
using System.Net.Http;
using System.Web.Http.Results;
using crds_angular.Services;
using crds_angular.Models.Json;

namespace crds_angular.test.controllers
{
    [TestFixture]
    class AccountControllerTest
    {
        private AccountController accountController;

        private const string USERNAME = "testme";
        private const string PASSWORD = "changeme";
        private const string FIRSTNAME = "Test";
        private const string NEW_PASSWORD = "changemeagain";

        [SetUp]
        public void SetUp()
        {            
            accountController = new AccountController();
            
        }

        [Test]
        public void ShouldReturnUnauthorized()
        {
            accountController.Request = new HttpRequestMessage();
            IHttpActionResult result = accountController.UpdatePassword(new NewPassword { password = "whatever" });
            Assert.IsInstanceOf(typeof(UnauthorizedResult), result);
        }

        [Test]
        public void ShouldReturnOk()
        {
            // First we need to get a sessionId
            string token = TranslationService.Login(USERNAME, PASSWORD);
            Assert.IsNotNull(token, "Token should be valid");
            
            // Set the cookie in the header
            HttpRequestMessage h = new HttpRequestMessage();
            h.Headers.Add("Cookie", "sessionId=" + token);
            accountController.Request = h;

            // Make the call...
            IHttpActionResult result = accountController.UpdatePassword(new NewPassword { password = NEW_PASSWORD });
            //OkNegotiatedContentResult<Object> o = (OkNegotiatedContentResult<Object>)result;
            Assert.IsInstanceOf(typeof(OkResult), result);

            IHttpActionResult result2 = accountController.UpdatePassword(new NewPassword { password = PASSWORD });
            Assert.IsInstanceOf(typeof(OkResult), result);

        }

    }
}