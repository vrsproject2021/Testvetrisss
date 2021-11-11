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
using GemBox.Spreadsheet;
using System.Drawing;

namespace VETRIS.Masters
{

    [AjaxPro.AjaxNamespace("VRSInstitutionBrw")]
    public partial class VRSInstitutionBrw : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.Master.Institution objCore = null;
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSInstitutionBrw));
            if (!CallBackBrw.CausedCallback)
                SetPageValue();
            SetAttributes();
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {
            btnAdd.Attributes.Add("onclick", "javascript:btnBrwAddUI_Onclick('Masters/VRSInstitutionDlg.aspx');");
            btnRefresh.Attributes.Add("onclick", "javascript:ResetRecord();view_Searchform();");
            btnSearch.Attributes.Add("onclick", "javascript:SearchRecord();view_Searchform();");
            btnClose.Attributes.Add("onclick", "javascript:btnBrwClose_Onclick();");
            btnExcel.Attributes.Add("onclick", "javascript:btnExcel_OnClick();");
            ddlCountry.Attributes.Add("onchange", "javascript:ddlCountry_OnChange()");

        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {

            int intUserRoleID = Convert.ToInt32(Request.QueryString["urid"]);
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid UserID = new Guid(Request.QueryString["uid"].ToString());
            string strID = string.Empty;
            string strTheme = Request.QueryString["th"];
            SetCSS(strTheme);

            FetchSearchParameters();
            DeleteFiles(UserID);
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

        #region FetchSearchParameters
        private void FetchSearchParameters()
        {
            objCore = new Core.Master.Institution();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();

            try
            {

                bReturn = objCore.FetchBrowserParameters(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {
                    #region Country
                    DataRow dr1 = ds.Tables["Country"].NewRow();
                    dr1["id"] = 0;
                    dr1["name"] = "Select One";
                    ds.Tables["Country"].Rows.InsertAt(dr1, 0);
                    ds.Tables["Country"].AcceptChanges();

                    ddlCountry.DataSource = ds.Tables["Country"];
                    ddlCountry.DataValueField = "id";
                    ddlCountry.DataTextField = "name";
                    ddlCountry.DataBind();
                    #endregion
                }
                else
                    hdnError.Value = strCatchMessage.Trim();

                ListItem objLI = new ListItem();
                objLI.Value = "0";
                objLI.Text = "Select One";

                ddlState.Items.Add(objLI);

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

        #region DeleteFiles
        private void DeleteFiles(Guid UserID)
        {
            string[] arrTemp = new string[0];
            arrTemp = Directory.GetFiles(Server.MapPath("~/Masters/Logo/Temp/"), UserID.ToString() + "_*");
            if (arrTemp.Length > 0)
            {
                for (int i = 0; i < arrTemp.Length; i++)
                {
                    File.Delete(arrTemp[i]);
                }
            }

           
        }
        #endregion

        #region CallBackBrw_Callback
        protected void CallBackBrw_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            SearchRecord(e.Parameters);
            grdBrw.Width = Unit.Percentage(100);
            grdBrw.RenderControl(e.Output);
            spnERR.RenderControl(e.Output);
        }
        #endregion
       
        #region SearchRecord
        private void SearchRecord(string[] arrRecord)
        {
            objCore = new Core.Master.Institution();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();

            try
            {
                objCore.CODE = arrRecord[0].Trim();
                objCore.NAME = arrRecord[1].Trim();
                objCore.IS_ACTIVE = arrRecord[2].Trim();
                objCore.COUNTRY_ID = Convert.ToInt32(arrRecord[3]);
                objCore.STATE_ID = Convert.ToInt32(arrRecord[4]);
                objCore.CITY = arrRecord[5].Trim();
                objCore.ZIP = arrRecord[6].Trim();
                objCore.DICOM_FILES_TRANSFER_METHOD = arrRecord[7].Trim();
                objCore.FAX_REPORTS = arrRecord[8].Trim();
                objCore.USER_ID = new Guid(arrRecord[9].Trim());


                bReturn = objCore.SearchBrowserList(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {

                    grdBrw.DataSource = ds.Tables["BrowserList"];
                    grdBrw.DataBind();
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

        #region FetchStates (AjaxPro Method)
        [AjaxPro.AjaxMethod()]
        public string[] FetchStates(string[] arrParams)
        {
            string strReturn = string.Empty; string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            objCore = new Core.Master.Institution();

            int i = 0;
            string[] arrRet = new string[0];

            try
            {

                objCore.COUNTRY_ID= Convert.ToInt32(arrParams[0].Trim());
                bReturn = objCore.FetchCountryWiseStates(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {

                    if (ds.Tables["States"].Rows.Count > 0)
                    {

                        arrRet = new string[(ds.Tables["States"].Rows.Count * 2) + 3];
                        arrRet[0] = "true";
                        arrRet[1] = "0";
                        arrRet[2] = "Select One";
                        i = 3;

                        foreach (DataRow dr in ds.Tables["States"].Rows)
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
                        arrRet[1] = "0";
                        arrRet[2] = "Select One";
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

        #region SetLicenseKey
        private void SetLicenseKey()
        {
            try
            {
                VETRIS.Core.CoreCommon.GetReportLicenseKey(Server.MapPath("~"));
                SpreadsheetInfo.SetLicense(CoreCommon.REPORT_LICENSE_KEY);

            }
            catch (Exception expErr)
            {
                Response.Write(expErr.Message);
            }
        }
        #endregion

        #region GenerateExcel(Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] GenerateExcel(string[] arrRecord)
        {
            objCore = new Core.Master.Institution();
            string strCatchMessage = ""; bool bReturn = false;
            string[] arrRet = new string[4];
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();
            string strRptFile = string.Empty;
            string strUserID = string.Empty;

            try
            {
                objComm.SetRegionalFormat();
                objCore.CODE = arrRecord[0].Trim();
                objCore.NAME = arrRecord[1].Trim();
                objCore.IS_ACTIVE = arrRecord[2].Trim();
                objCore.COUNTRY_ID = Convert.ToInt32(arrRecord[3]);
                objCore.STATE_ID = Convert.ToInt32(arrRecord[4]);
                objCore.CITY = arrRecord[5];
                objCore.ZIP = arrRecord[6];
                objCore.DICOM_FILES_TRANSFER_METHOD = arrRecord[7].Trim();
                objCore.FAX_REPORTS = arrRecord[8].Trim();
                objCore.USER_ID = new Guid(arrRecord[9].Trim());
                strUserID = objCore.USER_ID.ToString();

                bReturn = objCore.FetchExportList(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {
                    strRptFile = CreateReportExcel(ds, objCore);
                    arrRet[0] = "true";
                    arrRet[1] = "Masters/Temp/" + strUserID + "/" + strRptFile;
                    arrRet[2] = Server.MapPath("Masters/Temp/" + strUserID + "/" + strRptFile).Replace("ajaxpro\\", "");
                    arrRet[3] = "XLS";
                }
                else
                {
                    arrRet[0] = "false";
                    arrRet[1] = strCatchMessage;
                }
            }
            catch (Exception ex)
            {
                arrRet[0] = "catch";
                arrRet[1] = ex.Message.Trim();
            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }
            return arrRet;
        }

        #region CreateReportExcel
        private string CreateReportExcel(DataSet ds, Core.Master.Institution objCore)
        {
            string strCatchMsg = ""; bool bReturn = false;
            string strUserID = string.Empty;
            string strFolder = string.Empty;
            string strReportName = string.Empty;
            SetLicenseKey();
            ExcelFile objExcelFile = null; string strRowfiltertring = string.Empty;
            ExcelWorksheet sheet = null;
            
            //ExcelWorksheet objExcelWorksheet2 = null;
            string strRName = "Institutions_" + string.Format("{0:yyyy_MM_dd_HH_mm_ss}", DateTime.Now) + ".xlsx";
            objComm = new classes.CommonClass();
            try
            {
                strUserID = objCore.USER_ID.ToString();
                strFolder = Server.MapPath("Masters/Temp/" + strUserID).Replace("ajaxpro\\", "");
                objComm.SetRegionalFormat();
                if (!Directory.Exists(strFolder))
                {
                    Directory.CreateDirectory(strFolder);
                }
                strReportName = strFolder + "/" + strRName;
                strReportName = strReportName.Replace("ajaxpro\\", "");
                if (System.IO.File.Exists(strReportName) == true) { System.IO.File.Delete(strReportName); }

                objExcelFile = new ExcelFile();
                sheet = objExcelFile.Worksheets.Add("Institutions");

                int intRowIndex = 0; var printOptions = sheet.PrintOptions;
                printOptions.Portrait = false;
                printOptions.LeftMargin = 0.50;
                printOptions.RightMargin = 0.3;
                printOptions.TopMargin = 0.3;
                printOptions.BottomMargin = 0.3;
                printOptions.PaperSize = 9;
                printOptions.AutomaticPageBreakScalingFactor = 80;

                CreateHeader(ref intRowIndex, sheet);
                foreach (DataRow item in ds.Tables["ExportList"].Rows)
                {
                    CreateBodyRow(ref intRowIndex, item, sheet);
                }
                sheet.Columns[0].AutoFit();
                sheet.Columns[1].AutoFit();
                sheet.Columns[2].AutoFit();
                sheet.Columns[3].AutoFit();
                sheet.Columns[4].AutoFit();
                sheet.Columns[5].AutoFit();
                sheet.Columns[6].AutoFit();
                sheet.Columns[7].AutoFit();
                sheet.Columns[8].AutoFit();
                sheet.Columns[9].AutoFit();
                sheet.Columns[10].AutoFit();
                sheet.Columns[11].AutoFit();
                sheet.Columns[12].AutoFit();
                sheet.Columns[13].AutoFit();
                objExcelFile.SaveXlsx(strReportName);
            }
            catch (Exception expErr)
            {
                strReportName = "";
                strCatchMsg = expErr.Message;
            }
            finally
            {
                if (objExcelFile != null) objExcelFile = null;
                if (sheet != null) sheet = null;
                //ds.Dispose();
                objComm = null;

            }
            return strRName;
        } 
        #endregion

        #region CreateBodyRow
        private void CreateBodyRow(ref int row, DataRow dr, ExcelWorksheet sheet)
        {
            var style = new CellStyle();
            style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            style.VerticalAlignment = VerticalAlignmentStyle.Center;
            style.Borders.SetBorders(MultipleBorders.Bottom, Color.Black, LineStyle.Thin);
            style.Borders.SetBorders(MultipleBorders.Top, Color.Black, LineStyle.Thin);
            sheet.Cells.GetSubrange("A" + (row + 1).ToString(), "N" + (row + 1).ToString()).Style = style;

            sheet.Cells[row, 0].Value = Convert.ToString(dr["code"]);
            sheet.Cells[row, 1].Value = Convert.ToString(dr["name"]);
            sheet.Cells[row, 2].Value = Convert.ToString(dr["address_1"]);
            sheet.Cells[row, 3].Value = Convert.ToString(dr["address_2"]);
            sheet.Cells[row, 4].Value = Convert.ToString(dr["city"]);
            sheet.Cells[row, 5].Value = Convert.ToString(dr["state_name"]);
            sheet.Cells[row, 6].Value = Convert.ToString(dr["country_name"]);
            sheet.Cells[row, 7].Value = Convert.ToString(dr["zip"]);
            sheet.Cells[row, 8].Value = Convert.ToString(dr["email_id"]);
            sheet.Cells[row, 9].Value = Convert.ToString(dr["phone_no"]);
            sheet.Cells[row, 10].Value = Convert.ToString(dr["mobile_no"]);
            sheet.Cells[row, 11].Value = Convert.ToString(dr["contact_person_name"]);
            sheet.Cells[row, 12].Value = Convert.ToString(dr["contact_person_mobile"]);
            sheet.Cells[row, 13].Value = Convert.ToString(dr["is_active"]);
            row++;

        } 
        #endregion

        #region CreateHeader
        private void CreateHeader(ref int row, ExcelWorksheet sheet)
        {
            var style = new CellStyle();
            style.FillPattern.SetSolid(System.Drawing.Color.LightGray);
            style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            style.VerticalAlignment = VerticalAlignmentStyle.Center;
            style.Font.Weight = ExcelFont.BoldWeight;
            style.Borders.SetBorders(MultipleBorders.Bottom, Color.Black, LineStyle.Thin);

            sheet.Cells.GetSubrange("A1", "N1").Style = style;

            sheet.Cells[row, 0].Value = "Code";
            sheet.Cells[row, 1].Value = "Name";
            sheet.Cells[row, 2].Value = "Address 1";
            sheet.Cells[row, 3].Value = "Address 2";
            sheet.Cells[row, 4].Value = "City";
            sheet.Cells[row, 5].Value = "State";
            sheet.Cells[row, 6].Value = "Country";
            sheet.Cells[row, 7].Value = "Zip";
            sheet.Cells[row, 8].Value = "Email";
            sheet.Cells[row, 9].Value = "Phone#";
            sheet.Cells[row, 10].Value = "Mobile#";
            sheet.Cells[row, 11].Value = "Contact Person Name";
            sheet.Cells[row, 12].Value = "Contact Person Mobile#";
            sheet.Cells[row, 13].Value = "Active";
            row++;
        } 
        #endregion

        #endregion
    }
}