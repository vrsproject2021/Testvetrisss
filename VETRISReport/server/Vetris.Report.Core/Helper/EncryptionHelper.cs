using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Vetris.Report.Core.Helper
{
    public static class EncryptionHelper
    {
        #region DecryptString
        public static string DecryptString(string toDecryptString, string key = "7")
        {
            string LsReturn = "";
            try
            {
                toDecryptString = toDecryptString.Replace(" ", "+");
                byte[] keyArray;
                byte[] toDecryptArray = Convert.FromBase64String(toDecryptString);
                //string key = "7";
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF7.GetBytes(key));
                hashmd5.Clear();

                hashmd5 = null;
                byte[] key24Array = new byte[24];
                for (int i = 0; i < 16; i++)
                {
                    key24Array[i] = keyArray[i];
                }
                for (int i = 0; i < 7; i++)
                {
                    key24Array[i + 16] = keyArray[i];
                }
                keyArray = null;

                TripleDESCryptoServiceProvider tripledes = new TripleDESCryptoServiceProvider();
                tripledes.Key = key24Array;
                tripledes.Mode = CipherMode.ECB;
                tripledes.Padding = PaddingMode.PKCS7;
                ICryptoTransform cryptoTransform = tripledes.CreateDecryptor();
                byte[] resultArray = cryptoTransform.TransformFinalBlock(toDecryptArray, 0, toDecryptArray.Length);
                tripledes.Clear();

                tripledes = null;
                cryptoTransform = null;
                toDecryptArray = null;

                UTF8Encoding encoder = new UTF8Encoding();

                LsReturn = encoder.GetString(resultArray);

                resultArray = null;
                encoder = null;
                key = null;
            }
            catch
            {
                ;
            }
            return LsReturn;
        }
        #endregion

        #region EncryptString
        public static string EncryptString(string toEncryptString, string key="7")
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncryptString);
            //string key = "7";
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF7.GetBytes(key));
            hashmd5.Clear();
            byte[] key24Array = new byte[24];
            for (int i = 0; i < 16; i++)
            {
                key24Array[i] = keyArray[i];
            }
            for (int i = 0; i < 7; i++)
            {
                key24Array[i + 16] = keyArray[i];
            }
            TripleDESCryptoServiceProvider tripledes = new TripleDESCryptoServiceProvider();
            tripledes.Key = key24Array;
            tripledes.Mode = CipherMode.ECB;
            tripledes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cryptoTransform = tripledes.CreateEncryptor();
            byte[] resultArray = cryptoTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tripledes.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        #endregion        

        //Encryptor: https://www.md5online.org/
        //Decryptor: https://www.md5online.org/md5-decrypt.html
        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text  
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it  
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits  
                //for each byte  
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }
    }
}
