using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using VETRIS.Core;

namespace VETRIS.Registration
{
    [AjaxPro.AjaxNamespace("VRSRegistration")]
    public partial class VRSRegistration : System.Web.UI.Page
    {
        #region Members & Variables
        VETRIS.Core.Master.Institution objInstitution = null;
        VETRIS.Core.Registration.Registration objCore = null;
        VETRIS.Core.Master.Modality objModality = null;
        classes.CommonClass objComm;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSRegistration));
            SetAttributes();
            SetPageValue();
        }

        #region SetAttributes
        private void SetAttributes()
        {
            ddlCountry.Attributes.Add("onchange", "javascript:ddlCountry_OnChange()");
            //btnRegistration.Attributes.Add("onclick", "javascript:btnRegistration_OnClick();");
        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {
            hdnRootDir.Value = ConfigurationManager.AppSettings["RootDirectory"];

            FetchSearchParameters();
            FetchModalities();
        }
        #endregion

        #region FetchSearchParameters
        private void FetchSearchParameters()
        {
            objInstitution = new Core.Master.Institution();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();

            try
            {

                bReturn = objInstitution.FetchBrowserParameters(Server.MapPath("~"), ref ds, ref strCatchMessage);

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
                    ddlCountry.SelectedValue = "231";
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

        private void FetchModalities()
        {
            objModality = new Core.Master.Modality();
            string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();
            string returnMsg = string.Empty;
            try
            {

                bReturn = objModality.SearchRecords(Server.MapPath("~"), ref ds, ref returnMsg, ref strCatchMessage);

                if (bReturn)
                {
                    #region Modality

                    for (int i = 0; i < ds.Tables["BrowserList"].Rows.Count; i++)
                    {
                        chkModality.Items.Add(new ListItem(ds.Tables["BrowserList"].Rows[i]["name"].ToString(), ds.Tables["BrowserList"].Rows[i]["id"].ToString()));
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

        #region FetchStates (AjaxPro Method)
        [AjaxPro.AjaxMethod()]
        public string[] FetchStates(string countryId)
        {
            string strReturn = string.Empty; string strCatchMessage = ""; bool bReturn = false;
            DataSet ds = new DataSet();
            objInstitution = new Core.Master.Institution();

            int i = 0;
            string[] arrRet = new string[0];

            try
            {

                objInstitution.COUNTRY_ID = Convert.ToInt32(countryId);
                bReturn = objInstitution.FetchCountryWiseStates(Server.MapPath("~"), ref ds, ref strCatchMessage);

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
        public string[] SaveRecord(string[] ArrRecord, string[] ArrPhys, string[] ArrModalities)
        {
            bool bReturn = false;

            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty;
            string strCatchMessage = string.Empty;
            int intListIndex = 0;

            objCore = new Core.Registration.Registration();
            Core.Registration.PhysicianList[] objPHYS = new Core.Registration.PhysicianList[0];
            List<Core.Registration.InstitutionRegModalityLink> objModality = new List<Core.Registration.InstitutionRegModalityLink>();
            objComm = new classes.CommonClass();

            try
            {
                objCore.ID = new Guid(ArrRecord[0].Trim());
                objCore.NAME = ArrRecord[1].Trim();
                objCore.ADDRESS_LINE1 = ArrRecord[2].Trim();
                objCore.CITY = ArrRecord[3].Trim();
                objCore.ZIP = ArrRecord[4].Trim();
                //objCore.PREFERRED_PMT_METHOD = ArrRecord[5].Trim();
                objCore.Contact_Person_Name = ArrRecord[5].Trim();
                objCore.Contact_Person_Mobile = ArrRecord[6];
                objCore.COUNTRY_ID = Convert.ToInt32(ArrRecord[7]);
                objCore.STATE_ID = Convert.ToInt32(ArrRecord[8]);
                objCore.EMAIL_ID = ArrRecord[9].Trim();
                objCore.PHONE = ArrRecord[10].Trim();
                objCore.LOGIN_ID = ArrRecord[11].Trim();
                objCore.LOGIN_PASSWORD = ArrRecord[12].Trim();
                objCore.LOGIN_EMAIL_ID = ArrRecord[9].Trim();//objCore.LOGIN_EMAIL_ID = ArrRecord[14].Trim();
                objCore.LOGIN_MOBILE_NO = string.Empty;//objCore.LOGIN_MOBILE_NO = ArrRecord[15].Trim();
                objCore.IS_Modality_Selected = ArrRecord[13].Trim() == "Y" ? true : false;
                objCore.STATE_NAME = ArrRecord[14].Trim();
                objCore.COUNTRY_NAME = ArrRecord[15].Trim();
                objCore.SUBMITTED_BY = ArrRecord[16].Trim();
                objCore.IMAGE_SOFTWARE_NAME = ArrRecord[17].Trim();

                if (ArrRecord[12].Trim() != string.Empty)
                    objCore.LOGIN_PASSWORD = CoreCommon.EncryptString(ArrRecord[12].Trim());
                else
                    objCore.LOGIN_PASSWORD = string.Empty;

                objPHYS = new Core.Registration.PhysicianList[(ArrPhys.Length / 6)];

                intListIndex = 0;
                #region Populate physician details
                for (int i = 0; i < objPHYS.Length; i++)
                {
                    objPHYS[i] = new Core.Registration.PhysicianList();
                    objPHYS[i].ID = new Guid(ArrPhys[intListIndex]);
                    objPHYS[i].FIRST_NAME = ArrPhys[intListIndex + 1].Trim();
                    objPHYS[i].LAST_NAME = ArrPhys[intListIndex + 2].Trim();
                    objPHYS[i].CREDENTIALS = ArrPhys[intListIndex + 3].Trim();
                    objPHYS[i].EMAIL_ID = ArrPhys[intListIndex + 4].Trim();
                    objPHYS[i].MOBILE_NUMBER = ArrPhys[intListIndex + 5].Trim();
                    intListIndex = intListIndex + 6;
                }
                #endregion

                #region Populate Modality Link

                for (int i = 0; i < ArrModalities.Length; i++)
                {
                    var modality = new Core.Registration.InstitutionRegModalityLink();
                    modality.modality_id = Convert.ToInt32(ArrModalities[i]);
                    objModality.Add(modality);
                }

                #endregion
                //objCore.DEFAULT_FEE = Convert.ToDecimal(ArrRecord[16]);
                //objCore.NOTIFICATION_PREFERENCE = ArrRecord[17].Trim();
                //objCore.USER_ID = new Guid(ArrRecord[18].Trim());
                //objCore.MENU_ID = Convert.ToInt32(ArrRecord[19]);

                bReturn = objCore.SaveRecord(Server.MapPath("~"), objPHYS, objModality.ToArray(), ref strReturnMsg, ref strCatchMessage);

                if (bReturn)
                {
                    arrRet = new string[3];
                    arrRet[0] = "true";
                    arrRet[1] = strReturnMsg.Trim();
                    arrRet[2] = objCore.ID.ToString();
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