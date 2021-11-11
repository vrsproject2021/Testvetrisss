using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Configuration;
using AjaxPro;
using VETRIS.Core;

namespace VETRIS.Settings
{
    [AjaxPro.AjaxNamespace("VRSGlCodeMap")]
    public partial class VRSGlCodeMap : System.Web.UI.Page
    {
        #region Members & Variables
        classes.CommonClass objComm;
        VETRIS.Core.Settings.GLCodeMapping objCore;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSGlCodeMap));
            if ((!CallBackModality.CausedCallback)
                && (!CallBackService.CausedCallback)
                && (!CallBackNRH.CausedCallback)
                && (!CallBackRC.CausedCallback))
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
            btnClose1.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
            btnClose2.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
            btnAddModality.Attributes.Add("onclick", "javascript:btnAddModality_OnClick();");
            btnAddService.Attributes.Add("onclick", "javascript:btnAddService_OnClick();");
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
            LoadData(intMenuID,user_id);
        }
        #endregion

        #region SetCSS
        private void SetCSS(string strTheme)
        {
            string strServerPath = ConfigurationManager.AppSettings["ServerPath"];
            lnkSTYLE.Attributes["href"] = strServerPath + "/css/" + strTheme + "/style.css";
            lnkGRID.Attributes["href"] = strServerPath + "/css/" + strTheme + "/grid_style.css";
        }
        #endregion

        #region LoadData
        private void LoadData(int intMenuID, Guid UserID)
        {
            objCore = new Core.Settings.GLCodeMapping();
            string strCatchMessage = ""; string strReturnMessage = string.Empty; bool bReturn = false;
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();

            try
            {
                objComm.SetRegionalFormat();
                objCore.MENU_ID = intMenuID;
                objCore.USER_ID = UserID;

                bReturn = objCore.FetchGlMapping(Server.MapPath("~"), ref ds, ref strReturnMessage, ref strCatchMessage);

                if (bReturn)
                {

                    #region Modality
                    DataRow dr1 = ds.Tables["Modality"].NewRow();
                    dr1["id"] = "0";
                    dr1["name"] = "Select One";
                    ds.Tables["Modality"].Rows.InsertAt(dr1, 0);
                    ds.Tables["Modality"].AcceptChanges();

                    foreach (DataRow dr in ds.Tables["Modality"].Rows)
                    {
                        if (hdnModality.Value.Trim() != string.Empty) hdnModality.Value += objComm.RecordDivider;
                        hdnModality.Value += Convert.ToString(dr["id"]) + objComm.RecordDivider;
                        hdnModality.Value += Convert.ToString(dr["name"]).Trim();
                    }
                    #endregion

                    #region Category
                    DataRow dr2 = ds.Tables["Category"].NewRow();
                    dr2["id"] = "0";
                    dr2["name"] = "Select One";
                    ds.Tables["Category"].Rows.InsertAt(dr2, 0);
                    ds.Tables["Category"].AcceptChanges();

                    foreach (DataRow dr in ds.Tables["Category"].Rows)
                    {
                        if (hdnCategory.Value.Trim() != string.Empty) hdnCategory.Value += objComm.RecordDivider;
                        hdnCategory.Value += Convert.ToString(dr["id"]) + objComm.RecordDivider;
                        hdnCategory.Value += Convert.ToString(dr["name"]).Trim();
                    }
                    #endregion

                    #region Service
                    DataRow dr3 = ds.Tables["Services"].NewRow();
                    dr3["id"] = "0";
                    dr3["name"] = "Select One";
                    ds.Tables["Services"].Rows.InsertAt(dr3, 0);
                    ds.Tables["Services"].AcceptChanges();

                    foreach (DataRow dr in ds.Tables["Services"].Rows)
                    {
                        if (hdnService.Value.Trim() != string.Empty) hdnService.Value += objComm.RecordDivider;
                        hdnService.Value += Convert.ToString(dr["id"]) + objComm.RecordDivider;
                        hdnService.Value += Convert.ToString(dr["name"]).Trim();
                    }
                    #endregion

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

        #region Modality Grid

        #region CallBackModality_Callback
        protected void CallBackModality_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            string strAction = e.Parameters[0];

            switch (strAction)
            {
                case "L":
                    LoadModality();
                    break;
                case "A":
                    AddModality(e.Parameters);
                    break;
                case "D":
                    DeleteModality(e.Parameters);
                    break;
            }

            grdModality.Width = Unit.Percentage(100);
            grdModality.RenderControl(e.Output);
            spnErrMod.RenderControl(e.Output);
        }
        #endregion

        #region LoadModality
        private void LoadModality()
        {
            objCore = new Core.Settings.GLCodeMapping();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();

            try
            {

                bReturn = objCore.FetchModalities(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    grdModality.DataSource = ds.Tables["Modality"];
                    grdModality.DataBind();


                    spnErrMod.InnerHtml = "<input type=\"hidden\" id=\"hdnCBModErr\" value=\"\" />";
                }
                else
                    spnErrMod.InnerHtml = "<input type=\"hidden\" id=\"hdnCBModErr\" value=\"" + strCatchMessage + "\" />";



            }
            catch (Exception ex)
            {
                spnErrMod.InnerHtml = "<input type=\"hidden\" id=\"hdnCBModErr\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }
        }
        #endregion

        #region AddModality
        private void AddModality(string[] arrParams)
        {
            DataTable dtbl = new DataTable();
            objComm = new classes.CommonClass();
            string[] arrSep = { objComm.SecondaryRecordDivider };
            string[] arrRecords = arrParams[1].Split(arrSep, StringSplitOptions.None);

            int intSrl = 0;

            try
            {
                dtbl = CreateModalityTable();
                if (arrRecords.Length > 0)
                {
                    if (arrRecords[0].Trim() != "")
                    {
                        for (int i = 0; i < arrRecords.Length; i = i + 6)
                        {
                            DataRow dr = dtbl.NewRow();
                            intSrl = intSrl + 1;
                            dr["row_id"] = intSrl;
                            dr["category_id"] = Convert.ToInt32(arrRecords[i + 1].Trim());
                            dr["category_name"] = arrRecords[i + 2].Trim();
                            dr["modality_id"] = Convert.ToInt32(arrRecords[i + 3].Trim());
                            dr["modality_name"] = arrRecords[i + 4].Trim();
                            dr["gl_code"] = arrRecords[i + 5].Trim();
                            dtbl.Rows.Add(dr);
                        }

                    }
                }

                DataRow drNew = dtbl.NewRow();
                intSrl = intSrl + 1;
                drNew["row_id"] = intSrl;
                drNew["category_id"] = 0;
                drNew["category_name"] = "";
                drNew["modality_id"] = 0;
                drNew["modality_name"] = "";
                drNew["gl_code"] = "";
                dtbl.Rows.Add(drNew);

                grdModality.DataSource = dtbl;
                grdModality.DataBind();
                spnErrMod.InnerHtml = "<input type=\"hidden\" id=\"hdnCBModErr\" value=\"\" />";
            }
            catch (Exception ex)
            {
                spnErrMod.InnerHtml = "<input type=\"hidden\" id=\"hdnCBModErr\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                dtbl.Dispose();
                objComm = null;
            }
        }
        #endregion

        #region DeleteModality
        private void DeleteModality(string[] arrParams)
        {
            DataTable dtbl = new DataTable();
            objComm = new classes.CommonClass();
            int intID = Convert.ToInt32(arrParams[1]);
            string[] arrSep = { objComm.SecondaryRecordDivider };
            string[] arrRecords = arrParams[2].Split(arrSep, StringSplitOptions.None);
            int intSrl = 0;

            try
            {
                dtbl = CreateModalityTable();
                if (arrRecords[0].Trim() != "")
                {
                    for (int i = 0; i < arrRecords.Length; i = i + 6)
                    {
                        if (Convert.ToInt32(arrRecords[i]) != intID)
                        {
                            DataRow dr = dtbl.NewRow();
                            intSrl = intSrl + 1;
                            dr["row_id"] = intSrl;
                            dr["category_id"] = Convert.ToInt32(arrRecords[i + 1].Trim());
                            dr["category_name"] = arrRecords[i + 2].Trim();
                            dr["modality_id"] = Convert.ToInt32(arrRecords[i + 3].Trim());
                            dr["modality_name"] = arrRecords[i + 4].Trim();
                            dr["gl_code"] = arrRecords[i + 5].Trim();
                            dtbl.Rows.Add(dr);
                        }
                    }
                }

                grdModality.DataSource = dtbl;
                grdModality.DataBind();
                spnErrMod.InnerHtml = "<input type=\"hidden\" id=\"hdnCBModErr\" value=\"\" />";

            }
            catch (Exception ex)
            {
                spnErrMod.InnerHtml = "<input type=\"hidden\" id=\"hdnCBModErr\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                dtbl.Dispose();
                objComm = null;
            }
        }
        #endregion

        #region CreateModalityTable
        private DataTable CreateModalityTable()
        {
            DataTable dtbl = new DataTable();
            dtbl.Columns.Add("row_id", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("category_id", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("category_name", System.Type.GetType("System.String"));
            dtbl.Columns.Add("modality_id", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("modality_name", System.Type.GetType("System.String"));
            dtbl.Columns.Add("gl_code", System.Type.GetType("System.String"));
            dtbl.TableName = "Modality";
            return dtbl;
        }
        #endregion

        #endregion

        #region Service Grid

        #region CallBackService_Callback
        protected void CallBackService_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {

            string strAction = e.Parameters[0];

            switch (strAction)
            {
                case "L":
                    LoadService();
                    break;
                case "A":
                    AddService(e.Parameters);
                    break;
                case "D":
                    DeleteService(e.Parameters);
                    break;
            }
            grdService.Width = Unit.Percentage(100);
            grdService.RenderControl(e.Output);
            spnErrSvc.RenderControl(e.Output);
        }
        #endregion

        #region LoadService
        private void LoadService()
        {
            objCore = new Core.Settings.GLCodeMapping();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();

            try
            {

                bReturn = objCore.FetchServices(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    grdService.DataSource = ds.Tables["Service"];
                    grdService.DataBind();


                    spnErrSvc.InnerHtml = "<input type=\"hidden\" id=\"hdnCBSvcErr\" value=\"\" />";
                }
                else
                    spnErrSvc.InnerHtml = "<input type=\"hidden\" id=\"hdnCBSvcErr\" value=\"" + strCatchMessage + "\" />";



            }
            catch (Exception ex)
            {
                spnErrSvc.InnerHtml = "<input type=\"hidden\" id=\"hdnCBSvcErr\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }
        }
        #endregion

        #region AddService
        private void AddService(string[] arrParams)
        {
            DataTable dtbl = new DataTable();
            objComm = new classes.CommonClass();
            string[] arrSep = { objComm.SecondaryRecordDivider };
            string[] arrRecords = arrParams[1].Split(arrSep, StringSplitOptions.None);

            int intSrl = 0;

            try
            {
                dtbl = CreateServiceTable();
                if (arrRecords.Length > 0)
                {
                    if (arrRecords[0].Trim() != "")
                    {
                        for (int i = 0; i < arrRecords.Length; i = i + 5)
                        {
                            DataRow dr = dtbl.NewRow();
                            intSrl = intSrl + 1;
                            dr["row_id"] = intSrl;
                            dr["service_id"] = Convert.ToInt32(arrRecords[i + 1].Trim());
                            dr["modality_id"] = Convert.ToInt32(arrRecords[i + 2].Trim());
                            dr["gl_code_default"] = arrRecords[i + 3].Trim();
                            dr["gl_code_after_hrs"] = arrRecords[i + 4].Trim();
                            dtbl.Rows.Add(dr);
                        }

                    }
                }

                DataRow drNew = dtbl.NewRow();
                intSrl = intSrl + 1;
                drNew["row_id"] = intSrl;
                drNew["service_id"] = 0;
                drNew["modality_id"] = 0;
                drNew["gl_code_default"] = "";
                drNew["gl_code_after_hrs"] = "";
                dtbl.Rows.Add(drNew);

                grdService.DataSource = dtbl;
                grdService.DataBind();
                spnErrSvc.InnerHtml = "<input type=\"hidden\" id=\"hdnCBSvcErr\" value=\"\" />";
            }
            catch (Exception ex)
            {
                spnErrSvc.InnerHtml = "<input type=\"hidden\" id=\"hdnCBSvcErr\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                dtbl.Dispose();
                objComm = null;
            }
        }
        #endregion

        #region DeleteService
        private void DeleteService(string[] arrParams)
        {
            DataTable dtbl = new DataTable();
            objComm = new classes.CommonClass();
            int intID = Convert.ToInt32(arrParams[1]);
            string[] arrSep = { objComm.SecondaryRecordDivider };
            string[] arrRecords = arrParams[2].Split(arrSep, StringSplitOptions.None);
            int intSrl = 0;

            try
            {
                dtbl = CreateServiceTable();
                if (arrRecords[0].Trim() != "")
                {
                    for (int i = 0; i < arrRecords.Length; i = i + 5)
                    {
                        if (Convert.ToInt32(arrRecords[i]) != intID)
                        {
                            DataRow dr = dtbl.NewRow();
                            intSrl = intSrl + 1;
                            dr["row_id"] = intSrl;
                            dr["service_id"] = Convert.ToInt32(arrRecords[i + 1].Trim());
                            dr["modality_id"] = Convert.ToInt32(arrRecords[i + 2].Trim());
                            dr["gl_code_default"] = arrRecords[i + 3].Trim();
                            dr["gl_code_after_hrs"] = arrRecords[i + 4].Trim();
                            dtbl.Rows.Add(dr);
                        }
                    }
                }

                grdService.DataSource = dtbl;
                grdService.DataBind();
                spnErrSvc.InnerHtml = "<input type=\"hidden\" id=\"hdnCBSvcErr\" value=\"\" />";
            }
            catch (Exception ex)
            {
                spnErrSvc.InnerHtml = "<input type=\"hidden\" id=\"hdnCBSvcErr\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                dtbl.Dispose();
                objComm = null;
            }
        }
        #endregion

        #region CreateServiceTable
        private DataTable CreateServiceTable()
        {
            DataTable dtbl = new DataTable();
            dtbl.Columns.Add("row_id", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("service_id", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("modality_id", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("gl_code_default", System.Type.GetType("System.String"));
            dtbl.Columns.Add("gl_code_after_hrs", System.Type.GetType("System.String"));
            dtbl.TableName = "Services";
            return dtbl;
        }
        #endregion

        #endregion

        #region Other Account Head Grid

        #region CallBackNRH_Callback
        protected void CallBackNRH_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {

            LoadNonRevenueHead();
            grdNRH.Width = Unit.Percentage(100);
            grdNRH.RenderControl(e.Output);
            spnErrNRH.RenderControl(e.Output);
        }
        #endregion

        #region LoadNonRevenueHead
        private void LoadNonRevenueHead()
        {
            objCore = new Core.Settings.GLCodeMapping();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();

            try
            {

                bReturn = objCore.FetchNonRevenueHead(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    grdNRH.DataSource = ds.Tables["NonRevenueHead"];
                    grdNRH.DataBind();


                    spnErrNRH.InnerHtml = "<input type=\"hidden\" id=\"hdnCBNRHErr\" value=\"\" />";
                }
                else
                    spnErrNRH.InnerHtml = "<input type=\"hidden\" id=\"hdnCBNRHErr\" value=\"" + strCatchMessage + "\" />";



            }
            catch (Exception ex)
            {
                spnErrNRH.InnerHtml = "<input type=\"hidden\" id=\"hdnCBNRHErr\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }
        }
        #endregion

        #endregion

        #region Radiologist Charges Grid

        #region CallBackRC_Callback
        protected void CallBackRC_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {

            LoadRadiologistCharges();
            grdRC.Width = Unit.Percentage(100);
            grdRC.RenderControl(e.Output);
            spnErrRC.RenderControl(e.Output);
        }
        #endregion

        #region LoadRadiologistCharges
        private void LoadRadiologistCharges()
        {
            objCore = new Core.Settings.GLCodeMapping();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();

            try
            {

                bReturn = objCore.FetchRadiologistCharges(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    grdRC.DataSource = ds.Tables["RadiologistCharge"];
                    grdRC.DataBind();


                    spnErrRC.InnerHtml = "<input type=\"hidden\" id=\"hdnCBRCErr\" value=\"\" />";
                }
                else
                    spnErrRC.InnerHtml = "<input type=\"hidden\" id=\"hdnCBRCErr\" value=\"" + strCatchMessage + "\" />";



            }
            catch (Exception ex)
            {
                spnErrRC.InnerHtml = "<input type=\"hidden\" id=\"hdnCBRCErr\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }
        }
        #endregion

        #endregion

        #region SaveRecord (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] SaveRecord(string[] ArrRecord, string[] ArrModalityList, string[] ArrServiceList, string[] ArrNRHList, string[] ArrRCList)
        {
            bool bReturn = false;
            string[] arrRet = new string[3];
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty;
            VETRIS.Core.Settings.GLModalityList[] objGLModalityList = new VETRIS.Core.Settings.GLModalityList[0];
            VETRIS.Core.Settings.GLServiceList[] objGLServiceList = new VETRIS.Core.Settings.GLServiceList[0];
            VETRIS.Core.Settings.GLNonRevenueHead[] objGLNonRevenueHead = new VETRIS.Core.Settings.GLNonRevenueHead[0];
            VETRIS.Core.Settings.GLRadiologistCharges[] objGLRadiologistCharges = new VETRIS.Core.Settings.GLRadiologistCharges[0];
            int intListIndex = 0;
            objComm = new classes.CommonClass();
            objCore = new Core.Settings.GLCodeMapping();


            try
            {
                objCore.USER_ID = new Guid(ArrRecord[0]);
                objCore.MENU_ID = Convert.ToInt32(ArrRecord[1]);

                if (ArrModalityList.Length > 0)
                {
                    objGLModalityList = new VETRIS.Core.Settings.GLModalityList[(ArrModalityList.Length)/3];
                    for (int i = 0; i < objGLModalityList.Length; i++)
                    {
                        objGLModalityList[i] = new VETRIS.Core.Settings.GLModalityList();
                        objGLModalityList[i].CATEGORY_ID = Convert.ToInt32(ArrModalityList[intListIndex]);
                        objGLModalityList[i].MODALITY_ID = Convert.ToInt32(ArrModalityList[intListIndex + 1]);
                        objGLModalityList[i].GL_CODE = ArrModalityList[intListIndex + 2].Trim();
                        intListIndex = intListIndex + 3;
                    }
                }
                intListIndex = 0;

                if (ArrServiceList.Length > 0)
                {
                    objGLServiceList = new VETRIS.Core.Settings.GLServiceList[(ArrServiceList.Length) / 4];
                    for (int i = 0; i < objGLServiceList.Length; i++)
                    {
                        objGLServiceList[i] = new VETRIS.Core.Settings.GLServiceList();
                        objGLServiceList[i].SERVICE_ID = Convert.ToInt32(ArrServiceList[intListIndex]);
                        objGLServiceList[i].MODALITY_ID = Convert.ToInt32(ArrServiceList[intListIndex + 1]);
                        objGLServiceList[i].GL_CODE = ArrServiceList[intListIndex + 2].Trim();
                        objGLServiceList[i].GL_CODE_AFTER_HOURS = ArrServiceList[intListIndex + 3].Trim();
                        intListIndex = intListIndex + 4;
                    }
                }
                intListIndex = 0;

                if (ArrNRHList.Length > 0)
                {
                    objGLNonRevenueHead = new VETRIS.Core.Settings.GLNonRevenueHead[(ArrNRHList.Length) / 2];
                    for (int i = 0; i < objGLNonRevenueHead.Length; i++)
                    {
                        objGLNonRevenueHead[i] = new VETRIS.Core.Settings.GLNonRevenueHead();
                        objGLNonRevenueHead[i].CONTROL_CODE = ArrNRHList[intListIndex].Trim();
                        objGLNonRevenueHead[i].GL_CODE = ArrNRHList[intListIndex + 1].Trim();
                        intListIndex = intListIndex + 2;
                    }
                }
                intListIndex = 0;

                if (ArrRCList.Length > 0)
                {
                    objGLRadiologistCharges = new VETRIS.Core.Settings.GLRadiologistCharges[(ArrRCList.Length) / 2];
                    for (int i = 0; i < objGLRadiologistCharges.Length; i++)
                    {
                        objGLRadiologistCharges[i] = new VETRIS.Core.Settings.GLRadiologistCharges();
                        objGLRadiologistCharges[i].GROUP_ID = Convert.ToInt32(ArrRCList[intListIndex]);
                        objGLRadiologistCharges[i].GL_CODE = ArrRCList[intListIndex + 1].Trim();
                        intListIndex = intListIndex + 2;
                    }
                }
                intListIndex = 0;


                bReturn = objCore.SaveRecord(Server.MapPath("~"), objGLModalityList,objGLServiceList, objGLNonRevenueHead,objGLRadiologistCharges,ref strReturnMsg, ref strCatchMessage);

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
                objCore = null; objComm = null; objGLModalityList = null;
                strReturnMsg = null; strCatchMessage = null;
            }
            return arrRet;
        }
        #endregion
    }
}