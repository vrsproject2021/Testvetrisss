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


namespace VETRIS.Registration
{
    [AjaxPro.AjaxNamespace("VRSInstitutionRegistration")]
    public partial class VRSInstitutionRegistration : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.Master.Institution objCore = null;
        classes.CommonClass objComm;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSInstitutionRegistration));
            SetAttributes();
            if ((!CallBackPhys.CausedCallback))
                SetPageValue();
        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {
            //int intUserRoleID = Convert.ToInt32(Request.QueryString["urid"]);
            int intMenuID = 2;
            Guid UserID = new Guid("11111111-1111-1111-1111-111111111111");

            //hdnID.Value = Request.QueryString["id"];
          LoadDetails(intMenuID, UserID);
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {
            
            btnRegister.Attributes.Add("onclick", "javascript:btnRegister_OnClick();");
            ddlCountry.Attributes.Add("onchange", "javascript:ddlCountry_OnChange();");
            btnAddPhys.Attributes.Add("onclick", "javascript:btnAddPhys_OnClick();");

            txtZip.Attributes.Add("onkeypress", "javascript:return CheckInteger(event);");
            txtZip.Attributes.Add("onfocus", "javascript:this.select();");
            txtZip.Attributes.Add("onblur", "javascript:ResetValueInteger(this);");
            txtContMobile.Attributes.Add("onkeypress", "javascript:return CheckInteger(event);");
            txtContMobile.Attributes.Add("onfocus", "javascript:this.select();");
            txtContMobile.Attributes.Add("onblur", "javascript:ResetValueInteger(this);");
            txtTel.Attributes.Add("onkeypress", "javascript:return CheckInteger(event);");
            txtTel.Attributes.Add("onfocus", "javascript:this.select();");
            txtTel.Attributes.Add("onblur", "javascript:ResetValueInteger(this);");
            txtMobile.Attributes.Add("onkeypress", "javascript:return CheckInteger(event);");
            txtMobile.Attributes.Add("onfocus", "javascript:this.select();");
            txtMobile.Attributes.Add("onblur", "javascript:ResetValueInteger(this);");
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

            grdPhys.Width = Unit.Percentage(100);
            grdPhys.RenderControl(e.Output);
            spnErrPhys.RenderControl(e.Output);
        }
        #endregion

        #region LoadPhysicians
        private void LoadPhysicians(string[] arrParams)
        {
            objCore = new Core.Master.Institution();
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

        #region LoadDetails
        private void LoadDetails(int intMenuID, Guid UserID)
        {
            objCore = new Core.Master.Institution();
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

                    PopulateDropdowns(ds);
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

        #region PopulateDropdowns
        private void PopulateDropdowns(DataSet ds)
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

            #region States
            DataRow dr2 = ds.Tables["States"].NewRow();
            dr2["id"] = 0;
            dr2["name"] = "Select One";
            ds.Tables["States"].Rows.InsertAt(dr2, 0);
            ds.Tables["States"].AcceptChanges();

            ddlState.DataSource = ds.Tables["States"];
            ddlState.DataValueField = "id";
            ddlState.DataTextField = "name";
            ddlState.DataBind();
            #endregion

            #region Physicians

            DataRow dr3 = ds.Tables["Physicians"].NewRow();
            dr3["id"] = "00000000-0000-0000-0000-000000000000";
            dr3["name"] = "Select One";
            ds.Tables["Physicians"].Rows.InsertAt(dr3, 0);
            ds.Tables["Physicians"].AcceptChanges();

            foreach (DataRow dr in ds.Tables["Physicians"].Rows)
            {
                if (hdnPhysicians.Value.Trim() != string.Empty) hdnPhysicians.Value += objComm.RecordDivider;
                hdnPhysicians.Value += Convert.ToString(dr["id"]) + objComm.RecordDivider;
                hdnPhysicians.Value += Convert.ToString(dr["name"]).Trim();
            }


            #endregion

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

                objCore.COUNTRY_ID = Convert.ToInt32(arrParams[0].Trim());
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

        #region SaveRecord (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] SaveRecord(string[] ArrRecord, string[] ArrPhys )
        {
            bool bReturn = false;
            string serverPath = ConfigurationManager.AppSettings["ServerPath"]; 

            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty;
            string strCatchMessage = string.Empty;
            int intListIndex = 0;

            objCore = new Core.Master.Institution();
            objComm = new classes.CommonClass();

            Core.Master.DeviceList[] objDevice = new Core.Master.DeviceList[0];
            Core.Master.PhysicianList[] objPHYS = new Core.Master.PhysicianList[0];
            Core.Master.UserList objUser = new Core.Master.UserList();
            Core.Master.FeeList[] objFees = new Core.Master.FeeList[0];

            try
            {
                objCore.ID                      = new Guid(ArrRecord[0].Trim());
                objCore.CODE                    = ArrRecord[1].Trim();
                objCore.NAME                    = ArrRecord[2].Trim();
                objCore.IS_ACTIVE               = ArrRecord[3].Trim();
                objCore.ADDRESS_LINE1           = ArrRecord[4].Trim();
                objCore.ADDRESS_LINE2           = ArrRecord[5].Trim();
                objCore.CITY                    = ArrRecord[6].Trim();
                objCore.COUNTRY_ID              = Convert.ToInt32(ArrRecord[7]);
                objCore.STATE_ID                = Convert.ToInt32(ArrRecord[8]);
                objCore.ZIP                     = ArrRecord[9].Trim();
                objCore.EMAIL_ID                = ArrRecord[10].Trim();
                objCore.PHONE                   = ArrRecord[11].Trim();
                objCore.MOBILE                  = ArrRecord[12].Trim();
                objCore.CONTACT_PERSON_NAME     = ArrRecord[13].Trim();
                objCore.CONTACT_PERSION_MOBILE  = ArrRecord[14].Trim();
                objCore.USER_ID                 = new Guid(ArrRecord[15].Trim());
                objCore.MENU_ID                 = Convert.ToInt32(ArrRecord[16]);
                objCore.USER_UPDATE_URL         = Convert.ToString(ArrRecord[17]);
                objUser.LOGIN_ID                = ArrRecord[18].Trim();
                objUser.PASSWORD                = ArrRecord[19].Trim();
                objUser.EMAIL_ID                = ArrRecord[20].Trim();
                intListIndex = 0;

                objPHYS = new Core.Master.PhysicianList[(ArrPhys.Length / 6)];

                #region Populate physician details
                for (int i = 0; i < objPHYS.Length; i++)
                {
                    objPHYS[i]                  = new Core.Master.PhysicianList();
                    objPHYS[i].ID               = new Guid(ArrPhys[intListIndex]);
                    objPHYS[i].FIRST_NAME       = ArrPhys[intListIndex + 1].Trim();
                    objPHYS[i].LAST_NAME        = ArrPhys[intListIndex + 2].Trim();
                    objPHYS[i].CREDENTIALS      = ArrPhys[intListIndex + 3].Trim();
                    objPHYS[i].EMAIL_ID         = ArrPhys[intListIndex + 4].Trim();
                    objPHYS[i].MOBILE_NUMBER    = ArrPhys[intListIndex + 5].Trim();
                    intListIndex                = intListIndex + 6;
                }
                #endregion

                
                bReturn = objCore.Register(Server.MapPath("~"),serverPath, objPHYS,objUser, ref strReturnMsg, ref strCatchMessage);

                if (bReturn)
                {
                    //string[] ret = CallEradApiUserCreateUpdate(objCore.USER_UPDATE_URL, ArrUser);

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