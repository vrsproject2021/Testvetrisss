using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DICOMRouterInstaller
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            frmMain frm = new frmMain();
            if (args.Length==0) frm.UpdateOnly = "N";
            else frm.UpdateOnly = "Y";

            Application.Run(frm);
        }
       
    }
}
