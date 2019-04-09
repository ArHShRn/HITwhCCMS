using System;
using System.Security.Cryptography;
using System.Text;

namespace HITwhCMS.Backend.Utils
{
    public class AESHelper
    {
        /// <summary>
        /// AES256 Encode
        /// </summary>
        /// <param name="encryptStr">Plain</param>
        /// <param name="key">Key</param>
        /// <returns></returns>
        public static string Encrypt(string encryptStr, string key)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(encryptStr);

            RijndaelManaged rDel = new RijndaelManaged
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// AES256 Decode
        /// </summary>
        /// <param name="decryptStr">Encrypted String</param>
        /// <param name="key">Key</param>
        /// <returns></returns>
        public static string Decrypt(string decryptStr, string key)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
            byte[] toEncryptArray = Convert.FromBase64String(decryptStr);

            RijndaelManaged rDel = new RijndaelManaged
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
    }
}
