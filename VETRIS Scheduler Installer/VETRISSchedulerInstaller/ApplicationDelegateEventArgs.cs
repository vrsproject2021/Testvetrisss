using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VETRISSchedulerInstaller
{
    public class ApplicationDelegateEventArgs
    {
        private string _mStat = string.Empty;
        private int _mScreen = 0;
        public ApplicationDelegateEventArgs(string _status, int _screen)
        {
            this._mStat = _status;
            this._mScreen = _screen;
        }
        public string Status
        {
            get
            {
                return _mStat;
            }
            set { _mStat = value; }
        }
        public int Screen
        {
            get
            {
                return _mScreen;
            }
            set { _mScreen = value; }
        }
    }
}
