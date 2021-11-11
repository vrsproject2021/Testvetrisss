using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Text;
using System.Data;
using System.IO;
using VETRIS.Core;
using AjaxPro;

namespace VETRIS.CaseList
{
    [AjaxPro.AjaxNamespace("VRSMail")]
    public partial class VRSMail : System.Web.UI.Page
    {
        #region Members & Variables
        classes.CommonClass objComm;
        VETRIS.Core.Case.CaseStudy objCore;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSMail));
            SetAttributes();
            SetPageValue();
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {
            btnClose1.Attributes.Add("onclick", "javascript: parent.HideDataList();");
            btnClose2.Attributes.Add("onclick", "javascript: parent.HideDataList();");
        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {
            hdnID.Value = Request.QueryString["id"];
            Guid UserID = new Guid(Request.QueryString["uid"]);
            hdnFilePath.Value = Server.MapPath("~");
            LoadParameters();
        }
        #endregion

        #region LoadParameters
        private void LoadParameters()
        {
            objCore = new Core.Case.CaseStudy();
            string strCatchMessage = ""; string strReturnMessage = string.Empty; bool bReturn = false;
            DataSet ds = new DataSet();
            objComm = new classes.CommonClass();

            try
            {
                objCore.ID = new Guid(hdnID.Value);

                bReturn = objCore.LoadHeader(Server.MapPath("~"), ref ds, ref strReturnMessage, ref strCatchMessage);

                if (bReturn)
                {
                    lblInstitution.Text = objCore.INSTITUTION_NAME; txtInstMail.Text = objCore.INSTITUTION_EMAIL_ID;
                    lblPhysician.Text = objCore.PHYSICIAN_NAME; txtPhysMail.Text = objCore.PHYSICIAN_EMAIL_ID;
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
                ds.Dispose(); objComm = null; objCore = null;
            }
        }
        #endregion
    }
}