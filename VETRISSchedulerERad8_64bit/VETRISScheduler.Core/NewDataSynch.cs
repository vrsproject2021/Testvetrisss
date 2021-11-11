using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using VETRISScheduler.DAL;

namespace VETRISScheduler.Core
{
    public class NewDataSynch
    {
        #region Constructor
        public NewDataSynch()
        {
        }
        #endregion

        #region Variables
        string strSUID = string.Empty;
        #endregion

        #region Properties
        public string STUDY_UID
        {
            get { return strSUID; }
            set { strSUID = value; }
        }
        #endregion

        #region SaveNewSynchedData
        public bool SaveNewSynchedData(string ConfigPath,int intServiceID, string strSvcName, DataTable dtbl, ref int SyncCount, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; int intReturnType = 0;
            SqlParameter[] SqlRecordParams = new SqlParameter[29];
            string strSUID = string.Empty; string strField = string.Empty;

            SyncCount = 0;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);

                foreach (DataRow dr in dtbl.Rows)
                {
                    try
                    {
                        strSUID = Convert.ToString(dr["study_uid"]).Trim();
                        strField = "study_uid"; SqlRecordParams[0] = new SqlParameter("@study_uid", SqlDbType.NVarChar, 100); SqlRecordParams[0].Value = Convert.ToString(dr["study_uid"]).Trim();
                        strField = "study_date"; SqlRecordParams[1] = new SqlParameter("@study_date", SqlDbType.DateTime); SqlRecordParams[1].Value = Convert.ToDateTime(dr["study_date"]);
                        strField = "received_date"; SqlRecordParams[2] = new SqlParameter("@received_date", SqlDbType.DateTime); SqlRecordParams[2].Value = Convert.ToDateTime(dr["received_date"]);
                        strField = "accession_no"; SqlRecordParams[3] = new SqlParameter("@accession_no", SqlDbType.NVarChar, 20); SqlRecordParams[3].Value = Convert.ToString(dr["accession_no"]).Trim();
                        strField = "reason"; SqlRecordParams[4] = new SqlParameter("@reason", SqlDbType.NVarChar, 500); SqlRecordParams[4].Value = Convert.ToString(dr["reason"]).Trim();
                        strField = "institution_name"; SqlRecordParams[5] = new SqlParameter("@institution_name", SqlDbType.NVarChar, 100); SqlRecordParams[5].Value = Convert.ToString(dr["institution_name"]).Trim();
                        strField = "manufacturer_name"; SqlRecordParams[6] = new SqlParameter("@manufacturer_name", SqlDbType.NVarChar, 100); SqlRecordParams[6].Value = Convert.ToString(dr["manufacturer_name"]).Trim();
                        strField = "device_serial_no"; SqlRecordParams[7] = new SqlParameter("@device_serial_no", SqlDbType.NVarChar, 20); SqlRecordParams[7].Value = Convert.ToString(dr["device_serial_no"]).Trim();
                        strField = "referring_physician"; SqlRecordParams[8] = new SqlParameter("@referring_physician", SqlDbType.NVarChar, 200); SqlRecordParams[8].Value = Convert.ToString(dr["referring_physician"]).Trim();
                        strField = "patient_id"; SqlRecordParams[9] = new SqlParameter("@patient_id", SqlDbType.NVarChar, 20); SqlRecordParams[9].Value = Convert.ToString(dr["patient_id"]).Trim();
                        strField = "patient_name"; SqlRecordParams[10] = new SqlParameter("@patient_name", SqlDbType.NVarChar, 100); SqlRecordParams[10].Value = Convert.ToString(dr["patient_name"]).Trim();
                        strField = "patient_sex"; SqlRecordParams[11] = new SqlParameter("@patient_sex", SqlDbType.NVarChar, 10); SqlRecordParams[11].Value = Convert.ToString(dr["patient_sex"]).Trim();
                        strField = "patient_dob"; SqlRecordParams[12] = new SqlParameter("@patient_dob", SqlDbType.DateTime); if(dr["patient_dob"] != DBNull.Value) SqlRecordParams[12].Value = Convert.ToDateTime(dr["patient_dob"]); else  SqlRecordParams[12].Value = Convert.ToDateTime("01jan1900");
                        strField = "patient_age"; SqlRecordParams[13] = new SqlParameter("@patient_age", SqlDbType.NVarChar, 50); SqlRecordParams[13].Value = Convert.ToString(dr["patient_age"]);
                        strField = "patient_weight"; SqlRecordParams[14] = new SqlParameter("@patient_weight", SqlDbType.Decimal); SqlRecordParams[14].Value = Convert.ToDecimal(dr["patient_weight"]);
                        strField = "owner"; SqlRecordParams[15] = new SqlParameter("@owner_name", SqlDbType.NVarChar, 100); SqlRecordParams[15].Value = Convert.ToString(dr["owner"]).Trim();
                        strField = "species"; SqlRecordParams[16] = new SqlParameter("@species", SqlDbType.NVarChar, 30); SqlRecordParams[16].Value = Convert.ToString(dr["species"]).Trim();
                        strField = "breed"; SqlRecordParams[17] = new SqlParameter("@breed", SqlDbType.NVarChar, 50); SqlRecordParams[17].Value = Convert.ToString(dr["breed"]).Trim();
                        strField = "modality"; SqlRecordParams[18] = new SqlParameter("@modality", SqlDbType.NVarChar, 50); SqlRecordParams[18].Value = Convert.ToString(dr["modality"]).Trim();
                        strField = "body_part"; SqlRecordParams[19] = new SqlParameter("@body_part", SqlDbType.NVarChar, 50); SqlRecordParams[19].Value = Convert.ToString(dr["body_part"]).Trim();
                        strField = "manufacturer_model_no"; SqlRecordParams[20] = new SqlParameter("@manufacturer_model_no", SqlDbType.NVarChar, 50); SqlRecordParams[20].Value = Convert.ToString(dr["manufacturer_model_no"]).Trim();
                        strField = "spayed_neutered"; SqlRecordParams[21] = new SqlParameter("@sex_neutered", SqlDbType.NVarChar, 30); SqlRecordParams[21].Value = Convert.ToString(dr["sex_neutered"]).Trim();
                        strField = "img_count"; SqlRecordParams[22] = new SqlParameter("@img_count", SqlDbType.Int); SqlRecordParams[22].Value = Convert.ToInt32(dr["img_count"]);
                        strField = "study_desc"; SqlRecordParams[23] = new SqlParameter("@study_desc", SqlDbType.NVarChar, 500); SqlRecordParams[23].Value = Convert.ToString(dr["study_desc"]).Trim();
                        strField = "modality_ae_title"; SqlRecordParams[24] = new SqlParameter("@modality_ae_title", SqlDbType.NVarChar, 500); SqlRecordParams[24].Value = Convert.ToString(dr["modality_ae_title"]).Trim();
                        strField = "priority_id"; SqlRecordParams[25] = new SqlParameter("@priority_id", SqlDbType.Int); SqlRecordParams[25].Value = Convert.ToInt32(dr["priority_id"]);
                        strField = "object_count"; SqlRecordParams[26] = new SqlParameter("@object_count", SqlDbType.Int); if (dr["object_count"] != DBNull.Value) SqlRecordParams[26].Value = Convert.ToInt32(dr["object_count"]); else SqlRecordParams[26].Value = 0;
                        SqlRecordParams[27] = new SqlParameter("@error_msg", SqlDbType.VarChar, 500); SqlRecordParams[27].Direction = ParameterDirection.Output;
                        SqlRecordParams[28] = new SqlParameter("@return_type", SqlDbType.Int); SqlRecordParams[28].Direction = ParameterDirection.Output;

                        intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_new_data_synch_save", SqlRecordParams);

                        intReturnType = Convert.ToInt32(SqlRecordParams[28].Value); 
                        if (intReturnType == 0)
                        {
                            ReturnMessage = Convert.ToString(SqlRecordParams[27].Value);
                            CoreCommon.doLog(ConfigPath, intServiceID, strSvcName, "Core : SaveNewSynchedData() :: Study UID : " + strSUID + " - Error: " + ReturnMessage, true);
                        }
                        else if (intReturnType == 1)
                        {
                            SyncCount = SyncCount + 1;
                        }
                        bReturn = true;
                    }
                    catch (Exception ex)
                    {
                        CoreCommon.doLog(ConfigPath, intServiceID, strSvcName, "Core : SaveNewSynchedData() :: Study UID : " + strSUID +  " Field : " + strField + " - Exception: " + ex.Message.Trim(), true);
                    }

                }
                
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message + "-" + (SyncCount + 1).ToString(); }
            
            return bReturn;
        }
        #endregion

        #region CreateSynchFailureNotification (Suspended)
        //public bool CreateSynchFailureNotification(string ConfigPath, int ServiceID, string strSvcName, ref string CatchMessage)
        //{
        //    bool bReturn = false; int intExecReturn = 0; int intReturnType = 0;
        //    string strReturnMessage = string.Empty;
        //    SqlParameter[] SqlRecordParams = new SqlParameter[3];

        //    try
        //    {
        //        if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);


        //        SqlRecordParams[0] = new SqlParameter("@study_uid", SqlDbType.NVarChar, 100); SqlRecordParams[0].Value = strSUID;
        //        SqlRecordParams[1] = new SqlParameter("@error_msg", SqlDbType.VarChar, 500); SqlRecordParams[1].Direction = ParameterDirection.Output;
        //        SqlRecordParams[2] = new SqlParameter("@return_type", SqlDbType.Int); SqlRecordParams[2].Direction = ParameterDirection.Output;

        //        intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_new_study_synch_failure_notification_create", SqlRecordParams);
        //        intReturnType = Convert.ToInt32(SqlRecordParams[2].Value);
        //        strReturnMessage = Convert.ToString(SqlRecordParams[1].Value).Trim();

        //        if (intReturnType == 0)
        //        {
        //            CoreCommon.doLog(ConfigPath, ServiceID, strSvcName, "CreateSynchFailureNotification() - Error: " + strReturnMessage, true);
        //        }
        //        else if (intReturnType == 1)
        //        {
        //            CoreCommon.doLog(ConfigPath, ServiceID, strSvcName, "CreateSynchFailureNotification() - " + strReturnMessage, false);
        //        }
        //        bReturn = true;
        //    }
        //    catch (Exception expErr)
        //    { bReturn = false; CatchMessage = expErr.Message + "-" + strSUID; }

        //    return bReturn;
        //}
        #endregion
    }
}
