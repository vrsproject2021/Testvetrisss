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
    public class Physician
    {
        #region Constructor
        public Physician()
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
        int intCountryID = 0;
        string strZip = string.Empty;
        string strPhone = string.Empty;
        string strMobile = string.Empty;
        string strEmail = string.Empty;
        string strIsActive = "Y";
        string strXMLInstitution = string.Empty;
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
        public string EMAIL_ID
        {
            get { return strEmail; }
            set { strEmail = value; }
        }
        public string IS_ACTIVE
        {
            get { return strIsActive; }
            set { strIsActive = value; }
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
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "master_physician_fetch_brw", SqlRecordParams);
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
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "master_physician_fetch_dtls", SqlRecordParams);
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
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "master_physician_fetch_institutions", SqlRecordParams);
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
        public bool SaveRecord(string ConfigPath,  InstitutionList[] ArrPhysObj, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;

            if (ValidateRecord(ref ReturnMessage))
            {
                if(GenerateinstitutionXML(ArrPhysObj, ref CatchMessage))
                {
                    try
                    {
                        SqlParameter[] SqlRecordParams = new SqlParameter[19];
                        SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id; SqlRecordParams[0].Direction = ParameterDirection.InputOutput;
                        SqlRecordParams[1] = new SqlParameter("@code", SqlDbType.NVarChar, 5); SqlRecordParams[1].Value = strCode;
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
                        SqlRecordParams[12] = new SqlParameter("@is_active", SqlDbType.NChar, 1); SqlRecordParams[12].Value = strIsActive;
                        SqlRecordParams[13] = new SqlParameter("@xml_institution", SqlDbType.NText); if (strXMLInstitution.Trim() != string.Empty) SqlRecordParams[13].Value = strXMLInstitution.Trim(); else SqlRecordParams[13].Value = DBNull.Value;
                        SqlRecordParams[14] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[14].Value = UserID;
                        SqlRecordParams[15] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[15].Value = intMenuID;
                        SqlRecordParams[16] = new SqlParameter("@user_name", SqlDbType.NVarChar, 700); SqlRecordParams[16].Direction = ParameterDirection.Output;
                        SqlRecordParams[17] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[17].Direction = ParameterDirection.Output;
                        SqlRecordParams[18] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[18].Direction = ParameterDirection.Output;


                        if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                        intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "master_physician_save", SqlRecordParams);
                        intReturnValue = Convert.ToInt32(SqlRecordParams[18].Value);
                        if (intReturnValue == 1)
                            bReturn = true;
                        else
                            bReturn = false;

                        strUserName = Convert.ToString(SqlRecordParams[16].Value).Trim();
                        ReturnMessage = Convert.ToString(SqlRecordParams[17].Value);

                        Id = new Guid(Convert.ToString(SqlRecordParams[0].Value));


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
            if (strName.Trim() == string.Empty)
            {
                //if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage = "076";
            }
            if (strEmail.Trim() == string.Empty)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage = "112";
            }
            //else
            //{
            //    if (!CoreCommon.IsEmailValid(strEmail))
            //    {
            //        if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
            //        ReturnMessage += "084";
            //    }
            //}
            if (strMobile.Trim() == string.Empty)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage = "090";
            }

            if (ReturnMessage.Trim() != string.Empty)
                bReturn = false;

            return bReturn;
        }

        #endregion

        #region GenerateinstitutionXML
        private bool GenerateinstitutionXML(InstitutionList[] ArrObj, ref string CatchMessage)
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
                        //sbXML.Append("<physician_user_id>" + ArrObj[i].PHYSICIAN_USER_ID.ToString() + "</physician_user_id>");
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
        Guid InstitutionID = Guid.Empty;
       // Guid PhysicianUserID = Guid.Empty;

        decimal decCommission1stYr = 0;//(for Sales Person) Added on 4th SEP 2019 @BK
        decimal decCommission2ndYr = 0;//(for Sales Person) Added on 4th SEP 2019 @BK
        #endregion

        #region Properties
        public Guid ID
        {
            get { return InstitutionID; }
            set { InstitutionID = value; }
        }
        public decimal COMMISSION_1ST_YEAR//(for Sales Person) Added on 4th SEP 2019 @BK
        {
            get { return decCommission1stYr; }
            set { decCommission1stYr = value; }
        }
        public decimal COMMISSION_2ND_YEAR//(for Sales Person) Added on 4th SEP 2019 @BK
        {
            get { return decCommission2ndYr; }
            set { decCommission2ndYr = value; }
        }
        //public Guid PHYSICIAN_USER_ID
        //{
        //    get { return PhysicianUserID; }
        //    set { PhysicianUserID = value; }
        //}
        #endregion
    }
}
