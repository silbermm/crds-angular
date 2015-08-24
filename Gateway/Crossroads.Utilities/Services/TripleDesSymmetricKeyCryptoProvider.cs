using System;
using System.Security.Cryptography;
using System.Text;
using Crossroads.Utilities.Interfaces;

namespace Crossroads.Utilities.Services
{
    public class TripleDesSymmetricKeyCryptoProvider : ICryptoProvider
    {
        private readonly ICryptoTransform _encrypt;
        private readonly ICryptoTransform _decrypt;

        public TripleDesSymmetricKeyCryptoProvider(string base64EncodedKey, CipherMode cipherMode, PaddingMode paddingMode)
        {
            var tripleDes = TripleDES.Create();
            tripleDes.Mode = cipherMode;
            tripleDes.Padding = paddingMode;
            tripleDes.Key = Convert.FromBase64String(base64EncodedKey);

            _encrypt = tripleDes.CreateEncryptor();
            _decrypt = tripleDes.CreateDecryptor();
        }

        public string DecryptValue(string cipherText)
        {
            var enc = Convert.FromBase64String(cipherText);
            return (DecryptValue(enc));
        }

        public string DecryptValue(byte[] cipherBytes)
        {
            return (Encoding.UTF8.GetString(_decrypt.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length)));
        }

        public string EncryptValueToString(string plainText)
        {
            return (Convert.ToBase64String(EncryptValue(plainText)));
        }

        public byte[] EncryptValue(string plainText)
        {
            var plain = Encoding.UTF8.GetBytes(plainText);
            return (_encrypt.TransformFinalBlock(plain, 0, plain.Length));
        }
    }
}
