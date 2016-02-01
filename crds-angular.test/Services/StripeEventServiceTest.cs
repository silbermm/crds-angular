using System;
using System.Collections.Generic;
using System.Linq;
using crds_angular.Models.Crossroads.Stewardship;
using crds_angular.Services;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using MinistryPlatform.Models.DTO;
using Moq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp.Extensions;
using IDonationService = crds_angular.Services.Interfaces.IDonationService;
using IDonorService = crds_angular.Services.Interfaces.IDonorService;

namespace crds_angular.test.Services
{
    public class StripeEventServiceTest
    {
        private StripeEventService _fixture;
        private Mock<IPaymentService> _paymentService;
        private Mock<IDonationService> _donationService;
        private Mock<IDonorService> _donorService;
        private Mock<MinistryPlatform.Translation.Services.Interfaces.IDonorService> _mpDonorService;
        
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
            _donorService = new Mock<IDonorService>(MockBehavior.Strict);
            _mpDonorService = new Mock<MinistryPlatform.Translation.Services.Interfaces.IDonorService>(MockBehavior.Strict);

            _fixture = new StripeEventService(_paymentService.Object, _donationService.Object, _donorService.Object, _mpDonorService.Object, configuration.Object);
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

            _donationService.Setup(mocked => mocked.GetDonationBatchByProcessorTransferId("tx9876")).Returns((DonationBatchDTO)null);
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
                        Amount = 1443
                    })
                }
            };

            var charges = new List<StripeCharge>
            {
                new StripeCharge
                {
                    Id = "ch111",
                    Amount = 111,
                    Fee = 1,
                    Type = "charge"
                },
                new StripeCharge
                {
                    Id = "ch222",
                    Amount = 222,
                    Fee = 2,
                    Type = "charge"
                },
                new StripeCharge
                {
                    Id = "ch333",
                    Amount = 333,
                    Fee = 3,
                    Type = "charge"
                },
                new StripeCharge
                {
                    Id = "ch777",
                    Amount = 777, 
                    Fee = 7,
                    Type = "charge"
                },
                new StripeCharge
                {
                    Id = "ch444",
                    Amount = 444,
                    Fee = 4,
                    Type = "charge"
                },
                new StripeCharge
                {
                    Id = "ch555",
                    Amount = 555, 
                    Fee = 5,
                    Type = "refund"
                }
            };

           
            _donationService.Setup(mocked => mocked.GetDonationByProcessorPaymentId("ch111")).Returns(new DonationDTO
            {
                Id = "1111",
                BatchId = null
            });

            _donationService.Setup(mocked => mocked.GetDonationByProcessorPaymentId("ch222")).Returns(new DonationDTO
            {
                Id = "2222",
                BatchId = null
            });

            _donationService.Setup(mocked => mocked.GetDonationByProcessorPaymentId("ch333")).Returns(new DonationDTO
            {
                Id = "3333",
                BatchId = null
            });

            _donationService.Setup(mocked => mocked.GetDonationByProcessorPaymentId("ch444")).Throws(new Exception("Not gonna do it, wouldn't be prudent."));

            _paymentService.Setup(mocked => mocked.GetChargeRefund("ch555")).Returns(new StripeRefund
            {
                Data = new List<StripeRefundData>
                { new StripeRefundData()
                    {
                        Id = "ch555",
                        Amount = "987",
                        Charge = new StripeCharge {
                            Id = "re_123456"
                        }
                    }
                }
            });

            _donationService.Setup(mocked => mocked.GetDonationByProcessorPaymentId("ch555")).Returns(new DonationDTO
            {
                Id = "5555",
                BatchId = 1984
            });
            _donationService.Setup(mocked => mocked.GetDonationBatch(1984)).Returns(new DonationBatchDTO
            {
                Id = 5150,
                ProcessorTransferId = "OU812"
            });

            _donationService.Setup(mocked => mocked.GetDonationByProcessorPaymentId("ch777")).Returns(new DonationDTO
            {
                Id = "7777",
                BatchId = 2112
            });
            _donationService.Setup(mocked => mocked.GetDonationBatch(2112)).Returns(new DonationBatchDTO
            {
                Id = 2112,
                ProcessorTransferId = null
            });

            _donationService.Setup(mocked => mocked.GetDonationBatchByProcessorTransferId("tx9876")).Returns((DonationBatchDTO)null);
            _paymentService.Setup(mocked => mocked.GetChargesForTransfer("tx9876")).Returns(charges);
            _donationService.Setup(
                mocked => mocked.CreatePaymentProcessorEventError(e, It.IsAny<StripeEventResponseDTO>()));
            _donationService.Setup(mocked => mocked.UpdateDonationStatus(1111, 999, e.Created, null)).Returns(1111);
            _donationService.Setup(mocked => mocked.UpdateDonationStatus(2222, 999, e.Created, null)).Returns(2222);
            _donationService.Setup(mocked => mocked.UpdateDonationStatus(3333, 999, e.Created, null)).Returns(3333);
            _donationService.Setup(mocked => mocked.UpdateDonationStatus(7777, 999, e.Created, null)).Returns(7777);
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
            Assert.AreEqual(6, tp.TotalTransactionCount);
            Assert.AreEqual(4, tp.SuccessfulUpdates.Count);
            Assert.AreEqual(charges.GetRange(0, 4).Select(charge => charge.Id), tp.SuccessfulUpdates);
            Assert.AreEqual(2, tp.FailedUpdates.Count);
            Assert.AreEqual("ch444", tp.FailedUpdates[0].Key);
            Assert.AreEqual("Not gonna do it, wouldn't be prudent.", tp.FailedUpdates[0].Value);
            Assert.AreEqual("ch555", tp.FailedUpdates[1].Key);
            Assert.IsNotNull(tp.Batch);
            Assert.IsNotNull(tp.Deposit);
            Assert.IsNotNull(tp.Exception);

            _donationService.Verify(mocked => mocked.CreateDonationBatch(It.Is<DonationBatchDTO>(o =>
                o.BatchName.Matches(@"MP\d{12}")
                && o.SetupDateTime == o.FinalizedDateTime
                && o.BatchEntryType == 555
                && o.ItemCount == 4
                && o.BatchTotalAmount == ((111 + 222 + 333 + 777) / Constants.StripeDecimalConversionValue)
                && o.Donations != null
                && o.Donations.Count == 4
                && o.DepositId == 98765
                && o.ProcessorTransferId.Equals("tx9876")
            )));

            _donationService.Verify(mocked => mocked.CreateDeposit(It.Is<DepositDTO>(o =>
                o.DepositName.Matches(@"MP\d{12}")
                && !o.Exported
                && o.AccountNumber.Equals(" ")
                && o.BatchCount == 1
                && o.DepositDateTime != null
                && o.DepositTotalAmount == (1456 / Constants.StripeDecimalConversionValue)
                && o.ProcessorFeeTotal == (13 / Constants.StripeDecimalConversionValue)
                && o.DepositAmount == (1443 / Constants.StripeDecimalConversionValue)
                && o.Notes == null
                && o.ProcessorTransferId.Equals("tx9876")
            )));

            _paymentService.VerifyAll();
            _donationService.VerifyAll();
        }

        [Test]
        public void TestInvoicePaymentSucceededNoAmount()
        {
            var invoice = new StripeInvoice
            {
                Subscription = "sub_123",
                Amount = 0,
                Charge = "ch_123",
            };

            _fixture.InvoicePaymentSucceeded(DateTime.Now, invoice);
            _paymentService.VerifyAll();
            _donorService.VerifyAll();
        }

        [Test]
        public void TestInvoicePaymentSucceededNoCharge()
        {
            var invoice = new StripeInvoice
            {
                Subscription = "sub_123",
                Amount = 123,
                Charge = "   ",
            };

            _fixture.InvoicePaymentSucceeded(DateTime.Now, invoice);
            _paymentService.VerifyAll();
            _donorService.VerifyAll();
        }

        [Test]
        public void TestInvoicePaymentSucceeded()
        {
            var eventTimestamp = DateTime.Now;
            const string processorId = "cus_123";
            const string subscriptionId = "sub_123";
            const string chargeId = "ch_123";

            var invoice = new StripeInvoice
            {
                Subscription = subscriptionId,
                Amount = 12300,
                Charge = chargeId,
                Customer = processorId,
            };

            const int chargeAmount = 45600;
            int? feeAmount = 987;

            var charge = new StripeCharge
            {
                Amount = chargeAmount,
                BalanceTransaction = new StripeBalanceTransaction
                {
                    Amount = 78900,
                    Fee = feeAmount
                },
                Status = "succeeded"
            };

            _paymentService.Setup(mocked => mocked.GetCharge(chargeId)).Returns(charge);

            const int donorId = 321;
            const string programId = "3";
            const string paymentType = "Bank";
            const int recurringGiftId = 654;
            const int donorAccountId = 987;
            const int donationStatus = 4;
            const decimal amt = 789;

            var recurringGift = new CreateDonationDistDto
            {
                Amount = amt,
                DonorAccountId = donorAccountId,
                DonorId = donorId,
                PaymentType = paymentType,
                ProgramId = programId,
                RecurringGiftId = recurringGiftId
            };
            _donorService.Setup(mocked => mocked.GetRecurringGiftForSubscription(subscriptionId)).Returns(recurringGift);
            _mpDonorService.Setup(mocked => mocked.UpdateRecurringGiftFailureCount(recurringGift.RecurringGiftId.Value, Constants.ResetFailCount));

            _mpDonorService.Setup(
                mocked =>
                    mocked.CreateDonationAndDistributionRecord(
                        It.Is<DonationAndDistributionRecord>(
                            d => d.DonationAmt == (int) (chargeAmount/Constants.StripeDecimalConversionValue) && 
                                 d.FeeAmt == feeAmount && 
                                 d.DonorId == donorId &&
                                 d.ProgramId.Equals(programId) &&
                                 d.PledgeId == null &&
                                 d.ChargeId.Equals(chargeId) &&
                                 d.PymtType.Equals(paymentType) &&
                                 d.ProcessorId.Equals(processorId) &&
                                 d.RegisteredDonor &&
                                 !d.Anonymous &&
                                 d.RecurringGift &&
                                 d.RecurringGiftId == recurringGiftId &&
                                 d.DonorAcctId.Equals(donorAccountId+"") &&
                                 d.CheckScannerBatchName == null &&
                                 d.DonationStatus == donationStatus &&
                                 d.CheckNumber == null))).Returns(123);

            _fixture.InvoicePaymentSucceeded(eventTimestamp, invoice);
            _paymentService.VerifyAll();
            _mpDonorService.VerifyAll();
            _donorService.VerifyAll();
        }

        [Test]
        public void TestInvoicePaymentFailedNoCancel()
        {
            const string processorId = "cus_123";
            const string id = "9876";
            const string subscriptionId = "sub_123";
            const string charge = "ch_2468";
            const int recurringGiftId = 123456;

            var e = new StripeEvent
            {
                LiveMode = true,
                Type = "invoice.payment_failed",
                Created = DateTime.Now.AddDays(-1),
                Data = new StripeEventData
                {
                    Object = JObject.FromObject(new StripeInvoice()
                    {
                        Id = id,
                        Customer = processorId,
                        Charge = charge,
                        Subscription = subscriptionId
                    })
                }
            };

            var gift = new CreateDonationDistDto
            {
                Frequency = 1,
                RecurringGiftId = recurringGiftId
            };

            _mpDonorService.Setup(mocked => mocked.ProcessRecurringGiftDecline(subscriptionId));
            _mpDonorService.Setup(mocked => mocked.GetRecurringGiftForSubscription(subscriptionId)).Returns(gift);
           
            Assert.IsNull(_fixture.ProcessStripeEvent(e));
            _fixture.ProcessStripeEvent(e);
            _mpDonorService.VerifyAll();
        }

        [Test]
        public void TestInvoicePaymentFailedCancelPlanAndSubscription()
        {
            const string processorId = "cus_123";
            const string donorAccountProcessorId = "cus_456";
            const string subscriptionId = "sub_123";
            const int failCount = 3;
            const int recurringGiftId = 123456;
            const int donorId = 3421;
            const int frequency = 2;
            const string id = "9876";
            const string charge = "ch_2468";
            const string planId = "Donor ID #3421 weekly"; 

            var e = new StripeEvent
            {
                LiveMode = true,
                Type = "invoice.payment_failed",
                Created = DateTime.Now.AddDays(-1),
                Data = new StripeEventData
                {
                    Object = JObject.FromObject(new StripeInvoice()
                    {
                        Id = id,
                        Customer = processorId,
                        Charge = charge,
                        Subscription = subscriptionId
                    })
                }
            };

            var gift = new CreateDonationDistDto
            {
                Frequency = frequency,
                RecurringGiftId = recurringGiftId,
                SubscriptionId = subscriptionId,
                ConsecutiveFailureCount =  failCount,
                DonorId =  donorId,
                StripeCustomerId = donorAccountProcessorId
            };

             var plan = new StripePlan
             {
                 Id = planId
             };

             var subscription = new StripeSubscription
             {
                 Plan = plan
             };

            _mpDonorService.Setup(mocked => mocked.ProcessRecurringGiftDecline(subscriptionId));
            _mpDonorService.Setup(mocked => mocked.GetRecurringGiftForSubscription(subscriptionId)).Returns(gift);
            _paymentService.Setup(mocked => mocked.CancelSubscription(donorAccountProcessorId, subscriptionId)).Returns(subscription);
            _paymentService.Setup(mocked => mocked.CancelPlan(subscription.Plan.Id)).Returns(plan);
            _mpDonorService.Setup(mocked => mocked.CancelRecurringGift(recurringGiftId));
           

            Assert.IsNull(_fixture.ProcessStripeEvent(e));
            _fixture.ProcessStripeEvent(e);
            _donorService.VerifyAll();
            _mpDonorService.VerifyAll();
            _donorService.VerifyAll();
            _paymentService.VerifyAll();
        }
    }
}
