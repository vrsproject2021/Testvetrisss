using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using VETRIS.DAL;

namespace VETRIS.Core.Master
{
    public class BillingAccount
    {
        #region Constructor
        public BillingAccount()
        {

        }
        #endregion

        #region Variables
        int intMenuID = 0;
        Guid UserID = new Guid("00000000-0000-0000-0000-000000000000");
        int intUserRoleID = 0;
        string strUserName = string.Empty;

        Guid Id = new Guid("00000000-0000-0000-0000-000000000000");
        string strCode = string.Empty;
        string strName = string.Empty;
        string strAddress1 = string.Empty;
        string strAddress2 = string.Empty;
        string strCity = string.Empty;
        int intStateID = 0;
        int intCountryID = 231;
        string strZip = string.Empty;
        string strPhone = string.Empty;
        string strFaxNo = string.Empty;
        string strEmail = string.Empty;
        string strContactPersonName = string.Empty;
        string strContactPersonMobile = string.Empty;
        string strContactPersonEmail = string.Empty;
        Guid SalespersonId = new Guid("00000000-0000-0000-0000-000000000000");
        string strLoginID = string.Empty;
        string strPwd = string.Empty;
        string strUserEmailID = string.Empty;
        string strUserMobileNo = string.Empty;
        string strNotificationPref = "B";
        decimal decCommission1stYr = 0;
        decimal decCommission2ndYr = 0;
        decimal decDiscPer = 0;
        string strAccountantName = string.Empty;
        string strIsActive = "Y";
        string strApplyDef = "N";
        Guid InstId = new Guid("00000000-0000-0000-0000-000000000000");
        string strInstName = string.Empty;
        
        string strXMLInst = string.Empty;
        string strXMLContact = string.Empty;
        string strXMLFee = string.Empty;
        string strXMLModalityFee = string.Empty;
        string strXMLServiceFee = string.Empty;
        string strXMLPhysician = string.Empty;

        string CatchMessage = string.Empty;

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
        public string USER_NAME
        {
            get { return strUserName; }
            set { strUserName = value; }
        }
        public Guid ID
        {
            get { return Id; }
            set { Id = value; }
        }
        public string CODE
        {
            get { return strCode; }
            set { strCode = value; }
        }
        public string NAME
        {
            get { return strName; }
            set { strName = value; }
        }
        public string ADDRESS_LINE1
        {
            get { return strAddress1; }
            set { strAddress1 = value; }
        }
        public string ADDRESS_LINE2
        {
            get { return strAddress2; }
            set { strAddress2 = value; }
        }
        public string CITY
        {
            get { return strCity; }
            set { strCity = value; }
        }
        public int STATE_ID
        {
            get { return intStateID; }
            set { intStateID = value; }
        }
        public int COUNTRY_ID
        {
            get { return intCountryID; }
            set { intCountryID = value; }
        }
        public string ZIP
        {
            get { return strZip; }
            set { strZip = value; }
        }
        public string PHONE
        {
            get { return strPhone; }
            set { strPhone = value; }
        }
        public string FAX
        {
            get { return strFaxNo; }
            set { strFaxNo = value; }
        }
        public string EMAIL_ID
        {
            get { return strEmail; }
            set { strEmail = value; }
        }
        public string CONTACT_PERSON_NAME
        {
            get { return strContactPersonName; }
            set { strContactPersonName = value; }
        }
        public string CONTACT_PERSION_MOBILE
        {
            get { return strContactPersonMobile; }
            set { strContactPersonMobile = value; }
        }
        public string CONTACT_PERSION_EMAIL_ID
        {
            get { return strContactPersonEmail; }
            set { strContactPersonEmail = value; }
        }
        public Guid SALES_PERSON_ID
        {
            get { return SalespersonId; }
            set { SalespersonId = value; }
        }
        public decimal COMMISSION_1ST_YEAR
        {
            get { return decCommission1stYr; }
            set { decCommission1stYr = value; }
        }
        public decimal COMMISSION_2ND_YEAR
        {
            get { return decCommission2ndYr; }
            set { decCommission2ndYr = value; }
        }
        public string LOGIN_ID
        {
            get { return strLoginID; }
            set { strLoginID = value; }
        }
        public string LOGIN_PASSWORD
        {
            get { return strPwd; }
            set { strPwd = value; }
        }
        public string USER_EMAIL_ID
        {
            get { return strUserEmailID; }
            set { strUserEmailID = value; }
        }
        public string USER_MOBILE_NUMBER
        {
            get { return strUserMobileNo; }
            set { strUserMobileNo = value; }
        }
        public string NOTIFICATION_PREFERENCE
        {
            get { return strNotificationPref; }
            set { strNotificationPref = value; }
        }
        public decimal DISCOUNT_PERCENT
        {
            get { return decDiscPer; }
            set { decDiscPer = value; }
        }
        public string ACCOUNTANT_NAME 
        {
            get { return strAccountantName; }
            set { strAccountantName = value; }
        }
        public Guid INSTITUTION_ID
        {
            get { return InstId; }
            set { InstId = value; }
        }
        public string INSTITUTION_NAME
        {
            get { return strInstName; }
            set { strInstName = value; }
        }
        public string IS_ACTIVE
        {
            get { return strIsActive; }
            set { strIsActive = value; }
        }
        public string APPLY_DEFAULT_FEES
        {
            get { return strApplyDef; }
            set { strApplyDef = value; }
        }
        #endregion

        #region Browser Methods

        #region FetchBrowserParameters
        public bool FetchBrowserParameters(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "master_brw_fetch_params");
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Country";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region SearchBrowserList
        public bool SearchBrowserList(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[8];
            SqlRecordParams[0] = new SqlParameter("@code", SqlDbType.NVarChar, 5); SqlRecordParams[0].Value = strCode;
            SqlRecordParams[1] = new SqlParameter("@name", SqlDbType.NVarChar, 100); SqlRecordParams[1].Value = strName;
            SqlRecordParams[2] = new SqlParameter("@city", SqlDbType.NVarChar, 100); SqlRecordParams[2].Value = strCity;
            SqlRecordParams[3] = new SqlParameter("@zip", SqlDbType.NVarChar, 20); SqlRecordParams[3].Value = strZip;
            SqlRecordParams[4] = new SqlParameter("@country_id", SqlDbType.NVarChar, 20); SqlRecordParams[4].Value = intCountryID;
            SqlRecordParams[5] = new SqlParameter("@state_id", SqlDbType.NVarChar, 20); SqlRecordParams[5].Value = intStateID;
            SqlRecordParams[6] = new SqlParameter("@is_active", SqlDbType.NChar, 1); SqlRecordParams[6].Value = strIsActive;
            SqlRecordParams[7] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[7].Value = UserID;


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "master_billing_account_fetch_brw", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "BrowserList";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #endregion

        #region Dialog Methods

        #region LoadDetails
        public bool LoadDetails(string ConfigPath, ref DataSet ds, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;

            SqlParameter[] SqlRecordParams = new SqlParameter[5];


            try
            {
                SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
                SqlRecordParams[1] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[1].Value = intMenuID;
                SqlRecordParams[2] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = UserID;
                SqlRecordParams[3] = new SqlParameter("@error_code", SqlDbType.VarChar, 10); SqlRecordParams[3].Direction = ParameterDirection.Output;
                SqlRecordParams[4] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[4].Direction = ParameterDirection.Output;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "master_billing_account_fetch_dtls", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Details";
                    ds.Tables[1].TableName = "Country";
                    ds.Tables[2].TableName = "States";
                    ds.Tables[3].TableName = "SalesPersons";

                    #region Details
                    foreach (DataRow dr in ds.Tables["Details"].Rows)
                    {
                        strCode = Convert.ToString(dr["code"]).Trim();
                        strName = Convert.ToString(dr["name"]).Trim();
                        strAddress1 = Convert.ToString(dr["address_1"]).Trim();
                        strAddress2 = Convert.ToString(dr["address_2"]).Trim();
                        strCity = Convert.ToString(dr["city"]).Trim();
                        intStateID = Convert.ToInt32(dr["state_id"]);
                        intCountryID = Convert.ToInt32(dr["country_id"]);
                        strZip = Convert.ToString(dr["zip"]).Trim();
                        strEmail = Convert.ToString(dr["email_id"]).Trim();
                        strPhone = Convert.ToString(dr["phone_no"]).Trim();
                        strFaxNo = Convert.ToString(dr["fax_no"]).Trim();
                        strContactPersonName = Convert.ToString(dr["contact_person_name"]).Trim();
                        strContactPersonMobile = Convert.ToString(dr["contact_person_mobile"]).Trim();
                        strContactPersonEmail = Convert.ToString(dr["contact_person_email_id"]).Trim();
                        SalespersonId = new Guid(Convert.ToString(dr["salesperson_id"]));
                        decCommission1stYr = Convert.ToDecimal(dr["commission_1st_yr"]);
                        decCommission2ndYr = Convert.ToDecimal(dr["commission_2nd_yr"]);
                        strLoginID = Convert.ToString(dr["login_id"]).Trim();
                        strPwd = Convert.ToString(dr["login_pwd"]).Trim();if(strPwd != string.Empty) strPwd = CoreCommon.DecryptString(strPwd);
                        strUserEmailID = Convert.ToString(dr["user_email_id"]).Trim();
                        strUserMobileNo = Convert.ToString(dr["user_mobile_no"]).Trim();
                        strNotificationPref = Convert.ToString(dr["notification_pref"]).Trim();
                        decDiscPer = Convert.ToDecimal(dr["discount_per"]); 
                        strAccountantName = Convert.ToString(dr["accountant_name"]).Trim();
                        strIsActive = Convert.ToString(dr["is_active"]).Trim();
                    }
                    #endregion

                    bReturn = true;
                }
                else
                {
                    bReturn = false;
                    ReturnMessage = Convert.ToString(SqlRecordParams[3].Value);
                }

            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region LoadInstitutions
        public bool LoadInstitutions(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "master_billing_account_fetch_institutions", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Institutions";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region LoadContacts
        public bool LoadContacts(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "master_billing_account_fetch_contacts", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Contacts";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region LoadPhysicians
        public bool LoadPhysicians(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "master_billing_account_fetch_physicians", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Institutions";
                    ds.Tables[1].TableName = "Physicians";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        

        #region LoadModalityFees
        public bool LoadModalityFees(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[2];
            SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
            SqlRecordParams[1] = new SqlParameter("@apply_default", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = strApplyDef;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "master_billing_account_modality_fees_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "ModalityFees";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region LoadServiceFees
        public bool LoadServiceFees(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[2];
            SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
            SqlRecordParams[1] = new SqlParameter("@apply_default", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = strApplyDef;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "master_billing_account_service_fees_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "ServiceFees";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region FetchCountryWiseStates
        public bool FetchCountryWiseStates(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;

            SqlParameter[] SqlRecordParams = new SqlParameter[1];


            try
            {
                SqlRecordParams[0] = new SqlParameter("@country_id", SqlDbType.Int); SqlRecordParams[0].Value = intCountryID;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "country_wise_state_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "States";
                    bReturn = true;
                }
                else
                {
                    bReturn = false;
                }

            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region FechContactDetails
        public bool FechContactDetails(string ConfigPath, ref string CatchMessage)
        {
            bool bReturn = false;
            DataSet ds = new DataSet();
            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            SqlRecordParams[0] = new SqlParameter("@institution_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = InstId;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "master_billing_account_fetch_contact_details", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Contact";
                    foreach (DataRow dr in ds.Tables["Contact"].Rows)
                    {
                        strInstName = Convert.ToString(dr["institution_name"]).Trim();
                        strPhone = Convert.ToString(dr["phone_no"]).Trim();
                        strFaxNo = Convert.ToString(dr["fax_no"]).Trim();
                        strContactPersonName = Convert.ToString(dr["contact_person_name"]).Trim();
                        strContactPersonMobile = Convert.ToString(dr["contact_person_mobile"]).Trim();
                        strContactPersonEmail = Convert.ToString(dr["contact_person_email_id"]).Trim();

                    }

                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            finally { ds.Dispose(); }
            return bReturn;
        }
        #endregion

        #region FetchPhysicianDetails
        public bool FetchPhysicianDetails(string ConfigPath,ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            SqlRecordParams[0] = new SqlParameter("@institution_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = InstId;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "master_billing_account_fetch_physician_details", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Institutions";
                    ds.Tables[1].TableName = "Physicians";
                    

                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }
        #endregion

        #region Suspended

        #region LoadFees
        //public bool LoadFees(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        //{
        //    bool bReturn = false;
        //    SqlParameter[] SqlRecordParams = new SqlParameter[1];
        //    SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;

        //    try
        //    {
        //        if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
        //        ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "master_billing_account_fetch_fees", SqlRecordParams);
        //        if (ds.Tables.Count > 0)
        //        {
        //            ds.Tables[0].TableName = "Fees";
        //        }
        //        bReturn = true;
        //    }
        //    catch (Exception expErr)
        //    { bReturn = false; CatchMessage = expErr.Message; }


        //    return bReturn;
        //}
        #endregion

        #region SaveRecord
        //public bool SaveRecord(string ConfigPath, BAInstitutionList[] ArrInstObj, ContactList[] ArrContObj, PhysicianList[] ArrPhysObj, FeeList[] ArrFeeObj, ref string ReturnMessage, ref string CatchMessage)
        //{
        //    bool bReturn = false;
        //    int intReturnValue = 0; int intExecReturn = 0;

        //    if (ValidateRecord(ArrInstObj, ArrContObj,ArrFeeObj, ref ReturnMessage))
        //    {
        //        if ((GenerateInstitutionXML(ArrInstObj, ref CatchMessage) && (GenerateContactXML(ArrContObj, ref ReturnMessage, ref CatchMessage)) && (GeneratePhysicianXML(ArrPhysObj, ref ReturnMessage, ref CatchMessage)) && (GenerateFeeXML(ArrFeeObj, ref ReturnMessage, ref CatchMessage))))
        //        {
        //            try
        //            {
        //                SqlParameter[] SqlRecordParams = new SqlParameter[30];
        //                SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id; SqlRecordParams[0].Direction = ParameterDirection.InputOutput;
        //                SqlRecordParams[1] = new SqlParameter("@code", SqlDbType.NVarChar, 5); SqlRecordParams[1].Value = strCode; SqlRecordParams[1].Direction = ParameterDirection.InputOutput;
        //                SqlRecordParams[2] = new SqlParameter("@name", SqlDbType.NVarChar, 41); SqlRecordParams[2].Value = strName;
        //                SqlRecordParams[3] = new SqlParameter("@address_Line1", SqlDbType.NVarChar, 100); SqlRecordParams[3].Value = strAddress1;
        //                SqlRecordParams[4] = new SqlParameter("@address_Line2", SqlDbType.NVarChar, 100); SqlRecordParams[4].Value = strAddress2;
        //                SqlRecordParams[5] = new SqlParameter("@city", SqlDbType.NVarChar, 100); SqlRecordParams[5].Value = strCity;
        //                SqlRecordParams[6] = new SqlParameter("@state_id", SqlDbType.Int); SqlRecordParams[6].Value = intStateID;
        //                SqlRecordParams[7] = new SqlParameter("@country_id", SqlDbType.Int); SqlRecordParams[7].Value = intCountryID;
        //                SqlRecordParams[8] = new SqlParameter("@zip", SqlDbType.NVarChar, 20); SqlRecordParams[8].Value = strZip;
        //                SqlRecordParams[9] = new SqlParameter("@email_id", SqlDbType.NVarChar, 50); SqlRecordParams[9].Value = strEmail;
        //                //SqlRecordParams[10] = new SqlParameter("@phone_no", SqlDbType.NVarChar, 30); SqlRecordParams[10].Value = strPhone;
        //                //SqlRecordParams[11] = new SqlParameter("@fax_no", SqlDbType.NVarChar, 20); SqlRecordParams[11].Value = strFaxNo;
        //                //SqlRecordParams[12] = new SqlParameter("@contact_person_name", SqlDbType.NVarChar, 100); SqlRecordParams[12].Value = strContactPersonName;
        //                //SqlRecordParams[13] = new SqlParameter("@contact_person_mobile", SqlDbType.NVarChar, 100); SqlRecordParams[13].Value = strContactPersonMobile;
        //                //SqlRecordParams[14] = new SqlParameter("@contact_person_email_id", SqlDbType.NVarChar, 50); SqlRecordParams[14].Value = strContactPersonEmail;
        //                SqlRecordParams[10] = new SqlParameter("@salesperson_id", SqlDbType.UniqueIdentifier); SqlRecordParams[10].Value = SalespersonId;
        //                SqlRecordParams[11] = new SqlParameter("@commission_1st_yr", SqlDbType.Decimal); SqlRecordParams[11].Value = decCommission1stYr;
        //                SqlRecordParams[12] = new SqlParameter("@commission_2nd_yr", SqlDbType.Decimal); SqlRecordParams[12].Value = decCommission2ndYr;
        //                SqlRecordParams[13] = new SqlParameter("@login_id", SqlDbType.NVarChar, 50); SqlRecordParams[13].Value = strLoginID;
        //                SqlRecordParams[14] = new SqlParameter("@login_pwd", SqlDbType.NVarChar, 200); SqlRecordParams[14].Value = strPwd;
        //                SqlRecordParams[15] = new SqlParameter("@user_email_id", SqlDbType.NVarChar, 50); SqlRecordParams[15].Value = strUserEmailID;
        //                SqlRecordParams[16] = new SqlParameter("@user_mobile_no ", SqlDbType.NVarChar, 20); SqlRecordParams[16].Value = strUserMobileNo;
        //                SqlRecordParams[17] = new SqlParameter("@notification_pref", SqlDbType.NChar, 1); SqlRecordParams[17].Value = strNotificationPref;
        //                SqlRecordParams[18] = new SqlParameter("@discount_per", SqlDbType.Decimal); SqlRecordParams[18].Value = decDiscPer;
        //                SqlRecordParams[19] = new SqlParameter("@accountant_name", SqlDbType.NVarChar, 250); SqlRecordParams[19].Value = strAccountantName;
        //                SqlRecordParams[20] = new SqlParameter("@is_active", SqlDbType.NChar, 1); SqlRecordParams[20].Value = strIsActive;
        //                SqlRecordParams[21] = new SqlParameter("@xml_institution", SqlDbType.NText); if (strXMLInst.Trim() != string.Empty) SqlRecordParams[21].Value = strXMLInst.Trim(); else SqlRecordParams[21].Value = DBNull.Value;
        //                SqlRecordParams[22] = new SqlParameter("@xml_contacts", SqlDbType.NText); if (strXMLContact.Trim() != string.Empty) SqlRecordParams[22].Value = strXMLContact.Trim(); else SqlRecordParams[22].Value = DBNull.Value;
        //                SqlRecordParams[23] = new SqlParameter("@xml_fees", SqlDbType.NText); if (strXMLFee.Trim() != string.Empty) SqlRecordParams[23].Value = strXMLFee.Trim(); else SqlRecordParams[23].Value = DBNull.Value;
        //                SqlRecordParams[24] = new SqlParameter("@xml_physicians", SqlDbType.NText); if (strXMLPhysician.Trim() != string.Empty) SqlRecordParams[24].Value = strXMLPhysician.Trim(); else SqlRecordParams[24].Value = DBNull.Value;
        //                SqlRecordParams[25] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[25].Value = UserID;
        //                SqlRecordParams[26] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[26].Value = intMenuID;
        //                SqlRecordParams[27] = new SqlParameter("@user_name", SqlDbType.NVarChar, 700); SqlRecordParams[27].Direction = ParameterDirection.Output;
        //                SqlRecordParams[28] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[28].Direction = ParameterDirection.Output;
        //                SqlRecordParams[29] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[29].Direction = ParameterDirection.Output;
                       

        //                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
        //                intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "master_billing_account_save", SqlRecordParams);
        //                intReturnValue = Convert.ToInt32(SqlRecordParams[29].Value);
        //                if (intReturnValue == 1)
        //                    bReturn = true;
        //                else
        //                    bReturn = false;

        //                strUserName = Convert.ToString(SqlRecordParams[27].Value).Trim();
        //                ReturnMessage = Convert.ToString(SqlRecordParams[28].Value).Trim();

        //                Id = new Guid(Convert.ToString(SqlRecordParams[0].Value));
        //                strCode = Convert.ToString(SqlRecordParams[1].Value).Trim();

        //            }
        //            catch (Exception expErr)
        //            { bReturn = false; CatchMessage = expErr.Message; }
        //        }
        //        else
        //        {
        //            bReturn = false;

        //        }

        //    }
        //    else
        //    {
        //        bReturn = false;
        //    }

        //    return bReturn;
        //}
        #endregion

        #region ValidateRecord
        //private bool ValidateRecord(BAInstitutionList[] ArrInstObj, ContactList[] ArrContObj, FeeList[] ArrFeeObj, ref string ReturnMessage)
        //{
        //    bool bReturn = true;


        //    if (strName.Trim() == string.Empty)
        //    {
        //        if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
        //        ReturnMessage += "076";
        //    }

        //    if (strIsActive == "Y")
        //    {
        //        if (strLoginID.Trim() == string.Empty)
        //        {
        //            if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
        //            ReturnMessage += "112";
        //        }
        //        if (strPwd.Trim() == string.Empty)
        //        {
        //            if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
        //            ReturnMessage += "120";
        //        }
        //        if ((strNotificationPref == "E") || (strNotificationPref == "B"))
        //        {
        //            if (strUserEmailID.Trim() == string.Empty)
        //            {
        //                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
        //                ReturnMessage += "219";
        //            }
        //            else
        //            {
        //                if (!CoreCommon.IsEmailValid(strUserEmailID))
        //                {
        //                    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
        //                    ReturnMessage += "084";
        //                }
        //            }
        //        }

        //        if ((strNotificationPref == "S") || (strNotificationPref == "B"))
        //        {
        //            if (strUserMobileNo.Trim() == string.Empty)
        //            {
        //                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
        //                ReturnMessage += "220";
        //            }
        //        }

        //        if (ArrInstObj.Length == 0)
        //        {
        //            if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
        //            ReturnMessage += "218";
        //        }
        //        if (ArrContObj.Length != ArrInstObj.Length)
        //        {
        //            if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
        //            ReturnMessage += "226";
        //        }
        //        if (ArrFeeObj.Length == 0)
        //        {
        //            if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
        //            ReturnMessage += "137";
        //        }
        //    }


        //    if (ReturnMessage.Trim() != string.Empty)
        //        bReturn = false;

        //    return bReturn;
        //}
        #endregion

        #region GenerateFeeXML
        //private bool GenerateFeeXML(FeeList[] ArrObj, ref string ReturnMessage, ref string CatchMessage)
        //{
        //    bool bReturn = false;
        //    StringBuilder sbXML = new StringBuilder();

        //    try
        //    {
        //        if (ArrObj.Length > 0)
        //        {

        //            sbXML.Append("<fees>");

        //            for (int i = 0; i < ArrObj.Length; i = i + 1)
        //            {
        //                sbXML.Append("<row>");
        //                sbXML.Append("<rate_id><![CDATA[" + ArrObj[i].RATE_ID.ToString() + "]]></rate_id>");
        //                sbXML.Append("<fee_amount>" + ArrObj[i].FEE_AMOUNT.ToString() + "</fee_amount>");
        //                sbXML.Append("<fee_row_id>" + ArrObj[i].ROW_ID.ToString() + "</fee_row_id>");
        //                sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
        //                sbXML.Append("</row>");
        //            }

        //            sbXML.Append("</fees>");

        //        }

        //        strXMLFee = sbXML.ToString();
        //        bReturn = true;

        //    }
        //    catch (Exception LexpErr)
        //    {
        //        bReturn = false;
        //        CatchMessage = LexpErr.Message;
        //    }
        //    return bReturn;
        //}
        #endregion

        #endregion

        #region SaveRecord
        public bool SaveRecord(string ConfigPath, BAInstitutionList[] ArrInstObj, ContactList[] ArrContObj, PhysicianList[] ArrPhysObj, ModalityFeeList[] ArrModFeeObj, ServiceFeeList[] ArrSvcFeeObj, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;

            if (ValidateRecord(ArrInstObj, ArrContObj, ArrModFeeObj,ArrSvcFeeObj, ref ReturnMessage))
            {
                if ((GenerateInstitutionXML(ArrInstObj, ref CatchMessage) && (GenerateContactXML(ArrContObj, ref ReturnMessage, ref CatchMessage)) && (GeneratePhysicianXML(ArrPhysObj, ref ReturnMessage, ref CatchMessage)) && (GenerateModalityFeeXML(ArrModFeeObj, ref ReturnMessage, ref CatchMessage)) && (GenerateServiceFeeXML(ArrSvcFeeObj, ref ReturnMessage, ref CatchMessage))))
                {
                    try
                    {
                        SqlParameter[] SqlRecordParams = new SqlParameter[31];
                        SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id; SqlRecordParams[0].Direction = ParameterDirection.InputOutput;
                        SqlRecordParams[1] = new SqlParameter("@code", SqlDbType.NVarChar, 5); SqlRecordParams[1].Value = strCode; SqlRecordParams[1].Direction = ParameterDirection.InputOutput;
                        SqlRecordParams[2] = new SqlParameter("@name", SqlDbType.NVarChar, 100); SqlRecordParams[2].Value = strName;
                        SqlRecordParams[3] = new SqlParameter("@address_Line1", SqlDbType.NVarChar, 100); SqlRecordParams[3].Value = strAddress1;
                        SqlRecordParams[4] = new SqlParameter("@address_Line2", SqlDbType.NVarChar, 100); SqlRecordParams[4].Value = strAddress2;
                        SqlRecordParams[5] = new SqlParameter("@city", SqlDbType.NVarChar, 100); SqlRecordParams[5].Value = strCity;
                        SqlRecordParams[6] = new SqlParameter("@state_id", SqlDbType.Int); SqlRecordParams[6].Value = intStateID;
                        SqlRecordParams[7] = new SqlParameter("@country_id", SqlDbType.Int); SqlRecordParams[7].Value = intCountryID;
                        SqlRecordParams[8] = new SqlParameter("@zip", SqlDbType.NVarChar, 20); SqlRecordParams[8].Value = strZip;
                        SqlRecordParams[9] = new SqlParameter("@email_id", SqlDbType.NVarChar, 100); SqlRecordParams[9].Value = strEmail;
                        //SqlRecordParams[10] = new SqlParameter("@phone_no", SqlDbType.NVarChar, 30); SqlRecordParams[10].Value = strPhone;
                        //SqlRecordParams[11] = new SqlParameter("@fax_no", SqlDbType.NVarChar, 20); SqlRecordParams[11].Value = strFaxNo;
                        //SqlRecordParams[12] = new SqlParameter("@contact_person_name", SqlDbType.NVarChar, 100); SqlRecordParams[12].Value = strContactPersonName;
                        //SqlRecordParams[13] = new SqlParameter("@contact_person_mobile", SqlDbType.NVarChar, 100); SqlRecordParams[13].Value = strContactPersonMobile;
                        //SqlRecordParams[14] = new SqlParameter("@contact_person_email_id", SqlDbType.NVarChar, 50); SqlRecordParams[14].Value = strContactPersonEmail;
                        SqlRecordParams[10] = new SqlParameter("@salesperson_id", SqlDbType.UniqueIdentifier); SqlRecordParams[10].Value = SalespersonId;
                        SqlRecordParams[11] = new SqlParameter("@commission_1st_yr", SqlDbType.Decimal); SqlRecordParams[11].Value = decCommission1stYr;
                        SqlRecordParams[12] = new SqlParameter("@commission_2nd_yr", SqlDbType.Decimal); SqlRecordParams[12].Value = decCommission2ndYr;
                        SqlRecordParams[13] = new SqlParameter("@login_id", SqlDbType.NVarChar, 50); SqlRecordParams[13].Value = strLoginID;
                        SqlRecordParams[14] = new SqlParameter("@login_pwd", SqlDbType.NVarChar, 200); SqlRecordParams[14].Value = strPwd;
                        SqlRecordParams[15] = new SqlParameter("@user_email_id", SqlDbType.NVarChar, 100); SqlRecordParams[15].Value = strUserEmailID;
                        SqlRecordParams[16] = new SqlParameter("@user_mobile_no ", SqlDbType.NVarChar, 20); SqlRecordParams[16].Value = strUserMobileNo;
                        SqlRecordParams[17] = new SqlParameter("@notification_pref", SqlDbType.NChar, 1); SqlRecordParams[17].Value = strNotificationPref;
                        SqlRecordParams[18] = new SqlParameter("@discount_per", SqlDbType.Decimal); SqlRecordParams[18].Value = decDiscPer;
                        SqlRecordParams[19] = new SqlParameter("@accountant_name", SqlDbType.NVarChar, 250); SqlRecordParams[19].Value = strAccountantName;
                        SqlRecordParams[20] = new SqlParameter("@is_active", SqlDbType.NChar, 1); SqlRecordParams[20].Value = strIsActive;
                        SqlRecordParams[21] = new SqlParameter("@xml_institution", SqlDbType.NText); if (strXMLInst.Trim() != string.Empty) SqlRecordParams[21].Value = strXMLInst.Trim(); else SqlRecordParams[21].Value = DBNull.Value;
                        SqlRecordParams[22] = new SqlParameter("@xml_contacts", SqlDbType.NText); if (strXMLContact.Trim() != string.Empty) SqlRecordParams[22].Value = strXMLContact.Trim(); else SqlRecordParams[22].Value = DBNull.Value;
                        SqlRecordParams[23] = new SqlParameter("@xml_modality_fees", SqlDbType.NText); if (strXMLModalityFee.Trim() != string.Empty) SqlRecordParams[23].Value = strXMLModalityFee.Trim(); else SqlRecordParams[23].Value = DBNull.Value;
                        SqlRecordParams[24] = new SqlParameter("@xml_service_fees", SqlDbType.NText); if (strXMLServiceFee.Trim() != string.Empty) SqlRecordParams[24].Value = strXMLServiceFee.Trim(); else SqlRecordParams[24].Value = DBNull.Value;
                        SqlRecordParams[25] = new SqlParameter("@xml_physicians", SqlDbType.NText); if (strXMLPhysician.Trim() != string.Empty) SqlRecordParams[25].Value = strXMLPhysician.Trim(); else SqlRecordParams[25].Value = DBNull.Value;
                        SqlRecordParams[26] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[26].Value = UserID;
                        SqlRecordParams[27] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[27].Value = intMenuID;
                        SqlRecordParams[28] = new SqlParameter("@user_name", SqlDbType.NVarChar, 700); SqlRecordParams[28].Direction = ParameterDirection.Output;
                        SqlRecordParams[29] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[29].Direction = ParameterDirection.Output;
                        SqlRecordParams[30] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[30].Direction = ParameterDirection.Output;


                        if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                        intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "master_billing_account_save", SqlRecordParams);
                        intReturnValue = Convert.ToInt32(SqlRecordParams[30].Value);
                        if (intReturnValue == 1)
                            bReturn = true;
                        else
                            bReturn = false;

                        strUserName = Convert.ToString(SqlRecordParams[28].Value).Trim();
                        ReturnMessage = Convert.ToString(SqlRecordParams[29].Value).Trim();

                        Id = new Guid(Convert.ToString(SqlRecordParams[0].Value));
                        strCode = Convert.ToString(SqlRecordParams[1].Value).Trim();

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
        private bool ValidateRecord(BAInstitutionList[] ArrInstObj, ContactList[] ArrContObj, ModalityFeeList[] ArrModFeeObj, ServiceFeeList[] ArrSvcFeeObj, ref string ReturnMessage)
        {
            bool bReturn = true;


            if (strName.Trim() == string.Empty)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "076";
            }

            if (strIsActive == "Y")
            {
                if (strLoginID.Trim() == string.Empty)
                {
                    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                    ReturnMessage += "112";
                }
                if (strPwd.Trim() == string.Empty)
                {
                    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                    ReturnMessage += "120";
                }
                if ((strNotificationPref == "E") || (strNotificationPref == "B"))
                {
                    if (strUserEmailID.Trim() == string.Empty)
                    {
                        if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                        ReturnMessage += "219";
                    }
                    else
                    {
                        if (!CoreCommon.IsEmailValid(strUserEmailID))
                        {
                            if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                            ReturnMessage += "084";
                        }
                    }
                }

                if ((strNotificationPref == "S") || (strNotificationPref == "B"))
                {
                    if (strUserMobileNo.Trim() == string.Empty)
                    {
                        if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                        ReturnMessage += "220";
                    }
                }

                if (ArrInstObj.Length == 0)
                {
                    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                    ReturnMessage += "218";
                }
                if (ArrContObj.Length != ArrInstObj.Length)
                {
                    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                    ReturnMessage += "226";
                }
                if (ArrModFeeObj.Length == 0)
                {
                    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                    ReturnMessage += "137";
                }
                if (ArrSvcFeeObj.Length == 0)
                {
                    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                    ReturnMessage += "456";
                }
            }


            if (ReturnMessage.Trim() != string.Empty)
                bReturn = false;

            return bReturn;
        }


        #endregion

        #region GenerateInstitutionXML
        private bool GenerateInstitutionXML(BAInstitutionList[] ArrObj, ref string CatchMessage)
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
                        sbXML.Append("<institution_id>" + ArrObj[i].ID.ToString() + "</institution_id>");
                        sbXML.Append("<consult_applicable><![CDATA[" + ArrObj[i].CONSULT_APPLICABLE + "]]></consult_applicable>");
                        sbXML.Append("<storage_applicable><![CDATA[" + ArrObj[i].STORAGE_APPLICABLE + "]]></storage_applicable>");
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

        #region GenerateModalityFeeXML
        private bool GenerateModalityFeeXML(ModalityFeeList[] ArrObj, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            StringBuilder sbXML = new StringBuilder();

            try
            {
                if (ArrObj.Length > 0)
                {

                    sbXML.Append("<fees>");

                    for (int i = 0; i < ArrObj.Length; i = i + 1)
                    {
                        sbXML.Append("<row>");
                        sbXML.Append("<rate_id><![CDATA[" + ArrObj[i].RATE_ID.ToString() + "]]></rate_id>");
                        sbXML.Append("<fee_amount>" + ArrObj[i].FEE_AMOUNT.ToString() + "</fee_amount>");
                        sbXML.Append("<fee_amount_per_unit>" + ArrObj[i].FEE_AMOUNT_PER_UNIT.ToString() + "</fee_amount_per_unit>");
                        sbXML.Append("<study_max_amount>" + ArrObj[i].MAXIMUM_STUDY_FEE_AMOUNT.ToString() + "</study_max_amount>");
                        sbXML.Append("<fee_row_id>" + ArrObj[i].ROW_ID.ToString() + "</fee_row_id>");
                        sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                        sbXML.Append("</row>");
                    }

                    sbXML.Append("</fees>");

                }

                strXMLModalityFee = sbXML.ToString();
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

        #region GenerateServiceFeeXML
        private bool GenerateServiceFeeXML(ServiceFeeList[] ArrObj, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            StringBuilder sbXML = new StringBuilder();

            try
            {
                if (ArrObj.Length > 0)
                {

                    sbXML.Append("<fees>");

                    for (int i = 0; i < ArrObj.Length; i = i + 1)
                    {
                        sbXML.Append("<row>");
                        sbXML.Append("<rate_id><![CDATA[" + ArrObj[i].RATE_ID.ToString() + "]]></rate_id>");
                        sbXML.Append("<fee_amount>" + ArrObj[i].FEE_AMOUNT.ToString() + "</fee_amount>");
                        sbXML.Append("<fee_amount_after_hrs>" + ArrObj[i].FEE_AMOUNT_AFTER_HOURS.ToString() + "</fee_amount_after_hrs>");
                        sbXML.Append("<fee_row_id>" + ArrObj[i].ROW_ID.ToString() + "</fee_row_id>");
                        sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                        sbXML.Append("</row>");
                    }

                    sbXML.Append("</fees>");

                }

                strXMLServiceFee = sbXML.ToString();
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

        #region GenerateContactXML
        private bool GenerateContactXML(ContactList[] ArrObj, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            StringBuilder sbXML = new StringBuilder();

            try
            {
                if (ArrObj.Length > 0)
                {

                    sbXML.Append("<contact>");

                    for (int i = 0; i < ArrObj.Length; i = i + 1)
                    {
                        sbXML.Append("<row>");
                        sbXML.Append("<institution_id><![CDATA[" + ArrObj[i].INSTITUTION_ID.ToString() + "]]></institution_id>");
                        sbXML.Append("<phone_no><![CDATA[" + ArrObj[i].PHONE_NUMBER + "]]></phone_no>");
                        sbXML.Append("<fax_no><![CDATA[" + ArrObj[i].FAX_NUMBER + "]]></fax_no>");
                        sbXML.Append("<contact_person_name><![CDATA[" + ArrObj[i].CONTACT_PERSON + "]]></contact_person_name>");
                        sbXML.Append("<contact_person_mobile><![CDATA[" + ArrObj[i].CONTACT_PERSON_MOBILE_NUMBER + "]]></contact_person_mobile>");
                        sbXML.Append("<contact_person_email_id><![CDATA[" + ArrObj[i].CONTACT_PERSON_EMAIL_ID + "]]></contact_person_email_id>");
                        sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                        sbXML.Append("</row>");
                    }

                    sbXML.Append("</contact>");

                }

                strXMLContact = sbXML.ToString();
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

        #region GeneratePhysicianXML
        private bool GeneratePhysicianXML(PhysicianList[] ArrObj, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            StringBuilder sbXML = new StringBuilder();

            try
            {
                if (ArrObj.Length > 0)
                {

                    sbXML.Append("<physician>");

                    for (int i = 0; i < ArrObj.Length; i = i + 1)
                    {
                        #region Validations
                        if ((ArrObj[i].FIRST_NAME.Trim() + " " + ArrObj[i].LAST_NAME.Trim()).Trim() == string.Empty)
                        {
                            if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                            ReturnMessage = "087";
                            strUserName = (i + 1).ToString();
                        }


                       
                        if (ReturnMessage.Trim() != string.Empty)
                        {
                            break;
                        }
                        #endregion

                        sbXML.Append("<row>");
                        sbXML.Append("<institution_id>" + ArrObj[i].INSTITUTION_ID.ToString() + "</institution_id>");
                        sbXML.Append("<physician_id>" + ArrObj[i].ID.ToString() + "</physician_id>");
                        sbXML.Append("<physician_fname><![CDATA[" + ArrObj[i].FIRST_NAME + "]]></physician_fname>");
                        sbXML.Append("<physician_lname><![CDATA[" + ArrObj[i].LAST_NAME + "]]></physician_lname>");
                        sbXML.Append("<physician_credentials><![CDATA[" + ArrObj[i].CREDENTIALS + "]]></physician_credentials>");
                        sbXML.Append("<physician_email><![CDATA[" + ArrObj[i].EMAIL_ID + "]]></physician_email>");
                        sbXML.Append("<physician_mobile><![CDATA[" + ArrObj[i].MOBILE_NUMBER + "]]></physician_mobile>");
                        sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                        sbXML.Append("</row>");
                    }

                    if (ReturnMessage.Trim() != string.Empty)
                    {
                        bReturn = false;
                        sbXML.Clear();
                        strXMLPhysician = string.Empty;
                    }
                    else
                    {
                        bReturn = true;
                        sbXML.Append("</physician>");
                        strXMLPhysician = sbXML.ToString();
                    }


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

        #endregion
    }

    public class BAInstitutionList
    {
        #region Constructor
        public BAInstitutionList()
        {
        }
        #endregion

        #region Variables
        Guid InstitutionID = Guid.Empty;
        string strConsultApplicable = "N";
        string strStorageApplicable = "N";
        #endregion

        #region Properties
        public Guid ID
        {
            get { return InstitutionID; }
            set { InstitutionID = value; }
        }
        public string CONSULT_APPLICABLE
        {
            get { return strConsultApplicable; }
            set { strConsultApplicable = value; }
        }
        public string STORAGE_APPLICABLE
        {
            get { return strStorageApplicable; }
            set { strStorageApplicable = value; }
        }
        #endregion
    }
    public class ContactList
    {
        #region Constructor
        public ContactList()
        {
        }
        #endregion

        #region Variables
        Guid InstId = Guid.Empty;
        string strPhoneNo = string.Empty;
        string strFaxNo = string.Empty;
        string strContactPerson = string.Empty;
        string strContactPersonEmail = string.Empty;
        string strContPersonMobileNo = string.Empty;
        #endregion

        #region Properties
        public Guid INSTITUTION_ID
        {
            get { return InstId; }
            set { InstId = value; }
        }
        public string PHONE_NUMBER
        {
            get { return strPhoneNo; }
            set { strPhoneNo = value; }
        }
        public string FAX_NUMBER
        {
            get { return strFaxNo; }
            set { strFaxNo = value; }
        }
        public string CONTACT_PERSON
        {
            get { return strContactPerson; }
            set { strContactPerson = value; }
        }
        public string CONTACT_PERSON_EMAIL_ID
        {
            get { return strContactPersonEmail; }
            set { strContactPersonEmail = value; }
        }
        public string CONTACT_PERSON_MOBILE_NUMBER
        {
            get { return strContPersonMobileNo; }
            set { strContPersonMobileNo = value; }
        }
        #endregion
    }
    public class ModalityFeeList
    {
        #region Constructor
        public ModalityFeeList()
        {
        }
        #endregion

        #region Variables
        int intRowID = 0;
        Guid RateId = Guid.Empty;
        double dblFee = 0;
        double dblFeePerUnit = 0;
        double dblStudyMaxFee = 0;
        #endregion

        #region Properties
        public Guid RATE_ID
        {
            get { return RateId; }
            set { RateId = value; }
        }
        public double FEE_AMOUNT
        {
            get { return dblFee; }
            set { dblFee = value; }
        }
        public double FEE_AMOUNT_PER_UNIT
        {
            get { return dblFeePerUnit; }
            set { dblFeePerUnit = value; }
        }
        public double MAXIMUM_STUDY_FEE_AMOUNT
        {
            get { return dblStudyMaxFee; }
            set { dblStudyMaxFee = value; }
        }
        public int ROW_ID
        {
            get { return intRowID; }
            set { intRowID = value; }
        }
        #endregion
    }
    public class ServiceFeeList
    {
        #region Constructor
        public ServiceFeeList()
        {
        }
        #endregion

        #region Variables
        int intRowID = 0;
        Guid RateId = Guid.Empty;
        double dblFee = 0;
        double dblFeeAfterHr = 0;
        #endregion

        #region Properties
        public Guid RATE_ID
        {
            get { return RateId; }
            set { RateId = value; }
        }
        public double FEE_AMOUNT
        {
            get { return dblFee; }
            set { dblFee = value; }
        }
        public double FEE_AMOUNT_AFTER_HOURS
        {
            get { return dblFeeAfterHr; }
            set { dblFeeAfterHr = value; }
        }
        
        public int ROW_ID
        {
            get { return intRowID; }
            set { intRowID = value; }
        }
        #endregion
    }
}
