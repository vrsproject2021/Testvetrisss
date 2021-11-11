using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.Collections;
using System.Threading;
using System.Globalization;
using System.Resources;

namespace VETRIS.classes
{
    public class CommonClass
    {
        #region CommonClass
        public CommonClass()
        {


        }
        #endregion

        #region Members And Variables
        private string[] arrayMonths;
        private string[] arrayDays;
        private string strDateSeparator;
        private string strDateFormat;
        private string strCurrency;
        private string strCurrTimeZone;
        private int intDecDigit;
        public static char GcDivider = '±';
        public static string GsSecondoryDivider = "»";
        #endregion

        #region Property
        public string[] Months
        {
            get { return arrayMonths; }
        }
        public string CurrentTimeZone
        {
            get { return strCurrTimeZone; }
        }
        public string[] Days
        {
            get { return arrayDays; }
        }
        public string DateSeparator
        {
            get { return strDateSeparator; }
        }
        

        public string DateFormat
        {
            get { return strDateFormat; }
        }
        public string CurrencySymbol
        {
            get { return strCurrency; }
        }
        public int DecimalDigit
        {
            get { return intDecDigit; }
        }

        public char RecordDivider
        {
            get { return GcDivider; }
        }
        public string SecondaryRecordDivider
        {
            get { return GsSecondoryDivider; }
        }
        #endregion

        #region SetRegionalFormat
        public void SetRegionalFormat()
        {
            Thread.CurrentThread.CurrentCulture.ClearCachedData();
            CultureInfo ci = new CultureInfo(System.Configuration.ConfigurationManager.AppSettings["Lang"]);

            try
            {
                Thread.CurrentThread.CurrentCulture = ci;
                arrayMonths = Thread.CurrentThread.CurrentCulture.DateTimeFormat.AbbreviatedMonthNames;
                arrayDays = Thread.CurrentThread.CurrentCulture.DateTimeFormat.AbbreviatedDayNames;
                intDecDigit = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits;
                strDateFormat = Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern;
                strDateSeparator = Thread.CurrentThread.CurrentCulture.DateTimeFormat.DateSeparator;
                strCurrency = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencySymbol;
                strCurrTimeZone = TimeZone.CurrentTimeZone.StandardName;
                //TimeZone.CurrentTimeZone.ToLocalTime(DateTime.Now);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region GetLastDayOfMonth
        public int GetLastDayOfMonth(int month,int year)
        {
            int intLastDay = 0;
            DateTime dtValue;

            try
            {
                dtValue = new DateTime(year, month, 1);
                dtValue = dtValue.AddMonths(1);
                dtValue = dtValue.AddDays(-1);
                intLastDay = dtValue.Day;
                return intLastDay;
            }
            catch
            {
                return 1;
            }
        }
        #endregion

        #region IMString
        public string IMString(object LobjValue)
        {
            string LsReturnValue = null;
            try
            {
                if (LobjValue != DBNull.Value || LobjValue != null)
                    LsReturnValue = Convert.ToString(LobjValue).Trim();
                else
                    LsReturnValue = "";
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return LsReturnValue;
        }
        #endregion

        #region IMDBString
        public string IMDBString(object LobjValue)
        {
            string LsReturnValue = null;
            LsReturnValue = this.IMString(LobjValue);
            LsReturnValue = LsReturnValue.Replace("'", "''");

            return LsReturnValue;
        }
        #endregion

        #region IMNumeric
        public string IMNumeric(object LobjValue, int LiDecimalDigit)
        {
            string LsReturnValue = "";
            try
            {
                if (LobjValue != DBNull.Value || LobjValue != null)
                {
                    if (LobjValue.ToString().Length == 0)
                    {
                        LsReturnValue = "0";
                    }
                    else
                    {
                        if (LiDecimalDigit == 0)//return integer value
                        {
                            //LsReturnValue = Convert.ToDouble(LobjValue).ToString();
                            //Added by Pavel on 09/10/2009
                            if (Convert.ToString(LobjValue).Contains("."))
                                LsReturnValue = Convert.ToDecimal(LobjValue).ToString("N" + LiDecimalDigit);
                            else
                                LsReturnValue = Convert.ToDouble(LobjValue).ToString();
                        }
                        else//return as per culture
                        {
                            LsReturnValue = Convert.ToDecimal(LobjValue).ToString("N" + LiDecimalDigit);
                        }
                    }
                }
                else
                {
                    LsReturnValue = "";
                }
            }
            catch
            {
                LsReturnValue = "";
            }
            return LsReturnValue;
        }
        #endregion

        #region IMDateFormat
        public string IMDateFormat(object LobjValue, string strDateFormat)
        {
            string LsTemp;
            int LiDate, LiMonth, LiYear;
            DateTime LdtValue;

            try
            {
                LsTemp = "";
                strDateFormat = strDateFormat.ToLower().ToString();

                if (LobjValue != DBNull.Value || LobjValue != null || Convert.ToString(LobjValue) != "")
                {
                    string[] LarrayMonthName = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
                    //{ "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                    LdtValue = Convert.ToDateTime(LobjValue);
                    LiDate = LdtValue.Day;
                    LiMonth = LdtValue.Month;
                    LiYear = LdtValue.Year;

                    LsTemp = strDateFormat;
                    LsTemp = LsTemp.Replace("dd", "<e>"); ;
                    LsTemp = LsTemp.Replace("d", "<d>");
                    LsTemp = LsTemp.Replace("<e>", padZero(LiDate));
                    LsTemp = LsTemp.Replace("<d>", Convert.ToString(LiDate));
                    LsTemp = LsTemp.Replace("mmm", "<o>");
                    LsTemp = LsTemp.Replace("mm", "<n>");
                    LsTemp = LsTemp.Replace("m", "<m>");
                    LsTemp = LsTemp.Replace("<m>", Convert.ToString(LiMonth));
                    LsTemp = LsTemp.Replace("<n>", padZero(LiMonth));
                    LsTemp = LsTemp.Replace("<o>", LarrayMonthName[LiMonth - 1]);
                    LsTemp = LsTemp.Replace("yyyy", Convert.ToString(LiYear));
                }
                return LsTemp;
            }
            catch
            {
                return "";
            }
        }
        #endregion

        #region padZero
        public string padZero(int LiNum)
        {
            return (LiNum < 10) ? '0' + Convert.ToString(LiNum) : Convert.ToString(LiNum);
        }
        #endregion

        #region getShortMonth
        public string getShortMonth(int LiNum)
        {
            string[] LarrayMonthName = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            return LarrayMonthName[LiNum - 1];
        }
        #endregion

        #region IMDBDateFormat
        public string IMDBDateFormat(object LobjDateTimeValue)
        {
            string LsReturnValue = "";
            try
            {
                DateTime dtDate = Convert.ToDateTime(LobjDateTimeValue);
                string[] LarrayMonthName = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

                LsReturnValue = dtDate.Day.ToString() + LarrayMonthName[dtDate.Month - 1] + dtDate.Year.ToString();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return LsReturnValue;
        }
        #endregion

        #region SanitizeInput
        public string SanitizeInput(string LsInput)
        {
            if (LsInput.Length > 0)
            {
                LsInput = LsInput.Replace("'", "''");
                LsInput = LsInput.Replace("\"", "\"\"");
                LsInput = LsInput.Replace("%", "");
            }
            return LsInput;
        }
        #endregion

        #region ToInitcap
        public string ToInitcap(string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;
            char[] charArray = new char[str.Length];
            bool newWord = true;
            for (int i = 0; i < str.Length; ++i)
            {
                Char currentChar = str[i];
                if (Char.IsLetter(currentChar))
                {
                    if (newWord)
                    {
                        newWord = false;
                        currentChar = Char.ToUpper(currentChar);
                    }
                    else
                    {
                        currentChar = Char.ToLower(currentChar);
                    }
                }
                else if (Char.IsWhiteSpace(currentChar))
                {
                    newWord = true;
                }
                charArray[i] = currentChar;
            }
            return new string(charArray);
        }
        #endregion

        #region SetErrorResources
        public string SetErrorResources(string[] arrayCode, string strConfirmation, bool IsError, string LsText1, string LsText2)
        {
            string strResult = "";
            string strResDir;
            string strTemp = "";
            string strImg = "";
            string RootDir = ConfigurationManager.AppSettings["ServerPath"];

            ResourceManager RM = new ResourceManager("VETRIS.Common.Resources." + System.Configuration.ConfigurationManager.AppSettings["MessageResourceFileName"], this.GetType().Assembly);

            if (strConfirmation == "N")
            {
                //if (IsError)
                //    strImg = "<img src=\"/" + RootDir + "/images/error.png\" border=\"0\" align=\"absmiddle\"/>&nbsp;";
                //else
                //    strImg = "<img src=\"/" + RootDir + "/images/info.png\" border=\"0\" align=\"absmiddle\"/>&nbsp;";

                if (IsError)
                    strImg = "<img src=\"" + RootDir + "/images/error.png\" border=\"0\" align=\"absmiddle\"/>&nbsp;";
                else
                    strImg = "<img src=\"" + RootDir + "/images/info.png\" border=\"0\" align=\"absmiddle\"/>&nbsp;";
            }
            else
            {
                strImg = "<img src=\"" + RootDir + "/images/confirm.png\" border=\"0\" align=\"absmiddle\"/>&nbsp;";
            }

            try
            {
                strResDir = HttpContext.Current.Server.MapPath("Resources");
                if (strResDir.IndexOf("ajaxpro") > 0)
                {
                    strResDir = strResDir.Replace("ajaxpro", "Common");
                }
                else if (strResDir.IndexOf("\\" + RootDir + "\\") > 0)
                {
                    strResDir = strResDir.Substring(0, strResDir.IndexOf("\\" + RootDir + "\\")) + "\\" + RootDir + "\\Common\\Resources";
                }
                else if (strResDir.IndexOf("\\Common\\") < 0)
                {
                    strResDir = strResDir.Replace("\\Resources", "\\Common\\Resources");
                }

                //LobjResourceFile.ClassKey = System.Configuration.ConfigurationManager.AppSettings["MessageResourceFileName"];
                //LobjResourceFile.ResourceDir = strResDir;
                //LobjResourceFile.Culture = System.Configuration.ConfigurationManager.AppSettings["Lang"];

                for (int LiCounter = 0; LiCounter <= arrayCode.GetUpperBound(0); LiCounter++)
                {
                    if (LiCounter > 0) { strResult += "<br/>"; }
                    if (arrayCode[LiCounter].ToString().Trim() != "")
                    {   

                        // strTemp = LobjResourceFile.ControlText(arrayCode[LiCounter].ToString().Trim());

                        strTemp = RM.GetString(arrayCode[LiCounter].ToString().Trim());
                        if (strTemp == null) strTemp = arrayCode[LiCounter].ToString().Trim();
                        if (strTemp.Contains("t1#")) strTemp = strTemp.Replace("t1#", LsText1);
                        if (strTemp.Contains("t2#")) strTemp = strTemp.Replace("t2#", LsText2);
                        strTemp = strTemp.Replace("\n", "<br/>");
                        strTemp = strTemp.Replace("\\n", "<br/>");
                        
                        if (strTemp.Length > 0)
                        {
                            strResult += strImg + strTemp;
                        }
                        else
                        {
                            strResult += strImg + arrayCode[LiCounter].ToString();
                        }
                    }
                }
            }
            catch (Exception LexErr) { throw LexErr; }
            finally
            {
                RM = null;
            }
            return strResult;
        }
        #endregion

      
    }
}