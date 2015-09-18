using System;
using System.Collections.Generic;
using crds_angular.App_Start;
using crds_angular.Models.Crossroads.Stewardship;
using crds_angular.Services;
using crds_angular.Services.Interfaces;
using Crossroads.Utilities.Interfaces;
using MinistryPlatform.Models;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using MPServices=MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.test.Services
{
    public class DonationServiceTest
    {
        private DonationService _fixture;
        private Mock<MPServices.IDonationService> _mpDonationService;
        private Mock<MPServices.IDonorService> _mpDonorService;
        private Mock<MPServices.IAuthenticationService> _mpAuthenticationService;
        private Mock<IPaymentService> _paymentService;
        private Mock<MPServices.IContactService> _contactService;
        private Mock<IConfigurationWrapper> _configurationWrapper;

        [SetUp]
        public void SetUp()
        {
            AutoMapperConfig.RegisterMappings();

            _mpDonationService = new Mock<MPServices.IDonationService>(MockBehavior.Strict);
            _mpDonorService = new Mock<MPServices.IDonorService>(MockBehavior.Strict);
            _mpAuthenticationService = new Mock<MPServices.IAuthenticationService>();
            _paymentService = new Mock<IPaymentService>();
            _contactService = new Mock<MPServices.IContactService>();
            _configurationWrapper = new Mock<IConfigurationWrapper>();

            _configurationWrapper.Setup(mocked => mocked.GetConfigIntValue("DonorStatementTypeFamily")).Returns(456);

            _fixture = new DonationService(_mpDonationService.Object, _mpDonorService.Object, _mpAuthenticationService.Object, _paymentService.Object, _contactService.Object, _configurationWrapper.Object);
        }

        [Test]
        public void TestGetDonationBatch()
        {
            _mpDonationService.Setup(mocked => mocked.GetDonationBatch(123)).Returns(new DonationBatch
            {
                Id = 123,
                DepositId = 456,
                ProcessorTransferId = "789"
            });
            var result = _fixture.GetDonationBatch(123);
            _mpDonationService.VerifyAll();
            Assert.IsNotNull(result);
            Assert.AreEqual(123, result.Id);
            Assert.AreEqual(456, result.DepositId);
            Assert.AreEqual("789", result.ProcessorTransferId);
        }

        [Test]
        public void TestGetDonationBatchReturnsNull()
        {
            _mpDonationService.Setup(mocked => mocked.GetDonationBatch(123)).Returns((DonationBatch) null);
            var result = _fixture.GetDonationBatch(123);
            _mpDonationService.VerifyAll();
            Assert.IsNull(result);
        }

        [Test]
        public void TestGetDonationBatchByProcessorTransferId()
        {
            _mpDonationService.Setup(mocked => mocked.GetDonationBatchByProcessorTransferId("123")).Returns(new DonationBatch
            {
                Id = 123,
                DepositId = 456,
                ProcessorTransferId = "789"
            });
            var result = _fixture.GetDonationBatchByProcessorTransferId("123");
            _mpDonationService.VerifyAll();
            Assert.IsNotNull(result);
            Assert.AreEqual(123, result.Id);
            Assert.AreEqual(456, result.DepositId);
            Assert.AreEqual("789", result.ProcessorTransferId);
        }

        [Test]
        public void TestGetDonationByProcessorPaymentIdDonationNotFound()
        {
            _mpDonationService.Setup(mocked => mocked.GetDonationByProcessorPaymentId("123")).Returns((Donation) null);
            Assert.IsNull(_fixture.GetDonationByProcessorPaymentId("123"));
            _mpDonationService.VerifyAll();
        }

        [Test]
        public void TestGetDonationByProcessorPaymentId()
        {
            _mpDonationService.Setup(mocked => mocked.GetDonationByProcessorPaymentId("123")).Returns(new Donation
            {
                donationId = 123,
                donationAmt = 456,
                batchId = 789
            });
            var result = _fixture.GetDonationByProcessorPaymentId("123");
            _mpDonationService.VerifyAll();
            Assert.IsNotNull(result);
            Assert.AreEqual(123+"", result.Id);
            Assert.AreEqual(456, result.Amount);
            Assert.AreEqual(789, result.BatchId);
        }

        [Test]
        public void TestGetDonationBatchByDepositDonationIdNotFound()
        {
            _mpDonationService.Setup(mocked => mocked.GetDonationBatchByDepositId(12424)).Returns((DonationBatch)null);
            Assert.IsNull(_fixture.GetDonationBatchByDepositId(12424));
            _mpDonationService.VerifyAll();
        }

        [Test]
        public void TestGetDonationBatchByDepositId()
        {
            _mpDonationService.Setup(mocked => mocked.GetDonationBatchByDepositId(12424)).Returns(new DonationBatch
            {
                Id = 123,
                DepositId = 12424,
            });
            var result = _fixture.GetDonationBatchByDepositId(12424);
            _mpDonationService.VerifyAll();
            Assert.IsNotNull(result);
            Assert.AreEqual(123, result.Id);
            Assert.AreEqual(12424, result.DepositId);
        }

        [Test]
        public void TestGetSelectedDonationBatches()
        {
            _mpDonationService.Setup(mocked => mocked.GetSelectedDonationBatches(12424, "afdasfsafd")).Returns(MockDepositList);

            var result = _fixture.GetSelectedDonationBatches(12424, "afdasfsafd");
            _mpDonationService.VerifyAll();
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, 2);
            Assert.AreEqual(DateTime.Parse("2/12/2015"), result[1].DepositDateTime);
            Assert.AreEqual(456, result[0].Id);
        }

        private List<Deposit> MockDepositList()
        {
            return new List<Deposit>
            {
                new Deposit
                {
                    DepositDateTime = DateTime.Parse("12/01/2010"),
                    DepositName = "Test Deposit Name 1",
                    Id = 456,
                    DepositTotalAmount = Convert.ToDecimal(7829.00),
                    BatchCount = 1,
                    Exported = false,
                    ProcessorTransferId = "1233"
                },
                new Deposit
                {
                    DepositDateTime = DateTime.Parse("2/12/2015"),
                    DepositName = "Test Deposit Name 2",
                    Id = 4557657,
                    DepositTotalAmount = Convert.ToDecimal(4.00),
                    BatchCount = 11,
                    Exported = false,
                    ProcessorTransferId = "12325523"
                }
            };
        }

        [Test]
        public void TestUpdateDonationByIdWithOptionalParameters()
        {
            var d = DateTime.Now.AddDays(-1);
            _mpDonationService.Setup(mocked => mocked.UpdateDonationStatus(123, 4, d, "note")).Returns(456);
            var response = _fixture.UpdateDonationStatus(123, 4, d, "note");
            Assert.AreEqual(456, response);
            _mpDonationService.VerifyAll();
        }

        [Test]
        public void TestUpdateDonationByIdWithoutOptionalParameters()
        {
            _mpDonationService.Setup(mocked => mocked.UpdateDonationStatus(123, 4, It.IsNotNull<DateTime>(), null)).Returns(456);
            var response = _fixture.UpdateDonationStatus(123, 4, null);
            Assert.AreEqual(456, response);
            _mpDonationService.VerifyAll();
        }

        [Test]
        public void TestUpdateDonationByProcessorIdWithOptionalParameters()
        {
            var d = DateTime.Now.AddDays(-1);
            _mpDonationService.Setup(mocked => mocked.UpdateDonationStatus("ch_123", 4, d, "note")).Returns(456);
            var response = _fixture.UpdateDonationStatus("ch_123", 4, d, "note");
            Assert.AreEqual(456, response);
            _mpDonationService.VerifyAll();
        }

        [Test]
        public void TestUpdateDonationByProcessorIdWithoutOptionalParameters()
        {
            _mpDonationService.Setup(mocked => mocked.UpdateDonationStatus("ch_123", 4, It.IsNotNull<DateTime>(), null)).Returns(456);
            var response = _fixture.UpdateDonationStatus("ch_123", 4, null);
            Assert.AreEqual(456, response);
            _mpDonationService.VerifyAll();
        }

        [Test]
        public void TestCreateDonationBatch()
        {
            var dto = new DonationBatchDTO
            {
                DepositId = 123,
                BatchEntryType = 2,
                BatchName = "batch name",
                BatchTotalAmount = 456.78M,
                FinalizedDateTime = DateTime.Now,
                ItemCount = 5,
                SetupDateTime = DateTime.Now,
                ProcessorTransferId = "transfer 1",
                Id = 999 // Should be overwritten in service
            };
            dto.Donations.Add(new DonationDTO { Id = "102030"});
            _mpDonationService.Setup(
                mocked =>
                    mocked.CreateDonationBatch(dto.BatchName, dto.SetupDateTime, dto.BatchTotalAmount, dto.ItemCount,
                        dto.BatchEntryType, dto.DepositId, dto.FinalizedDateTime, dto.ProcessorTransferId)).Returns(987);
            _mpDonationService.Setup(mocked => mocked.AddDonationToBatch(987, 102030));

            var response = _fixture.CreateDonationBatch(dto);
            _mpDonationService.VerifyAll();
            Assert.AreSame(dto, response);
            Assert.AreEqual(987, response.Id);
        }

        [Test]
        public void TestCreateDeposit()
        {
            var dto = new DepositDTO
            {
                AccountNumber = "8675309",
                BatchCount = 5,
                DepositDateTime = DateTime.Now,
                DepositName = "deposit name",
                DepositTotalAmount = 456.78M,
                Exported = true,
                Notes = "blah blah blah",
                ProcessorTransferId = "transfer 1",
                Id = 123 // should be overwritten in service
            };

            _mpDonationService.Setup(
                mocked =>
                    mocked.CreateDeposit(dto.DepositName, dto.DepositTotalAmount, dto.DepositAmount, dto.ProcessorFeeTotal, dto.DepositDateTime, dto.AccountNumber,
                        dto.BatchCount, dto.Exported, dto.Notes, dto.ProcessorTransferId)).Returns(987);

            var response = _fixture.CreateDeposit(dto);
            _mpDonationService.VerifyAll();
            Assert.AreSame(dto, response);
            Assert.AreEqual(987, response.Id);
        }

        [Test]
        public void TestCreatePaymentProcessorEventError()
        {
            var stripeEvent = new StripeEvent
            {
                Created = DateTime.Now,
                Data = new StripeEventData
                {
                    Object = new StripeTransfer
                    {
                        Amount = 1000
                    }
                },
                LiveMode = true,
                Id = "123",
                Type = "transfer.paid"
            };

            var stripeEventResponse = new StripeEventResponseDTO
            {
                Exception = new ApplicationException()
            };

            var eventString = JsonConvert.SerializeObject(stripeEvent, Formatting.Indented);
            var responseString = JsonConvert.SerializeObject(stripeEventResponse, Formatting.Indented);

            _mpDonationService.Setup(
                mocked =>
                    mocked.CreatePaymentProcessorEventError(stripeEvent.Created, stripeEvent.Id, stripeEvent.Type,
                        eventString, responseString));

            _fixture.CreatePaymentProcessorEventError(stripeEvent, stripeEventResponse);
            _mpDonationService.VerifyAll();
        }

        [Test]
        public void TestGPExportFileName()
        {
            var date = DateTime.Today;
            var fileName = string.Format("XRDReceivables-Test_Batch_Name_{0}{1}{2}.txt", date.ToString("yy"), date.ToString("MM"), date.ToString("dd"));

            _mpDonationService.Setup(mocked => mocked.GetDonationBatchByDepositId(456)).Returns(new DonationBatch
            {
                Id = 123,
                DepositId = 456,
                ProcessorTransferId = "789",
                BatchName = "Test Batch Name",
            });
            var result = _fixture.GPExportFileName(456);

            _mpDonationService.VerifyAll();
            Assert.AreEqual(fileName, result);
        }

        [Test]
        public void TestGenerateGPExportFileNames()
        {
            var date = DateTime.Today;
            var fileName = string.Format("XRDReceivables-Test_BatchName_{0}{1}{2}.txt", date.ToString("yy"), date.ToString("MM"), date.ToString("dd"));

            _mpDonationService.Setup(mocked => mocked.GetSelectedDonationBatches(12424, "afdasfsafd")).Returns(MockDepositList);
            _mpDonationService.Setup(mocked => mocked.GetDonationBatchByDepositId(456)).Returns(new DonationBatch
            {
                Id = 123,
                DepositId = 456,
                ProcessorTransferId = "789",
                BatchName = "Test BatchName",
            });
            _mpDonationService.Setup(mocked => mocked.GetDonationBatchByDepositId(4557657)).Returns(new DonationBatch
            {
                Id = 1212213,
                DepositId = 4557657,
                ProcessorTransferId = "7846469",
                BatchName = "TestBatchName2",
            });

            var results = _fixture.GenerateGPExportFileNames(12424, "afdasfsafd");

            _mpDonationService.VerifyAll();
            Assert.AreEqual(fileName, results[0].ExportFileName);
        }

        [Test]
        public void TestGetGPExport()
        {
            const int depositId = 789;
            var mockedExport = MockGPExport();
            var expectedReturn = MockExpectedGPExportDTO();

            _mpDonationService.Setup(mocked => mocked.GetGPExport(depositId, It.IsAny<string>())).Returns(mockedExport);

            var result = _fixture.GetGPExport(depositId, "asdfafasdfas");

            _mpDonationService.VerifyAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(expectedReturn[0].DocumentType, mockedExport[0].DocumentType);
            Assert.AreEqual(expectedReturn[0].DocumentNumber, mockedExport[0].DonationId);
            Assert.AreEqual(expectedReturn[0].DocumentDescription, mockedExport[0].BatchName);
            Assert.AreEqual(expectedReturn[0].BatchId, mockedExport[0].BatchName);
            Assert.AreEqual(expectedReturn[0].ContributionDate, mockedExport[0].DonationDate.ToString("MM/dd/yyyy"));
            Assert.AreEqual(expectedReturn[0].SettlementDate, mockedExport[0].DepositDate.ToString("MM/dd/yyyy"));
            Assert.AreEqual(expectedReturn[0].CustomerId, mockedExport[0].CustomerId);
            Assert.AreEqual(expectedReturn[0].ContributionAmount, mockedExport[0].DonationAmount);
            Assert.AreEqual(expectedReturn[0].CheckbookId, mockedExport[0].CheckbookId);
            Assert.AreEqual(expectedReturn[0].CashAccount, mockedExport[0].ScholarshipExpenseAccount);
            Assert.AreEqual(expectedReturn[0].ReceivablesAccount, mockedExport[0].ReceivableAccount);
            Assert.AreEqual(expectedReturn[0].DistributionAccount, mockedExport[0].DistributionAccount);
            Assert.AreEqual(expectedReturn[0].DistributionAmount, mockedExport[0].Amount);
            Assert.AreEqual(expectedReturn[0].DistributionReference, "Processor Fees " + mockedExport[0].DonationDate);
            Assert.AreEqual(expectedReturn[0].CashAccount, mockedExport[0].ScholarshipExpenseAccount);
            Assert.AreEqual(expectedReturn[1].DistributionReference, "Contribution " + mockedExport[1].DonationDate);
            Assert.AreEqual(expectedReturn[1].CashAccount, mockedExport[1].CashAccount);

            Assert.AreEqual(expectedReturn[0].DocumentType, result[0].DocumentType);
            Assert.AreEqual(expectedReturn[0].DocumentNumber, result[0].DocumentNumber);
            Assert.AreEqual(expectedReturn[0].DocumentDescription, result[0].DocumentDescription);
            Assert.AreEqual(expectedReturn[0].BatchId, result[0].BatchId);
            Assert.AreEqual(expectedReturn[0].ContributionDate, result[0].ContributionDate);
            Assert.AreEqual(expectedReturn[0].SettlementDate, result[0].SettlementDate);
            Assert.AreEqual(expectedReturn[0].CustomerId, result[0].CustomerId);
            Assert.AreEqual(expectedReturn[0].ContributionAmount, result[0].ContributionAmount);
            Assert.AreEqual(expectedReturn[0].CheckbookId, result[0].CheckbookId);
            Assert.AreEqual(expectedReturn[0].CashAccount, result[0].CashAccount);
            Assert.AreEqual(expectedReturn[0].ReceivablesAccount, result[0].ReceivablesAccount);
            Assert.AreEqual(expectedReturn[0].DistributionAccount, result[0].DistributionAccount);
            Assert.AreEqual(expectedReturn[0].DistributionAmount, result[0].DistributionAmount);
            Assert.AreEqual(expectedReturn[0].DistributionReference, result[0].DistributionReference);
            Assert.AreEqual(expectedReturn[0].CashAccount, result[0].CashAccount);
            Assert.AreEqual(expectedReturn[1].DistributionReference, result[1].DistributionReference);
        }

        private List<GPExportDatumDTO> MockExpectedGPExportDTO()
        {
            return new List<GPExportDatumDTO>
            {
                new GPExportDatumDTO
                {
                    DocumentType = "SALE",
                    DocumentNumber = 10002,
                    DocumentDescription = "Test Batch",
                    BatchId = "Test Batch",
                    ContributionDate = new DateTime(2015, 3, 28, 8, 30, 0).ToString("MM/dd/yyyy"),
                    SettlementDate = new DateTime(2015, 3, 28, 8, 30, 0).ToString("MM/dd/yyyy"),
                    CustomerId = "CONTRIBUTI001",
                    ContributionAmount = "200.00",
                    CheckbookId = "PNC001",
                    CashAccount = "90551-031-02",
                    ReceivablesAccount = "90013-031-21",
                    DistributionAccount = "90001-031-22",
                    DistributionAmount = "200.00",
                    DistributionReference = "Processor Fees " + new DateTime(2015, 3, 28, 8, 30, 0)
                },
                new GPExportDatumDTO
                {
                    DocumentType = "SALE",
                    DocumentNumber = 10002,
                    DocumentDescription = "Test 2 Batch",
                    BatchId = "Test 2 Batch",
                    ContributionDate = new DateTime(2014, 3, 28, 8, 30, 0).ToString("MM/dd/yyyy"),
                    SettlementDate = new DateTime(2014, 3, 28, 8, 30, 0).ToString("MM/dd/yyyy"),
                    CustomerId = "CONTRIBUTI001",
                    ContributionAmount = "20.00",
                    CheckbookId = "PNC001",
                    CashAccount = "91213-031-20",
                    ReceivablesAccount = "90013-031-21",
                    DistributionAccount = "90001-031-22",
                    DistributionAmount = "20.00",
                    DistributionReference = "Contribution " + new DateTime(2014, 3, 28, 8, 30, 0)
                }
            };
        }

        private List<GPExportDatum> MockGPExport()
        {
            return new List<GPExportDatum>
            {
                new GPExportDatum
                {
                    DocumentType = "SALE",
                    DonationId = 10002,
                    BatchName = "Test Batch",
                    DonationDate = new DateTime(2015, 3, 28, 8, 30, 0),
                    DepositDate = new DateTime(2015, 3, 28, 8, 30, 0),
                    CustomerId = "CONTRIBUTI001",
                    DonationAmount = "200.00",
                    CheckbookId = "PNC001",
                    CashAccount = "91213-031-20",
                    ReceivableAccount = "90013-031-21",
                    DistributionAccount = "90001-031-22",
                    Amount = "200.00",
                    ProgramId = 12,
                    ProccessFeeProgramId = 12,
                    PaymentTypeId = 9,
                    ScholarshipExpenseAccount = "90551-031-02",
                    ScholarshipPaymentTypeId = 9,
                },
                new GPExportDatum
                {
                    DocumentType = "SALE",
                    DonationId = 10002,
                    BatchName = "Test Batch",
                    DonationDate = new DateTime(2014, 3, 28, 8, 30, 0),
                    DepositDate = new DateTime(2014, 3, 28, 8, 30, 0),
                    CustomerId = "CONTRIBUTI001",
                    DonationAmount = "20.00",
                    CheckbookId = "PNC001",
                    CashAccount = "91213-031-20",
                    ReceivableAccount = "90013-031-21",
                    DistributionAccount = "90001-031-22",
                    Amount = "20.00",
                    ProgramId = 112,
                    ProccessFeeProgramId = 12,
                    PaymentTypeId = 15,
                    ScholarshipExpenseAccount = "90551-031-02",
                    ScholarshipPaymentTypeId = 9,
                }
            };
        }
    
        [Test]
        public void TestGetDonationsForDonor()
        {
            var donations = new List<Donation>
            {
                new Donation
                {
                    donationAmt = 123,
                    donationId = 45,
                    donationDate = DateTime.Parse("1999-12-31 23:59:59"),
                    paymentTypeId = 2, // Cash
                },
                new Donation
                {
                    donationAmt = 567,
                    donationId = 67,
                    donationDate = DateTime.Parse("1999-11-30 23:59:59"),
                    paymentTypeId = 5, //bank
                    transactionCode = "tx_67"
                },
                new Donation
                {
                    donationAmt = 678,
                    donationId = 78,
                    donationDate = DateTime.Parse("1999-10-30 23:59:59"),
                    paymentTypeId = 4, //bank
                    transactionCode = "tx_78"
                }
            };

            var donor = new ContactDonor
            {
                ContactId = 987,
                DonorId = 123,
                StatementTypeId = 456,
                Details = new ContactDetails
                {
                    HouseholdId = 901
                }
            };

            var household = new Household
            {
                HouseholdMembers = new List<HouseholdMember>
                {
                    new HouseholdMember
                    {
                        DonorId = 678,
                        StatementTypeId = 456
                    },
                    new HouseholdMember
                    {
                        DonorId = 123,
                        StatementTypeId = 456
                    },
                    new HouseholdMember
                    {
                        DonorId = 444,
                        StatementTypeId = 455
                    },
                    new HouseholdMember
                    {
                        DonorId = 345,
                        StatementTypeId = 456
                    }
                }
            };
            _contactService.Setup(mocked => mocked.GetHouseholdById(901)).Returns(household);
            _mpDonorService.Setup(mocked => mocked.GetEmailViaDonorId(123)).Returns(donor);
            _mpDonorService.Setup(mocked => mocked.GetDonations(new [] {123, 678, 345}, "1999")).Returns(donations);
            _paymentService.Setup(mocked => mocked.GetCharge("tx_67")).Returns(new StripeCharge
            {
                Source = new StripeSource
                {
                    AccountNumberLast4 = "9876"
                }
            });
            _paymentService.Setup(mocked => mocked.GetCharge("tx_78")).Returns(new StripeCharge
            {
                Source = new StripeSource
                {
                    AccountNumberLast4 = "8765",
                    Brand = CardBrand.AmericanExpress
                }
            });
            var response = _fixture.GetDonationsForDonor(123, "1999", false);
            _mpDonorService.VerifyAll();
            _paymentService.VerifyAll();

            Assert.NotNull(response);
            Assert.NotNull(response.Donations);
            Assert.AreEqual(3, response.Donations.Count);
            Assert.AreEqual(donations[0].donationAmt + donations[1].donationAmt + donations[2].donationAmt, response.DonationTotalAmount);

            Assert.AreEqual(donations[2].donationDate, response.Donations[0].DonationDate);
            Assert.AreEqual("8765", response.Donations[0].Source.AccountNumberLast4);
            Assert.AreEqual(CreditCardType.AmericanExpress, response.Donations[0].Source.CardType);

            Assert.AreEqual(donations[1].donationDate, response.Donations[1].DonationDate);
            Assert.AreEqual("9876", response.Donations[1].Source.AccountNumberLast4);

            Assert.AreEqual(donations[0].donationDate, response.Donations[2].DonationDate);
            Assert.AreEqual("cash", response.Donations[2].Source.Name);
        }

        [Test]
        public void TestGetDonationsForAuthenticatedUser()
        {
            var donations = new List<Donation>
            {
                new Donation
                {
                    donationAmt = 123,
                    donationId = 45,
                    donationDate = DateTime.Parse("1999-12-31 23:59:59"),
                    paymentTypeId = 2, // Cash,
                    softCreditDonorId = 0,
                },
                new Donation
                {
                    donationAmt = 567,
                    donationId = 67,
                    donationDate = DateTime.Parse("1999-11-30 23:59:59"),
                    paymentTypeId = 5, //bank
                    transactionCode = "tx_67",
                    softCreditDonorId = 0,
                },
                new Donation
                {
                    donationAmt = 678,
                    donationId = 78,
                    donationDate = DateTime.Parse("1999-10-30 23:59:59"),
                    paymentTypeId = 4, // credit card
                    transactionCode = "tx_78",
                    softCreditDonorId = 0,
                }
            };
            _mpAuthenticationService.Setup(mocked => mocked.GetContactId("auth token")).Returns(90210);
            _mpDonorService.Setup(mocked => mocked.GetContactDonor(90210)).Returns(new ContactDonor
            {
                ContactId = 90210,
                DonorId = 123,
                StatementTypeId = 456
            });
            _mpDonorService.Setup(mocked => mocked.GetDonations(new [] {123}, "1999")).Returns(donations);
            _paymentService.Setup(mocked => mocked.GetCharge("tx_67")).Returns(new StripeCharge
            {
                Source = new StripeSource
                {
                    AccountNumberLast4 = "9876"
                }
            });
            _paymentService.Setup(mocked => mocked.GetCharge("tx_78")).Returns(new StripeCharge
            {
                Source = new StripeSource
                {
                    AccountNumberLast4 = "8765",
                    Brand = CardBrand.AmericanExpress
                }
            });
            var response = _fixture.GetDonationsForAuthenticatedUser("auth token", "1999");
            _mpAuthenticationService.VerifyAll();
            _mpDonorService.VerifyAll();
            _paymentService.VerifyAll();

            Assert.NotNull(response);
            Assert.NotNull(response.Donations);
            Assert.AreEqual(3, response.Donations.Count);
            Assert.AreEqual(donations[0].donationAmt + donations[1].donationAmt + donations[2].donationAmt, response.DonationTotalAmount);

            Assert.AreEqual(donations[2].donationDate, response.Donations[0].DonationDate);
            Assert.AreEqual("8765", response.Donations[0].Source.AccountNumberLast4);
            Assert.AreEqual(CreditCardType.AmericanExpress, response.Donations[0].Source.CardType);

            Assert.AreEqual(donations[1].donationDate, response.Donations[1].DonationDate);
            Assert.AreEqual("9876", response.Donations[1].Source.AccountNumberLast4);

            Assert.AreEqual(donations[0].donationDate, response.Donations[2].DonationDate);
            Assert.AreEqual("cash", response.Donations[2].Source.Name);
        }

        [Test]
        public void TestGetDonationYearsForDonor()
        {
            var donations = new List<Donation>
            {
                new Donation
                {
                    donationDate = DateTime.Parse("1999-12-31 23:59:59"),
                },
                new Donation
                {
                    donationDate = DateTime.Parse("2000-01-01 00:00:01"),
                },
                new Donation
                {
                    donationDate = DateTime.Parse("1999-11-30 23:59:59"),
                },
                new Donation
                {
                    donationDate = DateTime.Parse("1998-10-30 23:59:59"),
                }
            };

            var softCreditDonations = new List<Donation>
            {
                new Donation
                {
                    donationDate = DateTime.Parse("1997-12-31 23:59:59"),
                },
                new Donation
                {
                    donationDate = DateTime.Parse("2001-01-01 00:00:01"),
                },
                new Donation
                {
                    donationDate = DateTime.Parse("1999-11-30 23:59:59"),
                },
                new Donation
                {
                    donationDate = DateTime.Parse("1997-11-30 23:59:59"),
                },
                new Donation
                {
                    donationDate = DateTime.Parse("1996-10-30 23:59:59"),
                }
            };

            var expectedYears = new List<string>
            {
                "2001",
                "2000",
                "1999",
                "1998",
                "1997",
                "1996"
            };

            var donor = new ContactDonor
            {
                ContactId = 987,
                DonorId = 123,
                StatementTypeId = 456
            };
            _mpDonorService.Setup(mocked => mocked.GetEmailViaDonorId(123)).Returns(donor);
            _mpDonorService.Setup(mocked => mocked.GetDonations(new [] {123}, null)).Returns(donations);
            _mpDonorService.Setup(mocked => mocked.GetSoftCreditDonations(new [] {123}, null)).Returns(softCreditDonations);

            var response = _fixture.GetDonationYearsForDonor(123);
            _mpDonorService.VerifyAll();

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.AvailableDonationYears);
            Assert.AreEqual(expectedYears.Count, response.AvailableDonationYears.Count);
        }

        [Test]
        public void TestGetDonationYearsForAuthenticatedUser()
        {
            var donations = new List<Donation>
            {
                new Donation
                {
                    donationDate = DateTime.Parse("1999-12-31 23:59:59"),
                },
                new Donation
                {
                    donationDate = DateTime.Parse("2000-01-01 00:00:01"),
                },
                new Donation
                {
                    donationDate = DateTime.Parse("1999-11-30 23:59:59"),
                },
                new Donation
                {
                    donationDate = DateTime.Parse("1998-10-30 23:59:59"),
                }
            };

            var softCreditDonations = new List<Donation>
            {
                new Donation
                {
                    donationDate = DateTime.Parse("1997-12-31 23:59:59"),
                },
                new Donation
                {
                    donationDate = DateTime.Parse("2001-01-01 00:00:01"),
                },
                new Donation
                {
                    donationDate = DateTime.Parse("1999-11-30 23:59:59"),
                },
                new Donation
                {
                    donationDate = DateTime.Parse("1997-11-30 23:59:59"),
                },
                new Donation
                {
                    donationDate = DateTime.Parse("1996-10-30 23:59:59"),
                }
            };

            var expectedYears = new List<string>
            {
                "2001",
                "2000",
                "1999",
                "1998",
                "1997",
                "1996"
            };

            _mpAuthenticationService.Setup(mocked => mocked.GetContactId("auth token")).Returns(90210);
            _mpDonorService.Setup(mocked => mocked.GetContactDonor(90210)).Returns(new ContactDonor
            {
                ContactId = 90210,
                DonorId = 123,
                StatementTypeId = 456
            });

            _mpDonorService.Setup(mocked => mocked.GetDonations(new [] {123}, null)).Returns(donations);
            _mpDonorService.Setup(mocked => mocked.GetSoftCreditDonations(new [] {123}, null)).Returns(softCreditDonations);

            var response = _fixture.GetDonationYearsForAuthenticatedUser("auth token");
            _mpDonorService.VerifyAll();

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.AvailableDonationYears);
            Assert.AreEqual(expectedYears.Count, response.AvailableDonationYears.Count);
        }

        [Test]
        public void TestSoftCreditGetDonationsForDonor()
        {
            var donations = new List<Donation>
            {
                new Donation
                {
                    donationAmt = 123,
                    donationId = 45,
                    donationDate = DateTime.Parse("1999-12-31 23:59:59"),
                    paymentTypeId = 2, // Cash
                    softCreditDonorId = 123,
                    donorDisplayName = "Fidelity",
                },
                new Donation
                {
                    donationAmt = 567,
                    donationId = 67,
                    donationDate = DateTime.Parse("1999-11-30 23:59:59"),
                    paymentTypeId = 5, //bank
                    transactionCode = "tx_67",
                    softCreditDonorId = 123,
                    donorDisplayName = "US Bank",
                },
                new Donation
                {
                    donationAmt = 678,
                    donationId = 78,
                    donationDate = DateTime.Parse("1999-10-30 23:59:59"),
                    paymentTypeId = 4, //bank
                    transactionCode = "tx_78",
                    softCreditDonorId = 123,
                    donorDisplayName = "Citi",
                }
            };

            var donor = new ContactDonor
            {
                ContactId = 987,
                DonorId = 123,
                StatementTypeId = 456,
                Details = new ContactDetails
                {
                    HouseholdId = 901
                }
            };

            var household = new Household
            {
                HouseholdMembers = new List<HouseholdMember>
                {
                    new HouseholdMember
                    {
                        DonorId = 678,
                        StatementTypeId = 456
                    },
                    new HouseholdMember
                    {
                        DonorId = 123,
                        StatementTypeId = 456
                    },
                    new HouseholdMember
                    {
                        DonorId = 444,
                        StatementTypeId = 455
                    },
                    new HouseholdMember
                    {
                        DonorId = 345,
                        StatementTypeId = 456
                    }
                }
            };
            _contactService.Setup(mocked => mocked.GetHouseholdById(901)).Returns(household);
            _mpDonorService.Setup(mocked => mocked.GetEmailViaDonorId(123)).Returns(donor);
            _mpDonorService.Setup(mocked => mocked.GetSoftCreditDonations(new[] { 123, 678, 345 }, "1999")).Returns(donations);
            var response = _fixture.GetDonationsForDonor(123, "1999", true);
            _mpDonorService.VerifyAll();
            _paymentService.VerifyAll();

            Assert.NotNull(response);
            Assert.NotNull(response.Donations);
            Assert.AreEqual(3, response.Donations.Count);
            Assert.AreEqual(donations[0].donationAmt + donations[1].donationAmt + donations[2].donationAmt, response.DonationTotalAmount);

            Assert.AreEqual(donations[2].donationDate, response.Donations[0].DonationDate);
            Assert.AreEqual(null, response.Donations[0].Source.AccountNumberLast4);
            Assert.AreEqual(null, response.Donations[0].Source.CardType);
            Assert.AreEqual("Citi", response.Donations[0].Source.Name);
            Assert.AreEqual(PaymentType.SoftCredit, response.Donations[0].Source.SourceType);

            Assert.AreEqual(donations[1].donationDate, response.Donations[1].DonationDate);
            Assert.AreEqual(null, response.Donations[1].Source.AccountNumberLast4);
            Assert.AreEqual("US Bank", response.Donations[1].Source.Name);

            Assert.AreEqual(donations[0].donationDate, response.Donations[2].DonationDate);
            Assert.AreEqual("Fidelity", response.Donations[2].Source.Name);
        }

        [Test]
        public void TestSoftCreditGetDonationsForAuthenticatedUser()
        {
            var donations = new List<Donation>
            {
                new Donation
                {
                    donationAmt = 123,
                    donationId = 45,
                    donationDate = DateTime.Parse("1999-12-31 23:59:59"),
                    paymentTypeId = 2, // Cash
                    softCreditDonorId = 123,
                    donorDisplayName = "Fidelity",
                },
                new Donation
                {
                    donationAmt = 567,
                    donationId = 67,
                    donationDate = DateTime.Parse("1999-11-30 23:59:59"),
                    paymentTypeId = 5, //bank
                    transactionCode = "tx_67",
                    softCreditDonorId = 123,
                    donorDisplayName = "US Bank",
                },
                new Donation
                {
                    donationAmt = 678,
                    donationId = 78,
                    donationDate = DateTime.Parse("1999-10-30 23:59:59"),
                    paymentTypeId = 4, // credit card
                    transactionCode = "tx_78",
                    softCreditDonorId = 123,
                    donorDisplayName = "Citi",
                }
            };
            _mpAuthenticationService.Setup(mocked => mocked.GetContactId("auth token")).Returns(90210);
            _mpDonorService.Setup(mocked => mocked.GetContactDonor(90210)).Returns(new ContactDonor
            {
                ContactId = 90210,
                DonorId = 123,
                StatementTypeId = 456
            });
            _mpDonorService.Setup(mocked => mocked.GetSoftCreditDonations(new[] { 123 }, "1999")).Returns(donations);
            var response = _fixture.GetDonationsForAuthenticatedUser("auth token", "1999", true);
            _mpAuthenticationService.VerifyAll();
            _mpDonorService.VerifyAll();
            _paymentService.VerifyAll();

            Assert.NotNull(response);
            Assert.NotNull(response.Donations);
            Assert.AreEqual(3, response.Donations.Count);
            Assert.AreEqual(donations[0].donationAmt + donations[1].donationAmt + donations[2].donationAmt, response.DonationTotalAmount);

            Assert.AreEqual(donations[2].donationDate, response.Donations[0].DonationDate);
            Assert.AreEqual(null, response.Donations[0].Source.AccountNumberLast4);
            Assert.AreEqual(null, response.Donations[0].Source.CardType);
            Assert.AreEqual("Citi", response.Donations[0].Source.Name);

            Assert.AreEqual(donations[1].donationDate, response.Donations[1].DonationDate);
            Assert.AreEqual(null, response.Donations[1].Source.AccountNumberLast4);
            Assert.AreEqual("US Bank", response.Donations[1].Source.Name);

            Assert.AreEqual(donations[0].donationDate, response.Donations[2].DonationDate);
            Assert.AreEqual("Fidelity", response.Donations[2].Source.Name);
        }
    }
}