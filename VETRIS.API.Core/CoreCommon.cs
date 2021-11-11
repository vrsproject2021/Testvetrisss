using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace VETRIS.API.Core
{
    public class CoreCommon
    {
        #region Variables
        private static string DB_CONN_STRING = string.Empty;
        private static string DIVIDER = "±";

        #endregion

        #region Properties
        public static string CONNECTION_STRING
        {
            get { return DB_CONN_STRING; }
            set { DB_CONN_STRING = value; }
        }

        public static string STRING_DIVIDER
        {
            get { return DIVIDER; }
            set { DIVIDER = value; }
        }
       
        #endregion

        #region GetConnectionString
        public static void GetConnectionString(string strPath)
        {
            TextReader tr = new StreamReader(strPath + "\\vetris.cfg");
            string strConn = tr.ReadLine();
            strConn = DecryptString(strConn);
            DB_CONN_STRING = strConn.Trim();
        }
        #endregion

        #region DecryptString
        public static string DecryptString(string toDecryptString)
        {
            byte[] keyArray;
            byte[] toDecryptArray = Convert.FromBase64String(toDecryptString);
            string key = "7";
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
            ICryptoTransform cryptoTransform = tripledes.CreateDecryptor();
            byte[] resultArray = cryptoTransform.TransformFinalBlock(toDecryptArray, 0, toDecryptArray.Length);
            tripledes.Clear();
            UTF8Encoding encoder = new UTF8Encoding();
            return encoder.GetString(resultArray);
        }
        #endregion

        #region EncryptString
        public static string EncryptString(string toEncryptString)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncryptString);
            string key = "7";
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

        
    }
}
