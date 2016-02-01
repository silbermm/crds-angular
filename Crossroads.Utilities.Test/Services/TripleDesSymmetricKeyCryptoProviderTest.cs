using System;
using System.Security.Cryptography;
using Crossroads.Utilities.Services;
using NUnit.Framework;

namespace Crossroads.Utilities.Test.Services
{
    public class TripleDesSymmetricKeyCryptoProviderTest
    {
        private TripleDesSymmetricKeyCryptoProvider _fixture;

        [SetUp]
        public void SetUp()
        {
            const CipherMode cipherMode = CipherMode.ECB;
            const PaddingMode paddingMode = PaddingMode.PKCS7;

            var gen = TripleDES.Create();
            gen.Mode = cipherMode;
            gen.Padding = paddingMode;
            gen.GenerateKey();

            _fixture = new TripleDesSymmetricKeyCryptoProvider(Convert.ToBase64String(gen.Key), cipherMode, paddingMode);
        }

        [Test]
        public void TestEncryptAndDecrypt()
        {
            const string plainText = "The security of the world is now in your hands!";

            var cipherBytes = _fixture.EncryptValue(plainText);
            var cipherText = _fixture.EncryptValueToString(plainText);
            Assert.AreEqual(Convert.ToBase64String(cipherBytes), cipherText);

            Assert.AreEqual(plainText, _fixture.DecryptValue(cipherBytes));
            Assert.AreEqual(plainText, _fixture.DecryptValue(cipherText));
        }
    }
}
