using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using MinistryPlatform.Translation.Services;
namespace MinistryPlatform.Translation.Test
{
    [TestFixture]
    public class TranslationTests
    {
        [Test]
        public void ShouldFailLogin()
        {
            var obj = AuthenticationService.authenticate("", "");
            Assert.IsNull(obj, "When not authenticated this should be null");
        }

        [Test]
        public void ShouldLogin()
        {
            var obj = AuthenticationService.authenticate("tmaddox", "crds1234");
            Assert.IsNotNull(obj, "When authenticated this should be a JObject");
        }

        [Test]
        public void ShouldReturnContactId()
        {
            var token = AuthenticationService.authenticate("tmaddox", "crds1234");
            var obj = AuthenticationService.GetContactId(token);
            Assert.IsNotNull(obj, "Contact ID shouldn't be null");
        }

        //[Test]
        //public void ShouldGetPageRecords()
        //{
        //    var pageId = 455;
        //    var record = MinistryPlatform.Translation.Services.MinistryPlatform.GetMyPageRecords(pageId);
        //    Assert.IsNotNull(record);
        //    Assert.IsNotEmpty(record);
        //    Assert.AreEqual( "Tony", record.FirstOrDefault()["First_Name"].ToString());
        //}

        //[Test]
        //public void ShouldGetNoPageRecords()
        //{
        //    var pageId = 0;
        //    var record = MinistryPlatform.Translation.Services.MinistryPlatform.GetMyPageRecords(pageId);
        //    Assert.IsNull(record);
        //}

        //[Test]
        //public void ShouldGetPageRecord()
        //{
        //    var pageId = 292;
        //    var recordId = 618602;
        //    var record = MinistryPlatform.Translation.Services.MinistryPlatform.GetMyPageRecord(pageId, recordId);
        //    Assert.IsNotNull(record);
        //    Assert.IsNotEmpty(record);
        //    Assert.AreEqual("Andrew", record.FirstOrDefault()["First_Name"].ToString());
        //}

        //[Test]
        //public void ShouldGetNoPageRecord()
        //{
        //    var pageId = 292;
        //    var recordId = 0;
        //    var record = MinistryPlatform.Translation.Services.MinistryPlatform.GetMyPageRecord(pageId, recordId);
        //    Assert.IsNull(record);

        //}
    }
}
