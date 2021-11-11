using ClearCanvas.Dicom;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VETRIS.Core;
using DICOMLib;

namespace VETRIS.CaseList
{
    [AjaxPro.AjaxNamespace("VRSManualSubmissionTemp")]
    public partial class VRSManualSubmissionTemp : System.Web.UI.Page
    {
        #region Members & Variables
        classes.CommonClass objComm;
        VETRIS.Core.Case.CaseStudy objCore;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSManualSubmissionTemp));
            SetAttributes();
            if ((!CallBackST.CausedCallback) && (!CallBackSelST.CausedCallback) && (!CallBackSF.CausedCallback))
                SetPageValue();
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            btnSubmit1.Attributes.Add("onclick", "javascript:btnSubmit_OnClick('Y','X');");
            btnSubmit2.Attributes.Add("onclick", "javascript:btnSubmit_OnClick('Y','X');");
            btnClose1.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
            btnClose2.Attributes.Add("onclick", "javascript:btnClose_OnClick();");

            txtPLName.Attributes.Add("onchange", "javascript:txtPLName_OnChange();");

            txtDOS.Attributes.Add("onblur", "javascript:txtDOS_OnBlur();");
            imgDOS.Attributes.Add("onclick", "javascript:SetSelectedDate('" + txtDOS.ClientID + "','CalDOS');");

            ddlModality.Attributes.Add("onchange", "javascript:LoadStudyTypes();");
            //ddlInstitution.Attributes.Add("onchange", "javascript:ddlInstitution_OnChange();");
            ddlSpecies.Attributes.Add("onchange", "javascript:ddlSpecies_OnChange();");

            txtFromDt.Attributes.Add("onblur", "javascript:txtFromDt_OnBlur();");
            imgFrom.Attributes.Add("onclick", "javascript:SetSelectedDate('" + txtFromDt.ClientID + "','CalFrom');");
            txtPWt.Attributes.Add("onkeypress", "javascript:return parent.CheckDecimal(event);");
            txtPWt.Attributes.Add("onblur", "javascript:ResetValueDecimal(this,3);");
            txtPWt.Attributes.Add("onfocus", "javascript:this.select();");

        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {
            Guid UserID = new Guid(Request.QueryString["uid"]);
            hdnRegInstitutionId.Value = UserID.ToString();
            objComm = new classes.CommonClass();
            objComm.SetRegionalFormat();
            PopulateTimeDropDowns();
            txtDOS.Text = objComm.IMDateFormat(DateTime.Today, objComm.DateFormat);
            CalDOS.SelectedDate = DateTime.Today;
            ddlHr.SelectedValue = objComm.padZero(DateTime.Now.Hour);
            ddlMin.SelectedValue = objComm.padZero(DateTime.Now.Minute);
            CalFrom.SelectedDate = DateTime.Today;
            txtPWt.Text = objComm.IMNumeric(0, 3);
            objComm = null;
            hdnFilePath.Value = Server.MapPath("~");
            hdnTempFolder.Value = Server.MapPath("~") + "/CaseList/MSTemp/" + UserID.ToString();
            FetchParameters(UserID);
        }
        #endregion

        #region PopulateTimeDropDowns
        private void PopulateTimeDropDowns()
        {
            for (int i = 0; i <= 23; i++)
            {
                ListItem item = new ListItem();
                item.Value = objComm.padZero(i);
                item.Text = objComm.padZero(i);
                ddlHr.Items.Add(item);
            }
            for (int i = 0; i <= 59; i++)
            {
                ListItem item = new ListItem();
                item.Value = objComm.padZero(i);
                item.Text = objComm.padZero(i);
                ddlMin.Items.Add(item);
            }
        }
        #endregion

        #region CreateUserDirectory
        private void CreateUserDirectory(Guid UserID)
        {
            if (!Directory.Exists(Server.MapPath("~/CaseList/Temp/" + UserID.ToString())))
            {
                Directory.CreateDirectory(Server.MapPath("~/CaseList/Temp/" + UserID.ToString()));
            }
        }
        #endregion

        #region FetchParameters
        private void FetchParameters(Guid UserID)
        {
            objCore = new Core.Case.CaseStudy();
            string strCatchMessage = ""; bool bReturn = false; string strControlCode = string.Empty;
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();
            int intCnt = 0;

            try
            {
                objCore.USER_ID = UserID;

                bReturn = objCore.FetchBrowserParameters(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {

                    #region Institution
                    intCnt = ds.Tables["Institutions"].Rows.Count;
                    DataRow dr1 = ds.Tables["Institutions"].NewRow();
                    dr1["id"] = "00000000-0000-0000-0000-000000000000";
                    dr1["name"] = "Select One";
                    ds.Tables["Institutions"].Rows.InsertAt(dr1, 0);
                    ds.Tables["Institutions"].AcceptChanges();

                    //ddlInstitution.DataSource = ds.Tables["Institutions"];
                    //ddlInstitution.DataValueField = "id";
                    //ddlInstitution.DataTextField = "name";
                    //ddlInstitution.DataBind();

                    //if (intCnt == 1) ddlInstitution.SelectedIndex = 1; else ddlInstitution.SelectedIndex = 0;
                    #endregion

                    #region Params
                    foreach (DataRow dr in ds.Tables["APIParams"].Rows)
                    {
                        strControlCode = Convert.ToString(dr["control_code"]).Trim();
                        switch (strControlCode)
                        {
                            case "DCMMODIFYEXEPATH":
                                hdnDCMMODIFYEXEPATH.Value = Convert.ToString(dr["data_type_string"]).Trim();
                                break;
                            case "FTPABSPATH":
                                hdnFTPABSPATH.Value = Convert.ToString(dr["data_type_string"]).Trim();
                                break;
                            default:
                                break;
                        }
                    }
                    #endregion

                    #region Species
                    DataRow dr2 = ds.Tables["Species"].NewRow();
                    dr2["id"] = 0;
                    dr2["name"] = "Select One";
                    ds.Tables["Species"].Rows.InsertAt(dr2, 0);
                    ds.Tables["Species"].AcceptChanges();

                    ddlSpecies.DataSource = ds.Tables["Species"];
                    ddlSpecies.DataValueField = "id";
                    ddlSpecies.DataTextField = "name";
                    ddlSpecies.DataBind();
                    #endregion

                    #region Priority
                    DataRow dr3 = ds.Tables["Priority"].NewRow();
                    dr3["priority_id"] = "0";
                    dr3["priority_desc"] = "Select One";
                    ds.Tables["Priority"].Rows.InsertAt(dr3, 0);
                    ds.Tables["Priority"].AcceptChanges();

                    ddlPriority.DataSource = ds.Tables["Priority"];
                    ddlPriority.DataValueField = "priority_id";
                    ddlPriority.DataTextField = "priority_desc";
                    ddlPriority.DataBind();
                    #endregion

                    #region Category
                    DataRow dr4 = ds.Tables["Category"].NewRow();
                    dr4["id"] = "0";
                    dr4["name"] = "Select One";
                    ds.Tables["Category"].Rows.InsertAt(dr4, 0);
                    ds.Tables["Category"].AcceptChanges();

                    ddlCategory.DataSource = ds.Tables["Category"];
                    ddlCategory.DataValueField = "id";
                    ddlCategory.DataTextField = "name";
                    ddlCategory.DataBind();
                    #endregion

                    #region Physicians
                    if (ds.Tables["Physicians"].Rows.Count > 0)
                    {
                        DataRow dr5 = ds.Tables["Physicians"].NewRow();
                        dr5["id"] = "00000000-0000-0000-0000-000000000000";
                        dr5["name"] = "Select One";
                        ds.Tables["Physicians"].Rows.InsertAt(dr5, 0);
                        ds.Tables["Physicians"].AcceptChanges();
                    }
                    else
                    {
                        DataRow dr5 = ds.Tables["Physicians"].NewRow();
                        dr5["id"] = "00000000-0000-0000-0000-000000000000";
                        dr5["name"] = "Please select an institution";
                        ds.Tables["Physicians"].Rows.InsertAt(dr5, 0);
                        ds.Tables["Physicians"].AcceptChanges();
                    }

                    ddlPhys.DataSource = ds.Tables["Physicians"];
                    ddlPhys.DataValueField = "id";
                    ddlPhys.DataTextField = "name";
                    ddlPhys.DataBind();
                    #endregion

                    #region Modality
                    DataRow dr6 = ds.Tables["Modality"].NewRow();
                    dr6["id"] = "0";
                    dr6["name"] = "Select One";
                    ds.Tables["Modality"].Rows.InsertAt(dr6, 0);
                    ds.Tables["Modality"].AcceptChanges();

                    ddlModality.DataSource = ds.Tables["Modality"];
                    ddlModality.DataValueField = "id";
                    ddlModality.DataTextField = "name";
                    ddlModality.DataBind();
                    #endregion
                }
                else
                    hdnError.Value = strCatchMessage.Trim();


                CreateUserDirectory(UserID);

            }
            catch (Exception ex)
            {
                hdnError.Value = ex.Message.Trim();
            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }

        }
        #endregion

        #region  Grids

        #region Study Type Grid

        #region CallBackST_Callback
        protected void CallBackST_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            LoadStudyTypes(e.Parameters);
            grdST.Width = Unit.Percentage(100);
            grdST.RenderControl(e.Output);
            spnErrST.RenderControl(e.Output);
            spnTrackBy.RenderControl(e.Output);
        }
        #endregion

        #region LoadStudyTypes
        private void LoadStudyTypes(string[] arrParams)
        {
            objCore = new Core.Case.CaseStudy();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            Guid UserID = Guid.Empty;
            string strFileName = "";

            try
            {

                objCore.ID = new Guid(arrParams[0]);
                objCore.MODALITY_ID = Convert.ToInt32(arrParams[1]);
                objCore.CATEGORY_ID = Convert.ToInt32(arrParams[2]);

                bReturn = objCore.LoadStudyTypes(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    grdST.DataSource = ds.Tables["StudyTypes"];
                    grdST.DataBind();
                    spnTrackBy.InnerHtml = "<input type=\"hidden\" id=\"hdnModTrackBy\" value=\"I\" />";

                    foreach (DataRow dr in ds.Tables["TrackBy"].Rows)
                    {
                        spnTrackBy.InnerHtml = "<input type=\"hidden\" id=\"hdnModTrackBy\" value=\"" + Convert.ToString(dr["track_by"]) + "\" />";
                    }

                    spnErrST.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrST\" value=\"\" />";
                }
                else
                {
                    spnTrackBy.InnerHtml = "<input type=\"hidden\" id=\"hdnModTrackBy\" value=\"I\" />";
                    spnErrST.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrST\" value=\"" + strCatchMessage + "\" />";
                }



            }
            catch (Exception ex)
            {
                spnErrST.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrST\" value=\"" + ex.Message.Trim() + "\" />";
                spnTrackBy.InnerHtml = "<input type=\"hidden\" id=\"hdnModTrackBy\" value=\"I\" />";
            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }
        }
        #endregion

        #endregion

        #region Selected Study Type Grid

        #region CallBackSelST_Callback
        protected void CallBackSelST_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            string strAction = e.Parameters[0];
            switch (strAction)
            {
                case "L":
                    LoadSelectedStudyTypes(e.Parameters);
                    break;
                case "U":
                    UpdateStudyTypes(e.Parameters);
                    break;
            }

            grdSelST.Width = Unit.Percentage(100);
            grdSelST.RenderControl(e.Output);
            spnErrSelST.RenderControl(e.Output);
        }
        #endregion

        #region LoadSelectedStudyTypes
        private void LoadSelectedStudyTypes(string[] arrParams)
        {
            objCore = new Core.Case.CaseStudy();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            Guid UserID = Guid.Empty;
            string strFileName = "";

            try
            {

                objCore.ID = new Guid(arrParams[1]);
                objCore.MODALITY_ID = Convert.ToInt32(arrParams[2]);

                bReturn = objCore.LoadSelectedStudyTypes(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    grdSelST.DataSource = ds.Tables["SelStudyTypes"];
                    grdSelST.DataBind();


                    spnErrSelST.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrSelST\" value=\"\" />";
                }
                else
                    spnErrSelST.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrSelST\" value=\"" + strCatchMessage + "\" />";



            }
            catch (Exception ex)
            {
                spnErrSelST.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrSelST\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }
        }
        #endregion

        #region UpdateStudyTypes
        private void UpdateStudyTypes(string[] arrParams)
        {
            DataTable dtbl = new DataTable();
            objComm = new classes.CommonClass();
            string[] arrSep = { objComm.SecondaryRecordDivider };
            string[] arrRecords = arrParams[1].Split(arrSep, StringSplitOptions.None);
            int intSrl = 0;

            try
            {
                dtbl = CreateSTTable();
                if (arrRecords.Length > 0)
                {
                    if (arrRecords[0].Trim() != "")
                    {
                        for (int i = 0; i < arrRecords.Length; i = i + 2)
                        {
                            DataRow dr = dtbl.NewRow();
                            intSrl = intSrl + 1;
                            dr["srl_no"] = intSrl;
                            dr["study_type_id"] = arrRecords[i].Trim();
                            dr["study_type_name"] = arrRecords[i + 1].Trim();
                            dtbl.Rows.Add(dr);
                        }

                    }
                }



                grdSelST.DataSource = dtbl;
                grdSelST.DataBind();
                spnErrSelST.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrSelST\" value=\"\" />";
            }
            catch (Exception ex)
            {
                spnErrSelST.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrSelST\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                dtbl.Dispose();
                objComm = null;
            }
        }
        #endregion

        #region CreateSTTable
        private DataTable CreateSTTable()
        {
            DataTable dtbl = new DataTable();
            dtbl.Columns.Add("srl_no", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("study_type_id", System.Type.GetType("System.String"));
            dtbl.Columns.Add("study_type_name", System.Type.GetType("System.String"));
            dtbl.TableName = "SelStudyTypes";
            return dtbl;
        }
        #endregion

        #endregion

        #region Study File Grid

        #region CallBackSF_Callback
        protected void CallBackSF_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            string strAction = e.Parameters[0];

            switch (strAction)
            {
                case "L":
                    LoadFiles(e.Parameters);
                    break;
                case "A":
                    AddFiles(e.Parameters);
                    break;
                case "D":
                    DeleteFiles(e.Parameters);
                    break;
                case "C":
                    ClearFiles(e.Parameters);
                    break;
            }
            grdSF.Width = Unit.Percentage(100);
            grdSF.RenderControl(e.Output);
            spnERR.RenderControl(e.Output);
            spnVal.RenderControl(e.Output);
        }
        #endregion

        #region LoadFiles
        private void LoadFiles(string[] arrParams)
        {
            DataTable dtbl = new DataTable();
            objComm = new classes.CommonClass();
            string[] arrSep = { objComm.SecondaryRecordDivider };
            string[] arrRecords = arrParams[1].Split(arrSep, StringSplitOptions.None);
            string strPID = arrParams[2];
            string strAccnNo = arrParams[3];
            string strFile = string.Empty;
            string strUserID = arrParams[4];

            try
            {
                dtbl = CreateStudyFileTable();
                if (arrRecords.Length > 0)
                {
                    if (arrRecords[0].Trim() != "")
                    {
                        for (int i = 0; i < arrRecords.Length; i = i + 4)
                        {
                            DataRow dr = dtbl.NewRow();
                            dr["file_srl_no"] = arrRecords[i];
                            strFile = arrRecords[i + 1].Trim();
                            dr["file_name"] = arrRecords[i + 1].Trim();
                            dr["file_type"] = arrRecords[i + 2].Trim();
                            dr["file_type_desc"] = arrRecords[i + 3].Trim();
                            dtbl.Rows.Add(dr);
                        }

                    }
                }


                grdSF.DataSource = dtbl;
                grdSF.DataBind();
                spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"\" />";

                spnVal.InnerHtml = "<input type=\"hidden\" id=\"hdnAccNo\" value=\"" + strAccnNo + "\" />";
                spnVal.InnerHtml += "<input type=\"hidden\" id=\"hdnPid\" value=\"" + strPID + "\" />";
            }
            catch (Exception ex)
            {
                spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + ex.Message.Trim() + "\" />";
                spnVal.InnerHtml = "<input type=\"hidden\" id=\"hdnAccNo\" value=\"\" />";
                spnVal.InnerHtml += "<input type=\"hidden\" id=\"hdnPid\" value=\"\" />";
            }
            finally
            {
                dtbl.Dispose();
                objComm = null;
            }
        }
        #endregion

        #region AddFiles
        private void AddFiles(string[] arrParams)
        {
            DataTable dtbl = new DataTable();
            objComm = new classes.CommonClass();
            string[] arrSep = { objComm.SecondaryRecordDivider };
            string[] arrRecords = arrParams[1].Split(arrSep, StringSplitOptions.None);
            string[] arrNew = arrParams[2].Split(arrSep, StringSplitOptions.None);
            int intSrl = 0;
            string strAccnNo = string.Empty;
            string strPID = string.Empty;
            string strFile = string.Empty;
            string strMimeType = string.Empty;
            string strUserID = arrParams[3];
            DICOMLib.DicomDecoder dd = new DICOMLib.DicomDecoder();

            try
            {
                dtbl = CreateStudyFileTable();
                if (arrRecords.Length > 0)
                {
                    if (arrRecords[0].Trim() != "")
                    {
                        for (int i = 0; i < arrRecords.Length; i = i + 4)
                        {
                            DataRow dr = dtbl.NewRow();
                            intSrl = intSrl + 1;
                            dr["file_srl_no"] = intSrl;
                            dr["file_name"] = strFile = arrRecords[i + 1].Trim().Substring(arrRecords[i + 1].Trim().LastIndexOf("/") + 1, (arrRecords[i + 1].Trim().Length - (arrRecords[i + 1].Trim().LastIndexOf("/") + 1)));
                            dr["file_type"] = arrRecords[i + 2].Trim();
                            if (arrRecords[i + 2].Trim() == "D") dr["file_type_desc"] = "DICOM";
                            else if (arrRecords[i + 2].Trim() == "I")
                            {
                                strMimeType = classes.MIMEAssistant.GetMIMEType(Server.MapPath("~") + "/CaseList/MSTemp/" + strUserID + "/" + strFile);
                                switch (strMimeType)
                                {
                                    case "image/jpeg":
                                        dr["file_type_desc"] = "JPG/JPEG";
                                        break;
                                    case "image/gif":
                                        dr["file_type_desc"] = "GIF";
                                        break;
                                    case "image/png":
                                        dr["file_type_desc"] = "PNG";
                                        break;
                                    case "image/bmp":
                                        dr["file_type_desc"] = "BMP";
                                        break;
                                }

                            }
                            dtbl.Rows.Add(dr);

                            if (i == 0)
                            {
                                dd.DicomFileName = arrRecords[i + 1].Trim();
                                List<string> str = dd.dicomInfo;
                                strAccnNo = GetAccessionNo(str);
                                strPID = GetAccessionNo(str);
                            }
                        }

                    }
                }

                for (int i = 0; i < arrNew.Length; i = i + 2)
                {
                    DataRow drNew = dtbl.NewRow();
                    intSrl = intSrl + 1;
                    drNew["file_srl_no"] = intSrl;
                    drNew["file_name"] = strFile = arrNew[i].Trim().Substring(arrNew[i].Trim().LastIndexOf("/") + 1, (arrNew[i].Trim().Length - (arrNew[i].Trim().LastIndexOf("/") + 1)));
                    drNew["file_type"] = arrNew[i + 1];
                    if (arrNew[i + 1].Trim() == "D") drNew["file_type_desc"] = "DICOM";
                    else if (arrNew[i + 1].Trim() == "I")
                    {
                        strMimeType = classes.MIMEAssistant.GetMIMEType(Server.MapPath("~") + "/CaseList/MSTemp/" + strUserID + "/" + strFile);
                        switch (strMimeType)
                        {
                            case "image/jpeg":
                                drNew["file_type_desc"] = "JPG/JPEG";
                                break;
                            case "image/gif":
                                drNew["file_type_desc"] = "GIF";
                                break;
                            case "image/png":
                                drNew["file_type_desc"] = "PNG";
                                break;
                            case "image/bmp":
                                drNew["file_type_desc"] = "BMP";
                                break;
                        }

                    }

                    if (strAccnNo.Trim() == string.Empty)
                    {
                        if (i == 0)
                        {
                            dd.DicomFileName = arrNew[i].Trim();
                            List<string> str = dd.dicomInfo;
                            strAccnNo = GetAccessionNo(str);
                            strPID = GetAccessionNo(str);
                        }
                    }
                }

                grdSF.DataSource = dtbl;
                grdSF.DataBind();
                spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"\" />";

                spnVal.InnerHtml = "<input type=\"hidden\" id=\"hdnAccNo\" value=\"" + strAccnNo + "\" />";
                spnVal.InnerHtml += "<input type=\"hidden\" id=\"hdnPid\" value=\"" + strPID + "\" />";
            }
            catch (Exception ex)
            {
                spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + ex.Message.Trim() + "\" />";
                spnVal.InnerHtml = "<input type=\"hidden\" id=\"hdnAccNo\" value=\"\" />";
                spnVal.InnerHtml += "<input type=\"hidden\" id=\"hdnPid\" value=\"\" />";
            }
            finally
            {
                dtbl.Dispose();
                objComm = null;
                dd = null;
            }
        }
        #endregion

        #region DeleteFiles
        private void DeleteFiles(string[] arrParams)
        {
            DataTable dtbl = new DataTable();
            objComm = new classes.CommonClass();
            int intID = Convert.ToInt32(arrParams[1]);
            string[] arrSep = { objComm.SecondaryRecordDivider };
            string[] arrRecords = arrParams[2].Split(arrSep, StringSplitOptions.None);
            string strFolder = arrParams[3];
            string strFileName = string.Empty;
            int intSrl = 0;
            int intLoop = 0;
            string strAccnNo = string.Empty;
            string strPID = string.Empty;
            DICOMLib.DicomDecoder dd = new DICOMLib.DicomDecoder();

            try
            {
                dtbl = CreateStudyFileTable();
                strFolder = strFolder.Replace("\\", "/");

                if (arrRecords[0].Trim() != "")
                {
                    for (int i = 0; i < arrRecords.Length; i = i + 4)
                    {
                        if (Convert.ToInt32(arrRecords[i]) != intID)
                        {
                            DataRow dr = dtbl.NewRow();
                            intSrl = intSrl + 1;
                            dr["file_srl_no"] = intSrl;
                            dr["file_name"] = arrRecords[i + 1].Trim();
                            dr["file_type"] = arrRecords[i + 2].Trim();
                            dr["file_type_desc"] = arrRecords[i + 3].Trim();
                            dtbl.Rows.Add(dr);

                            if (intLoop == 0)
                            {
                                dd.DicomFileName = strFolder + "/" + arrRecords[i + 1].Trim();
                                List<string> str = dd.dicomInfo;
                                strAccnNo = GetAccessionNo(str);
                                strPID = GetAccessionNo(str);
                            }

                            intLoop = intLoop + 1;
                        }
                        else
                            strFileName = arrRecords[i + 1].Trim();
                    }
                }

                grdSF.DataSource = dtbl;
                grdSF.DataBind();
                spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"\" />";

                spnVal.InnerHtml = "<input type=\"hidden\" id=\"hdnAccNo\" value=\"" + strAccnNo + "\" />";
                spnVal.InnerHtml += "<input type=\"hidden\" id=\"hdnPid\" value=\"" + strPID + "\" />";


                if (strFileName.Trim() != string.Empty)
                {
                    if (File.Exists(strFolder + "/" + strFileName))
                    {
                        File.Delete(strFolder + "/" + strFileName);
                    }
                }

            }
            catch (Exception ex)
            {
                spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + ex.Message.Trim() + "\" />";
                spnVal.InnerHtml = "<input type=\"hidden\" id=\"hdnAccNo\" value=\"\" />";
                spnVal.InnerHtml += "<input type=\"hidden\" id=\"hdnPid\" value=\"\" />";
            }
            finally
            {
                dtbl.Dispose();
                objComm = null;
                dd = null;
            }
        }
        #endregion

        #region ClearFiles
        private void ClearFiles(string[] arrParams)
        {
            DataTable dtbl = new DataTable();
            objComm = new classes.CommonClass();

            string[] arrSep = { objComm.SecondaryRecordDivider };
            string[] arrRecords = arrParams[0].Split(arrSep, StringSplitOptions.None);
            string strFolder = arrParams[1];
            string strFileName = string.Empty;


            try
            {
                dtbl = CreateStudyFileTable();
                if (arrRecords[0].Trim() != "")
                {
                    strFolder = strFolder.Replace("\\", "/");
                    for (int i = 0; i < arrRecords.Length; i = i + 4)
                    {
                        strFileName = arrRecords[i + 1].Trim();
                        if (strFileName.Trim() != string.Empty)
                        {
                            if (File.Exists(strFolder + "/" + strFileName))
                            {
                                File.Delete(strFolder + "/" + strFileName);
                            }
                        }
                    }
                }

                grdSF.DataSource = dtbl;
                grdSF.DataBind();
                spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"\" />";

                spnVal.InnerHtml = "<input type=\"hidden\" id=\"hdnAccNo\" value=\"\" />";
                spnVal.InnerHtml += "<input type=\"hidden\" id=\"hdnPid\" value=\"\" />";
            }
            catch (Exception ex)
            {
                spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + ex.Message.Trim() + "\" />";
                spnVal.InnerHtml = "<input type=\"hidden\" id=\"hdnAccNo\" value=\"\" />";
                spnVal.InnerHtml += "<input type=\"hidden\" id=\"hdnPid\" value=\"\" />";
            }
            finally
            {
                dtbl.Dispose();
                objComm = null;
            }
        }
        #endregion

        #region CreateStudyFileTable
        private DataTable CreateStudyFileTable()
        {
            DataTable dtbl = new DataTable();
            dtbl.Columns.Add("file_srl_no", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("file_name", System.Type.GetType("System.String"));
            dtbl.Columns.Add("file_type", System.Type.GetType("System.String"));
            dtbl.Columns.Add("file_type_desc", System.Type.GetType("System.String"));
            dtbl.TableName = "files";
            return dtbl;
        }
        #endregion

        #endregion

        #endregion

        #region File Methods

        #region SetFile
        private void SetFile(byte[] DocData, string strFileName, string strPath)
        {
            string strFilePath = Server.MapPath("~") + "/" + strPath + "/" + strFileName;

            if (!Directory.Exists(Server.MapPath("~") + "/" + strPath)) Directory.CreateDirectory(Server.MapPath("~") + "/" + strPath);

            using (FileStream fs = new FileStream(strFilePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                fs.Write(DocData, 0, DocData.Length);
                fs.Flush();
                fs.Close();
            }

        }
        #endregion

        #region GetFileBytes
        private byte[] GetFileBytes(string strFileName)
        {
            byte[] buff = File.ReadAllBytes(strFileName);
            return buff;
        }
        #endregion

        #endregion

        

        #region FetchBreeds (AjaxPro Method)
        [AjaxPro.AjaxMethod()]
        public string[] FetchBreeds(string[] arrParams)
        {
            string strReturn = string.Empty; string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            objCore = new Core.Case.CaseStudy();

            int i = 0;
            string[] arrRet = new string[0];

            try
            {

                objCore.SPECIES_ID = Convert.ToInt32(arrParams[0].Trim());


                bReturn = objCore.FetchSpeciesWiseBreed(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {

                    if (ds.Tables["Breed"].Rows.Count > 0)
                    {

                        arrRet = new string[(ds.Tables["Breed"].Rows.Count * 2) + 3];
                        arrRet[0] = "true";
                        arrRet[1] = "00000000-0000-0000-0000-000000000000";
                        arrRet[2] = "Select One";
                        i = 3;

                        foreach (DataRow dr in ds.Tables["Breed"].Rows)
                        {
                            arrRet[i] = Convert.ToString(dr["id"]);
                            arrRet[i + 1] = Convert.ToString(dr["name"]).Trim();
                            i = i + 2;
                        }
                    }
                    else
                    {
                        arrRet = new string[3];
                        arrRet[0] = "true";
                        arrRet[1] = "00000000-0000-0000-0000-000000000000";

                        if (Convert.ToInt32(arrParams[0].Trim()) == 0)
                        {
                            arrRet[2] = "Please select a species";
                        }
                        else
                        {
                            arrRet[2] = "No breed found for this species";
                        }
                    }



                }
                else
                {

                    arrRet = new string[2];
                    arrRet[0] = "false";
                    arrRet[1] = strCatchMessage.Trim();
                }

            }
            catch (Exception ex)
            {
                arrRet = new string[2];
                arrRet[0] = "catch";
                arrRet[1] = ex.Message.ToString();
            }
            finally
            {
                ds.Dispose();
            }
            return arrRet;
        }
        #endregion

        #region FetchPhysicians (AjaxPro Method)
        [AjaxPro.AjaxMethod()]
        public string[] FetchPhysicians(string[] arrParams)
        {
            string strReturn = string.Empty; string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            objCore = new Core.Case.CaseStudy();

            int i = 0;
            string[] arrRet = new string[0];

            try
            {

                objCore.INSTITUTION_ID = new Guid(arrParams[0].Trim());
                bReturn = objCore.FetchInstitutionWisePhysician(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {
                    arrRet = new string[(ds.Tables["Physicians"].Rows.Count * 2) + 6];
                    arrRet[0] = "true";



                    if (ds.Tables["Physicians"].Rows.Count > 0)
                    {
                        arrRet[1] = "00000000-0000-0000-0000-000000000000";
                        arrRet[2] = "Select One";
                        i = 3;

                        foreach (DataRow dr in ds.Tables["Physicians"].Rows)
                        {
                            arrRet[i] = Convert.ToString(dr["id"]);
                            arrRet[i + 1] = Convert.ToString(dr["name"]).Trim();
                            i = i + 2;
                        }
                    }
                    else
                    {
                        arrRet = new string[3];
                        arrRet[0] = "true";
                        arrRet[1] = "00000000-0000-0000-0000-000000000000";
                        arrRet[2] = "Please select an instititution";
                        i = 3;
                    }
                    if (ds.Tables["Consult"].Rows.Count > 0)
                    {
                        arrRet[i] = Convert.ToString(ds.Tables["Consult"].Rows[0]["consult_applicable"]);
                        arrRet[i + 1] = Convert.ToString(ds.Tables["Consult"].Rows[0]["code"]);
                        arrRet[i + 2] = Convert.ToString(Convert.ToInt32(ds.Tables["Consult"].Rows[0]["patient_id_srl"]) + 1);
                    }
                    else
                    {
                        arrRet[i] = "N";
                        arrRet[i + 1] = "";
                        arrRet[i + 2] = "";
                    }

                }
                else
                {

                    arrRet = new string[2];
                    arrRet[0] = "false";
                    arrRet[1] = strCatchMessage.Trim();
                }

            }
            catch (Exception ex)
            {
                arrRet = new string[2];
                arrRet[0] = "catch";
                arrRet[1] = ex.Message.ToString();
            }
            finally
            {
                ds.Dispose();
            }
            return arrRet;
        }
        #endregion

        #region FetchRegPhysicians (AjaxPro Method)
        [AjaxPro.AjaxMethod()]
        public string[] FetchRegPhysicians(string arrParam)
        {
            string strReturn = string.Empty; string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            objCore = new Core.Case.CaseStudy();

            int i = 0;
            string[] arrRet = new string[0];

            try
            {
                objCore.INSTITUTION_ID = new Guid(arrParam.Trim());
                bReturn = objCore.FetchRegInstitutionDetails(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {
                    arrRet = new string[(ds.Tables["Physicians"].Rows.Count * 2) + 6];
                    arrRet[0] = "true";



                    if (ds.Tables["Physicians"].Rows.Count > 0)
                    {
                        arrRet[1] = "00000000-0000-0000-0000-000000000000";
                        arrRet[2] = "Select One";
                        i = 3;

                        foreach (DataRow dr in ds.Tables["Physicians"].Rows)
                        {
                            arrRet[i] = Convert.ToString(dr["physician_id"]);
                            arrRet[i + 1] = Convert.ToString(dr["physician_name"]).Trim();
                            i = i + 2;
                        }
                    }
                    else
                    {
                        arrRet = new string[3];
                        arrRet[0] = "true";
                        arrRet[1] = "00000000-0000-0000-0000-000000000000";
                        arrRet[2] = "Please select an instititution";
                        i = 3;
                    }
                    if (ds.Tables["Institutions_Reg"].Rows.Count > 0)
                    {
                        arrRet[i] = Convert.ToString(ds.Tables["Institutions_Reg"].Rows[0]["name"]);
                    }
                    else
                    {
                        arrRet[i] = "N";
                        arrRet[i + 1] = "";
                        arrRet[i + 2] = "";
                    }
                }
                else
                {

                    arrRet = new string[2];
                    arrRet[0] = "false";
                    arrRet[1] = strCatchMessage.Trim();
                }

            }
            catch (Exception ex)
            {
                arrRet = new string[2];
                arrRet[0] = "catch";
                arrRet[1] = ex.Message.ToString();
            }
            finally
            {
                ds.Dispose();
            }
            return arrRet;
        }
        #endregion

        #region SaveRecord (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] SaveRecord(string[] ArrRecord, string[] ArrSTs, string[] ArrDCM, string[] ArrImg)
        {
            bool bReturn = false; bool bFileValid = true;
            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty; string strFile = string.Empty;
            string strFileName = string.Empty; Guid UserID = Guid.Empty; string strOutputMsg = string.Empty;
            string strDCMMODIFYEXEPATH = string.Empty; string strInsName = string.Empty; string strSUID = string.Empty;
            string strExtn = string.Empty;

            objComm = new classes.CommonClass();
            objCore = new Core.Case.CaseStudy();
            int intListIndex = 0;
            Core.Case.StudyTypeList[] objSTs = new Core.Case.StudyTypeList[0];
            Core.Case.HeaderImageList[] objImg = new Core.Case.HeaderImageList[0];
            Core.Case.HeaderDICOMList[] objDCM = new Core.Case.HeaderDICOMList[0];
            Core.Case.HeaderDocumentList[] objDocs = new Core.Case.HeaderDocumentList[0];
            DICOMLib.DicomDecoder dd = new DICOMLib.DicomDecoder();

            try
            {
                #region Header Values
                objCore.SESSION_ID = "MS1D" + DateTime.Now.ToString("MMddyyHHmmss") + CoreCommon.RandomString(3);
                objCore.PATIENT_FIRST_NAME = ArrRecord[0].Trim();
                objCore.PATIENT_LAST_NAME = ArrRecord[1].Trim();
                objCore.PATIENT_NAME = (ArrRecord[1].Trim() + " " + ArrRecord[0].Trim()).Trim();
                objCore.PATIENT_GENDER = ArrRecord[2].Trim();
                objCore.OWNER_FIRST_NAME = ArrRecord[3].Trim();
                objCore.OWNER_LAST_NAME = ArrRecord[4].Trim();
                objCore.SEX_NEUTERED = ArrRecord[5].Trim();
                objCore.PATIENT_WEIGHT = Convert.ToDecimal(ArrRecord[6]);
                objCore.WEIGHT_UOM = ArrRecord[7].Trim();
                objCore.PATIENT_DOB = Convert.ToDateTime(ArrRecord[8]);
                objCore.PATIENT_AGE = ArrRecord[9].Trim();
                objCore.SPECIES_ID = Convert.ToInt32(ArrRecord[10]);
                objCore.BREED_ID = new Guid(ArrRecord[11]);
                objCore.STUDY_DATE = Convert.ToDateTime(ArrRecord[12]);
                objCore.PATIENT_ID = ArrRecord[13].Trim();
                objCore.ACCESSION_NO = ArrRecord[14].Trim();
                objCore.REASON = ArrRecord[15].Trim();
                objCore.PHYSICIAN_NOTE = ArrRecord[16].Trim();
                objCore.MODALITY_ID = Convert.ToInt32(ArrRecord[17]);
                objCore.PRIORITY_ID = Convert.ToInt32(ArrRecord[18]);
                objCore.INSTITUTION_ID = new Guid(ArrRecord[19]);
                strInsName = ArrRecord[20].Trim();
                objCore.PHYSICIAN_ID = new Guid(ArrRecord[21]);
                objCore.CONSULT_APPLIED = ArrRecord[22].Trim();
                objCore.CATEGORY_ID = Convert.ToInt32(ArrRecord[23]);
                strDCMMODIFYEXEPATH = ArrRecord[24].Trim();
                objCore.USER_ID = UserID = new Guid(ArrRecord[25]);
                #endregion

                #region Study Types
                objSTs = new Core.Case.StudyTypeList[(ArrSTs.Length)];

                for (int i = 0; i < objSTs.Length; i++)
                {
                    objSTs[i] = new Core.Case.StudyTypeList();
                    objSTs[i].SERIAL_NUMBER = i + 1;
                    objSTs[i].ID = new Guid(ArrSTs[intListIndex]);
                    intListIndex = intListIndex + 1;
                }
                #endregion

                #region DICOM files
                intListIndex = 0;
                objDCM = new Core.Case.HeaderDICOMList[(ArrDCM.Length / 2)];

                for (int i = 0; i < objDCM.Length; i++)
                {
                    strFile = ArrDCM[intListIndex + 1].Trim();
                    strFileName = Server.MapPath("~/CaseList/DCMTemp/" + UserID.ToString() + "/") + ArrDCM[intListIndex + 1].Trim();
                    strFileName = strFileName.Replace("ajaxpro\\", "");
                    dd.DicomFileName = strFileName;
                    List<string> str = dd.dicomInfo;

                    if (i == 0)
                    {
                        strSUID = GetStudyUID(str).Trim();
                        objCore.STUDY_UID = strSUID;
                    }

                    #region Update DICOM Files with unoiform Study UID & Institution name
                    if ((GetStudyUID(str).Trim() == strSUID) && (GetInstitutionName(str).Trim() == strInsName))
                    {
                        bFileValid = true;
                    }
                    else
                    {
                        if (ModifyDCMFile(strDCMMODIFYEXEPATH, strInsName, strSUID, strFileName, ref strOutputMsg, ref strReturnMsg))
                        {
                            dd.DicomFileName = strFileName;
                            str = dd.dicomInfo;
                            if ((GetStudyUID(str).Trim() != strSUID) || (GetInstitutionName(str).Trim() != strInsName))
                            {
                                bFileValid = false;
                                strReturnMsg = strOutputMsg;
                            }
                            else
                                bFileValid = true;
                        }
                        else
                            bFileValid = false;
                    }
                    #endregion


                    if (bFileValid)
                    {
                        objDCM[i] = new Core.Case.HeaderDICOMList();
                        objDCM[i].SERIAL_NUMBER = Convert.ToInt32(ArrDCM[intListIndex]);
                        objDCM[i].FILE_NAME = ArrDCM[intListIndex + 1].Trim();
                        objDCM[i].FILE_CONTENT = GetFileBytes(strFileName);
                        intListIndex = intListIndex + 2;

                        if (File.Exists(strFileName + ".bak"))
                        {
                            File.Delete(strFileName + ".bak");
                        }
                    }
                    else
                    {
                        strReturnMsg = strOutputMsg;
                        break;
                    }
                }
                #endregion

                if (bFileValid)
                {
                    if (ArrDCM.Length == 0)
                    {
                        #region Image Files
                        intListIndex = 0;
                        objImg = new Core.Case.HeaderImageList[(ArrImg.Length / 2)];

                        for (int i = 0; i < objImg.Length; i++)
                        {
                            if (i == 0)
                            {
                                objCore.STUDY_UID = GenrateStudyUID();
                                objCore.SERIES_UID = GenrateSeriesUID();
                                objCore.SERIES_NUMBER = CreateSeriesNumber();
                            }
                            strFileName = Server.MapPath("~/CaseList/IMGTemp/" + UserID.ToString() + "/") + ArrImg[intListIndex + 1].Trim();
                            strFileName = strFileName.Replace("ajaxpro\\", "");


                            objImg[i] = new Core.Case.HeaderImageList();
                            objImg[i].SERIAL_NUMBER = Convert.ToInt32(ArrImg[intListIndex]);
                            objImg[i].FILE_NAME = ArrImg[intListIndex + 1].Trim();
                            objImg[i].FILE_CONTENT = GetFileBytes(strFileName);

                            intListIndex = intListIndex + 2;
                        }
                        #endregion
                    }
                    else
                    {
                        #region Documents
                        intListIndex = 0;
                        objDocs = new Core.Case.HeaderDocumentList[(ArrImg.Length / 2)];

                        for (int i = 0; i < objDocs.Length; i++)
                        {
                            strFileName = Server.MapPath("~/CaseList/IMGTemp/" + UserID.ToString() + "/") + ArrImg[intListIndex + 1].Trim();
                            strFileName = strFileName.Replace("ajaxpro\\", "");
                            strExtn = Path.GetExtension(strFileName);

                            objDocs[i] = new Core.Case.HeaderDocumentList();
                            objDocs[i].SERIAL_NUMBER = Convert.ToInt32(ArrImg[intListIndex]);
                            objDocs[i].ID = new Guid("00000000-0000-0000-0000-000000000000");
                            objDocs[i].NAME = ArrImg[intListIndex + 1].Trim();
                            objDocs[i].FILE_NAME = ArrImg[intListIndex + 1].Trim();
                            objDocs[i].FILE_TYPE = strExtn;
                            objDocs[i].FILE_CONTENT = GetFileBytes(strFileName);

                            intListIndex = intListIndex + 2;
                        }
                        #endregion
                    }

                    intListIndex = 0;
                    bReturn = objCore.SaveManualSubmissionTemp(Server.MapPath("~"), objSTs, objDCM, objImg, objDocs, ref strReturnMsg, ref strCatchMessage);

                    #region Post Saving
                    if (bReturn)
                    {
                        arrRet = new string[3];
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
                    #endregion
                }
                else
                {
                    arrRet = new string[4];
                    arrRet[0] = "false";
                    arrRet[1] = "342";
                    arrRet[2] = strFile;
                    arrRet[3] = strReturnMsg;
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
                objCore = null; objComm = null; dd = null;
                strReturnMsg = null; strCatchMessage = null;
            }
            return arrRet;
        }
        #endregion

        #region File Update Methods

        #region DICOM

        #region GetStudyUID
        private string GetStudyUID(List<string> str)
        {

            string StudyUID = string.Empty;
            string s1, s4, s5, s11, s12;

            // Add items to the List View Control
            for (int i = 0; i < str.Count; ++i)
            {
                s1 = str[i];

                ExtractStrings(s1, out s4, out s5, out s11, out s12);

                if ((s11.ToUpper() == "0020") && (s12.ToUpper() == "000D"))
                {
                    StudyUID = s5.Replace("\0", "");
                    break;
                }

            }
            return StudyUID;

        }
        #endregion

        #region GetInstitutionName
        private string GetInstitutionName(List<string> str)
        {

            string InstName = string.Empty;
            string s1, s4, s5, s11, s12;

            // Add items to the List View Control
            for (int i = 0; i < str.Count; ++i)
            {
                s1 = str[i];

                ExtractStrings(s1, out s4, out s5, out s11, out s12);

                if ((s11.ToUpper() == "0008") && (s12.ToUpper() == "0080"))
                {
                    InstName = s5.Replace("\0", "");
                    InstName = s5.Replace("^", " ");
                    break;
                }

            }
            return InstName;

        }
        #endregion

        #region GetAccessionNo
        private string GetAccessionNo(List<string> str)
        {

            string AccNo = string.Empty;
            string s1, s4, s5, s11, s12;

            // Add items to the List View Control
            for (int i = 0; i < str.Count; ++i)
            {
                s1 = str[i];

                ExtractStrings(s1, out s4, out s5, out s11, out s12);

                if ((s11.ToUpper() == "0008") && (s12.ToUpper() == "0050"))
                {
                    AccNo = s5.Replace("\0", "");
                    break;
                }

            }
            return AccNo;

        }
        #endregion

        #region GetPatientID
        private string GetPatientID(List<string> str)
        {

            string PID = string.Empty;
            string s1, s4, s5, s11, s12;

            // Add items to the List View Control
            for (int i = 0; i < str.Count; ++i)
            {
                s1 = str[i];

                ExtractStrings(s1, out s4, out s5, out s11, out s12);

                if ((s11.ToUpper() == "0010") && (s12.ToUpper() == "0020"))
                {
                    PID = s5.Replace("\0", "");
                    break;
                }

            }
            return PID;

        }
        #endregion

        #region GetallTags
        private string[] GetallTags(List<string> str)
        {

            string strDescription = string.Empty;
            string UserCaseID = string.Empty;
            string ModalityID = string.Empty;
            string StrPName = string.Empty;
            string StudyDt = string.Empty;
            string StudyTime = string.Empty;
            string sDt = string.Empty;
            string sTime = string.Empty;
            string studyDtTime = string.Empty;
            string UserSeriesID = string.Empty;
            string SeriesNumber = string.Empty;
            string InstitutionName = string.Empty;
            string PatientID = string.Empty;
            string AccnNo = string.Empty;
            string RefPhys = string.Empty;
            string Manufacturer = string.Empty;
            string StationName = string.Empty;
            string Model = string.Empty;
            string ModalityAETitle = string.Empty;
            string Reason = string.Empty;
            string BirthDt = string.Empty;
            string bDt = string.Empty;
            string PatientSex = string.Empty;
            string PatientAge = string.Empty;
            //string PatientWt = string.Empty;//(0010,1030)
            //string Species = string.Empty;//(0010,2201)
            //string Breed = string.Empty;//(0010,2292)
            //string Owner = string.Empty;//(0010,2297)
            string PriorityID = string.Empty;


            // Add items to the List View Control
            for (int i = 0; i < str.Count; ++i)
            {
                string s1, s4, s5, s11, s12;
                s1 = str[i];

                ExtractStrings(s1, out s4, out s5, out s11, out s12);

                #region commented
                /*if ((s11.ToUpper() == "0008") && (s12.ToUpper() == "103E"))
                {
                    strDescription = s5.Replace("\0", "");
                    strDescription = s5.Replace("<", " ");
                    strDescription = s5.Replace(">", " ");

                }

                else if ((s11.ToUpper() == "0008") && (s12.ToUpper() == "0060"))
                {
                    ModalityID = s5.Replace("\0", "");

                }


                else if ((s11.ToUpper() == "0010") && (s12.ToUpper() == "0010"))
                {
                    Strname = s5.Replace("\0", "");
                    Strname = s5.Replace("^", " ");

                }
                else if ((s11.ToUpper() == "0010") && (s12.ToUpper() == "0030"))
                {
                    DOB = s5.Replace("\0", "");
                    DOB = DOB.Trim();
                    if (DOB != "")
                    {
                        string yy = DOB.Substring(0, 4);
                        string MM = DOB.Substring(4, 2);
                        string DD = DOB.Substring(6, 2);
                        result = yy + "-" + MM + "-" + DD;
                    }
                    else
                    {
                        result = "0000-00-00";
                    }
                }

                else */
                #endregion

                #region Tags
                s5 = s5.Replace("\t", "");
                s5 = s5.Replace("\n", "");

                switch (s11.ToUpper())
                {
                    case "0008":
                        #region s11 =0008
                        switch (s12.ToUpper())
                        {
                            case "0020":
                                StudyDt = s5.Replace("\0", "");
                                StudyDt = StudyDt.Trim();
                                if ((StudyDt.Length == 8))
                                {
                                    string yyyy = StudyDt.Substring(0, 4);
                                    string MM = StudyDt.Substring(4, 2);
                                    string DD = StudyDt.Substring(6, 2);
                                    sDt = yyyy + "-" + MM + "-" + DD;
                                }
                                else
                                {
                                    sDt = "0000-00-00";
                                }
                                break;
                            case "0030":
                                StudyTime = s5.Replace("\0", "");
                                StudyTime = StudyDt.Trim();
                                if ((StudyTime.Length == 13))
                                {
                                    string Hr = StudyDt.Substring(0, 2);
                                    string Min = StudyDt.Substring(2, 2);
                                    string Sec = StudyDt.Substring(4, 2);
                                    sTime = Hr + ":" + Min + ":" + Sec;
                                }
                                else
                                {
                                    sTime = "00:00:00";
                                }
                                break;
                            case "0050":
                                AccnNo = s5.Replace("\0", "");
                                break;
                            case "0060":
                                ModalityID = s5.Replace("\0", "");
                                break;
                            case "0070":
                                Manufacturer = s5.Replace("\0", "");
                                break;
                            case "0080":
                                InstitutionName = s5.Replace("\0", "");
                                InstitutionName = s5.Replace("^", " ");
                                break;
                            case "0090":
                                RefPhys = s5.Replace("\0", "");
                                RefPhys = s5.Replace("^", " ");
                                break;
                            case "1010":
                                StationName = s5.Replace("\0", "");
                                break;
                            case "1090":
                                Model = s5.Replace("\0", "");
                                break;
                            default:
                                break;
                        }
                        #endregion
                        break;
                    case "0010":
                        #region s11 =0010
                        switch (s12.ToUpper())
                        {
                            case "0010":
                                StrPName = s5.Replace("\0", "");
                                StrPName = s5.Replace("^", " ");
                                break;
                            case "0020":
                                PatientID = s5.Replace("\0", "");
                                break;
                            case "0030":
                                BirthDt = s5.Replace("\0", "");
                                BirthDt = BirthDt.Trim();
                                if ((BirthDt.Length == 8))
                                {
                                    string yyyy = BirthDt.Substring(0, 4);
                                    string MM = BirthDt.Substring(4, 2);
                                    string DD = BirthDt.Substring(6, 2);
                                    bDt = yyyy + "-" + MM + "-" + DD;
                                }
                                else
                                {
                                    bDt = "0000-00-00";
                                }
                                break;
                            case "0040":
                                PatientSex = s5.Replace("\0", "");
                                break;
                            case "1010":
                                PatientAge = s5.Replace("\0", "");
                                break;
                        }
                        #endregion
                        break;
                    case "0020":
                        #region s11 =0020
                        switch (s12.ToUpper())
                        {
                            case "000D":
                                UserCaseID = s5.Replace("\0", "");
                                break;
                            case "000E":
                                UserSeriesID = s5.Replace("\0", "");
                                break;
                            case "0011":
                                SeriesNumber = s5.Replace("\0", "");
                                break;
                        }
                        #endregion
                        break;
                    case "0032":
                        #region s11 =0032
                        switch (s12.ToUpper())
                        {
                            case "000C":
                                PriorityID = s5.Replace("\0", "");
                                break;
                        }
                        #endregion
                        break;
                    case "0040":
                        #region s11 =0040
                        switch (s12.ToUpper())
                        {
                            case "0241":
                                ModalityAETitle = s5.Replace("\0", "");
                                break;
                            case "1002":
                                Reason = s5.Replace("\0", "");
                                break;

                        }
                        #endregion
                        break;
                    default:
                        break;
                }
                #endregion
            }

            studyDtTime = sDt + " " + sTime;

            string[] arr = new string[17];
            arr[0] = UserCaseID;
            arr[1] = ModalityID;
            arr[2] = PatientID;
            arr[3] = StrPName;
            arr[4] = studyDtTime;
            arr[5] = InstitutionName;
            arr[6] = AccnNo;
            arr[7] = RefPhys;
            arr[8] = Manufacturer;
            arr[9] = StationName;
            arr[10] = Model;
            arr[11] = ModalityAETitle;
            arr[12] = Reason;
            arr[13] = bDt;
            arr[14] = PatientSex;
            arr[15] = PatientAge;
            arr[16] = PriorityID;

            return arr;

        }
        #endregion

        #region ExtractStrings
        void ExtractStrings(string s1, out string s4, out string s5, out string s11, out string s12)
        {
            int ind;
            string s2, s3;
            ind = s1.IndexOf("//");
            s2 = s1.Substring(0, ind);
            s11 = s1.Substring(0, 4);
            s12 = s1.Substring(4, 4);
            s3 = s1.Substring(ind + 2);
            ind = s3.IndexOf(":");
            s4 = s3.Substring(0, ind);
            s5 = s3.Substring(ind + 1);
        }
        #endregion

        #region ModifyDCMFile
        private bool ModifyDCMFile(string strDCMMODIFYEXEPATH, string strInsName, string strSUID, string strDCMPath, ref string strOutputMsg, ref string strReturnMessage)
        {
            bool bRet = false;
            string strProcOutput = string.Empty;
            string strProcError = string.Empty;

            try
            {
                Process ProcModSUID = new Process();
                ProcModSUID.StartInfo.UseShellExecute = false;
                ProcModSUID.StartInfo.FileName = strDCMMODIFYEXEPATH;
                ProcModSUID.StartInfo.Arguments = "-i \"(0020,000D)=" + strSUID + "\"" + " " + strDCMPath;
                ProcModSUID.StartInfo.RedirectStandardOutput = true;
                ProcModSUID.StartInfo.RedirectStandardError = true;
                ProcModSUID.Start();
                strProcOutput = ProcModSUID.StandardOutput.ReadToEnd();
                strProcError = ProcModSUID.StandardError.ReadToEnd();
                strOutputMsg = strProcOutput.Trim();
                bRet = true;


            }
            catch (Exception ex)
            {
                strReturnMessage = ex.Message.Trim();
            }


            if (bRet)
            {
                try
                {
                    Process ProcModInst = new Process();
                    ProcModInst.StartInfo.UseShellExecute = false;
                    ProcModInst.StartInfo.FileName = strDCMMODIFYEXEPATH;
                    ProcModInst.StartInfo.Arguments = "-i \"(0008,0080)=" + strInsName + "\"" + " " + strDCMPath;
                    ProcModInst.StartInfo.RedirectStandardOutput = true;
                    ProcModInst.StartInfo.RedirectStandardError = true;
                    ProcModInst.Start();
                    strProcOutput = ProcModInst.StandardOutput.ReadToEnd();
                    strProcError = ProcModInst.StandardError.ReadToEnd();
                    strOutputMsg = strProcOutput.Trim();
                    bRet = true;


                }
                catch (Exception ex)
                {
                    strReturnMessage = ex.Message.Trim();
                }

            }

            return bRet;
        }
        #endregion

        #endregion

        #region Images

        #region GenrateStudyUID
        private string GenrateStudyUID()
        {

            string strSUID = "";
            strSUID = DicomUid.GenerateUid().UID;
            return strSUID;
        }
        #endregion

        #region GenrateSeriesUID
        private string GenrateSeriesUID()
        {

            string strSeriesUID = "";
            strSeriesUID = DicomUid.GenerateUid().UID;
            return strSeriesUID;
        }
        #endregion

        #region CreateSeriesNumber
        private string CreateSeriesNumber()
        {
            Random random = new Random();
            string combination = "0123456789";
            StringBuilder sbRef = new StringBuilder();
            string strSeriesNo = "";

            for (int i = 0; i < 6; i++)
            {
                sbRef.Append(combination[random.Next(combination.Length)]);

            }
            strSeriesNo = sbRef.ToString();
            return strSeriesNo;
        }
        #endregion

        #endregion

        #endregion
    }
}