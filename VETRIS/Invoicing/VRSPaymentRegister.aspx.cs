using GemBox.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using VETRIS.Core;
using VETRIS.Core.MyPayments;

namespace VETRIS.Invoicing
{
    [AjaxPro.AjaxNamespace("VRSPaymentRegister")]
    public partial class VRSPaymentRegister : System.Web.UI.Page
    {
        #region Members & Variables
        ARPayments objCore = new ARPayments();
        classes.CommonClass objComm; 
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSPaymentRegister));
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
            objComm = new classes.CommonClass();
            objComm.SetRegionalFormat();
            txtFromDate.Text = objComm.IMDateFormat(DateTime.Today.Date.AddDays(-30), objComm.DateFormat);
            txtToDate.Text = objComm.IMDateFormat(DateTime.Today, objComm.DateFormat);
            grdBrw.Levels[0].Columns[4].HeadingText = grdBrw.Levels[0].Columns[4].HeadingText + " (" + objComm.CurrencySymbol + ")";
            objComm = null;
            SetCSS(strTheme);
        }
        #endregion

        #region SetCSS
        private void SetCSS(string strTheme)
        {
            string strServerPath = ConfigurationManager.AppSettings["ServerPath"];
            lnkSTYLE.Attributes["href"] = strServerPath + "/css/" + strTheme + "/style.css";
            lnkCAL.Attributes["href"] = strServerPath + "/css/" + strTheme + "/CalendarStyle.css";
            lnkTHEME.Attributes["href"] = strServerPath + "/css/" + strTheme + "/theme.css";
            lnkGRID.Attributes["href"] = strServerPath + "/css/" + strTheme + "/grid_style.css";
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {

            btnClose1.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
            btnSearch.Attributes.Add("onclick", "javascript:btnSearch_OnClick();");
            btnExcel.Attributes.Add("onclick", "javascript:btnExcel_OnClick();");

            txtFromDate.Attributes.Add("onblur", "javascript:ValidateDateEntered(this);");
            imgFromDt.Attributes.Add("onclick", "javascript:SetSelectedDate('" + txtFromDate.ClientID + "','CalFromDate');");
            txtToDate.Attributes.Add("onblur", "javascript:ValidateDateEntered(this);");
            imgToDt.Attributes.Add("onclick", "javascript:SetSelectedDate('" + txtToDate.ClientID + "','CalToDate');");
        }
        #endregion

        #region LoadReport
        private void LoadReport(string[] arrRecord)
        {
            objCore = new ARPayments();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();

            try
            {
                objComm.SetRegionalFormat();
                objCore.FromDate = Convert.ToDateTime(arrRecord[0]);
                objCore.ToDate = Convert.ToDateTime(arrRecord[1]);
                objCore.payment_mode = arrRecord[2].Trim();

                bReturn = objCore.PaymentRegisterBrowserList(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {
                    ds.Relations.Add(ds.Tables["Report"].Columns["id"], ds.Tables["AdjustmentInvoice"].Columns["ar_payments_id"]);
                    grdBrw.Levels[1].Columns[2].FormatString = objComm.DateFormat;
                    
                    //grdBrw.DataSource = ds.Tables["Report"];
                    grdBrw.DataSource = ds;
                    
                    grdBrw.Levels[0].Columns[2].FormatString = objComm.DateFormat;
                    grdBrw.PageSize = ds.Tables["Report"].Rows.Count;
                    grdBrw.Levels[0].Columns[4].HeadingText = grdBrw.Levels[0].Columns[4].HeadingText + " (" + objComm.CurrencySymbol + ")";
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

        #region CallBackBrw_Callback
        protected void CallBackBrw_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            LoadReport(e.Parameters);
            grdBrw.Width = Unit.Percentage(100);
            grdBrw.RenderControl(e.Output);
            spnERR.RenderControl(e.Output);
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
            objCore = new ARPayments();
            string[] arrRet = new string[4];
            objCore = new ARPayments();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            string strRptFile = string.Empty;
            objComm = new classes.CommonClass();

            try
            {
                objComm.SetRegionalFormat();
                objCore.FromDate = Convert.ToDateTime(arrRecord[0]);
                objCore.ToDate = Convert.ToDateTime(arrRecord[1]);
                objCore.payment_mode = arrRecord[2].Trim();

                bReturn = objCore.PaymentRegisterBrowserList(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {
                    strRptFile = CreateReportExcel(ds, objCore);
                    arrRet[0] = "true";
                    arrRet[1] = "Invoicing/Reports/Temp/" + strRptFile;
                    arrRet[2] = Server.MapPath("Invoicing/Reports/Temp/" + strRptFile).Replace("ajaxpro\\", ""); 
                    arrRet[3] = "XLS";
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
            return arrRet;
        }
        #endregion

        #region CreateReportExcel
        public string CreateReportExcel(DataSet ds, ARPayments core)
        {
            string strReportName = string.Empty;
            string strCatchMsg = ""; bool bReturn = false;
            SetLicenseKey();
            ExcelFile objExcelFile = null; string strRowfiltertring = string.Empty;
            ExcelWorksheet sheet = null;
            //ExcelWorksheet objExcelWorksheet2 = null;
            string strRName = "PaymentRegister_" + string.Format("{0:yyyy_MM_dd_HH_mm_ss}", DateTime.Now) + ".xlsx";
            objComm = new classes.CommonClass();
            try
            {
                objComm.SetRegionalFormat();
                strReportName = Server.MapPath("Invoicing/Reports/Temp/" + strRName);
                strReportName = strReportName.Replace("ajaxpro\\", "");
                if (System.IO.File.Exists(strReportName) == true) { System.IO.File.Delete(strReportName); }

                objExcelFile = new ExcelFile();
                sheet = objExcelFile.Worksheets.Add("Payment Register");

                int intRowIndex = 0;


                #region Details
                var data = new List<ARPayments>();
                var levelRows = new List<ARPaymentLevelRow>();
                foreach (DataRow dr in ds.Tables["AdjustmentInvoice"].Rows)
                {
                    ARPaymentLevelRow item = new ARPaymentLevelRow();
                    item.Id = new Guid(dr["adj_id"].ToString());
                    item.ArPaymentId = new Guid(dr["ar_payments_id"].ToString());
                    item.Invoice = Convert.ToString(dr["invoice_no"]).Trim();
                    item.Refund = Convert.ToString(dr["refundref_no"]??"");
                    item.Date = Convert.ToDateTime(dr["invoice_date"]);
                    item.AdjustedAmount = Convert.ToDecimal(dr["adj_amount"]);
                    levelRows.Add(item);

                }
                foreach (DataRow dr in ds.Tables["Report"].Rows)
                {
                    ARPayments item = new ARPayments();
                    item.id = new Guid(dr["id"].ToString());
                    item.billing_account_id = new Guid(dr["billing_account_id"].ToString());
                    item.payment_mode = Convert.ToString(dr["payment_mode_name"]).Trim();
                    item.payref_no = Convert.ToString(dr["payref_no"]).Trim();
                    item.payref_date = Convert.ToDateTime(dr["payref_date"]);
                    item.processing_ref_no = Convert.ToString(dr["processing_ref_no"]).Trim();
                    item.processing_ref_date = Convert.ToDateTime(dr["processing_ref_date"]);
                    item.processing_pg_name = Convert.ToString(dr["processing_pg_name"]).Trim();
                    item.auth_code = Convert.ToString(dr["auth_code"]).Trim();
                    item.cvv_response = Convert.ToString(dr["cvv_response"]).Trim();
                    item.avs_response = Convert.ToString(dr["avs_response"]).Trim();
                    item.billing_account_name = Convert.ToString(dr["billing_account_name"]).Trim();
                    item.payment_amount = Convert.ToDecimal(dr["payment_amount"]);
                    item.date_created = Convert.ToDateTime(dr["date_created"]);
                    item.AdjustedPayments = new List<ARPaymentLevelRow>();
                    if (levelRows.Where(x => x.ArPaymentId == item.id).Count() > 0)
                    {
                        item.AdjustedPayments.AddRange(levelRows.Where(x => x.ArPaymentId == item.id));
                    }
                    data.Add(item);
                }

                #endregion

                CreateHeader(ref intRowIndex, sheet);
                foreach (var item in data)
                {
                    CreateBodyRow(ref intRowIndex, item, sheet);
                }
                CreateSummaryRow(ref intRowIndex, data, sheet);
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

                var printOptions = sheet.PrintOptions;
                printOptions.Portrait = false;
                printOptions.LeftMargin = 0.50;
                printOptions.RightMargin = 0.3;
                printOptions.TopMargin = 0.3;
                printOptions.BottomMargin = 0.3;
                printOptions.PaperSize = 9;
                printOptions.AutomaticPageBreakScalingFactor = 80;

                objExcelFile.SaveXlsx(strReportName);
                strReportName = strRName;
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
            return strReportName;
        } 
        #endregion

        #region CreateHeader
        void CreateHeader(ref int row, ExcelWorksheet sheet)
        {
            var style = new CellStyle();
            style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            style.VerticalAlignment = VerticalAlignmentStyle.Center;
            style.Font.Weight = ExcelFont.BoldWeight;
            style.Borders.SetBorders(MultipleBorders.Bottom, Color.Black, LineStyle.Thin);

            sheet.Cells.GetSubrange("A1", "J1").Style = style;

            //sheet.Cells[row, 0].Value = "Sl. No.";
            sheet.Cells[row, 0].Value = "Payment Ref #";
            sheet.Cells[row, 1].Value = "Date";
            sheet.Cells[row, 2].Value = "Mode";
            sheet.Cells[row, 3].Value = "External Ref #";
            sheet.Cells[row, 4].Value = "Payment Gateway";

            sheet.Cells[row, 5].Value = "Auth Code";
            sheet.Cells[row, 6].Value = "AVS response";
            sheet.Cells[row, 7].Value = "CVV Response";
            sheet.Cells[row, 8].Value = "Billing Account Name";
            sheet.Cells[row, 9].Value = "Payment Amount ($)";

            row++;
        } 
        #endregion

        #region CreateBodyRow
        void CreateBodyRow(ref int row, ARPayments data, ExcelWorksheet sheet)
        {
            var style = new CellStyle();
            style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            style.VerticalAlignment = VerticalAlignmentStyle.Center;
            //style.Font.Weight = ExcelFont.BoldWeight;
            style.Borders.SetBorders(MultipleBorders.Bottom, Color.Black, LineStyle.Thin);
            style.Borders.SetBorders(MultipleBorders.Top, Color.Black, LineStyle.Thin);
            sheet.Cells.GetSubrange("A" + (row+1).ToString(), "J" + (row+1).ToString()).Style = style;

            sheet.Cells[row, 0].Value = data.payref_no;
            sheet.Cells[row, 1].Value = data.payref_date.ToString("dd-MM-yyyy");
            sheet.Cells[row, 2].Value = data.payment_mode;
            sheet.Cells[row, 3].Value = data.processing_ref_no;
            sheet.Cells[row, 4].Value = data.processing_pg_name;
            sheet.Cells[row, 5].Value = data.auth_code;
            sheet.Cells[row, 6].Value = data.avs_response;
            sheet.Cells[row, 7].Value = data.cvv_response;
            sheet.Cells[row, 8].Value = data.billing_account_name;
            sheet.Cells[row, 9].Value = data.payment_amount;
            sheet.Cells[row, 9].Style.NumberFormat = "$ #,##0.00;$ -#,##0.00";
            CreateLevelHeader(ref row,data.AdjustedPayments, sheet);
            row++;
        }

        
        #endregion

        #region Create Next Level Header
        void CreateLevelHeader(ref int row, List<ARPaymentLevelRow> data, ExcelWorksheet sheet)
        {
            var style = new CellStyle();
            style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            style.VerticalAlignment = VerticalAlignmentStyle.Center;
            //style.Font.Weight = ExcelFont.BoldWeight;
            //style.Borders.SetBorders(MultipleBorders.Bottom, Color.Black, LineStyle.Thin);

            sheet.Cells.GetSubrange("A" + row + 1, "J" + row + 1).Style = style;

            sheet.Cells[row + 1, 1].Value = "Invoice #"; sheet.Cells[row + 1, 1].Style.Font.Weight = ExcelFont.BoldWeight;
            sheet.Cells[row + 1, 2].Value = "Date"; sheet.Cells[row + 1, 2].Style.Font.Weight = ExcelFont.BoldWeight;
            sheet.Cells[row + 1, 3].Value = "Refund #"; sheet.Cells[row + 1, 3].Style.Font.Weight = ExcelFont.BoldWeight;
            sheet.Cells[row + 1, 4].Value = "Adjusted Amount ($)"; sheet.Cells[row + 1, 4].Style.Font.Weight = ExcelFont.BoldWeight;
            row++;

            int lvelRow = 1;
            foreach (var item in data)
            {
                CreateLevelBodyRow(row + lvelRow, item, sheet);
                lvelRow++;
            }
            row = row + lvelRow;
            //row++;

        }

        void CreateLevelBodyRow(int row, ARPaymentLevelRow data, ExcelWorksheet sheet)
        {
            sheet.Cells[row, 1].Value = data.Invoice;
            sheet.Cells[row, 2].Value = data.Date.ToString("dd-MM-yyyy");
            sheet.Cells[row, 3].Value = data.Refund;
            sheet.Cells[row, 4].Value = data.AdjustedAmount;
            sheet.Cells[row, 4].Style.NumberFormat = "$ #,##0.00;$ -#,##0.00";
            //row++;
        }
        #endregion

        #region CreateSummaryRow
        void CreateSummaryRow(ref int row, List<ARPayments> data, ExcelWorksheet sheet)
        {

            var style = new CellStyle();
            style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            style.VerticalAlignment = VerticalAlignmentStyle.Center;
            style.Font.Weight = ExcelFont.BoldWeight;
            style.Borders.SetBorders(MultipleBorders.Top, Color.Black, LineStyle.Thin);
            sheet.Cells.GetSubrange("A" + (row + 1).ToString(), "J" + (row + 1).ToString()).Style = style;

            sheet.Cells[row, 8].Value = "Total:";
            sheet.Cells[row, 9].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            sheet.Cells[row, 9].Value = data.Sum(i => i.payment_amount);
            sheet.Cells[row, 9].Style.NumberFormat = "$ #,##0.00;$ -#,##0.00";
            row++;

        } 
        #endregion
    }
}