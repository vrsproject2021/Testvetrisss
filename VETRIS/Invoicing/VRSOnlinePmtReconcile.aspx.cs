using GemBox.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VETRIS.Core;
using VETRIS.Core.MyPayments;

namespace VETRIS.Invoicing
{
    [AjaxPro.AjaxNamespace("VRSOnlinePmtReconcile")]
    public partial class VRSOnlinePmtReconcile : System.Web.UI.Page
    {
        #region Members & Variables
        ARReconciliation objCore = new ARReconciliation();
        classes.CommonClass objComm; 
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSOnlinePmtReconcile));
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
            objComm = new classes.CommonClass();
            objComm.SetRegionalFormat();
            txtFromDate.Text = objComm.IMDateFormat(DateTime.Today.Date.AddDays(-30), objComm.DateFormat);
            txtToDate.Text = objComm.IMDateFormat(DateTime.Today, objComm.DateFormat);
            objComm = null;

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
            objCore = new ARReconciliation();
            string strCatchMessage = "";
            string strReturnMessage = "";
            bool bReturn = false;
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();

            try
            {
                objComm.SetRegionalFormat();
                objCore.FromDate = Convert.ToDateTime(arrRecord[0]);
                objCore.ToDate = Convert.ToDateTime(arrRecord[1]);
                //var rec = new PullReconcilationDataService();
                //rec.Execute(Server.MapPath("~"), ref strReturnMessage, ref strCatchMessage, VETRIS.Global.API_Key, objCore.FromDate.Value, null);
               bReturn = objCore.ReconcilationBrowserList(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {
                    grdBrw.DataSource = ds;
                    grdBrw.Levels[0].Columns[2].FormatString = objComm.DateFormat;
                    grdBrw.Levels[0].Columns[4].FormatString = objComm.DateFormat;
                    grdBrw.Levels[0].Columns[11].FormatString = objComm.DateFormat;
                    grdBrw.PageSize = ds.Tables["Transactions"].Rows.Count;
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
            objCore = new ARReconciliation();
            string[] arrRet = new string[4];
            objCore = new ARReconciliation();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            string strRptFile = string.Empty;
            objComm = new classes.CommonClass();

            try
            {
                objComm.SetRegionalFormat();
                objCore.FromDate = Convert.ToDateTime(arrRecord[0]);
                objCore.ToDate = Convert.ToDateTime(arrRecord[1]);

                bReturn = objCore.ReconcilationBrowserList(Server.MapPath("~"), ref ds, ref strCatchMessage);

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
        public string CreateReportExcel(DataSet ds, ARReconciliation core)
        {
            string strReportName = string.Empty;
            string strCatchMsg = ""; bool bReturn = false;
            SetLicenseKey();
            ExcelFile objExcelFile = null; string strRowfiltertring = string.Empty;
            ExcelWorksheet sheet = null;
            //ExcelWorksheet objExcelWorksheet2 = null;
            string strRName = "BRS_" + string.Format("{0:yyyy_MM_dd_HH_mm_ss}", DateTime.Now) + ".xlsx";
            objComm = new classes.CommonClass();
            try
            {
                objComm.SetRegionalFormat();
                strReportName = Server.MapPath("Invoicing/Reports/Temp/" + strRName);
                strReportName = strReportName.Replace("ajaxpro\\", "");
                if (System.IO.File.Exists(strReportName) == true) { System.IO.File.Delete(strReportName); }

                objExcelFile = new ExcelFile();
                sheet = objExcelFile.Worksheets.Add("Reconciliation");

                int intRowIndex = 0;

                CreateHeader(ref intRowIndex, sheet);
                foreach (DataRow dr in ds.Tables["Transactions"].Rows)
                {
                    CreateBodyRow(ref intRowIndex, dr, sheet);
                }
                CreateSummaryRow(ref intRowIndex, ds.Tables["Transactions"].Rows, sheet);
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

            sheet.Cells.GetSubrange("A1", "M1").Style = style;

            //sheet.Cells[row, 0].Value = "Sl. No.";
            sheet.Cells[row, 0].Value = "Payment Ref #";
            sheet.Cells[row, 1].Value = "Date";
            sheet.Cells[row, 2].Value = "Refund Ref #";
            sheet.Cells[row, 3].Value = "Date";
            sheet.Cells[row, 4].Value = "Billing Account Name";
            sheet.Cells[row, 5].Value = "Charged Amount ($)";
            sheet.Cells[row, 6].Value = "Refund Amount ($)";
            sheet.Cells[row, 7].Value = "Payment Gateway";
            sheet.Cells[row, 8].Value = "External Ref #";
            sheet.Cells[row, 9].Value = "Auth Code";
            sheet.Cells[row, 10].Value = "Effected Date/Time";
            sheet.Cells[row, 11].Value = "Realized Amount ($)";
            sheet.Cells[row, 12].Value = "Difference ($)";

            row++;
        } 
        #endregion

        #region CreateBodyRow
        void CreateBodyRow(ref int row, DataRow dr, ExcelWorksheet sheet)
        {
            var style = new CellStyle();
            style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            style.VerticalAlignment = VerticalAlignmentStyle.Center;
            //style.Font.Weight = ExcelFont.BoldWeight;
            style.Borders.SetBorders(MultipleBorders.Bottom, Color.Black, LineStyle.Thin);
            style.Borders.SetBorders(MultipleBorders.Top, Color.Black, LineStyle.Thin);
            sheet.Cells.GetSubrange("A" + (row+1).ToString(), "M" + (row+1).ToString()).Style = style;

            sheet.Cells[row, 0].Value =Convert.ToString(dr["payref_no"]);
            sheet.Cells[row, 1].Value =Convert.ToDateTime(dr["payref_date"]).ToString("dd-MMM-yyyy");
            sheet.Cells[row, 2].Value = Convert.ToString(dr["refundref_no"]);
            if(dr["refundref_date"]!=DBNull.Value){
                sheet.Cells[row, 3].Value = Convert.ToDateTime(dr["refundref_date"]).ToString("dd-MMM-yyyy");
            }
            sheet.Cells[row, 4].Value = Convert.ToString(dr["billing_account_name"]);
            if (dr["payment_amount"] != DBNull.Value)
            {
                sheet.Cells[row,5].Value = Convert.ToDecimal(dr["payment_amount"]);
                sheet.Cells[row, 5].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            }
            if (dr["refund_amount"] != DBNull.Value)
            {
                sheet.Cells[row, 6].Value = Convert.ToDecimal(dr["refund_amount"]);
                sheet.Cells[row, 6].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            }
            sheet.Cells[row, 7].Value = Convert.ToString(dr["processing_pg_name"]);
            sheet.Cells[row, 8].Value = Convert.ToString(dr["processing_ref_no"]);
            sheet.Cells[row, 9].Value = Convert.ToString(dr["auth_code"]);
            sheet.Cells[row, 10].Value = Convert.ToDateTime(dr["effected_date_time"]).ToString("dd-MMM-yyyy hh:mm:ss tt");
            sheet.Cells[row, 11].Value = Convert.ToDecimal(dr["transaction_amount"]);
            sheet.Cells[row, 11].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            sheet.Cells[row, 12].Value = Convert.ToDecimal(dr["difference_amount"]);
            sheet.Cells[row, 12].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            sheet.Cells[row, 11].Style.NumberFormat = "$ #,##0.00;$ -#,##0.00";
            sheet.Cells[row, 12].Style.NumberFormat = "$ #,##0.00;$ -#,##0.00";
            sheet.Cells[row, 5].Style.NumberFormat = "$ #,##0.00;$ -#,##0.00";
            sheet.Cells[row, 6].Style.NumberFormat = "$ #,##0.00;$ -#,##0.00";
            
            row++;
        }

        
        #endregion

        
        #region CreateSummaryRow
        void CreateSummaryRow(ref int row, DataRowCollection data, ExcelWorksheet sheet)
        {

            var style = new CellStyle();
            style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
            style.VerticalAlignment = VerticalAlignmentStyle.Center;
            style.Font.Weight = ExcelFont.BoldWeight;
            style.Borders.SetBorders(MultipleBorders.Top, Color.Black, LineStyle.Thin);
            sheet.Cells.GetSubrange("A" + (row + 1).ToString(), "M" + (row + 1).ToString()).Style = style;
            decimal total1 = 0, total2 = 0, total3 = 0;

            foreach (DataRow dr in data)
            {
                if (dr["payment_amount"] != DBNull.Value)
                {
                    total1 += Convert.ToDecimal(dr["payment_amount"]);
                }
                if (dr["refund_amount"] != DBNull.Value)
                {
                    total2 += Convert.ToDecimal(dr["refund_amount"]);
                }
                if (dr["transaction_amount"] != DBNull.Value)
                {
                    total3 += Convert.ToDecimal(dr["transaction_amount"]);
                }
            }
            sheet.Cells[row, 4].Value = "Total:";
            sheet.Cells[row, 5].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            sheet.Cells[row, 5].Value = total1;
            sheet.Cells[row, 5].Style.NumberFormat = "$ #,##0.00;$ -#,##0.00";

            sheet.Cells[row, 6].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            sheet.Cells[row, 6].Value = total2;
            sheet.Cells[row, 6].Style.NumberFormat = "$ #,##0.00;$ -#,##0.00";

            sheet.Cells[row, 11].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            sheet.Cells[row, 11].Value = total3;
            sheet.Cells[row, 11].Style.NumberFormat = "$ #,##0.00;$ -#,##0.00";
            row++;

        }
        #endregion
        
    }
}