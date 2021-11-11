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

namespace VETRIS.Core.DownloadRouter
{
    public class DownloadRouter
    {
        #region Constructor
        public DownloadRouter()
        {
        }
        #endregion

        #region Variables
        Guid UserID = new Guid("00000000-0000-0000-0000-000000000000");
        string strVersion = string.Empty;
        #endregion

        #region Properties
       
        public Guid USER_ID
        {
            get { return UserID; }
            set { UserID = value; }
        }
        
        public string VERSION
        {
            get { return strVersion; }
            set { strVersion = value; }
        }
        
        #endregion

        #region FetchDetails
        public bool FetchDetails(string ConfigPath, ref DataSet ds,  ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            SqlRecordParams[0] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = UserID;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "dicom_router_dowload_dlts_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Version";
                    ds.Tables[1].TableName = "Institutions";

                    foreach (DataRow dr in ds.Tables["Version"].Rows)
                    {
                        strVersion = Convert.ToString(dr["version_no"]).Trim();
                    }

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
    }
}
