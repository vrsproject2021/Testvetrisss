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

namespace VETRIS.API.Core.CHAT
{
    public class Chat
    {
        #region Constructor
        public Chat()
        {
        }
        #endregion

        #region Variables
        private Guid UserID = Guid.Empty;
        private string strUserName = string.Empty;
        private string strUserRoleCode = string.Empty;
        private string strUserRoleDesc = string.Empty;
        private string strEmailID = string.Empty;
        private string strContactNo = string.Empty;
       
        #endregion

        #region Properties
        public Guid USER_ID
        {
            get { return UserID; }
            set { UserID = value; }
        }
        public string USER_ROLE_CODE
        {
            get { return strUserRoleCode; }
            set { strUserRoleCode = value; }
        }
        public string USER_NAME
        {
            get { return strUserName; }
            set { strUserName = value; }
        }
        public string USER_ROLE_DESCRIPTION
        {
            get { return strUserRoleDesc; }
            set { strUserRoleDesc = value; }
        }
        public string EMAIL_ID
        {
            get { return strEmailID; }
            set { strEmailID = value; }
        }
        public string CONTACT_NUMBER
        {
            get { return strContactNo; }
            set { strContactNo = value; }
        }
        #endregion

        #region FetchUserDetails
        public bool FetchUserDetails(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; int intRetStatus = 0;

            SqlParameter[] SqlRecordParams = new SqlParameter[8];
            SqlRecordParams[0] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = UserID;
            SqlRecordParams[1] = new SqlParameter("@user_role", SqlDbType.NVarChar, 5); SqlRecordParams[1].Direction = ParameterDirection.Output;
            SqlRecordParams[2] = new SqlParameter("@user_role_desc", SqlDbType.NVarChar, 30); SqlRecordParams[2].Direction = ParameterDirection.Output;
            SqlRecordParams[3] = new SqlParameter("@name", SqlDbType.NVarChar, 100); SqlRecordParams[3].Direction = ParameterDirection.Output;
            SqlRecordParams[4] = new SqlParameter("@email_id", SqlDbType.NVarChar, 100); SqlRecordParams[4].Direction = ParameterDirection.Output;
            SqlRecordParams[5] = new SqlParameter("@contact_no", SqlDbType.NVarChar, 100); SqlRecordParams[5].Direction = ParameterDirection.Output;
            SqlRecordParams[6] = new SqlParameter("@output_msg", SqlDbType.NVarChar, 100); SqlRecordParams[6].Direction = ParameterDirection.Output;
            SqlRecordParams[7] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[7].Direction = ParameterDirection.Output;


            try
            {
                if (ConfigPath.Trim() != string.Empty) CoreCommon.GetConnectionString(ConfigPath);

                intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "api_chat_user_validate", SqlRecordParams);
                intRetStatus = Convert.ToInt32(SqlRecordParams[7].Value);

                if (intRetStatus == 1)
                {
                    bReturn = true;
                    strUserRoleCode = Convert.ToString(SqlRecordParams[1].Value).Trim();
                    strUserRoleDesc = Convert.ToString(SqlRecordParams[2].Value).Trim();
                    strUserName = Convert.ToString(SqlRecordParams[3].Value).Trim();
                    strEmailID = Convert.ToString(SqlRecordParams[4].Value).Trim();
                    strContactNo = Convert.ToString(SqlRecordParams[5].Value).Trim();
                }
                else
                {
                    bReturn = false;
                }

                ReturnMessage = Convert.ToString(SqlRecordParams[6].Value).Trim();

            }
            catch (Exception expErr)
            {
                bReturn = false;
                CatchMessage = expErr.Message;
            }



            return bReturn;
        }
        #endregion
    }
}
