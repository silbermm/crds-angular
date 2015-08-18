using System;
using System.Collections.Generic;
using System.Data;
using crds_angular.Models.Crossroads.Stewardship;
using crds_angular.Services;
using crds_angular.Services.Interfaces;
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
            // TODO Finish this test
            //var dataReader = new Mock<IDataReader>();
            //dataReader.SetupSequence(mocked => mocked.Read()).Returns(true).Returns(false);
            //dataReader.SetupSequence(mocked => mocked[0]).Returns(1).Returns(2);
            //dataReader.SetupSequence(mocked => mocked[1]).Returns("name1").Returns("name2");
            //dataReader.SetupSequence(mocked => mocked[2]).Returns(dateTime1).Returns(dateTime2);
            //dataReader.Setup(mocked => mocked.IsDBNull(3)).Returns(false);
            //dataReader.SetupSequence(mocked => mocked.GetInt32(3)).Returns(0).Returns(1);


            //_fixture.GetChecksForBatch("batch123");
        }
    }
}
