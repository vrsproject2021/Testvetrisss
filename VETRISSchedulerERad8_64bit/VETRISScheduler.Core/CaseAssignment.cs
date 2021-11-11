using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using VETRISScheduler.DAL;

namespace VETRISScheduler.Core
{
    public class CaseAssignment
    {
        #region Constructor
        public CaseAssignment()
        {
        }
        #endregion

        #region Variables
        Guid StudyID = new Guid("00000000-0000-0000-0000-000000000000");
        string strSUID = string.Empty;
        #endregion

        #region Properties
        public Guid STUDY_ID
        {
            get { return StudyID; }
            set { StudyID = value; }
        }
        public string STUDY_UID
        {
            get { return strSUID; }
            set { strSUID = value; }
        }

        #endregion

        #region UpdateRoaster
        public bool UpdateRoaster(string ConfigPath, int ServiceID, string strSvcName, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; int intReturnType = 0;
            string strReturnMessage = string.Empty;
            SqlParameter[] SqlRecordParams = new SqlParameter[2];

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);

                SqlRecordParams[0] = new SqlParameter("@error_msg", SqlDbType.VarChar, 500); SqlRecordParams[0].Direction = ParameterDirection.Output;
                SqlRecordParams[1] = new SqlParameter("@return_type", SqlDbType.Int); SqlRecordParams[1].Direction = ParameterDirection.Output;

                intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_radiologist_roaster_update", SqlRecordParams);
                intReturnType = Convert.ToInt32(SqlRecordParams[1].Value);
                strReturnMessage = Convert.ToString(SqlRecordParams[0].Value).Trim();

                if (intReturnType == 0)
                {
                    CoreCommon.doLog(ConfigPath, ServiceID, strSvcName, "UpdateRoaster() - Error: " + strReturnMessage, true);
                }

                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }
        #endregion

        #region FetchStudiesToAssign
        public bool FetchStudiesToAssign(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_radiologist_assign_study_fetch");
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "StudyList";
                }
                bReturn = true;

            }
            catch (Exception expErr)
            {
                bReturn = false; CatchMessage = expErr.Message + " Study UID : " + strSUID;
            }

            return bReturn;
        }
        #endregion

        #region AssignRadiologist
        public bool AssignRadiologist(string ConfigPath, int ServiceID, string strSvcName, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; int intReturnType = 0;
            string strReturnMessage = string.Empty;
            SqlParameter[] SqlRecordParams = new SqlParameter[3];

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);


                SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = StudyID;
                SqlRecordParams[1] = new SqlParameter("@error_msg", SqlDbType.VarChar, 500); SqlRecordParams[1].Direction = ParameterDirection.Output;
                SqlRecordParams[2] = new SqlParameter("@return_type", SqlDbType.Int); SqlRecordParams[2].Direction = ParameterDirection.Output;

                intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_radiologist_assign", SqlRecordParams);
                intReturnType = Convert.ToInt32(SqlRecordParams[2].Value);
                strReturnMessage = Convert.ToString(SqlRecordParams[1].Value).Trim();

                if (intReturnType == 0)
                {
                    CoreCommon.doLog(ConfigPath, ServiceID, strSvcName, "AssignRadiologist() - Error: " + strReturnMessage, true);
                }

                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message + "-" + Convert.ToString(StudyID); }

            return bReturn;
        }
        #endregion
    }
}
