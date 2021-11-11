using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Data;
using System.Diagnostics;
using System.Data.OleDb;
using System.Configuration;

namespace VETRISRouter.Core
{
    public class CoreCommon
    {
        #region Variables
        private static string ACCESSORYDIR = AppDomain.CurrentDomain.BaseDirectory + "AccesoryDirs\\";
        //private static string ACCESSORYDIR = "E:\\DICOM ROUTER\\AccesoryDirs\\";
        private static string DBNAME = "DicomDB.accdb";
        private static string EXEPATH = ACCESSORYDIR + "DICOM-EXEs\\EXEs\\bin\\";
        private static string RCVEXENAME = "storescp.exe";
        private static string SNDEXENAME = "storescu.exe";
        private static string RCVEXEOPTIONS = " -v -fe .dcm -aet RAD365 11112 -od ";
        private static string SNDEXEOPTIONS = " -v +sd +r -aec CENTURYERAD 208.87.85.190 104 ";

        private static string DB_CONN_STRING = string.Empty;
        private static string DEVICE_DB_CONN_STRING = string.Empty;
        private static string DR_SETTINGS_STRING = string.Empty;
        #endregion

        #region Properties
        public static string ACCESSORY_DIR
        {
            get { return ACCESSORYDIR; }
            set { ACCESSORYDIR = value; }
        }
        public static string DB_NAME
        {
            get { return DBNAME; }
            set { DBNAME = value; }
        }
        public static string EXE_PATH
        {
            get { return EXEPATH; }
            set { EXEPATH = value; }
        }
        public static string RCVEXE_NAME
        {
            get { return RCVEXENAME; }
            set { RCVEXENAME = value; }
        }
        public static string SNDEXE_NAME
        {
            get { return SNDEXENAME; }
            set { SNDEXENAME = value; }
        }
        public static string RCVEXE_OPTIONS
        {
            get { return RCVEXEOPTIONS; }
            set { RCVEXEOPTIONS = value; }
        }
        public static string SNDEXE_OPTIONS
        {
            get { return SNDEXEOPTIONS; }
            set { SNDEXEOPTIONS = value; }
        }
        public static string CONNECTION_STRING
        {
            get { return DB_CONN_STRING; }
            set { DB_CONN_STRING = value; }
        }
        public static string DEVICE_CONNECTION_STRING
        {
            get { return DEVICE_DB_CONN_STRING; }
            set { DEVICE_DB_CONN_STRING = value; }
        }
        public static string SETTINGS_STRING
        {
            get { return DR_SETTINGS_STRING; }
            set { DR_SETTINGS_STRING = value; }
        }

        #endregion

        #region GetConnectionString
        public static void GetConnectionString(string LsPath)
        {

           DB_CONN_STRING = LsPath + "\\Configs\\Config.xml";

        }
        #endregion

        #region GetDeviceConnectionString
        public static void GetDeviceConnectionString(string LsPath)
        {

            DEVICE_DB_CONN_STRING = LsPath + "\\Configs\\Devices.xml";

        }
        #endregion

        #region GetSettingsString
        public static void GetSettingsString(string strPath)
        {
            TextReader tr = new StreamReader(strPath + "\\VDR.cfg");
            string strSettings = tr.ReadLine();
            strSettings = DecryptString(strSettings);
            DR_SETTINGS_STRING = strSettings.Trim();
        }
        #endregion

        #region doLog
        public static bool doLog(string ConfigPath, int intServiceID, string strServiceName, string strIserror, string strLogMessage)
        {
            bool bReturn = false;
            DataSet ds = new DataSet();
            DataTable dtbl = new DataTable();
            string strFileName = string.Empty;

            try
            {
                strFileName = "DRLog.xml";

                if (!File.Exists(ConfigPath + "\\" + strFileName))
                {
                    CreateLogTable(ref dtbl);
                    //dtbl.WriteXml(ConfigPath  + strFileName);
                }
                else
                {
                    ds.ReadXml(ConfigPath + "\\" + strFileName);
                    if (ds.Tables.Count > 0) dtbl = ds.Tables[0];
                    else CreateLogTable(ref dtbl);
                }


                DataRow dr = dtbl.NewRow();
                dtbl.TableName = "Logs";
                dr["service_id"] = intServiceID;
                dr["service_name"] = strServiceName;
                dr["is_error"] = strIserror;
                dr["log_date"] = DateTime.Now.ToString("ddMMMyyyy HH:mm:ss");
                dr["log_message"] = strLogMessage;
                dr["sent_to_vetris"] = "N";
                dtbl.Rows.Add(dr);
                dtbl.WriteXml(ConfigPath + "\\" + strFileName);
            }
            catch (Exception expErr)
            {
                EventLog.WriteEntry("doLog() :: Exception : ", expErr.Message, EventLogEntryType.Error);
                bReturn = false;
            }
            finally
            {
                dtbl = null;
                ds.Dispose();
            }

            return bReturn;
        }
        #endregion

        #region CreateLogTable
        public static void CreateLogTable(ref DataTable dtbl)
        {
            dtbl.Columns.Add("service_id", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("service_name", System.Type.GetType("System.String"));
            dtbl.Columns.Add("is_error", System.Type.GetType("System.String"));
            dtbl.Columns.Add("log_date", System.Type.GetType("System.String"));
            dtbl.Columns.Add("log_message", System.Type.GetType("System.String"));
            dtbl.Columns.Add("sent_to_vetris", System.Type.GetType("System.String"));
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
    }
}
