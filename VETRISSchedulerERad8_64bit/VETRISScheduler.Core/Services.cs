using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;

namespace VETRISScheduler.Core
{
    public class Services
    {
        #region Members & Variables
        private string strServiceName = string.Empty;
        private string strError = string.Empty;
        #endregion

        #region Properties
        public string SERVICE_NAME
        {
            get { return strServiceName; }
            set { strServiceName = value; }
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
                    TimeSpan timeout = TimeSpan.FromMilliseconds(100000);

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
                string s = service.DisplayName;
                strError = "(" + service.Status.ToString() + "...)";

            }
            catch
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
