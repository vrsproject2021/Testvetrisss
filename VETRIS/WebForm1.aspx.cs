using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Xml.Serialization;
using System.IO;
using eRADCls;

namespace VETRIS
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GetData();
        }

        private void GetData()
        {
            string sSession = string.Empty;
            string sCatchMsg = string.Empty;
            string sError = string.Empty;
            string sXMLOutput = string.Empty;
            string sURL = "http://dev.vcradiology.com/epws/API";
            DataSet ds = new DataSet();
            string[] arrFields = new string[0];
            DateTime DtFrom = DateTime.Now.AddDays(-14); DateTime DtTill = DateTime.Now;
            

            bool bRet = false;

            RadWebClass objRWC = new RadWebClass();
            try
            {
                //bRet = objRWC.GetSession("192.168.65.83", sURL,"admin", ref sSession, ref sCatchMsg, ref sError);
                if (bRet)
                {
                    arrFields = new string[5];
                    arrFields[0] = "SYUI";
                    arrFields[1] = "RCVD";
                    arrFields[2] = "PANM";
                    arrFields[3] = "INSN";
                    arrFields[4] = "STAT";
                    bRet = objRWC.GetStudyDateWise(sSession, sURL,arrFields,DtFrom,DtTill, ref sXMLOutput, ref sCatchMsg, ref sError);
                    if (bRet)
                    {
                        //if(!Directory.Exists(Server.MapPath("~") + "/Temp")) Directory.CreateDirectory(Server.MapPath("~") + "/Temp");
                        //if (File.Exists(Server.MapPath("~") + "/Temp/Temp.xml")) File.Delete(Server.MapPath("~") + "/Temp/Temp.xml");
                        //FileStream stream = new FileStream(Server.MapPath("~") + "/Temp/Temp.xml", FileMode.CreateNew);

                        System.IO.StringReader xmlSR = new System.IO.StringReader(sXMLOutput);

                        ds.ReadXml(xmlSR);
                        //TextBox1.Text = sXMLOutput;

                    }
                    //else
                    //    <Show Error>;
                }
            }
            catch (Exception ex)
            {
                TextBox1.Text = ex.Message;
            }
            finally { ds.Dispose(); }
        }

    }
}