using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinistryPlatform.Translation.Services;
using NUnit.Framework;

namespace MinistryPlatform.Translation.Test.Services
{
    [TestFixture]
    public class GroupParticipantServiceTests
    {
        [Test]
        public void GetServingParticipantsTest()
        {
            var service = new GroupParticipantService();
            service.GetServingParticipants();

        }
    }
}
