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

namespace VETRIS.Core.Master
{
    public class SalesPerson
    {
        #region Constructor
        public SalesPerson()
        {

        }
        #endregion

        #region Variables
        int intMenuID               = 0;
        Guid UserID                 = new Guid("00000000-0000-0000-0000-000000000000");
        int intUserRoleID           = 0;
        string strUserName          = string.Empty;

        Guid Id                     = new Guid("00000000-0000-0000-0000-000000000000");
        string strCode              = string.Empty;
        string strName              = string.Empty;
        string strFname             = string.Empty;
        string strLname             = string.Empty;
        string strAddress1          = string.Empty;
        string strAddress2          = string.Empty;
        string strCity              = string.Empty;
        int intStateID              = 0;
        int intCountryID            = 0;
        string strZip               = string.Empty;
        string strPhone             = string.Empty;
        string strMobile            = string.Empty;
        string strEmail             = string.Empty;
        string strLoginID           = string.Empty;
        string strPwd               = string.Empty;
        string strPACSUserID        = string.Empty;
        string strPACSUserPwd       = string.Empty;
        string strIsActive          = "Y";
        string strNotificationPref  = "B";
        decimal commission_1st_yr   = 0;
        decimal commission_2nd_yr   = 0;

        string strXMLInstitution         = string.Empty;
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
        public string FIRST_NAME
        {
            get { return strFname; }
            set { strFname = value; }
        }
        public string LAST_NAME
        {
            get { return strLname; }
            set { strLname = value; }
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
        public string EMAIL_ID
        {
            get { return strEmail; }
            set { strEmail = value; }
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
        public string PACS_USER_ID
        {
            get { return strPACSUserID; }
            set { strPACSUserID = value; }
        }
        public string PACS_USER_PASSWORD
        {
            get { return strPACSUserPwd; }
            set { strPACSUserPwd = value; }
        }
        public string IS_ACTIVE
        {
            get { return strIsActive; }
            set { strIsActive = value; }
        }
        public string NOTIFICATION_PREFERENCE
        {
            get { return strNotificationPref; }
            set { strNotificationPref = value; }
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
            SqlRecordParams[3] = new SqlParameter("@mobile_no", SqlDbType.NVarChar, 20); SqlRecordParams[3].Value = strMobile;
            SqlRecordParams[4] = new SqlParameter("@country_id", SqlDbType.NVarChar, 20); SqlRecordParams[4].Value = intCountryID;
            SqlRecordParams[5] = new SqlParameter("@state_id", SqlDbType.NVarChar, 20); SqlRecordParams[5].Value = intStateID;
            SqlRecordParams[6] = new SqlParameter("@is_active", SqlDbType.NChar, 1); SqlRecordParams[6].Value = strIsActive;
            SqlRecordParams[7] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[7].Value = UserID;


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "master_salesperson_fetch_brw", SqlRecordParams);
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
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "master_salesperson_fetch_dtls", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Details";
                    ds.Tables[1].TableName = "Country";
                    ds.Tables[2].TableName = "States";
                    ds.Tables[3].TableName = "Institutions";
                    ds.Tables[4].TableName = "Users";

                    #region Details

                    foreach (DataRow dr in ds.Tables["Details"].Rows)
                    {
                        strCode = Convert.ToString(dr["code"]).Trim();
                        strFname = Convert.ToString(dr["fname"]).Trim();
                        strLname = Convert.ToString(dr["lname"]).Trim();
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
                        strLoginID = Convert.ToString(dr["login_id"]).Trim();
                        if (Convert.ToString(dr["login_pwd"]).Trim() != string.Empty) strPwd = CoreCommon.DecryptString(Convert.ToString(dr["login_pwd"]).Trim());
                        else strPwd = string.Empty;
                        strPACSUserID = Convert.ToString(dr["pacs_user_id"]).Trim();
                        if (Convert.ToString(dr["pacs_password"]).Trim() != string.Empty) strPACSUserPwd = CoreCommon.DecryptString(Convert.ToString(dr["pacs_password"]).Trim());
                        else strPACSUserPwd = string.Empty;
                        strNotificationPref = Convert.ToString(dr["notification_pref"]).Trim();
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
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "master_salesperson_fetch_institutions", SqlRecordParams);
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

        #region SaveRecord
        public bool SaveRecord(string ConfigPath,InstitutionList[] ArrInstObj,   ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;

            if (ValidateRecord(ref ReturnMessage))
            {
                if (GenerateInstitutionXML(ArrInstObj, ref CatchMessage))
                {
                    try
                    {
                        SqlParameter[] SqlRecordParams = new SqlParameter[24];
                        SqlRecordParams[0]  = new SqlParameter("@id", SqlDbType.UniqueIdentifier);          SqlRecordParams[0].Value = Id; SqlRecordParams[0].Direction = ParameterDirection.InputOutput;
                        SqlRecordParams[1]  = new SqlParameter("@fname", SqlDbType.NVarChar, 80);           SqlRecordParams[1].Value = strFname;
                        SqlRecordParams[2]  = new SqlParameter("@lname", SqlDbType.NVarChar, 80);           SqlRecordParams[2].Value = strLname;
                        SqlRecordParams[3]  = new SqlParameter("@address_Line1", SqlDbType.NVarChar, 100);  SqlRecordParams[3].Value = strAddress1;
                        SqlRecordParams[4]  = new SqlParameter("@address_Line2", SqlDbType.NVarChar, 100);  SqlRecordParams[4].Value = strAddress2;
                        SqlRecordParams[5]  = new SqlParameter("@city", SqlDbType.NVarChar, 100);           SqlRecordParams[5].Value = strCity;
                        SqlRecordParams[6]  = new SqlParameter("@zip", SqlDbType.NVarChar, 20);             SqlRecordParams[6].Value = strZip;
                        SqlRecordParams[7]  = new SqlParameter("@state_id", SqlDbType.Int);                 SqlRecordParams[7].Value = intStateID;
                        SqlRecordParams[8]  = new SqlParameter("@country_id", SqlDbType.Int);               SqlRecordParams[8].Value = intCountryID;
                        SqlRecordParams[9]  = new SqlParameter("@email_id", SqlDbType.NVarChar, 50);        SqlRecordParams[9].Value = strEmail;
                        SqlRecordParams[10] = new SqlParameter("@phone", SqlDbType.NVarChar, 30);           SqlRecordParams[10].Value = strPhone;
                        SqlRecordParams[11] = new SqlParameter("@mobile", SqlDbType.NVarChar, 20);          SqlRecordParams[11].Value = strMobile;
                        SqlRecordParams[12] = new SqlParameter("@login_id", SqlDbType.NVarChar, 10);        SqlRecordParams[12].Value = strLoginID;
                        SqlRecordParams[13] = new SqlParameter("@login_password", SqlDbType.NVarChar, 200); SqlRecordParams[13].Value = strPwd;
                        SqlRecordParams[14] = new SqlParameter("@pacs_user_id", SqlDbType.NVarChar, 20);    SqlRecordParams[14].Value = strPACSUserID;
                        SqlRecordParams[15] = new SqlParameter("@pacs_password", SqlDbType.NVarChar, 200);  SqlRecordParams[15].Value = strPACSUserPwd;
                        SqlRecordParams[16] = new SqlParameter("@is_active", SqlDbType.NChar, 1);           SqlRecordParams[16].Value = strIsActive;
                        SqlRecordParams[17] = new SqlParameter("@notification_pref", SqlDbType.NChar, 1);   SqlRecordParams[17].Value = strNotificationPref;
                        SqlRecordParams[18] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[18].Value = UserID;
                        SqlRecordParams[19] = new SqlParameter("@menu_id", SqlDbType.Int);                  SqlRecordParams[19].Value = intMenuID;
                        SqlRecordParams[20] = new SqlParameter("@xml_institution", SqlDbType.NText); if (strXMLInstitution.Trim() != string.Empty) SqlRecordParams[20].Value = strXMLInstitution.Trim(); else SqlRecordParams[20].Value = DBNull.Value;
                        SqlRecordParams[21] = new SqlParameter("@user_name", SqlDbType.NVarChar, 700);      SqlRecordParams[21].Direction = ParameterDirection.Output;
                        SqlRecordParams[22] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10);      SqlRecordParams[22].Direction = ParameterDirection.Output;
                        SqlRecordParams[23] = new SqlParameter("@return_status", SqlDbType.Int);            SqlRecordParams[23].Direction = ParameterDirection.Output;


                        if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                        intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "master_salesperson_save", SqlRecordParams);
                        intReturnValue = Convert.ToInt32(SqlRecordParams[23].Value);
                        
                        if (intReturnValue == 1)
                            bReturn = true;
                        else
                            bReturn = false;

                        Id              = new Guid(Convert.ToString(SqlRecordParams[0].Value));
                        strUserName     = Convert.ToString(SqlRecordParams[21].Value).Trim();
                        ReturnMessage   = Convert.ToString(SqlRecordParams[22].Value);

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
        private bool ValidateRecord(ref string ReturnMessage)
        {
            bool bReturn = true;

            //if (strCode.Trim() == string.Empty)
            //{
            //    ReturnMessage = "075";
            //}
            if ((strFname.Trim() + " " + strLname.Trim()).Trim() == string.Empty)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage = "076";
            }
            if (strEmail.Trim() == string.Empty)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage = "082";
            }
            else
            {
                if (!CoreCommon.IsEmailValid(strEmail))
                {
                    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                    ReturnMessage += "084";
                }
            }
            if (strMobile.Trim() == string.Empty)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage = "090";
            }

            if (strLoginID.Trim() == string.Empty)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage = "112";
            }
            if (strPwd.Trim() == string.Empty)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage = "120";
            }

            if (ReturnMessage.Trim() != string.Empty)
                bReturn = false;

            return bReturn;
        }

        #endregion

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
                        sbXML.Append("<institution_id>" + ArrObj[i].ID.ToString() + "</institution_id>");
                        sbXML.Append("<commission1Year><![CDATA[" + ArrObj[i].COMMISSION_1ST_YEAR + "]]></commission1Year>");
                        sbXML.Append("<commission2Year><![CDATA[" + ArrObj[i].COMMISSION_2ND_YEAR + "]]></commission2Year>");
                        sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                        sbXML.Append("</row>");
                    }

                    sbXML.Append("</institution>");
                    strXMLInstitution = sbXML.ToString();


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

    //public class InstitutionList
    //{
    //    #region Constructor
    //    public InstitutionList()
    //    {
    //    }
    //    #endregion

    //    #region Variables
    //    Guid DeviceID = Guid.Empty;
    //    string strManufacturer = string.Empty;
    //    string strModality = string.Empty;
    //    string strAETitle = string.Empty;
    //    string strWeightUOM = string.Empty;//Added on 2nd SEP 2019 @BK
    //    #endregion

    //    #region Properties
    //    public Guid ID
    //    {
    //        get { return DeviceID; }
    //        set { DeviceID = value; }
    //    }
    //    public string MANUFACTURER
    //    {
    //        get { return strManufacturer; }
    //        set { strManufacturer = value; }
    //    }
    //    public string MODALITY
    //    {
    //        get { return strModality; }
    //        set { strModality = value; }
    //    }
    //    public string MODALITY_AE_TITLE
    //    {
    //        get { return strAETitle; }
    //        set { strAETitle = value; }
    //    }
    //    /// <summary>
    //    ///  WEIGHT_UOM; Added on 2nd SEP 2019 @BK
    //    /// </summary>
    //    public string WEIGHT_UOM
    //    {
    //        get { return strWeightUOM; }
    //        set { strWeightUOM = value; }
    //    }
    //    #endregion
    //} 
  
}
