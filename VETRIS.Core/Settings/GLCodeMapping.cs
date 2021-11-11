using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace VETRIS.Core.Settings
{
    public class GLCodeMapping
    {
        #region Constructor
        public GLCodeMapping()
        {
        }
        #endregion

        #region Variables
        int intUserRoleID = 0;
        int intMenuID = 0;
        Guid UserId = new Guid("00000000-0000-0000-0000-000000000000");
        int intID = 0;
        string strXMLModality = string.Empty;
        string strXMLService = string.Empty;
        string strXMLNRH = string.Empty;
        string strXMLRC = string.Empty;
        string strUserName = string.Empty;
        #endregion

        #region Properties
        public int USER_ROLE_ID
        {
            get { return intUserRoleID; }
            set { intUserRoleID = value; }
        }
        public int MENU_ID
        {
            get { return intMenuID; }
            set { intMenuID = value; }
        }
        public int ID
        {
            get { return intID; }
            set { intID = value; }
        }
        public Guid USER_ID
        {
            get { return UserId; }
            set { UserId = value; }
        }
        public string USER_NAME
        {
            get { return strUserName; }
            set { strUserName = value; }
        }
        #endregion

        #region FetchGlMapping
        public bool FetchGlMapping(string ConfigPath, ref DataSet ds, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;

            SqlParameter[] SqlRecordParams = new SqlParameter[4];
            SqlRecordParams[0] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[0].Value = intMenuID;
            SqlRecordParams[1] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = UserId;
            SqlRecordParams[2] = new SqlParameter("@error_code", SqlDbType.VarChar, 10); SqlRecordParams[2].Direction = ParameterDirection.Output;
            SqlRecordParams[3] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[3].Direction = ParameterDirection.Output;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "settings_gl_code_map_fetch", SqlRecordParams);
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
                    ReturnMessage = Convert.ToString(SqlRecordParams[2].Value);
                }

            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region FetchModalities
        public bool FetchModalities(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "settings_gl_code_map_modality_category_fetch");
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Modality";
                    bReturn = true;
                }

            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region FetchServices
        public bool FetchServices(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "settings_gl_code_map_services_fetch");
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Service";
                    bReturn = true;
                }

            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region FetchNonRevenueHead
        public bool FetchNonRevenueHead(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "settings_gl_code_map_non_revenue_fetch");
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "NonRevenueHead";
                    bReturn = true;
                }

            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region FetchRadiologistCharges
        public bool FetchRadiologistCharges(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "settings_gl_code_map_radiologist_charge_fetch");
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "RadiologistCharge";
                    bReturn = true;
                }

            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region SaveRecord
        public bool SaveRecord(string ConfigPath, GLModalityList[] ArrModalityObj, GLServiceList[] ArrServiceObj, GLNonRevenueHead[] ArrNRHObj, GLRadiologistCharges[] ArrRCObj, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false; int intReturnValue = 0; int intExecReturn = 0;


            if (GenerateModalityXML(ArrModalityObj, ref CatchMessage) && GenerateServiceXML(ArrServiceObj, ref CatchMessage) && GenerateNonRevenueXML(ArrNRHObj, ref CatchMessage) && GenerateRadiologistChargeXML(ArrRCObj, ref CatchMessage))
            {
                SqlParameter[] SqlRecordParams = new SqlParameter[9];
                SqlRecordParams[0] = new SqlParameter("@xml_modality", SqlDbType.NText); if (strXMLModality.Trim() != string.Empty) SqlRecordParams[0].Value = strXMLModality; else SqlRecordParams[0].Value = DBNull.Value;
                SqlRecordParams[1] = new SqlParameter("@xml_service", SqlDbType.NText); if (strXMLService.Trim() != string.Empty) SqlRecordParams[1].Value = strXMLService; else SqlRecordParams[1].Value = DBNull.Value;
                SqlRecordParams[2] = new SqlParameter("@xml_nonrevenue_head", SqlDbType.NText); if (strXMLNRH.Trim() != string.Empty) SqlRecordParams[2].Value = strXMLNRH; else SqlRecordParams[2].Value = DBNull.Value;
                SqlRecordParams[3] = new SqlParameter("@xml_rad_charge", SqlDbType.NText); if (strXMLRC.Trim() != string.Empty) SqlRecordParams[3].Value = strXMLRC; else SqlRecordParams[3].Value = DBNull.Value;
                SqlRecordParams[4] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[4].Value = UserId;
                SqlRecordParams[5] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[5].Value = intMenuID;
                SqlRecordParams[6] = new SqlParameter("@user_name", SqlDbType.NVarChar, 30); SqlRecordParams[6].Direction = ParameterDirection.Output;
                SqlRecordParams[7] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[7].Direction = ParameterDirection.Output;
                SqlRecordParams[8] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[8].Direction = ParameterDirection.Output;

                try
                {

                    if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                    intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "settings_gl_code_map_save", SqlRecordParams);
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
                bReturn = false;


            return bReturn;
        }

        #endregion

        #region GenerateModalityXML
        private bool GenerateModalityXML(GLModalityList[] ArrObj, ref string CatchMessage)
        {
            bool bReturn = false;
            StringBuilder sbXML = new StringBuilder();
            try
            {

                sbXML.Append("<modality>");

                for (int i = 0; i < ArrObj.Length; i++)
                {
                    sbXML.Append("<row>");
                    sbXML.Append("<category_id>" + ArrObj[i].CATEGORY_ID.ToString() + "</category_id>");
                    sbXML.Append("<modality_id>" + ArrObj[i].MODALITY_ID.ToString() + "</modality_id>");
                    sbXML.Append("<gl_code><![CDATA[" + ArrObj[i].GL_CODE + "]]></gl_code>");
                    sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                    sbXML.Append("</row>");
                }

                sbXML.Append("</modality>");
                strXMLModality = sbXML.ToString();
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

        #region GenerateServiceXML
        private bool GenerateServiceXML(GLServiceList[] ArrObj, ref string CatchMessage)
        {
            bool bReturn = false;
            StringBuilder sbXML = new StringBuilder();
            try
            {

                sbXML.Append("<service>");

                for (int i = 0; i < ArrObj.Length; i++)
                {
                    sbXML.Append("<row>");
                    sbXML.Append("<service_id>" + ArrObj[i].SERVICE_ID.ToString() + "</service_id>");
                    sbXML.Append("<modality_id>" + ArrObj[i].MODALITY_ID.ToString() + "</modality_id>");
                    sbXML.Append("<gl_code><![CDATA[" + ArrObj[i].GL_CODE + "]]></gl_code>");
                    sbXML.Append("<gl_code_after_hrs><![CDATA[" + ArrObj[i].GL_CODE_AFTER_HOURS + "]]></gl_code_after_hrs>");
                    sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                    sbXML.Append("</row>");
                }

                sbXML.Append("</service>");
                strXMLService = sbXML.ToString();
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

        #region GenerateNonRevenueXML
        private bool GenerateNonRevenueXML(GLNonRevenueHead[] ArrObj, ref string CatchMessage)
        {
            bool bReturn = false;
            StringBuilder sbXML = new StringBuilder();
            try
            {

                sbXML.Append("<nrh>");

                for (int i = 0; i < ArrObj.Length; i++)
                {
                    sbXML.Append("<row>");
                    sbXML.Append("<control_code><![CDATA[" + ArrObj[i].CONTROL_CODE + "]]></control_code>");
                    sbXML.Append("<gl_code><![CDATA[" + ArrObj[i].GL_CODE + "]]></gl_code>");
                    sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                    sbXML.Append("</row>");
                }

                sbXML.Append("</nrh>");
                strXMLNRH = sbXML.ToString();
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

        #region GenerateRadiologistChargeXML
        private bool GenerateRadiologistChargeXML(GLRadiologistCharges[] ArrObj, ref string CatchMessage)
        {
            bool bReturn = false;
            StringBuilder sbXML = new StringBuilder();
            try
            {

                sbXML.Append("<rc>");

                for (int i = 0; i < ArrObj.Length; i++)
                {
                    sbXML.Append("<row>");
                    sbXML.Append("<group_id>" + ArrObj[i].GROUP_ID.ToString() + "</group_id>");
                    sbXML.Append("<gl_code><![CDATA[" + ArrObj[i].GL_CODE + "]]></gl_code>");
                    sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                    sbXML.Append("</row>");
                }

                sbXML.Append("</rc>");
                strXMLRC = sbXML.ToString();
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

    public class GLModalityList
    {
        #region Constructor
        public GLModalityList()
        {
        }
        #endregion

        #region Variables
        int intCategoryID = 0;
        int intModalityID = 0;
        string strGLCode = string.Empty;
        #endregion

        #region Properties
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
        public string GL_CODE
        {
            get { return strGLCode; }
            set { strGLCode = value; }
        }
        #endregion
    }
    public class GLServiceList
    {
        #region Constructor
        public GLServiceList()
        {
        }
        #endregion

        #region Variables
        int intServiceID = 0;
        int intModalityID = 0;
        string strGLCode = string.Empty;
        string strGLCodeAfterHrs = string.Empty;
        #endregion

        #region Properties
        public int SERVICE_ID
        {
            get { return intServiceID; }
            set { intServiceID = value; }
        }
        public int MODALITY_ID
        {
            get { return intModalityID; }
            set { intModalityID = value; }
        }
        public string GL_CODE
        {
            get { return strGLCode; }
            set { strGLCode = value; }
        }
        public string GL_CODE_AFTER_HOURS
        {
            get { return strGLCodeAfterHrs; }
            set { strGLCodeAfterHrs = value; }
        }
        #endregion
    }
    public class GLNonRevenueHead
    {
        #region Constructor
        public GLNonRevenueHead()
        {
        }
        #endregion

        #region Variables
        string strControlCode = string.Empty;
        string strGLCode = string.Empty;
        #endregion

        #region Properties
        public string CONTROL_CODE
        {
            get { return strControlCode; }
            set { strControlCode = value; }
        }
        public string GL_CODE
        {
            get { return strGLCode; }
            set { strGLCode = value; }
        }
        #endregion
    }

    public class GLRadiologistCharges
    {
        #region Constructor
        public GLRadiologistCharges()
        {
        }
        #endregion

        #region Variables
        int intGroupID = 0;
        string strGLCode = string.Empty;
        #endregion

        #region Properties
        public int GROUP_ID
        {
            get { return intGroupID; }
            set { intGroupID = value; }
        }
        public string GL_CODE
        {
            get { return strGLCode; }
            set { strGLCode = value; }
        }
        #endregion
    }
}
