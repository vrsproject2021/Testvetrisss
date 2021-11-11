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

namespace VETRIS.Profile
{
     [AjaxPro.AjaxNamespace("VRSInstitutionDlg")]
    public partial class VRSInstitutionDlg : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.Profile.Institution objCore = null;
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSInstitutionDlg));
            SetAttributes();
            if ((!CallBackPhys.CausedCallback) && (!CallBackCred.CausedCallback) 
                //&& (!CallBackPromo.CausedCallback)
                )
                SetPageValue();
        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {
            int intUserRoleID = Convert.ToInt32(Request.QueryString["urid"]);
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            Guid UserID = new Guid(Request.QueryString["uid"]);

            hdnID.Value = Request.QueryString["id"];
            LoadDetails(intMenuID, UserID);
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
        #region SetAttributes
        private void SetAttributes()
        {
           
            btnSave1.Attributes.Add("onclick", "javascript:btnSave_OnClick();");
            btnSave2.Attributes.Add("onclick", "javascript:btnSave_OnClick();");
            btnReset1.Attributes.Add("onclick", "javascript:btnReset_OnClick();");
            btnReset2.Attributes.Add("onclick", "javascript:btnReset_OnClick();");
            btnClose1.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
            btnClose2.Attributes.Add("onclick", "javascript:btnClose_OnClick();");

            
            btnAddPhys.Attributes.Add("onclick", "javascript:btnAddPhys_OnClick();");
            btnAddCred.Attributes.Add("onclick", "javascript:btnAddCred_OnClick();");
          
        }
        #endregion

        #region LoadDetails
        private void LoadDetails(int intMenuID, Guid UserID)
        {
            objCore = new Core.Profile.Institution();
            string strCatchMessage = ""; string strReturnMessage = string.Empty; bool bReturn = false;
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();

            try
            {
                objComm.SetRegionalFormat();
                objCore.ID = new Guid(hdnID.Value);
                objCore.MENU_ID = intMenuID;
                objCore.USER_ID = UserID;

                bReturn = objCore.LoadDetails(Server.MapPath("~"), ref ds, ref strReturnMessage, ref strCatchMessage);

                if (bReturn)
                {
                    lblCode.Text = objCore.CODE;
                    lblName.Text = objCore.NAME;
                    lblAddr1.Text = objCore.ADDRESS_LINE1;
                    lblAddr2.Text = objCore.ADDRESS_LINE2;
                    lblCity.Text = objCore.CITY;
                    lblCountry.Text = objCore.COUNTRY_NAME;
                    lblState.Text = objCore.STATE_NAME;
                    lblZip.Text = objCore.ZIP;

                    txtEmailID.Text = objCore.EMAIL_ID;
                    txtTel.Text = objCore.PHONE;
                    txtMobile.Text = objCore.MOBILE;
                    txtContPerson.Text = objCore.CONTACT_PERSON_NAME;
                    txtContMobile.Text = objCore.CONTACT_PERSION_MOBILE;
                    if (objCore.IS_ACTIVE == "Y") lblStatus.Text = "Active"; else lblStatus.Text = "Inactive";
                    
                    
                    lblBA.Text= objCore.BILLING_ACCOUNT_NAME;
                    if (objCore.DICOM_FILES_TRANSFER_METHOD == "M") lblDRInstalled.Text = "Yes"; else lblDRInstalled.Text = "No";

                }
                else
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
                ds.Dispose(); objCore = null; objComm = null;
            }
        }
        #endregion

        #region Physician Grid

        #region CallBackPhys_Callback
        protected void CallBackPhys_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            string strAction = e.Parameters[0];

            switch (strAction)
            {
                case "L":
                    LoadPhysicians(e.Parameters);
                    break;
                case "A":
                    AddPhysician(e.Parameters);
                    break;
                case "D":
                    DeletePhysician(e.Parameters);
                    break;
            }

            if (grdPhys.RecordCount > 10) grdPhys.ShowHeader = true;
            grdPhys.Width = Unit.Percentage(100);
            grdPhys.RenderControl(e.Output);
            spnErrPhys.RenderControl(e.Output);
        }
        #endregion

        #region LoadPhysicians
        private void LoadPhysicians(string[] arrParams)
        {
            objCore = new Core.Profile.Institution();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            Guid UserID = Guid.Empty;
            string strFileName = "";

            try
            {

                objCore.ID = new Guid(arrParams[1]);

                bReturn = objCore.LoadPhysicians(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    grdPhys.DataSource = ds.Tables["Physicians"];
                    grdPhys.DataBind();


                    spnErrPhys.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrPhys\" value=\"\" />";
                }
                else
                    spnErrPhys.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrPhys\" value=\"" + strCatchMessage + "\" />";



            }
            catch (Exception ex)
            {
                spnErrPhys.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrPhys\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }
        }
        #endregion

        #region AddPhysician
        private void AddPhysician(string[] arrParams)
        {
            DataTable dtbl = new DataTable();
            objComm = new classes.CommonClass();
            string[] arrSep = { objComm.SecondaryRecordDivider };
            string[] arrRecords = arrParams[1].Split(arrSep, StringSplitOptions.None);

            int intSrl = 0;

            try
            {
                dtbl = CreatePhysicianTable();
                if (arrRecords.Length > 0)
                {
                    if (arrRecords[0].Trim() != "")
                    {
                        for (int i = 0; i < arrRecords.Length; i = i + 7)
                        {
                            DataRow dr = dtbl.NewRow();
                            intSrl = intSrl + 1;
                            dr["rec_id"] = intSrl;
                            dr["physician_id"] = arrRecords[i + 1].Trim();
                            dr["physician_fname"] = arrRecords[i + 2].Trim();
                            dr["physician_lname"] = arrRecords[i + 3].Trim();
                            dr["physician_credentials"] = arrRecords[i + 4].Trim();
                            dr["physician_email"] = arrRecords[i + 5].Trim();
                            dr["physician_mobile"] = arrRecords[i + 6].Trim();
                            dr["del"] = "";
                            dtbl.Rows.Add(dr);
                        }

                    }
                }

                DataRow drNew = dtbl.NewRow();
                intSrl = intSrl + 1;
                drNew["rec_id"] = intSrl;
                drNew["physician_id"] = "00000000-0000-0000-0000-000000000000";
                drNew["physician_fname"] = "";
                drNew["physician_lname"] = "";
                drNew["physician_credentials"] = "";
                drNew["physician_email"] = "";
                drNew["physician_mobile"] = "";
                drNew["del"] = "";
                dtbl.Rows.Add(drNew);

                grdPhys.DataSource = dtbl;
                grdPhys.DataBind();
                spnErrPhys.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrPhys\" value=\"\" />";
            }
            catch (Exception ex)
            {
                spnErrPhys.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrPhys\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                dtbl.Dispose();
                objComm = null;
            }
        }
        #endregion

        #region DeletePhysician
        private void DeletePhysician(string[] arrParams)
        {
            DataTable dtbl = new DataTable();
            objComm = new classes.CommonClass();
            int intID = Convert.ToInt32(arrParams[1]);
            string[] arrSep = { objComm.SecondaryRecordDivider };
            string[] arrRecords = arrParams[2].Split(arrSep, StringSplitOptions.None);
            int intSrl = 0;

            try
            {
                dtbl = CreatePhysicianTable();
                if (arrRecords[0].Trim() != "")
                {
                    for (int i = 0; i < arrRecords.Length; i = i + 7)
                    {
                        if (Convert.ToInt32(arrRecords[i]) != intID)
                        {
                            DataRow dr = dtbl.NewRow();
                            intSrl = intSrl + 1;
                            dr["rec_id"] = intSrl;
                            dr["physician_id"] = arrRecords[i + 1].Trim();
                            dr["physician_fname"] = arrRecords[i + 2].Trim();
                            dr["physician_lname"] = arrRecords[i + 3].Trim();
                            dr["physician_credentials"] = arrRecords[i + 4].Trim();
                            dr["physician_email"] = arrRecords[i + 5].Trim();
                            dr["physician_mobile"] = arrRecords[i + 6].Trim();
                            dr["del"] = "";
                            dtbl.Rows.Add(dr);
                        }
                    }
                }

                grdPhys.DataSource = dtbl;
                grdPhys.DataBind();
                spnErrPhys.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrPhys\" value=\"\" />";

            }
            catch (Exception ex)
            {
                spnErrPhys.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrPhys\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                dtbl.Dispose();
                objComm = null;
            }
        }
        #endregion

        #region CreatePhysicianTable
        private DataTable CreatePhysicianTable()
        {
            DataTable dtbl = new DataTable();
            dtbl.Columns.Add("rec_id", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("physician_id", System.Type.GetType("System.String"));
            dtbl.Columns.Add("physician_fname", System.Type.GetType("System.String"));
            dtbl.Columns.Add("physician_lname", System.Type.GetType("System.String"));
            dtbl.Columns.Add("physician_credentials", System.Type.GetType("System.String"));
            dtbl.Columns.Add("physician_email", System.Type.GetType("System.String"));
            dtbl.Columns.Add("physician_mobile", System.Type.GetType("System.String"));
            dtbl.Columns.Add("del", System.Type.GetType("System.String"));
            dtbl.TableName = "Physicians";
            return dtbl;
        }
        #endregion

        #endregion

        #region User Grid

        #region CallBackCred_Callback
        protected void CallBackCred_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            string strAction = e.Parameters[0];

            switch (strAction)
            {
                case "L":
                    LoadUsers(e.Parameters);
                    break;
                case "A":
                    AddUser(e.Parameters);
                    break;
                case "D":
                    DeleteUser(e.Parameters);
                    break;
            }

            grdCred.Width = Unit.Percentage(100);
            grdCred.RenderControl(e.Output);
            spnErrCred.RenderControl(e.Output);
        }
        #endregion

        #region LoadUsers
        private void LoadUsers(string[] arrParams)
        {
            objCore = new Core.Profile.Institution();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            Guid UserID = Guid.Empty;
            string strFileName = "";

            try
            {

                objCore.ID = new Guid(arrParams[1]);

                bReturn = objCore.LoadUsers(Server.MapPath("~"), ref ds, ref strCatchMessage);
                if (bReturn)
                {
                    grdCred.DataSource = ds.Tables["Users"];
                    grdCred.DataBind();


                    spnErrCred.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrCred\" value=\"\" />";
                }
                else
                    spnErrCred.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrCred\" value=\"" + strCatchMessage + "\" />";



            }
            catch (Exception ex)
            {
                spnErrCred.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrCred\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                ds.Dispose();
                objCore = null;
            }
        }
        #endregion

        #region AddUser
        private void AddUser(string[] arrParams)
        {
            DataTable dtbl = new DataTable();
            objComm = new classes.CommonClass();
            string[] arrSep = { objComm.SecondaryRecordDivider };
            string[] arrRecords = arrParams[1].Split(arrSep, StringSplitOptions.None);

            int intSrl = 0;

            try
            {
                dtbl = CreateUserTable();
                if (arrRecords.Length > 0)
                {
                    if (arrRecords[0].Trim() != "")
                    {
                        for (int i = 0; i < arrRecords.Length; i = i + 9)
                        {
                            DataRow dr = dtbl.NewRow();
                            intSrl = intSrl + 1;
                            dr["rec_id"] = intSrl;
                            dr["id"] = arrRecords[i + 1].Trim();
                            dr["login_id"] = arrRecords[i + 2].Trim();
                            dr["password"] = arrRecords[i + 3].Trim();
                            dr["pacs_user_id"] = arrRecords[i + 4].Trim();
                            dr["pacs_password"] = arrRecords[i + 5].Trim();
                            dr["email_id"] = arrRecords[i + 6].Trim();
                            dr["contact_no"] = arrRecords[i + 7].Trim();
                            dr["is_active"] = arrRecords[i + 8].Trim();
                            dtbl.Rows.Add(dr);
                        }

                    }
                }

                DataRow drNew = dtbl.NewRow();
                intSrl = intSrl + 1;
                drNew["rec_id"] = intSrl;
                drNew["id"] = "00000000-0000-0000-0000-000000000000";
                drNew["login_id"] = "";
                drNew["password"] = "";
                drNew["pacs_user_id"] = "";
                drNew["pacs_password"] = "";
                drNew["email_id"] = "";
                drNew["contact_no"] = "";
                drNew["is_active"] = "Y";
                dtbl.Rows.Add(drNew);

                grdCred.DataSource = dtbl;
                grdCred.DataBind();
                spnErrCred.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrCred\" value=\"\" />";
            }
            catch (Exception ex)
            {
                spnErrCred.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrCred\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                dtbl.Dispose();
                objComm = null;
            }
        }
        #endregion

        #region DeleteUser
        private void DeleteUser(string[] arrParams)
        {
            DataTable dtbl = new DataTable();
            objComm = new classes.CommonClass();
            int intID = Convert.ToInt32(arrParams[1]);
            string[] arrSep = { objComm.SecondaryRecordDivider };
            string[] arrRecords = arrParams[2].Split(arrSep, StringSplitOptions.None);
            int intSrl = 0;

            try
            {
                dtbl = CreateUserTable();
                if (arrRecords[0].Trim() != "")
                {
                    for (int i = 0; i < arrRecords.Length; i = i + 9)
                    {
                        if (Convert.ToInt32(arrRecords[i]) != intID)
                        {
                            DataRow dr = dtbl.NewRow();
                            intSrl = intSrl + 1;
                            dr["rec_id"] = intSrl;
                            dr["id"] = arrRecords[i + 1].Trim();
                            dr["login_id"] = arrRecords[i + 2].Trim();
                            dr["password"] = arrRecords[i + 3].Trim();
                            dr["pacs_user_id"] = arrRecords[i + 4].Trim();
                            dr["pacs_password"] = arrRecords[i + 5].Trim();
                            dr["email_id"] = arrRecords[i + 6].Trim();
                            dr["contact_no"] = arrRecords[i + 7].Trim();
                            dr["is_active"] = arrRecords[i + 8].Trim();
                            dtbl.Rows.Add(dr);
                        }
                    }
                }

                grdCred.DataSource = dtbl;
                grdCred.DataBind();
                spnErrCred.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrCred\" value=\"\" />";

            }
            catch (Exception ex)
            {
                spnErrCred.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrCred\" value=\"" + ex.Message.Trim() + "\" />";
            }
            finally
            {
                dtbl.Dispose();
                objComm = null;
            }
        }
        #endregion

        #region CreateUserTable
        private DataTable CreateUserTable()
        {
            DataTable dtbl = new DataTable();
            dtbl.Columns.Add("rec_id", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("id", System.Type.GetType("System.String"));
            dtbl.Columns.Add("login_id", System.Type.GetType("System.String"));
            dtbl.Columns.Add("password", System.Type.GetType("System.String"));
            dtbl.Columns.Add("pacs_user_id", System.Type.GetType("System.String"));
            dtbl.Columns.Add("pacs_password", System.Type.GetType("System.String"));
            dtbl.Columns.Add("email_id", System.Type.GetType("System.String"));
            dtbl.Columns.Add("contact_no", System.Type.GetType("System.String"));
            dtbl.Columns.Add("is_active", System.Type.GetType("System.String"));
            dtbl.TableName = "Users";
            return dtbl;
        }
        #endregion

        #endregion

        #region Promotion Grid

        //#region CallBackPromo_Callback
        //protected void CallBackPromo_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        //{

        //    LoadPromotions(e.Parameters);
        //    grdPromo.Width = Unit.Percentage(100);
        //    grdPromo.RenderControl(e.Output);
        //    spnErrPromo.RenderControl(e.Output);
        //}
        //#endregion

        //#region LoadPromotions
        //private void LoadPromotions(string[] arrParams)
        //{
        //    objCore = new Core.Profile.Institution();
        //    string strCatchMessage = ""; bool bReturn = false;
        //    DataSet ds = new DataSet();
        //    objComm = new classes.CommonClass();

        //    try
        //    {
        //        objComm.SetRegionalFormat();
        //        objCore.ID = new Guid(arrParams[0]);

        //        bReturn = objCore.LoadPromotions(Server.MapPath("~"), ref ds, ref strCatchMessage);
        //        if (bReturn)
        //        {
        //            grdPromo.DataSource = ds.Tables["Promotions"];
        //            grdPromo.DataBind();

        //            grdPromo.Levels[0].Columns[4].FormatString = objComm.DateFormat;
        //            grdPromo.Levels[0].Columns[7].FormatString = objComm.DateFormat;
        //            grdPromo.Levels[0].Columns[8].FormatString = objComm.DateFormat;

        //            spnErrPromo.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrPromo\" value=\"\" />";
        //        }
        //        else
        //            spnErrPromo.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrPromo\" value=\"" + strCatchMessage + "\" />";



        //    }
        //    catch (Exception ex)
        //    {
        //        spnErrPromo.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErrPromo\" value=\"" + ex.Message.Trim() + "\" />";
        //    }
        //    finally
        //    {
        //        ds.Dispose();
        //        objCore = null;
        //        objComm = null;
        //    }
        //}
        //#endregion

        #endregion

        #region FetchPhysicianDetails (AjaxPro Method)
        [AjaxPro.AjaxMethod()]
        public string[] FetchPhysicianDetails(string[] arrParams)
        {
            string strReturn = string.Empty; string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            objCore = new Core.Profile.Institution();
            string[] arrRet = new string[0];

            try
            {

                objCore.EMAIL_ID = arrParams[0].Trim();
                objCore.NAME = arrParams[1].Trim();
                objCore.MOBILE = arrParams[2].Trim();

                bReturn = objCore.FetchPhysicianDetails(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {

                    if (ds.Tables["Physician"].Rows.Count > 0)
                    {

                        arrRet = new string[5];
                        arrRet[0] = "true";
                        foreach (DataRow dr in (ds.Tables["Physician"].Rows))
                        {
                            arrRet[1] = Convert.ToString(dr["id"]);
                            arrRet[2] = Convert.ToString(dr["email_id"]).Trim();
                            arrRet[3] = Convert.ToString(dr["name"]).Trim();
                            arrRet[4] = Convert.ToString(dr["mobile_no"]).Trim();
                        }

                    }
                    else
                    {
                        arrRet = new string[2];
                        arrRet[0] = "true";
                        arrRet[1] = "00000000-0000-0000-0000-000000000000";
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

        #region FetchUserDetails (AjaxPro Method)
        [AjaxPro.AjaxMethod()]
        public string[] FetchUserDetails(string strLoginID)
        {
            string strReturn = string.Empty; string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            objCore = new Core.Profile.Institution();
            string[] arrRet = new string[0];

            try
            {

                objCore.LOGIN_ID = strLoginID.Trim();

                bReturn = objCore.FetchUserDetails(Server.MapPath("~"), ref ds, ref strCatchMessage);

                if (bReturn)
                {

                    if (ds.Tables["User"].Rows.Count > 0)
                    {

                        arrRet = new string[9];
                        arrRet[0] = "true";
                        foreach (DataRow dr in (ds.Tables["User"].Rows))
                        {
                            arrRet[1] = Convert.ToString(dr["id"]);
                            arrRet[2] = Convert.ToString(dr["login_id"]);
                            if (Convert.ToString(dr["password"]).Trim() != "")
                                arrRet[3] = CoreCommon.DecryptString(Convert.ToString(dr["password"]).Trim());
                            else
                                arrRet[3] = "";
                            arrRet[4] = Convert.ToString(dr["pacs_user_id"]).Trim();

                            if (Convert.ToString(dr["pacs_password"]).Trim() != "")
                                arrRet[5] = CoreCommon.DecryptString(Convert.ToString(dr["pacs_password"]).Trim());
                            else
                                arrRet[5] = "";
                            arrRet[6] = Convert.ToString(dr["email_id"]).Trim();
                            arrRet[7] = Convert.ToString(dr["contact_no"]).Trim();
                            arrRet[8] = Convert.ToString(dr["is_active"]).Trim();
                        }

                    }
                    else
                    {
                        arrRet = new string[2];
                        arrRet[0] = "true";
                        arrRet[1] = "00000000-0000-0000-0000-000000000000";
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
        public string[] SaveRecord(string[] ArrRecord, string[] ArrPhys, string[] ArrUser)
        {
            bool bReturn = false;

            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty;
            string strCatchMessage = string.Empty;
            int intListIndex = 0;

            objCore = new Core.Profile.Institution();
            objComm = new classes.CommonClass();


            Core.Profile.PhysicianList[] objPHYS = new Core.Profile.PhysicianList[0];
            Core.Profile.InstitutionUserList[] objUser = new Core.Profile.InstitutionUserList[0];


            try
            {
                objCore.ID = new Guid(ArrRecord[0].Trim());
               
                objCore.EMAIL_ID = ArrRecord[1].Trim();
                objCore.PHONE = ArrRecord[2].Trim();
                objCore.MOBILE = ArrRecord[3].Trim();
                objCore.CONTACT_PERSON_NAME = ArrRecord[4].Trim();
                objCore.CONTACT_PERSION_MOBILE = ArrRecord[5].Trim();
                objCore.USER_ID = new Guid(ArrRecord[6].Trim());
                objCore.MENU_ID = Convert.ToInt32(ArrRecord[7]);

                objPHYS = new Core.Profile.PhysicianList[(ArrPhys.Length / 6)];

                #region Populate physician details
                for (int i = 0; i < objPHYS.Length; i++)
                {
                    objPHYS[i] = new Core.Profile.PhysicianList();
                    objPHYS[i].ID = new Guid(ArrPhys[intListIndex]);
                    objPHYS[i].FIRST_NAME = ArrPhys[intListIndex + 1].Trim();
                    objPHYS[i].LAST_NAME = ArrPhys[intListIndex + 2].Trim();
                    objPHYS[i].CREDENTIALS = ArrPhys[intListIndex + 3].Trim();
                    objPHYS[i].EMAIL_ID = ArrPhys[intListIndex + 4].Trim();
                    objPHYS[i].MOBILE_NUMBER = ArrPhys[intListIndex + 5].Trim();
                    intListIndex = intListIndex + 6;
                }
                #endregion

                intListIndex = 0;

                objUser = new Core.Profile.InstitutionUserList[(ArrUser.Length / 8)];

                #region Populate User Details
                for (int i = 0; i < objUser.Length; i++)
                {
                    objUser[i] = new Core.Profile.InstitutionUserList();
                    objUser[i].ID = new Guid(ArrUser[intListIndex]);
                    objUser[i].LOGIN_ID = ArrUser[intListIndex + 1].Trim();
                    if (ArrUser[intListIndex + 2].Trim() != string.Empty)
                    {
                        ArrUser[intListIndex + 2] = ArrUser[intListIndex + 2].Trim().ToLower();
                        ArrUser[intListIndex + 2] = CoreCommon.EncryptString(ArrUser[intListIndex + 2]);
                        objUser[i].PASSWORD = ArrUser[intListIndex + 2];
                    }
                    else
                        objUser[i].PASSWORD = "";

                    objUser[i].PACS_USER_ID = ArrUser[intListIndex + 3].Trim();
                    if (ArrUser[intListIndex + 4].Trim() != string.Empty) ArrUser[intListIndex + 4] = CoreCommon.EncryptString(ArrUser[intListIndex + 4]);
                    objUser[i].PACS_PASSWORD = ArrUser[intListIndex + 4].Trim();
                    objUser[i].EMAIL_ID = ArrUser[intListIndex + 5].Trim();
                    objUser[i].CONTACT_NUMBER = ArrUser[intListIndex + 6].Trim();
                    objUser[i].IS_ACTIVE = ArrUser[intListIndex + 7].Trim();
                    intListIndex = intListIndex + 8;
                }
                #endregion

                intListIndex = 0;

                bReturn = objCore.SaveRecord(Server.MapPath("~"), objPHYS, objUser,ref strReturnMsg, ref strCatchMessage);

                if (bReturn)
                {
                    arrRet = new string[4];
                    arrRet[0] = "true";
                    arrRet[1] = strReturnMsg.Trim();
                    arrRet[2] = objCore.ID.ToString();
                    arrRet[3] = objCore.CODE;
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