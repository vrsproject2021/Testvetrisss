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

namespace VETRIS.Core.Invoicing
{
    public class InvoiceProcess
    {
        #region Constructor
        public InvoiceProcess()
        {

        }

        #endregion

        #region Variables
        int intMenuID = 0;
        Guid UserID = new Guid("00000000-0000-0000-0000-000000000000");
        int intUserRoleID = 0;
        Guid CycleID = new Guid("00000000-0000-0000-0000-000000000000");
        Guid AccountID = new Guid("00000000-0000-0000-0000-000000000000");
        Guid InstID = new Guid("00000000-0000-0000-0000-000000000000");
        string strStatus = "A";
        string strUserName = string.Empty;
        string strXMLInst = string.Empty;
        string strXMLStudy = string.Empty;
        string strXMLAcct = string.Empty;
        int intProcCount = 0;
        int intAppvCount = 0;
        string strMailType = string.Empty;
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
        public Guid BILLING_ACCOUNT_ID
        {
            get { return AccountID; }
            set { AccountID = value; }
        }
        public Guid INSTITUTION_ID
        {
            get { return InstID; }
            set { InstID = value; }
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
        public string MAIL_TYPE
        {
            get { return strMailType; }
            set { strMailType = value; }
        }
        #endregion

        #region FetchParameters
        public bool FetchParameters(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];

            try
            {
                SqlRecordParams[0] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = UserID;
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "invoicing_details_fetch_params",SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Cycle";
                    ds.Tables[1].TableName = "Account";
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
                    SqlRecordParams[1] = new SqlParameter("@billing_account_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = AccountID;
                    SqlRecordParams[2] = new SqlParameter("@menu_id", SqlDbType.BigInt); SqlRecordParams[2].Value = intMenuID;
                    SqlRecordParams[3] = new SqlParameter("@user_id", SqlDbType.NVarChar, 50); SqlRecordParams[3].Value = UserID;
                    SqlRecordParams[4] = new SqlParameter("@user_name", SqlDbType.NVarChar, 700); SqlRecordParams[4].Direction = ParameterDirection.Output;
                    SqlRecordParams[5] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[5].Direction = ParameterDirection.Output;
                    SqlRecordParams[6] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[6].Direction = ParameterDirection.Output;

                    if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                    ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "invoicing_process_dtls_fetch", SqlRecordParams);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "InvoiceHdr";
                        ds.Tables[1].TableName = "InvoiceDtlsHdr";
                        ds.Tables[2].TableName = "InvoiceDtls";
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
                    SqlRecordParams[1] = new SqlParameter("@billing_account_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = AccountID;


                    if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                    ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "invoicing_process_view_fetch", SqlRecordParams);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "InvoiceHdr";
                        ds.Tables[1].TableName = "InvoiceDtlsHdr";
                        ds.Tables[2].TableName = "InvoiceDtls";
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

        #region FetchFinalRecords
        public bool FetchFinalRecords(string ConfigPath, ref DataSet ds, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[2];

            try
            {
                if (ValidateLoad(ref ReturnMessage))
                {
                    SqlRecordParams[0] = new SqlParameter("@billing_cycle_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = CycleID;
                    SqlRecordParams[1] = new SqlParameter("@billing_account_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = AccountID;


                    if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                    ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "invoicing_process_final_fetch", SqlRecordParams);
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].TableName = "InvoiceHdr";
                        ds.Tables[1].TableName = "InvoiceDtlsHdr";
                        ds.Tables[2].TableName = "InvoiceDtls";
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
                    SqlRecordParams[1] = new SqlParameter("@billing_account_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = AccountID;

                    if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                    ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "invoicing_process_status_fetch", SqlRecordParams);

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
        public bool SaveRecord(string ConfigPath, BillingAccountList[] ArrAcctObj, InstitutionList[] ArrInstObj, StudyList[] ArrStudyObj, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;

            if (ValidateRecord(ArrAcctObj, ArrInstObj, ArrStudyObj, ref ReturnMessage))
            {
                if ((GenerateAccountXML(ArrAcctObj, ref CatchMessage)) && (GenerateInstitutionXML(ArrInstObj, ref CatchMessage) && (GenerateStudyXML(ArrStudyObj, ref CatchMessage))))
                {
                    try
                    {
                        SqlParameter[] SqlRecordParams = new SqlParameter[9];
                        SqlRecordParams[0] = new SqlParameter("@billing_cycle_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = CycleID;
                        SqlRecordParams[1] = new SqlParameter("@xml_account", SqlDbType.NText); SqlRecordParams[1].Value = strXMLAcct.Trim();
                        SqlRecordParams[2] = new SqlParameter("@xml_institution", SqlDbType.NText); SqlRecordParams[2].Value = strXMLInst.Trim();
                        SqlRecordParams[3] = new SqlParameter("@xml_study", SqlDbType.NText); SqlRecordParams[3].Value = strXMLStudy.Trim();
                        SqlRecordParams[4] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[4].Value = intMenuID;
                        SqlRecordParams[5] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[5].Value = UserID;
                        SqlRecordParams[6] = new SqlParameter("@user_name", SqlDbType.NVarChar, 700); SqlRecordParams[6].Direction = ParameterDirection.Output;
                        SqlRecordParams[7] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[7].Direction = ParameterDirection.Output;
                        SqlRecordParams[8] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[8].Direction = ParameterDirection.Output;


                        if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                        intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "invoicing_process_dtls_save", SqlRecordParams);
                        intReturnValue = Convert.ToInt32(SqlRecordParams[8].Value);
                        if (intReturnValue == 1)
                            bReturn = true;
                        else
                            bReturn = false;

                        strUserName = Convert.ToString(SqlRecordParams[6].Value).Trim();
                        ReturnMessage = Convert.ToString(SqlRecordParams[7].Value).Trim();


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
        private bool ValidateRecord(BillingAccountList[] ArrAcctObj, InstitutionList[] ArrInstObj, StudyList[] ArrStudyObj, ref string ReturnMessage)
        {
            bool bReturn = true;


            if (CycleID == new Guid("00000000-0000-0000-0000-000000000000"))
            {
                ReturnMessage = "229";
            }

            if (ArrAcctObj.Length == 0)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "235";
            }
            if (ArrInstObj.Length == 0)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "236";
            }
            if (ArrStudyObj.Length == 0)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "237";
            }



            if (ReturnMessage.Trim() != string.Empty)
                bReturn = false;

            return bReturn;
        }


        #endregion

        #region BulkApprove
        public bool BulkApprove(string ConfigPath, BillingAccountList[] ArrAcctObj, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;

            if (ValidateBulkRecord(ArrAcctObj, ref ReturnMessage))
            {
                if ((GenerateAccountXML(ArrAcctObj, ref CatchMessage)))
                {
                    try
                    {
                        SqlParameter[] SqlRecordParams = new SqlParameter[7];
                        SqlRecordParams[0] = new SqlParameter("@billing_cycle_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = CycleID;
                        SqlRecordParams[1] = new SqlParameter("@xml_account", SqlDbType.NText); SqlRecordParams[1].Value = strXMLAcct.Trim();
                        SqlRecordParams[2] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[2].Value = intMenuID;
                        SqlRecordParams[3] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[3].Value = UserID;
                        SqlRecordParams[4] = new SqlParameter("@user_name", SqlDbType.NVarChar, 700); SqlRecordParams[4].Direction = ParameterDirection.Output;
                        SqlRecordParams[5] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[5].Direction = ParameterDirection.Output;
                        SqlRecordParams[6] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[6].Direction = ParameterDirection.Output;


                        if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                        intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "invoicing_process_bulk_approve", SqlRecordParams);
                        intReturnValue = Convert.ToInt32(SqlRecordParams[6].Value);
                        if (intReturnValue == 1)
                            bReturn = true;
                        else
                            bReturn = false;

                        strUserName = Convert.ToString(SqlRecordParams[4].Value).Trim();
                        ReturnMessage = Convert.ToString(SqlRecordParams[5].Value).Trim();


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

        #region BulkUnfinal
        public bool BulkUnfinal(string ConfigPath, BillingAccountList[] ArrAcctObj, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;

            if (ValidateBulkRecord(ArrAcctObj, ref ReturnMessage))
            {
                if ((GenerateAccountXML(ArrAcctObj, ref CatchMessage)))
                {
                    try
                    {
                        SqlParameter[] SqlRecordParams = new SqlParameter[7];
                        SqlRecordParams[0] = new SqlParameter("@billing_cycle_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = CycleID;
                        SqlRecordParams[1] = new SqlParameter("@xml_account", SqlDbType.NText); SqlRecordParams[1].Value = strXMLAcct.Trim();
                        SqlRecordParams[2] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[2].Value = intMenuID;
                        SqlRecordParams[3] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[3].Value = UserID;
                        SqlRecordParams[4] = new SqlParameter("@user_name", SqlDbType.NVarChar, 700); SqlRecordParams[4].Direction = ParameterDirection.Output;
                        SqlRecordParams[5] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[5].Direction = ParameterDirection.Output;
                        SqlRecordParams[6] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[6].Direction = ParameterDirection.Output;


                        if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                        intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "invoicing_process_bulk_disapprove", SqlRecordParams);
                        intReturnValue = Convert.ToInt32(SqlRecordParams[6].Value);
                        if (intReturnValue == 1)
                            bReturn = true;
                        else
                            bReturn = false;

                        strUserName = Convert.ToString(SqlRecordParams[4].Value).Trim();
                        ReturnMessage = Convert.ToString(SqlRecordParams[5].Value).Trim();


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

        #region ValidateBulkRecord
        private bool ValidateBulkRecord(BillingAccountList[] ArrAcctObj, ref string ReturnMessage)
        {
            bool bReturn = true;


            if (CycleID == new Guid("00000000-0000-0000-0000-000000000000"))
            {
                ReturnMessage = "229";
            }

            if (ArrAcctObj.Length == 0)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "235";
            }
            


            if (ReturnMessage.Trim() != string.Empty)
                bReturn = false;

            return bReturn;
        }


        #endregion

        #region GenerateAccountXML
        private bool GenerateAccountXML(BillingAccountList[] ArrObj, ref string CatchMessage)
        {
            bool bReturn = false;
            StringBuilder sbXML = new StringBuilder();

            try
            {
                if (ArrObj.Length > 0)
                {

                    sbXML.Append("<account>");

                    for (int i = 0; i < ArrObj.Length; i = i + 1)
                    {
                        sbXML.Append("<row>");
                        sbXML.Append("<billing_account_id><![CDATA[" + ArrObj[i].ID.ToString() + "]]></billing_account_id>");
                        sbXML.Append("<total_amount>" + ArrObj[i].TOTAL_AMOUNT.ToString() + "</total_amount>");
                        sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                        sbXML.Append("</row>");
                    }

                    sbXML.Append("</account>");
                    strXMLAcct = sbXML.ToString();


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

        #region GenerateInstitutionXML
        private bool GenerateInstitutionXML(InstitutionList[] ArrObj, ref string CatchMessage)
        {
            bool bReturn = false;
            StringBuilder sbXML = new StringBuilder();

            try
            {
                if (ArrObj.Length > 0)
                {

                    sbXML.Append("<institution>");

                    for (int i = 0; i < ArrObj.Length; i = i + 1)
                    {
                        sbXML.Append("<row>");
                        sbXML.Append("<institution_id><![CDATA[" + ArrObj[i].ID.ToString() + "]]></institution_id>");
                        sbXML.Append("<billing_account_id><![CDATA[" + ArrObj[i].BILLING_ACCOUNT_ID.ToString() + "]]></billing_account_id>");
                        sbXML.Append("<total_amount>" + ArrObj[i].TOTAL_AMOUNT.ToString() + "</total_amount>");
                        sbXML.Append("<approved><![CDATA[" + ArrObj[i].APPROVED.ToString() + "]]></approved>");
                        sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                        sbXML.Append("</row>");
                    }

                    sbXML.Append("</institution>");
                    strXMLInst = sbXML.ToString();


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

        #region GenerateStudyXML
        private bool GenerateStudyXML(StudyList[] ArrObj, ref string CatchMessage)
        {
            bool bReturn = false;
            StringBuilder sbXML = new StringBuilder();

            try
            {
                if (ArrObj.Length > 0)
                {

                    sbXML.Append("<study>");

                    for (int i = 0; i < ArrObj.Length; i = i + 1)
                    {
                        sbXML.Append("<row>");
                        sbXML.Append("<study_id><![CDATA[" + ArrObj[i].ID.ToString() + "]]></study_id>");
                        sbXML.Append("<institution_id><![CDATA[" + ArrObj[i].INSTITUTION_ID.ToString() + "]]></institution_id>");
                        sbXML.Append("<billing_account_id><![CDATA[" + ArrObj[i].BILLING_ACCOUNT_ID.ToString() + "]]></billing_account_id>");
                        sbXML.Append("<patient_name><![CDATA[" + ArrObj[i].PATIENT_NAME.ToString() + "]]></patient_name>");
                        sbXML.Append("<approved><![CDATA[" + ArrObj[i].APPROVED.ToString() + "]]></approved>");
                        sbXML.Append("<billed><![CDATA[" + ArrObj[i].BILLED.ToString() + "]]></billed>");
                        sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                        sbXML.Append("</row>");
                    }

                    sbXML.Append("</study>");
                    strXMLStudy = sbXML.ToString();


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

        #region Mail Sending

        #region FetchMailParameters
        public bool FetchMailParameters(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;

            try
            {
                SqlParameter[] SqlRecordParams = new SqlParameter[5];
                SqlRecordParams[0] = new SqlParameter("@billing_cycle_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = CycleID;
                SqlRecordParams[1] = new SqlParameter("@billing_account_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = AccountID;
                SqlRecordParams[2] = new SqlParameter("@institution_id", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = InstID;
                SqlRecordParams[3] = new SqlParameter("@mail_type", SqlDbType.NChar, 1); SqlRecordParams[3].Value = strMailType;
                SqlRecordParams[4] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[4].Value = UserID;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "invoicing_process_mail_param_fetch", SqlRecordParams);

                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "BillingCycle";
                    ds.Tables[1].TableName = "MaitTo";
                    ds.Tables[2].TableName = "MaitCC";
                    ds.Tables[3].TableName = "Sender";
                    ds.Tables[4].TableName = "Company";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }
            finally { ds.Dispose(); }


            return bReturn;
        }
        #endregion

        #endregion

        
    }

    public class BillingAccountList
    {
        #region Constructor
        public BillingAccountList()
        {
        }
        #endregion

        #region Variables
        Guid Id = Guid.Empty;
        double dblTotAmt = 0;
        #endregion

        #region Properties
        public Guid ID
        {
            get { return Id; }
            set { Id = value; }
        }
        public double TOTAL_AMOUNT
        {
            get { return dblTotAmt; }
            set { dblTotAmt = value; }
        }
        #endregion
    }

    public class InstitutionList
    {
        #region Constructor
        public InstitutionList()
        {
        }
        #endregion

        #region Variables
        Guid Id = Guid.Empty;
        Guid BillingAccountID = Guid.Empty;
        double dblTotAmt = 0;
        string strApproved = "N";
        #endregion

        #region Properties
        public Guid ID
        {
            get { return Id; }
            set { Id = value; }
        }
        public Guid BILLING_ACCOUNT_ID
        {
            get { return BillingAccountID; }
            set { BillingAccountID = value; }
        }
        public double TOTAL_AMOUNT
        {
            get { return dblTotAmt; }
            set { dblTotAmt = value; }
        }
        public string APPROVED
        {
            get { return strApproved; }
            set { strApproved = value; }
        }
        #endregion
    }

    public class StudyList
    {
        #region Constructor
        public StudyList()
        {
        }
        #endregion

        #region Variables
        Guid Id = Guid.Empty;
        Guid InstitutionID = Guid.Empty;
        Guid BillingAccountID = Guid.Empty;
        string strPatientName = string.Empty;
        string strBilled = "Y";
        string strApproved = "N";
        #endregion

        #region Properties
        public Guid ID
        {
            get { return Id; }
            set { Id = value; }
        }
        public Guid INSTITUTION_ID
        {
            get { return InstitutionID; }
            set { InstitutionID = value; }
        }
        public Guid BILLING_ACCOUNT_ID
        {
            get { return BillingAccountID; }
            set { BillingAccountID = value; }
        }
        public string PATIENT_NAME
        {
            get { return strPatientName; }
            set { strPatientName = value; }
        }
        public string BILLED
        {
            get { return strBilled; }
            set { strBilled = value; }
        }
        public string APPROVED
        {
            get { return strApproved; }
            set { strApproved = value; }
        }
        #endregion
    }
}
