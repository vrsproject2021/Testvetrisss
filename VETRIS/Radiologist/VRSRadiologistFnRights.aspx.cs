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

namespace VETRIS.Radiologist
{
    [AjaxPro.AjaxNamespace("VRSRadiologistFnRights")]
    public partial class VRSRadiologistFnRights : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.Master.Radiologist objCore = null;
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSRadiologistFnRights));
            SetAttributes();
            if ((!CallBackRights.CausedCallback) && (!CallBackModality.CausedCallback) 
                && (!CallBackInst.CausedCallback) && (!CallBackSelInst.CausedCallback) 
                && (!CallBackST.CausedCallback) && (!CallBackSelST.CausedCallback)
                && (!CallBackRad.CausedCallback) && (!CallBackSelRad.CausedCallback)
                && (!CallBackSpecies.CausedCallback))
                SetPageValue();
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
            SetCSS(strTheme);
            hdnID.Value = Request.QueryString["id"];
            lblName.Text = Request.QueryString["nm"];
            LockRecord(intMenuID, UserID, SessionID);
        }
        #endregion

        #region SetCSS
        private void SetCSS(string strTheme)
        {
            string strServerPath = ConfigurationManager.AppSettings["ServerPath"];
            lnkSTYLE.Attributes["href"] = strServerPath + "/css/" + strTheme + "/style.css";
            lnkTHEME.Attributes["href"] = strServerPath + "/css/" + strTheme + "/theme.css";
            lnkGRID.Attributes["href"] = strServerPath + "/css/" + strTheme + "/grid_style.css";
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {
            btnSave1.Attributes.Add("onclick", "javascript:btnSave_OnClick();");
            btnSave2.Attributes.Add("onclick", "javascript:btnSave_OnClick();");
            btnReset1.Attributes.Add("onclick", "javascript:btnReset_OnClick();");
            btnReset2.Attributes.Add("onclick", "javascript:btnReset_OnClick();");
            btnClose1.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
            btnClose2.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
        }
        #endregion

        #region LockRecord
        private void LockRecord(int intMenuID, Guid UserID, Guid SessionID)
        {
            CommonFunctions objCF = new CommonFunctions();
            string strCatchMessage = ""; string strReturnMessage = string.Empty; bool bReturn = false;
            
            objComm = new classes.CommonClass();

            try
            {
               
                objCF.RECORD_ID_UI = new Guid(hdnID.Value);
                objCF.MENU_ID = intMenuID;
                objCF.USER_ID = UserID;
                objCF.USER_SESSION_ID = SessionID;

                bReturn = objCF.LockRecordUI(Server.MapPath("~"), ref strReturnMessage, ref strCatchMessage);

                if (!bReturn)
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
                objComm = null; objCF = null;
            }
        }
        #endregion


        #region Functional Rights

        #region CallBackRights_Callback
        protected void CallBackRights_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {

            LoadRights(e.Parameters);
            grdRights.Width = Unit.Percentage(100);
            grdRights.RenderControl(e.Output);
            spnERR.RenderControl(e.Output);
            
        }
        #endregion

        #region LoadRights
        private void LoadRights(string[] arrParams)
        {
            objCore = new Core.Master.Radiologist();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();

            try
            {

                objCore.ID = new Guid(arrParams[0]);
                bReturn = objCore.LoadFunctionalRights(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    grdRights.DataSource = ds.Tables["FnRights"];
                    grdRights.DataBind();
                    spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"\" />";
                }
                else
                {

                    spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + strCatchMessage + "\" />";
                }
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

        #endregion

        #region Modality Rights

        #region CallBackModality_Callback
        protected void CallBackModality_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {

            LoadModality(e.Parameters);
            grdModality.Width = Unit.Percentage(100);
            grdModality.RenderControl(e.Output);
            spnModErr.RenderControl(e.Output);

        }
        #endregion

        #region LoadModality
        private void LoadModality(string[] arrParams)
        {
            objCore = new Core.Master.Radiologist();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();

            try
            {

                objCore.ID = new Guid(arrParams[0]);
                bReturn = objCore.LoadModalityRights(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    grdModality.DataSource = ds.Tables["Modality"];
                    grdModality.DataBind();
                    spnModErr.InnerHtml = "<input type=\"hidden\" id=\"hdnCBModErr\" value=\"\" />";
                }
                else
                {

                    spnModErr.InnerHtml = "<input type=\"hidden\" id=\"hdnCBModErr\" value=\"" + strCatchMessage + "\" />";
                }
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

        #endregion

        #region Species Grid

        #region CallBackSpecies_Callback
        protected void CallBackSpecies_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {

            LoadSpecies(e.Parameters);
            grdSpecies.Width = Unit.Percentage(100);
            grdSpecies.RenderControl(e.Output);
            spnSpeciesErr.RenderControl(e.Output);

        }
        #endregion

        #region LoadSpecies
        private void LoadSpecies(string[] arrParams)
        {
            objCore = new Core.Master.Radiologist();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();

            try
            {

                objCore.ID = new Guid(arrParams[0]);
                bReturn = objCore.LoadSpeciesRights(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    grdSpecies.DataSource = ds.Tables["Species"];
                    grdSpecies.DataBind();
                    spnSpeciesErr.InnerHtml = "<input type=\"hidden\" id=\"hdnCBSpeciesErr\" value=\"\" />";
                }
                else
                {

                    spnSpeciesErr.InnerHtml = "<input type=\"hidden\" id=\"hdnCBSpeciesErr\" value=\"" + strCatchMessage + "\" />";
                }
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

        #endregion

        #region Institution Grid

        #region CallBackInst_Callback
        protected void CallBackInst_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            LoadInstitutions(e.Parameters);
            grdInst.Width = Unit.Percentage(99);
            grdInst.RenderControl(e.Output);
            spnInstERR.RenderControl(e.Output);

        }
        #endregion

        #region LoadInstitutions
        private void LoadInstitutions(string[] arrParams)
        {
            objCore = new Core.Master.Radiologist();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();

            try
            {

                objCore.ID = new Guid(arrParams[0]);
                bReturn = objCore.LoadInstitutions(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    grdInst.DataSource = ds.Tables["Institutions"];
                    grdInst.DataBind();
                    spnInstERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrInst\" value=\"\" />";
                }
                else
                {

                    spnInstERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrInst\" value=\"" + strCatchMessage + "\" />";
                }
            }
            catch (Exception ex)
            {
                spnInstERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrInst\" value=\"" + ex.Message.Trim() + "\" />";

            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }
        }
        #endregion

        #endregion

        #region Selected Institution Grid

        #region CallBackSelInst_Callback
        protected void CallBackSelInst_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            string strAction = e.Parameters[0];
            switch (strAction)
            {
                case "L":
                    LoadSelectedInstitutions(e.Parameters);
                    break;
                case "U":
                    UpdateInstitutions(e.Parameters);
                    break;
            }

            grdSelInst.Width = Unit.Percentage(99);
            grdSelInst.RenderControl(e.Output);
            spnSelInstERR.RenderControl(e.Output);
        }
        #endregion

        #region LoadSelectedInstitutions
        private void LoadSelectedInstitutions(string[] arrParams)
        {
            objCore = new Core.Master.Radiologist();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            Guid UserID = Guid.Empty;
            string strFileName = "";

            try
            {

                objCore.ID = new Guid(arrParams[1]);

                bReturn = objCore.LoadExceptionInstitutions(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    grdSelInst.DataSource = ds.Tables["Institutions"];
                    grdSelInst.DataBind();
                    spnErrSelST.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrSelInst\" value=\"\" />";
                }
                else
                    spnErrSelST.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrSelInst\" value=\"" + strCatchMessage + "\" />";


            }
            catch (Exception ex)
            {
                spnErrSelST.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrSelInst\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }
        }
        #endregion

        #region UpdateInstitutions
        private void UpdateInstitutions(string[] arrParams)
        {
            DataTable dtbl = new DataTable();
            objComm = new classes.CommonClass();
            string[] arrSep = { objComm.SecondaryRecordDivider };
            string[] arrRecords = arrParams[1].Split(arrSep, StringSplitOptions.None);
            int intSrl = 0;

            try
            {
                dtbl = CreateInstTable();
                if (arrRecords.Length > 0)
                {
                    if (arrRecords[0].Trim() != "")
                    {
                        for (int i = 0; i < arrRecords.Length; i = i + 3)
                        {
                            DataRow dr = dtbl.NewRow();
                            dr["institution_id"] = arrRecords[i].Trim();
                            dr["institution_code"] = arrRecords[i + 1].Trim();
                            dr["institution_name"] = arrRecords[i + 2].Trim();
                            dtbl.Rows.Add(dr);
                        }

                    }
                }

                grdSelInst.DataSource = dtbl;
                grdSelInst.DataBind();
                spnSelInstERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrSelInst\" value=\"\" />";
            }
            catch (Exception ex)
            {
                spnSelInstERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrSelInst\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                dtbl.Dispose();
                objComm = null;
            }
        }
        #endregion

        #region CreateInstTable
        private DataTable CreateInstTable()
        {
            DataTable dtbl = new DataTable();
            dtbl.Columns.Add("institution_id", System.Type.GetType("System.String"));
            dtbl.Columns.Add("institution_code", System.Type.GetType("System.String"));
            dtbl.Columns.Add("institution_name", System.Type.GetType("System.String"));
            dtbl.TableName = "Institutions";
            return dtbl;
        }
        #endregion

        #endregion

        #region Study Type Grid

        #region CallBackST_Callback
        protected void CallBackST_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            LoadStudyTypes(e.Parameters);
            grdST.Width = Unit.Percentage(99);
            grdST.RenderControl(e.Output);
            spnSTErr.RenderControl(e.Output);

        }
        #endregion

        #region LoadStudyTypes
        private void LoadStudyTypes(string[] arrParams)
        {
            objCore = new Core.Master.Radiologist();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();

            try
            {

                objCore.ID = new Guid(arrParams[0]);
                bReturn = objCore.LoadStudyTypes(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    grdST.DataSource = ds.Tables["StudyTypes"];
                    grdST.DataBind();
                    spnSTErr.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrST\" value=\"\" />";
                }
                else
                {

                    spnSTErr.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrST\" value=\"" + strCatchMessage + "\" />";
                }
            }
            catch (Exception ex)
            {
                spnSTErr.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrST\" value=\"" + ex.Message.Trim() + "\" />";

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

            grdSelST.Width = Unit.Percentage(99);
            grdSelST.RenderControl(e.Output);
            spnErrSelST.RenderControl(e.Output);
        }
        #endregion

        #region LoadSelectedStudyTypes
        private void LoadSelectedStudyTypes(string[] arrParams)
        {
            objCore = new Core.Master.Radiologist();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            Guid UserID = Guid.Empty;
            string strFileName = "";

            try
            {

                objCore.ID = new Guid(arrParams[1]);

                bReturn = objCore.LoadExceptionStudyTypes(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    grdSelST.DataSource = ds.Tables["StudyTypes"];
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
                        for (int i = 0; i < arrRecords.Length; i = i + 3)
                        {
                            DataRow dr = dtbl.NewRow();
                            dr["study_type_id"] = arrRecords[i].Trim();
                            dr["modality"] = arrRecords[i + 1].Trim();
                            dr["study_type"] = arrRecords[i + 2].Trim();
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
            dtbl.Columns.Add("study_type_id", System.Type.GetType("System.String"));
            dtbl.Columns.Add("modality", System.Type.GetType("System.String"));
            dtbl.Columns.Add("study_type", System.Type.GetType("System.String"));
            dtbl.TableName = "StudyTypes";
            return dtbl;
        }
        #endregion

        #endregion

        #region Radiologist Grid

        #region CallBackRad_Callback
        protected void CallBackRad_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            LoadRadiologists(e.Parameters);
            grdRad.Width = Unit.Percentage(99);
            grdRad.RenderControl(e.Output);
            spnRadERR.RenderControl(e.Output);

        }
        #endregion

        #region LoadRadiologists
        private void LoadRadiologists(string[] arrParams)
        {
            objCore = new Core.Master.Radiologist();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();

            try
            {

                objCore.ID = new Guid(arrParams[0]);
                bReturn = objCore.LoadRadiologists(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    grdRad.DataSource = ds.Tables["Radiologists"];
                    grdRad.DataBind();
                    spnRadERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrRad\" value=\"\" />";
                }
                else
                {

                    spnRadERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrRad\" value=\"" + strCatchMessage + "\" />";
                }
            }
            catch (Exception ex)
            {
                spnRadERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrRad\" value=\"" + ex.Message.Trim() + "\" />";

            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }
        }
        #endregion

        #endregion

        #region Selected Radiologist Grid

        #region CallBackSelRad_Callback
        protected void CallBackSelRad_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            string strAction = e.Parameters[0];
            switch (strAction)
            {
                case "L":
                    LoadSelectedrdiologists(e.Parameters);
                    break;
                case "U":
                    UpdateRadiologists(e.Parameters);
                    break;
            }

            grdSelRad.Width = Unit.Percentage(99);
            grdSelRad.RenderControl(e.Output);
            spnSelRadERR.RenderControl(e.Output);
        }
        #endregion

        #region LoadSelectedrdiologists
        private void LoadSelectedrdiologists(string[] arrParams)
        {
            objCore = new Core.Master.Radiologist();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            Guid UserID = Guid.Empty;
            string strFileName = "";

            try
            {

                objCore.ID = new Guid(arrParams[1]);

                bReturn = objCore.LoadOtherRadiologists(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    grdSelRad.DataSource = ds.Tables["Radiologists"];
                    grdSelRad.DataBind();
                    spnSelRadERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrSelRad\" value=\"\" />";
                }
                else
                    spnSelRadERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrSelRad\" value=\"" + strCatchMessage + "\" />";


            }
            catch (Exception ex)
            {
                spnSelRadERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrSelRad\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }
        }
        #endregion

        #region UpdateRadiologists
        private void UpdateRadiologists(string[] arrParams)
        {
            DataTable dtbl = new DataTable();
            objComm = new classes.CommonClass();
            string[] arrSep = { objComm.SecondaryRecordDivider };
            string[] arrRecords = arrParams[1].Split(arrSep, StringSplitOptions.None);
            int intSrl = 0;

            try
            {
                dtbl = CreateRadiologistTable();
                if (arrRecords.Length > 0)
                {
                    if (arrRecords[0].Trim() != "")
                    {
                        for (int i = 0; i < arrRecords.Length; i = i + 3)
                        {
                            DataRow dr = dtbl.NewRow();
                            dr["radiologist_id"] = arrRecords[i].Trim();
                            dr["radiologist_code"] = arrRecords[i + 1].Trim();
                            dr["radiologist_name"] = arrRecords[i + 2].Trim();
                            dtbl.Rows.Add(dr);
                        }

                    }
                }

                grdSelRad.DataSource = dtbl;
                grdSelRad.DataBind();
                spnSelRadERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrSelRad\" value=\"\" />";
            }
            catch (Exception ex)
            {
                spnSelRadERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrSelRad\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                dtbl.Dispose();
                objComm = null;
            }
        }
        #endregion

        #region CreateRadiologistTable
        private DataTable CreateRadiologistTable()
        {
            DataTable dtbl = new DataTable();
            dtbl.Columns.Add("radiologist_id", System.Type.GetType("System.String"));
            dtbl.Columns.Add("radiologist_code", System.Type.GetType("System.String"));
            dtbl.Columns.Add("radiologist_name", System.Type.GetType("System.String"));
            dtbl.TableName = "Radiologists";
            return dtbl;
        }
        #endregion

        #endregion

        #region SaveRecord (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] SaveRecord(string[] ArrRecord, string[] arrRights, string[] arrMod, string[] arrSpec, string[] arrInst, string[] arrST, string[] arrRad)
        {
            int intListIndex = 0;
            bool bReturn = false;

            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty;
            string strCatchMessage = string.Empty;
            Core.Master.FubnctionalRightsList[] objFN = new Core.Master.FubnctionalRightsList[0];
            Core.Master.ModalityList[] objMod = new Core.Master.ModalityList[0];
            Core.Master.Species[] objSpec = new Core.Master.Species[0];
            Core.Master.ExceptionInstitutionList[] objInst = new Core.Master.ExceptionInstitutionList[0];
            Core.Master.ExceptionStudyList[] objST = new Core.Master.ExceptionStudyList[0];
            Core.Master.OtherRadiologistList[] objRad = new Core.Master.OtherRadiologistList[0];

            objCore = new Core.Master.Radiologist();
            objComm = new classes.CommonClass();

            try
            {
                objCore.ID = new Guid(ArrRecord[0].Trim());
                objCore.USER_ID = new Guid(ArrRecord[1].Trim());
                objCore.MENU_ID = Convert.ToInt32(ArrRecord[2]);


                objFN = new Core.Master.FubnctionalRightsList[(arrRights.Length)];
                for (int i = 0; i < objFN.Length; i++)
                {
                    objFN[i] = new Core.Master.FubnctionalRightsList();
                    objFN[i].CODE = arrRights[intListIndex].Trim();
                    intListIndex = intListIndex + 1;
                }
                intListIndex = 0;

                objMod = new Core.Master.ModalityList[(arrMod.Length)];
                for (int i = 0; i < objMod.Length; i++)
                {
                    objMod[i] = new Core.Master.ModalityList();
                    objMod[i].ID = Convert.ToInt32(arrMod[intListIndex]);
                    intListIndex = intListIndex + 1;
                }
                intListIndex = 0;

                objSpec = new Core.Master.Species[(arrSpec.Length)];
                for (int i = 0; i < objSpec.Length; i++)
                {
                    objSpec[i] = new Core.Master.Species();
                    objSpec[i].ID = Convert.ToInt32(arrSpec[intListIndex]);
                    intListIndex = intListIndex + 1;
                }
                intListIndex = 0;

                objInst = new Core.Master.ExceptionInstitutionList[(arrInst.Length)];
                for (int i = 0; i < objInst.Length; i++)
                {
                    objInst[i] = new Core.Master.ExceptionInstitutionList();
                    objInst[i].ID = new Guid(arrInst[intListIndex]);
                    intListIndex = intListIndex + 1;
                }
                intListIndex = 0;

                objST = new Core.Master.ExceptionStudyList[(arrST.Length)];
                for (int i = 0; i < objST.Length; i++)
                {
                    objST[i] = new Core.Master.ExceptionStudyList();
                    objST[i].ID = new Guid(arrST[intListIndex]);
                    intListIndex = intListIndex + 1;
                }
                intListIndex = 0;

                objRad = new Core.Master.OtherRadiologistList[(arrRad.Length)];
                for (int i = 0; i < objRad.Length; i++)
                {
                    objRad[i] = new Core.Master.OtherRadiologistList();
                    objRad[i].ID = new Guid(arrRad[intListIndex]);
                    intListIndex = intListIndex + 1;
                }
                intListIndex = 0;

                bReturn = objCore.SaveRights(Server.MapPath("~"), objFN,objMod,objSpec, objInst,objST,objRad, ref strReturnMsg, ref strCatchMessage);

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