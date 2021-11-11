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
    public class Radiologist
    {
        #region Constructor
        public Radiologist()
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
        string strCredentials       = string.Empty;// Added on 12th SEP 2019 @BK
        string strAddress1          = string.Empty;
        string strAddress2          = string.Empty;
        string strCity              = string.Empty;
        int intStateID              = 0;
        int intCountryID            = 0;
        string strZip               = string.Empty;
        string strPhone             = string.Empty;
        string strMobile            = string.Empty;
        string strEmail             = string.Empty;
        Guid LoginUserId            = new Guid("00000000-0000-0000-0000-000000000000");
        string strLoginID           = string.Empty;
        string strPwd               = string.Empty;
        string strPACSUserID        = string.Empty;
        string strPACSPwd           = string.Empty;
        string strIsActive          = "Y";
        string strIdentityColor     = string.Empty;
        string strNotificationPref  = "B";
        string strSignage           = string.Empty;
        string strScheduleView      = "O";
        string strNotes             = string.Empty;
        string strReqTrans          = "N";
        int intAcctGroupID          = 0;
        string strAssignStudyDef    = "N";
        int intTimeZoneID           = 0;
        int intMaxWU                = 0;
        string strXML               = string.Empty;
        string strInstXML           = string.Empty;
        string strWUXML           = string.Empty;
        string strRightXML          = string.Empty;
        string strModalityXML       = string.Empty;
        string strSpeciesXML = string.Empty;
        string strStXML             = string.Empty;
        string strRadXML            = string.Empty;
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
        public string CREDENTIALS // Added on 12th SEP 2019 @BK
        {
            get { return strCredentials; }
            set { strCredentials = value; }
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
        public int TIME_ZONE_ID
        {
            get { return intTimeZoneID; }
            set { intTimeZoneID = value; }
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
        public Guid LOGIN_USER_ID
        {
            get { return LoginUserId; }
            set { LoginUserId = value; }
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
        public string PACS_PASSWORD
        {
            get { return strPACSPwd; }
            set { strPACSPwd = value; }
        }
        public int ACCOUNT_GROUP_ID
        {
            get { return intAcctGroupID; }
            set { intAcctGroupID = value; }
        }
        public string IS_ACTIVE
        {
            get { return strIsActive; }
            set { strIsActive = value; }
        }
        public string IDENTIFICATION_COLOR
        {
            get { return strIdentityColor; }
            set { strIdentityColor = value; }
        }
        public string NOTIFICATION_PREFERENCE
        {
            get { return strNotificationPref; }
            set { strNotificationPref = value; }
        }
        public string SIGNAGE
        {
            get { return strSignage; }
            set { strSignage = value; }
        }
        public string VIEW_SCHEDULE
        {
            get { return strScheduleView; }
            set { strScheduleView = value; }
        }
        public int MAXIMUM_WORK_UNITS
        {
            get { return intMaxWU; }
            set { intMaxWU = value; }
        }
        public string NOTES
        {
            get { return strNotes; }
            set { strNotes = value; }
        }
        public string REQUIRE_TRANSCRIPTION
        {
            get { return strReqTrans; }
            set { strReqTrans = value; }
        }
        public string ASSIGN_STUDY_BY_DEFAULT
        {
            get { return strAssignStudyDef; }
            set { strAssignStudyDef = value; }
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
                    ds.Tables[5].TableName = "TimeZone";
                    ds.Tables[6].TableName = "Group";
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
            SqlParameter[] SqlRecordParams = new SqlParameter[9];
            SqlRecordParams[0] = new SqlParameter("@code", SqlDbType.NVarChar, 5); SqlRecordParams[0].Value = strCode;
            SqlRecordParams[1] = new SqlParameter("@name", SqlDbType.NVarChar, 100); SqlRecordParams[1].Value = strName;
            SqlRecordParams[2] = new SqlParameter("@acct_group_id", SqlDbType.Int); SqlRecordParams[2].Value = intAcctGroupID;
            SqlRecordParams[3] = new SqlParameter("@mobile_no", SqlDbType.NVarChar, 20); SqlRecordParams[3].Value = strMobile;
            SqlRecordParams[4] = new SqlParameter("@country_id", SqlDbType.NVarChar, 20); SqlRecordParams[4].Value = intCountryID;
            SqlRecordParams[5] = new SqlParameter("@state_id", SqlDbType.NVarChar, 20); SqlRecordParams[5].Value = intStateID;
            SqlRecordParams[6] = new SqlParameter("@is_active", SqlDbType.NChar, 1); SqlRecordParams[6].Value = strIsActive;
            SqlRecordParams[7] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[7].Value = UserID;
            SqlRecordParams[8] = new SqlParameter("@timezone_id", SqlDbType.NVarChar, 20); SqlRecordParams[8].Value = intTimeZoneID;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "master_radiologists_fetch_brw", SqlRecordParams);
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
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "master_radiologists_fetch_dtls", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Details";
                    ds.Tables[1].TableName = "Country";
                    ds.Tables[2].TableName = "States";
                    ds.Tables[3].TableName = "Users";
                    ds.Tables[4].TableName = "Group";
                    ds.Tables[5].TableName = "TimeZone";

                    #region Details

                    foreach (DataRow dr in ds.Tables["Details"].Rows)
                    {
                        strCode             = Convert.ToString(dr["code"]).Trim();
                        strFname            = Convert.ToString(dr["fname"]).Trim();
                        strLname            = Convert.ToString(dr["lname"]).Trim();
                        strName             = Convert.ToString(dr["name"]).Trim();
                        strCredentials      = Convert.ToString(dr["credentials"]).Trim();// Added on 12th SEP 2019 @BK
                        strAddress1         = Convert.ToString(dr["address_1"]).Trim();
                        strAddress2         = Convert.ToString(dr["address_2"]).Trim();
                        strCity             = Convert.ToString(dr["city"]).Trim();
                        intStateID          = Convert.ToInt32(dr["state_id"]);
                        intCountryID        = Convert.ToInt32(dr["country_id"]);
                        strZip              = Convert.ToString(dr["zip"]).Trim();
                        strEmail            = Convert.ToString(dr["email_id"]).Trim();
                        strPhone            = Convert.ToString(dr["phone_no"]).Trim();
                        strMobile           = Convert.ToString(dr["mobile_no"]).Trim();

                        LoginUserId         = new Guid(Convert.ToString(dr["login_user_id"]));
                        strLoginID          = Convert.ToString(dr["login_id"]).Trim();
                        if (Convert.ToString(dr["login_pwd"]).Trim() != string.Empty)
                            strPwd          = CoreCommon.DecryptString(Convert.ToString(dr["login_pwd"]).Trim());
                        else 
                            strPwd          = string.Empty;

                        strPACSUserID       = Convert.ToString(dr["pacs_user_id"]).Trim();
                        if (Convert.ToString(dr["pacs_user_pwd"]).Trim() != string.Empty)
                            strPACSPwd      = CoreCommon.DecryptString(Convert.ToString(dr["pacs_user_pwd"]).Trim());
                        else
                            strPACSPwd      = string.Empty;
                       
                        strIdentityColor    = Convert.ToString(dr["identity_color"]).Trim();
                        strNotificationPref = Convert.ToString(dr["notification_pref"]).Trim();
                        strSignage          = Convert.ToString(dr["signage"]).Trim();
                        strScheduleView     = Convert.ToString(dr["schedule_view"]).Trim();
                        strNotes            = Convert.ToString(dr["notes"]).Trim();
                        strReqTrans         = Convert.ToString(dr["transcription_required"]).Trim();
                        intAcctGroupID      = Convert.ToInt32(dr["acct_group_id"]);
                        strAssignStudyDef   = Convert.ToString(dr["assign_merged_study"]).Trim();
                        intTimeZoneID       = Convert.ToInt32(dr["timezone_id"]);
                        intMaxWU            = Convert.ToInt32(dr["max_wu_per_hr"]);
                        strIsActive         = Convert.ToString(dr["is_active"]).Trim();
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

        #region LoadModality
        public bool LoadModality(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "master_radiologist_modality_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Modality";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region SaveRecord
        public bool SaveRecord(string ConfigPath, ModalityList[] ArrObj, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;

            if (ValidateRecord(ArrObj,ref ReturnMessage))
            {
                if (GenerateXML(ArrObj, ref CatchMessage))
                {
                    try
                    {
                        SqlParameter[] SqlRecordParams = new SqlParameter[34];
                        SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value           = Id; SqlRecordParams[0].Direction = ParameterDirection.InputOutput;
                        SqlRecordParams[1] = new SqlParameter("@fname", SqlDbType.NVarChar, 80); SqlRecordParams[1].Value            = strFname;
                        SqlRecordParams[2] = new SqlParameter("@lname", SqlDbType.NVarChar, 80); SqlRecordParams[2].Value            = strLname;
                        SqlRecordParams[3] = new SqlParameter("@address_Line1", SqlDbType.NVarChar, 100); SqlRecordParams[3].Value   = strAddress1;
                        SqlRecordParams[4] = new SqlParameter("@address_Line2", SqlDbType.NVarChar, 100); SqlRecordParams[4].Value   = strAddress2;
                        SqlRecordParams[5] = new SqlParameter("@city", SqlDbType.NVarChar, 100); SqlRecordParams[5].Value            = strCity;
                        SqlRecordParams[6] = new SqlParameter("@zip", SqlDbType.NVarChar, 20); SqlRecordParams[6].Value              = strZip;
                        SqlRecordParams[7] = new SqlParameter("@state_id", SqlDbType.Int); SqlRecordParams[7].Value                  = intStateID;
                        SqlRecordParams[8] = new SqlParameter("@country_id", SqlDbType.Int); SqlRecordParams[8].Value                = intCountryID;
                        SqlRecordParams[9] = new SqlParameter("@timezone_id", SqlDbType.Int); SqlRecordParams[9].Value               = intTimeZoneID;
                        
                        SqlRecordParams[10] = new SqlParameter("@email_id", SqlDbType.NVarChar, 50); SqlRecordParams[10].Value       = strEmail;
                        SqlRecordParams[11] = new SqlParameter("@phone", SqlDbType.NVarChar, 30); SqlRecordParams[11].Value          = strPhone;
                        SqlRecordParams[12] = new SqlParameter("@mobile", SqlDbType.NVarChar, 20); SqlRecordParams[12].Value         = strMobile;
                        SqlRecordParams[13] = new SqlParameter("@login_id", SqlDbType.NVarChar, 50); SqlRecordParams[13].Value       = strLoginID;
                        SqlRecordParams[14] = new SqlParameter("@login_pwd", SqlDbType.NVarChar, 200); SqlRecordParams[14].Value     = strPwd;
                        SqlRecordParams[15] = new SqlParameter("@pacs_user_id", SqlDbType.NVarChar, 50); SqlRecordParams[15].Value   = strPACSUserID;
                        SqlRecordParams[16] = new SqlParameter("@pacs_user_pwd", SqlDbType.NVarChar, 200); SqlRecordParams[16].Value = strPACSPwd;

                        SqlRecordParams[17] = new SqlParameter("@is_active", SqlDbType.NChar, 1); SqlRecordParams[17].Value          = strIsActive;
                        SqlRecordParams[18] = new SqlParameter("@identity_color", SqlDbType.NVarChar,10); SqlRecordParams[18].Value  = strIdentityColor;
                        SqlRecordParams[19] = new SqlParameter("@notification_pref", SqlDbType.NChar, 1); SqlRecordParams[19].Value  = strNotificationPref;
                        SqlRecordParams[20] = new SqlParameter("@signage", SqlDbType.NText); SqlRecordParams[20].Value               = strSignage;
                        SqlRecordParams[21] = new SqlParameter("@schedule_view", SqlDbType.NChar,1); SqlRecordParams[21].Value       = strScheduleView;
                        SqlRecordParams[22] = new SqlParameter("@notes", SqlDbType.NText); SqlRecordParams[22].Value                 = strNotes;
                        SqlRecordParams[23] = new SqlParameter("@transcription_required", SqlDbType.NChar, 1); SqlRecordParams[23].Value = strReqTrans;
                        SqlRecordParams[24] = new SqlParameter("@acct_group_id", SqlDbType.Int); SqlRecordParams[24].Value               = intAcctGroupID;
                        SqlRecordParams[25] = new SqlParameter("@assign_merged_study", SqlDbType.NChar,1); SqlRecordParams[25].Value     = strAssignStudyDef;
                        SqlRecordParams[26] = new SqlParameter("@max_wu_per_hr", SqlDbType.Int); SqlRecordParams[26].Value             = intMaxWU;
                        
                        SqlRecordParams[27] = new SqlParameter("@xml_modality", SqlDbType.NText); SqlRecordParams[27].Value              = strXML;
                        
                        SqlRecordParams[28] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[28].Value    = UserID;
                        SqlRecordParams[29] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[29].Value                 = intMenuID;
                        SqlRecordParams[30] = new SqlParameter("@user_name", SqlDbType.NVarChar, 700); SqlRecordParams[30].Direction = ParameterDirection.Output;
                        SqlRecordParams[31] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[31].Direction = ParameterDirection.Output;
                        SqlRecordParams[32] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[32].Direction       = ParameterDirection.Output;
                        SqlRecordParams[33] = new SqlParameter("@credentials", SqlDbType.NVarChar, 100); SqlRecordParams[33].Value   = strCredentials;// Added on 12th SEP 2019 @BK


                        if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                        intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "master_radiologists_save", SqlRecordParams);
                        intReturnValue = Convert.ToInt32(SqlRecordParams[32].Value);
                        if (intReturnValue == 1)
                            bReturn = true;
                        else
                            bReturn = false;

                        strUserName = Convert.ToString(SqlRecordParams[30].Value).Trim();
                        ReturnMessage = Convert.ToString(SqlRecordParams[31].Value);

                    }
                    catch (Exception expErr)
                    { bReturn = false; CatchMessage = expErr.Message; }
                
                }
                else
                    bReturn=false;

            }
            else
            {
                bReturn = false;
            }

            return bReturn;
        }
        #endregion

        #region ValidateRecord
        private bool ValidateRecord(ModalityList[] ArrObj,ref string ReturnMessage)
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
            if (intTimeZoneID == 0)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage = "443";
            }
            if (intAcctGroupID==0)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage = "411";
            }
            if ((strNotificationPref == "E") || (strNotificationPref == "B"))
            {
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
            }

            if ((strNotificationPref == "S") || (strNotificationPref == "B"))
            {
                if (strMobile.Trim() == string.Empty)
                {
                    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                    ReturnMessage = "090";
                }
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
            if (strPACSUserID.Trim() == string.Empty)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage = "080";
            }
            if (strPACSPwd.Trim() == string.Empty)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage = "081";
            }
            //if (strNotes.Trim() != string.Empty)
            //{
            //    if (strNotes.Trim().Length > 500)
            //    {
            //        if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
            //        ReturnMessage = "322";
            //    }
            //}
            if (ArrObj.Length == 0)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage = "205";
            }

            if (ReturnMessage.Trim() != string.Empty)
                bReturn = false;

            return bReturn;
        }

        #endregion

        #region GenerateXML
        private bool GenerateXML(ModalityList[] ArrObj, ref string CatchMessage)
        {
            bool bReturn = false;
            StringBuilder sbXML = new StringBuilder();

            try
            {
                if (ArrObj.Length > 0)
                {

                    sbXML.Append("<modality>");

                    for (int i = 0; i < ArrObj.Length; i = i + 1)
                    {
                        sbXML.Append("<row>");
                        sbXML.Append("<modality_id>" + ArrObj[i].ID.ToString() + "</modality_id>");
                        sbXML.Append("<prelim_fee>" + ArrObj[i].PRELIMINARY_STUDY_FEE.ToString() + "</prelim_fee>");
                        sbXML.Append("<final_fee>" + ArrObj[i].FINAL_STUDY_FEE.ToString() + "</final_fee>");
                        sbXML.Append("<addl_STAT_fee>" + ArrObj[i].ADDITION_FEE_FOR_STAT_PRELIM.ToString() + "</addl_STAT_fee>");
                        sbXML.Append("<work_unit>" + ArrObj[i].WORK_UNIT.ToString() + "</work_unit>");
                        sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                        sbXML.Append("</row>");
                    }

                    sbXML.Append("</modality>");
                    strXML = sbXML.ToString();


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

        #region Functional Rights

        #region LoadFunctionalRights
        public bool LoadFunctionalRights(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "master_radiologist_functional_rights_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "FnRights";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region LoadModalityRights
        public bool LoadModalityRights(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "master_radiologist_modality_rights_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Modality";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region LoadSpeciesRights
        public bool LoadSpeciesRights(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "master_radiologist_species_rights_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Species";
                }
                bReturn = true;
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
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "master_radiologist_institution_fetch", SqlRecordParams);
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

        #region LoadExceptionInstitutions
        public bool LoadExceptionInstitutions(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "master_radiologist_exception_institution_fetch", SqlRecordParams);
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

        #region LoadStudyTypes
        public bool LoadStudyTypes(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "master_radiologist_study_type_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "StudyTypes";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region LoadExceptionStudyTypes
        public bool LoadExceptionStudyTypes(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "master_radiologist_exception_study_type_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "StudyTypes";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region LoadRadiologists
        public bool LoadRadiologists(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "master_radiologist_radiologist_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Radiologists";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region LoadOtherRadiologists
        public bool LoadOtherRadiologists(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "master_radiologist_other_radiologist_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Radiologists";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region SaveRights
        public bool SaveRights(string ConfigPath, FubnctionalRightsList[] ArrFNObj, ModalityList[] ArrModObj, Species[] ArrSpecObj, ExceptionInstitutionList[] ArrInstObj, ExceptionStudyList[] ArrSTObj, OtherRadiologistList[] ArrRadObj, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;


            if (GenerateFunctionalRightXML(ArrFNObj, ref CatchMessage) && GenerateModalityXML(ArrModObj, ref CatchMessage)
                && GenerateSpeciesXML(ArrSpecObj, ref CatchMessage)
                && GenerateExceptionInstitutionXML(ArrInstObj, ref CatchMessage) && GenerateExceptionStudyTypeXML(ArrSTObj, ref CatchMessage)
                && GenerateOtherRadiologistXML(ArrRadObj, ref CatchMessage))
                {
                    try
                    {
                        SqlParameter[] SqlRecordParams = new SqlParameter[12];
                        SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
                        SqlRecordParams[1] = new SqlParameter("@xml_fn_rights", SqlDbType.NText); if (strRightXML.Trim() == "") SqlRecordParams[1].Value = DBNull.Value; else SqlRecordParams[1].Value = strRightXML;
                        SqlRecordParams[2] = new SqlParameter("@xml_modality", SqlDbType.NText); if (strModalityXML.Trim() == "") SqlRecordParams[2].Value = DBNull.Value; else SqlRecordParams[2].Value = strModalityXML;
                        SqlRecordParams[3] = new SqlParameter("@xml_species", SqlDbType.NText); if (strSpeciesXML.Trim() == "") SqlRecordParams[3].Value = DBNull.Value; else SqlRecordParams[3].Value = strSpeciesXML;
                        SqlRecordParams[4] = new SqlParameter("@xml_institution", SqlDbType.NText); if (strInstXML.Trim() == "") SqlRecordParams[4].Value = DBNull.Value; else SqlRecordParams[4].Value = strInstXML;
                        SqlRecordParams[5] = new SqlParameter("@xml_study_types", SqlDbType.NText); if (strStXML.Trim() == "") SqlRecordParams[5].Value = DBNull.Value; else SqlRecordParams[5].Value = strStXML;
                        SqlRecordParams[6] = new SqlParameter("@xml_radiologist", SqlDbType.NText); if (strRadXML.Trim() == "") SqlRecordParams[6].Value = DBNull.Value; else SqlRecordParams[6].Value = strRadXML;
                        SqlRecordParams[7] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[7].Value = UserID;
                        SqlRecordParams[8] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[8].Value = intMenuID;
                        SqlRecordParams[9] = new SqlParameter("@user_name", SqlDbType.NVarChar, 700); SqlRecordParams[9].Direction = ParameterDirection.Output;
                        SqlRecordParams[10] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[10].Direction = ParameterDirection.Output;
                        SqlRecordParams[11] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[11].Direction = ParameterDirection.Output;

                        if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                        intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "master_radiologists_rights_save", SqlRecordParams);
                        intReturnValue = Convert.ToInt32(SqlRecordParams[11].Value);
                        if (intReturnValue == 1)
                            bReturn = true;
                        else
                            bReturn = false;

                        strUserName = Convert.ToString(SqlRecordParams[9].Value).Trim();
                        ReturnMessage = Convert.ToString(SqlRecordParams[10].Value);

                    }
                    catch (Exception expErr)
                    { bReturn = false; CatchMessage = expErr.Message; }

                }
                else
                    bReturn = false;

            

            return bReturn;
        }
        #endregion

        #region GenerateFunctionalRightXML
        private bool GenerateFunctionalRightXML(FubnctionalRightsList[] ArrObj, ref string CatchMessage)
        {
            bool bReturn = false;
            StringBuilder sbXML = new StringBuilder();

            try
            {
                if (ArrObj.Length > 0)
                {

                    sbXML.Append("<rights>");

                    for (int i = 0; i < ArrObj.Length; i = i + 1)
                    {
                        sbXML.Append("<row>");
                        sbXML.Append("<right_code><![CDATA[" + ArrObj[i].CODE + "]]></right_code>");
                        sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                        sbXML.Append("</row>");
                    }

                    sbXML.Append("</rights>");
                    strRightXML = sbXML.ToString();


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

        #region GenerateModalityXML
        private bool GenerateModalityXML(ModalityList[] ArrObj, ref string CatchMessage)
        {
            bool bReturn = false;
            StringBuilder sbXML = new StringBuilder();

            try
            {
                if (ArrObj.Length > 0)
                {

                    sbXML.Append("<modality>");

                    for (int i = 0; i < ArrObj.Length; i = i + 1)
                    {
                        sbXML.Append("<row>");
                        sbXML.Append("<modality_id>" + ArrObj[i].ID.ToString() + "</modality_id>");
                        sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                        sbXML.Append("</row>");
                    }

                    sbXML.Append("</modality>");
                    strModalityXML = sbXML.ToString();


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

        #region GenerateSpeciesXML
        private bool GenerateSpeciesXML(Species[] ArrObj, ref string CatchMessage)
        {
            bool bReturn = false;
            StringBuilder sbXML = new StringBuilder();

            try
            {
                if (ArrObj.Length > 0)
                {

                    sbXML.Append("<species>");

                    for (int i = 0; i < ArrObj.Length; i = i + 1)
                    {
                        sbXML.Append("<row>");
                        sbXML.Append("<species_id>" + ArrObj[i].ID.ToString() + "</species_id>");
                        sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                        sbXML.Append("</row>");
                    }

                    sbXML.Append("</species>");
                    strSpeciesXML = sbXML.ToString();


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

        #region GenerateExceptionInstitutionXML
        private bool GenerateExceptionInstitutionXML(ExceptionInstitutionList[] ArrObj, ref string CatchMessage)
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
                        sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                        sbXML.Append("</row>");
                    }

                    sbXML.Append("</institution>");
                    strInstXML = sbXML.ToString();


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

        #region GenerateExceptionStudyTypeXML
        private bool GenerateExceptionStudyTypeXML(ExceptionStudyList[] ArrObj, ref string CatchMessage)
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
                        sbXML.Append("<study_type_id><![CDATA[" + ArrObj[i].ID.ToString() + "]]></study_type_id>");
                        sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                        sbXML.Append("</row>");
                    }

                    sbXML.Append("</study>");
                    strStXML = sbXML.ToString();


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

        #region GenerateOtherRadiologistXML
        private bool GenerateOtherRadiologistXML(OtherRadiologistList[] ArrObj, ref string CatchMessage)
        {
            bool bReturn = false;
            StringBuilder sbXML = new StringBuilder();

            try
            {
                if (ArrObj.Length > 0)
                {

                    sbXML.Append("<radiologist>");

                    for (int i = 0; i < ArrObj.Length; i = i + 1)
                    {
                        sbXML.Append("<row>");
                        sbXML.Append("<radiologist_id><![CDATA[" + ArrObj[i].ID.ToString() + "]]></radiologist_id>");
                        sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                        sbXML.Append("</row>");
                    }

                    sbXML.Append("</radiologist>");
                    strRadXML = sbXML.ToString();


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


    public class ModalityList
    {
        #region Constructor
        public ModalityList()
        {
        }
        #endregion

        #region Variables
        int inID = 0;
        int intSrlNo = 0;
        double dblPrelimFee = 0;
        double dblFinalFee = 0;
        double dblAddlFee = 0;
        int intWorkUnit = 0;
        #endregion

        #region Properties
        public int ID
        {
            get { return inID; }
            set { inID = value; }
        }
        public int SERIAL_NUMBER
        {
            get { return intSrlNo; }
            set { intSrlNo = value; }
        }
        public double PRELIMINARY_STUDY_FEE
        {
            get { return dblPrelimFee; }
            set { dblPrelimFee = value; }
        }
        public double FINAL_STUDY_FEE
        {
            get { return dblFinalFee; }
            set { dblFinalFee = value; }
        }
        public double ADDITION_FEE_FOR_STAT_PRELIM
        {
            get { return dblAddlFee; }
            set { dblAddlFee = value; }
        }
        public int WORK_UNIT
        {
            get { return intWorkUnit; }
            set { intWorkUnit = value; }
        }
        #endregion
    }
    public class FubnctionalRightsList
    {
        #region Constructor
        public FubnctionalRightsList()
        {
        }
        #endregion

        #region Variables
        string strCode = string.Empty;
        #endregion

        #region Properties
        public string CODE
        {
            get { return strCode; }
            set { strCode = value; }
        }

        #endregion
    }
    public class ExceptionInstitutionList
    {
        #region Constructor
        public ExceptionInstitutionList()
        {
        }
        #endregion

        #region Variables
        Guid Id = new Guid();
        #endregion

        #region Properties
        public Guid ID
        {
            get { return Id; }
            set { Id = value; }
        }

        #endregion
    }
    public class ExceptionStudyList
    {
        #region Constructor
        public ExceptionStudyList()
        {
        }
        #endregion

        #region Variables
        Guid Id = new Guid();
        #endregion

        #region Properties
        public Guid ID
        {
            get { return Id; }
            set { Id = value; }
        }

        #endregion
    }
    public class OtherRadiologistList
    {
        #region Constructor
        public OtherRadiologistList()
        {
        }
        #endregion

        #region Variables
        Guid Id = new Guid();
        #endregion

        #region Properties
        public Guid ID
        {
            get { return Id; }
            set { Id = value; }
        }

        #endregion
    }
    
}
