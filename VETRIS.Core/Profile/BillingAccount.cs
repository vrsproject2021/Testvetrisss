using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using VETRIS.DAL;

namespace VETRIS.Core.Profile
{
    public class BillingAccount
    {
        #region Constructor
        public BillingAccount()
        {

        }
        #endregion

        #region Variables
        int intMenuID = 0;
        Guid UserID = new Guid("00000000-0000-0000-0000-000000000000");
        int intUserRoleID = 0;
        string strUserName = string.Empty;

        Guid Id = new Guid("00000000-0000-0000-0000-000000000000");
        string strCode = string.Empty;
        string strName = string.Empty;
        string strAddress1 = string.Empty;
        string strAddress2 = string.Empty;
        string strCity = string.Empty;
        int intStateID = 0;
        string strStateName = string.Empty;
        int intCountryID = 231;
        string strCountryName = string.Empty;
        string strZip = string.Empty;
        string strPhone = string.Empty;
        string strFaxNo = string.Empty;
        string strEmail = string.Empty;
        string strContactPersonName = string.Empty;
        string strContactPersonMobile = string.Empty;
        string strContactPersonEmail = string.Empty;
        Guid SalespersonId = new Guid("00000000-0000-0000-0000-000000000000");
        string strLoginID = string.Empty;
        string strPwd = string.Empty;
        string strUserEmailID = string.Empty;
        string strUserMobileNo = string.Empty;
        string strNotificationPref = "B";
        decimal decCommission1stYr = 0;
        decimal decCommission2ndYr = 0;
        decimal decDiscPer = 0;
        string strAccountantName = string.Empty;
        string strIsActive = "Y";
        Guid InstId = new Guid("00000000-0000-0000-0000-000000000000");
        string strInstName = string.Empty;

        string strXMLInst = string.Empty;
        string strXMLContact = string.Empty;
        string strXMLFee = string.Empty;
        string strXMLPhysician = string.Empty;

        string CatchMessage = string.Empty;

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
        public string CODE
        {
            get { return strCode; }
            set { strCode = value; }
        }
        public string NAME
        {
            get { return strName; }
            set { strName = value; }
        }
        public string ADDRESS_LINE1
        {
            get { return strAddress1; }
            set { strAddress1 = value; }
        }
        public string ADDRESS_LINE2
        {
            get { return strAddress2; }
            set { strAddress2 = value; }
        }
        public string CITY
        {
            get { return strCity; }
            set { strCity = value; }
        }
        public int STATE_ID
        {
            get { return intStateID; }
            set { intStateID = value; }
        }
        public string STATE_NAME
        {
            get { return strStateName; }
            set { strStateName = value; }
        }
        public int COUNTRY_ID
        {
            get { return intCountryID; }
            set { intCountryID = value; }
        }
        public string COUNTRY_NAME
        {
            get { return strCountryName; }
            set { strCountryName = value; }
        }
        public string ZIP
        {
            get { return strZip; }
            set { strZip = value; }
        }
        public string PHONE
        {
            get { return strPhone; }
            set { strPhone = value; }
        }
        public string FAX
        {
            get { return strFaxNo; }
            set { strFaxNo = value; }
        }
        public string EMAIL_ID
        {
            get { return strEmail; }
            set { strEmail = value; }
        }
        public string CONTACT_PERSON_NAME
        {
            get { return strContactPersonName; }
            set { strContactPersonName = value; }
        }
        public string CONTACT_PERSION_MOBILE
        {
            get { return strContactPersonMobile; }
            set { strContactPersonMobile = value; }
        }
        public string CONTACT_PERSION_EMAIL_ID
        {
            get { return strContactPersonEmail; }
            set { strContactPersonEmail = value; }
        }
        public Guid SALES_PERSON_ID
        {
            get { return SalespersonId; }
            set { SalespersonId = value; }
        }
        public decimal COMMISSION_1ST_YEAR
        {
            get { return decCommission1stYr; }
            set { decCommission1stYr = value; }
        }
        public decimal COMMISSION_2ND_YEAR
        {
            get { return decCommission2ndYr; }
            set { decCommission2ndYr = value; }
        }
        public string LOGIN_ID
        {
            get { return strLoginID; }
            set { strLoginID = value; }
        }
        public string LOGIN_PASSWORD
        {
            get { return strPwd; }
            set { strPwd = value; }
        }
        public string USER_EMAIL_ID
        {
            get { return strUserEmailID; }
            set { strUserEmailID = value; }
        }
        public string USER_MOBILE_NUMBER
        {
            get { return strUserMobileNo; }
            set { strUserMobileNo = value; }
        }
        public string NOTIFICATION_PREFERENCE
        {
            get { return strNotificationPref; }
            set { strNotificationPref = value; }
        }
        public decimal DISCOUNT_PERCENT
        {
            get { return decDiscPer; }
            set { decDiscPer = value; }
        }
        public string ACCOUNTANT_NAME
        {
            get { return strAccountantName; }
            set { strAccountantName = value; }
        }
        public Guid INSTITUTION_ID
        {
            get { return InstId; }
            set { InstId = value; }
        }
        public string INSTITUTION_NAME
        {
            get { return strInstName; }
            set { strInstName = value; }
        }
        public string IS_ACTIVE
        {
            get { return strIsActive; }
            set { strIsActive = value; }
        }
        #endregion

        #region Methods

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
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "master_billing_account_fetch_dtls", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Details";

                    #region Details
                    foreach (DataRow dr in ds.Tables["Details"].Rows)
                    {
                        strCode = Convert.ToString(dr["code"]).Trim();
                        strName = Convert.ToString(dr["name"]).Trim();
                        strAddress1 = Convert.ToString(dr["address_1"]).Trim();
                        strAddress2 = Convert.ToString(dr["address_2"]).Trim();
                        strCity = Convert.ToString(dr["city"]).Trim();
                        intStateID = Convert.ToInt32(dr["state_id"]);
                        intCountryID = Convert.ToInt32(dr["country_id"]);
                        strZip = Convert.ToString(dr["zip"]).Trim();
                        strEmail = Convert.ToString(dr["email_id"]).Trim();
                        strPhone = Convert.ToString(dr["phone_no"]).Trim();
                        strFaxNo = Convert.ToString(dr["fax_no"]).Trim();
                        strContactPersonName = Convert.ToString(dr["contact_person_name"]).Trim();
                        strContactPersonMobile = Convert.ToString(dr["contact_person_mobile"]).Trim();
                        strContactPersonEmail = Convert.ToString(dr["contact_person_email_id"]).Trim();
                        strLoginID = Convert.ToString(dr["login_id"]).Trim();
                        strPwd = Convert.ToString(dr["login_pwd"]).Trim(); if (strPwd != string.Empty) strPwd = CoreCommon.DecryptString(strPwd);
                        strUserEmailID = Convert.ToString(dr["user_email_id"]).Trim();
                        strUserMobileNo = Convert.ToString(dr["user_mobile_no"]).Trim();
                        strNotificationPref = Convert.ToString(dr["notification_pref"]).Trim();
                        strIsActive = Convert.ToString(dr["is_active"]).Trim();
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

        #region LoadInstitutions
        public bool LoadInstitutions(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "profile_billing_account_fetch_institutions", SqlRecordParams);
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

        #region LoadContacts
        public bool LoadContacts(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "master_billing_account_fetch_contacts", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Contacts";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region LoadPhysicians
        public bool LoadPhysicians(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "master_billing_account_fetch_physicians", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Institutions";
                    ds.Tables[1].TableName = "Physicians";
                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }


            return bReturn;
        }
        #endregion

        #region FechContactDetails
        public bool FechContactDetails(string ConfigPath, ref string CatchMessage)
        {
            bool bReturn = false;
            DataSet ds = new DataSet();
            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            SqlRecordParams[0] = new SqlParameter("@institution_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = InstId;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "master_billing_account_fetch_contact_details", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Contact";
                    foreach (DataRow dr in ds.Tables["Contact"].Rows)
                    {
                        strInstName = Convert.ToString(dr["institution_name"]).Trim();
                        strPhone = Convert.ToString(dr["phone_no"]).Trim();
                        strFaxNo = Convert.ToString(dr["fax_no"]).Trim();
                        strContactPersonName = Convert.ToString(dr["contact_person_name"]).Trim();
                        strContactPersonMobile = Convert.ToString(dr["contact_person_mobile"]).Trim();
                        strContactPersonEmail = Convert.ToString(dr["contact_person_email_id"]).Trim();

                    }

                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            finally { ds.Dispose(); }
            return bReturn;
        }
        #endregion

        #region FetchPhysicianDetails
        public bool FetchPhysicianDetails(string ConfigPath, ref DataSet ds, ref string CatchMessage)
        {
            bool bReturn = false;
            SqlParameter[] SqlRecordParams = new SqlParameter[1];
            SqlRecordParams[0] = new SqlParameter("@institution_id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = InstId;

            try
            {
                if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                ds = DAL.DataHelper.ExecuteDataset(CoreCommon.CONNECTION_STRING, "master_billing_account_fetch_physician_details", SqlRecordParams);
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].TableName = "Institutions";
                    ds.Tables[1].TableName = "Physicians";


                }
                bReturn = true;
            }
            catch (Exception expErr)
            { bReturn = false; CatchMessage = expErr.Message; }

            return bReturn;
        }
        #endregion

        #region SaveRecord
        public bool SaveRecord(string ConfigPath, ContactList[] ArrContObj, PhysicianList[] ArrPhysObj, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;

            if (ValidateRecord(ref ReturnMessage))
            {
                if ((GenerateContactXML(ArrContObj, ref ReturnMessage, ref CatchMessage)) && (GeneratePhysicianXML(ArrPhysObj, ref ReturnMessage, ref CatchMessage)))
                {
                    try
                    {
                        SqlParameter[] SqlRecordParams = new SqlParameter[13];
                        SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id; 
                        SqlRecordParams[1] = new SqlParameter("@login_id", SqlDbType.NVarChar, 50); SqlRecordParams[1].Value = strLoginID;
                        SqlRecordParams[2] = new SqlParameter("@login_pwd", SqlDbType.NVarChar, 200); SqlRecordParams[2].Value = strPwd;
                        SqlRecordParams[3] = new SqlParameter("@user_email_id", SqlDbType.NVarChar, 50); SqlRecordParams[3].Value = strUserEmailID;
                        SqlRecordParams[4] = new SqlParameter("@user_mobile_no ", SqlDbType.NVarChar, 20); SqlRecordParams[4].Value = strUserMobileNo;
                        SqlRecordParams[5] = new SqlParameter("@notification_pref", SqlDbType.NChar, 1); SqlRecordParams[5].Value = strNotificationPref;
                        SqlRecordParams[6] = new SqlParameter("@xml_contacts", SqlDbType.NText); if (strXMLContact.Trim() != string.Empty) SqlRecordParams[6].Value = strXMLContact.Trim(); else SqlRecordParams[6].Value = DBNull.Value;
                        SqlRecordParams[7] = new SqlParameter("@xml_physicians", SqlDbType.NText); if (strXMLPhysician.Trim() != string.Empty) SqlRecordParams[7].Value = strXMLPhysician.Trim(); else SqlRecordParams[7].Value = DBNull.Value;
                        SqlRecordParams[8] = new SqlParameter("@updated_by", SqlDbType.UniqueIdentifier); SqlRecordParams[8].Value = UserID;
                        SqlRecordParams[9] = new SqlParameter("@menu_id", SqlDbType.Int); SqlRecordParams[9].Value = intMenuID;
                        SqlRecordParams[10] = new SqlParameter("@user_name", SqlDbType.NVarChar, 700); SqlRecordParams[10].Direction = ParameterDirection.Output;
                        SqlRecordParams[11] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[11].Direction = ParameterDirection.Output;
                        SqlRecordParams[12] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[12].Direction = ParameterDirection.Output;


                        if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                        intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "profile_billing_account_save", SqlRecordParams);
                        intReturnValue = Convert.ToInt32(SqlRecordParams[12].Value);
                        if (intReturnValue == 1)
                            bReturn = true;
                        else
                            bReturn = false;

                        strUserName = Convert.ToString(SqlRecordParams[10].Value).Trim();
                        ReturnMessage = Convert.ToString(SqlRecordParams[11].Value).Trim();

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
        private bool ValidateRecord(ref string ReturnMessage)
        {
            bool bReturn = true;

            if (strIsActive == "Y")
            {
                if (strLoginID.Trim() == string.Empty)
                {
                    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                    ReturnMessage += "112";
                }
                if (strPwd.Trim() == string.Empty)
                {
                    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                    ReturnMessage += "120";
                }
                if ((strNotificationPref == "E") || (strNotificationPref == "B"))
                {
                    if (strUserEmailID.Trim() == string.Empty)
                    {
                        if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                        ReturnMessage += "219";
                    }
                    else
                    {
                        if (!CoreCommon.IsEmailValid(strUserEmailID))
                        {
                            if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                            ReturnMessage += "084";
                        }
                    }
                }

                if ((strNotificationPref == "S") || (strNotificationPref == "B"))
                {
                    if (strUserMobileNo.Trim() == string.Empty)
                    {
                        if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                        ReturnMessage += "220";
                    }
                }

              
               
            }


            if (ReturnMessage.Trim() != string.Empty)
                bReturn = false;

            return bReturn;
        }


        #endregion

        #region GenerateContactXML
        private bool GenerateContactXML(ContactList[] ArrObj, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            StringBuilder sbXML = new StringBuilder();

            try
            {
                if (ArrObj.Length > 0)
                {

                    sbXML.Append("<contact>");

                    for (int i = 0; i < ArrObj.Length; i = i + 1)
                    {
                        sbXML.Append("<row>");
                        sbXML.Append("<institution_id><![CDATA[" + ArrObj[i].INSTITUTION_ID.ToString() + "]]></institution_id>");
                        sbXML.Append("<phone_no><![CDATA[" + ArrObj[i].PHONE_NUMBER + "]]></phone_no>");
                        sbXML.Append("<fax_no><![CDATA[" + ArrObj[i].FAX_NUMBER + "]]></fax_no>");
                        sbXML.Append("<contact_person_name><![CDATA[" + ArrObj[i].CONTACT_PERSON + "]]></contact_person_name>");
                        sbXML.Append("<contact_person_mobile><![CDATA[" + ArrObj[i].CONTACT_PERSON_MOBILE_NUMBER + "]]></contact_person_mobile>");
                        sbXML.Append("<contact_person_email_id><![CDATA[" + ArrObj[i].CONTACT_PERSON_EMAIL_ID + "]]></contact_person_email_id>");
                        sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                        sbXML.Append("</row>");
                    }

                    sbXML.Append("</contact>");

                }

                strXMLContact = sbXML.ToString();
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

        #region GeneratePhysicianXML
        private bool GeneratePhysicianXML(PhysicianList[] ArrObj, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            StringBuilder sbXML = new StringBuilder();

            try
            {
                if (ArrObj.Length > 0)
                {

                    sbXML.Append("<physician>");

                    for (int i = 0; i < ArrObj.Length; i = i + 1)
                    {
                        #region Validations
                        if ((ArrObj[i].FIRST_NAME.Trim() + " " + ArrObj[i].LAST_NAME.Trim()).Trim() == string.Empty)
                        {
                            if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                            ReturnMessage = "087";
                            strUserName = (i + 1).ToString();
                        }



                        if (ReturnMessage.Trim() != string.Empty)
                        {
                            break;
                        }
                        #endregion

                        sbXML.Append("<row>");
                        sbXML.Append("<institution_id>" + ArrObj[i].INSTITUTION_ID.ToString() + "</institution_id>");
                        sbXML.Append("<physician_id>" + ArrObj[i].ID.ToString() + "</physician_id>");
                        sbXML.Append("<physician_fname><![CDATA[" + ArrObj[i].FIRST_NAME + "]]></physician_fname>");
                        sbXML.Append("<physician_lname><![CDATA[" + ArrObj[i].LAST_NAME + "]]></physician_lname>");
                        sbXML.Append("<physician_credentials><![CDATA[" + ArrObj[i].CREDENTIALS + "]]></physician_credentials>");
                        sbXML.Append("<physician_email><![CDATA[" + ArrObj[i].EMAIL_ID + "]]></physician_email>");
                        sbXML.Append("<physician_mobile><![CDATA[" + ArrObj[i].MOBILE_NUMBER + "]]></physician_mobile>");
                        sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                        sbXML.Append("</row>");
                    }

                    if (ReturnMessage.Trim() != string.Empty)
                    {
                        bReturn = false;
                        sbXML.Clear();
                        strXMLPhysician = string.Empty;
                    }
                    else
                    {
                        bReturn = true;
                        sbXML.Append("</physician>");
                        strXMLPhysician = sbXML.ToString();
                    }


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

    public class ContactList
    {
        #region Constructor
        public ContactList()
        {
        }
        #endregion

        #region Variables
        Guid InstId = Guid.Empty;
        string strPhoneNo = string.Empty;
        string strFaxNo = string.Empty;
        string strContactPerson = string.Empty;
        string strContactPersonEmail = string.Empty;
        string strContPersonMobileNo = string.Empty;
        #endregion

        #region Properties
        public Guid INSTITUTION_ID
        {
            get { return InstId; }
            set { InstId = value; }
        }
        public string PHONE_NUMBER
        {
            get { return strPhoneNo; }
            set { strPhoneNo = value; }
        }
        public string FAX_NUMBER
        {
            get { return strFaxNo; }
            set { strFaxNo = value; }
        }
        public string CONTACT_PERSON
        {
            get { return strContactPerson; }
            set { strContactPerson = value; }
        }
        public string CONTACT_PERSON_EMAIL_ID
        {
            get { return strContactPersonEmail; }
            set { strContactPersonEmail = value; }
        }
        public string CONTACT_PERSON_MOBILE_NUMBER
        {
            get { return strContPersonMobileNo; }
            set { strContPersonMobileNo = value; }
        }
        #endregion
    }
}
