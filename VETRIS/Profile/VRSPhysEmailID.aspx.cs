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


namespace VETRIS.Profile
{
    public partial class VRSPhysEmailID : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.Profile.Institution objCore = null;
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            SetAttributes();
            if ((!CallBackEmail.CausedCallback))
                SetPageValue();
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {

            btnClose.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
            btnAdd.Attributes.Add("onclick", "javascript:btnAdd_OnClick();");
            btnDone.Attributes.Add("onclick", "javascript:btnDone_OnClick();");
        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {

            objComm = new classes.CommonClass();
            //hdnID.Value = Request.QueryString["id"];
            //hdnPhysID.Value = Request.QueryString["phys"];
            hdnDivider.Value = objComm.RecordDivider.ToString();
            hdnSecDivider.Value = objComm.SecondaryRecordDivider;
            objComm = null;
        }
        #endregion

        #region CallBackEmail_Callback
        protected void CallBackEmail_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            string strAction = e.Parameters[0];

            switch (strAction)
            {
                case "L":
                    LoadEmail(e.Parameters);
                    break;
                case "A":
                    AddEmail(e.Parameters);
                    break;
                case "D":
                    DeleteEmail(e.Parameters);
                    break;
            }

            grdEmail.Width = Unit.Percentage(100);
            grdEmail.RenderControl(e.Output);
            spnErr.RenderControl(e.Output);
        }
        #endregion

        #region LoadEmail
        private void LoadEmail(string[] arrParams)
        {
            string strCatchMessage = ""; bool bReturn = false;
            DataTable dtbl = CreateEmailTable();
            string strEmails = string.Empty;
            string[] arr = new string[0];


            try
            {

                
                strEmails = arrParams[1].Trim();

                if (strEmails.Trim() != "")
                {
                    if (strEmails.Contains(';'))
                    {
                        arr = strEmails.Split(';');
                        for (int i = 0; i < arr.Length; i++)
                        {
                            DataRow dr = dtbl.NewRow();
                            dr["rec_id"] = i + 1;
                            dr["physician_email"] = arr[i];
                            dr["del"] = "";
                            dtbl.Rows.Add(dr);
                        }
                    }
                    else
                    {
                        DataRow dr = dtbl.NewRow();
                        dr["rec_id"] = 1;
                        dr["physician_email"] = strEmails;
                        dr["del"] = "";
                        dtbl.Rows.Add(dr);
                    }
                }


                grdEmail.DataSource = dtbl;
                grdEmail.DataBind();
                spnErr.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"\" />";

            }
            catch (Exception ex)
            {
                spnErr.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + ex.Message.Trim() + "\" />";
            }
           
        }
        #endregion

        #region AddEmail
        private void AddEmail(string[] arrParams)
        {
            DataTable dtbl = new DataTable();
            objComm = new classes.CommonClass();
            string[] arrSep = { objComm.SecondaryRecordDivider };
            string[] arrRecords = arrParams[1].Split(arrSep, StringSplitOptions.None);

            int intSrl = 0;

            try
            {
                dtbl = CreateEmailTable();
                if (arrRecords.Length > 0)
                {
                    if (arrRecords[0].Trim() != "")
                    {
                        for (int i = 0; i < arrRecords.Length; i = i + 2)
                        {
                            DataRow dr = dtbl.NewRow();
                            intSrl = intSrl + 1;
                            dr["rec_id"] = intSrl;
                            dr["physician_email"] = arrRecords[i + 1].Trim();
                            dr["del"] = "";
                            dtbl.Rows.Add(dr);
                        }

                    }
                }

                DataRow drNew = dtbl.NewRow();
                intSrl = intSrl + 1;
                drNew["rec_id"] = intSrl;
                drNew["physician_email"] = "";
                drNew["del"] = "";
                dtbl.Rows.Add(drNew);

                grdEmail.DataSource = dtbl;
                grdEmail.DataBind();
                spnErr.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"\" />";
            }
            catch (Exception ex)
            {
                spnErr.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                dtbl.Dispose();
                objComm = null;
            }
        }
        #endregion

        #region DeleteEmail
        private void DeleteEmail(string[] arrParams)
        {
            DataTable dtbl = new DataTable();
            objComm = new classes.CommonClass();
            int intID = Convert.ToInt32(arrParams[1]);
            string[] arrSep = { objComm.SecondaryRecordDivider };
            string[] arrRecords = arrParams[2].Split(arrSep, StringSplitOptions.None);
            int intSrl = 0;

            try
            {
                dtbl = CreateEmailTable();
                if (arrRecords[0].Trim() != "")
                {
                    for (int i = 0; i < arrRecords.Length; i = i + 2)
                    {
                        if (Convert.ToInt32(arrRecords[i]) != intID)
                        {
                            DataRow dr = dtbl.NewRow();
                            intSrl = intSrl + 1;
                            dr["rec_id"] = intSrl;
                            dr["physician_email"] = arrRecords[i + 1].Trim();
                            dr["del"] = "";
                            dtbl.Rows.Add(dr);
                        }
                    }
                }

                grdEmail.DataSource = dtbl;
                grdEmail.DataBind();
                spnErr.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"\" />";

            }
            catch (Exception ex)
            {
                spnErr.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                dtbl.Dispose();
                objComm = null;
            }
        }
        #endregion

        #region CreateEmailTable
        private DataTable CreateEmailTable()
        {
            DataTable dtbl = new DataTable();
            dtbl.Columns.Add("rec_id", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("physician_email", System.Type.GetType("System.String"));
            dtbl.Columns.Add("del", System.Type.GetType("System.String"));
            dtbl.TableName = "Emails";
            return dtbl;
        }
        #endregion
    }
}