using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using VETRIS.API.Core;

namespace VETRIS.API.Models
{
    public class Chat
    {
        #region Variables
        bool bResponseStatus = false;
        string strResponseCode = string.Empty;
        string strResponseMessage = string.Empty;
        private Guid UserID = Guid.Empty;
        private string strUserName = string.Empty;
        private string strUserRoleCode = string.Empty;
        private string strUserRoleDesc = string.Empty;
        private string strEmailID = string.Empty;
        private string strContactNo = string.Empty;
        #endregion

        #region Properties
        public Guid USER_ID
        {
            get { return UserID; }
            set { UserID = value; }
        }
        public string USER_ROLE_CODE
        {
            get { return strUserRoleCode; }
            set { strUserRoleCode = value; }
        }
        public string USER_NAME
        {
            get { return strUserName; }
            set { strUserName = value; }
        }
        public string USER_ROLE_DESCRIPTION
        {
            get { return strUserRoleDesc; }
            set { strUserRoleDesc = value; }
        }
        public string EMAIL_ID
        {
            get { return strEmailID; }
            set { strEmailID = value; }
        }
        public string CONTACT_NUMBER
        {
            get { return strContactNo; }
            set { strContactNo = value; }
        }
        public string RESPONSE_MESSAGE
        {
            get { return strResponseMessage; }
            set { strResponseMessage = value; }
        }
        public bool RESPONSE_STATUS
        {
            get { return bResponseStatus; }
            set { bResponseStatus = value; }
        }
        #endregion

        
        #region FetchUserDetails
        public bool FetchUserDetails()
        {
            bool bReturn = false;
            string strReturnMsg = string.Empty;
            string strCatchMsg = string.Empty;
            VETRIS.API.Core.CHAT.Chat objCore = new Core.CHAT.Chat();

            try
            {
                objCore.USER_ID = UserID;
                bReturn = objCore.FetchUserDetails(AppDomain.CurrentDomain.BaseDirectory, ref strReturnMsg, ref strCatchMsg);

                if(bReturn)
                {
                    strUserRoleCode = objCore.USER_ROLE_CODE;
                    strUserRoleDesc = objCore.USER_ROLE_DESCRIPTION;
                    strUserName = objCore.USER_NAME;
                    strEmailID = objCore.EMAIL_ID;
                    strContactNo = objCore.CONTACT_NUMBER;
                }
               
                if (strCatchMsg.Trim() == string.Empty)
                    strResponseMessage = strReturnMsg;
                else
                    strResponseMessage = strCatchMsg;
            }
            catch (Exception expr)
            {
                bReturn = false;

                //LsResponseCode = "ERR";
                strResponseMessage = expr.Message;
            }
            finally
            { objCore = null; }

            bResponseStatus = bReturn;

            return bReturn;
        }
        #endregion


    }
}