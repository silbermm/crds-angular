using System;
using System.Collections.Generic;
using System.Linq;
using crds_angular.Models.Crossroads.Stewardship;
using crds_angular.Services;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using Moq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp.Extensions;

namespace crds_angular.test.Services
{
    public class StripeEventServiceTest
    {
        private StripeEventService _fixture;
        private Mock<IPaymentService> _paymentService;
        private Mock<IDonationService> _donationService;

        [SetUp]
        public void SetUp()
        {
            var configuration = new Mock<IConfigurationWrapper>();
            configuration.Setup(mocked => mocked.GetConfigIntValue("DonationStatusDeposited")).Returns(999);
            configuration.Setup(mocked => mocked.GetConfigIntValue("DonationStatusSucceeded")).Returns(888);
            configuration.Setup(mocked => mocked.GetConfigIntValue("DonationStatusDeclined")).Returns(777);
            configuration.Setup(mocked => mocked.GetConfigIntValue("BatchEntryTypePaymentProcessor")).Returns(555);

            _paymentService = new Mock<IPaymentService>(MockBehavior.Strict);
            _donationService = new Mock<IDonationService>(MockBehavior.Strict);
            _fixture = new StripeEventService(_paymentService.Object, _donationService.Object, configuration.Object);
        }

        [Test]
        public void TestProcessStripeEventNoMatchingEventHandler()
        {
            var e = new StripeEvent
            {
                LiveMode = true,
                Type = "not.this.one"
            };

            Assert.IsNull(_fixture.ProcessStripeEvent(e));
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

            _donationService.Setup(mocked => mocked.UpdateDonationStatus("9876", 888, e.Created, null)).Returns(123);
            Assert.IsNull(_fixture.ProcessStripeEvent(e));
            _paymentService.VerifyAll();
            _donationService.VerifyAll();
        }

        [Test]
        public void TestChargeFailed()
        {
            var e = new StripeEvent
            {
                LiveMode = true,
                Type = "charge.failed",
                Created = DateTime.Now.AddDays(-1),
                Data = new StripeEventData
                {
                    Object = JObject.FromObject(new StripeCharge
                    {
                        Id = "9876",
                        FailureCode = "invalid_routing_number",
                        FailureMessage = "description from stripe"
                    })
                }
            };

            _donationService.Setup(mocked => mocked.UpdateDonationStatus("9876", 777, e.Created, "invalid_routing_number: description from stripe")).Returns(123);
            _donationService.Setup(mocked => mocked.ProcessDeclineEmail("9876"));
            Assert.IsNull(_fixture.ProcessStripeEvent(e));
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

            _paymentService.Setup(mocked => mocked.GetChargesForTransfer("tx9876")).Returns(new List<StripeCharge>());
            var result = _fixture.ProcessStripeEvent(e);
            Assert.IsNotNull(_fixture.ProcessStripeEvent(e));
            Assert.IsInstanceOf<TransferPaidResponseDTO>(result);
            var tp = (TransferPaidResponseDTO)result;
            Assert.AreEqual(0, tp.TotalTransactionCount);
            Assert.AreEqual(0, tp.SuccessfulUpdates.Count);
            Assert.AreEqual(0, tp.FailedUpdates.Count);

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
                        Amount = 50000
                    })
                }
            };

            var charges = new List<StripeCharge>
            {
                new StripeCharge
                {
                    Id = "ch111",
                    Amount = 111
                },
                new StripeCharge
                {
                    Id = "ch222",
                    Amount = 222
                },
                new StripeCharge
                {
                    Id = "ch333",
                    Amount = 333
                },
                new StripeCharge
                {
                    Id = "ch444",
                    Amount = 444
                }
            };

            _paymentService.Setup(mocked => mocked.GetChargesForTransfer("tx9876")).Returns(charges);
            _donationService.Setup(
                mocked => mocked.CreatePaymentProcessorEventError(e, It.IsAny<StripeEventResponseDTO>()));
            _donationService.Setup(mocked => mocked.UpdateDonationStatus("ch111", 999, e.Created, null)).Returns(1111);
            _donationService.Setup(mocked => mocked.UpdateDonationStatus("ch222", 999, e.Created, null)).Returns(2222);
            _donationService.Setup(mocked => mocked.UpdateDonationStatus("ch333", 999, e.Created, null)).Returns(3333);
            _donationService.Setup(mocked => mocked.UpdateDonationStatus("ch444", 999, e.Created, null)).Throws(new Exception("Not gonna do it, wouldn't be prudent."));
            _donationService.Setup(mocked => mocked.CreateDeposit(It.IsAny<DepositDTO>())).Returns(
                (DepositDTO o) =>
                {
                    o.Id = 98765;
                    return (o);
                });
            _donationService.Setup(mocked => mocked.CreateDonationBatch(It.IsAny<DonationBatchDTO>())).Returns((DonationBatchDTO o) => o);

            var result = _fixture.ProcessStripeEvent(e);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<TransferPaidResponseDTO>(result);
            var tp = (TransferPaidResponseDTO)result;
            Assert.AreEqual(4, tp.TotalTransactionCount);
            Assert.AreEqual(3, tp.SuccessfulUpdates.Count);
            Assert.AreEqual(charges.GetRange(0, 3).Select(charge => charge.Id), tp.SuccessfulUpdates);
            Assert.AreEqual(1, tp.FailedUpdates.Count);
            Assert.AreEqual("ch444", tp.FailedUpdates[0].Key);
            Assert.AreEqual("Not gonna do it, wouldn't be prudent.", tp.FailedUpdates[0].Value);
            Assert.IsNotNull(tp.Batch);
            Assert.IsNotNull(tp.Deposit);
            Assert.IsNotNull(tp.Exception);

            _donationService.Verify(mocked => mocked.CreateDonationBatch(It.Is<DonationBatchDTO>(o =>
                o.BatchName.Matches(@"MP\d{12}")
                && o.SetupDateTime == o.FinalizedDateTime
                && o.BatchEntryType == 555
                && o.ItemCount == 3
                && o.BatchTotalAmount == (111 + 222 + 333) / 100M
                && o.Donations != null
                && o.Donations.Count == 3
                && o.DepositId == 98765
                && o.ProcessorTransferId.Equals("tx9876")
            )));

            _donationService.Verify(mocked => mocked.CreateDeposit(It.Is<DepositDTO>(o =>
                o.DepositName.Matches(@"MP\d{12}")
                && !o.Exported
                && o.AccountNumber.Equals(" ")
                && o.BatchCount == 1
                && o.DepositDateTime != null
                && o.DepositTotalAmount == 500M
                && o.Notes == null
                && o.ProcessorTransferId.Equals("tx9876")
            )));

            _paymentService.VerifyAll();
            _donationService.VerifyAll();
        }
    }
}
