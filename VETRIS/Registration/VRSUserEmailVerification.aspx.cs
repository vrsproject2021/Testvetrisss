using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.IO;
using System.Net;
using System.Configuration;
using VETRIS.Core;
using AjaxPro;

namespace VETRIS.Registration
{
    public partial class VRSUserEmailVerification : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.Master.Institution objCore = null;
        classes.CommonClass objComm;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageValue();
        }


        #region SetPageValue
        private void SetPageValue()
        {
            //int intUserRoleID = Convert.ToInt32(Request.QueryString["urid"]);
            //int intMenuID = 2;
            //Guid UserID = new Guid("11111111-1111-1111-1111-111111111111");
            Guid InstitutionID = new Guid(Convert.ToString(Request.QueryString["code"]));
            //hdnID.Value = Request.QueryString["id"];
            VerifyUserDetails(InstitutionID);
        }
        #endregion

        #region Verify Email 
        
        public string[] VerifyUserDetails(Guid id)
        {
            bool bReturn = false;
            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty;
            string strCatchMessage = string.Empty;
            int intListIndex = 0;

            string url = Request.Url.Host;
            string url1 = Request.Url.AbsoluteUri;
            string url2 = ConfigurationManager.AppSettings["ServerPath"]; 

            objCore = new Core.Master.Institution();
            objComm = new classes.CommonClass();

            try
            {
                objCore.ID = id;
                
                intListIndex = 0;


                bReturn = objCore.VerifyEmail(Server.MapPath("~"), ref strReturnMsg, ref strCatchMessage);

                if (bReturn)
                {
                   
                    arrRet = new string[2];
                    arrRet[0] = "true";
                    arrRet[1] = strReturnMsg.Trim();

                    lblStatus.InnerText = "Your Email ID has been Verified and Account activation completed";
                }
                else
                {
                    lblStatus.InnerText = "Your Email ID is not authenticated and Account activation not completed";
                    if (strCatchMessage.Trim() != "")
                    {
                        arrRet = new string[2];
                        arrRet[0] = "catch";
                        arrRet[1] = strCatchMessage.Trim();
                    }
                    else
                    {
                        arrRet = new string[2];
                        arrRet[0] = "false";
                        arrRet[1] = strReturnMsg.Trim();
                    }
                }
            }
            catch (Exception expErr)
            {
                bReturn = false;
                arrRet = new string[2];
                arrRet[0] = "catch";
                arrRet[1] = expErr.Message.Trim();
            }
            finally
            {
                objCore = null; 
                objComm = null;
                strReturnMsg = null; 
                strCatchMessage = null;
            }
            return arrRet;
        }
        #endregion
    }
}