using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VETRIS.Core.Dashboard;

namespace VETRIS.Dashboard.HourlySubmittedCases
{
    [AjaxPro.AjaxNamespace("VRSHourlySubmittedCases")]
    public partial class VRSHourlySubmittedCases : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.Dashboard.Dashboard objCore = null;
        classes.CommonClass objComm;
        int dashboardId = 0;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSHourlySubmittedCases));
            SetPageValue();
        }

        #region SetPageValue
        private void SetPageValue()
        {
            hdnRefreshTime.Value = Request.QueryString["sec"];
            hdnIsRefreshBtn.Value = Request.QueryString["isrefresh"];
            hdnDesc.Value = Request.QueryString["desc"];
            hdnreportTitle.Value = Request.QueryString["rt"];
            string strTheme = Request.QueryString["th"];
            hdnTheme.Value = strTheme;
            SetCSS(strTheme);
            GetModality();
        }
        #endregion

        #region SetCSS
        private void SetCSS(string strTheme)
        {
            string strServerPath = ConfigurationManager.AppSettings["ServerPath"];
            lnkSTYLE.Attributes["href"] = strServerPath + "/css/" + strTheme + "/style.css";
            dashboardSTYLE.Attributes["href"] = strServerPath + "/css/" + strTheme + "/dashboard.css";
        }
        #endregion

        [AjaxPro.AjaxMethod()]
        public void GetModality()
        {
            objCore = new Core.Dashboard.Dashboard();
            string strCatchMessage = "";
            bool bReturn = false;
            DataSet ds = new DataSet();

            try
            {
                bReturn = objCore.FetchModality(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    #region Modality
                    DataRow dr1 = ds.Tables["Modality"].NewRow();
                    dr1["id"] = 0;
                    dr1["name"] = "All";
                    ds.Tables["Modality"].Rows.InsertAt(dr1, 0);
                    ds.Tables["Modality"].AcceptChanges();

                    ddlModality.DataSource = ds.Tables["Modality"];
                    ddlModality.DataValueField = "id";
                    ddlModality.DataTextField = "name";
                    ddlModality.DataBind();
                    #endregion
                }

            }
            catch (Exception ex)
            {

            }

        }

        #region SearchRecord
        [AjaxPro.AjaxMethod()]
        public LineData SearchRecord(string[] arrRecord)
        {
            objCore = new Core.Dashboard.Dashboard();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            string strUserRole = string.Empty;
            objComm = new classes.CommonClass();
            LineData data = null;
            try
            {
                objComm.SetRegionalFormat();
                objCore.FROM_DATE = Convert.ToDateTime(arrRecord[0]);
                objCore.TO_DATE = Convert.ToDateTime(arrRecord[1]);
                objCore.TYPE = Convert.ToInt32(arrRecord[2]);
                objCore.MODALITY_ID = Convert.ToInt32(arrRecord[3]);
                bReturn = objCore.SearchLineChartSubmittedCases(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {
                    data = new LineData { labels = new List<string>(), datasets = new List<SlotData>() };
                    Regex creg = new Regex(@"\d+-\d+");
                    foreach (DataColumn c in ds.Tables["submitted_cases"].Columns)  //loop through the columns. 
                    {
                        if (creg.IsMatch(c.ColumnName))
                        {
                            data.labels.Add(c.ColumnName);
                        }
                    }

                    foreach (DataRow dr in ds.Tables["submitted_cases"].Rows)
                    {
                        var dataset = new SlotData { label = dr["modality"].ToString(), data = new List<int>() };
                        foreach (DataColumn c in dr.Table.Columns)  //loop through the columns. 
                        {
                            if (creg.IsMatch(c.ColumnName))
                            {
                                dataset.data.Add(Convert.ToInt32(dr[c.ColumnName].ToString()));
                            }
                        }
                        data.datasets.Add(dataset);
                    }
                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }
            return data;
        }
        #endregion
    }
}