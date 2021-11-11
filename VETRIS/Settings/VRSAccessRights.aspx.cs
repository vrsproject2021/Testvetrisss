using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Text;
using System.Data;
using AjaxPro;
using VETRIS.Core;

namespace VETRIS.Settings
{
    [AjaxPro.AjaxNamespace("VRSAccessRights")]
    public partial class VRSAccessRights : System.Web.UI.Page
    {
        #region Members & Variables
        classes.CommonClass objComm;
        VETRIS.Core.Settings.AccessRights objCore;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSAccessRights));
            if ((!CallBackUserRole.CausedCallback)
                && (!CallBackRights.CausedCallback)
                && (!CallBackAssign.CausedCallback))
                SetPageValue();
            SetAttributes();
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            btnSave1.Attributes.Add("onclick", "javascript:btnSave_OnClick();");
            btnSave2.Attributes.Add("onclick", "javascript:btnSave_OnClick();");
            btnReset1.Attributes.Add("onclick", "javascript:btnReset_OnClick();");
            btnReset2.Attributes.Add("onclick", "javascript:btnReset_OnClick();");
            btnClose1.Attributes.Add("onclick", "javascript:btnBrwClose_Onclick();");
            btnClose2.Attributes.Add("onclick", "javascript:btnBrwClose_Onclick();");
        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {
            int intUserRoleID = Convert.ToInt32(Request.QueryString["urid"]);
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid user_id = new Guid(Request.QueryString["uid"]);
            string strTheme = Request.QueryString["th"];
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

        #region CallBackUserRole_Callback
        protected void CallBackUserRole_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {

            FetchUserRoles();
            divUserRole.RenderControl(e.Output);
        }
        #endregion

        #region FetchUserRoles
        private void FetchUserRoles()
        {
            objCore = new VETRIS.Core.Settings.AccessRights();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();

            try
            {
                bReturn = objCore.FetchUserRoles(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {
                    BuildUserRoleTreeView(ds.Tables["BrowserList"]);
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
            finally
            {
                ds.Dispose(); objComm = null;
                //objCore = null;
            }
        }
        #endregion

        #region User Role Tree

        #region BuildUserRoleTreeView
        private void BuildUserRoleTreeView(DataTable dtbl)
        {
            tvUserRoles.ImagesBaseUrl = "../images/";
            tvUserRoles.NodeCssClass = "TreeNode";
            tvUserRoles.SelectedNodeCssClass = "SelectedTreeNode";
            tvUserRoles.HoverNodeCssClass = "HoverTreeNode";
            tvUserRoles.LineImageWidth = 19;
            tvUserRoles.LineImageHeight = 20;
            tvUserRoles.DefaultImageWidth = 16;
            tvUserRoles.DefaultImageHeight = 16;
            tvUserRoles.ItemSpacing = 0;
            tvUserRoles.NodeLabelPadding = 3;
            tvUserRoles.ShowLines = true;
            tvUserRoles.LineImagesFolderUrl = "../images/lines/";
            tvUserRoles.EnableViewState = true;
            PopulateUserRoleTreeView(dtbl);

        }

        #endregion

        #region PopulateUserRoleTreeView
        private void PopulateUserRoleTreeView(DataTable dtbl)
        {
            DataView dvRole = new DataView(dtbl);
            try
            {
                if (dtbl.Rows.Count > 0)
                {
                    ComponentArt.Web.UI.TreeViewNode modNode = new ComponentArt.Web.UI.TreeViewNode();
                    modNode.Text = "User Roles";
                    modNode.ID = "0";
                    modNode.ImageUrl = "usergroup.png";

                    for (int intI = 0; intI < dtbl.Rows.Count; intI++)
                    {
                        ComponentArt.Web.UI.TreeViewNode ugNode = new ComponentArt.Web.UI.TreeViewNode();
                        ugNode.Text = Convert.ToString(dtbl.Rows[intI]["name"]);
                        ugNode.ID = Convert.ToString(dtbl.Rows[intI]["id"]);
                        ugNode.ImageUrl = "usergroup.png";
                        modNode.Nodes.Add(ugNode);
                    }
                    tvUserRoles.Nodes.Add(modNode);
                    modNode.Expanded = true;

                }
            }
            catch (Exception expErr)
            {
                Response.Write(expErr.Message);
            }
        }

        #endregion

        #endregion

        #region CallBackRights_CallBack
        protected void CallBackRights_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {

            FetchAccessRights(e.Parameters);
            divRights.RenderControl(e.Output);
        }
        #endregion

        #region FetchAccessRights
        private void FetchAccessRights(string[] arrParams)
        {
            objCore = new VETRIS.Core.Settings.AccessRights();
            string strCatchMessage = ""; bool bReturn = false; string strReturnMessage = string.Empty;
            int intCompanyID = 0;
            DataSet ds = new DataSet();

            try
            {
                objCore.ID = Convert.ToInt32(arrParams[0]);
                objCore.MENU_ID = Convert.ToInt32(arrParams[1]);
                objCore.USER_ID = new Guid(arrParams[2]);

                bReturn = objCore.FetchAccessRights(Server.MapPath("~"),  ref ds, ref strReturnMessage, ref strCatchMessage);

                if (bReturn)
                {
                    BuildMenuRightsTreeView(ds.Tables["MenuRights"]);
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

        #region Menu Rights Tree

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

            int intFnID = 0;

            try
            {
                ComponentArt.Web.UI.TreeViewNode modNode = new ComponentArt.Web.UI.TreeViewNode();

                modNode.Text = "Menus";
                modNode.ID = "X";
                modNode.ImageUrl = "menu.png";


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
                        l0Node.Text = strMenuName0;
                        l0Node.ID = strMenuID0;
                        l0Node.ImageUrl = "menu.png";
                        l0Node.ShowCheckBox = true;
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
                                l1Node.Text = strMenuName1;
                                l1Node.ID = strMenuID1;
                                l1Node.ImageUrl = "menu.png";
                                l1Node.ShowCheckBox = true;
                                l1Node.Attributes.Add("ntype", "MENU");
                                l0Node.Nodes.Add(l1Node);
                                //l0Node.Expanded = true;
                            }
                            #endregion
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


        #endregion


        #region CallBackAssign_CallBack
        protected void CallBackAssign_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {

            FetchAssignedRights(e.Parameters);
            divAssigned.RenderControl(e.Output);
        }
        #endregion

        #region FetchAssignedRights
        private void FetchAssignedRights(string[] arrParams)
        {
            objCore = new VETRIS.Core.Settings.AccessRights();
            string strCatchMessage = ""; bool bReturn = false; string strReturnMessage = string.Empty;
            int intCompanyID = 0;
            DataSet ds = new DataSet();

            try
            {
                objCore.ID = Convert.ToInt32(arrParams[0]);
                objCore.MENU_ID = Convert.ToInt32(arrParams[1]);
                objCore.USER_ID = new Guid(arrParams[2]);

                bReturn = objCore.FetchAssignRights(Server.MapPath("~"),  ref ds, ref strReturnMessage, ref strCatchMessage);
                if (bReturn)
                {

                    BuildAssignedRightsTreeView(ds.Tables["MenuRights"]);

                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
            finally
            {
                ds.Dispose(); objComm = null;
                //objCore = null;
            }
        }
        #endregion

        #region Assigned Rights Tree


        #region BuildAssignedRightsTreeView
        private void BuildAssignedRightsTreeView(DataTable dtblMenuRights)
        {
            tvAssignedRights.ImagesBaseUrl = "../images/";
            tvAssignedRights.NodeCssClass = "TreeNode";
            tvAssignedRights.SelectedNodeCssClass = "SelectedTreeNode";
            tvAssignedRights.HoverNodeCssClass = "HoverTreeNode";
            tvAssignedRights.LineImageWidth = 19;
            tvAssignedRights.LineImageHeight = 20;
            tvAssignedRights.DefaultImageWidth = 16;
            tvAssignedRights.DefaultImageHeight = 16;
            tvAssignedRights.ItemSpacing = 0;
            tvAssignedRights.NodeLabelPadding = 3;
            tvAssignedRights.ShowLines = true;
            tvAssignedRights.LineImagesFolderUrl = "../images/lines/";
            tvAssignedRights.EnableViewState = true;
            PopulateAssignedRightsTreeView(dtblMenuRights);

        }

        #endregion

        #region PopulateAssignedRightsTreeView
        private void PopulateAssignedRightsTreeView(DataTable dtblMenuRights)
        {
            string strMenuID0 = string.Empty; string strMenuName0 = string.Empty;
            string strMenuID1 = string.Empty; string strMenuName1 = string.Empty;

            int intFnID = 0;

            try
            {
                ComponentArt.Web.UI.TreeViewNode modNode = new ComponentArt.Web.UI.TreeViewNode();

                modNode.Text = "Menu & Functions";
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
                        l0Node.Text = strMenuName0;
                        l0Node.ID = strMenuID0;
                        l0Node.ImageUrl = "menu.png";
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
                                l1Node.Text = strMenuName1;
                                l1Node.ID = strMenuID1;
                                l1Node.ImageUrl = "menu.png";
                                l1Node.Attributes.Add("ntype", "MENU");
                                l0Node.Nodes.Add(l1Node);
                                //l0Node.Expanded = true;
                            }
                            #endregion
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

                if (modNode.Nodes.Count > 0)
                    //   tvRights.Nodes.Add(modNode);
                    tvAssignedRights.Nodes.Add(modNode);


            }
            catch (Exception expErr)
            {
                Response.Write(expErr.Message);
            }
            finally
            { ; }
        }

        #endregion

        #endregion

        #region SaveRecord (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] SaveRecord(string[] ArrRecord, string[] ArrMenuList)
        {
            bool bReturn = false;
            string[] arrRet = new string[3];
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty;
            VETRIS.Core.Settings.MenuList[] objMenuList = new VETRIS.Core.Settings.MenuList[0];
            int intListIndex = 0;
            objComm = new classes.CommonClass();
            objCore = new VETRIS.Core.Settings.AccessRights();


            try
            {
                objCore.ID = Convert.ToInt32(ArrRecord[0]);
                objCore.USER_ID = new Guid(ArrRecord[1]);
                objCore.MENU_ID = Convert.ToInt32(ArrRecord[2]);

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


                bReturn = objCore.SaveRecord(Server.MapPath("~"),  objMenuList, ref strReturnMsg, ref strCatchMessage);

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
                objCore = null; objComm = null; objMenuList = null;
                strReturnMsg = null; strCatchMessage = null;
            }
            return arrRet;
        }
        #endregion

        #region DeleteRecord (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] DeleteRecord(string[] ArrRecord)
        {
            bool bReturn = false;
            string[] arrRet = new string[3];
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty;
            objComm = new classes.CommonClass();
            objCore = new VETRIS.Core.Settings.AccessRights();


            try
            {
                objCore.ID = Convert.ToInt32(ArrRecord[0]);
                objCore.DELETED_MODULE_ID = Convert.ToInt32(ArrRecord[1]);
                objCore.DELETED_MENU_ID = Convert.ToInt32(ArrRecord[2]);
                objCore.USER_ID = new Guid(ArrRecord[3]);
                objCore.MENU_ID = Convert.ToInt32(ArrRecord[4]);

                bReturn = objCore.DeleteRecord(Server.MapPath("~"),  ref strReturnMsg, ref strCatchMessage);

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