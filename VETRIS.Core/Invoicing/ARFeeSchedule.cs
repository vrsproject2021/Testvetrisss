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
    public class ARFeeSchedule
    {
        #region Constructor
        public ARFeeSchedule()
        {
        }
        #endregion

        #region Variables
        int intMenuID = 0;
        Guid UserID = new Guid("00000000-0000-0000-0000-000000000000");
        int intUserRoleID = 0;
        string strUserName = string.Empty;

        Guid Id = new Guid("00000000-0000-0000-0000-000000000000");
        int intCategoryID = 0;
        int intModalityID = 0;
        int intServiceID = 0;
        string strInvoiceBy = string.Empty;
        int intDefCountFrom = 0;
        int intDefCountTo = 0;
        double dblFee = 0;
        double dblFeePerUnit = 0;
        double dblStudyMaxFee = 0;
        double dblFeeAfterHr = 0;
        string strGLCode = string.Empty;
        int intRowID = 0;
        string strType = string.Empty;
        string strXML = string.Empty;
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

        public int CATEGORY_ID
        {
            get { return intCategoryID; }
            set { intCategoryID = value; }
        }
        public int MODALITY_ID
        {
            get { return intModalityID; }
            set { intModalityID = value; }
        }
        public int SERVICE_ID
        {
            get { return intServiceID; }
            set { intServiceID = value; }
        }
        public string INVOICE_BY
        {
            get { return strInvoiceBy; }
            set { strInvoiceBy = value; }
        }
        public int DEFAULT_COUNT_FROM
        {
            get { return intDefCountFrom; }
            set { intDefCountFrom = value; }
        }
        public int DEFAULT_COUNT_TO
        {
            get { return intDefCountTo; }
            set { intDefCountTo = value; }
        }
        public double FEE_AMOUNT
        {
            get { return dblFee; }
            set { dblFee = value; }
        }
        public double FEE_AMOUNT_PER_UNIT
        {
            get { return dblFeePerUnit; }
            set { dblFeePerUnit = value; }
        }
        public double STUDY_MAXIMUM_FEE_AMOUNT
        {
            get { return dblStudyMaxFee; }
            set { dblStudyMaxFee = value; }
        }
        public double FEE_AMOUNT_AFTER_HOUR
        {
            get { return dblFeeAfterHr; }
            set { dblFeeAfterHr = value; }
        }
        public string GL_CODE
        {
            get { return strGLCode; }
            set { strGLCode = value; }
        }
        public int ROW_ID
        {
            get { return intRowID; }
            set { intRowID = value; }
        }
        public string TYPE
        {
            get { return strType; }
            set { strType = value; }
        }
        #endregion

        #region FetchParameters
        public bool FetchParameters(string ConfigPath, ref DataSet ds, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[4];
            SqlRecordParams[0] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[0].Value = intMenuID;
            SqlRecordParams[1] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = UserID;
            SqlRecordParams[2] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[2].Direction = ParameterDirection.Output;
            SqlRecordParams[3] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[3].Direction = ParameterDirection.Output;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "ar_fee_shcedule_params_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Category";
                    ds.Tables[1].TableName = "Modality";
                    ds.Tables[2].TableName = "Services";
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

        #region LoadModalityDetails
        public bool LoadModalityDetails(string ConfigPath, ref DataSet ds,  ref string CatchMessage)
        {
            bool bReturn = false;
            


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING,CommandType.StoredProcedure, "ar_fee_schedule_modality_fetch");
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "ModalityFeeList";
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

        #region LoadServiceDetails
        public bool LoadServiceDetails(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;



            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "ar_fee_schedule_service_fetch");
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "ServiceFeeList";
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

        #region SaveModalityRecord
        public bool SaveModalityRecord(string ConfigPath, ARFeeSchedule[] ArrObj, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0; int intRowID = 0;
            string[] arrTempFiles = new string[0];
            string strFile = string.Empty;

            if (ValidateModalityRecord(ArrObj, ref ReturnMessage, ref intRowID))
            {
                if (GenerateModalityXML(ArrObj, ref CatchMessage))
                {
                    try
                    {
                        SqlParameter[] SqlRecordParams = new SqlParameter[6];
                        SqlRecordParams[0] = new SqlParameter("@xml_fees", SqlDbType.NText); SqlRecordParams[0].Value = strXML.Trim();
                        SqlRecordParams[1] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = UserID;
                        SqlRecordParams[2] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[2].Value = intMenuID;
                        SqlRecordParams[3] = new SqlParameter("@user_name", SqlDbType.NVarChar, 500); SqlRecordParams[3].Direction = ParameterDirection.Output;
                        SqlRecordParams[4] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[4].Direction = ParameterDirection.Output;
                        SqlRecordParams[5] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[5].Direction = ParameterDirection.Output;


                        if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                        intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "ar_fee_schedule_modality_save", SqlRecordParams);
                        intReturnValue = Convert.ToInt32(SqlRecordParams[5].Value);
                        if (intReturnValue == 1)
                            bReturn = true;
                        else
                            bReturn = false;

                        strUserName = Convert.ToString(SqlRecordParams[3].Value).Trim();
                        ReturnMessage = Convert.ToString(SqlRecordParams[4].Value);
                    }
                    catch (Exception expErr)
                    { bReturn = false; CatchMessage = expErr.Message; }
                }
                else
                {
                    bReturn = false;

                }

            }
            else
            {
                bReturn = false;
                strUserName = intRowID.ToString();
            }

            return bReturn;
        }
        #endregion

        #region SaveServiceRecord
        public bool SaveServiceRecord(string ConfigPath, ARFeeSchedule[] ArrObj, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0; int intRowID = 0;
            string[] arrTempFiles = new string[0];
            string strFile = string.Empty;

            if (ValidateServiceRecord(ArrObj, ref ReturnMessage, ref intRowID))
            {
                if (GenerateServiceXML(ArrObj, ref CatchMessage))
                {
                    try
                    {
                        SqlParameter[] SqlRecordParams = new SqlParameter[6];
                        SqlRecordParams[0] = new SqlParameter("@xml_fees", SqlDbType.NText); SqlRecordParams[0].Value = strXML.Trim();
                        SqlRecordParams[1] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = UserID;
                        SqlRecordParams[2] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[2].Value = intMenuID;
                        SqlRecordParams[3] = new SqlParameter("@user_name", SqlDbType.NVarChar, 500); SqlRecordParams[3].Direction = ParameterDirection.Output;
                        SqlRecordParams[4] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[4].Direction = ParameterDirection.Output;
                        SqlRecordParams[5] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[5].Direction = ParameterDirection.Output;


                        if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                        intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "ar_fee_schedule_service_save", SqlRecordParams);
                        intReturnValue = Convert.ToInt32(SqlRecordParams[5].Value);
                        if (intReturnValue == 1)
                            bReturn = true;
                        else
                            bReturn = false;

                        strUserName = Convert.ToString(SqlRecordParams[3].Value).Trim();
                        ReturnMessage = Convert.ToString(SqlRecordParams[4].Value);
                    }
                    catch (Exception expErr)
                    { bReturn = false; CatchMessage = expErr.Message; }
                }
                else
                {
                    bReturn = false;

                }

            }
            else
            {
                bReturn = false;
                strUserName = intRowID.ToString();
            }

            return bReturn;
        }
        #endregion

        #region DeleteRecord
        public bool DeleteRecord(string ConfigPath, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;



            try
            {
                SqlParameter[] SqlRecordParams = new SqlParameter[7];
                SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
                SqlRecordParams[1] = new SqlParameter("@type", SqlDbType.NVarChar, 500); SqlRecordParams[1].Value = strType;
                SqlRecordParams[2] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = UserID;
                SqlRecordParams[3] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[3].Value = intMenuID;
                SqlRecordParams[4] = new SqlParameter("@user_name", SqlDbType.NVarChar, 500); SqlRecordParams[4].Direction = ParameterDirection.Output;
                SqlRecordParams[5] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[5].Direction = ParameterDirection.Output;
                SqlRecordParams[6] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[6].Direction = ParameterDirection.Output;


                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "ar_fee_schedule_delete", SqlRecordParams);
                intReturnValue = Convert.ToInt32(SqlRecordParams[6].Value);
                if (intReturnValue == 1)
                    bReturn = true;
                else
                    bReturn = false;

                strUserName = Convert.ToString(SqlRecordParams[4].Value).Trim();
                ReturnMessage = Convert.ToString(SqlRecordParams[5].Value);
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }





            return bReturn;
        }
        #endregion

        #region ValidateModalityRecord
        private bool ValidateModalityRecord(ARFeeSchedule[] ArrObj, ref string ReturnMessage, ref int RowID)
        {
            bool bReturn = true;
            if (ArrObj.Length == 0)
            {
                ReturnMessage = "067";
            }
            else
            {
                for (int i = 0; i < ArrObj.Length; i++)
                {
                    if (ArrObj[i].CATEGORY_ID == 0)
                    {
                        ReturnMessage = "292";
                        RowID = ArrObj[i].ROW_ID;
                        break;
                    }
                    if (ArrObj[i].MODALITY_ID == 0)
                    {
                        if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                        ReturnMessage = "255";
                        RowID = ArrObj[i].ROW_ID;
                        break;
                    }

                    if (ArrObj[i].DEFAULT_COUNT_FROM > ArrObj[i].DEFAULT_COUNT_TO)
                    {
                        if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                        if (ArrObj[i].INVOICE_BY == "B") ReturnMessage = "461";
                        else if (ArrObj[i].INVOICE_BY == "I") ReturnMessage = "133";
                        else if (ArrObj[i].INVOICE_BY == "M") ReturnMessage = "453";
                        RowID = ArrObj[i].ROW_ID;
                        break;
                    }
                    //if (ArrObj[i].FEE_AMOUNT <= 0)
                    //{
                    //    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                    //    ReturnMessage = "134";
                    //    RowID = ArrObj[i].ROW_ID;
                    //    break;
                    //}
                }
            }


            if (ReturnMessage.Trim() != string.Empty)
                bReturn = false;

            return bReturn;
        }
        #endregion

        #region GenerateModalityXML
        private bool GenerateModalityXML(ARFeeSchedule[] ArrObj, ref string CatchMessage)
        {
            bool bReturn = false;
            StringBuilder sbXML = new StringBuilder();

            try
            {
                if (ArrObj.Length > 0)
                {

                    sbXML.Append("<fees>");

                    for (int i = 0; i < ArrObj.Length; i = i + 1)
                    {
                        sbXML.Append("<row>");
                        sbXML.Append("<id>" + ArrObj[i].ID.ToString() + "</id>");
                        sbXML.Append("<category_id>" + ArrObj[i].CATEGORY_ID.ToString() + "</category_id>");
                        sbXML.Append("<modality_id>" + ArrObj[i].MODALITY_ID.ToString() + "</modality_id>");
                        sbXML.Append("<invoice_by><![CDATA[" + ArrObj[i].INVOICE_BY + "]]></invoice_by>");
                        sbXML.Append("<default_count_from>" + ArrObj[i].DEFAULT_COUNT_FROM.ToString() + "</default_count_from>");
                        sbXML.Append("<default_count_to>" + ArrObj[i].DEFAULT_COUNT_TO.ToString() + "</default_count_to>");
                        sbXML.Append("<fee_amount>" + ArrObj[i].FEE_AMOUNT.ToString() + "</fee_amount>");
                        sbXML.Append("<fee_amount_per_unit>" + ArrObj[i].FEE_AMOUNT_PER_UNIT.ToString() + "</fee_amount_per_unit>");
                        sbXML.Append("<study_max_amount>" + ArrObj[i].STUDY_MAXIMUM_FEE_AMOUNT.ToString() + "</study_max_amount>");
                        sbXML.Append("<gl_code><![CDATA[" + ArrObj[i].GL_CODE + "]]></gl_code>");
                        sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                        sbXML.Append("</row>");
                    }

                    sbXML.Append("</fees>");
                    strXML = sbXML.ToString();


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

        #region ValidateServiceRecord
        private bool ValidateServiceRecord(ARFeeSchedule[] ArrObj, ref string ReturnMessage, ref int RowID)
        {
            bool bReturn = true;
            if (ArrObj.Length == 0)
            {
                ReturnMessage = "067";
            }
            else
            {
                for (int i = 0; i < ArrObj.Length; i++)
                {
                    if (ArrObj[i].SERVICE_ID == 0)
                    {
                        ReturnMessage = "454";
                        RowID = ArrObj[i].ROW_ID;
                        break;
                    }
                    if (ArrObj[i].DEFAULT_COUNT_FROM > ArrObj[i].DEFAULT_COUNT_TO)
                    {
                        if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                        if (ArrObj[i].INVOICE_BY == "B") ReturnMessage = "461";
                        else if (ArrObj[i].INVOICE_BY == "I") ReturnMessage = "133";
                        else if (ArrObj[i].INVOICE_BY == "M") ReturnMessage = "453";
                        RowID = ArrObj[i].ROW_ID;
                        break;
                    }
                    //if (ArrObj[i].FEE_AMOUNT <= 0)
                    //{
                    //    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                    //    ReturnMessage = "134";
                    //    RowID = ArrObj[i].ROW_ID;
                    //    break;
                    //}
                }
            }


            if (ReturnMessage.Trim() != string.Empty)
                bReturn = false;

            return bReturn;
        }
        #endregion

        #region GenerateServiceXML
        private bool GenerateServiceXML(ARFeeSchedule[] ArrObj, ref string CatchMessage)
        {
            bool bReturn = false;
            StringBuilder sbXML = new StringBuilder();

            try
            {
                if (ArrObj.Length > 0)
                {

                    sbXML.Append("<fees>");

                    for (int i = 0; i < ArrObj.Length; i = i + 1)
                    {
                        sbXML.Append("<row>");
                        sbXML.Append("<id>" + ArrObj[i].ID.ToString() + "</id>");
                        sbXML.Append("<service_id>" + ArrObj[i].SERVICE_ID.ToString() + "</service_id>");
                        sbXML.Append("<modality_id>" + ArrObj[i].MODALITY_ID.ToString() + "</modality_id>");
                        sbXML.Append("<invoice_by><![CDATA[" + ArrObj[i].INVOICE_BY + "]]></invoice_by>");
                        sbXML.Append("<default_count_from>" + ArrObj[i].DEFAULT_COUNT_FROM.ToString() + "</default_count_from>");
                        sbXML.Append("<default_count_to>" + ArrObj[i].DEFAULT_COUNT_TO.ToString() + "</default_count_to>");
                        sbXML.Append("<fee_amount>" + ArrObj[i].FEE_AMOUNT.ToString() + "</fee_amount>");
                        sbXML.Append("<fee_amount_after_hrs>" + ArrObj[i].FEE_AMOUNT_AFTER_HOUR.ToString() + "</fee_amount_after_hrs>");
                        sbXML.Append("<gl_code><![CDATA[" + ArrObj[i].GL_CODE + "]]></gl_code>");
                        sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                        sbXML.Append("</row>");
                    }

                    sbXML.Append("</fees>");
                    strXML = sbXML.ToString();


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
    }
}
