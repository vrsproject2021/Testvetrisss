using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using VETRISScheduler.DAL;
using System.Diagnostics;

namespace VETRISScheduler.Core
{
    public class CoreCommon
    {
        #region Variables
        private static string DB_CONN_STRING = string.Empty;
        private static string LOG_DB_CONN_STRING = string.Empty;
        #endregion

        #region Properties
        public static string CONNECTION_STRING
        {
            get { return DB_CONN_STRING; }
            set { DB_CONN_STRING = value; }
        }
        public static string LOGDB_CONNECTION_STRING
        {
            get { return LOG_DB_CONN_STRING; }
            set { LOG_DB_CONN_STRING = value; }
        }
        #endregion

        #region GetConnectionString
        public static void GetConnectionString(string LsPath)
        {
            TextReader tr = new StreamReader(LsPath + "\\vetris.cfg");
            string strConn = tr.ReadLine();
            strConn = DecryptString(strConn);
            DB_CONN_STRING = strConn.Trim();
        }
        #endregion

        #region GetLogDBConnectionString
        public static void GetLogDBConnectionString(string LsPath)
        {
            TextReader tr = new StreamReader(LsPath + "\\vetrislog.cfg");
            string strConn = tr.ReadLine();
            strConn = DecryptString(strConn);
            LOG_DB_CONN_STRING = strConn.Trim();
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

        #region doLog
        public static bool doLog(string ConfigPath, int ServiceID, string ServiceName, string LogMessage, bool IsError)
        {
            bool bReturn = false; int intExecReturn = 0; int intReturnType = 0;
            SqlParameter[] SqlRecordParams = new SqlParameter[5];


            try
            {
                SqlRecordParams[0] = new SqlParameter("@is_error", SqlDbType.Bit); SqlRecordParams[0].Value = IsError;
                SqlRecordParams[1] = new SqlParameter("@service_id", SqlDbType.Int); SqlRecordParams[1].Value = ServiceID;
                SqlRecordParams[2] = new SqlParameter("@log_message", SqlDbType.VarChar, 8000); SqlRecordParams[2].Value = LogMessage;
                SqlRecordParams[3] = new SqlParameter("@error_msg", SqlDbType.VarChar, 100); SqlRecordParams[3].Direction = ParameterDirection.Output;
                SqlRecordParams[4] = new SqlParameter("@return_type", SqlDbType.Int); SqlRecordParams[4].Direction = ParameterDirection.Output;

                if (DB_CONN_STRING == string.Empty) GetConnectionString(ConfigPath);
                intExecReturn = DataHelper.ExecuteNonQuery(DB_CONN_STRING, CommandType.StoredProcedure, "scheduler_log_save", SqlRecordParams);
                intReturnType = Convert.ToInt32(SqlRecordParams[4].Value);
                if (intReturnType == 0)
                {
                    EventLog.WriteEntry(ServiceName, Convert.ToString(SqlRecordParams[2].Value), EventLogEntryType.Error);
                    bReturn = false;
                }
                else
                    bReturn = true;

            }
            catch (SqlException expErr)
            {
                EventLog.WriteEntry(ServiceName, expErr.Message, EventLogEntryType.Error);
                bReturn = false;
            }

            return bReturn;

        }
        #endregion

        #region IsDicomFile
        public static bool IsDicomFile(string strFileWithPath)
        {

            bool bRet = false;

            BinaryReader br = new BinaryReader(new FileStream(strFileWithPath, FileMode.Open, FileAccess.Read), Encoding.ASCII);

            byte[] preamble = new byte[132];

            br.Read(preamble, 0, 132);

            if (preamble[128] != 'D' || preamble[129] != 'I' || preamble[130] != 'C' || preamble[131] != 'M')
            {

                bRet = false;

            }

            else
            {
                bRet = true;

            }
            br.Close();
            return bRet;

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
