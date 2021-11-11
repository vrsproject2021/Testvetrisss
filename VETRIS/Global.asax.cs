using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using VETRIS.Core.MyPayments;

namespace VETRIS
{
    public class Global : System.Web.HttpApplication
    {

        public static string Google_Translate_Api_Key = "";
        public static string TokenizationKey = "";
        public static string API_Key="";
        public static string Transaction_Gateway_Url="";
        protected void Application_Start(object sender, EventArgs e)
        {
            try
            {
                ARPayments objCore = new ARPayments();
                string retMsg = "", catchMsg = "";
                var settings = objCore.GetPaymentGatewayParameters(Server.MapPath("~"), ref retMsg, ref catchMsg);
                Transaction_Gateway_Url = settings.PAYMENT_GATEWAY_TRANSACTION_URL;
                TokenizationKey = settings.TOKENIZATION_API_KEY;
                API_Key = settings.PAYMENT_GATEWAY_API_KEY;
                // 
                // var apikeys = objCore.GetAllSettingParameters(Server.MapPath("~"), ref retMsg, ref catchMsg);
                // read from settings
                //
                Google_Translate_Api_Key = "AIzaSyBr4HHl8MQY1-qCmN7uDYEfK2T-R2EZMMw";
            }
            catch { }
        }

    }
}