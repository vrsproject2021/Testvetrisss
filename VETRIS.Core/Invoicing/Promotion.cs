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
    public class Promotion
    {

        #region Constructor
        public Promotion()
        {
        }
        #endregion

        #region Variables
        int intMenuID = 0;
        Guid UserID = new Guid("00000000-0000-0000-0000-000000000000");
        int intUserRoleID = 0;
        string strUserName = string.Empty;

        Guid Id = new Guid("00000000-0000-0000-0000-000000000000");
        Guid BillingAccountId = new Guid("00000000-0000-0000-0000-000000000000");
        Guid ReasonId = new Guid("00000000-0000-0000-0000-000000000000");
        string strName = string.Empty;
        DateTime dtCreatedDate = DateTime.Today;
        DateTime dtFromDate = DateTime.Today;
        DateTime dtToDate = DateTime.Today;
        DateTime dtValidTillDate = DateTime.Today;
        string strXMLPromo = string.Empty;
        string strXMLModCredit = string.Empty;
        decimal decDiscPer = 0;
        
        string strStatus = string.Empty;
        string strType = "";
        Guid CreatedBy = new Guid("00000000-0000-0000-0000-000000000000");

        int intRowID = 0;
        #endregion

        #region Properties
        public Guid ID
        {
            get { return Id; }
            set { Id = value; }
        }
        public Guid BILLING_ACCOUNT_ID
        {
            get { return BillingAccountId; }
            set { BillingAccountId = value; }
        }
        public Guid REASON_ID
        {
            get { return ReasonId; }
            set { ReasonId = value; }
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
        public Guid CREATED_BY
        {
            get { return CreatedBy; }
            set { CreatedBy = value; }
        }
        public string STATUS 
        {
            get { return strStatus;}
            set { strStatus = value; }
        }
        public string TYPE 
        {
            get { return strType; }
            set { strType = value; }
        }
        public DateTime CREATED_DATE
        {
            get { return dtCreatedDate; }
            set { dtCreatedDate = value; }
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
        public DateTime VALID_TILL_DATE
        {
            get { return dtValidTillDate; }
            set { dtValidTillDate = value; }
        }
        public decimal DISCOUNT 
        {
            get { return decDiscPer; }
            set { decDiscPer = value; }
        }


        public int ROW_ID
        {
            get { return intRowID; }
            set { intRowID = value; }
        }
        #endregion

        #region FetchParameters
        public bool FetchParameters(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;


            try
            {
                SqlParameter[] SqlRecordParams = new SqlParameter[1];
                SqlRecordParams[0] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[0].Value = intMenuID;
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "invoicing_promotion_fetch_params", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "User";
                    ds.Tables[1].TableName = "Account";
                    ds.Tables[2].TableName = "Reasons";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region Browser Methods

        #region SearchBrowserList
        public bool SearchBrowserList(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[9];
            SqlRecordParams[0] = new SqlParameter("@billing_account_id", SqlDbType.UniqueIdentifier, 100);  SqlRecordParams[0].Value = BillingAccountId;
            SqlRecordParams[1] = new SqlParameter("@promotion_type", SqlDbType.NChar,1); SqlRecordParams[1].Value = strType;
            SqlRecordParams[2] = new SqlParameter("@is_active", SqlDbType.NChar, 1); SqlRecordParams[2].Value = strStatus;
            SqlRecordParams[3] = new SqlParameter("@created_by", SqlDbType.UniqueIdentifier);SqlRecordParams[3].Value = CreatedBy;
            SqlRecordParams[4] = new SqlParameter("@reason_id", SqlDbType.UniqueIdentifier); SqlRecordParams[4].Value = ReasonId;
            SqlRecordParams[5] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[5].Value = intMenuID;
            SqlRecordParams[6] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier);SqlRecordParams[6].Value = UserID;
            SqlRecordParams[7] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10);SqlRecordParams[7].Direction = ParameterDirection.Output;
            SqlRecordParams[8] = new SqlParameter("@return_status", SqlDbType.Int);SqlRecordParams[8].Direction = ParameterDirection.Output;


            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "invoicing_promotion_fetch_brw", SqlRecordParams);
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
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "invoicing_promotion_fetch_dtls", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Details";
                    ds.Tables[1].TableName = "Users";
                    ds.Tables[2].TableName = "Accounts";
                    ds.Tables[3].TableName = "Reasons";

                    #region Details
                    foreach (DataRow dr in ds.Tables["Details"].Rows)
                    {
                        strType = Convert.ToString(dr["promotion_type"]).Trim();
                        BillingAccountId = new Guid(dr["billing_account_id"].ToString());
                        dtFromDate = Convert.ToDateTime(dr["valid_from"]);
                        dtToDate = Convert.ToDateTime(dr["valid_till"]);
                        dtCreatedDate = Convert.ToDateTime(dr["date_created"]);
                        strStatus = Convert.ToString(dr["is_active"]).Trim();
                        ReasonId = new Guid(dr["reason_id"].ToString());
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

        #region LoadPromotions
        public bool LoadPromotions(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[2];
            SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;
            SqlRecordParams[1] = new SqlParameter("@billing_account_id", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = BillingAccountId;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "invoicing_promotions_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Promotions";
                    ds.Tables[1].TableName = "Institutions";
                    ds.Tables[2].TableName = "Modality";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region SaveRecord
        public bool SaveRecord(string ConfigPath, PromotionList[] ArrObj,  ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;

            if (ValidateRecord(ArrObj,ref ReturnMessage))
            {
                if ((GeneratePromotionXML(ArrObj, ref CatchMessage)))
                {
                    try
                    {
                        SqlParameter[] SqlRecordParams = new SqlParameter[13];
                        SqlRecordParams[0]  = new SqlParameter("@id", SqlDbType.UniqueIdentifier);SqlRecordParams[0].Value = Id; SqlRecordParams[0].Direction = ParameterDirection.InputOutput;
                        SqlRecordParams[1] = new SqlParameter("@promotion_type", SqlDbType.NVarChar, 1); SqlRecordParams[1].Value = strType;
                        SqlRecordParams[2] = new SqlParameter("@billing_account_id", SqlDbType.UniqueIdentifier); SqlRecordParams[2].Value = BillingAccountId;
                        SqlRecordParams[3] = new SqlParameter("@valid_from", SqlDbType.DateTime); SqlRecordParams[3].Value = dtFromDate;
                        SqlRecordParams[4] = new SqlParameter("@valid_till", SqlDbType.DateTime); SqlRecordParams[4].Value = dtToDate;
                        SqlRecordParams[5] = new SqlParameter("@reason_id", SqlDbType.UniqueIdentifier); SqlRecordParams[5].Value = ReasonId;
                        SqlRecordParams[6] = new SqlParameter("@is_active", SqlDbType.NChar, 1); SqlRecordParams[6].Value = strStatus;
                        SqlRecordParams[7] = new SqlParameter("@xml_details", SqlDbType.NText); SqlRecordParams[7].Value = strXMLPromo.Trim(); 
                        SqlRecordParams[8] = new SqlParameter("@user_id", SqlDbType.UniqueIdentifier);  SqlRecordParams[8].Value = UserID;
                        SqlRecordParams[9] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[9].Value = intMenuID;
                        SqlRecordParams[10] = new SqlParameter("@user_name", SqlDbType.NVarChar, 150); SqlRecordParams[10].Direction = ParameterDirection.Output;
                        SqlRecordParams[11] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[11].Direction = ParameterDirection.Output;
                        SqlRecordParams[12] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[12].Direction = ParameterDirection.Output;


                        if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                        intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "invoicing_promotion_save", SqlRecordParams);
                        intReturnValue = Convert.ToInt32(SqlRecordParams[12].Value);
                        if (intReturnValue == 1)
                            bReturn = true;
                        else
                            bReturn = false;

                        strUserName = Convert.ToString(SqlRecordParams[10].Value).Trim();
                        ReturnMessage = Convert.ToString(SqlRecordParams[11].Value).Trim();
                        Id = new Guid(Convert.ToString(SqlRecordParams[0].Value));

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
            }

            return bReturn;
        }
        #endregion

        #region ValidateRecord
        private bool ValidateRecord(PromotionList[] ArrObj, ref string ReturnMessage)
        {
            bool bReturn = true;

            if (strType.Trim() == string.Empty)
            {
                ReturnMessage = "259";
            }
            if (ReasonId == new Guid("00000000-0000-0000-0000-000000000000"))
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "265";
            }
            if (BillingAccountId == new Guid("00000000-0000-0000-0000-000000000000"))
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "225";
            }
            if (strType == "D")
            {
                if (dtFromDate.Year == 1900)
                {
                    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                    ReturnMessage = "214";
                }
            }
            if (dtToDate.Year == 1900)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage = "215";
            }

            if (strType == "D")
            {
                if(dtFromDate > dtToDate)
                {
                    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                    ReturnMessage = "216";
                }
            }

            if (ArrObj.Length == 0)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "260";
            }


            if (ReturnMessage.Trim() != string.Empty)
                bReturn = false;

            return bReturn;
        }


        #endregion

        #region GeneratePromotionXML
        private bool GeneratePromotionXML(PromotionList[] ArrObj, ref string CatchMessage)
        {
            bool bReturn = false;
            StringBuilder sbXML = new StringBuilder();

            try
            {
                if (ArrObj.Length > 0)
                {

                    sbXML.Append("<promo>");

                    for (int i = 0; i < ArrObj.Length; i = i + 1)
                    {
                        sbXML.Append("<row>");
                        sbXML.Append("<line_no>" + ArrObj[i].LINE_NUMBER.ToString() + "</line_no>");
                        sbXML.Append("<id><![CDATA[" + ArrObj[i].ID.ToString() + "]]></id>");
                        sbXML.Append("<institution_id><![CDATA[" + ArrObj[i].INSTITUTION_ID.ToString() + "]]></institution_id>");
                        sbXML.Append("<modality_id>" + ArrObj[i].MODALITY_ID.ToString() + "</modality_id>");
                        sbXML.Append("<free_credits>" + ArrObj[i].FREE_CREDITS.ToString() + "</free_credits>");
                        sbXML.Append("<discount_percent>" + ArrObj[i].DISCOUNT_PERCENT.ToString() + "</discount_percent>");
                        sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                        sbXML.Append("</row>");
                    }

                    sbXML.Append("</promo>");
                    strXMLPromo = sbXML.ToString();


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
    public class PromotionList
    {
        #region Constructor
        public PromotionList()
        {
        }
        #endregion

        #region Variables
        Guid Id = Guid.Empty;
        int intLineNo = 0;
        Guid InstitutionId = Guid.Empty;
        int intModailtyID = 0;
        int intFreeCredits = 0;
        decimal decDiscPer = 0;
        #endregion

        #region Properties
        public Guid ID
        {
            get { return Id; }
            set { Id = value; }
        }
        public int LINE_NUMBER
        {
            get { return intLineNo; }
            set { intLineNo = value; }
        }
        public Guid INSTITUTION_ID
        {
            get { return InstitutionId; }
            set { InstitutionId = value; }
        }
        public int MODALITY_ID
        {
            get { return intModailtyID; }
            set { intModailtyID = value; }
        }
        public int FREE_CREDITS
        {
            get { return intFreeCredits; }
            set { intFreeCredits = value; }
        }
        public decimal DISCOUNT_PERCENT
        {
            get { return decDiscPer; }
            set { decDiscPer = value; }
        }
        #endregion
    }

}
