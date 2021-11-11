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
    [AjaxPro.AjaxNamespace("VRSFeesTemplate")]
    public partial class VRSFeesTemplate : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.Invoicing.ARFeeSchedule objCore = null;
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSFeesTemplate));
            SetAttributes();
            if ((!CallBackMF.CausedCallback) && (!CallBackSF.CausedCallback))
                SetPageValue();
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {
            btnAddMF1.Attributes.Add("onclick", "javascript:AddModality();");
            btnAddMF2.Attributes.Add("onclick", "javascript:AddModality();");
            btnSaveMF1.Attributes.Add("onclick", "javascript:SaveModality();");
            btnSaveMF2.Attributes.Add("onclick", "javascript:SaveModality();");
            btnAddSF1.Attributes.Add("onclick", "javascript:AddService();");
            btnAddSF2.Attributes.Add("onclick", "javascript:AddService();");
            btnSaveSF1.Attributes.Add("onclick", "javascript:SaveService();");
            btnSaveSF2.Attributes.Add("onclick", "javascript:SaveService();");
            btnClose1.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
            btnClose2.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {

            int intUserRoleID = Convert.ToInt32(Request.QueryString["urid"]);
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid UserID = new Guid(Request.QueryString["uid"].ToString());
            string strID = string.Empty;
            FetchParameters(UserID, intMenuID);
            string strTheme = Request.QueryString["th"];
            SetCSS(strTheme);
        }
        #endregion
        #region SetCSS
        private void SetCSS(string strTheme)
        {
            string strServerPath = ConfigurationManager.AppSettings["ServerPath"];
            lnkSTYLE.Attributes["href"] = strServerPath + "/css/" + strTheme + "/style.css";
            lnkGRID.Attributes["href"] = strServerPath + "/css/" + strTheme + "/grid_style.css";
            lnkTAB.Attributes["href"] = strServerPath + "/css/" + strTheme + "/tabStyle1.css";
        }
        #endregion

        #region FetchParameters
        private void FetchParameters(Guid UserID,int intMenuID)
        {
            objCore = new Core.Invoicing.ARFeeSchedule();
            string strCatchMessage = ""; string strReturnMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();

            try
            {
                objCore.USER_ID = UserID;
                objCore.MENU_ID = intMenuID;

                bReturn = objCore.FetchParameters(Server.MapPath("~"), ref ds,ref strReturnMessage, ref strCatchMessage);

                if (bReturn)
                {
                    #region Modality
                    DataRow dr1 = ds.Tables["Modality"].NewRow();
                    dr1["id"] = 0;
                    dr1["name"] = "Select One";
                    dr1["invoice_by"] = "";
                    ds.Tables["Modality"].Rows.InsertAt(dr1, 0);
                    ds.Tables["Modality"].AcceptChanges();


                    foreach (DataRow dr in ds.Tables["Modality"].Rows)
                    {
                        if (hdnModality.Value.Trim() != string.Empty) hdnModality.Value += objComm.RecordDivider;
                        hdnModality.Value += Convert.ToString(dr["id"]) + objComm.RecordDivider;
                        hdnModality.Value += Convert.ToString(dr["name"]).Trim() + objComm.RecordDivider;
                        hdnModality.Value += Convert.ToString(dr["invoice_by"]).Trim();
                    }
                    #endregion

                    #region Services
                    DataRow dr2 = ds.Tables["Services"].NewRow();
                    dr2["id"] = 0;
                    dr2["name"] = "Select One";
                    dr2["priority_id"] = 0;
                    ds.Tables["Services"].Rows.InsertAt(dr2, 0);
                    ds.Tables["Services"].AcceptChanges();


                    foreach (DataRow dr in ds.Tables["Services"].Rows)
                    {
                        if (hdnServices.Value.Trim() != string.Empty) hdnServices.Value += objComm.RecordDivider;
                        hdnServices.Value += Convert.ToString(dr["id"]) + objComm.RecordDivider;
                        hdnServices.Value += Convert.ToString(dr["name"]).Trim() + objComm.RecordDivider;
                        hdnServices.Value += Convert.ToString(dr["priority_id"]).Trim();
                    }
                    #endregion

                    #region Category
                    DataRow dr3 = ds.Tables["Category"].NewRow();
                    dr3["id"] = 0;
                    dr3["name"] = "Select One";
                    ds.Tables["Category"].Rows.InsertAt(dr3, 0);


                    foreach (DataRow dr in ds.Tables["Category"].Rows)
                    {
                        if (hdnCategory.Value.Trim() != string.Empty) hdnCategory.Value += objComm.RecordDivider;
                        hdnCategory.Value += Convert.ToString(dr["id"]) + objComm.RecordDivider;
                        hdnCategory.Value += Convert.ToString(dr["name"]).Trim();
                    }
                    #endregion
                }
                else
                    hdnError.Value = "false" + objComm.RecordDivider + strCatchMessage.Trim();


            }
            catch (Exception ex)
            {
                hdnError.Value = "catch" + objComm.RecordDivider + ex.Message.Trim();
            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }

        }
        #endregion

        #region Grid

        #region Modality

        #region CallBackMF_Callback
        protected void CallBackMF_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            string strAction = e.Parameters[0];

            switch (strAction)
            {
                case "L":
                    LoadModalityFee(e.Parameters);
                    break;
                case "A":
                    AddModalityRecord(e.Parameters);
                    break;
                case "D":
                    DeleteModalityRecord(e.Parameters);
                    break;
            }
            grdMF.Width = Unit.Percentage(100);
            grdMF.RenderControl(e.Output);
            spnERR.RenderControl(e.Output);
        }
        #endregion

        #region LoadModalityFee
        private void LoadModalityFee(string[] arrParams)
        {
            objCore = new Core.Invoicing.ARFeeSchedule();
            string strCatchMessage = ""; 
            bool bReturn = false;
            DataSet ds = new DataSet();

            try
            {
                objCore.USER_ID = new Guid(arrParams[1]);
                objCore.MENU_ID = Convert.ToInt32(arrParams[2]);

                bReturn = objCore.LoadModalityDetails(Server.MapPath("~"), ref ds,ref strCatchMessage);
                if (bReturn)
                {
                    grdMF.DataSource = ds.Tables["ModalityFeeList"];
                    grdMF.DataBind();
                    spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"\" />";
                }
                else
                    spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + strCatchMessage + "\" />";



            }
            catch (Exception ex)
            {
                spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }
        }
        #endregion

        #region AddModalityRecord
        private void AddModalityRecord(string[] arrParams)
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
                        for (int i = 0; i < arrRecords.Length; i = i + 12)
                        {
                            DataRow dr = dtbl.NewRow();
                            intSrl = intSrl + 1;
                            dr["row_id"] = intSrl;
                            dr["id"] = arrRecords[i + 1].Trim();
                            dr["category_id"] = Convert.ToInt32(arrRecords[i + 2]);
                            dr["modality_id"] = Convert.ToInt32(arrRecords[i + 3]);
                            dr["invoice_by"] = arrRecords[i + 4].Trim();
                            dr["invoice_by_desc"] = arrRecords[i + 5].Trim();
                            dr["default_count_from"] = Convert.ToInt32(arrRecords[i + 6]);
                            dr["default_count_to"] = Convert.ToInt32(arrRecords[i + 7]);
                            dr["fee_amount"] = Convert.ToDecimal(arrRecords[i + 8]);
                            dr["fee_amount_per_unit"] = Convert.ToDecimal(arrRecords[i + 9]);
                            dr["study_max_amount"] = Convert.ToDecimal(arrRecords[i + 10]);
                            dr["gl_code"] = arrRecords[i + 11].Trim();
                            dtbl.Rows.Add(dr);
                        }

                    }
                }

                DataRow drNew = dtbl.NewRow();
                intSrl = intSrl + 1;
                drNew["row_id"] = intSrl;
                drNew["id"] = "00000000-0000-0000-0000-000000000000";
                drNew["category_id"] = 0;
                drNew["modality_id"] = 0;
                drNew["invoice_by"] = "";
                drNew["invoice_by_desc"] = "";
                drNew["default_count_from"] = 0;
                drNew["default_count_to"] = 0;
                drNew["fee_amount"] = 0;
                drNew["fee_amount_per_unit"] = 0;
                drNew["study_max_amount"] = 0;
                drNew["gl_code"] = "";
                dtbl.Rows.Add(drNew);

                DataView dv = new DataView(dtbl);
                dv.Sort = "row_id asc";

                grdMF.DataSource = dv.ToTable();
                grdMF.DataBind();
                spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"\" />";
            }
            catch (Exception ex)
            {
                spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                dtbl.Dispose();
                objComm = null;
            }
        }
        #endregion

        #region DeleteModalityRecord
        private void DeleteModalityRecord(string[] arrParams)
        {
            DataTable dtbl = new DataTable();
            objComm = new classes.CommonClass();
            objCore = new Core.Invoicing.ARFeeSchedule();
            int intID = Convert.ToInt32(arrParams[1]);
            string[] arrSep = { objComm.SecondaryRecordDivider };
            string[] arrRecords = arrParams[2].Split(arrSep, StringSplitOptions.None);
            Guid UserID = new Guid(arrParams[3].Trim());
            int intMenuID = Convert.ToInt32(arrParams[4].Trim());

            int intSrl = 0;
            Guid delId = new Guid("00000000-0000-0000-0000-000000000000");


            string strCatchMessage = ""; string strReturnMessage = "";
            bool bReturn = false;

            try
            {
                dtbl = CreateModalityTable();
                if (arrRecords[0].Trim() != "")
                {
                    for (int i = 0; i < arrRecords.Length; i = i + 12)
                    {
                        if (Convert.ToInt32(arrRecords[i]) != intID)
                        {
                            DataRow dr = dtbl.NewRow();
                            intSrl = intSrl + 1;
                            dr["row_id"] = intSrl;
                            dr["id"] = arrRecords[i + 1].Trim();
                            dr["category_id"] = Convert.ToInt32(arrRecords[i + 2]);
                            dr["modality_id"] = Convert.ToInt32(arrRecords[i + 3]);
                            dr["invoice_by"] = arrRecords[i + 4].Trim();
                            dr["invoice_by_desc"] = arrRecords[i + 5].Trim();
                            dr["default_count_from"] = Convert.ToInt32(arrRecords[i + 6]);
                            dr["default_count_to"] = Convert.ToInt32(arrRecords[i + 7]);
                            dr["fee_amount"] = Convert.ToDecimal(arrRecords[i + 8]);
                            dr["fee_amount_per_unit"] = Convert.ToDecimal(arrRecords[i + 9]);
                            dr["study_max_amount"] = Convert.ToDecimal(arrRecords[i + 10]);
                            dr["gl_code"] = arrRecords[i + 11].Trim();
                            dtbl.Rows.Add(dr);
                        }
                        else
                        {
                            delId = new Guid(arrRecords[i + 1].Trim());
                            if (delId != new Guid("00000000-0000-0000-0000-000000000000"))
                            {
                                objCore.ID = delId;
                                objCore.TYPE = "M";
                                objCore.USER_ID = UserID;
                                objCore.MENU_ID = intMenuID;

                                bReturn = objCore.DeleteRecord(Server.MapPath("~"), ref strReturnMessage, ref strCatchMessage);
                                if (!bReturn)
                                {
                                    spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + strCatchMessage + "\" />";
                                }
                            }
                        }
                    }
                }

                grdMF.DataSource = dtbl;
                grdMF.DataBind();
                spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"\" />";

            }
            catch (Exception ex)
            {
                spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                dtbl.Dispose();
                objCore = null;
                objComm = null;
            }
        }
        #endregion

        #region CreateModalityTable
        private DataTable CreateModalityTable()
        {
            DataTable dtbl = new DataTable();
            dtbl.Columns.Add("row_id", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("id", System.Type.GetType("System.String"));
            dtbl.Columns.Add("category_id", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("modality_id", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("invoice_by", System.Type.GetType("System.String"));
            dtbl.Columns.Add("invoice_by_desc", System.Type.GetType("System.String"));
            dtbl.Columns.Add("default_count_from", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("default_count_to", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("fee_amount", System.Type.GetType("System.Decimal"));
            dtbl.Columns.Add("fee_amount_per_unit", System.Type.GetType("System.Decimal"));
            dtbl.Columns.Add("study_max_amount", System.Type.GetType("System.Decimal"));
            dtbl.Columns.Add("gl_code", System.Type.GetType("System.String"));
            dtbl.TableName = "ModalityFeeList";
            return dtbl;
        }
        #endregion

        #endregion

        #region Services

        #region CallBackSF_Callback
        protected void CallBackSF_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            string strAction = e.Parameters[0];

            switch (strAction)
            {
                case "L":
                    LoadServiceFee(e.Parameters);
                    break;
                case "A":
                    AddServiceRecord(e.Parameters);
                    break;
                case "D":
                    DeleteServiceRecord(e.Parameters);
                    break;
            }
            grdSF.Width = Unit.Percentage(100);
            grdSF.RenderControl(e.Output);
            spnSvcErr.RenderControl(e.Output);
        }
        #endregion

        #region LoadServiceFee
        private void LoadServiceFee(string[] arrParams)
        {
            objCore = new Core.Invoicing.ARFeeSchedule();
            string strCatchMessage = "";
            bool bReturn = false;
            DataSet ds = new DataSet();

            try
            {
                objCore.USER_ID = new Guid(arrParams[1]);
                objCore.MENU_ID = Convert.ToInt32(arrParams[2]);

                bReturn = objCore.LoadServiceDetails(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    grdSF.DataSource = ds.Tables["ServiceFeeList"];
                    grdSF.DataBind();
                    spnSvcErr.InnerHtml = "<input type=\"hidden\" id=\"hdnCBSvcErr\" value=\"\" />";
                }
                else
                    spnSvcErr.InnerHtml = "<input type=\"hidden\" id=\"hdnCBSvcErr\" value=\"" + strCatchMessage + "\" />";



            }
            catch (Exception ex)
            {
                spnSvcErr.InnerHtml = "<input type=\"hidden\" id=\"hdnCBSvcErr\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }
        }
        #endregion

        #region AddServiceRecord
        private void AddServiceRecord(string[] arrParams)
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
                        for (int i = 0; i < arrRecords.Length; i = i + 11)
                        {
                            DataRow dr = dtbl.NewRow();
                            intSrl = intSrl + 1;
                            dr["row_id"] = intSrl;
                            dr["id"] = arrRecords[i + 1].Trim();
                            dr["service_id"] = Convert.ToInt32(arrRecords[i + 2]);
                            dr["modality_id"] = Convert.ToInt32(arrRecords[i + 3]);
                            dr["invoice_by"] = arrRecords[i + 4].Trim();
                            dr["invoice_by_desc"] = arrRecords[i + 5].Trim();
                            dr["default_count_from"] = Convert.ToInt32(arrRecords[i + 6]);
                            dr["default_count_to"] = Convert.ToInt32(arrRecords[i + 7]);
                            dr["fee_amount"] = Convert.ToDecimal(arrRecords[i + 8]);
                            dr["fee_amount_after_hrs"] = Convert.ToDecimal(arrRecords[i + 9]);
                            dr["gl_code"] = arrRecords[i + 10].Trim();
                            dtbl.Rows.Add(dr);
                        }

                    }
                }

                DataRow drNew = dtbl.NewRow();
                intSrl = intSrl + 1;
                drNew["row_id"] = intSrl;
                drNew["id"] = "00000000-0000-0000-0000-000000000000";
                drNew["service_id"] = 0;
                drNew["modality_id"] = 0;
                drNew["invoice_by"] = "";
                drNew["invoice_by_desc"] = "";
                drNew["default_count_from"] = 0;
                drNew["default_count_to"] = 0;
                drNew["fee_amount"] = 0;
                drNew["fee_amount_after_hrs"] = 0;
                drNew["gl_code"] = "";
                dtbl.Rows.Add(drNew);

                DataView dv = new DataView(dtbl);
                dv.Sort = "row_id asc";

                grdSF.DataSource = dv.ToTable();
                grdSF.DataBind();
                spnSvcErr.InnerHtml = "<input type=\"hidden\" id=\"hdnCBSvcErr\" value=\"\" />";
            }
            catch (Exception ex)
            {
                spnSvcErr.InnerHtml = "<input type=\"hidden\" id=\"hdnCBSvcErr\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                dtbl.Dispose();
                objComm = null;
            }
        }
        #endregion

        #region DeleteServiceRecord
        private void DeleteServiceRecord(string[] arrParams)
        {
            DataTable dtbl = new DataTable();
            objComm = new classes.CommonClass();
            objCore = new Core.Invoicing.ARFeeSchedule();
            int intID = Convert.ToInt32(arrParams[1]);
            string[] arrSep = { objComm.SecondaryRecordDivider };
            string[] arrRecords = arrParams[2].Split(arrSep, StringSplitOptions.None);
            Guid UserID = new Guid(arrParams[3].Trim());
            int intMenuID = Convert.ToInt32(arrParams[4].Trim());

            int intSrl = 0;
            Guid delId = new Guid("00000000-0000-0000-0000-000000000000");


            string strCatchMessage = ""; string strReturnMessage = "";
            bool bReturn = false;

            try
            {
                dtbl = CreateServiceTable();
                if (arrRecords[0].Trim() != "")
                {
                    for (int i = 0; i < arrRecords.Length; i = i + 11)
                    {
                        if (Convert.ToInt32(arrRecords[i]) != intID)
                        {
                            DataRow dr = dtbl.NewRow();
                            intSrl = intSrl + 1;
                            dr["row_id"] = intSrl;
                            dr["id"] = arrRecords[i + 1].Trim();
                            dr["service_id"] = Convert.ToInt32(arrRecords[i + 2]);
                            dr["modality_id"] = Convert.ToInt32(arrRecords[i + 3]);
                            dr["invoice_by"] = arrRecords[i + 4].Trim();
                            dr["invoice_by_desc"] = arrRecords[i + 5].Trim();
                            dr["default_count_from"] = Convert.ToInt32(arrRecords[i + 6]);
                            dr["default_count_to"] = Convert.ToInt32(arrRecords[i + 7]);
                            dr["fee_amount"] = Convert.ToDecimal(arrRecords[i + 8]);
                            dr["fee_amount_after_hrs"] = Convert.ToDecimal(arrRecords[i + 9]);
                            dr["gl_code"] = arrRecords[i + 10].Trim();
                            dtbl.Rows.Add(dr);
                        }
                        else
                        {
                            delId = new Guid(arrRecords[i + 1].Trim());
                            if (delId != new Guid("00000000-0000-0000-0000-000000000000"))
                            {
                                objCore.ID = delId;
                                objCore.TYPE = "S";
                                objCore.USER_ID = UserID;
                                objCore.MENU_ID = intMenuID;

                                bReturn = objCore.DeleteRecord(Server.MapPath("~"), ref strReturnMessage, ref strCatchMessage);
                                if (!bReturn)
                                {
                                    spnSvcErr.InnerHtml = "<input type=\"hidden\" id=\"hdnCBSvcErr\" value=\"" + strCatchMessage + "\" />";
                                }
                            }
                        }
                    }
                }

                grdSF.DataSource = dtbl;
                grdSF.DataBind();
                spnSvcErr.InnerHtml = "<input type=\"hidden\" id=\"hdnCBSvcErr\" value=\"\" />";

            }
            catch (Exception ex)
            {
                spnSvcErr.InnerHtml = "<input type=\"hidden\" id=\"hdnCBSvcErr\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                dtbl.Dispose();
                objCore = null;
                objComm = null;
            }
        }
        #endregion

        #region CreateServiceTable
        private DataTable CreateServiceTable()
        {
            DataTable dtbl = new DataTable();
            dtbl.Columns.Add("row_id", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("id", System.Type.GetType("System.String"));
            dtbl.Columns.Add("service_id", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("modality_id", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("invoice_by", System.Type.GetType("System.String"));
            dtbl.Columns.Add("invoice_by_desc", System.Type.GetType("System.String"));
            dtbl.Columns.Add("default_count_from", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("default_count_to", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("fee_amount", System.Type.GetType("System.Decimal"));
            dtbl.Columns.Add("fee_amount_after_hrs", System.Type.GetType("System.Decimal"));
            dtbl.Columns.Add("gl_code", System.Type.GetType("System.String"));
            dtbl.TableName = "ServiceFeeList";
            return dtbl;
        }
        #endregion

        #endregion

        #endregion

        #region SaveModalityRecord (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] SaveModalityRecord(string[] ArrRecord, string[] ArrParams)
        {
            bool bReturn = false;
            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty;
            int intCompanyID = 0; string strFileName = string.Empty; Guid UserID = Guid.Empty;
            objComm = new classes.CommonClass();
            objCore = new Core.Invoicing.ARFeeSchedule();
            int intListIndex = 0;
            Core.Invoicing.ARFeeSchedule[] objRecords = new Core.Invoicing.ARFeeSchedule[0];

            try
            {

                objCore.USER_ID = UserID = new Guid(ArrParams[0]);
                objCore.MENU_ID = Convert.ToInt32(ArrParams[1]);


                objRecords = new Core.Invoicing.ARFeeSchedule[(ArrRecord.Length / 11)];

                for (int i = 0; i < objRecords.Length; i++)
                {
                    objRecords[i] = new Core.Invoicing.ARFeeSchedule();
                    objRecords[i].ROW_ID = Convert.ToInt32(ArrRecord[intListIndex]);
                    objRecords[i].ID = new Guid(ArrRecord[intListIndex + 1]);
                    objRecords[i].CATEGORY_ID = Convert.ToInt32(ArrRecord[intListIndex + 2]);
                    objRecords[i].MODALITY_ID = Convert.ToInt32(ArrRecord[intListIndex + 3]);
                    objRecords[i].INVOICE_BY = ArrRecord[intListIndex + 4].Trim();
                    objRecords[i].DEFAULT_COUNT_FROM = Convert.ToInt32(ArrRecord[intListIndex + 5]);
                    objRecords[i].DEFAULT_COUNT_TO = Convert.ToInt32(ArrRecord[intListIndex + 6]);
                    objRecords[i].FEE_AMOUNT = Convert.ToDouble(ArrRecord[intListIndex + 7]);
                    objRecords[i].FEE_AMOUNT_PER_UNIT = Convert.ToDouble(ArrRecord[intListIndex + 8]);
                    objRecords[i].STUDY_MAXIMUM_FEE_AMOUNT = Convert.ToDouble(ArrRecord[intListIndex + 9]);
                    objRecords[i].GL_CODE = ArrRecord[intListIndex + 10].Trim();
                    intListIndex = intListIndex + 11;
                }

                intListIndex = 0;


                bReturn = objCore.SaveModalityRecord(Server.MapPath("~"), objRecords, ref strReturnMsg, ref strCatchMessage);

                if (bReturn)
                {
                    arrRet = new string[2];
                    arrRet[0] = "true";
                    arrRet[1] = strReturnMsg.Trim();
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

        #region SaveServiceRecord (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] SaveServiceRecord(string[] ArrRecord, string[] ArrParams)
        {
            bool bReturn = false;
            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty;
            int intCompanyID = 0; string strFileName = string.Empty; Guid UserID = Guid.Empty;
            objComm = new classes.CommonClass();
            objCore = new Core.Invoicing.ARFeeSchedule();
            int intListIndex = 0;
            Core.Invoicing.ARFeeSchedule[] objRecords = new Core.Invoicing.ARFeeSchedule[0];

            try
            {

                objCore.USER_ID = UserID = new Guid(ArrParams[0]);
                objCore.MENU_ID = Convert.ToInt32(ArrParams[1]);


                objRecords = new Core.Invoicing.ARFeeSchedule[(ArrRecord.Length / 10)];

                for (int i = 0; i < objRecords.Length; i++)
                {
                    objRecords[i] = new Core.Invoicing.ARFeeSchedule();
                    objRecords[i].ROW_ID = Convert.ToInt32(ArrRecord[intListIndex]);
                    objRecords[i].ID = new Guid(ArrRecord[intListIndex + 1]);
                    objRecords[i].SERVICE_ID = Convert.ToInt32(ArrRecord[intListIndex + 2]);
                    objRecords[i].MODALITY_ID = Convert.ToInt32(ArrRecord[intListIndex + 3]);
                    objRecords[i].INVOICE_BY = ArrRecord[intListIndex + 4].Trim();
                    objRecords[i].DEFAULT_COUNT_FROM = Convert.ToInt32(ArrRecord[intListIndex + 5]);
                    objRecords[i].DEFAULT_COUNT_TO = Convert.ToInt32(ArrRecord[intListIndex + 6]);
                    objRecords[i].FEE_AMOUNT = Convert.ToDouble(ArrRecord[intListIndex + 7]);
                    objRecords[i].FEE_AMOUNT_AFTER_HOUR = Convert.ToDouble(ArrRecord[intListIndex + 8]);
                    objRecords[i].GL_CODE = ArrRecord[intListIndex + 9].Trim();
                    intListIndex = intListIndex + 10;
                }

                intListIndex = 0;


                bReturn = objCore.SaveServiceRecord(Server.MapPath("~"), objRecords, ref strReturnMsg, ref strCatchMessage);

                if (bReturn)
                {
                    arrRet = new string[2];
                    arrRet[0] = "true";
                    arrRet[1] = strReturnMsg.Trim();
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