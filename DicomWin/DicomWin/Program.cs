using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DicomWin
{
    static class Program
    {

        [STAThread]
        static void Main()
        {
            var args = Environment.GetCommandLineArgs();
            
            //if (!SingleInstance.Start())
            //{
               
            //    if (args.Length == 2)
            //    {
                    
            //        SingleInstance.SendArguments(args[1]);
            //    }
                    
            //    return;
            //}
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {

                Form1 mainForm = new Form1();
                Application.Run(mainForm);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            //SingleInstance.Stop();


        }

    }
}
