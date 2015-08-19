using System;
using System.Collections.Generic;
using System.Data;
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
        private Mock<IDbConnection> _dbConnection;
        private Mock<IDonorService> _donorService;
        private Mock<IPaymentService> _paymentService;
        private Mock<MPServices.IDonorService> _mpDonorService;

        [SetUp]
        public void SetUp()
        {
            _dbConnection = new Mock<IDbConnection>(MockBehavior.Strict);
            _donorService = new Mock<IDonorService>(MockBehavior.Strict);
            _paymentService = new Mock<IPaymentService>(MockBehavior.Strict);
            _mpDonorService = new Mock<MPServices.IDonorService>(MockBehavior.Strict);
            _fixture = new EzScanCheckScannerService(_dbConnection.Object, _donorService.Object, _paymentService.Object, _mpDonorService.Object);
        }

        [Test]
        public void TestGetOpenBatches()
        {
            var dateTime1 = DateTime.Now.AddDays(1);
            var dateTime2 = DateTime.Now.AddDays(2);

            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(mocked => mocked.Read()).Returns(true).Returns(true).Returns(false);
            dataReader.SetupSequence(mocked => mocked[0]).Returns(1).Returns(2);
            dataReader.SetupSequence(mocked => mocked[1]).Returns("name1").Returns("name2");
            dataReader.SetupSequence(mocked => mocked[2]).Returns(dateTime1).Returns(dateTime2);
            dataReader.Setup(mocked => mocked.IsDBNull(3)).Returns(false);
            dataReader.SetupSequence(mocked => mocked.GetInt32(3)).Returns(0).Returns(1);

            var dbCommand = new Mock<IDbCommand>();
            dbCommand.Setup(mocked => mocked.Dispose());
            dbCommand.SetupSet(mocked => mocked.CommandType = CommandType.Text).Verifiable();
            dbCommand.SetupSet(mocked => mocked.CommandText = "SELECT ID, IDBatch, DateProcess, BatchStatus FROM batches WHERE COALESCE(BatchStatus, 0) <> 1 ORDER BY DateProcess DESC").Verifiable();
            dbCommand.Setup(mocked => mocked.ExecuteReader()).Returns(dataReader.Object);

            _dbConnection.Setup(mocked => mocked.Open());
            _dbConnection.Setup(mocked => mocked.CreateCommand()).Returns(dbCommand.Object);
            _dbConnection.Setup(mocked => mocked.Close());

            var result = _fixture.GetOpenBatches();
            _dbConnection.VerifyAll();
            dataReader.VerifyAll();
            dbCommand.VerifyAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);

            Assert.AreEqual(1, result[0].Id);
            Assert.AreEqual("name1", result[0].Name);
            Assert.AreEqual(dateTime1, result[0].ScanDate);
            Assert.AreEqual(BatchStatus.NotExported, result[0].Status);

            Assert.AreEqual(2, result[1].Id);
            Assert.AreEqual("name2", result[1].Name);
            Assert.AreEqual(dateTime2, result[1].ScanDate);
            Assert.AreEqual(BatchStatus.Exported, result[1].Status);
        }

        [Test]
        public void TestGetChecksForBatch()
        {
            const int checkId = 1;
            const string accountNumber = "123456789";
            const double amount = 5.67;
            const string checkNumber = "5150";
            var scanDate = DateTime.Now.AddDays(1);
            const string routingNumber = "OU812";
            const string name1 = "edward";
            var checkDate = DateTime.Now.AddDays(2);
            const string name2 = "alex";
            const string addressLine1 = "1234 main st";
            const string addressLine2 = "suite 123";
            const string city = "anytown";
            const string state = "OH";
            const string postalCode = "90210";

            var dataReader = new Mock<IDataReader>();
            dataReader.SetupSequence(mocked => mocked.Read()).Returns(true).Returns(false);
            dataReader.SetupGet(mocked => mocked[0]).Returns(checkId);
            dataReader.SetupGet(mocked => mocked[1]).Returns(accountNumber);
            dataReader.SetupGet(mocked => mocked[2]).Returns(amount);
            dataReader.SetupGet(mocked => mocked[3]).Returns(checkNumber);
            dataReader.SetupGet(mocked => mocked[4]).Returns(scanDate);
            dataReader.SetupGet(mocked => mocked[5]).Returns(routingNumber);
            dataReader.SetupGet(mocked => mocked[6]).Returns(name1);
            dataReader.SetupGet(mocked => mocked[7]).Returns(checkDate);
            dataReader.SetupGet(mocked => mocked[8]).Returns(name2);
            dataReader.SetupGet(mocked => mocked[9]).Returns(addressLine1);
            dataReader.SetupGet(mocked => mocked[10]).Returns(addressLine2);
            dataReader.SetupGet(mocked => mocked[11]).Returns(city);
            dataReader.SetupGet(mocked => mocked[12]).Returns(state);
            dataReader.SetupGet(mocked => mocked[13]).Returns(postalCode);

            var dbCommand = new Mock<IDbCommand>();
            dbCommand.Setup(mocked => mocked.Dispose());
            dbCommand.SetupSet(mocked => mocked.CommandType = CommandType.Text).Verifiable();
            dbCommand.SetupSet(mocked => mocked.CommandText = "SELECT ID, Account, Amount, CheckNo, DateScan, Route, Payor, DateCheck, Payor2, Address, Address2, City, State, Zip FROM items WHERE IDBatch = @IDBatch").Verifiable();
            dbCommand.Setup(mocked => mocked.ExecuteReader()).Returns(dataReader.Object);

            var idBatchParam = new Mock<IDbDataParameter>();
            idBatchParam.SetupSet(mocked => mocked.ParameterName = "IDBatch");
            idBatchParam.SetupSet(mocked => mocked.DbType = DbType.String);
            idBatchParam.SetupSet(mocked => mocked.Value = "batch123");

            var parameters = new Mock<IDataParameterCollection>(MockBehavior.Strict);
            parameters.Setup(mocked => mocked.Add(idBatchParam.Object)).Returns(1);

            dbCommand.Setup(mocked => mocked.CreateParameter()).Returns(idBatchParam.Object);
            dbCommand.SetupGet(mocked => mocked.Parameters).Returns(parameters.Object);
            dbCommand.Setup(mocked => mocked.Prepare());

            _dbConnection.Setup(mocked => mocked.Open());
            _dbConnection.Setup(mocked => mocked.CreateCommand()).Returns(dbCommand.Object);
            _dbConnection.Setup(mocked => mocked.Close());

            var result = _fixture.GetChecksForBatch("batch123");
            _dbConnection.VerifyAll();
            dataReader.VerifyAll();
            dbCommand.VerifyAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(checkId, result[0].Id);
            Assert.AreEqual(accountNumber, result[0].AccountNumber);
            Assert.AreEqual((decimal)amount, result[0].Amount);
            Assert.AreEqual(checkNumber, result[0].CheckNumber);
            Assert.AreEqual(scanDate, result[0].ScanDate);
            Assert.AreEqual(routingNumber, result[0].RoutingNumber);
            Assert.AreEqual(name1, result[0].Name1);
            Assert.AreEqual(checkDate, result[0].CheckDate);
            Assert.AreEqual(name2, result[0].Name2);
            Assert.AreEqual(addressLine1, result[0].Address.Line1);
            Assert.AreEqual(addressLine2, result[0].Address.Line2);
            Assert.AreEqual(city, result[0].Address.City);
            Assert.AreEqual(state, result[0].Address.State);
            Assert.AreEqual(postalCode, result[0].Address.PostalCode);
        }

        [Test]
        public void TestUpdateBatchStatus()
        {
            var dbCommand = new Mock<IDbCommand>();
            dbCommand.Setup(mocked => mocked.Dispose());
            dbCommand.SetupSet(mocked => mocked.CommandType = CommandType.Text).Verifiable();
            dbCommand.SetupSet(mocked => mocked.CommandText = "UPDATE batches SET BatchStatus = @BatchStatus WHERE IDBatch = @IDBatch").Verifiable();
            dbCommand.Setup(mocked => mocked.ExecuteNonQuery());

            var idBatchParam = new Mock<IDbDataParameter>();
            idBatchParam.SetupSet(mocked => mocked.ParameterName = "IDBatch");
            idBatchParam.SetupSet(mocked => mocked.DbType = DbType.String);
            idBatchParam.SetupSet(mocked => mocked.Value = "batch123");

            var batchStatusParam = new Mock<IDbDataParameter>();
            batchStatusParam.SetupSet(mocked => mocked.ParameterName = "BatchStatus");
            batchStatusParam.SetupSet(mocked => mocked.DbType = DbType.Int16);
            batchStatusParam.SetupSet(mocked => mocked.Value = 1);

            var parameters = new Mock<IDataParameterCollection>(MockBehavior.Strict);
            parameters.Setup(mocked => mocked.Add(idBatchParam.Object)).Returns(1);
            parameters.Setup(mocked => mocked.Add(batchStatusParam.Object)).Returns(2);

            dbCommand.Setup(mocked => mocked.CreateParameter()).Returns(idBatchParam.Object);
            dbCommand.SetupGet(mocked => mocked.Parameters).Returns(parameters.Object);
            dbCommand.Setup(mocked => mocked.Prepare());

            _dbConnection.Setup(mocked => mocked.Open());
            _dbConnection.Setup(mocked => mocked.CreateCommand()).Returns(dbCommand.Object);
            _dbConnection.Setup(mocked => mocked.Close());

            var result = _fixture.UpdateBatchStatus("batch123", BatchStatus.Exported);
            _dbConnection.VerifyAll();
            dbCommand.VerifyAll();

            Assert.NotNull(result);
            Assert.AreEqual("batch123", result.Name);
            Assert.AreEqual(BatchStatus.Exported, result.Status);
        }

        [Test]
        public void TestCreateDonationsForBatch()
        {
            var checks = new List<CheckScannerCheck>
            {
                new CheckScannerCheck
                {
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
                RegisteredUser = true
            };

            var fixtureMock = new Mock<EzScanCheckScannerService>(_dbConnection.Object, _donorService.Object, _paymentService.Object, _mpDonorService.Object);
            fixtureMock.Setup(mocked => mocked.GetChecksForBatch("batch123")).Returns(checks);
            fixtureMock.Setup(mocked => mocked.CreateDonationsForBatch(It.IsAny<CheckScannerBatch>())).CallBase();
            fixtureMock.Setup(mocked => mocked.UpdateBatchStatus("batch123", BatchStatus.Exported)).Returns(new CheckScannerBatch());

            _donorService.Setup(mocked => mocked.GetContactDonorForDonorAccount(checks[0].AccountNumber, checks[0].RoutingNumber)).Returns(contactDonorExisting);
            _paymentService.Setup(mocked => mocked.ChargeCustomer(contactDonorExisting.ProcessorId, (int) checks[0].Amount, contactDonorExisting.DonorId)).Returns(new StripeCharge
            {
                Id = "1020304",
                BalanceTransaction = new StripeBalanceTransaction
                {
                    Fee = 123
                }
            });
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
                                                               true)).Returns(321);

            var contactDonorNew = new ContactDonor
            {
                ProcessorId = "222000222",
                DonorId = 222222,
                RegisteredUser = false
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
                        string.Empty,
                        "tok123",
                        It.IsAny<DateTime>()))
                .Returns(contactDonorNew);
            _paymentService.Setup(mocked => mocked.ChargeCustomer(contactDonorNew.ProcessorId, (int)checks[1].Amount, contactDonorNew.DonorId)).Returns(new StripeCharge
            {
                Id = "40302010"
            });
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
                                                               false)).Returns(654);



            var result = fixtureMock.Object.CreateDonationsForBatch(new CheckScannerBatch
            {
                Name = "batch123",
                ProgramId = 9090
            });
            fixtureMock.VerifyAll();
            _donorService.VerifyAll();
            _mpDonorService.VerifyAll();
            _paymentService.VerifyAll();
            Assert.NotNull(result);
            Assert.NotNull(result.Checks);
            Assert.AreEqual(2, result.Checks.Count);
            Assert.AreEqual(BatchStatus.Exported, result.Status);
        }
    }
}
