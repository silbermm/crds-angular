using System;
using crds_angular.Controllers.API;
using crds_angular.Models.Crossroads.Stewardship;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using Crossroads.Utilities.Messaging.Interfaces;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http.Controllers;
using System.Web.Http.Results;
using crds_angular.Exceptions;
using crds_angular.Models.Json;
using MinistryPlatform.Models;
using MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.test.controllers
{
    public class CheckScannerControllerTest
    {
        private CheckScannerController _fixture;
        private Mock<IConfigurationWrapper> _configuration;
        private Mock<ICheckScannerService> _checkScannerService;
        private Mock<IAuthenticationService> _authenticationService;
        private Mock<ICommunicationService> _communicationService;
        private Mock<IMessageQueueFactory> _messageQueueFactory;
        private Mock<IMessageFactory> _messageFactory;

        private const string AuthType = "auth_type";
        private const string AuthToken = "auth_token";

        [SetUp]
        public void SetUp()
        {
            _configuration = new Mock<IConfigurationWrapper>();
            _checkScannerService = new Mock<ICheckScannerService>(MockBehavior.Strict);
            _authenticationService = new Mock<IAuthenticationService>();
            _communicationService = new Mock<ICommunicationService>();
            _messageQueueFactory = new Mock<IMessageQueueFactory>(MockBehavior.Strict);
            _messageFactory = new Mock<IMessageFactory>(MockBehavior.Strict);

            _configuration.Setup(mocked => mocked.GetConfigValue("CheckScannerDonationsAsynchronousProcessingMode")).Returns("false");
            _configuration.Setup(mocked => mocked.GetConfigValue("CheckScannerDonationsQueueName")).Returns("CheckScannerBatchQueue");

            _fixture = new CheckScannerController(_configuration.Object, _checkScannerService.Object, _authenticationService.Object, _communicationService.Object, _messageQueueFactory.Object, _messageFactory.Object);

            _fixture.Request = new HttpRequestMessage();
            _fixture.Request.Headers.Authorization = new AuthenticationHeaderValue(AuthType, AuthToken);
            _fixture.RequestContext = new HttpRequestContext();
        }

        [Test]
        public void TestGetOpenBatches()
        {
            var batches = new List<CheckScannerBatch>
            {
                new CheckScannerBatch
                {
                    Id = 1,
                    Name = "Name 1",
                    ScanDate = DateTime.Now,
                    Status = BatchStatus.NotExported
                },
                new CheckScannerBatch
                {
                    Id = 2,
                    Name = "Name 2",
                    ScanDate = DateTime.Now,
                    Status = BatchStatus.NotExported
                }
            };
            _checkScannerService.Setup(mocked => mocked.GetBatches(true)).Returns(batches);

            var result = _fixture.GetBatches();
            _checkScannerService.VerifyAll();
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkNegotiatedContentResult<List<CheckScannerBatch>>>(result);
            var okResult = (OkNegotiatedContentResult<List<CheckScannerBatch>>) result;
            Assert.IsNotNull(okResult.Content);
            Assert.AreSame(batches, okResult.Content);
        }

        [Test]
        public void TestGetAllBatches()
        {
            var batches = new List<CheckScannerBatch>
            {
                new CheckScannerBatch
                {
                    Id = 1,
                    Name = "Name 1",
                    ScanDate = DateTime.Now,
                    Status = BatchStatus.NotExported
                },
                new CheckScannerBatch
                {
                    Id = 2,
                    Name = "Name 2",
                    ScanDate = DateTime.Now,
                    Status = BatchStatus.NotExported
                }
            };
            _checkScannerService.Setup(mocked => mocked.GetBatches(false)).Returns(batches);

            var result = _fixture.GetBatches(false);
            _checkScannerService.VerifyAll();
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkNegotiatedContentResult<List<CheckScannerBatch>>>(result);
            var okResult = (OkNegotiatedContentResult<List<CheckScannerBatch>>)result;
            Assert.IsNotNull(okResult.Content);
            Assert.AreSame(batches, okResult.Content);
        }

        [Test]
        public void TestGetChecksForBatch()
        {
            var checks = new List<CheckScannerCheck>
            {
                new CheckScannerCheck
                {
                    AccountNumber = "123",
                    RoutingNumber = "456",
                    Amount = 111M
                },
                new CheckScannerCheck
                {
                    AccountNumber = "789",
                    RoutingNumber = "012",
                    Amount = 222M
                },
            };
            _checkScannerService.Setup(mocked => mocked.GetChecksForBatch("batch123")).Returns(checks);

            var result = _fixture.GetChecksForBatch("batch123");
            _checkScannerService.VerifyAll();
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkNegotiatedContentResult<List<CheckScannerCheck>>>(result);
            var okResult = (OkNegotiatedContentResult<List<CheckScannerCheck>>)result;
            Assert.IsNotNull(okResult.Content);
            Assert.AreSame(checks, okResult.Content);
        }

        [Test]
        public void TestCreateDonationsForBatch()
        {
            var batch = new CheckScannerBatch
            {
                Name = "batch123",
                ProgramId = 987
            };

            var batchResult = new CheckScannerBatch
            {
                Name = "batch123",
                ProgramId = 987,
            };
            batchResult.Checks.Add(new CheckScannerCheck
            {
                AccountNumber = "111",
                RoutingNumber = "222"
            });
            batchResult.Checks.Add(new CheckScannerCheck
            {
                AccountNumber = "333",
                RoutingNumber = "444"
            });
            _checkScannerService.Setup(mocked => mocked.CreateDonationsForBatch(batch)).Returns(batchResult);

            var result = _fixture.CreateDonationsForBatch(batch);
            _checkScannerService.VerifyAll();
            _messageFactory.VerifyAll();
            _messageQueueFactory.VerifyAll();
            Assert.NotNull(result);
            Assert.IsInstanceOf<OkNegotiatedContentResult<CheckScannerBatch>>(result);
            var okResult = (OkNegotiatedContentResult<CheckScannerBatch>)result;
            Assert.IsNotNull(okResult.Content);
            Assert.AreSame(batchResult, okResult.Content);
        }

        [Test]
        public void TestGetContactDonorForCheck()
        {
            var encryptedKey = "disCv2kF/8HlRCWeTqolok1G4imf1cNZershgmCCFDI=";
            var donorDetail = new EZScanDonorDetails
            {
                DisplayName = "Peyton Manning",
                Address = new PostalAddress()
                {
                    Line1 = "1 Superbowl Way",
                    Line2 = "Suite 1000",
                    City = "Denver",
                    State = "CO",
                    PostalCode = "11111-2222"
                  }
            };

            _checkScannerService.Setup(mocked => mocked.GetContactDonorForCheck(encryptedKey)).Returns(donorDetail);

            var result = _fixture.GetDonorForCheck(encryptedKey);
            _checkScannerService.VerifyAll();
            
            Assert.NotNull(result);
       
            Assert.IsInstanceOf<OkNegotiatedContentResult<EZScanDonorDetails>>(result);
            var okResult = (OkNegotiatedContentResult<EZScanDonorDetails>)result;
            Assert.IsNotNull(okResult.Content);
            Assert.AreSame(donorDetail, okResult.Content);
        }
    }
}
