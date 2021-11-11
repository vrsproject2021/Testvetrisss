using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VETRISAccountsScheduler
{
    static class Program
    {
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static Mutex mutex = null;

        [STAThread]
       
        static void Main()
        {
            const string appName = "VETRISAccountsScheduler";
            bool createdNew;
            mutex = new Mutex(true, appName, out createdNew);  
            if (!createdNew)
            {
                MessageBox.Show("An instance of this application is already running...", "VETIS Account Ssheduler : Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //app is already running! Exiting the application  
                return;
            }  

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            Application.Run(new frmMain());
            //Application.Run(new frmTestcs());
        }
    }
}
