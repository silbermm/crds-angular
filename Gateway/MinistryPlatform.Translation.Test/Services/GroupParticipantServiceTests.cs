using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinistryPlatform.Translation.Services;
using Moq;
using NUnit.Framework;

namespace MinistryPlatform.Translation.Test.Services
{
    [TestFixture]
    public class GroupParticipantServiceTests
    {
        private GroupParticipantService _fixture;
        private Mock<IDbConnection> _mpDbConnectionMock;
        private Mock<IDbCommand> _mpDbCommandMock;
        private Mock<IDataReader> _mpDataReaderMock;

        [SetUp]
        public void SetUp()
        {
            _mpDbConnectionMock = new Mock<IDbConnection>();
            _mpDbCommandMock = new Mock<IDbCommand>();
            _mpDataReaderMock = new Mock<IDataReader>();
            _fixture = new GroupParticipantService(_mpDbConnectionMock.Object);
        }
        [Test]
        public void GetServingParticipantsTest()
        {

            //var service = new GroupParticipantService();
            _fixture.GetServingParticipants();

        }
    }
}
