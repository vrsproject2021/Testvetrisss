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

namespace VETRIS.Core.HouseKeeping
{
    public class DRLog
    {
        #region Constructor
        public DRLog()
        {
        }
        #endregion

        #region Variables
        int intMenuID = 0;
        Guid UserID = new Guid("00000000-0000-0000-0000-000000000000");
        int intUserRoleID = 0;
        string strUserName = string.Empty;

        DateTime dtDateFrom = DateTime.Today.AddDays(-7);
        DateTime dtDateTill = DateTime.Today;
        Guid InstitutionID = new Guid("00000000-0000-0000-0000-000000000000");
        int intServiceID = 0;
        string strType = "N";
        string strLogMessage = string.Empty;
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

        public DateTime DATE_FROM
        {
            get { return dtDateFrom; }
            set { dtDateFrom = value; }
        }
        public DateTime DATE_TILL
        {
            get { return dtDateTill; }
            set { dtDateTill = value; }
        }
        public Guid INSTITUTION_ID
        {
            get { return InstitutionID; }
            set { InstitutionID = value; }
        }
        public int SERVICE_ID
        {
            get { return intServiceID; }
            set { intServiceID = value; }
        }
        public string LOG_TYPE
        {
            get { return strType; }
            set { strType = value; }
        }
        public string LOG_MESSAGE
        {
            get { return strLogMessage; }
            set { strLogMessage = value; }
        }
        #endregion

        #region SearchBrowserList
        public bool SearchBrowserList(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[6];
            SqlRecordParams[0] = new SqlParameter("@study_date_from", SqlDbType.DateTime); SqlRecordParams[0].Value = dtDateFrom;
            SqlRecordParams[1] = new SqlParameter("@study_date_till", SqlDbType.DateTime); SqlRecordParams[1].Value = dtDateTill;
            SqlRecordParams[2] = new SqlParameter("@institution_id", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = InstitutionID;
            SqlRecordParams[3] = new SqlParameter("@service_id", SqlDbType.Int); SqlRecordParams[3].Value = intServiceID;
            SqlRecordParams[4] = new SqlParameter("@type", SqlDbType.NChar, 1); SqlRecordParams[4].Value = strType;
            SqlRecordParams[5] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[5].Value = UserID;


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "hk_dicom_router_log_fetch", SqlRecordParams);
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

        #region SearchUserActivityList
        public bool SearchUserActivityList(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[4];
            SqlRecordParams[0] = new SqlParameter("@date_from", SqlDbType.DateTime); SqlRecordParams[0].Value = dtDateFrom;
            SqlRecordParams[1] = new SqlParameter("@date_till", SqlDbType.DateTime); SqlRecordParams[1].Value = dtDateTill;
            SqlRecordParams[2] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = UserID;
            SqlRecordParams[3] = new SqlParameter("@activity_text", SqlDbType.NVarChar,8000); SqlRecordParams[3].Value = strLogMessage;


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "hk_user_activity_log_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "UserActivity";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region FetchBrowserParameters
        public bool FetchBrowserParameters(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];

            try
            {
                SqlRecordParams[0] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = UserID;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "case_list_brw_fetch_params", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[2].TableName = "Institutions";
                    ds.Tables[19].TableName = "Users";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion
    }
}
