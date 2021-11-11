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
    public class ChargesDiscount
    {
         #region Constractor
        public ChargesDiscount()
        {

        }
        #endregion

        #region Variables
        Guid UserID = new Guid("00000000-0000-0000-0000-000000000000");
        Guid BillingAccountID = new Guid("00000000-0000-0000-0000-000000000000");
        int intMenuID = 0;
        int intUserRoleID = 0;
        decimal dscChargesDisc = 0;
        string strBillingAccountName = string.Empty;
        string strCode = string.Empty;
        string strUserName = string.Empty;
        string CatchMessage = string.Empty;
        

        string strXML = string.Empty;
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

        public Guid BILLING_ACCOUNT_ID
        {
            get { return BillingAccountID; }
            set { BillingAccountID = value; }
        }

        public string BILLING_ACCOUNT_NAME 
        {
            get { return strBillingAccountName; }
            set { strBillingAccountName = value; }
        }

        public string CODE 
        {
            get { return strCode; }
            set { strCode = value; }
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
        public decimal CHARGES_DISCOUNT 
        {
            get { return dscChargesDisc; }
            set { dscChargesDisc = value; }
        }
        #endregion

        #region SearchBrowserList
        public bool LoadRecords(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[2];
            SqlRecordParams[0] = new SqlParameter("@menu_id", SqlDbType.BigInt); SqlRecordParams[0].Value = intMenuID;
            SqlRecordParams[1] = new SqlParameter("@user_id", SqlDbType.NVarChar, 50); SqlRecordParams[1].Value = UserID;



            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "invoicing_charges_discount_fetch", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "ChargesDiscount";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region SaveRecord
        public bool SaveRecord(string ConfigPath,ChargesDiscount[] ArrObj, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false; string strCode = string.Empty;
            int intReturnValue = 0; int intExecReturn = 0;
                if (GenerateXML(ArrObj, ref CatchMessage))
                {
            
                try
                {
                    SqlParameter[] SqlRecordParams = new SqlParameter[6];
                    SqlRecordParams[0] = new SqlParameter("@xml_data", SqlDbType.NText); SqlRecordParams[0].Value = strXML.Trim();
                    SqlRecordParams[1] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[1].Value = UserID;
                    SqlRecordParams[2] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[2].Value = intMenuID;
                    SqlRecordParams[3] = new SqlParameter("@user_name", SqlDbType.NVarChar, 700); SqlRecordParams[3].Direction = ParameterDirection.Output;
                    SqlRecordParams[4] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[4].Direction = ParameterDirection.Output;
                    SqlRecordParams[5] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[5].Direction = ParameterDirection.Output;

                    if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                    intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "invoicing_charges_discount_save", SqlRecordParams);
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
                    strUserName = strCode.ToString();
                }

           
            return bReturn;
        }
        #endregion

        #region GenerateXML
        private bool GenerateXML(ChargesDiscount[] ArrObj, ref string CatchMessage)
        {
            bool bReturn = false;
            StringBuilder sbXML = new StringBuilder();

            try
            {
                if (ArrObj.Length > 0)
                {

                    sbXML.Append("<chargesdiscount>");

                    for (int i = 0; i < ArrObj.Length; i = i + 1)
                    {
                        sbXML.Append("<row>");
                        sbXML.Append("<billing_account_id>" + ArrObj[i].BILLING_ACCOUNT_ID.ToString() + "</billing_account_id>");
                        sbXML.Append("<charges_discount><![CDATA[" + ArrObj[i].CHARGES_DISCOUNT + "]]></charges_discount>");
                        sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                        sbXML.Append("</row>");
                    }

                    sbXML.Append("</chargesdiscount>");
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
