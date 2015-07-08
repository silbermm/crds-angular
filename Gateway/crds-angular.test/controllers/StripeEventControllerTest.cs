using System;
using System.Collections.Generic;
using System.Web.Http.Results;
using Moq;
using NUnit.Framework;
using crds_angular.Controllers.API;
using crds_angular.Models.Crossroads;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using Newtonsoft.Json.Linq;

namespace crds_angular.test.controllers
{
    public class StripeEventControllerTest
    {
        private StripeEventController _fixture;
        private Mock<IPaymentService> _paymentService;
        private Mock<IDonationService> _donationService;

        [SetUp]
        public void SetUp()
        {
            var configuration = new Mock<IConfigurationWrapper>();
            configuration.Setup(mocked => mocked.GetConfigIntValue("DonationStatusDeposited")).Returns(999);
            configuration.Setup(mocked => mocked.GetConfigIntValue("DonationStatusSucceeded")).Returns(888);
            configuration.Setup(mocked => mocked.GetConfigValue("StripeWebhookLiveMode")).Returns("true");

            _paymentService = new Mock<IPaymentService>(MockBehavior.Strict);
            _donationService = new Mock<IDonationService>(MockBehavior.Strict);
            _fixture = new StripeEventController(_paymentService.Object, _donationService.Object, configuration.Object);
        }

        [Test]
        public void TestProcessStripeEventNullEvent()
        {
            var result = _fixture.ProcessStripeEvent(null);
            Assert.IsInstanceOf<InvalidModelStateResult>(result);
            _paymentService.VerifyAll();
            _donationService.VerifyAll();
        }

        [Test]
        public void TestProcessStripeEventInvalidModelState()
        {
            _fixture.ModelState.AddModelError("Data", new Exception("invalid"));
            var result = _fixture.ProcessStripeEvent(new StripeEvent());
            Assert.IsInstanceOf<InvalidModelStateResult>(result);
            _paymentService.VerifyAll();
            _donationService.VerifyAll();
        }

        [Test]
        public void TestProcessStripeEventLiveModeMismatch()
        {
            var e = new StripeEvent
            {
                LiveMode = false
            };

            var result = _fixture.ProcessStripeEvent(e);
            Assert.IsInstanceOf<OkResult>(result);
            _paymentService.VerifyAll();
            _donationService.VerifyAll();
        }

        [Test]
        public void TestProcessStripeEventNoMatchingEventHandler()
        {
            var e = new StripeEvent
            {
                LiveMode = true,
                Type = "not.this.one"
            };

            var result = _fixture.ProcessStripeEvent(e);
            Assert.IsInstanceOf<OkResult>(result);
            _paymentService.VerifyAll();
            _donationService.VerifyAll();
        }

        [Test]
        public void TestChargeSucceeded()
        {
            var e = new StripeEvent
            {
                LiveMode = true,
                Type = "charge.succeeded",
                Created = DateTime.Now.AddDays(-1),
                Data = new StripeEventData
                {
                    Object = JObject.FromObject(new StripeCharge
                    {
                        Id = "9876"
                    })
                }
            };

            _donationService.Setup(mocked => mocked.UpdateDonationStatus("9876", 888, e.Created, null));
            var result = _fixture.ProcessStripeEvent(e);
            Assert.IsInstanceOf<OkResult>(result);
            _paymentService.VerifyAll();
            _donationService.VerifyAll();
        }

        [Test]
        public void TestTransferPaidNoChargesFound()
        {
            var e = new StripeEvent
            {
                LiveMode = true,
                Type = "transfer.paid",
                Created = DateTime.Now.AddDays(-1),
                Data = new StripeEventData
                {
                    Object = JObject.FromObject(new StripeTransfer
                    {
                        Id = "tx9876",
                    })
                }
            };

            _paymentService.Setup(mocked => mocked.GetChargesForTransfer("tx9876")).Returns(new List<string>());
            var result = _fixture.ProcessStripeEvent(e);
            Assert.IsInstanceOf<OkResult>(result);
            _paymentService.VerifyAll();
            _donationService.VerifyAll();
        }

        [Test]
        public void TestTransferPaid()
        {
            var e = new StripeEvent
            {
                LiveMode = true,
                Type = "transfer.paid",
                Created = DateTime.Now.AddDays(-1),
                Data = new StripeEventData
                {
                    Object = JObject.FromObject(new StripeTransfer
                    {
                        Id = "tx9876",
                    })
                }
            };

            var charges = new List<string>
            {
                "111",
                "222",
                "333",
            };

            _paymentService.Setup(mocked => mocked.GetChargesForTransfer("tx9876")).Returns(charges);
            _donationService.Setup(mocked => mocked.UpdateDonationStatus("111", 999, e.Created, null));
            _donationService.Setup(mocked => mocked.UpdateDonationStatus("222", 999, e.Created, null));
            _donationService.Setup(mocked => mocked.UpdateDonationStatus("333", 999, e.Created, null));

            var result = _fixture.ProcessStripeEvent(e);
            Assert.IsInstanceOf<OkResult>(result);
            _paymentService.VerifyAll();
            _donationService.VerifyAll();

        }

    }
}
