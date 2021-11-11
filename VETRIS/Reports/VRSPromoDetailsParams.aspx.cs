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
using System.IO;
using VETRIS.Core;
using VETRIS.Core.MyPayments;
using VETRIS.Core.Reports;

namespace VETRIS.Reports
{
    [AjaxPro.AjaxNamespace("VRSPromoDetailsParams")]
    public partial class VRSPromoDetailsParams : System.Web.UI.Page
    {
        #region Members & Variables
        Report objCore = new Report();
        classes.CommonClass objComm;
       
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSPromoDetailsParams));
            SetAttributes();
            SetPageValue();
        }
        
        #endregion

        protected void CalFromDate_DayRender(object sender, DayRenderEventArgs e)
        {
            int year = DateTime.Now.Year;
            DateTime firstDay = new DateTime(year, 1, 1);
            if (e.Day.Date < firstDay)
            {
                e.Day.IsSelectable = false;
                e.Cell.ForeColor = System.Drawing.Color.Gray;
            }
        }

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
            int year = DateTime.Now.Year;
            DateTime firstDay = new DateTime(year, 1, 1);
            txtFromDate.Attributes["min"] = DateTime.Now.ToString("yyyy-MM-dd");
            txtToDate.Text = objComm.IMDateFormat(DateTime.Today, objComm.DateFormat);
            txtToDate.Attributes["max"] = DateTime.Now.ToString("yyyy-MM-dd");
            //int year = DateTime.Now.Year;
            //DateTime firstDay = new DateTime(year, 1, 1);
            CalFromDate.DisabledDates.SelectRange(Convert.ToDateTime("01jan1900"), firstDay.AddDays(-1));
            
            DateTime lastday = DateTime.Now.Date.AddDays(1);
            year = DateTime.Now.Year + 10;
            DateTime featureToDate = new DateTime(year, 1, 1);
            CalToDate.DisabledDates.SelectRange(lastday, featureToDate);
            //Disable feature from date
            for (int i = 0; i < 365; i++)
            {
                CalFromDate.DisabledDates.Add(lastday.AddDays(i));
            }
            //Disable previous to date from current year
            for (int i = 0; i < 365; i++)
            {
                CalToDate.DisabledDates.Add(firstDay.AddDays(-i-1));
            }
            
            objComm = null;
            FetchSearchParameters(UserID);
            SetCSS(strTheme);
        }
        #endregion

        #region SetCSS
        private void SetCSS(string strTheme)
        {
            string strServerPath = ConfigurationManager.AppSettings["ServerPath"];
            lnkSTYLE.Attributes["href"] = strServerPath + "/css/" + strTheme + "/style.css?v=" + DateTime.Now.Ticks.ToString();
            lnkCAL.Attributes["href"] = strServerPath + "/css/" + strTheme + "/CalendarStyle.css?v=" + DateTime.Now.Ticks.ToString();
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {
            btnReset1.Attributes.Add("onclick", "javascript:btnReset_OnClick();");
            btnClose1.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
            btnReset2.Attributes.Add("onclick", "javascript:btnReset_OnClick();");
            btnClose2.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
            btnExcel.Attributes.Add("onclick", "javascript:btnExcel_OnClick();");

            txtFromDate.Attributes.Add("onblur", "javascript:ValidateDateEntered(this);");
            imgFromDt.Attributes.Add("onclick", "javascript:SetSelectedDate('" + txtFromDate.ClientID + "','CalFromDate');");
            txtToDate.Attributes.Add("onblur", "javascript:ValidateDateEntered(this);");
            imgToDt.Attributes.Add("onclick", "javascript:SetSelectedDate('" + txtToDate.ClientID + "','CalToDate');");
        }
        #endregion

        #region GetPromoReasonList (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public dynamic GetPromoReasonList()
        {
            objCore = new Report();
            string strCatchMessage = ""; bool bReturn = false; int intCnt = 0;
            string strControlCode = string.Empty;
            DataSet ds = new DataSet();

            string strSession = string.Empty;
            string strErr = string.Empty;
            objComm = new classes.CommonClass();
            try
            {
                bReturn = objCore.FetchPromoReasonParameters(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {
                    return ds.Tables[0];
                }
                else
                    return null;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        #endregion

        #region FetchSearchParameters
        private void FetchSearchParameters(Guid UserID)
        {
            objCore = new Report();
            string strCatchMessage = ""; bool bReturn = false; int intCnt = 0;
            string strControlCode = string.Empty;
            DataSet ds = new DataSet();
            
            string strSession = string.Empty;
            string strErr = string.Empty;
            objComm = new classes.CommonClass();

            try
            {
                objCore.USER_ID = UserID;

                bReturn = objCore.FetchPromoReasonParameters(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {
                    #region promo reason
                    //DataRow dr1 = ds.Tables["promo_reasons"].NewRow();
                    //dr1["id"] = Guid.Empty;
                    //dr1["reason"] = "All";
                    //ds.Tables["promo_reasons"].Rows.InsertAt(dr1, 0);
                    //ds.Tables["promo_reasons"].AcceptChanges();

                    //ddlPromoReason.DataSource = ds.Tables["promo_reasons"];
                    //ddlPromoReason.DataValueField = "id";
                    //ddlPromoReason.DataTextField = "reason";
                    //ddlPromoReason.DataBind();
                    #endregion

                    #region Institution
                    intCnt = ds.Tables["institutions"].Rows.Count;
                    DataRow dr3 = ds.Tables["institutions"].NewRow();
                    dr3["id"] = Guid.Empty;
                    dr3["name"] = "All";
                    ds.Tables["institutions"].Rows.InsertAt(dr3, 0);
                    ds.Tables["institutions"].AcceptChanges();

                    ddlInstitution.DataSource = ds.Tables["institutions"];
                    ddlInstitution.DataValueField = "id";
                    ddlInstitution.DataTextField = "name";
                    ddlInstitution.DataBind();

                    if (intCnt == 1) ddlInstitution.SelectedIndex = 1;
                    #endregion
                }
                else
                    hdnError.Value = strCatchMessage.Trim();

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

        #region GenerateExcel
        [AjaxPro.AjaxMethod()]
        public string[] GenerateExcel(string[] arrRecord)
        {
            objCore = new Report();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();
            string[] arrRet = new string[4];
            string strRptFile = string.Empty;
            try
            {
                objComm.SetRegionalFormat();
                objCore.FromDate = Convert.ToDateTime(arrRecord[0]);
                objCore.ToDate = Convert.ToDateTime(arrRecord[1]);
                objCore.PromoReasonId = Convert.ToString(arrRecord[2].Trim());
                objCore.InstitutionId = Guid.Parse(arrRecord[3].Trim());

                bReturn = objCore.LoadPromoReportList(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {
                    strRptFile = CreateReportExcel(ds, objCore);
                    arrRet[0] = "true";
                    arrRet[1] = "Reports/Temp/" + strRptFile;
                    arrRet[2] = Server.MapPath("Reports/Temp/" + strRptFile).Replace("ajaxpro\\", "");
                    arrRet[3] = "XLS";
                    //spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"\" />";
                }
                //else
                    //spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + strCatchMessage + "\" />";

            }
            catch (Exception ex)
            {
                //spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose();
                objCore = null;
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

        #region CreateReportExcel
        public string CreateReportExcel(DataSet ds, Report core)
        {
            string strReportName = string.Empty;
            string strCatchMsg = ""; bool bReturn = false;
            SetLicenseKey();
            ExcelFile objExcelFile = null; string strRowfiltertring = string.Empty;
            ExcelWorksheet sheet = null;
            //ExcelWorksheet objExcelWorksheet2 = null;
            string strRName = "DiscountApplied_" + string.Format("{0:yyyy_MM_dd_HH_mm_ss}", DateTime.Now) + ".xlsx";
            objComm = new classes.CommonClass();
            try
            {
                objComm.SetRegionalFormat();
                if (!Directory.Exists(Server.MapPath("Reports/Temp"))) Directory.CreateDirectory(Server.MapPath("Reports/Temp"));
                strReportName = Server.MapPath("Reports/Temp/" + strRName);
                strReportName = strReportName.Replace("ajaxpro\\", "");
                if (System.IO.File.Exists(strReportName) == true) { System.IO.File.Delete(strReportName); }

                objExcelFile = new ExcelFile();
                sheet = objExcelFile.Worksheets.Add("Discount Applied");

                int intRowIndex = 0;


                #region Details
                var data = new List<Report>();
                
                foreach (DataRow dr in ds.Tables["Report"].Rows)
                {
                    Report item = new Report();
                    item.id = new Guid(dr["id"].ToString());
                    item.promo_reason = dr["reason"].ToString();
                    item.received_date = Convert.ToDateTime(dr["received_date"]);
                    item.patient_name = Convert.ToString(dr["patient_name"]).Trim();
                    item.category = dr["category"].ToString();
                    item.modality = Convert.ToString(dr["modality"]);
                    item.priority = Convert.ToString(dr["priority"]);
                    item.institution = Convert.ToString(dr["institution"]).Trim();
                    item.billing_account = Convert.ToString(dr["billing_account"]).Trim();
                    item.invoice_no = Convert.ToString(dr["invoice_no"]).Trim();
                    item.invoice_date = Convert.ToDateTime(dr["invoice_date"]);
                    item.discount_type = Convert.ToString(dr["discount_type"]).Trim();
                    item.discount_percentage = Convert.ToDecimal(dr["discount_percentage"]);
                    item.discount_amount = Convert.ToDecimal(dr["discount_amount"]);
                    item.study_cost = Convert.ToDecimal(dr["study_cost"]);
                    //if (levelRows.Where(x => x.ArPaymentId == item.id).Count() > 0)
                    //{
                    //    item.AdjustedPayments.AddRange(levelRows.Where(x => x.ArPaymentId == item.id));
                    //}
                    data.Add(item);
                }

                #endregion

                CreateHeader(ref intRowIndex, sheet);
                List<GroupData> groupData = new List<GroupData>();
                if (data.Count > 0)
                {
                    groupData = data.GroupBy(i => i.promo_reason).Select(i => new GroupData { Reports = i.ToList(),GroupAmount=i.Sum(l=> l.discount_amount.Value) }).ToList();
                }
                foreach (var item in groupData)
                {
                    CreateBodyRow(ref intRowIndex, item.Reports,item.GroupAmount, sheet);
                }
                //Grand total
                var style = new CellStyle();
                style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                style.Font.Weight = ExcelFont.BoldWeight;
                sheet.Cells.GetSubrange("A" + (intRowIndex + 1).ToString(), "N" + (intRowIndex + 1).ToString()).Style = style;
                sheet.Cells[intRowIndex, 12].Value = "Grand Total($):";
                sheet.Cells[intRowIndex, 13].Value = groupData.Sum(i => i.Reports.Sum(j=> j.discount_amount));
                sheet.Cells[intRowIndex, 13].Style.NumberFormat = " #,##0.00; -#,##0.00";

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

            sheet.Cells.GetSubrange("A1", "N1").Style = style;

            //sheet.Cells[row, 0].Value = "Sl. No.";
            sheet.Cells[row, 0].Value = "Reason for Promotion";
            sheet.Cells[row, 1].Value = "Received Date";
            sheet.Cells[row, 2].Value = "Patient Name";
            sheet.Cells[row, 3].Value = "Category";
            sheet.Cells[row, 4].Value = "Modality";

            sheet.Cells[row, 5].Value = "Prirority";
            sheet.Cells[row, 6].Value = "Institution";
            sheet.Cells[row, 7].Value = "Billing Account Name";
            sheet.Cells[row, 8].Value = "Invoice #";
            sheet.Cells[row, 9].Value = "Invoice Date";
            sheet.Cells[row, 10].Value = "Study Cost($)";
            sheet.Cells[row, 11].Value = "Discount Type";
            sheet.Cells[row, 12].Value = "Discount Rate";
            sheet.Cells[row, 13].Value = "Discount Amount($)";

            row++;
        }
        #endregion

        void CreateBodyRow(ref int row, List<Report> reports,decimal GroupAmount, ExcelWorksheet sheet)
        {
            var style = new CellStyle();
            string groupReason = string.Empty;
            string reasonFirstRow = string.Empty;
            objComm = new classes.CommonClass();
            objComm.SetRegionalFormat();
            foreach (var data in reports)
            {
                //style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                //style.VerticalAlignment = VerticalAlignmentStyle.Center;
                //style.Font.Weight = ExcelFont.BoldWeight;
                style.Borders.SetBorders(MultipleBorders.Bottom, Color.Black, LineStyle.Thin);
                style.Borders.SetBorders(MultipleBorders.Top, Color.Black, LineStyle.Thin);
                sheet.Cells.GetSubrange("A" + (row + 1).ToString(), "N" + (row + 1).ToString()).Style = style;
                groupReason = data.promo_reason;
                if (reasonFirstRow != data.promo_reason)
                    sheet.Cells[row, 0].Value = data.promo_reason;
                reasonFirstRow = data.promo_reason;
                sheet.Cells[row, 1].Value = data.received_date.ToString(objComm.DateFormat);
                sheet.Cells[row, 2].Value = data.patient_name;
                sheet.Cells[row, 3].Value = data.category;
                sheet.Cells[row, 4].Value = data.modality;
                sheet.Cells[row, 5].Value = data.priority;
                sheet.Cells[row, 6].Value = data.institution;
                sheet.Cells[row, 7].Value = data.billing_account;
                sheet.Cells[row, 8].Value = data.invoice_no;
                sheet.Cells[row, 9].Value = data.invoice_date.ToString(objComm.DateFormat);
                sheet.Cells[row, 10].Value = data.study_cost;
                sheet.Cells[row, 10].Style.NumberFormat = " #,##0.00; -#,##0.00";
                sheet.Cells[row, 11].Value = data.discount_type;
                sheet.Cells[row, 12].Value = data.discount_percentage;
                sheet.Cells[row, 12].Style.NumberFormat = " #,##0.00; -#,##0.00";
                sheet.Cells[row, 13].Value = data.discount_amount;
                sheet.Cells[row, 13].Style.NumberFormat = " #,##0.00; -#,##0.00";
                row++;
            }
            sheet.Rows[row].InsertEmpty(1);
            row++;
            style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            style.Font.Weight = ExcelFont.BoldWeight;
            sheet.Cells.GetSubrange("A" + (row + 1).ToString(), "N" + (row + 1).ToString()).Style = style;
            sheet.Cells[row, 12].Value = groupReason+" Total($):";
            sheet.Cells[row, 13].Value = GroupAmount;
            sheet.Cells[row, 13].Style.NumberFormat = " #,##0.00; -#,##0.00";
            row++;
            sheet.Rows[row].InsertEmpty(1);
            row++;
        }
    }

    public class GroupData
    {
        public List<Report> Reports { get; set; }
        public decimal GroupAmount { get; set; }
    }
}