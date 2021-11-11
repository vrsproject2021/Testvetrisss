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

namespace VETRIS.Radiologist
{
    public partial class VRSRadAssignBrw : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.Radiologist.AssignStudy objCore = null;
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!CallBackBrw.CausedCallback)
                SetPageValue();
            SetAttributes();
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {

            btnRefresh.Attributes.Add("onclick", "javascript:ResetRecord();view_Searchform();");
            btnSearch.Attributes.Add("onclick", "javascript:SearchRecord();view_Searchform();");
            btnReset.Attributes.Add("onclick", "javascript:ResetRecord();");
            btnClose.Attributes.Add("onclick", "javascript:btnBrwClose_Onclick();");


            txtFromDt.Attributes.Add("onblur", "javascript:ValidateDateEntered(this);");
            imgFrom.Attributes.Add("onclick", "javascript:SetSelectedDate('" + txtFromDt.ClientID + "','CalFrom');");
            txtTillDt.Attributes.Add("onblur", "javascript:ValidateDateEntered(this);");
            imgTill.Attributes.Add("onclick", "javascript:SetSelectedDate('" + txtTillDt.ClientID + "','CalTill');");

            btnMultipleAssign.Attributes.Add("onclick", "javascript:btnMultipleAssign_OnClick();");
        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {

            int intUserRoleID = Convert.ToInt32(Request.QueryString["urid"]);
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid UserID = new Guid(Request.QueryString["uid"].ToString());
            string strID = string.Empty;


            objComm = new classes.CommonClass();
            objComm.SetRegionalFormat();
            txtFromDt.Text = objComm.IMDateFormat(DateTime.Today.AddDays(-7), objComm.DateFormat);
            txtTillDt.Text = objComm.IMDateFormat(DateTime.Today, objComm.DateFormat);
            objComm = null;
            string strTheme = Request.QueryString["th"];
            SetCSS(strTheme);
            FetchSearchParameters(UserID);
            DeleteUserDirectory(UserID);

        }
        #endregion

        #region SetCSS
        private void SetCSS(string strTheme)
        {
            string strServerPath = ConfigurationManager.AppSettings["ServerPath"];
            lnkSTYLE.Attributes["href"] = strServerPath + "/css/" + strTheme + "/style.css";
            lnkCAL.Attributes["href"] = strServerPath + "/css/" + strTheme + "/CalendarStyle.css";
            lnkGRID.Attributes["href"] = strServerPath + "/css/" + strTheme + "/grid_style.css";
        }
        #endregion

        #region DeleteUserDirectory
        private void DeleteUserDirectory(Guid UserID)
        {
            string[] arrTemp = new string[0];
            string[] arrFiles = new string[0];

            if (Directory.Exists(Server.MapPath("~/Study/Temp/" + UserID.ToString())))
            {

                arrTemp = Directory.GetFiles(Server.MapPath("~") + "/Study/Temp/" + UserID.ToString());
                if (arrTemp.Length > 0)
                {
                    for (int i = 0; i < arrTemp.Length; i++)
                    {
                        File.Delete(arrTemp[i]);
                    }
                }
                Directory.Delete(Server.MapPath("~") + "/Study/Temp/" + UserID.ToString());

            }

            arrTemp = new string[0];
            if (Directory.Exists(Server.MapPath("~/CaseList/Temp/" + UserID.ToString())))
            {

                arrTemp = Directory.GetFiles(Server.MapPath("~") + "/CaseList/Temp/" + UserID.ToString());
                if (arrTemp.Length > 0)
                {
                    for (int i = 0; i < arrTemp.Length; i++)
                    {
                        File.Delete(arrTemp[i]);
                    }
                }

                arrTemp = new string[0];
                arrTemp = Directory.GetDirectories(Server.MapPath("~") + "/CaseList/Temp/" + UserID.ToString());
                if (arrTemp.Length > 0)
                {
                    for (int i = 0; i < arrTemp.Length; i++)
                    {
                        arrFiles = new string[0];
                        arrFiles = Directory.GetFiles(arrTemp[i]);
                        for (int j = 0; j < arrFiles.Length; j++)
                        {
                            File.Delete(arrFiles[j]);
                        }

                        Directory.Delete(arrTemp[i]);
                    }
                }

                Directory.Delete(Server.MapPath("~") + "/CaseList/Temp/" + UserID.ToString());

            }
        }
        #endregion

        #region FetchSearchParameters
        private void FetchSearchParameters(Guid UserID)
        {
            objCore = new Core.Radiologist.AssignStudy();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            int intCnt = 0;
            objComm = new classes.CommonClass();

            try
            {
                objCore.USER_ID = UserID;

                bReturn = objCore.FetchBrowserParameters(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {
                    #region Modality
                    DataRow dr1 = ds.Tables["Modality"].NewRow();
                    dr1["id"] = 0;
                    dr1["name"] = "Select One";
                    ds.Tables["Modality"].Rows.InsertAt(dr1, 0);
                    ds.Tables["Modality"].AcceptChanges();

                    ddlModality.DataSource = ds.Tables["Modality"];
                    ddlModality.DataValueField = "id";
                    ddlModality.DataTextField = "name";
                    ddlModality.DataBind();
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

                    #region Institution
                    intCnt = ds.Tables["Institutions"].Rows.Count;
                    DataRow dr3 = ds.Tables["Institutions"].NewRow();
                    dr3["id"] = "00000000-0000-0000-0000-000000000000";
                    dr3["name"] = "Select One";
                    ds.Tables["Institutions"].Rows.InsertAt(dr3, 0);
                    ds.Tables["Institutions"].AcceptChanges();

                    ddlInstitution.DataSource = ds.Tables["Institutions"];
                    ddlInstitution.DataValueField = "id";
                    ddlInstitution.DataTextField = "name";
                    ddlInstitution.DataBind();

                    if (intCnt == 1) ddlInstitution.SelectedIndex = 1;
                    #endregion

                    #region Status
                    DataRow dr4 = ds.Tables["WLStatus"].NewRow();
                    dr4["status_id"] = "-1";
                    dr4["status_desc"] = "All";
                    ds.Tables["WLStatus"].Rows.InsertAt(dr4, 0);
                    ds.Tables["WLStatus"].AcceptChanges();

                    ddlStatus.DataSource = ds.Tables["WLStatus"];
                    ddlStatus.DataValueField = "status_id";
                    ddlStatus.DataTextField = "status_desc";
                    ddlStatus.DataBind();
                    #endregion

                    #region Category
                    DataRow dr5 = ds.Tables["Category"].NewRow();
                    dr5["id"] = "0";
                    dr5["name"] = "Select One";
                    ds.Tables["Category"].Rows.InsertAt(dr5, 0);
                    ds.Tables["Category"].AcceptChanges();

                    ddlCategory.DataSource = ds.Tables["Category"];
                    ddlCategory.DataValueField = "id";
                    ddlCategory.DataTextField = "name";
                    ddlCategory.DataBind();
                    #endregion

                    #region Priority

                    foreach (DataRow dr in ds.Tables["Priority"].Rows)
                    {
                        if (hdnPriority.Value.Trim() != string.Empty) hdnPriority.Value += objComm.RecordDivider;
                        hdnPriority.Value += Convert.ToString(dr["priority_id"]) + objComm.RecordDivider;
                        hdnPriority.Value += Convert.ToString(dr["priority_desc"]).Trim();
                    }
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

        #region CallBackBrw_Callback
        protected void CallBackBrw_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            SearchRecord(e.Parameters);
            grdBrw.Width = Unit.Percentage(99);
            grdBrw.RenderControl(e.Output);
            spnERR.RenderControl(e.Output);
        }
        #endregion

        #region SearchRecord
        private void SearchRecord(string[] arrRecord)
        {
            objCore = new Core.Radiologist.AssignStudy();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            string strUserRole = string.Empty;
            objComm = new classes.CommonClass();


            try
            {
                objComm.SetRegionalFormat();
                objCore.PATIENT_NAME = arrRecord[0].Trim();
                objCore.MODALITY_ID = Convert.ToInt32(arrRecord[1]);
                objCore.FILTER_BY_RECEIVED_DATE = arrRecord[2].Trim();
                objCore.RECEIVED_DATE_FROM = Convert.ToDateTime(arrRecord[3]);
                objCore.RECEIVED_DATE_TILL = Convert.ToDateTime(arrRecord[4]);
                objCore.INSTITUTION_ID = new Guid(arrRecord[5]);
                objCore.PACS_STATUS_ID = Convert.ToInt32(arrRecord[6]);
                objCore.CATEGORY_ID = Convert.ToInt32(arrRecord[7]);
                objCore.SPECIES_ID = Convert.ToInt32(arrRecord[8]);
                objCore.USER_ID = new Guid(arrRecord[9].Trim());
                strUserRole = arrRecord[10].Trim();

                bReturn = objCore.SearchWorklistBrowserList(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {

                    grdBrw.DataSource = ds.Tables["BrowserList"];
                    grdBrw.Levels[0].Columns[3].FormatString = objComm.DateFormat + " HH:mm";
                    grdBrw.Levels[0].Columns[5].FormatString = objComm.DateFormat + " HH:mm";
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
    }
}