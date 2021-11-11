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

namespace VETRIS.Settings
{
    [AjaxPro.AjaxNamespace("VRSUserDlg")]
    public partial class VRSUserDlg : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.Settings.User objCore = null;
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSUserDlg));
            if(!CallBackRights.CausedCallback)
                SetPageValue();
            SetAttributes();
        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {
            int intUserRoleID = Convert.ToInt32(Request.QueryString["urid"]);
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid UserID = new Guid(Request.QueryString["uid"]);
            Guid SessionID = new Guid(Request.QueryString["sid"]);
            string strTheme = Request.QueryString["th"];
            if (Request.QueryString["cf"] != null) hdnCF.Value = Request.QueryString["cf"];

            hdnID.Value = Request.QueryString["id"];
            LoadDetails(intMenuID, UserID, SessionID);
            SetCSS(strTheme);
        }
        #endregion

        #region SetCSS
        private void SetCSS(string strTheme)
        {
            string strServerPath = ConfigurationManager.AppSettings["ServerPath"];
            lnkSTYLE.Attributes["href"] = strServerPath + "/css/" + strTheme + "/style.css";
            lnkTHEME.Attributes["href"] = strServerPath + "/css/" + strTheme + "/theme.css";
            lnkTV.Attributes["href"] = strServerPath + "/css/" + strTheme + "/treeStyle.css";
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {
            btnAdd1.Attributes.Add("onclick", "javascript:btnNew_OnClick();");
            btnAdd2.Attributes.Add("onclick", "javascript:btnNew_OnClick();");
            btnSave1.Attributes.Add("onclick", "javascript:btnSave_OnClick();");
            btnSave2.Attributes.Add("onclick", "javascript:btnSave_OnClick();");
            btnReset1.Attributes.Add("onclick", "javascript:btnReset_OnClick();");
            btnReset2.Attributes.Add("onclick", "javascript:btnReset_OnClick();");
            btnClose1.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
            btnClose2.Attributes.Add("onclick", "javascript:btnClose_OnClick();");

            txtCode.Attributes.Add("onblur", "javascript:ConvertToUpperCase(this);");
            ddlRole.Attributes.Add("onchange", "javascript:ddlRole_OnChange();");
           
        }
        #endregion

        #region LoadDetails
        private void LoadDetails(int intMenuID, Guid UserID, Guid SessionID)
        {
            objCore = new Core.Settings.User();
            string strCatchMessage = ""; string strReturnMessage = string.Empty; bool bReturn = false;
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();

            try
            {
                objCore.ID = new Guid(hdnID.Value);
                objCore.MENU_ID = intMenuID;
                objCore.USER_ID = UserID;
                objCore.SESSION_ID = SessionID;

                bReturn = objCore.LoadDetails(Server.MapPath("~"), ref ds, ref strReturnMessage, ref strCatchMessage);

                if (bReturn)
                {

                    PopulateDropdowns(ds);
                    txtCode.Text = objCore.CODE;
                    txtName.Text = objCore.NAME; 
                    ddlRole.SelectedValue = objCore.ROLE_ID.ToString();
                    txtInstName.Text = objCore.INSTITUTION_NAME;
                    txtBAName.Text = objCore.BILLING_ACCOUNT_NAME;
                    txtEmailID.Text = objCore.EMAIL_ID;
                    txtContactNo.Text = objCore.CONTACT_NUMBER;
                    txtLoginID.Text = objCore.LOGIN_ID;
                    txtPwd.Text = objCore.LOGIN_PASSWORD;
                    txtPwd.Attributes.Add("value", objCore.LOGIN_PASSWORD);
                    txtPACSUserID.Text = objCore.PACS_USER_ID;
                    txtPACSPwd.Text = objCore.PACS_USER_PASSWORD;
                    txtPACSPwd.Attributes.Add("value", objCore.PACS_USER_PASSWORD);

                    if (objCore.ALLOW_MANUAL_SUBMISSION == "Y") rdoMSYes.Checked = true;
                    else if (objCore.ALLOW_MANUAL_SUBMISSION == "N") rdoMSNo.Checked = true;

                    if (objCore.ALLOW_DASHBOARD_VIEW == "Y") rdoDBYes.Checked = true;
                    else if (objCore.ALLOW_DASHBOARD_VIEW == "N") rdoDBNo.Checked = true;
                    
                    if (objCore.IS_ACTIVE == "Y") rdoStatYes.Checked = true;
                    else if (objCore.IS_ACTIVE == "N") rdoStatNo.Checked = true;

                    if ((objCore.ROLE_CODE == "IU") || (objCore.ROLE_CODE == "AU") || (objCore.ROLE_CODE == "RDL") || (objCore.ROLE_CODE == "SALES"))
                    {
                        txtName.ReadOnly = true;
                        ddlRole.Enabled = false;
                        txtEmailID.ReadOnly = true;
                        txtContactNo.ReadOnly = true;
                        txtLoginID.ReadOnly = true;
                        txtPwd.ReadOnly = true;
                        txtPACSUserID.ReadOnly = true;
                        txtPACSPwd.ReadOnly = true;
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
                ds.Dispose(); objCore = null; objComm = null;
            }
        }
        #endregion

        #region PopulateDropdowns
        private void PopulateDropdowns(DataSet ds)
        {
            #region UserRoles
            DataRow dr1 = ds.Tables["UserRoles"].NewRow();
            dr1["id"] = 0;
            dr1["name"] = "Select One";
            ds.Tables["UserRoles"].Rows.InsertAt(dr1, 0);
            ds.Tables["UserRoles"].AcceptChanges();

            ddlRole.DataSource = ds.Tables["UserRoles"];
            ddlRole.DataValueField = "id";
            ddlRole.DataTextField = "name";
            ddlRole.DataBind();
            #endregion

        }
        #endregion

        #region CallBackRights_CallBack
        protected void CallBackRights_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            string strAction = e.Parameters[0];
            switch (strAction)
            {
                case "U":
                    FetchUserAccessRights(e.Parameters);
                    break;
                case "UR":
                    FetchUserRoleAccessRights(e.Parameters);
                    break;
            }
            
            divRights.RenderControl(e.Output);
        }
        #endregion

        #region FetchUserAccessRights
        private void FetchUserAccessRights(string[] arrParams)
        {
            objCore = new Core.Settings.User();
            string strCatchMessage = ""; bool bReturn = false; string strReturnMessage = string.Empty;
            int intCompanyID = 0;
            DataSet ds = new DataSet();

            try
            {
                objCore.ID =new Guid(arrParams[1]);

                bReturn = objCore.FetchUserAccessRights(Server.MapPath("~"), ref ds, ref strReturnMessage, ref strCatchMessage);

                if (bReturn)
                {
                    BuildMenuRightsTreeView(ds.Tables["Rights"]);
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
            finally
            {
                ds.Dispose(); objComm = null;
                objCore = null;
            }
        }
        #endregion

        #region FetchUserRoleAccessRights
        private void FetchUserRoleAccessRights(string[] arrParams)
        {
            objCore = new Core.Settings.User();
            string strCatchMessage = ""; bool bReturn = false; string strReturnMessage = string.Empty;
            int intCompanyID = 0;
            DataSet ds = new DataSet();

            try
            {
                objCore.ROLE_ID = Convert.ToInt32(arrParams[1]);

                bReturn = objCore.FetchUserRoleAccessRights(Server.MapPath("~"), ref ds, ref strReturnMessage, ref strCatchMessage);

                if (bReturn)
                {
                    BuildMenuRightsTreeView(ds.Tables["Rights"]);
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
            finally
            {
                ds.Dispose(); objComm = null;
                objCore = null;
            }
        }
        #endregion

        #region BuildMenuRightsTreeView
        private void BuildMenuRightsTreeView(DataTable dtblMenuRights)
        {
            tvRights.ImagesBaseUrl = "../images/";
            tvRights.NodeCssClass = "TreeNode";
            tvRights.SelectedNodeCssClass = "SelectedTreeNode";
            tvRights.HoverNodeCssClass = "HoverTreeNode";
            tvRights.LineImageWidth = 19;
            tvRights.LineImageHeight = 20;
            tvRights.DefaultImageWidth = 16;
            tvRights.DefaultImageHeight = 16;
            tvRights.ItemSpacing = 0;
            tvRights.NodeLabelPadding = 3;
            tvRights.ShowLines = true;
            tvRights.LineImagesFolderUrl = "../images/lines/";
            tvRights.EnableViewState = true;
            PopulateMenuRightsTreeView(dtblMenuRights);

        }

        #endregion

        #region PopulateMenuRightsTreeView
        private void PopulateMenuRightsTreeView(DataTable dtblMenuRights)
        {
            string strMenuID0 = string.Empty; string strMenuName0 = string.Empty;
            string strMenuID1 = string.Empty; string strMenuName1 = string.Empty;
            string strAssigned0 = string.Empty; string strAssigned1 = string.Empty;

            int intFnID = 0;

            try
            {
                ComponentArt.Web.UI.TreeViewNode modNode = new ComponentArt.Web.UI.TreeViewNode();

                modNode.Text = "Menus";
                modNode.ID = "X";
                modNode.ImageUrl = "menu.png";
                modNode.Checked = true;

                #region Menu Level 0
                if (dtblMenuRights.Rows.Count > 0)
                {
                    DataView dvLev0 = new DataView(dtblMenuRights);
                    dvLev0.RowFilter = "parent_id=0";

                    foreach (DataRow dr0 in dvLev0.ToTable().Rows)
                    {
                        ComponentArt.Web.UI.TreeViewNode l0Node = new ComponentArt.Web.UI.TreeViewNode();
                        strMenuID0 = Convert.ToString(dr0["menu_id"]);
                        strMenuName0 = Convert.ToString(dr0["menu_desc"]);
                        strAssigned0 = Convert.ToString(dr0["assigned"]);
                        l0Node.Text = strMenuName0;
                        l0Node.ID = strMenuID0;
                        l0Node.ImageUrl = "menu.png";
                        l0Node.ShowCheckBox = true;
                        if (strAssigned0 == "Y") l0Node.Checked = true;
                        l0Node.Attributes.Add("ntype", "MENU");

                        DataView dvLev1 = new DataView(dtblMenuRights);
                        dvLev1.RowFilter = "parent_id=" + strMenuID0;

                        if (dvLev1.ToTable().Rows.Count > 0)
                        {
                            #region Menu Level 1
                            foreach (DataRow dr1 in dvLev1.ToTable().Rows)
                            {
                                ComponentArt.Web.UI.TreeViewNode l1Node = new ComponentArt.Web.UI.TreeViewNode();
                                strMenuID1 = Convert.ToString(dr1["menu_id"]);
                                strMenuName1 = Convert.ToString(dr1["menu_desc"]);
                                strAssigned1 = Convert.ToString(dr1["assigned"]);
                                l1Node.Text = strMenuName1;
                                l1Node.ID = strMenuID1;
                                l1Node.ImageUrl = "menu.png";
                                l1Node.ShowCheckBox = true;
                                if (strAssigned1 == "Y") l1Node.Checked = true;
                                l1Node.Attributes.Add("ntype", "MENU");
                                l0Node.Nodes.Add(l1Node);
                                //l0Node.Expanded = true;
                            }
                            #endregion

                            if (strAssigned0 == "Y") l0Node.Expanded = true;
                        }
                        else
                            modNode.Nodes.Add(l0Node);

                        dvLev1.Dispose();
                        if (l0Node.Nodes.Count > 0)
                        {
                            modNode.Nodes.Add(l0Node);
                            modNode.Expanded = true;
                        }
                    }
                }
                #endregion

                if (modNode.Nodes.Count > 0) tvRights.Nodes.Add(modNode);


            }
            catch (Exception expErr)
            {
                Response.Write(expErr.Message);
            }
            finally
            { ; }
        }

        #endregion

        #region SaveRecord (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] SaveRecord(string[] ArrRecord, string[] ArrMenuList)
        {
            bool bReturn = false;
            string strReturnMsg = string.Empty;
            string strCatchMessage = string.Empty;
            VETRIS.Core.Settings.MenuList[] objMenuList = new VETRIS.Core.Settings.MenuList[0];
            int intListIndex = 0;
            string[] arrRet = new string[0];

            objCore = new Core.Settings.User();
            objComm = new classes.CommonClass();

            try
            {
                objCore.ID = new Guid(ArrRecord[0].Trim());
                objCore.CODE = ArrRecord[1].Trim();
                objCore.NAME = ArrRecord[2].Trim();
                objCore.IS_ACTIVE = ArrRecord[3].Trim();
                objCore.ROLE_ID   = Convert.ToInt32(ArrRecord[4]);
                objCore.EMAIL_ID = ArrRecord[5].Trim();
                objCore.CONTACT_NUMBER = ArrRecord[6].Trim();
                objCore.LOGIN_ID = ArrRecord[7].Trim();
                if (ArrRecord[8].Trim() != string.Empty) objCore.LOGIN_PASSWORD = CoreCommon.EncryptString(ArrRecord[8].Trim());
                else objCore.LOGIN_PASSWORD = string.Empty;
                objCore.PACS_USER_ID = ArrRecord[9].Trim();
                if (ArrRecord[10].Trim() != string.Empty) objCore.PACS_USER_PASSWORD = CoreCommon.EncryptString(ArrRecord[10].Trim());
                else objCore.PACS_USER_PASSWORD = string.Empty;
                objCore.ALLOW_MANUAL_SUBMISSION = ArrRecord[11].Trim();
                objCore.ALLOW_DASHBOARD_VIEW = ArrRecord[12].Trim();
                objCore.USER_ID = new Guid(ArrRecord[13].Trim());
                objCore.MENU_ID = Convert.ToInt32(ArrRecord[14]);
                objCore.SESSION_ID = new Guid(ArrRecord[15].Trim());

                if (ArrMenuList.Length > 0)
                {
                    objMenuList = new VETRIS.Core.Settings.MenuList[(ArrMenuList.Length)];
                    for (int i = 0; i < objMenuList.Length; i++)
                    {
                        objMenuList[i] = new VETRIS.Core.Settings.MenuList();
                        objMenuList[i].MENU_ID = Convert.ToInt32(ArrMenuList[intListIndex]);
                        intListIndex = intListIndex + 1;
                    }
                }

                intListIndex = 0;

                bReturn = objCore.SaveRecord(Server.MapPath("~"), objMenuList,ref strReturnMsg, ref strCatchMessage);

                if (bReturn)
                {
                    arrRet = new string[3];
                    arrRet[0] = "true";
                    arrRet[1] = strReturnMsg.Trim();
                    arrRet[2] = objCore.ID.ToString();
                }
                else
                {
                    if (strCatchMessage.Trim() != "")
                    {
                        arrRet = new string[2];
                        arrRet[0] = "catch";
                        arrRet[1] = strCatchMessage.Trim();
                    }
                    else
                    {
                        arrRet = new string[3];
                        arrRet[0] = "false";
                        arrRet[1] = strReturnMsg.Trim();
                        arrRet[2] = objCore.USER_NAME;
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
                objCore = null; objComm = null;
                strReturnMsg = null; strCatchMessage = null;
            }
            return arrRet;
        }
        #endregion
    }
}