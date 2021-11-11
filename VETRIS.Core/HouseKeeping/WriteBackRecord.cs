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
    public class WriteBackRecord
    {
        #region Constructor
        public WriteBackRecord()
        {
        }
        #endregion

        #region Variables
        int intMenuID = 0;
        Guid UserID = new Guid("00000000-0000-0000-0000-000000000000");
        int intUserRoleID = 0;
        string strUserName = string.Empty;

        Guid Id = new Guid("00000000-0000-0000-0000-000000000000");
        DateTime dtFrom = DateTime.Today.AddDays(-15);
        DateTime dtTill = DateTime.Today;
        string strWB = string.Empty;
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
        public DateTime FROM_DATE
        {
            get { return dtFrom; }
            set { dtFrom = value; }
        }
        public DateTime TILL_DATE
        {
            get { return dtTill; }
            set { dtTill = value; }
        }
        public string WRITE_BACK
        {
            get { return strWB; }
            set { strWB = value; }
        }
        #endregion

        #region FetchStudies
        public bool FetchStudies(string ConfigPath,ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[2];
            

            try
            {
                SqlRecordParams[0] = new SqlParameter("@from_date", SqlDbType.DateTime); SqlRecordParams[0].Value = dtFrom;
                SqlRecordParams[1] = new SqlParameter("@till_date", SqlDbType.DateTime); SqlRecordParams[1].Value = dtTill;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "hk_write_back_study_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Studies";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }
            finally
            {
                ds.Dispose();
            }

            return bReturn;
        }
        #endregion

        #region UpdateStudy
        public bool UpdateStudy(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; int intReturnType = 0;
            SqlParameter[] SqlRecordParams = new SqlParameter[4];


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);

                SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
                SqlRecordParams[1] = new SqlParameter("@pacs_wb", SqlDbType.NChar,1); SqlRecordParams[1].Value = strWB;
                SqlRecordParams[2] = new SqlParameter("@error_code", SqlDbType.VarChar, 10); SqlRecordParams[2].Direction = ParameterDirection.Output;
                SqlRecordParams[3] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[3].Direction = ParameterDirection.Output;

                intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "hk_write_back_study_flag_update", SqlRecordParams);

                intReturnType = Convert.ToInt32(SqlRecordParams[3].Value);
                if (intReturnType == 0)
                {
                    bReturn = false;
                }
                else
                {
                    bReturn = true;
                }

                ReturnMessage = Convert.ToString(SqlRecordParams[2].Value);
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }
        #endregion
    }
}
