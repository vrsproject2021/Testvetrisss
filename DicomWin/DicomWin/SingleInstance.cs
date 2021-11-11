using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DicomWin
{
    public static class SingleInstance
    {
        
        static Mutex mutex;
        static public bool Start()
        {
            bool onlyInstance = false;
            string strLoc = Assembly.GetExecutingAssembly().Location;
            FileSystemInfo fileInfo = new FileInfo(strLoc);
            string sExeName = fileInfo.Name;
            string mutexName = String.Format("Global\\{0}", sExeName);

            

            mutex = new Mutex(true, mutexName, out onlyInstance);
            return onlyInstance;
        }
        static public void SendArguments(string arg)
        {
            //Win32.PostMessage(
            //    (IntPtr)WinApi.HWND_BROADCAST,
            //    WM_SHOWFIRSTINSTANCE,
            //    IntPtr.Zero,
            //    IntPtr.Zero);
            var hwnd=Win32.FindWindow(null,"eRAD PACS Viewer Mediator");
            Win32.SendWindowsStringMessage(hwnd, 0, arg, 2); 
        }
        static public void Stop()
        {
            mutex.ReleaseMutex();
        }
    }
}
