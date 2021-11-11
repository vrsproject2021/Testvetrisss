using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Reflection;
using System.Drawing;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using VETRIS.DAL;

namespace VETRIS.Core.Master
{
    public class Institution
    {
        #region Constructor
        public Institution()
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
        string strMobile = string.Empty;
        string strEmail = string.Empty;
        string strContactPersonName = string.Empty;
        string strContactPersonMobile = string.Empty;
        string strNotes = string.Empty;
        Guid SalespersonId = new Guid("00000000-0000-0000-0000-000000000000");
        decimal decCommission1stYr = 0;// Added on 4th SEP 2019 @BK
        decimal decCommission2ndYr = 0;// Added on 4th SEP 2019 @BK
        decimal decDiscPer = 0;
        string strAccountantName = string.Empty;// Added on 3rd SEP 2019 @BK
        string strIsActive = "Y";
        string strLoginID = string.Empty;
        string strInfoSrc = string.Empty;
        int intBInfoSrc = 0;
        string strExitingBillAcctLink = "N";
        Guid BillAcctId = new Guid("00000000-0000-0000-0000-000000000000");
        Guid UnmatchInstID = new Guid("00000000-0000-0000-0000-000000000000");
        string strFmtDCMFiles = "N";
        string strDCMFileXferMode = "N";
        string strStudyImgManualRecPath = string.Empty;
        string strUSRUPDURL = string.Empty;
        string strConsultApplicable = "N";
        string strStorageApplicable = "N";
        string strCustomRpt = "N";
        string strCompressFiles = "Y";
        string strFaxRpt = "N";
        string strFaxNo = string.Empty;
        string strRptFmt = "P";
        Image imgLogo = null;
        byte[] btLogoImg = null;
        string strImageType = string.Empty;
        string strXMLDevice = string.Empty;
        string strXMLPhysician = string.Empty;
        string strXMLUser = string.Empty;
        string strXMLSalesPerson = string.Empty;
        string strXMLFee = string.Empty;
        string strXMLTag = string.Empty;
        string strXMLInstitutionCategory = string.Empty;
        string strXMLInst = string.Empty;
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
        public string MOBILE
        {
            get { return strMobile; }
            set { strMobile = value; }
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
        public Guid UNMATCHED_INSTITUTION_ID
        {
            get { return UnmatchInstID; }
            set { UnmatchInstID = value; }
        }
        public string NOTES
        {
            get { return strNotes; }
            set { strNotes = value; }
        }
        public string LOGIN_ID
        {
            get { return strLoginID; }
            set { strLoginID = value; }
        }
        public Guid SALES_PERSON_ID
        {
            get { return SalespersonId; }
            set { SalespersonId = value; }
        }
        public decimal COMMISSION_1ST_YEAR// Added on 4th SEP 2019 @BK
        {
            get { return decCommission1stYr; }
            set { decCommission1stYr = value; }
        }
        public decimal COMMISSION_2ND_YEAR// Added on 4th SEP 2019 @BK
        {
            get { return decCommission2ndYr; }
            set { decCommission2ndYr = value; }
        }
        public decimal DISCOUNT_PERCENT
        {
            get { return decDiscPer; }
            set { decDiscPer = value; }
        }

        public string ACCOUNTANT_NAME // Added on 3rd SEP 2019 @BK
        {
            get { return strAccountantName; }
            set { strAccountantName = value; }
        }
        public string SOURCE_OF_INFORMATION
        {
            get { return strInfoSrc; }
            set { strInfoSrc = value; }
        }
        public int BUSSINESS_SOURCE_ID
        {
            get { return intBInfoSrc; }
            set { intBInfoSrc = value; }

        }
        public string LINKED_TO_EXISTING_BILLING_ACCOUNT
        {
            get { return strExitingBillAcctLink; }
            set { strExitingBillAcctLink = value; }
        }
        public Guid BILLING_ACCOUNT_ID
        {
            get { return BillAcctId; }
            set { BillAcctId = value; }
        }
        public string FORMAT_DICOM_FILES
        {
            get { return strFmtDCMFiles; }
            set { strFmtDCMFiles = value; }
        }
        public string DICOM_FILES_TRANSFER_METHOD
        {
            get { return strDCMFileXferMode; }
            set { strDCMFileXferMode = value; }
        }
        public string COMPRESS_DICOM_FILES_TO_TRANSFER
        {
            get { return strCompressFiles; }
            set { strCompressFiles = value; }
        }
        public string STUDY_IMAGE_FILES_MANUAL_RECEIVING_PATH
        {
            get { return strStudyImgManualRecPath; }
            set { strStudyImgManualRecPath = value; }
        }
        public string IS_ACTIVE
        {
            get { return strIsActive; }
            set { strIsActive = value; }
        }
        public string USER_UPDATE_URL
        {
            get { return strUSRUPDURL; }
            set { strUSRUPDURL = value; }
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
        public string CUSTOMIZE_REPORTS
        {
            get { return strCustomRpt; }
            set { strCustomRpt = value; }
        }
        public string FAX_REPORTS
        {
            get { return strFaxRpt; }
            set { strFaxRpt = value; }
        }
        public string REPORT_FORMAT
        {
            get { return strRptFmt; }
            set { strRptFmt = value; }
        }
        public Image LOGO_IMAGE
        {
            get { return imgLogo; }
            set { imgLogo = value; }
        }
        public byte[] LOGO
        {
            get { return btLogoImg; }
            set { btLogoImg = value; }
        }
        public string IMAGE_TYPE
        {
            get { return strImageType; }
            set { strImageType = value; }
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
            SqlParameter[] SqlRecordParams = new SqlParameter[10];
            SqlRecordParams[0] = new SqlParameter("@code", SqlDbType.NVarChar, 5); SqlRecordParams[0].Value = strCode;
            SqlRecordParams[1] = new SqlParameter("@name", SqlDbType.NVarChar, 100); SqlRecordParams[1].Value = strName;
            SqlRecordParams[2] = new SqlParameter("@city", SqlDbType.NVarChar, 100); SqlRecordParams[2].Value = strCity;
            SqlRecordParams[3] = new SqlParameter("@zip", SqlDbType.NVarChar, 20); SqlRecordParams[3].Value = strZip;
            SqlRecordParams[4] = new SqlParameter("@country_id", SqlDbType.NVarChar, 20); SqlRecordParams[4].Value = intCountryID;
            SqlRecordParams[5] = new SqlParameter("@state_id", SqlDbType.NVarChar, 20); SqlRecordParams[5].Value = intStateID;
            SqlRecordParams[6] = new SqlParameter("@dcm_file_xfer_pacs_mode", SqlDbType.NChar, 1); SqlRecordParams[6].Value = strDCMFileXferMode;
            SqlRecordParams[7] = new SqlParameter("@fax_rpt", SqlDbType.NChar, 1); SqlRecordParams[7].Value = strFaxRpt;
            SqlRecordParams[8] = new SqlParameter("@is_active", SqlDbType.NChar, 1); SqlRecordParams[8].Value = strIsActive;
            SqlRecordParams[9] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[9].Value = UserID;


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "master_institution_fetch_brw", SqlRecordParams);
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

        #region FetchExportList
        public bool FetchExportList(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[10];
            SqlRecordParams[0] = new SqlParameter("@code", SqlDbType.NVarChar, 5); SqlRecordParams[0].Value = strCode;
            SqlRecordParams[1] = new SqlParameter("@name", SqlDbType.NVarChar, 100); SqlRecordParams[1].Value = strName;
            SqlRecordParams[2] = new SqlParameter("@city", SqlDbType.NVarChar, 100); SqlRecordParams[2].Value = strCity;
            SqlRecordParams[3] = new SqlParameter("@zip", SqlDbType.NVarChar, 20); SqlRecordParams[3].Value = strZip;
            SqlRecordParams[4] = new SqlParameter("@country_id", SqlDbType.NVarChar, 20); SqlRecordParams[4].Value = intCountryID;
            SqlRecordParams[5] = new SqlParameter("@state_id", SqlDbType.NVarChar, 20); SqlRecordParams[5].Value = intStateID;
            SqlRecordParams[6] = new SqlParameter("@dcm_file_xfer_pacs_mode", SqlDbType.NChar, 1); SqlRecordParams[6].Value = strDCMFileXferMode;
            SqlRecordParams[7] = new SqlParameter("@fax_rpt", SqlDbType.NChar, 1); SqlRecordParams[7].Value = strFaxRpt;
            SqlRecordParams[8] = new SqlParameter("@is_active", SqlDbType.NChar, 1); SqlRecordParams[8].Value = strIsActive;
            SqlRecordParams[9] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[9].Value = UserID;


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "master_institution_fetch_export", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "ExportList";
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
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "master_institution_fetch_dtls", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Details";
                    ds.Tables[1].TableName = "Country";
                    ds.Tables[2].TableName = "States";
                    ds.Tables[3].TableName = "Physicians";
                    ds.Tables[4].TableName = "SalesPersons";
                    ds.Tables[5].TableName = "URL";
                    ds.Tables[6].TableName = "BusinessSrc";
                    ds.Tables[7].TableName = "BillingAccount";

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
                        strMobile = Convert.ToString(dr["mobile_no"]).Trim();
                        strFaxNo = Convert.ToString(dr["fax_no"]).Trim();
                        strContactPersonName = Convert.ToString(dr["contact_person_name"]).Trim();
                        strContactPersonMobile = Convert.ToString(dr["contact_person_mobile"]).Trim();
                        strNotes = Convert.ToString(dr["notes"]).Trim();
                        SalespersonId = new Guid(Convert.ToString(dr["salesperson_id"]));
                        decCommission1stYr = Convert.ToDecimal(dr["commission_1st_yr"]);// Added on 4th SEP 2019 @BK
                        decCommission2ndYr = Convert.ToDecimal(dr["commission_2nd_yr"]);// Added on 4th SEP 2019 @BK
                        decDiscPer = Convert.ToDecimal(dr["discount_per"]);
                        strAccountantName = Convert.ToString(dr["accountant_name"]).Trim();// Added on 3rd SEP 2019 @BK
                        //strInfoSrc = Conve   rt.ToString(dr["info_source"]).Trim();
                        intBInfoSrc = Convert.ToInt32(dr["business_source_id"].ToString());
                        strExitingBillAcctLink = Convert.ToString(dr["link_existing_bill_acct"]).Trim();
                        BillAcctId = new Guid(Convert.ToString(dr["billing_account_id"]));
                        strFmtDCMFiles = Convert.ToString(dr["format_dcm_files"]).Trim();
                        strDCMFileXferMode = Convert.ToString(dr["dcm_file_xfer_pacs_mode"]).Trim();
                        strCompressFiles = Convert.ToString(dr["xfer_files_compress"]).Trim();
                        strStudyImgManualRecPath = Convert.ToString(dr["study_img_manual_receive_path"]).Trim();
                        strConsultApplicable = Convert.ToString(dr["consult_applicable"]).Trim();
                        strStorageApplicable = Convert.ToString(dr["storage_applicable"]).Trim();
                        strCustomRpt = Convert.ToString(dr["custom_report"]).Trim();
                        strFaxRpt = Convert.ToString(dr["fax_rpt"]).Trim();
                        strRptFmt = Convert.ToString(dr["rpt_format"]).Trim();
                        if (dr["logo_img"] != DBNull.Value) btLogoImg = (byte[])dr["logo_img"];
                        strImageType = Convert.ToString(dr["image_content_type"]).Trim();
                        strIsActive = Convert.ToString(dr["is_active"]).Trim();
                    }
                    foreach (DataRow dr in ds.Tables["URL"].Rows)
                    {
                        strUSRUPDURL = Convert.ToString(dr["USRUPDURL"]).Trim();
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

        #region AutoGenInstcode
        public bool AutoGenInstcodeStr(string ConfigPath, ref string strCode, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            SqlRecordParams[0] = new SqlParameter("@cdlen", SqlDbType.Int); SqlRecordParams[0].Value = 5;
            DataSet ds;
            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "auto_gen_instcode", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "AutoGenInstcodeString";
                    if (ds.Tables["AutoGenInstcodeString"].Rows.Count > 0)
                    {
                        strCode = ds.Tables["AutoGenInstcodeString"].Rows[0]["inst_code"].ToString();
                    }
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region LoadDevices
        public bool LoadDevices(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "master_institution_fetch_devices", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Devices";
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
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "master_institution_fetch_physicians", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Physicians";
                    //foreach (DataRow dr in ds.Tables["Physicians"].Rows)
                    //{
                    //    if (Convert.ToString(dr["physician_pacs_password"]).Trim() != string.Empty)
                    //        dr["physician_pacs_password"] = CoreCommon.DecryptString(Convert.ToString(dr["physician_pacs_password"]));
                    //}
                    //ds.Tables["Physicians"].AcceptChanges();
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region LoadUsers
        public bool LoadUsers(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "master_institution_fetch_users", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Users";
                    foreach (DataRow dr in ds.Tables["Users"].Rows)
                    {
                        if (Convert.ToString(dr["password"]).Trim() != string.Empty)
                            dr["password"] = CoreCommon.DecryptString(Convert.ToString(dr["password"]));
                        if (Convert.ToString(dr["pacs_password"]).Trim() != string.Empty)
                            dr["pacs_password"] = CoreCommon.DecryptString(Convert.ToString(dr["pacs_password"]));
                    }
                    ds.Tables["Users"].AcceptChanges();
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region LoadFees
        public bool LoadFees(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "master_institution_fetch_fees", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Fees";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region LoadPromotions
        public bool LoadPromotions(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "master_institution_fetch_promotions", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Promotions";
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

        #region FetchPhysicianDetails
        public bool FetchPhysicianDetails(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;

            SqlParameter[] SqlRecordParams = new SqlParameter[3];


            try
            {
                SqlRecordParams[0] = new SqlParameter("@email_id", SqlDbType.NVarChar, 50); SqlRecordParams[0].Value = strEmail;
                SqlRecordParams[1] = new SqlParameter("@mobile_no", SqlDbType.NVarChar, 20); SqlRecordParams[1].Value = strMobile;
                SqlRecordParams[2] = new SqlParameter("@name", SqlDbType.NVarChar, 200); SqlRecordParams[2].Value = strName;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "master_fetch_physician_dtls", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Physician";
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

        #region LoadDiomTags
        public bool LoadDiomTags(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "master_institution_fetch_tags", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Tags";
                    
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region LoadStudyTypeCategory
        public bool LoadStudyTypeCategory(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "master_institution_fetch_study_category", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "StudyTypeCategory";

                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region LoadMismatchInstitution
        public bool LoadMismatchInstitution(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "master_institution_fetch_mismatch_institution", SqlRecordParams);
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

        #region FetchUserDetails
        public bool FetchUserDetails(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;

            SqlParameter[] SqlRecordParams = new SqlParameter[1];


            try
            {
                SqlRecordParams[0] = new SqlParameter("@login_id", SqlDbType.NVarChar, 50); SqlRecordParams[0].Value = strLoginID;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "master_fetch_user_dtls", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "User";
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

        #region FetchBillingAccounts
        public bool FetchBillingAccounts(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;

            SqlParameter[] SqlRecordParams = new SqlParameter[1];


            try
            {
                SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "master_institution_fetch_billing_accounts", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "BillingAccounts";
                    ds.Tables[1].TableName = "AccountId";
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

        #region GetImage
        public void GetImage(object LogoImage)
        {
            byte[] imageData = (byte[])LogoImage;
            btLogoImg = imageData;

            using (MemoryStream ms = new MemoryStream(imageData, 0, imageData.Length))
            {
                ms.Write(imageData, 0, imageData.Length);
                //Set image variable value using memory stream.
                imgLogo = Image.FromStream(ms, true);
            }
        }
        #endregion

        #region ReadImageFile
        public byte[] ReadImageFile(string strPath)
        {
            //Initialize byte array with a null value initially.
            byte[] data = null;

            //Use FileInfo object to get file size.
            FileInfo fInfo = new FileInfo(strPath);
            long numBytes = fInfo.Length;

            //Open FileStream to read file
            FileStream fStream = new FileStream(strPath, FileMode.Open, FileAccess.Read);

            //Use BinaryReader to read file stream into byte array.
            BinaryReader br = new BinaryReader(fStream);

            //When you use BinaryReader, you need to supply number of bytes to read from file.
            //In this case we want to read entire file. So supplying total number of bytes.
            data = br.ReadBytes((int)numBytes);
            fStream.Close();
            return data;
        }
        #endregion

        #region SaveRecord
        public bool SaveRecord(string ConfigPath, DeviceList[] ArrDevObj, PhysicianList[] ArrPhysObj, UserList[] ArrUserObj, TagList[] ArrTagObj, StudyTypeCategoryList[] ArrInstCatObj, AlternateNameList[] ArrInstObj, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;

            if (ValidateRecord(ArrTagObj,ref ReturnMessage))
            {
                if ((GenerateDeviceXML(ArrDevObj, ref CatchMessage) && (GeneratePhysicianXML(ArrPhysObj, ref ReturnMessage, ref CatchMessage)) && (GenerateUserXML(ArrUserObj, ref ReturnMessage, ref CatchMessage)) && (GenerateTagXML(ArrTagObj, ref ReturnMessage, ref CatchMessage))
                    && (GenerateStudyTypeCategoryXML(ArrInstCatObj, ref ReturnMessage, ref CatchMessage)) && (GenerateAlternateNameXML(ArrInstObj, ref ReturnMessage, ref CatchMessage))))
                {
                    try
                    {
                        SqlParameter[] SqlRecordParams = new SqlParameter[47];
                        SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id; SqlRecordParams[0].Direction = ParameterDirection.InputOutput;
                        SqlRecordParams[1] = new SqlParameter("@code", SqlDbType.NVarChar, 5); SqlRecordParams[1].Value = strCode; SqlRecordParams[1].Direction = ParameterDirection.InputOutput;
                        SqlRecordParams[2] = new SqlParameter("@name", SqlDbType.NVarChar, 100); SqlRecordParams[2].Value = strName;
                        SqlRecordParams[3] = new SqlParameter("@email_id", SqlDbType.NVarChar, 50); SqlRecordParams[3].Value = strEmail;
                        SqlRecordParams[4] = new SqlParameter("@address_Line1", SqlDbType.NVarChar, 100); SqlRecordParams[4].Value = strAddress1;
                        SqlRecordParams[5] = new SqlParameter("@address_Line2", SqlDbType.NVarChar, 100); SqlRecordParams[5].Value = strAddress2;
                        SqlRecordParams[6] = new SqlParameter("@city", SqlDbType.NVarChar, 100); SqlRecordParams[6].Value = strCity;
                        SqlRecordParams[7] = new SqlParameter("@zip", SqlDbType.NVarChar, 20); SqlRecordParams[7].Value = strZip;
                        SqlRecordParams[8] = new SqlParameter("@state_id", SqlDbType.Int); SqlRecordParams[8].Value = intStateID;
                        SqlRecordParams[9] = new SqlParameter("@country_id", SqlDbType.Int); SqlRecordParams[9].Value = intCountryID;
                        SqlRecordParams[10] = new SqlParameter("@phone", SqlDbType.NVarChar, 30); SqlRecordParams[10].Value = strPhone;
                        SqlRecordParams[11] = new SqlParameter("@mobile", SqlDbType.NVarChar, 20); SqlRecordParams[11].Value = strMobile;
                        SqlRecordParams[12] = new SqlParameter("@contact_person_name", SqlDbType.NVarChar, 100); SqlRecordParams[12].Value = strContactPersonName;
                        SqlRecordParams[13] = new SqlParameter("@contact_person_mob", SqlDbType.NVarChar, 100); SqlRecordParams[13].Value = strContactPersonMobile;
                        SqlRecordParams[14] = new SqlParameter("@notes", SqlDbType.NVarChar, 250); SqlRecordParams[14].Value = strNotes;
                        SqlRecordParams[15] = new SqlParameter("@salesperson_id", SqlDbType.UniqueIdentifier); SqlRecordParams[15].Value = SalespersonId;
                        SqlRecordParams[16] = new SqlParameter("@commission_1st_yr", SqlDbType.Decimal); SqlRecordParams[16].Value = decCommission1stYr;
                        SqlRecordParams[17] = new SqlParameter("@commission_2nd_yr", SqlDbType.Decimal); SqlRecordParams[17].Value = decCommission2ndYr;
                        SqlRecordParams[18] = new SqlParameter("@discount_per", SqlDbType.Decimal); SqlRecordParams[18].Value = decDiscPer;
                        SqlRecordParams[19] = new SqlParameter("@business_source_id", SqlDbType.Int); SqlRecordParams[19].Value = intBInfoSrc;
                        SqlRecordParams[20] = new SqlParameter("@accountant_name", SqlDbType.NVarChar, 250); SqlRecordParams[20].Value = strAccountantName;//Added on 3rd SEP 2019 @BK
                        SqlRecordParams[21] = new SqlParameter("@link_existing_bill_acct", SqlDbType.NChar, 1); SqlRecordParams[21].Value = strExitingBillAcctLink;
                        SqlRecordParams[22] = new SqlParameter("@billing_account_id", SqlDbType.UniqueIdentifier); SqlRecordParams[22].Value = BillAcctId;
                        SqlRecordParams[23] = new SqlParameter("@format_dcm_files", SqlDbType.NChar, 1); SqlRecordParams[23].Value = strFmtDCMFiles;
                        SqlRecordParams[24] = new SqlParameter("@dcm_file_xfer_pacs_mode", SqlDbType.NChar, 1); SqlRecordParams[24].Value = strDCMFileXferMode;
                        SqlRecordParams[25] = new SqlParameter("@study_img_manual_receive_path", SqlDbType.NVarChar, 250); SqlRecordParams[25].Value = strStudyImgManualRecPath;
                        SqlRecordParams[26] = new SqlParameter("@consult_applicable", SqlDbType.NChar, 1); SqlRecordParams[26].Value = strConsultApplicable;
                        SqlRecordParams[27] = new SqlParameter("@storage_applicable", SqlDbType.NChar, 1); SqlRecordParams[27].Value = strStorageApplicable;
                        SqlRecordParams[28] = new SqlParameter("@custom_report", SqlDbType.NChar, 1); SqlRecordParams[28].Value = strCustomRpt;
                        SqlRecordParams[29] = new SqlParameter("@logo_img", SqlDbType.Image); if (btLogoImg != null) SqlRecordParams[29].Value = (object)btLogoImg; else SqlRecordParams[29].Value = DBNull.Value;
                        SqlRecordParams[30] = new SqlParameter("@image_content_type", SqlDbType.NVarChar, 20); SqlRecordParams[30].Value = strImageType;
                        SqlRecordParams[31] = new SqlParameter("@xfer_files_compress", SqlDbType.NChar, 1); SqlRecordParams[31].Value = strCompressFiles;
                        SqlRecordParams[32] = new SqlParameter("@fax_rpt", SqlDbType.NChar, 1); SqlRecordParams[32].Value = strFaxRpt;
                        SqlRecordParams[33] = new SqlParameter("@fax_no", SqlDbType.NVarChar, 30); SqlRecordParams[33].Value = strFaxNo;
                        SqlRecordParams[34] = new SqlParameter("@rpt_format", SqlDbType.NChar, 1); SqlRecordParams[34].Value = strRptFmt;
                        SqlRecordParams[35] = new SqlParameter("@is_active", SqlDbType.NChar, 1); SqlRecordParams[35].Value = strIsActive;
                        SqlRecordParams[36] = new SqlParameter("@xml_device", SqlDbType.NText); if (strXMLDevice.Trim() != string.Empty) SqlRecordParams[36].Value = strXMLDevice.Trim(); else SqlRecordParams[36].Value = DBNull.Value;
                        SqlRecordParams[37] = new SqlParameter("@xml_physician", SqlDbType.NText); if (strXMLPhysician.Trim() != string.Empty) SqlRecordParams[37].Value = strXMLPhysician.Trim(); else SqlRecordParams[37].Value = DBNull.Value;
                        SqlRecordParams[38] = new SqlParameter("@xml_user", SqlDbType.NText); if (strXMLUser.Trim() != string.Empty) SqlRecordParams[38].Value = strXMLUser.Trim(); else SqlRecordParams[38].Value = DBNull.Value;
                        SqlRecordParams[39] = new SqlParameter("@xml_tags", SqlDbType.NText); if (strXMLTag.Trim() != string.Empty) SqlRecordParams[39].Value = strXMLTag.Trim(); else SqlRecordParams[39].Value = DBNull.Value;
                        SqlRecordParams[40] = new SqlParameter("@xml_inst", SqlDbType.NText); if (strXMLInst.Trim() != string.Empty) SqlRecordParams[40].Value = strXMLInst.Trim(); else SqlRecordParams[40].Value = DBNull.Value;
                        SqlRecordParams[41] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[41].Value = UserID;
                        SqlRecordParams[42] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[42].Value = intMenuID;
                        SqlRecordParams[43] = new SqlParameter("@user_name", SqlDbType.NVarChar, 700); SqlRecordParams[43].Direction = ParameterDirection.Output;
                        SqlRecordParams[44] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[44].Direction = ParameterDirection.Output;
                        SqlRecordParams[45] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[45].Direction = ParameterDirection.Output;
                        SqlRecordParams[46] = new SqlParameter("@xml_ins_category", SqlDbType.NText); if (strXMLInstitutionCategory.Trim() != string.Empty) SqlRecordParams[46].Value = strXMLInstitutionCategory.Trim(); else SqlRecordParams[46].Value = DBNull.Value;

                        if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                        intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "master_institution_save", SqlRecordParams);
                        intReturnValue = Convert.ToInt32(SqlRecordParams[45].Value);
                        if (intReturnValue == 1)
                            bReturn = true;
                        else
                            bReturn = false;

                        strUserName = Convert.ToString(SqlRecordParams[43].Value).Trim();
                        ReturnMessage = Convert.ToString(SqlRecordParams[44].Value).Trim();

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

        #region Registration
        /// <summary>
        /// Added on 17th SEP 2019
        /// This Method added for new Registration
        /// </summary>
        /// <param name="ConfigPath"></param>
        /// <param name="ArrPhysObj"></param>
        /// <param name="ReturnMessage"></param>
        /// <param name="CatchMessage"></param>
        /// <returns></returns>
        /// 
        #region Register
        public bool Register(string ConfigPath, string ServerPath, PhysicianList[] ArrPhysObj, UserList ArrUserObj, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;
            if (ValidateRegistration(ArrUserObj, ref ReturnMessage))
            {
                if (GeneratePhysicianXML(ArrPhysObj, ref ReturnMessage, ref CatchMessage))
                {
                    try
                    {
                        SqlParameter[] SqlRecordParams = new SqlParameter[25];
                        SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id; SqlRecordParams[0].Direction = ParameterDirection.InputOutput;
                        SqlRecordParams[1] = new SqlParameter("@code", SqlDbType.NVarChar, 5); SqlRecordParams[1].Value = strCode; SqlRecordParams[1].Direction = ParameterDirection.InputOutput;
                        SqlRecordParams[2] = new SqlParameter("@name", SqlDbType.NVarChar, 100); SqlRecordParams[2].Value = strName;
                        SqlRecordParams[3] = new SqlParameter("@email_id", SqlDbType.NVarChar, 50); SqlRecordParams[3].Value = strEmail;
                        SqlRecordParams[4] = new SqlParameter("@address_Line1", SqlDbType.NVarChar, 100); SqlRecordParams[4].Value = strAddress1;
                        SqlRecordParams[5] = new SqlParameter("@address_Line2", SqlDbType.NVarChar, 100); SqlRecordParams[5].Value = strAddress2;
                        SqlRecordParams[6] = new SqlParameter("@city", SqlDbType.NVarChar, 100); SqlRecordParams[6].Value = strCity;
                        SqlRecordParams[7] = new SqlParameter("@zip", SqlDbType.NVarChar, 20); SqlRecordParams[7].Value = strZip;
                        SqlRecordParams[8] = new SqlParameter("@state_id", SqlDbType.Int); SqlRecordParams[8].Value = intStateID;
                        SqlRecordParams[9] = new SqlParameter("@country_id", SqlDbType.Int); SqlRecordParams[9].Value = intCountryID;
                        SqlRecordParams[10] = new SqlParameter("@phone", SqlDbType.NVarChar, 30); SqlRecordParams[10].Value = strPhone;
                        SqlRecordParams[11] = new SqlParameter("@mobile", SqlDbType.NVarChar, 20); SqlRecordParams[11].Value = strMobile;
                        SqlRecordParams[12] = new SqlParameter("@contact_person_name", SqlDbType.NVarChar, 100); SqlRecordParams[12].Value = strContactPersonName;
                        SqlRecordParams[13] = new SqlParameter("@contact_person_mob", SqlDbType.NVarChar, 100); SqlRecordParams[13].Value = strContactPersonMobile;
                        SqlRecordParams[14] = new SqlParameter("@discount_per", SqlDbType.Decimal); SqlRecordParams[14].Value = decDiscPer;
                        SqlRecordParams[15] = new SqlParameter("@is_active", SqlDbType.NVarChar, 30); SqlRecordParams[15].Value = strIsActive;
                        SqlRecordParams[16] = new SqlParameter("@xml_physician", SqlDbType.NText); if (strXMLPhysician.Trim() != string.Empty) SqlRecordParams[16].Value = strXMLPhysician.Trim(); else SqlRecordParams[16].Value = DBNull.Value;
                        SqlRecordParams[17] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[17].Value = UserID;
                        SqlRecordParams[18] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[18].Value = intMenuID;
                        SqlRecordParams[19] = new SqlParameter("@user_login_id", SqlDbType.NVarChar, 50); SqlRecordParams[19].Value = ArrUserObj.LOGIN_ID;
                        SqlRecordParams[20] = new SqlParameter("@user_pwd", SqlDbType.NVarChar, 200); SqlRecordParams[20].Value = ArrUserObj.PASSWORD;
                        SqlRecordParams[21] = new SqlParameter("@user_email_id", SqlDbType.NVarChar, 50); SqlRecordParams[21].Value = ArrUserObj.EMAIL_ID;
                        SqlRecordParams[22] = new SqlParameter("@user_name", SqlDbType.NVarChar, 700); SqlRecordParams[22].Direction = ParameterDirection.Output;
                        SqlRecordParams[23] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[23].Direction = ParameterDirection.Output;
                        SqlRecordParams[24] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[24].Direction = ParameterDirection.Output;


                        if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                        intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "registration_institution_save", SqlRecordParams);
                        intReturnValue = Convert.ToInt32(SqlRecordParams[24].Value);


                        strUserName = Convert.ToString(SqlRecordParams[22].Value).Trim();
                        ReturnMessage = Convert.ToString(SqlRecordParams[23].Value).Trim();

                        Id = new Guid(Convert.ToString(SqlRecordParams[0].Value));
                        strCode = Convert.ToString(SqlRecordParams[1].Value).Trim();
                        if (intReturnValue == 1)
                        {
                            SendActivationEmail(ConfigPath, ServerPath, Id);
                            bReturn = true;

                        }
                        else
                            bReturn = false;

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

        #region ValidateRegistration
        private bool ValidateRegistration(UserList ArrUserObj, ref string ReturnMessage)
        {
            bool bReturn = true;

            /*if ((strCode.Trim() == string.Empty) && (strIsActive == "Y"))
            {
                ReturnMessage = "075";
            }*/
            if (strName.Trim() == string.Empty)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "076";
            }
            if (intCountryID == 0)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "186";
            }
            if (intStateID == 0)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "187";
            }

            if (strEmail.Trim() == string.Empty)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "082";
            }
            //else
            //{
            //    if (!CoreCommon.IsEmailValid(strEmail))
            //    {
            //        if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
            //        ReturnMessage += "084";
            //    }
            //}
            if (strContactPersonMobile.ToString() == string.Empty)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "090";
            }
            if (ArrUserObj.LOGIN_ID.Trim() == string.Empty)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "007";
            }
            if (ArrUserObj.PASSWORD.Trim() == string.Empty)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "008";
            }
            if (ArrUserObj.EMAIL_ID.Trim() == string.Empty)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "188";
            }


            if (ReturnMessage.Trim() != string.Empty)
                bReturn = false;

            return bReturn;
        }
        #endregion

        #endregion

        #region ValidateRecord
        private bool ValidateRecord(TagList[] ArrTagObj,ref string ReturnMessage)
        {
            bool bReturn = true;

            /*if ((strCode.Trim() == string.Empty) && (strIsActive == "Y"))
            {
                ReturnMessage = "075";
            }*/
            if (strName.Trim() == string.Empty)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "076";
            }
            if (strFaxRpt == "Y" && strFaxNo.Trim() == string.Empty)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "417";
            }

            if (strNotes.Trim() != string.Empty)
            {
                if (strNotes.Trim().Length > 250)
                {
                    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                    ReturnMessage += "098";
                }
            }

            if (strExitingBillAcctLink == "Y") 
            {
                if (BillAcctId == Guid.Parse("{00000000-0000-0000-0000-000000000000}"))
                {
                    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                    ReturnMessage += "225";
                }
            }

            if (strFmtDCMFiles.Trim() == "Y")
            {
                if (ArrTagObj.Length == 0)
                {
                    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                    ReturnMessage += "248";
                }
            }

            //if (ArrFeeObj.Length == 0)
            //{
            //    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
            //    ReturnMessage += "137";
            //}

            if (ReturnMessage.Trim() != string.Empty)
                bReturn = false;

            return bReturn;
        }
        #endregion

        #region GenerateDeviceXML
        private bool GenerateDeviceXML(DeviceList[] ArrObj, ref string CatchMessage)
        {
            bool bReturn = false;
            StringBuilder sbXML = new StringBuilder();

            try
            {
                if (ArrObj.Length > 0)
                {

                    sbXML.Append("<device>");

                    for (int i = 0; i < ArrObj.Length; i = i + 1)
                    {
                        sbXML.Append("<row>");
                        sbXML.Append("<device_id>" + ArrObj[i].ID.ToString() + "</device_id>");
                        sbXML.Append("<manufacturer><![CDATA[" + ArrObj[i].MANUFACTURER + "]]></manufacturer>");
                        sbXML.Append("<modality><![CDATA[" + ArrObj[i].MODALITY + "]]></modality>");
                        sbXML.Append("<modality_ae_title><![CDATA[" + ArrObj[i].MODALITY_AE_TITLE + "]]></modality_ae_title>");
                        sbXML.Append("<weight_uom><![CDATA[" + ArrObj[i].WEIGHT_UOM + "]]></weight_uom>");
                        sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                        sbXML.Append("</row>");
                    }

                    sbXML.Append("</device>");
                    strXMLDevice = sbXML.ToString();


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


                        //if (ArrObj[i].EMAIL_ID.Trim() != string.Empty)
                        //{
                        //    if (!CoreCommon.IsEmailValid(ArrObj[i].EMAIL_ID.Trim()))
                        //    {
                        //        if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                        //        ReturnMessage += "089";
                        //        //ReturnMessage += ArrObj[i].EMAIL_ID.Trim();
                        //        strUserName = (i + 1).ToString();
                        //    }
                        //}
                        if (ReturnMessage.Trim() != string.Empty)
                        {
                            break;
                        }
                        #endregion

                        sbXML.Append("<row>");
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

        #region GenerateUserXML
        private bool GenerateUserXML(UserList[] ArrObj, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            StringBuilder sbXML = new StringBuilder();

            try
            {
                if (ArrObj.Length > 0)
                {

                    sbXML.Append("<user>");

                    for (int i = 0; i < ArrObj.Length; i = i + 1)
                    {
                        #region Validations
                        if ((ArrObj[i].LOGIN_ID).Trim() == string.Empty)
                        {
                            if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                            ReturnMessage = "115";
                            strUserName = (i + 1).ToString();
                        }


                        if (ArrObj[i].PASSWORD.Trim() == string.Empty)
                        {
                            if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                            ReturnMessage += "116";
                            strUserName = (i + 1).ToString();
                        }


                        if (ArrObj[i].EMAIL_ID.Trim() != string.Empty)
                        {
                            if (!CoreCommon.IsEmailValid(ArrObj[i].EMAIL_ID.Trim()))
                            {
                                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                                ReturnMessage += "117";
                                strUserName = (i + 1).ToString();
                            }
                        }

                        if (ArrObj[i].PACS_USER_ID.Trim() == string.Empty)
                        {
                            if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                            ReturnMessage += "100";
                            strUserName = (i + 1).ToString();
                        }
                        if (ArrObj[i].PACS_PASSWORD.Trim() == string.Empty)
                        {
                            if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                            ReturnMessage += "101";
                            strUserName = (i + 1).ToString();
                        }


                        if (ReturnMessage.Trim() != string.Empty)
                        {
                            break;
                        }
                        #endregion

                        sbXML.Append("<row>");
                        sbXML.Append("<user_user_id><![CDATA[" + ArrObj[i].ID.ToString() + "]]></user_user_id>");
                        sbXML.Append("<user_login_id><![CDATA[" + ArrObj[i].LOGIN_ID + "]]></user_login_id>");
                        sbXML.Append("<user_pwd><![CDATA[" + ArrObj[i].PASSWORD + "]]></user_pwd>");
                        sbXML.Append("<user_pacs_user_id><![CDATA[" + ArrObj[i].LOGIN_ID + "]]></user_pacs_user_id>");
                        sbXML.Append("<user_pacs_password><![CDATA[" + ArrObj[i].PASSWORD + "]]></user_pacs_password>");
                        sbXML.Append("<user_email_id><![CDATA[" + ArrObj[i].EMAIL_ID + "]]></user_email_id>");
                        sbXML.Append("<user_contact_no><![CDATA[" + ArrObj[i].CONTACT_NUMBER + "]]></user_contact_no>");
                        sbXML.Append("<is_active><![CDATA[" + ArrObj[i].IS_ACTIVE + "]]></is_active>");
                        sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                        sbXML.Append("</row>");
                    }

                    if (ReturnMessage.Trim() != string.Empty)
                    {
                        bReturn = false;
                        sbXML.Clear();
                        strXMLUser = string.Empty;
                    }
                    else
                    {
                        bReturn = true;
                        sbXML.Append("</user>");
                        strXMLUser = sbXML.ToString();
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

        #region GenerateFeeXML
        private bool GenerateFeeXML(FeeList[] ArrObj, ref string ReturnMessage, ref string CatchMessage)
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
                        sbXML.Append("<fee_row_id>" + ArrObj[i].ROW_ID.ToString() + "</fee_row_id>");
                        sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                        sbXML.Append("</row>");
                    }

                    sbXML.Append("</fees>");

                }

                strXMLFee = sbXML.ToString();
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

        #region GenerateTagXML
        private bool GenerateTagXML(TagList[] ArrObj, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            StringBuilder sbXML = new StringBuilder();

            try
            {
                if (ArrObj.Length > 0)
                {

                    sbXML.Append("<tag>");

                    for (int i = 0; i < ArrObj.Length; i = i + 1)
                    {
                        

                        sbXML.Append("<row>");
                        sbXML.Append("<group_id><![CDATA[" + ArrObj[i].GROUP_ID.ToString() + "]]></group_id>");
                        sbXML.Append("<element_id><![CDATA[" + ArrObj[i].ELEMENT_ID + "]]></element_id>");
                        sbXML.Append("<default_value><![CDATA[" + ArrObj[i].DEFAULT_VALUE + "]]></default_value>");
                        sbXML.Append("<junk_characters><![CDATA[" + ArrObj[i].JUNK_CHARACTER + "]]></junk_characters>");
                        sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                        sbXML.Append("</row>");
                    }

                    sbXML.Append("</tag>");

                    strXMLTag = sbXML.ToString();

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

        #region GenerateStudyTypeCategoryXML
        private bool GenerateStudyTypeCategoryXML(StudyTypeCategoryList[] ArrObj, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            StringBuilder sbXML = new StringBuilder();

            try
            {
                if (ArrObj.Length > 0)
                {
                    sbXML.Append("<inst_category>");
                    for (int i = 0; i < ArrObj.Length; i = i + 1)
                    {
                        sbXML.Append("<row>");
                        sbXML.Append("<category_id>" + ArrObj[i].CATEGORY_ID.ToString() + "</category_id>");
                        sbXML.Append("<institution_id><![CDATA[" + ArrObj[i].INSTITUTION_ID + "]]></institution_id>");
                        sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                        sbXML.Append("</row>");
                    }
                    sbXML.Append("</inst_category>");
                    strXMLInstitutionCategory = sbXML.ToString();
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

        #region GenerateAlternateNameXML
        private bool GenerateAlternateNameXML(AlternateNameList[] ArrObj, ref string ReturnMessage, ref string CatchMessage)
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
                        sbXML.Append("<inst_id><![CDATA[" + ArrObj[i].INSTITUTION_ID.ToString() + "]]></inst_id>");
                        sbXML.Append("<inst_name><![CDATA[" + ArrObj[i].INSTITUTION_NAME + "]]></inst_name>");
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

        #endregion

        #region Emailverification
        private void SendActivationEmail(string ConfigPath, string ServerPath, Guid userId)
        {

            DataSet ds = new DataSet();
            string[] lsArrMailServer = { "", "", "", "", "", "" }; bool lbRetSettings = false;
            string verificationCode = Guid.NewGuid().ToString();

            string verificationString = "userID=" + CoreCommon.EncryptString(Convert.ToString(userId)) + "&code=" + CoreCommon.EncryptString(Convert.ToString(verificationCode));
            //string json 

            int intExecReturn = 0;

            string lsMailSentFrom = string.Empty;
            string lsMailText = string.Empty;
            string lsMailSign = string.Empty;
            string lsMailSubject = string.Empty;
            string[] lsUserDtls = new string[10];

            //--------------------------------
            //SqlParameter[] SqlRecordParams = new SqlParameter[2];
            //SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier);              SqlRecordParams[0].Value = userId;
            //SqlRecordParams[1] = new SqlParameter("@verification_code", SqlDbType.NVarChar, 5);    SqlRecordParams[1].Value = verificationCode;


            //if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
            //intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "registration_user_email_verification_code_save", SqlRecordParams);

            try
            {

                SqlParameter[] SqlRecordParams2 = new SqlParameter[0];
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "general_settings_fetch");

                ds.Tables[0].TableName = "Settings";
                lbRetSettings = true;

                foreach (DataRow drs in ds.Tables["Settings"].Rows)
                {

                    if (drs["control_code"].ToString().Trim().Equals("MAILSVRNAME"))
                    {
                        lsArrMailServer[0] = drs["data_type_string"].ToString().Trim();

                    }
                    if (drs["control_code"].ToString().Trim().Equals("MAILSVRPORT"))
                    {
                        lsArrMailServer[1] = drs["data_type_number"].ToString().Trim();

                    }
                    if (drs["control_code"].ToString().Trim().Equals("MAILSVRUSRCODE"))
                    {
                        lsArrMailServer[2] = drs["data_type_string"].ToString().Trim();
                        lsMailSentFrom = drs["data_type_string"].ToString().Trim();
                    }
                    if (drs["control_code"].ToString().Trim().Equals("MAILSVRUSRPWD"))
                    {
                        lsArrMailServer[3] = drs["data_type_string"].ToString().Trim();

                    }
                    if (drs["control_code"].ToString().Trim().Equals("MAILSSLENABLED"))
                    {
                        lsArrMailServer[4] = drs["data_type_string"].ToString().Trim();

                    }
                    if (drs["control_code"].ToString().Trim().Equals("APPURL"))
                    {
                        lsArrMailServer[5] = drs["data_type_string"].ToString().Trim();

                    }
                }
            }
            catch (Exception ex)
            {
                //asCatchMessage = ex.Message;
                lbRetSettings = false;
            }


            //lsMailText = "Hello " + lsUserDtls[0]+ ",";
            lsMailText = "Hello " + strContactPersonName + ",";
            lsMailText += "<br /><br />Please click the following link to activate your account";
            //lsMailText += "<br /><a href ='" + " http://localhost/VETRIS/Registration/VRSUserEmailVerification.aspx?Activation=" + verificationString + "'> Click here</a> to activate your account.";
            lsMailText += "<br /><a href ='" + ServerPath + "/Registration/VRSUserEmailVerification.aspx?code=" + userId + "'> Click here</a> to activate your account.";
            lsMailText += "<br /><br />Thanks";
            lsMailSign = "VETCHOICH";
            lsMailSubject = " Email Verification Link";

            if (lbRetSettings && intExecReturn == 1)
            {

                SendActivationCodeToUser(lsMailSentFrom, lsMailText, lsMailSign, lsMailSubject, lsArrMailServer);
            }

        }

        private void SendActivationCodeToUser(string LsSenderEmailId, string LsMailText, string LsMailSign, string LsMailSubject, string[] LsArrMailServer)
        {

            //string lsName = asOccupantDtls[0].ToString();
            //string lsEmailID = asOccupantDtls[1].ToString();
            StringBuilder lsbMailMessage = new StringBuilder();
            MailSender lobjMail = new MailSender();
            try
            {
                if (LsArrMailServer[0].Trim() != "")
                {
                    lobjMail.SMTPServer = LsArrMailServer[0].Trim();
                    lobjMail.MailServerPortNo = Convert.ToInt32(LsArrMailServer[1].Trim());
                    lobjMail.MailFrom = LsSenderEmailId;
                    //lobjMail.MailTo = strEmail;
                    lobjMail.MailTo = "bbaidya@rad365tech.com";
                    //lobjMail.MailCC = "pguha@rad365tech.com";
                    lobjMail.MailSubject = LsMailSubject;
                    lsbMailMessage.Append(GenerateUserAccountMailBody(LsMailText, LsMailSign));
                    lobjMail.MailBody = lsbMailMessage.ToString();
                    //lobjMail.Attachments = 1;

                    //lobjMail.AttachedFile = new string[1];
                    //lobjMail.AttachedFileName = new string[1];
                    //lobjMail.AttachedFile[0] = asFilePath + asOccupantDtls[2].ToString();
                    //lobjMail.AttachedFileName[0] = asOccupantDtls[2].ToString();

                    lobjMail.EmbedContent = true;
                    lobjMail.MailFrom = LsArrMailServer[2].Trim();
                    lobjMail.MailServerPassword = LsArrMailServer[3].Trim();
                    //bjMail.PropertyFilePath = LsMailTemplatePath;

                    if (LsArrMailServer[4].Trim() == "Y")
                        lobjMail.MailServerSSLEnabled = true;
                    else
                        lobjMail.MailServerSSLEnabled = false;

                    lobjMail.SendMail(ref CatchMessage);
                }
            }
            catch (Exception ex)
            {
                ;
            }
            finally
            {
                lobjMail = null;
            }
        }

        private string GenerateUserAccountMailBody(string LsMailText, string LsMailSign)
        {
            StringBuilder LsbMailBody = new StringBuilder();
            //StreamReader LswRe = File.OpenText(LsMailTemplatePath + "AYWUserAccount.htm");
            //string LsServerPath = System.Configuration.ConfigurationManager.AppSettings["ServerPath"];

            try
            {
                //LsMailText = LsMailText.Replace("\n", "<br/>");//[NWL]
                //LsMailText = LsMailText.Replace("[NWL]", "<br/>");//[NWL]
                LsbMailBody.AppendLine(LsMailText);

                //  LsbMailBody.Replace("###MailText###", LsMailText);
                //LsbMailBody.Replace("[OCCUPANT_NAME]", asUserDtls[0].ToString());
                //LsbMailBody.Replace("[BILL_MONTH]", asUserDtls[3].ToString());
                LsbMailBody.Append("<br/>");
                LsbMailBody.Append(LsMailSign);
            }
            catch (Exception LexpErr)
            { ;}


            return LsbMailBody.ToString();
        }

        public bool VerifyEmail(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;

            try
            {
                SqlParameter[] SqlRecordParams = new SqlParameter[3];
                SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
                SqlRecordParams[1] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[1].Direction = ParameterDirection.Output;
                SqlRecordParams[2] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[2].Direction = ParameterDirection.Output;


                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "registration_email_verified", SqlRecordParams);
                intReturnValue = Convert.ToInt32(SqlRecordParams[2].Value);
                ReturnMessage = Convert.ToString(SqlRecordParams[1].Value).Trim();

                Id = new Guid(Convert.ToString(SqlRecordParams[0].Value));
                if (intReturnValue == 1)
                {

                    bReturn = true;

                }
                else
                    bReturn = false;

            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }

        #endregion

        //public string generateRandomString()
        //{
        //    string randomstring = "";
        //    string characters = "$@#^~_+-!0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        //    int length = 10;
        //    int charLength = characters.Length;
        //    var random = new Random();
        //    string[] stringChars = new string[10];
        //    for (int i = 0; i < length;  i++) 
        //    {
        //        stringChars[i] = characters[random.Next(0, charLength - 1)].ToString();
        //    }
        //    return randomstring;// = new String(stringChars);

        //}

        
    }

    public class PhysicianList
    {
        #region Constructor
        public PhysicianList()
        {
        }
        #endregion

        #region Variables
        Guid InstId = Guid.Empty;
        Guid Id = Guid.Empty;
        string strFName = string.Empty;
        string strLName = string.Empty;
        string strCred = string.Empty;
        string strLoginEmail = string.Empty;
        string strEmail = string.Empty;
        string strMobile = string.Empty;
        string strPACSUserID = string.Empty;
        string strPACSPwd = string.Empty;
        #endregion

        #region Properties
        public Guid INSTITUTION_ID
        {
            get { return InstId; }
            set { InstId = value; }
        }
        public Guid ID
        {
            get { return Id; }
            set { Id = value; }
        }
        public string FIRST_NAME
        {
            get { return strFName; }
            set { strFName = value; }
        }
        public string LAST_NAME
        {
            get { return strLName; }
            set { strLName = value; }
        }
        public string CREDENTIALS
        {
            get { return strCred; }
            set { strCred = value; }
        }
        public string EMAIL_ID
        {
            get { return strEmail; }
            set { strEmail = value; }
        }
        public string MOBILE_NUMBER
        {
            get { return strMobile; }
            set { strMobile = value; }
        }
        #endregion
    }
    public class DeviceList
    {
        #region Constructor
        public DeviceList()
        {
        }
        #endregion

        #region Variables
        Guid DeviceID = Guid.Empty;
        string strManufacturer = string.Empty;
        string strModality = string.Empty;
        string strAETitle = string.Empty;
        string strWeightUOM = string.Empty;//Added on 2nd SEP 2019 @BK
        #endregion

        #region Properties
        public Guid ID
        {
            get { return DeviceID; }
            set { DeviceID = value; }
        }
        public string MANUFACTURER
        {
            get { return strManufacturer; }
            set { strManufacturer = value; }
        }
        public string MODALITY
        {
            get { return strModality; }
            set { strModality = value; }
        }
        public string MODALITY_AE_TITLE
        {
            get { return strAETitle; }
            set { strAETitle = value; }
        }
        /// <summary>
        ///  WEIGHT_UOM; Added on 2nd SEP 2019 @BK
        /// </summary>
        public string WEIGHT_UOM
        {
            get { return strWeightUOM; }
            set { strWeightUOM = value; }
        }
        #endregion
    }
    public class UserList
    {
        #region Constructor
        public UserList()
        {
        }
        #endregion

        #region Variables
        Guid Id = Guid.Empty;
        string strLoginID = string.Empty;
        string strPwd = string.Empty;
        string strEmail = string.Empty;
        string strContactNo = string.Empty;
        string strPACSUserID = string.Empty;
        string strPACSPwd = string.Empty;
        string strIsActive = string.Empty;
        #endregion

        #region Properties
        public Guid ID
        {
            get { return Id; }
            set { Id = value; }
        }
        public string LOGIN_ID
        {
            get { return strLoginID; }
            set { strLoginID = value; }
        }
        public string PASSWORD
        {
            get { return strPwd; }
            set { strPwd = value; }
        }
        public string EMAIL_ID
        {
            get { return strEmail; }
            set { strEmail = value; }
        }
        public string CONTACT_NUMBER
        {
            get { return strContactNo; }
            set { strContactNo = value; }
        }
        public string PACS_USER_ID
        {
            get { return strPACSUserID; }
            set { strPACSUserID = value; }
        }
        public string PACS_PASSWORD
        {
            get { return strPACSPwd; }
            set { strPACSPwd = value; }
        }
        public string IS_ACTIVE
        {
            get { return strIsActive; }
            set { strIsActive = value; }
        }
        #endregion
    }
    public class FeeList
    {
        #region Constructor
        public FeeList()
        {
        }
        #endregion

        #region Variables
        int intRowID = 0;
        Guid RateId = Guid.Empty;
        double dblFee = 0;
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
        public int ROW_ID
        {
            get { return intRowID; }
            set { intRowID = value; }
        }
        #endregion
    }
    public class TagList
    {
        #region Constructor
        public TagList()
        {
        }
        #endregion

        #region Variables
        string strGroupID = string.Empty;
        string strElementID = string.Empty;
        string strDefVal= string.Empty;
        string strJunkChar = string.Empty;
        #endregion

        #region Properties
        public string GROUP_ID
        {
            get { return strGroupID; }
            set { strGroupID = value; }
        }
        public string ELEMENT_ID
        {
            get { return strElementID; }
            set { strElementID = value; }
        }
        public string DEFAULT_VALUE
        {
            get { return strDefVal; }
            set { strDefVal = value; }
        }
        public string JUNK_CHARACTER
        {
            get { return strJunkChar; }
            set { strJunkChar = value; }
        }
        #endregion
    }

    public class StudyTypeCategoryList
    {
        #region Constructor
        public StudyTypeCategoryList()
        {
        }
        #endregion

        #region Variables
        Guid institutionId = Guid.Empty;
        int categoryId = 0;
        #endregion

        #region Properties
        public Guid INSTITUTION_ID
        {
            get { return institutionId; }
            set { institutionId = value; }
        }
        public int CATEGORY_ID
        {
            get { return categoryId; }
            set { categoryId = value; }
        }
        #endregion
    }
    public class AlternateNameList
    {
        #region Constructor
        public AlternateNameList()
        {
        }
        #endregion

        #region Variables
        Guid InstID = Guid.Empty;
        string strInstName = string.Empty;
        #endregion

        #region Properties
        public Guid INSTITUTION_ID
        {
            get { return InstID; }
            set { InstID = value; }
        }
        public string INSTITUTION_NAME
        {
            get { return strInstName; }
            set { strInstName = value; }
        }
        #endregion
    }
}
