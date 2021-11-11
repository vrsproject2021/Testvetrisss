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

namespace VETRIS.Core
{
    public class CoreCommon
    {
        #region Variables
        private static string DB_CONN_STRING = string.Empty;
        private static string DB_VERSION = string.Empty;
        private static string DIVIDER = "±";
        private static string RPT_LICENSE_KEY = string.Empty;
        #endregion

        #region Properties
        public static string CONNECTION_STRING
        {
            get { return DB_CONN_STRING; }
            set { DB_CONN_STRING = value; }
        }
        public static string DATABASE_VERSION
        {
            get { return DB_VERSION; }
            set { DB_VERSION = value; }
        }
        public static string STRING_DIVIDER
        {
            get { return DIVIDER; }
            set { DIVIDER = value; }
        }
        public static string REPORT_LICENSE_KEY
        {
            get { return RPT_LICENSE_KEY; }
            set { RPT_LICENSE_KEY = value; }
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

        #region GetDBVersion
        public static bool GetDBVersion(string strPath, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            SqlRecordParams[0] = new SqlParameter("@version_no", SqlDbType.NVarChar, 50); SqlRecordParams[0].Value = DB_VERSION; SqlRecordParams[0].Direction = ParameterDirection.InputOutput;
           

            try
            {
                if (DB_CONN_STRING.Trim() == string.Empty) GetConnectionString(strPath);
                intExecReturn = DAL.DataHelper.ExecuteNonQuery(DB_CONN_STRING, CommandType.StoredProcedure, "get_version", SqlRecordParams);
                DB_VERSION = Convert.ToString(SqlRecordParams[0].Value);
                bReturn = true;

            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region IsEmailValid
        public static bool IsEmailValid(string EmailAddr)
        {
            bool bReturn = false;

            if (EmailAddr != null || EmailAddr != "")
            {
                string MatchEmailPattern = @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
                                            + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
                                                + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
                                                + @"([a-zA-Z0-9]+[\w-]+\.)+[a-zA-Z]{2,4})$";

                //Regex n = new Regex("(?<user>[^@]+)@(?<host>.+)");
                Regex n = new Regex(MatchEmailPattern);
                Match v = n.Match(EmailAddr);

                if (!v.Success || EmailAddr.Length != v.Length)
                {
                    bReturn = false;
                }
                else
                {
                    bReturn = true;
                }

            }
            return bReturn;
        }
        #endregion

        #region GetReportLicenseKey
        public static void GetReportLicenseKey(string strPath)
        {
            TextReader tr = new StreamReader(strPath + "\\RptKey.key");
            string strLicense = tr.ReadLine();
            strLicense = DecryptString(strLicense);
            RPT_LICENSE_KEY = strLicense.Trim();
        }
        #endregion

        #region RandomString
        public static string RandomString(int length)
        {
            string strChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var cString = new char[length];
            string strFinalString = string.Empty;
            var random = new Random();

            for (int i = 0; i < cString.Length; i++)
            {
                cString[i] = strChars[random.Next(strChars.Length)];
            }
            strFinalString = new String(cString);


            return strFinalString;
        }
        #endregion
    }
}
