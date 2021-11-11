using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.IO;
using System.Configuration;
using VETRIS.Core;
using AjaxPro;


namespace VETRIS.Invoicing
{
    [AjaxPro.AjaxNamespace("VRSInvoiceParams")]
    public partial class VRSInvoiceParams : System.Web.UI.Page
    {

        #region Members & Variables
        VETRIS.Core.Invoicing.BillCycle objCoreBillCycle = null;
        VETRIS.Core.Invoicing.InvoiceParams objCore = null;
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSInvoiceParams));
            SetAttributes();
            SetPageValue();
        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {
            int intUserRoleID = Convert.ToInt32(Request.QueryString["urid"]);
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid UserID = new Guid(Request.QueryString["uid"]);
            string strTheme = Request.QueryString["th"];

            hdnID.Value = Request.QueryString["id"];
            objComm = new classes.CommonClass();
            objComm.SetRegionalFormat();
            objComm = null;

            LoadRecord(intUserRoleID, intMenuID, UserID);

            SetCSS(strTheme);
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {

            btnSave1.Attributes.Add("onclick", "javascript:btnSave_OnClick();");
            btnSave2.Attributes.Add("onclick", "javascript:btnSave_OnClick();");
            btnClose1.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
            btnClose2.Attributes.Add("onclick", "javascript:btnClose_OnClick();");

            txtDUEDTDAYS.Attributes.Add("onkeypress", "javascript:return parent.CheckInteger(event);");
            txtDUEDTDAYS.Attributes.Add("onblur", "javascript:ResetValueInteger(this,'0');");
            txtSTARTINVSRL.Attributes.Add("onkeypress", "javascript:return parent.CheckInteger(event);");
            txtSTARTINVSRL.Attributes.Add("onblur", "javascript:ResetValueInteger(this,'0');");
            txtCALCMINUTEFACT.Attributes.Add("onkeypress", "javascript:return parent.CheckInteger(event);");
            txtCALCMINUTEFACT.Attributes.Add("onblur", "javascript:ResetValueInteger(this,'0');");

        }
        #endregion

        #region LoadRecord
        private void LoadRecord(int intUserRoleID, int intMenuID, Guid UserId)
        {
            objCore = new VETRIS.Core.Invoicing.InvoiceParams();
            string strCatchMessage = ""; 
            string strReturnMessage = string.Empty; 
            bool bReturn = false;
            int intFnID = 0;

            string strControlCode = string.Empty;
            string strUIPrefix = string.Empty; 
            string strControlValue = string.Empty;
            int intControlValue = 0; 
            double dblControlValue = 0; 
            DateTime dtControlValue = DateTime.Today;
            string strPMSVal = string.Empty;
            string strHelptext = string.Empty; 
            string strValuetype = string.Empty;


            DataSet ds = new DataSet();
            StringBuilder sb = new StringBuilder();
            objComm = new classes.CommonClass();
            objComm.SetRegionalFormat();

            try
            {

                objCore.USER_ROLE_ID = intUserRoleID;
                objCore.MENU_ID = intMenuID;
                objCore.USER_ID = UserId;


                bReturn = objCore.LoadRecord(Server.MapPath("~"), ref ds, ref strReturnMessage, ref strCatchMessage);

                if (bReturn)
                {
                    foreach (DataRow dr in ds.Tables["Parameters"].Rows)
                    {
                        strControlCode = Convert.ToString(dr["control_code"]).Trim();
                        strControlValue = Convert.ToString(dr["data_value_char"]).Trim();
                        intControlValue = Convert.ToInt32(dr["data_value_int"]);
                        dblControlValue = Convert.ToDouble(dr["data_value_dec"]);
                        strUIPrefix = Convert.ToString(dr["ui_prefix"]).Trim();
                        strValuetype = Convert.ToString(dr["value_type"]).Trim();

                        switch (strUIPrefix)
                        {
                            
                            
                            case "txt":
                                TextBox txt = (TextBox)this.Page.FindControl(strUIPrefix + strControlCode);
                                if (txt.TextMode == TextBoxMode.Password)
                                {
                                    if ((Convert.ToString(dr["data_value_char"]).Trim() != ""))
                                    {
                                        txt.Attributes.Add("value", CoreCommon.DecryptString(Convert.ToString(dr["data_value_char"]).Trim()));
                                        strControlValue = CoreCommon.DecryptString(strControlValue.Trim());
                                    }
                                    else
                                        txt.Attributes.Add("value", string.Empty);
                                }
                                else if (txt.TextMode == TextBoxMode.MultiLine)
                                {
                                    if ((Convert.ToString(dr["data_value_char"]).Trim() != ""))
                                    {
                                        txt.Text = Convert.ToString(dr["data_value_char"]).Trim();
                                        strControlValue = Convert.ToString(dr["data_value_char"]).Trim();
                                    }
                                }
                                if (strValuetype == "CHAR")
                                {
                                    if (strControlValue.Trim() == "")
                                    {
                                        txt.Attributes.Add("value", "");
                                    }
                                    else
                                    {
                                        txt.Attributes.Add("value", strControlValue);
                                    }

                                }
                                else if (strValuetype == "DESC")
                                {
                                    //switch (strControlCode)
                                    //{
                                    //    case "MAXRMBPPASSAMTVAR":
                                    //        strControlValue = objComm.IMNumeric(dblControlValue, objComm.DecimalDigit).Replace(",", "");
                                    //        txt.Attributes.Add("onblur", "javascript:ResetValueDecimal(this," + objComm.DecimalDigit.ToString() + ");");
                                    //        break;
                                    //    default:
                                    //        break;
                                    //}
                                    txt.Attributes.Add("value", dblControlValue.ToString());
                                    txt.Attributes.Add("onkeypress", "javascript:return parent.CheckDecimal(event);");
                                    txt.Attributes.Add("onblur", "javascript:ResetValueDecimal(this," + objComm.DecimalDigit.ToString() + ");");
                                    txt.Attributes.Add("onfocus", "javascript:this.select();");

                                }
                                else if (strValuetype == "INT")
                                {
                                    txt.Attributes.Add("value", intControlValue.ToString());
                                    txt.Attributes.Add("onkeypress", "javascript:return parent.CheckInteger(event);");
                                    txt.Attributes.Add("onfocus", "javascript:this.select();");

                                }
                                else
                                {

                                    if (strControlValue.Trim() == "")
                                        txt.Text = Convert.ToString(strControlValue);
                                    else
                                        txt.Text = strControlValue.Trim();
                                }
                                break;
                            case "hdn":
                                System.Web.UI.HtmlControls.HtmlInputHidden hdn = (System.Web.UI.HtmlControls.HtmlInputHidden)this.Page.FindControl(strUIPrefix + strControlCode);
                                if (strValuetype == "CHAR")
                                {
                                    if (strControlValue.Trim() == "")
                                    {
                                        hdn.Attributes.Add("value", "");
                                    }
                                    else
                                    {
                                        hdn.Attributes.Add("value", strControlValue);
                                    }

                                }
                                break;
                            case "rtb":
                                CKEditor.NET.CKEditorControl rtb = (CKEditor.NET.CKEditorControl)this.Page.FindControl(strUIPrefix + strControlCode);
                                if (strValuetype == "CHAR")
                                {
                                    rtb.Text = strControlValue;

                                }
                                break;
                            
                        }

                        
                    }

                }
                else
                {
                    if (strCatchMessage.Trim() != string.Empty)
                        hdnError.Value = "catch" + objComm.RecordDivider + strCatchMessage.Trim();
                    else
                        hdnError.Value = "false" + objComm.RecordDivider + strReturnMessage.Trim();
                }


            }
            catch (Exception ex)
            {
                hdnError.Value = "catch" + objComm.RecordDivider + ex.Message.Trim();
            }
            finally
            {
                ds.Dispose(); objComm = null; objCore = null;
            }
        }
        #endregion

        #region SetCSS
        private void SetCSS(string strTheme)
        {
            string strServerPath = ConfigurationManager.AppSettings["ServerPath"];
            lnkSTYLE.Attributes["href"] = strServerPath + "/css/" + strTheme + "/style.css";
            rtbINVMAILTXT.ContentsCss = strServerPath + "/css/" + strTheme + "/contents.css";
            

        }
        #endregion

        #region SaveRecord (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] SaveRecord(string[] ArrRecord, string[] ArrControlList)
        {
            bool bReturn = false;
            string[] arrRet = new string[3];
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty;
            int intCompanyID = 0;
           VETRIS.Core.Invoicing.ControlList[] objControlList = new Core.Invoicing.ControlList[0];
            int intListIndex = 0;
            objComm = new classes.CommonClass();
            objCore = new VETRIS.Core.Invoicing.InvoiceParams();


            try
            {
                objCore.USER_ID = new Guid(ArrRecord[0]);
                objCore.MENU_ID = Convert.ToInt32(ArrRecord[1]);
                if (ArrControlList[31].Trim() != string.Empty)
                {
                    ArrControlList[31] = CoreCommon.EncryptString(ArrControlList[31].Trim());
                }
                if (ArrControlList[79].Trim() != string.Empty)
                {
                    ArrControlList[79] = CoreCommon.EncryptString(ArrControlList[79].Trim());
                }
                
                objControlList = new Core.Invoicing.ControlList[(ArrControlList.Length / 6)];

                for (int i = 0; i < objControlList.Length; i++)
                {
                    objControlList[i] = new Core.Invoicing.ControlList();
                    objControlList[i].CONTROL_CODE = ArrControlList[intListIndex].Trim();
                    objControlList[i].DATA_VALUE_CHAR = ArrControlList[intListIndex + 1].Trim();
                    objControlList[i].DATA_VALUE_INT = Convert.ToInt32(ArrControlList[intListIndex + 2]);
                    objControlList[i].DATA_VALUE_DEC = Convert.ToDecimal(ArrControlList[intListIndex + 3]);
                    objControlList[i].VALUE_TYPE = ArrControlList[intListIndex + 4];
                    objControlList[i].UI_PREFIX = ArrControlList[intListIndex + 5];
                    intListIndex = intListIndex + 6;
                }

                bReturn = objCore.SaveRecord(Server.MapPath("~"), objControlList, ref strReturnMsg, ref strCatchMessage);

                if (bReturn)
                {

                    arrRet[0] = "true";
                    arrRet[1] = strReturnMsg.Trim();
                }
                else
                {

                    if (strCatchMessage.Trim() != "")
                    {
                        arrRet[0] = "catch";
                        arrRet[1] = strCatchMessage.Trim();

                    }
                    else
                    {
                        arrRet[0] = "false";
                        arrRet[1] = strReturnMsg.Trim();
                        arrRet[2] = objCore.USER_NAME;
                    }
                }
            }
            catch (Exception expErr)
            {
                bReturn = false;
                arrRet[0] = "catch";
                arrRet[1] = expErr.Message.Trim();
            }
            finally
            {
                objCore = null; objComm = null;
                strReturnMsg = null; strCatchMessage = null;
            }
            return arrRet;
        }
        #endregion 

    }
}