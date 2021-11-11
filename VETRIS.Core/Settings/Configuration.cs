using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VETRIS.Core.Settings
{
    public class Configuration
    {
        #region Variables
        string strUserName = string.Empty;
        Guid userId = new Guid("00000000-0000-0000-0000-000000000000");
        int menuId = 0;
        string controlCode = string.Empty;
        string dataTypeString = string.Empty;
        int dataTypeNumber = 0;
        decimal dataTypeDecimal = 0;
        int groupId = 0;
        string dataType = string.Empty;
        string controlDesc = string.Empty;
        int intServiceID = 0;
        int intRecordID = 0;
        string strIsAfterHrs = string.Empty;
        string strAvailable = string.Empty;
        string strDisplayMsg = string.Empty;
        string strXMLSettings = string.Empty;
        string strXMLSAModality = string.Empty;
        string strXMLSASpecies = string.Empty;
        string strXMLOT = string.Empty;
        string strXMLDASH = string.Empty;
        string strXMLDashAging = string.Empty;
        string strXMLInst = string.Empty;
        string strFilter = string.Empty;
        #endregion

        #region Properties
        public Guid USER_ID
        {
            get { return userId; }
            set { userId = value; }
        }
        public int MENU_ID
        {
            get { return menuId; }
            set { menuId = value; }
        }
        public string CONTROL_CODE
        {
            get { return controlCode; }
            set { controlCode = value; }
        }

        public string DATA_TYPE_STRING
        {
            get { return dataTypeString; }
            set { dataTypeString = value; }
        }

        public int DATA_TYPE_NUMBER
        {
            get { return dataTypeNumber; }
            set { dataTypeNumber = value; }
        }
        public decimal DATA_TYPE_DECIMAL
        {
            get { return dataTypeDecimal; }
            set { dataTypeDecimal = value; }
        }
        public int GROUP_ID
        {
            get { return groupId; }
            set { groupId = value; }
        }
        public string DATA_TYPE
        {
            get { return dataType; }
            set { dataType = value; }
        }
        public string CONTROL_DESC
        {
            get { return controlDesc; }
            set { controlDesc = value; }
        }
        public int SERVICE_ID
        {
            get { return intServiceID; }
            set { intServiceID = value; }
        }
        public int RECORD_ID
        {
            get { return intRecordID; }
            set { intRecordID = value; }
        }
        public string AFTER_HOURS
        {
            get { return strIsAfterHrs; }
            set { strIsAfterHrs = value; }
        }
        public string AVAILABLE
        {
            get { return strAvailable; }
            set { strAvailable = value; }
        }
        public string DISPLAY_MESSAGE
        {
            get { return strDisplayMsg; }
            set { strDisplayMsg = value; }
        }
        public string FILTER_BY
        {
            get { return strFilter; }
            set { strFilter = value; }
        }
        public string USER_NAME
        {
            get { return strUserName; }
            set { strUserName = value; }
        }
        #endregion

        #region ConfigurationBrowserList

        #region ConfigurationBrowserListFetch
        public bool ConfigurationBrowserListFetch(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[4];
            SqlRecordParams[0] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[0].Value = menuId;
            SqlRecordParams[1] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = userId;
            SqlRecordParams[2] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[2].Direction = ParameterDirection.Output;
            SqlRecordParams[3] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[3].Direction = ParameterDirection.Output;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "general_settings_fetch", SqlRecordParams);
                //ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "general_settings_fetch");
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "sys_group";
                    ds.Tables[1].TableName = "sys_settings";

                    foreach (DataRow dr in ds.Tables["sys_settings"].Rows)
                    {
                        if (Convert.ToString(dr["is_password"]) == "Y")
                        {
                            dr["data_type_string"] = CoreCommon.DecryptString(Convert.ToString(dr["data_type_string"]).Trim());
                            ds.Tables["sys_settings"].AcceptChanges();
                        }
                    }

                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }
        #endregion

        #region SaveRecord
        public bool SaveRecord(string ConfigPath, General_Settings[] objSettings, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;

            if (ValidateRecord(objSettings, ref ReturnMessage))
            {
                if ((GenerateSettingsXML(objSettings, ref CatchMessage)))
                {
                    try
                    {
                        SqlParameter[] SqlRecordParams = new SqlParameter[6];
                        SqlRecordParams[0] = new SqlParameter("@xml_general_settings", SqlDbType.NText); SqlRecordParams[0].Value = strXMLSettings.Trim();
                        SqlRecordParams[1] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[1].Value = menuId;
                        SqlRecordParams[2] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = userId;
                        SqlRecordParams[3] = new SqlParameter("@user_name", SqlDbType.NVarChar, 700); SqlRecordParams[3].Direction = ParameterDirection.Output;
                        SqlRecordParams[4] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[4].Direction = ParameterDirection.Output;
                        SqlRecordParams[5] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[5].Direction = ParameterDirection.Output;

                        if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                        intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "general_settings_save", SqlRecordParams);
                        intReturnValue = Convert.ToInt32(SqlRecordParams[5].Value);
                        if (intReturnValue == 1)
                            bReturn = true;
                        else
                            bReturn = false;

                        strUserName = Convert.ToString(SqlRecordParams[3].Value).Trim();
                        ReturnMessage = Convert.ToString(SqlRecordParams[4].Value).Trim();


                    }
                    catch (Exception expErr)
                    { bReturn = false; CatchMessage = expErr.Message; }

                }
            }
            else
            {
                bReturn = false;
            }

            return bReturn;
        }
        #endregion

        #region ValidateRecord
        private bool ValidateRecord(General_Settings[] objSettings, ref string ReturnMessage)
        {
            bool bReturn = true;
            if (objSettings.Length == 0)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "235";
            }
            if (ReturnMessage.Trim() != string.Empty)
                bReturn = false;

            return bReturn;
        }


        #endregion

        #region GenerateSettingsXML
        private bool GenerateSettingsXML(General_Settings[] ArrObj, ref string CatchMessage)
        {
            bool bReturn = false;
            StringBuilder sbXML = new StringBuilder();

            try
            {
                if (ArrObj.Length > 0)
                {

                    sbXML.Append("<general_settings>");

                    for (int i = 0; i < ArrObj.Length; i = i + 1)
                    {
                        sbXML.Append("<row>");
                        sbXML.Append("<control_code><![CDATA[" + ArrObj[i].control_code.ToString() + "]]></control_code>");
                        sbXML.Append("<data_type_number>" + ArrObj[i].data_type_number.ToString() + "</data_type_number>");
                        sbXML.Append("<data_type_string><![CDATA[" + ArrObj[i].data_type_string.ToString() + "]]></data_type_string>");
                        sbXML.Append("<data_type_decimal>" + ArrObj[i].data_type_decimal.ToString() + "</data_type_decimal>");
                        sbXML.Append("<updated_by>" + userId + "</updated_by>");
                        sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                        sbXML.Append("</row>");
                    }
                    sbXML.Append("</general_settings>");
                    strXMLSettings = sbXML.ToString();
                }
                bReturn = true;
            }
            catch (Exception LexpErr)
            {
                bReturn = false;
                CatchMessage = LexpErr.Message;
            }
            return bReturn;
        }
        #endregion

        #endregion

        #region OperationTimeList

        #region OperationTimeListFetch
        public bool OperationTimeListFetch(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[4];
            SqlRecordParams[0] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[0].Value = menuId;
            SqlRecordParams[1] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = userId;
            SqlRecordParams[2] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[2].Direction = ParameterDirection.Output;
            SqlRecordParams[3] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[3].Direction = ParameterDirection.Output;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "operation_time_fetch", SqlRecordParams);
                //ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "general_settings_fetch");
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "operation_time";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }
        #endregion

        #region SaveOperationTime
        public bool SaveOperationTime(string ConfigPath, OperationTime[] ArrObj, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;


            if ((GenerateOperationTimeXML(ArrObj, ref CatchMessage)))
            {
                try
                {
                    SqlParameter[] SqlRecordParams = new SqlParameter[6];
                    SqlRecordParams[0] = new SqlParameter("@xml_data", SqlDbType.NText); SqlRecordParams[0].Value = strXMLOT.Trim();
                    SqlRecordParams[1] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[1].Value = menuId;
                    SqlRecordParams[2] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = userId;
                    SqlRecordParams[3] = new SqlParameter("@user_name", SqlDbType.NVarChar, 700); SqlRecordParams[3].Direction = ParameterDirection.Output;
                    SqlRecordParams[4] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[4].Direction = ParameterDirection.Output;
                    SqlRecordParams[5] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[5].Direction = ParameterDirection.Output;

                    if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                    intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "operation_time_save", SqlRecordParams);
                    intReturnValue = Convert.ToInt32(SqlRecordParams[5].Value);
                    if (intReturnValue == 1)
                        bReturn = true;
                    else
                        bReturn = false;

                    strUserName = Convert.ToString(SqlRecordParams[3].Value).Trim();
                    ReturnMessage = Convert.ToString(SqlRecordParams[4].Value).Trim();


                }
                catch (Exception expErr)
                { bReturn = false; CatchMessage = expErr.Message; }

            }


            return bReturn;
        }
        #endregion

        #region GenerateOperationTimeXML
        private bool GenerateOperationTimeXML(OperationTime[] ArrObj, ref string CatchMessage)
        {
            bool bReturn = false;
            StringBuilder sbXML = new StringBuilder();

            try
            {
                if (ArrObj.Length > 0)
                {

                    sbXML.Append("<OT>");

                    for (int i = 0; i < ArrObj.Length; i = i + 1)
                    {
                        sbXML.Append("<row>");
                        sbXML.Append("<day_no>" + ArrObj[i].day_no.ToString() + "</day_no>");
                        sbXML.Append("<from_time><![CDATA[" + ArrObj[i].from_time + "]]></from_time>");
                        sbXML.Append("<till_time><![CDATA[" + ArrObj[i].till_time + "]]></till_time>");
                        sbXML.Append("<time_zone_id>" + ArrObj[i].time_zone_id.ToString() + "</time_zone_id>");
                        sbXML.Append("<message_display><![CDATA[" + ArrObj[i].message_display + "]]></message_display>");
                        sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                        sbXML.Append("</row>");
                    }
                    sbXML.Append("</OT>");
                    strXMLOT = sbXML.ToString();
                }
                bReturn = true;
            }
            catch (Exception LexpErr)
            {
                bReturn = false;
                CatchMessage = LexpErr.Message;
            }
            return bReturn;
        }
        #endregion

        #endregion

        #region Service Availability List (Normal Hours)

        #region ServiceAvailabilityListFetch
        public bool ServiceAvailabilityListFetch(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[4];
            SqlRecordParams[0] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[0].Value = menuId;
            SqlRecordParams[1] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = userId;
            SqlRecordParams[2] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[2].Direction = ParameterDirection.Output;
            SqlRecordParams[3] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[3].Direction = ParameterDirection.Output;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "settings_service_modality_available_fetch", SqlRecordParams);

                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Service";
                    ds.Tables[1].TableName = "Modality";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }
        #endregion

        #region ServiceAvailabilitySpeciesListFetch
        public bool ServiceAvailabilitySpeciesListFetch(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[4];
            SqlRecordParams[0] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[0].Value = menuId;
            SqlRecordParams[1] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = userId;
            SqlRecordParams[2] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[2].Direction = ParameterDirection.Output;
            SqlRecordParams[3] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[3].Direction = ParameterDirection.Output;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "settings_service_species_available_fetch", SqlRecordParams);

                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Service";
                    ds.Tables[1].TableName = "Species";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }
        #endregion

        #region SaveServiceAvaiability
        public bool SaveServiceAvaiability(string ConfigPath, ServiceModalityAvaiability[] ArrModObj, ServiceSpeciesAvaiability[] ArrSpeciesObj, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;


            if ((GenerateServiceModalityAvaiabilityXML(ArrModObj, ref CatchMessage)) && (GenerateServiceSpeciesAvaiabilityXML(ArrSpeciesObj, ref CatchMessage)))
            {
                try
                {
                    SqlParameter[] SqlRecordParams = new SqlParameter[7];
                    SqlRecordParams[0] = new SqlParameter("@xml_modality", SqlDbType.NText); SqlRecordParams[0].Value = strXMLSAModality.Trim();
                    SqlRecordParams[1] = new SqlParameter("@xml_species", SqlDbType.NText); SqlRecordParams[1].Value = strXMLSASpecies.Trim();
                    SqlRecordParams[2] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[2].Value = menuId;
                    SqlRecordParams[3] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[3].Value = userId;
                    SqlRecordParams[4] = new SqlParameter("@user_name", SqlDbType.NVarChar, 700); SqlRecordParams[4].Direction = ParameterDirection.Output;
                    SqlRecordParams[5] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[5].Direction = ParameterDirection.Output;
                    SqlRecordParams[6] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[6].Direction = ParameterDirection.Output;

                    if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                    intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "settings_service_avaiable_save", SqlRecordParams);
                    intReturnValue = Convert.ToInt32(SqlRecordParams[6].Value);
                    if (intReturnValue == 1)
                        bReturn = true;
                    else
                        bReturn = false;

                    strUserName = Convert.ToString(SqlRecordParams[4].Value).Trim();
                    ReturnMessage = Convert.ToString(SqlRecordParams[5].Value).Trim();


                }
                catch (Exception expErr)
                { bReturn = false; CatchMessage = expErr.Message; }

            }


            return bReturn;
        }
        #endregion

        #endregion

        #region Exception Institutions

        #region ExceptionInstitutionFetch
        public bool ExceptionInstitutionFetch(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[7];
            SqlRecordParams[0] = new SqlParameter("@service_id", SqlDbType.Int); SqlRecordParams[0].Value = intServiceID;
            SqlRecordParams[1] = new SqlParameter("@record_id", SqlDbType.Int); SqlRecordParams[1].Value = intRecordID;
            SqlRecordParams[2] = new SqlParameter("@after_hours", SqlDbType.NChar, 1); SqlRecordParams[2].Value = strIsAfterHrs;
            SqlRecordParams[3] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[3].Value = menuId;
            SqlRecordParams[4] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[4].Value = userId;
            SqlRecordParams[5] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[5].Direction = ParameterDirection.Output;
            SqlRecordParams[6] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[6].Direction = ParameterDirection.Output;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                if(strFilter=="MODALITY") ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "settings_service_modality_exception_institution_fetch", SqlRecordParams);
                else if (strFilter == "SPECIES") ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "settings_service_species_exception_institution_fetch", SqlRecordParams);

                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Institutions";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }
        #endregion

        #region SaveExceptionInstitutions
        public bool SaveExceptionInstitutions(string ConfigPath, ExceptionInstitutions[] ArrObj, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;


            if ((GenerateInstitutionXML(ArrObj, ref CatchMessage)))
            {
                try
                {
                    SqlParameter[] SqlRecordParams = new SqlParameter[11];
                    SqlRecordParams[0] = new SqlParameter("@service_id", SqlDbType.Int); SqlRecordParams[0].Value = intServiceID;
                    SqlRecordParams[1] = new SqlParameter("@record_id", SqlDbType.Int); SqlRecordParams[1].Value = intRecordID;
                    SqlRecordParams[2] = new SqlParameter("@after_hours", SqlDbType.NChar, 1); SqlRecordParams[2].Value = strIsAfterHrs;
                    SqlRecordParams[3] = new SqlParameter("@available", SqlDbType.NChar, 1); SqlRecordParams[3].Value = strAvailable;
                    SqlRecordParams[4] = new SqlParameter("@message_display", SqlDbType.NVarChar, 500); SqlRecordParams[4].Value = strDisplayMsg;
                    SqlRecordParams[5] = new SqlParameter("@xml_data", SqlDbType.NText); SqlRecordParams[5].Value = strXMLInst.Trim();
                    SqlRecordParams[6] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[6].Value = menuId;
                    SqlRecordParams[7] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[7].Value = userId;
                    SqlRecordParams[8] = new SqlParameter("@user_name", SqlDbType.NVarChar, 700); SqlRecordParams[8].Direction = ParameterDirection.Output;
                    SqlRecordParams[9] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[9].Direction = ParameterDirection.Output;
                    SqlRecordParams[10] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[10].Direction = ParameterDirection.Output;

                    if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                    if(strFilter=="MODALITY")
                       intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "settings_service_modality_exception_institution_save", SqlRecordParams);
                    else if (strFilter == "SPECIES")
                        intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "settings_service_species_exception_institution_save", SqlRecordParams);

                    intReturnValue = Convert.ToInt32(SqlRecordParams[10].Value);
                    if (intReturnValue == 1)
                        bReturn = true;
                    else
                        bReturn = false;

                    strUserName = Convert.ToString(SqlRecordParams[8].Value).Trim();
                    ReturnMessage = Convert.ToString(SqlRecordParams[9].Value).Trim();


                }
                catch (Exception expErr)
                { bReturn = false; CatchMessage = expErr.Message; }

            }


            return bReturn;
        }
        #endregion

        #region GenerateInstitutionXML
        private bool GenerateInstitutionXML(ExceptionInstitutions[] ArrObj, ref string CatchMessage)
        {
            bool bReturn = false;
            StringBuilder sbXML = new StringBuilder();

            try
            {
                if (ArrObj.Length > 0)
                {

                    sbXML.Append("<institution>");

                    for (int i = 0; i < ArrObj.Length; i = i + 1)
                    {
                        sbXML.Append("<row>");
                        sbXML.Append("<id><![CDATA[" + Convert.ToString(ArrObj[i].id) + "]]></id>");
                        sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                        sbXML.Append("</row>");
                    }
                    sbXML.Append("</institution>");
                    strXMLInst = sbXML.ToString();
                }
                bReturn = true;
            }
            catch (Exception LexpErr)
            {
                bReturn = false;
                CatchMessage = LexpErr.Message;
            }
            return bReturn;
        }
        #endregion

        #endregion

        #region GenerateServiceModalityAvaiabilityXML
        private bool GenerateServiceModalityAvaiabilityXML(ServiceModalityAvaiability[] ArrObj, ref string CatchMessage)
        {
            bool bReturn = false;
            StringBuilder sbXML = new StringBuilder();

            try
            {
                if (ArrObj.Length > 0)
                {

                    sbXML.Append("<service>");

                    for (int i = 0; i < ArrObj.Length; i = i + 1)
                    {
                        sbXML.Append("<row>");
                        sbXML.Append("<service_id>" + ArrObj[i].service_id.ToString() + "</service_id>");
                        sbXML.Append("<modality_id>" + ArrObj[i].modality_id.ToString() + "</modality_id>");
                        sbXML.Append("<available><![CDATA[" + ArrObj[i].avaiable + "]]></available>");
                        sbXML.Append("<message_display><![CDATA[" + ArrObj[i].message_display + "]]></message_display>");
                        sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                        sbXML.Append("</row>");
                    }
                    sbXML.Append("</service>");
                    strXMLSAModality = sbXML.ToString();
                }
                bReturn = true;
            }
            catch (Exception LexpErr)
            {
                bReturn = false;
                CatchMessage = LexpErr.Message;
            }
            return bReturn;
        }
        #endregion

        #region GenerateServiceSpeciesAvaiabilityXML
        private bool GenerateServiceSpeciesAvaiabilityXML(ServiceSpeciesAvaiability[] ArrObj, ref string CatchMessage)
        {
            bool bReturn = false;
            StringBuilder sbXML = new StringBuilder();

            try
            {
                if (ArrObj.Length > 0)
                {

                    sbXML.Append("<species>");

                    for (int i = 0; i < ArrObj.Length; i = i + 1)
                    {
                        sbXML.Append("<row>");
                        sbXML.Append("<service_id>" + ArrObj[i].service_id.ToString() + "</service_id>");
                        sbXML.Append("<species_id>" + ArrObj[i].species_id.ToString() + "</species_id>");
                        sbXML.Append("<available><![CDATA[" + ArrObj[i].avaiable + "]]></available>");
                        sbXML.Append("<message_display><![CDATA[" + ArrObj[i].message_display + "]]></message_display>");
                        sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                        sbXML.Append("</row>");
                    }
                    sbXML.Append("</species>");
                    strXMLSASpecies = sbXML.ToString();
                }
                bReturn = true;
            }
            catch (Exception LexpErr)
            {
                bReturn = false;
                CatchMessage = LexpErr.Message;
            }
            return bReturn;
        }
        #endregion

        #region Service Availability List (After Hours)

        #region ServiceAvailabilityAfterHoursListFetch
        public bool ServiceAvailabilityAfterHoursListFetch(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[4];
            SqlRecordParams[0] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[0].Value = menuId;
            SqlRecordParams[1] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = userId;
            SqlRecordParams[2] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[2].Direction = ParameterDirection.Output;
            SqlRecordParams[3] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[3].Direction = ParameterDirection.Output;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "settings_service_modality_available_after_hours_fetch", SqlRecordParams);
                //ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "general_settings_fetch");
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Service";
                    ds.Tables[1].TableName = "Modality";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }
        #endregion

        #region ServiceAvailabilitySpeciesAfterHoursListFetch
        public bool ServiceAvailabilitySpeciesAfterHoursListFetch(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[4];
            SqlRecordParams[0] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[0].Value = menuId;
            SqlRecordParams[1] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = userId;
            SqlRecordParams[2] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[2].Direction = ParameterDirection.Output;
            SqlRecordParams[3] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[3].Direction = ParameterDirection.Output;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "settings_service_species_available_after_hours_fetch", SqlRecordParams);
                //ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "general_settings_fetch");
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Service";
                    ds.Tables[1].TableName = "Species";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }
        #endregion

        #region SaveServiceModalityAvaiabilityAfterHours
        public bool SaveServiceModalityAvaiabilityAfterHours(string ConfigPath, ServiceModalityAvaiability[] ArrModObj, ServiceSpeciesAvaiability[] ArrSpeciesObj, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;


            if ((GenerateServiceModalityAvaiabilityXML(ArrModObj, ref CatchMessage)) && (GenerateServiceSpeciesAvaiabilityXML(ArrSpeciesObj, ref CatchMessage)))
            {
                try
                {
                    SqlParameter[] SqlRecordParams = new SqlParameter[7];
                    SqlRecordParams[0] = new SqlParameter("@xml_modality", SqlDbType.NText); SqlRecordParams[0].Value = strXMLSAModality.Trim();
                    SqlRecordParams[1] = new SqlParameter("@xml_species", SqlDbType.NText); SqlRecordParams[1].Value = strXMLSASpecies.Trim();
                    SqlRecordParams[2] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[2].Value = menuId;
                    SqlRecordParams[3] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[3].Value = userId;
                    SqlRecordParams[4] = new SqlParameter("@user_name", SqlDbType.NVarChar, 700); SqlRecordParams[4].Direction = ParameterDirection.Output;
                    SqlRecordParams[5] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[5].Direction = ParameterDirection.Output;
                    SqlRecordParams[6] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[6].Direction = ParameterDirection.Output;

                    if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                    intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "settings_service_available_after_hours_save", SqlRecordParams);
                    intReturnValue = Convert.ToInt32(SqlRecordParams[6].Value);
                    if (intReturnValue == 1)
                        bReturn = true;
                    else
                        bReturn = false;

                    strUserName = Convert.ToString(SqlRecordParams[4].Value).Trim();
                    ReturnMessage = Convert.ToString(SqlRecordParams[5].Value).Trim();


                }
                catch (Exception expErr)
                { bReturn = false; CatchMessage = expErr.Message; }

            }


            return bReturn;
        }
        #endregion

        #endregion

        #region DashboardSettingsList

        #region DashboardSettingsListFetch
        public bool DashboardSettingsListFetch(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[4];
            SqlRecordParams[0] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[0].Value = menuId;
            SqlRecordParams[1] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = userId;
            SqlRecordParams[2] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[2].Direction = ParameterDirection.Output;
            SqlRecordParams[3] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[3].Direction = ParameterDirection.Output;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "dashboard_settings_fetch", SqlRecordParams);
                //ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "general_settings_fetch");
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "parent_dashboard";
                    ds.Tables[1].TableName = "dashboard_settings";
                    ds.Tables[2].TableName = "dashboard_settings_aging";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }
        #endregion

        #region SaveDashboardSettings
        public bool SaveDashboardSettings(string ConfigPath, DashboardSettings[] ArrObj, DashboardSettingsAging[] agingObj, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;


            if ((GenerateDashboardSettingsXML(ArrObj, ref CatchMessage) && GenerateDashboardSettingsAgingXML(agingObj, ref CatchMessage)))
            {
                try
                {
                    SqlParameter[] SqlRecordParams = new SqlParameter[7];
                    SqlRecordParams[0] = new SqlParameter("@xml_data", SqlDbType.NText); SqlRecordParams[0].Value = strXMLDASH.Trim();
                    SqlRecordParams[1] = new SqlParameter("@xml_data_aging", SqlDbType.NText); SqlRecordParams[1].Value = strXMLDashAging.Trim();
                    SqlRecordParams[2] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[2].Value = menuId;
                    SqlRecordParams[3] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[3].Value = userId;
                    SqlRecordParams[4] = new SqlParameter("@user_name", SqlDbType.NVarChar, 700); SqlRecordParams[4].Direction = ParameterDirection.Output;
                    SqlRecordParams[5] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[5].Direction = ParameterDirection.Output;
                    SqlRecordParams[6] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[6].Direction = ParameterDirection.Output;

                    if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                    intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "dashboard_settings_save", SqlRecordParams);
                    intReturnValue = Convert.ToInt32(SqlRecordParams[6].Value);
                    if (intReturnValue == 1)
                        bReturn = true;
                    else
                        bReturn = false;

                    strUserName = Convert.ToString(SqlRecordParams[4].Value).Trim();
                    ReturnMessage = Convert.ToString(SqlRecordParams[5].Value).Trim();


                }
                catch (Exception expErr)
                { bReturn = false; CatchMessage = expErr.Message; }

            }


            return bReturn;
        }
        #endregion

        #region GenerateDashboardSettingsXML
        private bool GenerateDashboardSettingsXML(DashboardSettings[] ArrObj, ref string CatchMessage)
        {
            bool bReturn = false;
            StringBuilder sbXML = new StringBuilder();

            try
            {
                if (ArrObj.Length > 0)
                {

                    sbXML.Append("<dashboard_settings>");

                    for (int i = 0; i < ArrObj.Length; i = i + 1)
                    {
                        sbXML.Append("<row>");
                        sbXML.Append("<id>" + ArrObj[i].id.ToString() + "</id>");
                        sbXML.Append("<parent_id>" + ArrObj[i].parent_id.ToString() + "</parent_id>");
                        sbXML.Append("<menu_desc>" + ArrObj[i].menu_desc + "</menu_desc>");
                        sbXML.Append("<nav_url>" + ArrObj[i].nav_url + "</nav_url>");
                        sbXML.Append("<icon>" + ArrObj[i].icon + "</icon>");
                        sbXML.Append("<display_index>" + ArrObj[i].display_index.ToString() + "</display_index>");
                        sbXML.Append("<refresh_time>" + ArrObj[i].refresh_time.ToString() + "</refresh_time>");
                        sbXML.Append("<is_enabled>" + ArrObj[i].is_enabled + "</is_enabled>");
                        sbXML.Append("<is_default>" + ArrObj[i].is_default + "</is_default>");
                        sbXML.Append("<is_refresh_button>" + ArrObj[i].is_refresh_button + "</is_refresh_button>");
                        sbXML.Append("<title>" + ArrObj[i].title + "</title>");
                        sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                        sbXML.Append("</row>");
                    }
                    sbXML.Append("</dashboard_settings>");
                    strXMLDASH = sbXML.ToString();
                }
                bReturn = true;
            }
            catch (Exception LexpErr)
            {
                bReturn = false;
                CatchMessage = LexpErr.Message;
            }
            return bReturn;
        }
        #endregion

        #region GenerateDashboardSettingsAgingXML
        private bool GenerateDashboardSettingsAgingXML(DashboardSettingsAging[] ArrObj, ref string CatchMessage)
        {
            bool bReturn = false;
            StringBuilder sbXML = new StringBuilder();

            try
            {
                if (ArrObj.Length > 0)
                {

                    sbXML.Append("<dashboard_settings_aging>");

                    for (int i = 0; i < ArrObj.Length; i = i + 1)
                    {
                        sbXML.Append("<row>");
                        sbXML.Append("<id>" + ArrObj[i].id.ToString() + "</id>");
                        sbXML.Append("<dashboard_menu_id>" + ArrObj[i].dashboard_menu_id.ToString() + "</dashboard_menu_id>");
                        sbXML.Append("<key>" + ArrObj[i].key + "</key>");
                        sbXML.Append("<slot_count>" + ArrObj[i].slot_count + "</slot_count>");
                        sbXML.Append("<slot_1>" + ArrObj[i].slot_1.ToString() + "</slot_1>");
                        sbXML.Append("<slot_2>" + ArrObj[i].slot_2.ToString() + "</slot_2>");
                        sbXML.Append("<slot_3>" + ArrObj[i].slot_3.ToString() + "</slot_3>");
                        sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                        sbXML.Append("</row>");
                    }
                    sbXML.Append("</dashboard_settings_aging>");
                    strXMLDashAging = sbXML.ToString();
                }
                bReturn = true;
            }
            catch (Exception LexpErr)
            {
                bReturn = false;
                CatchMessage = LexpErr.Message;
            }
            return bReturn;
        }
        #endregion
        #endregion
    }

    public class General_Settings
    {
        public string control_code { get; set; }
        public int data_type_number { get; set; }
        public string data_type_string { get; set; }
        public decimal data_type_decimal { get; set; }
        public Guid updated_by { get; set; }
        public int group_id { get; set; }
        public string data_type { get; set; }
        public string control_desc { get; set; }
    }
    public class OperationTime
    {
        public int day_no { get; set; }
        public string from_time { get; set; }
        public string till_time { get; set; }
        public int time_zone_id { get; set; }
        public string message_display { get; set; }
    }
    public class ServiceModalityAvaiability
    {
        public int service_id { get; set; }
        public int modality_id { get; set; }
        public string avaiable { get; set; }
        public string message_display { get; set; }
    }
    public class ServiceSpeciesAvaiability
    {
        public int service_id { get; set; }
        public int species_id { get; set; }
        public string avaiable { get; set; }
        public string message_display { get; set; }
    }
    public class ExceptionInstitutions
    {
        public Guid id { get; set; }
    }
    public class DashboardSettings
    {
        public int id { get; set; }
        public int parent_id { get; set; }
        public string menu_desc { get; set; }
        public string nav_url { get; set; }
        public string icon { get; set; }
        public int display_index { get; set; }
        public string is_enabled { get; set; }
        public string is_default { get; set; }
        public int? refresh_time { get; set; }
        public string is_refresh_button { get; set; }
        public string title { get; set; }
    }
    public class DashboardSettingsAging
    {
        public int id { get; set; }
        public int dashboard_menu_id { get; set; }
        public string key { get; set; }
        public int slot_count { get; set; }
        public int slot_1 { get; set; }
        public int slot_2 { get; set; }
        public int? slot_3 { get; set; }
        public int? slot_4 { get; set; }
    }
}
