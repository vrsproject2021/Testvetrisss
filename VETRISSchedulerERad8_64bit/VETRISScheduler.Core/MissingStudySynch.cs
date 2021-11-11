using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using VETRISScheduler.DAL;

namespace VETRISScheduler.Core
{
    public class MissingStudySynch
    {
        #region Constructor
        public MissingStudySynch()
        {
        }
        #endregion

        #region SaveMissingSynchedData
        public bool SaveMissingSynchedData(string ConfigPath, string strSvcName,int intServiceID, DataTable dtbl, ref int SyncCount, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false; int intExecReturn = 0; int intReturnType = 0;
            SqlParameter[] SqlRecordParams = new SqlParameter[40];
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
                        strField = "reason"; SqlRecordParams[4] = new SqlParameter("@reason", SqlDbType.NVarChar, 2000); SqlRecordParams[4].Value = Convert.ToString(dr["reason"]).Trim();
                        strField = "institution_name"; SqlRecordParams[5] = new SqlParameter("@institution_name", SqlDbType.NVarChar, 100); SqlRecordParams[5].Value = Convert.ToString(dr["institution_name"]).Trim();
                        strField = "manufacturer_name"; SqlRecordParams[6] = new SqlParameter("@manufacturer_name", SqlDbType.NVarChar, 100); SqlRecordParams[6].Value = Convert.ToString(dr["manufacturer_name"]).Trim();
                        strField = "device_serial_no"; SqlRecordParams[7] = new SqlParameter("@device_serial_no", SqlDbType.NVarChar, 20); SqlRecordParams[7].Value = Convert.ToString(dr["device_serial_no"]).Trim();
                        strField = "referring_physician"; SqlRecordParams[8] = new SqlParameter("@referring_physician", SqlDbType.NVarChar, 200); SqlRecordParams[8].Value = Convert.ToString(dr["referring_physician"]).Trim();
                        strField = "patient_id"; SqlRecordParams[9] = new SqlParameter("@patient_id", SqlDbType.NVarChar, 20); SqlRecordParams[9].Value = Convert.ToString(dr["patient_id"]).Trim();
                        strField = "patient_name"; SqlRecordParams[10] = new SqlParameter("@patient_name", SqlDbType.NVarChar, 100); SqlRecordParams[10].Value = Convert.ToString(dr["patient_name"]).Trim();
                        strField = "patient_sex"; SqlRecordParams[11] = new SqlParameter("@patient_sex", SqlDbType.NVarChar, 10); SqlRecordParams[11].Value = Convert.ToString(dr["patient_sex"]).Trim();
                        strField = "patient_dob"; SqlRecordParams[12] = new SqlParameter("@patient_dob", SqlDbType.DateTime); SqlRecordParams[12].Value = Convert.ToDateTime(dr["patient_dob"]);
                        strField = "patient_age"; SqlRecordParams[13] = new SqlParameter("@patient_age", SqlDbType.NVarChar, 50); SqlRecordParams[13].Value = Convert.ToString(dr["patient_age"]);
                        strField = "patient_weight_lbs"; SqlRecordParams[14] = new SqlParameter("@patient_weight_lbs", SqlDbType.Decimal); SqlRecordParams[14].Value = Convert.ToDecimal(dr["patient_weight_lbs"]);
                        strField = "owner"; SqlRecordParams[15] = new SqlParameter("@owner_name", SqlDbType.NVarChar, 100); SqlRecordParams[15].Value = Convert.ToString(dr["owner"]).Trim();
                        strField = "species"; SqlRecordParams[16] = new SqlParameter("@species", SqlDbType.NVarChar, 30); SqlRecordParams[16].Value = Convert.ToString(dr["species"]).Trim();
                        strField = "breed"; SqlRecordParams[17] = new SqlParameter("@breed", SqlDbType.NVarChar, 50); SqlRecordParams[17].Value = Convert.ToString(dr["breed"]).Trim();
                        strField = "modality"; SqlRecordParams[18] = new SqlParameter("@modality", SqlDbType.NVarChar, 50); SqlRecordParams[18].Value = Convert.ToString(dr["modality"]).Trim();
                        strField = "body_part"; SqlRecordParams[19] = new SqlParameter("@body_part", SqlDbType.NVarChar, 50); SqlRecordParams[19].Value = Convert.ToString(dr["body_part"]).Trim();
                        strField = "manufacturer_model_no"; SqlRecordParams[20] = new SqlParameter("@manufacturer_model_no", SqlDbType.NVarChar, 50); SqlRecordParams[20].Value = Convert.ToString(dr["manufacturer_model_no"]).Trim();
                        strField = "spayed_neutered"; SqlRecordParams[21] = new SqlParameter("@spayed_neutered", SqlDbType.NVarChar, 30); SqlRecordParams[21].Value = Convert.ToString(dr["spayed_neutered"]).Trim();
                        strField = "img_count"; SqlRecordParams[22] = new SqlParameter("@img_count", SqlDbType.Int); SqlRecordParams[22].Value = Convert.ToString(dr["img_count"]);
                        strField = "study_desc"; SqlRecordParams[23] = new SqlParameter("@study_desc", SqlDbType.NVarChar, 500); SqlRecordParams[23].Value = string.Empty;
                        strField = "modality_ae_title"; SqlRecordParams[24] = new SqlParameter("@modality_ae_title", SqlDbType.NVarChar, 500); SqlRecordParams[24].Value = Convert.ToString(dr["modality_ae_title"]).Trim();
                        strField = "priority_id"; SqlRecordParams[25] = new SqlParameter("@priority_id", SqlDbType.Int); SqlRecordParams[25].Value = Convert.ToInt32(dr["priority_id"]); strField = "priority_id";
                        strField = "radiologist_name"; SqlRecordParams[26] = new SqlParameter("@radiologist", SqlDbType.NVarChar, 250); SqlRecordParams[26].Value = Convert.ToString(dr["radiologist_name"]).Trim();
                        strField = "study_status_pacs"; SqlRecordParams[27] = new SqlParameter("@study_status_pacs", SqlDbType.Int); SqlRecordParams[27].Value = Convert.ToInt32(dr["study_status_pacs"]);
                        strField = "study_type_name_1"; SqlRecordParams[28] = new SqlParameter("@study_type_1", SqlDbType.NVarChar, 50); SqlRecordParams[28].Value = Convert.ToString(dr["study_type_name_1"]).Trim();
                        strField = "study_type_name_2"; SqlRecordParams[29] = new SqlParameter("@study_type_2", SqlDbType.NVarChar, 50); SqlRecordParams[29].Value = Convert.ToString(dr["study_type_name_2"]).Trim();
                        strField = "study_type_name_3"; SqlRecordParams[30] = new SqlParameter("@study_type_3", SqlDbType.NVarChar, 50); SqlRecordParams[30].Value = Convert.ToString(dr["study_type_name_3"]).Trim();
                        strField = "study_type_name_4"; SqlRecordParams[31] = new SqlParameter("@study_type_4", SqlDbType.NVarChar, 50); SqlRecordParams[31].Value = Convert.ToString(dr["study_type_name_4"]).Trim();
                        strField = "sales_person"; SqlRecordParams[32] = new SqlParameter("@sales_person", SqlDbType.NVarChar, 100); SqlRecordParams[32].Value = Convert.ToString(dr["sales_person"]).Trim();
                        strField = "patient_weight_kgs"; SqlRecordParams[33] = new SqlParameter("@patient_weight_kgs", SqlDbType.Decimal); SqlRecordParams[33].Value = Convert.ToDecimal(dr["patient_weight_kgs"]);
                        strField = "object_count"; SqlRecordParams[34] = new SqlParameter("@object_count", SqlDbType.Int); if (dr["object_count"] != DBNull.Value) SqlRecordParams[34].Value = Convert.ToInt32(dr["object_count"]); else SqlRecordParams[34].Value = 0;
                        strField = "physician_note"; SqlRecordParams[35] = new SqlParameter("@physician_note", SqlDbType.NVarChar, 2000); SqlRecordParams[35].Value = Convert.ToString(dr["physician_note"]).Trim();
                        strField = "service_codes"; SqlRecordParams[36] = new SqlParameter("@service_codes", SqlDbType.NVarChar, 250); SqlRecordParams[36].Value = Convert.ToString(dr["service_codes"]).Trim();
                        strField = "submit_on"; SqlRecordParams[37] = new SqlParameter("@submit_on", SqlDbType.DateTime); SqlRecordParams[37].Value = Convert.ToDateTime(dr["submit_on"]);
                        SqlRecordParams[38] = new SqlParameter("@error_msg", SqlDbType.VarChar, 500); SqlRecordParams[38].Direction = ParameterDirection.Output;
                        SqlRecordParams[39] = new SqlParameter("@return_type", SqlDbType.Int); SqlRecordParams[39].Direction = ParameterDirection.Output;

                        intExecReturn = DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "scheduler_missing_study_synch_save", SqlRecordParams);

                        intReturnType = Convert.ToInt32(SqlRecordParams[39].Value);
                        if (intReturnType == 0)
                        {
                            ReturnMessage = Convert.ToString(SqlRecordParams[38].Value);
                            CoreCommon.doLog(ConfigPath, intServiceID, strSvcName, "SaveMissingSynchedData() : " + ReturnMessage, true);
                        }
                        else if (intReturnType == 1)
                        {
                            SyncCount = SyncCount + 1;
                        }

                        bReturn = true;
                    }
                    catch (Exception ex)
                    {
                        CoreCommon.doLog(ConfigPath, intServiceID, strSvcName, "Core : SaveMissingSynchedData() :: Study UID : " + strSUID + "Field : " + strField + " - Exception: " + ex.Message.Trim(), true);
                    }

                }


            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message + "-" + (SyncCount + 1).ToString(); }

            return bReturn;
        }
        #endregion
    }
}
