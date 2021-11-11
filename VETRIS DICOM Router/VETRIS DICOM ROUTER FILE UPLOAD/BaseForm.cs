using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VETRIS_DICOM_ROUTER_FILE_UPLOAD
{
    public class BaseForm:Form
    {
        public delegate void LoadCompletedEventHandler();
        public event LoadCompletedEventHandler LoadCompleted;

        public BaseForm()
        {
            this.Shown += new EventHandler(BaseForm_Shown);
        }

        void BaseForm_Shown(object sender, EventArgs e)
        {
            Application.DoEvents();
            if (LoadCompleted != null)
                LoadCompleted();
        }
    }
}
