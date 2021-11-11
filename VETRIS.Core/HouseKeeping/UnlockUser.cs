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
    public class UnlockUser
    {

        #region Constructor
        public UnlockUser()
        {
        }
        #endregion

        #region Variables
        Guid UserID = new Guid("00000000-0000-0000-0000-000000000000");
        Guid UserSessID = new Guid("00000000-0000-0000-0000-000000000000");
        Guid UpdatedBy = new Guid("00000000-0000-0000-0000-000000000000");
        Guid SessionID = new Guid("00000000-0000-0000-0000-000000000000");
        string strCode = string.Empty;
        string strName = string.Empty;
        #endregion

        #region Properties
        public Guid USER_ID
        {
            get { return UserID; }
            set { UserID = value; }
        }
        public Guid USER_SESSION_ID
        {
            get { return UserSessID; }
            set { UserSessID = value; }
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
        public Guid UPDATED_BY
        {
            get { return UpdatedBy; }
            set { UpdatedBy = value; }
        }
        public Guid SESSION_ID
        {
            get { return SessionID; }
            set { SessionID = value; }
        }
        #endregion

        #region SearchBrowserList
        public bool SearchBrowserList(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;

            SqlParameter[] SqlRecordParams = new SqlParameter[3];
            SqlRecordParams[0] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = UserID;
            SqlRecordParams[1] = new SqlParameter("@code", SqlDbType.NVarChar, 6); SqlRecordParams[1].Value = strCode;
            SqlRecordParams[2] = new SqlParameter("@name", SqlDbType.NVarChar, 30); SqlRecordParams[2].Value = strName;


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "hk_unlock_user_fetch", SqlRecordParams);
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

        #region UserUnlock
        public bool UserUnlock(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false; int intReturnValue = 0; int intExecReturn = 0;
            SqlParameter[] SqlRecordParams = new SqlParameter[6];


            SqlRecordParams[0] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = UserID;
            SqlRecordParams[1] = new SqlParameter("@session_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = UserSessID;
            SqlRecordParams[2] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = UpdatedBy;
            SqlRecordParams[3] = new SqlParameter("@update_session_id", SqlDbType.UniqueIdentifier); SqlRecordParams[3].Value = SessionID;
            SqlRecordParams[4] = new SqlParameter("@error_code", SqlDbType.VarChar, 10); SqlRecordParams[4].Direction = ParameterDirection.Output;
            SqlRecordParams[5] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[5].Direction = ParameterDirection.Output;
            
            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty)
                    CoreCommon.GetConnectionString(ConfigPath);

                intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "hk_unlock_user", SqlRecordParams);
                intReturnValue = Convert.ToInt32(SqlRecordParams[5].Value);

                if (intReturnValue == 1)
                    bReturn = true;
                else
                    bReturn = false;

                ReturnMessage = Convert.ToString(SqlRecordParams[4].Value);

            }
            catch (Exception LexpErr)
            { bReturn = false; CatchMessage = LexpErr.Message; }


            return bReturn;
        }
        #endregion
    }
}
