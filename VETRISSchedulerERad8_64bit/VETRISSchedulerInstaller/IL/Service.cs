using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;
using System.Management;

namespace VETRISSchedulerInstaller.IL
{
    public class Service
    {
        #region Members & Variables
        private string strServiceName = string.Empty;
        private string strServiceDisplayName = string.Empty;
        private string strError = string.Empty;
        private string strServiceExePath = string.Empty;
        #endregion

        #region Properties
        public string SERVICE_NAME
        {
            get { return strServiceName; }
            set { strServiceName = value; }
        }
        public string SERVICE_DISPLAY_NAME
        {
            get { return strServiceDisplayName; }
            set { strServiceDisplayName = value; }
        }
        public string SERVICE_EXECUTABLE_PATH
        {
            get { return strServiceExePath; }
            set { strServiceExePath = value; }
        }
        public string ERROR
        {
            get { return strError; }
            set { strError = value; }
        }

        #endregion

        #region Start
        public bool Start()
        {
            ServiceController service = new ServiceController(strServiceName);
            if (service.Status == ServiceControllerStatus.Stopped)
            {
                try
                {
                    TimeSpan timeout = TimeSpan.FromMilliseconds(1500);

                    service.Start();
                    service.WaitForStatus(ServiceControllerStatus.Running, timeout);

                    service.Dispose();
                    service = null;
                    return true;
                }
                catch (Exception ex)
                {
                    strError = ex.Message;
                    return false;
                }
            }
            else
            {
                strError = string.Format("Service {0} is already started. Could not start service.", strServiceName);
                return false;
            }
        }
        #endregion

        #region Stop
        public bool Stop()
        {
            ServiceController service = new ServiceController(strServiceName);
            if (service.Status != ServiceControllerStatus.Stopped)
            {
                try
                {
                    TimeSpan timeout = TimeSpan.FromMilliseconds(1500);

                    service.Stop();
                    service.WaitForStatus(ServiceControllerStatus.Running, timeout);

                    return true;
                }
                catch (Exception ex)
                {
                    strError = ex.Message;
                    return true;
                }
                finally
                {
                    service.Dispose();
                    service = null;
                }
            }
            else
            {
                strError = string.Format("Service {0} is already stopped. Could not stop service.", strServiceName);
                return false;
            }

        }
        #endregion

        #region CheckStatus
        public string CheckStatus()
        {
            ServiceController service = null;
            try
            {
                service = new ServiceController(strServiceName);
                strServiceDisplayName = service.DisplayName;
                strError = "(" + service.Status.ToString() + "...)";

                using (ManagementObject wmiService = new ManagementObject("Win32_Service.Name='" + strServiceName + "'"))
                {
                    wmiService.Get();
                    strServiceExePath = wmiService["PathName"].ToString();
                }
            }
            catch (Exception ex)
            {
                strError = "Not Installed";
            }
            finally
            {
                service.Dispose();
                service = null;
            }
            return strError;
        }
        #endregion
    }
}
