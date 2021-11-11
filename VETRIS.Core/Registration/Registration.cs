using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VETRIS.Core.Registration
{
    public class Registration
    {
        #region Constructor
        public Registration()
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
        int intCountryID = 0;
        string strCountryName = string.Empty;
        string strZip = string.Empty;
        string strPhone = string.Empty;
        string strMobile = string.Empty;
        string strEmail = string.Empty;
        string strContactPersonName = string.Empty;
        string strContactPersonMobile = string.Empty;
        string strSubmitBy = string.Empty;
        string strImgSoftwareName = string.Empty;
        //Guid LoginUserId = new Guid("00000000-0000-0000-0000-000000000000");
        string strLoginID = string.Empty;
        string strPwd = string.Empty;
        string strLoginEmail = string.Empty;
        string strLoginMobile = string.Empty;
        string strPaymentMethod = string.Empty;
        string strIsEmailVerified = string.Empty;
        string strIsMobileVerified = string.Empty;
        bool IsModalitySelected = false;
        string strXMLPhysician = string.Empty;
        string strXMLModalityLink = string.Empty;

        //decimal decDefaultFee = 0;
        //string strNotificationPref = "B";
        //string strIsActive = "Y";
        #endregion

        #region Properties
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
        public string MOBILE
        {
            get { return strMobile; }
            set { strMobile = value; }
        }
        public string EMAIL_ID
        {
            get { return strEmail; }
            set { strEmail = value; }
        }
        public string Contact_Person_Name
        {
            get { return strContactPersonName; }
            set { strContactPersonName = value; }
        }
        public string Contact_Person_Mobile
        {
            get { return strContactPersonMobile; }
            set { strContactPersonMobile = value; }
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
        public string LOGIN_EMAIL_ID
        {
            get { return strLoginEmail; }
            set { strLoginEmail = value; }
        }
        public string LOGIN_MOBILE_NO
        {
            get { return strLoginMobile; }
            set { strLoginMobile = value; }
        }
        public string PREFERRED_PMT_METHOD
        {
            get { return strPaymentMethod; }
            set { strPaymentMethod = value; }
        }
        public string IS_EMAIL_VERIFIED
        {
            get { return strIsEmailVerified; }
            set { strIsEmailVerified = value; }
        }
        public string IS_MOBILE_VERIFIED
        {
            get { return strIsMobileVerified; }
            set { strIsMobileVerified = value; }
        }
        public bool IS_Modality_Selected
        {
            get { return IsModalitySelected; }
            set { IsModalitySelected = value; }
        }
        public string SUBMITTED_BY
        {
            get { return strSubmitBy; }
            set { strSubmitBy = value; }
        }
        public string IMAGE_SOFTWARE_NAME
        {
            get { return strImgSoftwareName; }
            set { strImgSoftwareName = value; }
        }
        #endregion

        #region SaveRecord
        public bool SaveRecord(string ConfigPath, VETRIS.Core.Registration.PhysicianList[] ArrPhysObj, InstitutionRegModalityLink[] ArrModalityLink, ref string ReturnMessage, ref string CatchMessage)
        {
            bool bReturn = false;
            int intReturnValue = 0; int intExecReturn = 0;

            if (ValidateRecord(ArrPhysObj, ref ReturnMessage) && GeneratePhysicianLinkXML(ArrPhysObj, ref ReturnMessage, ref CatchMessage) && GenerateModalityLinkXML(ArrModalityLink, ref CatchMessage))
            {

                try
                {
                    SqlParameter[] SqlRecordParams = new SqlParameter[29];
                    SqlRecordParams[0] = new SqlParameter("@id", SqlDbType.UniqueIdentifier); SqlRecordParams[0].Value = Id; SqlRecordParams[0].Direction = ParameterDirection.InputOutput;
                    SqlRecordParams[1] = new SqlParameter("@name", SqlDbType.NVarChar, 80); SqlRecordParams[1].Value = strName;
                    SqlRecordParams[2] = new SqlParameter("@address_1", SqlDbType.NVarChar, 80); SqlRecordParams[2].Value = strAddress1;
                    SqlRecordParams[3] = new SqlParameter("@address_2", SqlDbType.NVarChar, 100); SqlRecordParams[3].Value = "";
                    SqlRecordParams[4] = new SqlParameter("@city", SqlDbType.NVarChar, 100); SqlRecordParams[4].Value = strCity;
                    SqlRecordParams[5] = new SqlParameter("@state_id", SqlDbType.Int); SqlRecordParams[5].Value = intStateID;
                    SqlRecordParams[6] = new SqlParameter("@country_id", SqlDbType.Int); SqlRecordParams[6].Value = intCountryID;
                    SqlRecordParams[7] = new SqlParameter("@zip", SqlDbType.NVarChar,20); SqlRecordParams[7].Value = strZip;
                    SqlRecordParams[8] = new SqlParameter("@email_id", SqlDbType.NVarChar,50); SqlRecordParams[8].Value = strEmail;
                    SqlRecordParams[9] = new SqlParameter("@phone_no", SqlDbType.NVarChar, 50); SqlRecordParams[9].Value = strPhone;
                    SqlRecordParams[10] = new SqlParameter("@mobile_no", SqlDbType.NVarChar, 30); SqlRecordParams[10].Value = strMobile;
                    SqlRecordParams[11] = new SqlParameter("@contact_person_name", SqlDbType.NVarChar, 100); SqlRecordParams[11].Value = strContactPersonName;
                    SqlRecordParams[12] = new SqlParameter("@contact_person_mobile", SqlDbType.NVarChar, 100); SqlRecordParams[12].Value = strContactPersonMobile;
                    SqlRecordParams[13] = new SqlParameter("@login_id", SqlDbType.NVarChar, 20); SqlRecordParams[13].Value = strLoginID;
                    SqlRecordParams[14] = new SqlParameter("@login_password", SqlDbType.NVarChar,200); SqlRecordParams[14].Value = strPwd;
                    SqlRecordParams[15] = new SqlParameter("@login_email_id", SqlDbType.NVarChar, 100); SqlRecordParams[15].Value = strLoginEmail;
                    SqlRecordParams[16] = new SqlParameter("@login_mobile_no", SqlDbType.NVarChar, 20); SqlRecordParams[16].Value = strLoginMobile;
                    //SqlRecordParams[17] = new SqlParameter("@preferred_pmt_method", SqlDbType.NVarChar,5); SqlRecordParams[17].Value = strPaymentMethod;
                    SqlRecordParams[17] = new SqlParameter("@is_email_verified", SqlDbType.NChar, 1); SqlRecordParams[17].Value = string.IsNullOrEmpty(strIsEmailVerified) ? "N" : strIsEmailVerified;
                    SqlRecordParams[18] = new SqlParameter("@is_mobile_verified", SqlDbType.NChar, 1); SqlRecordParams[18].Value = strIsMobileVerified;
                    SqlRecordParams[19] = new SqlParameter("@submitted_by", SqlDbType.NVarChar, 200); SqlRecordParams[19].Value = strSubmitBy;
                    SqlRecordParams[20] = new SqlParameter("@img_software_name", SqlDbType.NVarChar, 200); SqlRecordParams[20].Value = strImgSoftwareName;
                    
                    SqlRecordParams[21] = new SqlParameter("@xml_modality_link", SqlDbType.NText); if (strXMLModalityLink.Trim() != string.Empty) SqlRecordParams[21].Value = strXMLModalityLink.Trim(); else SqlRecordParams[21].Value = DBNull.Value;
                    SqlRecordParams[22] = new SqlParameter("@xml_physician_link", SqlDbType.NText); if (strXMLPhysician.Trim() != string.Empty) SqlRecordParams[22].Value = strXMLPhysician.Trim(); else SqlRecordParams[22].Value = DBNull.Value;
                    SqlRecordParams[23] = new SqlParameter("@code", SqlDbType.NVarChar, 5); SqlRecordParams[23].Value = strCode; SqlRecordParams[23].Direction = ParameterDirection.InputOutput;
                    SqlRecordParams[24] = new SqlParameter("@state_name", SqlDbType.NVarChar, 100); SqlRecordParams[24].Value = strStateName;
                    SqlRecordParams[25] = new SqlParameter("@country_name", SqlDbType.NVarChar, 100); SqlRecordParams[25].Value = strCountryName;
                    SqlRecordParams[26] = new SqlParameter("@user_name", SqlDbType.NVarChar, 700); SqlRecordParams[26].Direction = ParameterDirection.Output;
                    SqlRecordParams[27] = new SqlParameter("@error_code", SqlDbType.NVarChar, 10); SqlRecordParams[27].Direction = ParameterDirection.Output;
                    SqlRecordParams[28] = new SqlParameter("@return_status", SqlDbType.Int); SqlRecordParams[28].Direction = ParameterDirection.Output;

                    if (CoreCommon.CONNECTION_STRING == string.Empty) CoreCommon.GetConnectionString(ConfigPath);
                    intExecReturn = DAL.DataHelper.ExecuteNonQuery(CoreCommon.CONNECTION_STRING, CommandType.StoredProcedure, "registration_save", SqlRecordParams);
                    intReturnValue = Convert.ToInt32(SqlRecordParams[28].Value);
                    if (intReturnValue == 1)
                        bReturn = true;
                    else
                        bReturn = false;

                    strUserName = Convert.ToString(SqlRecordParams[26].Value).Trim();
                    ReturnMessage = Convert.ToString(SqlRecordParams[27].Value);

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
        private bool ValidateRecord(VETRIS.Core.Registration.PhysicianList[] ArrPhysObj, ref string ReturnMessage)
        {
            bool bReturn = true;

            if (string.IsNullOrEmpty(strName))
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage = "352";
            }
            if (string.IsNullOrEmpty(strAddress1))
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "353";
            }
            if (intCountryID <= 0)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "354";
            }
            if (string.IsNullOrEmpty(strCity))
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "416";
            }
            if (intStateID <= 0)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "355";
            }
            if (string.IsNullOrEmpty(strZip))
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "356";
            }
            if (string.IsNullOrEmpty(strContactPersonName))
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "361";
            }

            if (string.IsNullOrEmpty(strContactPersonMobile))
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "090";
            }
            if (string.IsNullOrEmpty(strPhone))
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "413";
            }
            if (string.IsNullOrEmpty(strEmail))
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "082";
            }
            else
            {
                if (!CoreCommon.IsEmailValid(strEmail))
                {
                    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                    ReturnMessage += "084";
                }
            }
            if (!IsModalitySelected)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "357";
            }
            //if (string.IsNullOrEmpty(strPaymentMethod) || strPaymentMethod == "0")
            //{
            //    if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
            //    ReturnMessage += "358";
            //}
            if (string.IsNullOrEmpty(strLoginID))
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "359";
            }
            if (string.IsNullOrEmpty(strPwd))
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "360";
            }
            if (!string.IsNullOrEmpty(strLoginEmail) && !CoreCommon.IsEmailValid(strLoginEmail))
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "084";
            }
            if (string.IsNullOrEmpty(strSubmitBy))
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "412";
            }
            if (ArrPhysObj.Length == 0)
            {
                if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                ReturnMessage += "364";
            }

            if (ReturnMessage.Trim() != string.Empty)
                bReturn = false;

            return bReturn;
        }

        #endregion

        #region GeneratePhysicianLinkXML
        private bool GeneratePhysicianLinkXML(PhysicianList[] ArrObj, ref string ReturnMessage, ref string CatchMessage)
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
                        if (string.IsNullOrEmpty(ArrObj[i].FIRST_NAME) && string.IsNullOrEmpty(ArrObj[i].LAST_NAME))
                        {
                            if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                            ReturnMessage = "087";
                            strUserName = (i + 1).ToString();
                        }
                        if (!string.IsNullOrEmpty(ArrObj[i].EMAIL_ID) && !CoreCommon.IsEmailValid(ArrObj[i].EMAIL_ID))
                        {
                            if (ReturnMessage != string.Empty) ReturnMessage += CoreCommon.STRING_DIVIDER;
                            ReturnMessage += "084";
                        }
                        if (ReturnMessage.Trim() != string.Empty)
                        {
                            break;
                        }
                        #endregion

                        sbXML.Append("<row>");
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

        #region GenerateModalityLinkXML
        private bool GenerateModalityLinkXML(InstitutionRegModalityLink[] ArrObj, ref string CatchMessage)
        {
            bool bReturn = false;
            StringBuilder sbXML = new StringBuilder();

            try
            {
                if (ArrObj.Length > 0)
                {

                    sbXML.Append("<modality_link>");

                    for (int i = 0; i < ArrObj.Length; i ++)
                    {
                        sbXML.Append("<row>");
                        sbXML.Append("<modality_id>" + ArrObj[i].modality_id.ToString() + "</modality_id>");
                        sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                        sbXML.Append("</row>");
                    }

                    sbXML.Append("</modality_link>");
                    strXMLModalityLink = sbXML.ToString();


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

    public class PhysicianList
    {
        #region Constructor
        public PhysicianList()
        {
        }
        #endregion

        #region Variables
        Guid InstId = Guid.Empty;
        Guid Id = Guid.Empty;
        string strName = string.Empty;
        string strFName = string.Empty;
        string strLName = string.Empty;
        string strCred = string.Empty;
       // string strLoginEmail = string.Empty;
        string strEmail = string.Empty;
        string strMobile = string.Empty;
        //string strPACSUserID = string.Empty;
        //string strPACSPwd = string.Empty;
        #endregion

        #region Properties
        public Guid INSTITUTION_ID
        {
            get { return InstId; }
            set { InstId = value; }
        }
        public Guid ID
        {
            get { return Id; }
            set { Id = value; }
        }
        public string NAME
        {
            get { return strName; }
            set { strName = value; }
        }
        public string FIRST_NAME
        {
            get { return strFName; }
            set { strFName = value; }
        }
        public string LAST_NAME
        {
            get { return strLName; }
            set { strLName = value; }
        }
        public string CREDENTIALS
        {
            get { return strCred; }
            set { strCred = value; }
        }
        public string EMAIL_ID
        {
            get { return strEmail; }
            set { strEmail = value; }
        }
        public string MOBILE_NUMBER
        {
            get { return strMobile; }
            set { strMobile = value; }
        }
        #endregion
    }

    public class InstitutionRegModalityLink
    {
        public int modality_id { get; set; }
        public Guid institution_id { get; set; }
    }
}
