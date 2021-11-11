using ClearCanvas.Dicom;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Configuration;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VETRIS.Core;
using DICOMLib;

namespace VETRIS.CaseList
{
    [AjaxPro.AjaxNamespace("VRSManualSubmissionUploadFile")]
    public partial class VRSManualSubmissionUploadFile : System.Web.UI.Page
    {
        #region Members & Variables
        classes.CommonClass objComm;
        VETRIS.Core.Case.CaseStudy objCore;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VRSManualSubmissionUploadFile));
            SetAttributes();
            if ((!CallBackSF.CausedCallback))
                SetPageValue();
        }
        #endregion

        #region SetAttributes
        private void SetAttributes()
        {
            int intMenuID = Convert.ToInt32(Request.QueryString["mid"]);
            btnSubmit1.Attributes.Add("onclick", "javascript:btnSubmit_OnClick();");
            btnSubmit2.Attributes.Add("onclick", "javascript:btnSubmit_OnClick();");
            btnClose1.Attributes.Add("onclick", "javascript:btnClose_OnClick();");
            btnClose2.Attributes.Add("onclick", "javascript:btnClose_OnClick();");

        }
        #endregion

        #region SetPageValue
        private void SetPageValue()
        {
            //Guid UserID = new Guid(Request.QueryString["uid"]);

           // Guid InstID = new Guid(Request.QueryString["InstID"]);
            Guid UserID = new Guid(Request.QueryString["uid"]);
            string strTheme = Request.QueryString["th"];
            

           // hdnRegInstitutionId.Value = InstID.ToString();
            objComm = new classes.CommonClass();
            objComm.SetRegionalFormat();
            
            objComm = null;
            hdnFilePath.Value = Server.MapPath("~");
            hdnTempFolder.Value = Server.MapPath("~") + "/CaseList/MSTemp/" + UserID.ToString();

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

        #region Study File Grid

        #region CallBackSF_Callback
        protected void CallBackSF_Callback(object sender, ComponentArt.Web.UI.CallBackEventArgs e)
        {
            string strAction = e.Parameters[0];

            switch (strAction)
            {

                case "A":
                    AddFiles(e.Parameters);
                    break;
                case "D":
                    DeleteFiles(e.Parameters);
                    break;
                case "C":
                    ClearFiles(e.Parameters);
                    break;
            }
            grdSF.Width = Unit.Percentage(100);
            grdSF.RenderControl(e.Output);
            spnERR.RenderControl(e.Output);
        }
        #endregion

        #region AddFiles
        private void AddFiles(string[] arrParams)
        {
            DataTable dtbl = new DataTable();
            objComm = new classes.CommonClass();
            string[] arrSep = { objComm.SecondaryRecordDivider };
            string[] arrRecords = arrParams[1].Split(arrSep, StringSplitOptions.None);
            string[] arrNew = arrParams[2].Split(arrSep, StringSplitOptions.None);
            int intSrl = 0;
            string strFile = string.Empty;
            string strMimeType = string.Empty;
            string strUserID=arrParams[3];

            try
            {
                dtbl = CreateStudyFileTable();
                if (arrRecords.Length > 0)
                {
                    if (arrRecords[0].Trim() != "")
                    {
                        for (int i = 0; i < arrRecords.Length; i = i + 4)
                        {
                            DataRow dr = dtbl.NewRow();
                            intSrl = intSrl + 1;
                            dr["file_srl_no"] = intSrl;
                            dr["file_name"] = strFile =arrRecords[i + 1].Trim().Substring(arrRecords[i + 1].Trim().LastIndexOf("/") + 1, (arrRecords[i + 1].Trim().Length - (arrRecords[i + 1].Trim().LastIndexOf("/") + 1)));
                            dr["file_type"] = arrRecords[i + 2].Trim();
                            if (arrRecords[i + 2].Trim() == "D") dr["file_type_desc"] = "DICOM";
                            else if (arrRecords[i + 2].Trim() == "I")
                            {
                                strMimeType = classes.MIMEAssistant.GetMIMEType(Server.MapPath("~") + "/CaseList/MSTemp/" + strUserID + "/" + strFile);
                                switch (strMimeType)
                                {
                                    case "image/jpeg":
                                        dr["file_type_desc"] = "JPG/JPEG";
                                        break;
                                    case "image/gif":
                                        dr["file_type_desc"] = "GIF";
                                        break;
                                    case "image/png":
                                        dr["file_type_desc"] = "PNG";
                                        break;
                                    case "image/bmp":
                                        dr["file_type_desc"] = "BMP";
                                        break;
                                }
                                
                            }
                            else if (arrRecords[i + 2].Trim() == "P") dr["file_type_desc"] = "PDF";

                            dtbl.Rows.Add(dr);
                        }

                    }
                }

                for (int i = 0; i < arrNew.Length; i = i + 2)
                {
                    DataRow drNew = dtbl.NewRow();
                    intSrl = intSrl + 1;
                    drNew["file_srl_no"] = intSrl;
                    drNew["file_name"] = strFile = arrNew[i].Trim().Substring(arrNew[i].Trim().LastIndexOf("/") + 1, (arrNew[i].Trim().Length - (arrNew[i].Trim().LastIndexOf("/") + 1)));
                    drNew["file_type"] = arrNew[i + 1];
                    if (arrNew[i + 1].Trim() == "D") drNew["file_type_desc"] = "DICOM";
                    else if (arrNew[i + 1].Trim() == "I")
                    {
                        strMimeType = classes.MIMEAssistant.GetMIMEType(Server.MapPath("~") + "/CaseList/MSTemp/" + strUserID + "/" + strFile);
                        switch (strMimeType)
                        {
                            case "image/jpeg":
                                drNew["file_type_desc"] = "JPG/JPEG";
                                break;
                            case "image/gif":
                                drNew["file_type_desc"] = "GIF";
                                break;
                            case "image/png":
                                drNew["file_type_desc"] = "PNG";
                                break;
                            case "image/bmp":
                                drNew["file_type_desc"] = "BMP";
                                break;
                        }

                    }
                    else if (arrNew[i + 1].Trim() == "P") drNew["file_type_desc"] = "PDF";
                    dtbl.Rows.Add(drNew);
                }

                grdSF.DataSource = dtbl;
                grdSF.DataBind();
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

        #region DeleteFiles
        private void DeleteFiles(string[] arrParams)
        {
            DataTable dtbl = new DataTable();
            objComm = new classes.CommonClass();
            int intID = Convert.ToInt32(arrParams[1]);
            string[] arrSep = { objComm.SecondaryRecordDivider };
            string[] arrRecords = arrParams[2].Split(arrSep, StringSplitOptions.None);
            string strFolder = arrParams[3];
            string strFileName = string.Empty;
            int intSrl = 0;
            int intLoop = 0;

            try
            {
                dtbl = CreateStudyFileTable();
                strFolder = strFolder.Replace("\\", "/");

                if (arrRecords[0].Trim() != "")
                {
                    for (int i = 0; i < arrRecords.Length; i = i + 4)
                    {
                        if (Convert.ToInt32(arrRecords[i]) != intID)
                        {
                            DataRow dr = dtbl.NewRow();
                            intSrl = intSrl + 1;
                            dr["file_srl_no"] = intSrl;
                            dr["file_name"] = arrRecords[i + 1].Trim();
                            dr["file_type"] = arrRecords[i + 2].Trim();
                            dr["file_type_desc"] = arrRecords[i + 3].Trim();
                            dtbl.Rows.Add(dr);

                            intLoop = intLoop + 1;
                        }
                        else
                            strFileName = arrRecords[i + 1].Trim();
                    }
                }

                grdSF.DataSource = dtbl;
                grdSF.DataBind();
                spnERR.InnerHtml = "<input type=\"hidden\" id=\"hdnCBErr\" value=\"\" />";


                if (strFileName.Trim() != string.Empty)
                {
                    if (File.Exists(strFolder + "/" + strFileName))
                    {
                        File.Delete(strFolder + "/" + strFileName);
                    }
                }

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

        #region ClearFiles
        private void ClearFiles(string[] arrParams)
        {
            DataTable dtbl = new DataTable();
            objComm = new classes.CommonClass();

            string[] arrSep = { objComm.SecondaryRecordDivider };
            string[] arrRecords = arrParams[0].Split(arrSep, StringSplitOptions.None);
            string strFolder = arrParams[1];
            string strFileName = string.Empty;


            try
            {
                dtbl = CreateStudyFileTable();
                if (arrRecords[0].Trim() != "")
                {
                    strFolder = strFolder.Replace("\\", "/");
                    for (int i = 0; i < arrRecords.Length; i = i + 4)
                    {
                        strFileName = arrRecords[i + 1].Trim();
                        if (strFileName.Trim() != string.Empty)
                        {
                            if (File.Exists(strFolder + "/" + strFileName))
                            {
                                File.Delete(strFolder + "/" + strFileName);
                            }
                        }
                    }
                }

                grdSF.DataSource = dtbl;
                grdSF.DataBind();
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

        #region CreateStudyFileTable
        private DataTable CreateStudyFileTable()
        {
            DataTable dtbl = new DataTable();
            dtbl.Columns.Add("file_srl_no", System.Type.GetType("System.Int32"));
            dtbl.Columns.Add("file_name", System.Type.GetType("System.String"));
            dtbl.Columns.Add("file_type", System.Type.GetType("System.String"));
            dtbl.Columns.Add("file_type_desc", System.Type.GetType("System.String"));
            dtbl.TableName = "files";
            return dtbl;
        }
        #endregion

        #endregion

        #region GetStudyDetails (Ajaxpro Method)
        [AjaxPro.AjaxMethod()]
        public string[] GetStudyDetails(string[] arrFiles,string strUserID)
        {
            bool bReturn = false; bool bFileValid = true;
            string[] arrRet = new string[0];
            string strReturnMsg = string.Empty; string strCatchMessage = string.Empty; string strFile = string.Empty;
            string strFilePath = string.Empty;

            #region study variables
            string[] arr = new string[0];
            string strSUID = string.Empty;
            DateTime dtStudy = DateTime.Now;
            string strPatientID = string.Empty;
            string strPatientName = string.Empty;
            string strPatientFname = string.Empty;
            string strPatientLname = string.Empty;
            string strDt = "0000-00-00";
            string strModalityID = string.Empty;
            string strAccnNo = string.Empty;
            string strRefPhys = string.Empty;
            string strReason = string.Empty;
            DateTime dtDOB = DateTime.Today;
            string strBdt = string.Empty;
            string strPatientSex = string.Empty;
            string strPatientAge = string.Empty;
            int intPriorityID = 0;

            string[] arrDt = new string[0];
            string[] arrTime = new string[0];
            string[] arrDateTime = new string[0];
            #endregion

            objComm = new classes.CommonClass();
            int intListIndex = 0;
            DICOMLib.DicomDecoder dd = new DICOMLib.DicomDecoder();

            try
            {
                objComm.SetRegionalFormat();
                for (int i = 0; i < arrFiles.Length; i=i+4)
                {
                    if (arrFiles[i + 2].Trim() == "D")
                    {
                        strFile = arrFiles[i + 1].Trim();
                        strFilePath = Server.MapPath("~") + "/CaseList/MSTemp/" + strUserID + "/" + strFile;
                        strFilePath = strFilePath.Replace ("ajaxpro\\",string.Empty);

                        dd.DicomFileName = strFilePath;
                        List<string> str = dd.dicomInfo;

                        arr = new string[17];
                        arr = GetallTags(str);
                        strSUID = arr[0].Trim();

                        if (strSUID.Trim() != string.Empty)
                        {
                            #region Get File Data
                            strModalityID = arr[1].Trim();
                            strPatientID = arr[2].Trim();
                            strPatientName = arr[3].Trim();

                            if (strPatientName != string.Empty)
                            {
                                if (strPatientName.Contains(' '))
                                {
                                    strPatientFname = strPatientName.Substring(0, strPatientName.LastIndexOf(' '));
                                    strPatientLname = strPatientName.Substring(strPatientName.LastIndexOf(' '), (strPatientName.Length - strPatientName.LastIndexOf(' ')));
                                }
                                else
                                {
                                    strPatientFname = strPatientName;
                                    strPatientLname = string.Empty;
                                }
                            }

                            strDt = arr[4].Trim();
                            if ((arr[4].Trim().Substring(0,10) == "0000-00-00") || (arr[4].Trim() == string.Empty)) dtStudy = DateTime.Now;
                            else
                            {
                                arrDateTime = arr[4].Trim().Split(' ');
                                arrDt = arrDateTime[0].Split('-');
                                arrTime = arrDateTime[1].Split(':');

                                dtStudy = new DateTime(Convert.ToInt32(arrDt[0]),
                                                        Convert.ToInt32(arrDt[1]),
                                                        Convert.ToInt32(arrDt[2]),
                                                        Convert.ToInt32(arrTime[0]),
                                                        Convert.ToInt32(arrTime[1]),
                                                        Convert.ToInt32(arrTime[2]));

                                if (dtStudy.Year <= 1900)
                                {
                                    dtStudy = DateTime.Now;
                                }
                            }

                                                      

                            strAccnNo = arr[6].Trim();
                            strRefPhys = arr[7].Trim();
                            strReason = arr[12].Trim();

                            strBdt = arr[13].Trim();
                            if ((arr[13].Trim() == "0000-00-00") || (arr[13].Trim() == string.Empty)) dtDOB = dtStudy;
                            else
                            {
                                arrDt = arr[13].Split('-');
                                dtDOB = new DateTime(Convert.ToInt32(arrDt[0]),
                                                     Convert.ToInt32(arrDt[1]),
                                                     Convert.ToInt32(arrDt[2]),
                                                     0, 0, 0);

                                if (dtDOB.Year <= 1900)
                                {
                                    dtDOB = dtStudy;
                                }
                            }

                            strPatientSex = arr[14].Trim();
                            strPatientAge = arr[15].Trim();

                            if (arr[16].Trim() != string.Empty) intPriorityID = Convert.ToInt32(arr[16].Trim());

                            #endregion

                            arrRet = new string[12];
                            arrRet[0] = "true";
                            arrRet[1] = strModalityID.Trim();
                            arrRet[2] = strPatientID.Trim();
                            arrRet[3] = strPatientFname.Trim();
                            arrRet[4] = strPatientLname.Trim();
                            arrRet[5] = objComm.IMDateFormat(dtStudy,objComm.DateFormat) + " " + objComm.padZero(dtStudy.Hour) + ":" + objComm.padZero(dtStudy.Minute);
                            arrRet[6] = strAccnNo.Trim();
                            arrRet[7] = strReason.Trim();
                            arrRet[8] = objComm.IMDateFormat(dtDOB, objComm.DateFormat);
                            arrRet[9] = strPatientSex.Trim();
                            arrRet[10] = strPatientAge.Trim();
                            arrRet[11] = intPriorityID.ToString();
                        }
                        else
                        {
                            arrRet= new string[3];
                            arrRet[0] = "false";
                            arrRet[1] = "391";
                            arrRet[2] = strFile;
                        }

                        break;
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
                objCore = null; objComm = null; dd = null;
                strReturnMsg = null; strCatchMessage = null;
            }
            return arrRet;
        }
        #endregion


        #region DICOM File  Methods

        #region GetallTags
        private string[] GetallTags(List<string> str)
        {

            string strDescription = string.Empty;
            string StudyUID = string.Empty;
            string ModalityID = string.Empty;
            string StrPName = string.Empty;
            string StudyDt = string.Empty;
            string StudyTime = string.Empty;
            string sDt = string.Empty;
            string sTime = string.Empty;
            string studyDtTime = string.Empty;
            string UserSeriesID = string.Empty;
            string SeriesNumber = string.Empty;
            string InstitutionName = string.Empty;
            string PatientID = string.Empty;
            string AccnNo = string.Empty;
            string RefPhys = string.Empty;
            string Manufacturer = string.Empty;
            string StationName = string.Empty;
            string Model = string.Empty;
            string ModalityAETitle = string.Empty;
            string Reason = string.Empty;
            string BirthDt = string.Empty;
            string bDt = string.Empty;
            string PatientSex = string.Empty;
            string PatientAge = string.Empty;
            //string PatientWt = string.Empty;//(0010,1030)
            //string Species = string.Empty;//(0010,2201)
            //string Breed = string.Empty;//(0010,2292)
            //string Owner = string.Empty;//(0010,2297)
            string PriorityID = string.Empty;


            // Add items to the List View Control
            for (int i = 0; i < str.Count; ++i)
            {
                string s1, s4, s5, s11, s12;
                s1 = str[i];

                ExtractStrings(s1, out s4, out s5, out s11, out s12);

                #region commented
                /*if ((s11.ToUpper() == "0008") && (s12.ToUpper() == "103E"))
                {
                    strDescription = s5.Replace("\0", "");
                    strDescription = s5.Replace("<", " ");
                    strDescription = s5.Replace(">", " ");

                }

                else if ((s11.ToUpper() == "0008") && (s12.ToUpper() == "0060"))
                {
                    ModalityID = s5.Replace("\0", "");

                }


                else if ((s11.ToUpper() == "0010") && (s12.ToUpper() == "0010"))
                {
                    Strname = s5.Replace("\0", "");
                    Strname = s5.Replace("^", " ");

                }
                else if ((s11.ToUpper() == "0010") && (s12.ToUpper() == "0030"))
                {
                    DOB = s5.Replace("\0", "");
                    DOB = DOB.Trim();
                    if (DOB != "")
                    {
                        string yy = DOB.Substring(0, 4);
                        string MM = DOB.Substring(4, 2);
                        string DD = DOB.Substring(6, 2);
                        result = yy + "-" + MM + "-" + DD;
                    }
                    else
                    {
                        result = "0000-00-00";
                    }
                }

                else */
                #endregion

                #region Tags
                s5 = s5.Replace("\t", "");
                s5 = s5.Replace("\n", "");

                switch (s11.ToUpper())
                {
                    case "0008":
                        #region s11 =0008
                        switch (s12.ToUpper())
                        {
                            case "0020":
                                StudyDt = s5.Replace("\0", "");
                                StudyDt = StudyDt.Trim();
                                if ((StudyDt.Length == 8))
                                {
                                    string yyyy = StudyDt.Substring(0, 4);
                                    string MM = StudyDt.Substring(4, 2);
                                    string DD = StudyDt.Substring(6, 2);
                                    sDt = yyyy + "-" + MM + "-" + DD;
                                }
                                else
                                {
                                    sDt = "0000-00-00";
                                }
                                break;
                            case "0030":
                                StudyTime = s5.Replace("\0", "");
                                StudyTime = StudyTime.Trim();
                                if ((StudyTime.Length == 6))
                                {
                                    string Hr = StudyTime.Substring(0, 2);
                                    string Min = StudyTime.Substring(2, 2);
                                    string Sec = StudyTime.Substring(4, 2);
                                    sTime = Hr + ":" + Min + ":" + Sec;
                                }
                                else
                                {
                                    sTime = "00:00:00";
                                }
                                break;
                            case "0050":
                                AccnNo = s5.Replace("\0", "");
                                break;
                            case "0060":
                                ModalityID = s5.Replace("\0", "");
                                break;
                            case "0070":
                                Manufacturer = s5.Replace("\0", "");
                                break;
                            case "0080":
                                if (InstitutionName.Trim() == string.Empty)
                                {
                                    InstitutionName = s5.Replace("\0", "");
                                    InstitutionName = s5.Replace("^", " ");
                                }
                                break;
                            case "0090":
                                RefPhys = s5.Replace("\0", "");
                                RefPhys = s5.Replace("^", " ");
                                break;
                            case "1010":
                                StationName = s5.Replace("\0", "");
                                break;
                            case "1090":
                                Model = s5.Replace("\0", "");
                                break;
                            default:
                                break;
                        }
                        #endregion
                        break;
                    case "0010":
                        #region s11 =0010
                        switch (s12.ToUpper())
                        {
                            case "0010":
                                StrPName = s5.Replace("\0", "");
                                StrPName = s5.Replace("^", " ");
                                break;
                            case "0020":
                                PatientID = s5.Replace("\0", "");
                                break;
                            case "0030":
                                BirthDt = s5.Replace("\0", "");
                                BirthDt = BirthDt.Trim();
                                if ((BirthDt.Length == 8))
                                {
                                    string yyyy = BirthDt.Substring(0, 4);
                                    string MM = BirthDt.Substring(4, 2);
                                    string DD = BirthDt.Substring(6, 2);
                                    bDt = yyyy + "-" + MM + "-" + DD;
                                }
                                else
                                {
                                    bDt = "0000-00-00";
                                }
                                break;
                            case "0040":
                                PatientSex = s5.Replace("\0", "");
                                break;
                            case "1010":
                                PatientAge = s5.Replace("\0", "");
                                break;
                        }
                        #endregion
                        break;
                    case "0020":
                        #region s11 =0020
                        switch (s12.ToUpper())
                        {
                            case "000D":
                                if (StudyUID.Trim() == string.Empty)
                                {
                                    StudyUID = s5.Replace("\0", "");
                                }
                                break;
                            case "000E":
                                UserSeriesID = s5.Replace("\0", "");
                                break;
                            case "0011":
                                SeriesNumber = s5.Replace("\0", "");
                                break;
                        }
                        #endregion
                        break;
                    case "0032":
                        #region s11 =0032
                        switch (s12.ToUpper())
                        {
                            case "000C":
                                PriorityID = s5.Replace("\0", "");
                                break;
                        }
                        #endregion
                        break;
                    case "0040":
                        #region s11 =0040
                        switch (s12.ToUpper())
                        {
                            case "0241":
                                ModalityAETitle = s5.Replace("\0", "");
                                break;
                            case "1002":
                                Reason = s5.Replace("\0", "");
                                break;

                        }
                        #endregion
                        break;
                    default:
                        break;
                }
                #endregion
            }

            studyDtTime = sDt + " " + sTime;

            string[] arr = new string[17];
            arr[0] = StudyUID;
            arr[1] = ModalityID;
            arr[2] = PatientID;
            arr[3] = StrPName;
            arr[4] = studyDtTime;
            arr[5] = InstitutionName;
            arr[6] = AccnNo;
            arr[7] = RefPhys;
            arr[8] = Manufacturer;
            arr[9] = StationName;
            arr[10] = Model;
            arr[11] = ModalityAETitle;
            arr[12] = Reason;
            arr[13] = bDt;
            arr[14] = PatientSex;
            arr[15] = PatientAge;
            arr[16] = PriorityID;

            return arr;

        }
        #endregion

        #region ExtractStrings
        void ExtractStrings(string s1, out string s4, out string s5, out string s11, out string s12)
        {
            int ind;
            string s2, s3;
            ind = s1.IndexOf("//");
            s2 = s1.Substring(0, ind);
            s11 = s1.Substring(0, 4);
            s12 = s1.Substring(4, 4);
            s3 = s1.Substring(ind + 2);
            ind = s3.IndexOf(":");
            s4 = s3.Substring(0, ind);
            s5 = s3.Substring(ind + 1);
        }
        #endregion

        #endregion
    }
}