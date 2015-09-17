using System;
using System.Collections.Generic;
using crds_angular.App_Start;
using crds_angular.DataAccess.Interfaces;
using crds_angular.Models.Crossroads.Stewardship;
using crds_angular.Services;
using crds_angular.Services.Interfaces;
using MinistryPlatform.Models;
using Moq;
using NUnit.Framework;
using MPServices=MinistryPlatform.Translation.Services.Interfaces;

namespace crds_angular.test.Services
{
    public class EzScanCheckScannerServiceTest
    {
        private EzScanCheckScannerService _fixture;
        private Mock<ICheckScannerDao> _checkScannerDao;
        private Mock<IDonorService> _donorService;
        private Mock<IPaymentService> _paymentService;
        private Mock<MPServices.IDonorService> _mpDonorService;

        [SetUp]
        public void SetUp()
        {
            _checkScannerDao = new Mock<ICheckScannerDao>(MockBehavior.Strict);
            _donorService = new Mock<IDonorService>(MockBehavior.Strict);
            _paymentService = new Mock<IPaymentService>(MockBehavior.Strict);
            _mpDonorService = new Mock<MPServices.IDonorService>(MockBehavior.Strict);
            _fixture = new EzScanCheckScannerService(_checkScannerDao.Object, _donorService.Object, _paymentService.Object, _mpDonorService.Object);
            AutoMapperConfig.RegisterMappings();
        }

        [Test]
        public void TestGetOpenBatches()
        {
            var batches = new List<CheckScannerBatch>();
            _checkScannerDao.Setup(mocked => mocked.GetBatches(true)).Returns(batches);

            var result = _fixture.GetBatches();
            _checkScannerDao.VerifyAll();

            Assert.IsNotNull(result);
            Assert.AreSame(batches, result);
        }

        [Test]
        public void TestGetAllBatches()
        {
            var batches = new List<CheckScannerBatch>();
            _checkScannerDao.Setup(mocked => mocked.GetBatches(false)).Returns(batches);

            var result = _fixture.GetBatches(false);
            _checkScannerDao.VerifyAll();

            Assert.IsNotNull(result);
            Assert.AreSame(batches, result);
        }

        [Test]
        public void TestGetChecksForBatch()
        {
            var checks = new List<CheckScannerCheck>();

            _checkScannerDao.Setup(m => m.GetChecksForBatch("batch123")).Returns(checks);

            var result = _fixture.GetChecksForBatch("batch123");
            _checkScannerDao.VerifyAll();

            Assert.IsNotNull(result);
            Assert.AreSame(checks, result);
        }

        [Test]
        public void TestUpdateBatchStatus()
        {
            var batch = new CheckScannerBatch
            {
                Status = BatchStatus.NotExported
            };
            _checkScannerDao.Setup(mocked => mocked.UpdateBatchStatus("batch123", BatchStatus.Exported)).Returns(batch);

            var result = _fixture.UpdateBatchStatus("batch123", BatchStatus.Exported);
            _checkScannerDao.VerifyAll();

            Assert.IsNotNull(result);
            Assert.AreSame(batch, result);
        }

        [Test]
        public void TestCreateDonationsForBatch()
        {
            var checks = new List<CheckScannerCheck>
            {
                new CheckScannerCheck
                {
                    Id = 11111,
                    AccountNumber = "111",
                    Address = new Address
                    {
                        Line1 = "1 line 1",
                        Line2 = "1 line 2",
                        City = "1 city",
                        State = "1 state",
                        PostalCode = "1 postal"
                    },
                    Amount = 1111,
                    CheckDate = DateTime.Now.AddHours(1),
                    CheckNumber = "11111",
                    Name1 = "1 name 1",
                    Name2 = "1 name 2",
                    RoutingNumber = "1010",
                    ScanDate = DateTime.Now.AddHours(2)
                },
                new CheckScannerCheck
                {
                    Id = 22222,
                    AccountNumber = "222",
                    Address = new Address
                    {
                        Line1 = "2 line 1",
                        Line2 = "2 line 2",
                        City = "2 city",
                        State = "2 state",
                        PostalCode = "2 postal"
                    },
                    Amount = 2222,
                    CheckDate = DateTime.Now.AddHours(3),
                    CheckNumber = "22222",
                    Name1 = "2 name 1",
                    Name2 = "2 name 2",
                    RoutingNumber = "2020",
                    ScanDate = DateTime.Now.AddHours(4)
                }
            };

            var contactDonorExisting = new ContactDonor
            {
                ProcessorId = "111000111",
                DonorId = 111111,
                RegisteredUser = true,
                Account = new DonorAccount
                {
                    ProcessorAccountId = "py_dgsttety6737hjjhweiu3"
                }
            };
            var encryptedKey = "PH/rty1234";
          
            _checkScannerDao.Setup(mocked => mocked.GetChecksForBatch("batch123")).Returns(checks);
            _checkScannerDao.Setup(mocked => mocked.UpdateBatchStatus("batch123", BatchStatus.Exported)).Returns(new CheckScannerBatch());
            _checkScannerDao.Setup(mocked => mocked.UpdateCheckStatus(11111, true, null));
            _checkScannerDao.Setup(mocked => mocked.UpdateCheckStatus(22222, true, null));

            _donorService.Setup(mocked => mocked.GetContactDonorForDonorAccount(checks[0].AccountNumber, checks[0].RoutingNumber)).Returns(contactDonorExisting);
            _paymentService.Setup(mocked => mocked.ChargeCustomer(contactDonorExisting.ProcessorId, contactDonorExisting.Account.ProcessorAccountId, (int) checks[0].Amount, contactDonorExisting.DonorId)).Returns(new StripeCharge
            {
                Id = "1020304",
                Source = new SourceData
                {
                    id = "py_dgsttety6737hjjhweiu3"
                },
                BalanceTransaction = new StripeBalanceTransaction
                {
                    Fee = 123
                }
            });
            _mpDonorService.Setup(mocked => mocked.CreateEncodedAndEncryptedAccountAndRoutingNumber(checks[0].AccountNumber, checks[0].RoutingNumber)).Returns(encryptedKey);
            _mpDonorService.Setup(mocked => mocked.UpdateDonorAccount(encryptedKey, contactDonorExisting.Account.ProcessorAccountId, contactDonorExisting.ProcessorId));
            _mpDonorService.Setup(
                mocked =>
                    mocked.CreateDonationAndDistributionRecord((int) checks[0].Amount,
                                                               123,
                                                               contactDonorExisting.DonorId,
                                                               "9090",
                                                               "1020304",
                                                               "check",
                                                               contactDonorExisting.ProcessorId,
                                                               It.IsAny<DateTime>(),
                                                               true, "batch123")).Returns(321);

            var contactDonorNew = new ContactDonor
            {
                ProcessorId = "222000222",
                DonorId = 222222,
                RegisteredUser = false,
                Account = new DonorAccount
                {
                    ProcessorAccountId = "cus_123456789"
                }
            };

            var mockCharge = new StripeCharge
            {
                Id = "40302010",
                Source = new SourceData
                {
                    id = "ba_dgsttety6737hjjhweiu3"
                }
            };

            _donorService.Setup(mocked => mocked.GetContactDonorForDonorAccount(checks[1].AccountNumber, checks[1].RoutingNumber)).Returns((ContactDonor) null);
            _paymentService.Setup(mocked => mocked.CreateToken(checks[1].AccountNumber, checks[1].RoutingNumber)).Returns("tok123");
            _donorService.Setup(
                mocked =>
                    mocked.CreateOrUpdateContactDonor(
                        It.Is<ContactDonor>(
                            o =>
                                o.Details.DisplayName.Equals("2 name 1") && o.Details.Address.Line1.Equals("2 line 1") && o.Details.Address.Line2.Equals("2 line 2") &&
                                o.Details.Address.City.Equals("2 city") && o.Details.Address.State.Equals("2 state") && o.Details.Address.PostalCode.Equals("2 postal") &&
                                o.Account.RoutingNumber.Equals("2020") && o.Account.AccountNumber.Equals("222") && o.Account.Type == AccountType.Checking),
                        It.IsAny<string>(),
                        string.Empty,
                        "tok123",
                        It.IsAny<DateTime>()))
                .Returns(contactDonorNew);
            _paymentService.Setup(mocked => mocked.ChargeCustomer(contactDonorNew.ProcessorId, contactDonorNew.Account.ProcessorAccountId, (int)checks[1].Amount, contactDonorNew.DonorId)).Returns(mockCharge);

            _mpDonorService.Setup(mocked => mocked.CreateEncodedAndEncryptedAccountAndRoutingNumber(checks[1].AccountNumber, checks[1].RoutingNumber)).Returns(encryptedKey);
            _mpDonorService.Setup(mocked => mocked.UpdateDonorAccount(encryptedKey, mockCharge.Source.id, contactDonorNew.ProcessorId));

            _mpDonorService.Setup(
                mocked =>
                    mocked.CreateDonationAndDistributionRecord((int)checks[1].Amount,
                                                               null,
                                                               contactDonorNew.DonorId,
                                                               "9090",
                                                               "40302010",
                                                               "check",
                                                               contactDonorNew.ProcessorId,
                                                               It.IsAny<DateTime>(),
                                                               false, "batch123")).Returns(654);



            var result = _fixture.CreateDonationsForBatch(new CheckScannerBatch
            {
                Name = "batch123",
                ProgramId = 9090
            });
            _checkScannerDao.VerifyAll();
            _donorService.VerifyAll();
            _mpDonorService.VerifyAll();
            _paymentService.VerifyAll();
            Assert.NotNull(result);
            Assert.NotNull(result.Checks);
            Assert.AreEqual(2, result.Checks.Count);
            Assert.AreEqual(BatchStatus.Exported, result.Status);
        }

        [Test]
        public void TestGetContactDonorForCheck()
        {
            const string encryptedKey = "disCv2kF/8HlRCWeTqolok1G4imf1cNZershgmCCFDI=";
            const string addr1 = "12 Scenic Dr";
            const string addr2 = "Penthouse Suite";
            const string city = "Honolulu";
            const string state = "HI";
            const string zip = "68168-1618";
            const string displayName = "Vacationing Vera";

            var details = new ContactDetails
            {
                DisplayName = displayName,
                Address = new PostalAddress()
                {
                    Line1 = addr1,
                    Line2 = addr2,
                    City = city,
                    State = state,
                    PostalCode = zip
                }
            };

            _donorService.Setup(mocked => mocked.GetContactDonorForCheckAccount(encryptedKey)).Returns(details);
            var result = _fixture.GetContactDonorForCheck(encryptedKey);

            _donorService.VerifyAll();
            Assert.IsNotNull(details);
            Assert.AreEqual(result.DisplayName, details.DisplayName);
            Assert.AreEqual(result.Address, details.Address);
        }

        [Test]
        public void TestExistingCreateDonor()
        {
            var check = new CheckScannerCheck
            {
                Id = 11111,
                AccountNumber = "111",
                Address = new Address
                {
                    Line1 = "1 line 1",
                    Line2 = "1 line 2",
                    City = "1 city",
                    State = "1 state",
                    PostalCode = "1 postal"
                },
                Amount = 1111,
                CheckDate = DateTime.Now.AddHours(1),
                CheckNumber = "11111",
                Name1 = "1 name 1",
                Name2 = "1 name 2",
                RoutingNumber = "1010",
                ScanDate = DateTime.Now.AddHours(2)
            };

            var contactDonorExisting = new ContactDonor
            {
                ProcessorId = "111000111",
                DonorId = 111111,
                RegisteredUser = true
            };

            _donorService.Setup(mocked => mocked.GetContactDonorForDonorAccount(check.AccountNumber, check.RoutingNumber)).Returns(contactDonorExisting);

            var result = _fixture.CreateDonor(check);
            _donorService.VerifyAll();
            Assert.NotNull(result);
            Assert.AreEqual(contactDonorExisting, result);
        }

        [Test]
        public void TestCreateForCreateDonor()
        {
            var check = new CheckScannerCheck
            {
                Id = 11111,
                AccountNumber = "111",
                Address = new Address
                {
                    Line1 = "1 line 1",
                    Line2 = "1 line 2",
                    City = "1 city",
                    State = "1 state",
                    PostalCode = "1 postal"
                },
                Amount = 1111,
                CheckDate = DateTime.Now.AddHours(1),
                CheckNumber = "11111",
                Name1 = "1 name 1",
                Name2 = "1 name 2",
                RoutingNumber = "1010",
                ScanDate = DateTime.Now.AddHours(2)
            };


            var contactDonorNew = new ContactDonor
            {
                ProcessorId = "222000222",
                DonorId = 222222,
                RegisteredUser = false,
                Account = new DonorAccount
                {
                    ProcessorAccountId = "py_dgsttety6737hjjhweiu3"
                }
            };
            const string token = "12t4token";
            const string encryptedKey = "PH/rty1234";
      
            _donorService.Setup(mocked => mocked.GetContactDonorForDonorAccount(check.AccountNumber, check.RoutingNumber)).Returns((ContactDonor)null);
            _paymentService.Setup(mocked => mocked.CreateToken(check.AccountNumber, check.RoutingNumber)).Returns(token);
            _donorService.Setup(
                mocked =>
                    mocked.CreateOrUpdateContactDonor(
                        It.Is<ContactDonor>(
                            o => 
                                o.Details.DisplayName.Equals(check.Name1) && o.Details.Address.Line1.Equals(check.Address.Line1) && o.Details.Address.Line2.Equals(check.Address.Line2) &&
                                o.Details.Address.City.Equals(check.Address.City) && o.Details.Address.State.Equals(check.Address.State) && o.Details.Address.PostalCode.Equals(check.Address.PostalCode) &&
                                o.Account.RoutingNumber.Equals(check.RoutingNumber) && o.Account.AccountNumber.Equals(check.AccountNumber) && o.Account.Type == AccountType.Checking),
                        It.IsAny<string>(),
                        string.Empty,
                        token,
                        It.IsAny<DateTime>()))
                .Returns(contactDonorNew);

            _mpDonorService.Setup(mocked => mocked.CreateEncodedAndEncryptedAccountAndRoutingNumber(check.AccountNumber, check.RoutingNumber)).Returns(encryptedKey);
            _mpDonorService.Setup(mocked => mocked.UpdateDonorAccount(encryptedKey, contactDonorNew.Account.ProcessorAccountId, contactDonorNew.ProcessorId));

            var result = _fixture.CreateDonor(check);
            _donorService.VerifyAll();
            Assert.NotNull(result);
            Assert.AreEqual(contactDonorNew, result);
        }
    }
}
