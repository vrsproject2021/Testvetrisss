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

namespace VETRIS.Core.Study
{
    public class DCMStudy
    {
        #region Constructor
        public DCMStudy()
        {
        }
        #endregion

        #region Variables
        int intMenuID = 0;
        Guid UserID = new Guid("00000000-0000-0000-0000-000000000000");
        int intUserRoleID = 0;
        string strUserName = string.Empty;
        DateTime dtStudyDate = DateTime.Today;

        Guid Id = new Guid("00000000-0000-0000-0000-000000000000");
        string strPatientID = string.Empty;
        string strStudyUID = string.Empty;

        string strFilterDownloadDt = "N";
        DateTime dtDlDateFrom = DateTime.Today.AddDays(-7);
        DateTime dtDlDateTill = DateTime.Today;
        DateTime dtDlDate = DateTime.Today;
        string strPatientName = string.Empty;
        string strPatientFName = string.Empty;
        string strPatientLName = string.Empty;
        Guid InstitutionID = new Guid("00000000-0000-0000-0000-000000000000");
        string strInstitutionName = string.Empty;
        string strInstitutionCode = string.Empty;
        int intFileCount = 0;
        int intFileXfered = 0;
        string strApproved = string.Empty;
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
        public string PATIENT_ID
        {
            get { return strPatientID; }
            set { strPatientID = value; }
        }
        public string PATIENT_NAME
        {
            get { return strPatientName; }
            set { strPatientName = value; }
        }
        public string PATIENT_FIRST_NAME
        {
            get { return strPatientFName; }
            set { strPatientFName = value; }
        }
        public string PATIENT_LAST_NAME
        {
            get { return strPatientLName; }
            set { strPatientLName = value; }
        }
        public string STUDY_UID
        {
            get { return strStudyUID; }
            set { strStudyUID = value; }
        }
        public string FILTER_BY_STUDY_DATE
        {
            get { return strFilterDownloadDt; }
            set { strFilterDownloadDt = value; }
        }
        public DateTime STUDY_DATE_FROM
        {
            get { return dtDlDateFrom; }
            set { dtDlDateFrom = value; }
        }
        public DateTime STUDY_DATE_TILL
        {
            get { return dtDlDateTill; }
            set { dtDlDateTill = value; }
        }
        public DateTime DOWNLOAD_DATE
        {
            get { return dtDlDate; }
            set { dtDlDate = value; }
        }
        public DateTime STUDY_DATE
        {
            get { return dtStudyDate; }
            set { dtStudyDate = value; }
        }
        public Guid INSTITUTION_ID
        {
            get { return InstitutionID; }
            set { InstitutionID = value; }
        }
        public string INSTITUTION_NAME
        {
            get { return strInstitutionName; }
            set { strInstitutionName = value; }
        }
        public string INSTITUTION_CODE
        {
            get { return strInstitutionCode; }
            set { strInstitutionCode = value; }
        }
        public int FILE_COUNT
        {
            get { return intFileCount; }
            set { intFileCount = value; }
        }
        public int FILE_TRANSFERED
        {
            get { return intFileXfered; }
            set { intFileXfered = value; }
        }
        public string APPROVED
        {
            get { return strApproved; }
            set { strApproved = value; }
        }
        #endregion

        #region Browser Methods

        #region SearchBrowser
        public bool SearchBrowser(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[9];

            SqlRecordParams[0] = new SqlParameter("@patient_id", SqlDbType.NVarChar, 200); SqlRecordParams[0].Value = strPatientID;
            SqlRecordParams[1] = new SqlParameter("@patient_name", SqlDbType.NVarChar, 200); SqlRecordParams[1].Value = strPatientName;
            SqlRecordParams[2] = new SqlParameter("@institution_id", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = InstitutionID;
            SqlRecordParams[3] = new SqlParameter("@consider_study_date", SqlDbType.NChar, 1); SqlRecordParams[3].Value = strFilterDownloadDt;
            SqlRecordParams[4] = new SqlParameter("@study_date_from", SqlDbType.DateTime); SqlRecordParams[4].Value = dtDlDateFrom;
            SqlRecordParams[5] = new SqlParameter("@study_date_till", SqlDbType.DateTime); SqlRecordParams[5].Value = dtDlDateTill;
            SqlRecordParams[6] = new SqlParameter("@study_uid", SqlDbType.NVarChar, 100); SqlRecordParams[6].Value = strStudyUID;
            SqlRecordParams[7] = new SqlParameter("@approved", SqlDbType.NChar, 1); SqlRecordParams[7].Value = strApproved;
            SqlRecordParams[8] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[8].Value = UserID;


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "study_rec_dcm_img_fetch_brw", SqlRecordParams);
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
                    ds.Tables[0].TableName = "Modality";
                    ds.Tables[1].TableName = "Species";
                    ds.Tables[2].TableName = "Institutions";
                    ds.Tables[3].TableName = "InProgressStatus";
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

        #region LoadHeader
        public bool LoadHeader(string ConfigPath, ref DataSet ds, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false; string strControlCode = string.Empty;

            SqlParameter[] SqlRecordParams = new SqlParameter[5];


            try
            {
                SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
                SqlRecordParams[1] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[1].Value = intMenuID;
                SqlRecordParams[2] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = UserID;
                SqlRecordParams[3] = new SqlParameter("@error_code", SqlDbType.VarChar, 10); SqlRecordParams[3].Direction = ParameterDirection.Output;
                SqlRecordParams[4] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[4].Direction = ParameterDirection.Output;

                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "study_rec_dcm_fetch_hdr", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Details";
                   

                    #region Details

                    foreach (DataRow dr in ds.Tables["Details"].Rows)
                    {
                        Id = new Guid(Convert.ToString(dr["id"]).Trim());
                        strStudyUID = Convert.ToString(dr["study_uid"]).Trim();
                        dtStudyDate = Convert.ToDateTime(dr["study_date"]);
                        dtDlDate = Convert.ToDateTime(dr["date_downloaded"]);
                        strPatientID = Convert.ToString(dr["patient_id"]).Trim();
                        strPatientFName = Convert.ToString(dr["patient_fname"]).Trim();
                        strPatientLName = Convert.ToString(dr["patient_lname"]).Trim();
                        InstitutionID = new Guid(Convert.ToString(dr["institution_id"]));
                        strInstitutionCode = Convert.ToString(dr["institution_code"]).Trim();
                        strInstitutionName = Convert.ToString(dr["institution_name"]).Trim();
                        intFileCount = Convert.ToInt32(dr["file_count"]);
                        intFileXfered = Convert.ToInt32(dr["file_xfer_count"]);
                        strApproved =  Convert.ToString(dr["approve_for_pacs"]).Trim();
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

        #region LoadFiles
        public bool LoadFiles(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "study_rec_dcm_fetch_files", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Files";
                }
                bReturn = true;
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
                
                    SqlParameter[] SqlRecordParams = new SqlParameter[12];
                    try
                    {
                        SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
                        SqlRecordParams[1] = new SqlParameter("@study_uid", SqlDbType.NVarChar,100); SqlRecordParams[1].Value = strStudyUID;
                        SqlRecordParams[2] = new SqlParameter("@study_date", SqlDbType.DateTime); SqlRecordParams[2].Value = dtStudyDate;
                        SqlRecordParams[3] = new SqlParameter("@patient_id", SqlDbType.NVarChar, 20); SqlRecordParams[3].Value = strPatientID;
                        SqlRecordParams[4] = new SqlParameter("@patient_fname", SqlDbType.NVarChar, 40); SqlRecordParams[4].Value = strPatientFName;
                        SqlRecordParams[5] = new SqlParameter("@patient_lname", SqlDbType.NVarChar, 40); SqlRecordParams[5].Value = strPatientLName;
                        SqlRecordParams[6] = new SqlParameter("@approve_for_pacs", SqlDbType.NChar, 1); SqlRecordParams[6].Value = strApproved;
                        SqlRecordParams[7] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[7].Value = UserID;
                        SqlRecordParams[8] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[8].Value = intMenuID;
                        SqlRecordParams[9] = new SqlParameter("@user_name", SqlDbType.NVarChar, 500); SqlRecordParams[9].Direction = ParameterDirection.Output;
                        SqlRecordParams[10] = new SqlParameter("@error_code", SqlDbType.VarChar, 10); SqlRecordParams[10].Direction = ParameterDirection.Output;
                        SqlRecordParams[11] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[11].Direction = ParameterDirection.Output;

                        if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                        intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "study_rec_dcm_save", SqlRecordParams);
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

            if (strPatientID.Trim() == string.Empty)
            {
                ReturnMessage = "048";
            }
            if (strPatientName.Trim() == string.Empty)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage = "049";
            }
            if (dtStudyDate.Year == 1900)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage = "178";
            }

            if (ReturnMessage.Trim() != string.Empty)
                bReturn = false;

            return bReturn;
        }

        #endregion

        #endregion
    }
}
