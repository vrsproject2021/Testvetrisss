using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using VETRIS.DAL;

namespace VETRIS.Core.AP
{
    public class TranscriptionistPayment
    {
        #region Constractor
        public TranscriptionistPayment()
        {

        }
        #endregion

        #region Variables
        int intMenuID = 0;
        Guid UserID = new Guid("00000000-0000-0000-0000-000000000000");
        int intUserRoleID = 0;
        Guid CycleID = new Guid("00000000-0000-0000-0000-000000000000");
        Guid TranscriptionistID = new Guid("00000000-0000-0000-0000-000000000000");
        string strStatus = "A";
        string strApproved = "N";
        string strUserName = string.Empty;
        string strXMLTranscriptionist = string.Empty;
        int intProcCount = 0;
        int intAppvCount = 0;
        #endregion

        #region Properties
        public int MENU_ID
        {
            get { return intMenuID; }
            set { intMenuID = value; }
        }
        public Guid USER_ID
        {
            get { return UserID; }
            set { UserID = value; }
        }
        public int USER_ROLE_ID
        {
            get { return intUserRoleID; }
            set { intUserRoleID = value; }
        }
        public Guid BILLING_CYCLE_ID
        {
            get { return CycleID; }
            set { CycleID = value; }
        }
        public Guid TRANSCRIPTIONIST_ID
        {
            get { return TranscriptionistID; }
            set { TranscriptionistID = value; }
        }
        public string APPROVED
        {
            get { return strApproved; }
            set { strApproved = value; }
        }
        public string STATUS
        {
            get { return strStatus; }
            set { strStatus = value; }
        }
        public int RECORD_PROCESSED_COUNT
        {
            get { return intProcCount; }
            set { intProcCount = value; }
        }
        public int RECORD_APPROVED_COUNT
        {
            get { return intAppvCount; }
            set { intAppvCount = value; }
        }
        public string USER_NAME
        {
            get { return strUserName; }
            set { strUserName = value; }
        }
        #endregion

        #region FetchParameters
        public bool FetchParameters(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "ap_transcriptionist_payment_details_fetch_params");
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Cycle";
                    ds.Tables[1].TableName = "Transcriptionist";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region LoadDetails
        public bool LoadDetails(string ConfigPath, ref DataSet ds, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false; int intReturnValue = 0;
            SqlParameter[] SqlRecordParams = new SqlParameter[7];

            try
            {
                if (ValidateLoad(ref ReturnMessage))
                {
                    SqlRecordParams[0] = new SqlParameter("@billing_cycle_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = CycleID;
                    SqlRecordParams[1] = new SqlParameter("@transcriptionist_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = TranscriptionistID;
                    SqlRecordParams[2] = new SqlParameter("@menu_id", SqlDbType.BigInt); SqlRecordParams[2].Value = intMenuID;
                    SqlRecordParams[3] = new SqlParameter("@user_id", SqlDbType.NVarChar, 50); SqlRecordParams[3].Value = UserID;
                    SqlRecordParams[4] = new SqlParameter("@user_name", SqlDbType.NVarChar, 700); SqlRecordParams[4].Direction = ParameterDirection.Output;
                    SqlRecordParams[5] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[5].Direction = ParameterDirection.Output;
                    SqlRecordParams[6] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[6].Direction = ParameterDirection.Output;

                    if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                    ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "ap_transcriptionist_payment_dtls_fetch", SqlRecordParams);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "PaymentHdr";
                        ds.Tables[1].TableName = "PaymentDtls";
                        bReturn = true;
                    }
                    else
                    {
                        intReturnValue = Convert.ToInt32(SqlRecordParams[6].Value);
                        if (intReturnValue == 1)
                            bReturn = true;
                        else
                            bReturn = false;

                        strUserName = Convert.ToString(SqlRecordParams[4].Value).Trim();
                        ReturnMessage = Convert.ToString(SqlRecordParams[5].Value);
                    }
                }
                else
                    bReturn = false;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region ValidateLoad
        private bool ValidateLoad(ref string ReturnMessage)
        {
            bool bReturn = true;
            if (CycleID == new Guid("00000000-0000-0000-0000-000000000000"))
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "229";
            }

            if (ReturnMessage.Trim() != string.Empty)
                bReturn = false;

            return bReturn;
        }

        #endregion

        #region FetchProcessedRecords
        public bool FetchProcessedRecords(string ConfigPath, ref DataSet ds, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[2];

            try
            {
                if (ValidateLoad(ref ReturnMessage))
                {
                    SqlRecordParams[0] = new SqlParameter("@billing_cycle_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = CycleID;
                    SqlRecordParams[1] = new SqlParameter("@transcriptionist_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = TranscriptionistID;


                    if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                    ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "ap_transcriptionist_payment_view_fetch", SqlRecordParams);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "PaymentHdr";
                        ds.Tables[1].TableName = "PaymentDtls";
                        bReturn = true;
                    }
                    else
                        bReturn = false;
                }
                else
                    bReturn = false;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region FetchProcessStatus
        public bool FetchProcessStatus(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            DataSet ds = new DataSet();

            try
            {
                if (ValidateLoad(ref ReturnMessage))
                {

                    SqlParameter[] SqlRecordParams = new SqlParameter[2];
                    SqlRecordParams[0] = new SqlParameter("@billing_cycle_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = CycleID;
                    SqlRecordParams[1] = new SqlParameter("@transcriptionist_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = TranscriptionistID;

                    if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                    ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "ap_transcriptionist_payment_status_fetch", SqlRecordParams);

                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            intAppvCount = Convert.ToInt32(dr["approve_count"]);
                            intProcCount = Convert.ToInt32(dr["process_count"]);
                        }
                    }

                    bReturn = true;


                }
                else
                    bReturn = false;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }
            finally { ds.Dispose(); }


            return bReturn;
        }
        #endregion

        #region SaveRecord
        public bool SaveRecord(string ConfigPath, TranscriptionistList[] ArrTransObj,  ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;

            if (ValidateRecord(ArrTransObj, ref ReturnMessage))
            {
                if ((GenerateTranscriptionistXML(ArrTransObj, ref CatchMessage)))
                {
                    try
                    {
                        SqlParameter[] SqlRecordParams = new SqlParameter[8];
                        SqlRecordParams[0] = new SqlParameter("@billing_cycle_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = CycleID;
                        SqlRecordParams[1] = new SqlParameter("@approved", SqlDbType.NChar,1); SqlRecordParams[1].Value = strApproved;
                        SqlRecordParams[2] = new SqlParameter("@xml_transcriptionist", SqlDbType.NText); SqlRecordParams[2].Value = strXMLTranscriptionist.Trim();
                        SqlRecordParams[3] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[3].Value = intMenuID;
                        SqlRecordParams[4] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[4].Value = UserID;
                        SqlRecordParams[5] = new SqlParameter("@user_name", SqlDbType.NVarChar, 700); SqlRecordParams[5].Direction = ParameterDirection.Output;
                        SqlRecordParams[6] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[6].Direction = ParameterDirection.Output;
                        SqlRecordParams[7] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[7].Direction = ParameterDirection.Output;


                        if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                        intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "ap_transcriptionist_payment_dtls_save", SqlRecordParams);
                        intReturnValue = Convert.ToInt32(SqlRecordParams[7].Value);
                        if (intReturnValue == 1)
                            bReturn = true;
                        else
                            bReturn = false;

                        strUserName = Convert.ToString(SqlRecordParams[5].Value).Trim();
                        ReturnMessage = Convert.ToString(SqlRecordParams[6].Value).Trim();


                    }
                    catch (Exception expErr)
                    { bReturn = false; CatchMessage = expErr.Message; }
                }
                else
                {
                    bReturn = false;

                }

            }
            else
            {
                bReturn = false;
            }

            return bReturn;
        }
        #endregion

        #region ValidateRecord
        private bool ValidateRecord(TranscriptionistList[] ArrTransObj, ref string ReturnMessage)
        {
            bool bReturn = true;


            if (CycleID == new Guid("00000000-0000-0000-0000-000000000000"))
            {
                ReturnMessage = "229";
            }

            if (ArrTransObj.Length == 0)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "405";
            }


            if (ReturnMessage.Trim() != string.Empty)
                bReturn = false;

            return bReturn;
        }
        #endregion

        #region GenerateTranscriptionistXML
        private bool GenerateTranscriptionistXML(TranscriptionistList[] ArrObj, ref string CatchMessage)
        {
            bool bReturn = false;
            StringBuilder sbXML = new StringBuilder();

            try
            {
                if (ArrObj.Length > 0)
                {

                    sbXML.Append("<transcriptionist>");

                    for (int i = 0; i < ArrObj.Length; i = i + 1)
                    {
                        sbXML.Append("<row>");
                        sbXML.Append("<transcriptionist_id><![CDATA[" + ArrObj[i].ID.ToString() + "]]></transcriptionist_id>");
                        sbXML.Append("<study_id><![CDATA[" + ArrObj[i].STUDY_ID.ToString() + "]]></study_id>");
                        sbXML.Append("<study_uid><![CDATA[" + ArrObj[i].STUDY_UID.ToString() + "]]></study_uid>");
                        sbXML.Append("<adhoc_amount>" + ArrObj[i].ADHOC_AMOUNT.ToString() + "</adhoc_amount>");
                        sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                        sbXML.Append("</row>");
                    }

                    sbXML.Append("</transcriptionist>");
                    strXMLTranscriptionist = sbXML.ToString();


                }
                bReturn = true;
            }
            catch (Exception LexpErr)
            {
                bReturn = false;
                CatchMessage = LexpErr.Message;
            }
            return bReturn;
        }
        #endregion
    }

    public class TranscriptionistList
    {
        #region Constructor
        public TranscriptionistList()
        {
        }
        #endregion

        #region Variables
        Guid Id = Guid.Empty;
        Guid StudyID = Guid.Empty;
        string strSUID = string.Empty;
        double dblAdhocAmt = 0;
        #endregion

        #region Properties
        public Guid ID
        {
            get { return Id; }
            set { Id = value; }
        }
        public Guid STUDY_ID
        {
            get { return StudyID; }
            set { StudyID = value; }
        }
        public double ADHOC_AMOUNT
        {
            get { return dblAdhocAmt; }
            set { dblAdhocAmt = value; }
        }
        public string STUDY_UID
        {
            get { return strSUID; }
            set { strSUID = value; }
        }
        #endregion
    }
}
