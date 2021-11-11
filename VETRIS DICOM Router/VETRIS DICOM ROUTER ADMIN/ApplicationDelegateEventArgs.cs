using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VETRIS_DICOM_ROUTER_ADMIN
{
    public class ApplicationDelegateEventArgs
    {
        private string _mStat = string.Empty;
        public ApplicationDelegateEventArgs(string _status)
        {
            this._mStat = _status;
        }
        public string Status
        {
            get
            {
                return _mStat;
            }
            set { _mStat = value; }
        }
    }
}
