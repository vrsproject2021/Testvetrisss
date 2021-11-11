using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DicomWin
{
    internal class Win32
    {
        public const int HWND_BROADCAST = 0xffff;
        public static readonly int WM_STUDY = RegisterWindowMessage("WM_STUDY");
        [DllImport("user32")]
        public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);
        [DllImport("user32")]
        public static extern int RegisterWindowMessage(string message);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, ref COPYDATASTRUCT lParam);
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        public static extern Int32 FindWindow(String lpClassName, String lpWindowName);
        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpData;
        }
        public const int WM_COPYDATA = 0x4A;
        public static int SendWindowsStringMessage(int hWnd, int wParam, string msg, int tag)
        {
            int result = 0;

            if (hWnd > 0)
            {
                byte[] sarr = System.Text.Encoding.Default.GetBytes(msg);
                int len = sarr.Length;
                COPYDATASTRUCT cds;
                cds.dwData = (IntPtr)tag;
                cds.lpData = msg;
                cds.cbData = len + 1;
                result = SendMessage((IntPtr)hWnd, Win32.WM_COPYDATA, wParam, ref cds);
            }

            return result;
        }
    }
}
