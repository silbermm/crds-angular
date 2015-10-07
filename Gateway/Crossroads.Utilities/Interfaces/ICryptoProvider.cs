namespace Crossroads.Utilities.Interfaces
{
    public interface ICryptoProvider
    {
        /// <summary>
        /// Decrypt a Base64-encoded cipherText.
        /// </summary>
        /// <param name="cipherText">An encrypted byte array that has been Base-64 encoded to a string</param>
        /// <returns>The decrypted plain text value</returns>
        string DecryptValue(string cipherText);

        /// <summary>
        /// Decrypt an encrypted byte array.
        /// </summary>
        /// <param name="cipherBytes">An encrypted byte array</param>
        /// <returns>The decrypted plain text value</returns>
        string DecryptValue(byte[] cipherBytes);

        /// <summary>
        /// Encrypt a plaintext value into a Base-64 encoded cipher text byte array.
        /// </summary>
        /// <param name="plainText">The plain text to encrypt</param>
        /// <returns>A string representing the Base-64-encoded encrypted byte array</returns>
        string EncryptValueToString(string plainText);

        /// <summary>
        /// Encrypt a plaintext value into a cipher text byte array
        /// </summary>
        /// <param name="plainText">The plain text to encrypt</param>
        /// <returns>The encrypted byte array</returns>
        byte[] EncryptValue(string plainText);
    }
}
