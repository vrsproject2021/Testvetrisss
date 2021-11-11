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

namespace VETRIS.Masters
{
    [AjaxPro.AjaxNamespace("VRSSpecies")]
    public partial class VRSSpecies : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.Master.Species objCore = null;
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSSpecies));
            SetAttributes();
            if ((!CallBackBrw.CausedCallback))
                SetPageValue();
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {

            btnRefresh.Attributes.Add("onclick", "javascript:ResetRecord();view_Searchform();");
            btnSearch.Attributes.Add("onclick", "javascript:SearchRecord();view_Searchform();");
            btnAdd.Attributes.Add("onclick", "javascript:btnAdd_OnClick();");
            btnSave.Attributes.Add("onclick", "javascript:btnSave_OnClick();");
            btnClose.Attributes.Add("onclick", "javascript:btnBrwClose_Onclick();");

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

        #region Grid

        #region CallBackBrw_Callback
        protected void CallBackBrw_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            string strAction = e.Parameters[0];

            switch (strAction)
            {
                case "L":
                    LoadRecords(e.Parameters);
                    break;
                case "A":
                    AddRecord(e.Parameters);
                    break;
                case "D":
                    DeleteRecord(e.Parameters);
                    break;
            }
            grdBrw.Width = Unit.Percentage(100);
            grdBrw.RenderControl(e.Output);
            spnERR.RenderControl(e.Output);
        }
        #endregion

        #region LoadRecords
        private void LoadRecords(string[] arrParams)
        {
            objCore = new Core.Master.Species();
            string strCatchMessage = ""; string strReturnMessage = "";
            bool bReturn = false;
            DataSet ds = new DataSet();
            Guid UserID = Guid.Empty;
            string strFileName = "";

            try
            {

                objCore.CODE = Convert.ToString(arrParams[1]).Trim();
                objCore.NAME = Convert.ToString(arrParams[2]).Trim();
                objCore.IS_ACTIVE = Convert.ToString(arrParams[3]).Trim();
                objCore.USER_ID = new Guid(arrParams[4]);
                objCore.MENU_ID = Convert.ToInt32(arrParams[5]);

                bReturn = objCore.SearchRecords(Server.MapPath("~"), ref ds, ref strReturnMessage, ref strCatchMessage);
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

        #region AddRecord
        private void AddRecord(string[] arrParams)
        {
            DataTable dtbl = new DataTable();
            objComm = new classes.CommonClass();
            string[] arrSep = { objComm.SecondaryRecordDivider };
            string[] arrRecords = arrParams[1].Split(arrSep, StringSplitOptions.None);
            int intSrl = 0;

            try
            {
                dtbl = CreateTable();
                if (arrRecords.Length > 0)
                {
                    if (arrRecords[0].Trim() != "")
                    {
                        for (int i = 0; i < arrRecords.Length; i = i + 6)
                        {
                            DataRow dr = dtbl.NewRow();
                            intSrl = intSrl + 1;
                            dr["rec_id"] = intSrl;
                            dr["id"] = arrRecords[i + 1].Trim();
                            dr["code"] = arrRecords[i + 2].Trim();
                            dr["name"] = arrRecords[i + 3].Trim();
                            dr["is_active"] = arrRecords[i + 4].Trim();
                            dr["changed"] = arrRecords[i + 5].Trim();
                            dr["action"] = "";
                            dtbl.Rows.Add(dr);
                        }

                    }
                }

                DataRow drNew = dtbl.NewRow();
                intSrl = intSrl + 1;
                drNew["rec_id"] = intSrl;
                drNew["id"] = 0;
                drNew["code"] = "";
                drNew["name"] = "";
                drNew["is_active"] = "Y";
                drNew["changed"] = "Y";
                drNew["action"] = "";
                dtbl.Rows.Add(drNew);

                DataView dv = new DataView(dtbl);
                dv.Sort = "rec_id desc";

                grdBrw.DataSource = dtbl;
                grdBrw.DataBind();
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

        #region DeleteRecord
        private void DeleteRecord(string[] arrParams)
        {
            DataTable dtbl = new DataTable();
            objComm = new classes.CommonClass();
            int intID = Convert.ToInt32(arrParams[1]);
            string[] arrSep = { objComm.SecondaryRecordDivider };
            string[] arrRecords = arrParams[2].Split(arrSep, StringSplitOptions.None);
            int intSrl = 0;

            try
            {
                dtbl = CreateTable();
                if (arrRecords[0].Trim() != "")
                {
                    for (int i = 0; i < arrRecords.Length; i = i + 6)
                    {
                        if (Convert.ToInt32(arrRecords[i]) != intID)
                        {
                            DataRow dr = dtbl.NewRow();
                            intSrl = intSrl + 1;
                            dr["rec_id"] = intSrl;
                            dr["id"] = arrRecords[i + 1].Trim();
                            dr["code"] = arrRecords[i + 2].Trim();
                            dr["name"] = arrRecords[i + 3].Trim();
                            dr["is_active"] = arrRecords[i + 4].Trim();
                            dr["changed"] = arrRecords[i + 5].Trim();
                            dr["action"] = "";
                            dtbl.Rows.Add(dr);
                        }
                    }
                }

                grdBrw.DataSource = dtbl;
                grdBrw.DataBind();
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

        #region CreateTable
        private DataTable CreateTable()
        {
            DataTable dtbl = new DataTable();
            dtbl.Columns.Add("rec_id", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("id", System.Type.GetType("System.String"));
            dtbl.Columns.Add("code", System.Type.GetType("System.String"));
            dtbl.Columns.Add("name", System.Type.GetType("System.String"));
            dtbl.Columns.Add("is_active", System.Type.GetType("System.String"));
            dtbl.Columns.Add("changed", System.Type.GetType("System.String"));
            dtbl.Columns.Add("action", System.Type.GetType("System.String"));
            dtbl.TableName = "BrowserList";
            return dtbl;
        }
        #endregion

        #endregion

        #region SaveRecord (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] SaveRecord(string[] ArrRecord, string[] ArrParams)
        {
            bool bReturn = false;
            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty;
            int intCompanyID = 0; string strFileName = string.Empty; Guid UserID = Guid.Empty;
            objComm = new classes.CommonClass();
            objCore = new Core.Master.Species();
            int intListIndex = 0;
            Core.Master.Species[] objRecords = new Core.Master.Species[0];

            try
            {

                objCore.USER_ID = UserID = new Guid(ArrParams[0]);
                objCore.MENU_ID = Convert.ToInt32(ArrParams[1]);


                objRecords = new Core.Master.Species[(ArrRecord.Length / 5)];

                for (int i = 0; i < objRecords.Length; i++)
                {
                    objRecords[i] = new Core.Master.Species();
                    objRecords[i].ROW_ID = Convert.ToInt32(ArrRecord[intListIndex]);
                    objRecords[i].ID = Convert.ToInt32(ArrRecord[intListIndex + 1]);
                    objRecords[i].CODE = ArrRecord[intListIndex + 2].Trim();
                    objRecords[i].NAME = ArrRecord[intListIndex + 3].Trim();
                    objRecords[i].IS_ACTIVE = ArrRecord[intListIndex + 4].Trim();
                    intListIndex = intListIndex + 5;
                }

                intListIndex = 0;


                bReturn = objCore.SaveRecord(Server.MapPath("~"), objRecords, ref strReturnMsg, ref strCatchMessage);

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