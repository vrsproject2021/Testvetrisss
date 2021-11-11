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
    public class BillCycle
    {
        #region Constructor
        public BillCycle()
        {
        }
        #endregion

        #region Variables
        int intMenuID = 0;
        Guid UserID = new Guid("00000000-0000-0000-0000-000000000000");
        int intUserRoleID = 0;
        string strUserName = string.Empty;

        Guid Id = new Guid("00000000-0000-0000-0000-000000000000");
        string strName = string.Empty;
        DateTime dtFromDate = DateTime.Today;
        DateTime dtToDate = DateTime.Today;
        string strIsLocked = string.Empty;
        string strXML = string.Empty;
        int intRowID = 0;
        #endregion

        #region Properties
        public Guid ID
        {
            get { return Id; }
            set { Id = value; }
        }
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

        public string NAME
        {
            get { return strName; }
            set { strName = value; }
        }
        
        public DateTime FROM_DATE
        {
            get { return dtFromDate; }
            set { dtFromDate = value; }
        }
        public DateTime TO_DATE
        {
            get { return dtToDate; }
            set { dtToDate = value; }
        }

        public string IS_LOCKED
        {
            get { return strIsLocked; }
            set { strIsLocked = value; }
        }
        public int ROW_ID
        {
            get { return intRowID; }
            set { intRowID = value; }
        }
        #endregion

        #region Browser Methods

        #region SearchBrowserList
        public bool SearchBrowserList(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[5];
            SqlRecordParams[0] = new SqlParameter("@name", SqlDbType.NVarChar, 100);        SqlRecordParams[0].Value = strName;
            SqlRecordParams[1] = new SqlParameter("@menu_id", SqlDbType.Int);               SqlRecordParams[1].Value = intMenuID;
            SqlRecordParams[2] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier);  SqlRecordParams[2].Value = UserID;
            SqlRecordParams[3] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10);   SqlRecordParams[3].Direction = ParameterDirection.Output;
            SqlRecordParams[4] = new SqlParameter("@return_status", SqlDbType.Int);         SqlRecordParams[4].Direction = ParameterDirection.Output;


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "invoicing_billing_cycle_fetch_brw", SqlRecordParams);
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
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "invoicing_billing_cycle_fetch_dtls", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Details";

                    #region Details

                    foreach (DataRow dr in ds.Tables["Details"].Rows)
                    {

                        strName = Convert.ToString(dr["name"]).Trim(); 
                        dtFromDate = Convert.ToDateTime(dr["date_from"]);
                        dtToDate = Convert.ToDateTime(dr["date_till"]);
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

        #region SaveRecord
        public bool SaveRecord(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;

            if (ValidateRecord(ref ReturnMessage))
            {

                try
                {
                    SqlParameter[] SqlRecordParams = new SqlParameter[9];
                    SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id; SqlRecordParams[0].Direction = ParameterDirection.InputOutput;
                    SqlRecordParams[1] = new SqlParameter("@name", SqlDbType.NVarChar, 80); SqlRecordParams[1].Value = strName;
                    SqlRecordParams[2] = new SqlParameter("@date_from", SqlDbType.DateTime); SqlRecordParams[2].Value = dtFromDate;
                    SqlRecordParams[3] = new SqlParameter("@date_till", SqlDbType.DateTime); SqlRecordParams[3].Value = dtToDate;
                    SqlRecordParams[4] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[4].Value = UserID;
                    SqlRecordParams[5] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[5].Value = intMenuID;
                    SqlRecordParams[6] = new SqlParameter("@user_name", SqlDbType.NVarChar, 700); SqlRecordParams[6].Direction = ParameterDirection.Output;
                    SqlRecordParams[7] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[7].Direction = ParameterDirection.Output;
                    SqlRecordParams[8] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[8].Direction = ParameterDirection.Output;

                    if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                    intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "invoicing_billing_cycle_save", SqlRecordParams);
                    intReturnValue = Convert.ToInt32(SqlRecordParams[8].Value);
                    if (intReturnValue == 1)
                        bReturn = true;
                    else
                        bReturn = false;

                    strUserName = Convert.ToString(SqlRecordParams[6].Value).Trim();
                    ReturnMessage = Convert.ToString(SqlRecordParams[7].Value);

                }
                catch (Exception expErr)
                { bReturn = false; CatchMessage = expErr.Message; }



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

            if (strName.Trim().Trim() == string.Empty)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage = "076";
            }
            if (dtFromDate.Year == 1900)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage = "214";
            }

            if (dtToDate.Year == 1900)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage = "215";
            }
            
            //if (Convert.ToDateTime(String.Format(dtFromDate.Trim(),"dd-MM-yyyy")) > Convert.ToDateTime(String.Format(dtToDate.Trim(),"dd-MM-yyyy")))
            //{
            //    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
            //    ReturnMessage = "216";
            //}

            if (ReturnMessage.Trim() != string.Empty)
                bReturn = false;

            return bReturn;
        }

        #endregion

        #endregion
    }
}
