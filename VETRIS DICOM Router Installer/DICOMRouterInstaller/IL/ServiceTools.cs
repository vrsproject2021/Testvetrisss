using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;
using IWshRuntimeLibrary;

namespace DICOMRouterInstaller.IL.ServiceTools
{
    //class ServiceTools
    //{

    //}
    #region enum ServiceManagerRights
    public enum ServiceManagerRights
    {
        /// <summary>
        /// 
        /// </summary>
        Connect = 0x0001,
        /// <summary>
        /// 
        /// </summary>
        CreateService = 0x0002,
        /// <summary>
        /// 
        /// </summary>
        EnumerateService = 0x0004,
        /// <summary>
        /// 
        /// </summary>
        Lock = 0x0008,
        /// <summary>
        /// 
        /// </summary>
        QueryLockStatus = 0x0010,
        /// <summary>
        /// 
        /// </summary>
        ModifyBootConfig = 0x0020,
        /// <summary>
        /// 
        /// </summary>
        StandardRightsRequired = 0xF0000,
        /// <summary>
        /// 
        /// </summary>
        AllAccess = (StandardRightsRequired | Connect | CreateService |
        EnumerateService | Lock | QueryLockStatus | ModifyBootConfig)
    }
    #endregion

    #region enum ServiceRights
    public enum ServiceRights
    {
        /// <summary>
        /// 
        /// </summary>
        QueryConfig = 0x1,
        /// <summary>
        /// 
        /// </summary>
        ChangeConfig = 0x2,
        /// <summary>
        /// 
        /// </summary>
        QueryStatus = 0x4,
        /// <summary>
        /// 
        /// </summary>
        EnumerateDependants = 0x8,
        /// <summary>
        /// 
        /// </summary>
        Start = 0x10,
        /// <summary>
        /// 
        /// </summary>
        Stop = 0x20,
        /// <summary>
        /// 
        /// </summary>
        PauseContinue = 0x40,
        /// <summary>
        /// 
        /// </summary>
        Interrogate = 0x80,
        /// <summary>
        /// 
        /// </summary>
        UserDefinedControl = 0x100,
        /// <summary>
        /// 
        /// </summary>
        Delete = 0x00010000,
        /// <summary>
        /// 
        /// </summary>
        StandardRightsRequired = 0xF0000,
        /// <summary>
        /// 
        /// </summary>
        AllAccess = (StandardRightsRequired | QueryConfig | ChangeConfig |
        QueryStatus | EnumerateDependants | Start | Stop | PauseContinue |
        Interrogate | UserDefinedControl)
    }
    #endregion

    #region enum ServiceBootFlag
    public enum ServiceBootFlag
    {
        /// <summary>
        /// 
        /// </summary>
        Start = 0x00000000,
        /// <summary>
        /// 
        /// </summary>
        SystemStart = 0x00000001,
        /// <summary>
        /// 
        /// </summary>
        AutoStart = 0x00000002,
        /// <summary>
        /// 
        /// </summary>
        DemandStart = 0x00000003,
        /// <summary>
        /// 
        /// </summary>
        Disabled = 0x00000004
    }
    #endregion

    #region enum ServiceState
    public enum ServiceState
    {
        /// <summary>
        /// 
        /// </summary>
        Unknown = -1, // The state cannot be (has not been) retrieved.
        /// <summary>
        /// 
        /// </summary>
        NotFound = 0, // The service is not known on the host server.
        /// <summary>
        /// 
        /// </summary>
        Stop = 1, // The service is NET stopped.
        /// <summary>
        /// 
        /// </summary>
        Run = 2, // The service is NET started.
        /// <summary>
        /// 
        /// </summary>
        Stopping = 3,
        /// <summary>
        /// 
        /// </summary>
        Starting = 4,
    }
    #endregion

    #region enum ServiceControl
    public enum ServiceControl
    {
        /// <summary>
        /// 
        /// </summary>
        Stop = 0x00000001,
        /// <summary>
        /// 
        /// </summary>
        Pause = 0x00000002,
        /// <summary>
        /// 
        /// </summary>
        Continue = 0x00000003,
        /// <summary>
        /// 
        /// </summary>
        Interrogate = 0x00000004,
        /// <summary>
        /// 
        /// </summary>
        Shutdown = 0x00000005,
        /// <summary>
        /// 
        /// </summary>
        ParamChange = 0x00000006,
        /// <summary>
        /// 
        /// </summary>
        NetBindAdd = 0x00000007,
        /// <summary>
        /// 
        /// </summary>
        NetBindRemove = 0x00000008,
        /// <summary>
        /// 
        /// </summary>
        NetBindEnable = 0x00000009,
        /// <summary>
        /// 
        /// </summary>
        NetBindDisable = 0x0000000A
    }
    #endregion

    #region enum ServiceError
    public enum ServiceError
    {
        /// <summary>
        /// 
        /// </summary>
        Ignore = 0x00000000,
        /// <summary>
        /// 
        /// </summary>
        Normal = 0x00000001,
        /// <summary>
        /// 
        /// </summary>
        Severe = 0x00000002,
        /// <summary>
        /// 
        /// </summary>
        Critical = 0x00000003
    }
    #endregion

    #region class ServiceError
    public class ServiceInstaller
    {
        private const int STANDARD_RIGHTS_REQUIRED = 0xF0000;
        private const int SERVICE_WIN32_OWN_PROCESS = 0x00000010;

        [StructLayout(LayoutKind.Sequential)]
        private class SERVICE_STATUS
        {
            public int dwServiceType = 0;
            public ServiceState dwCurrentState = 0;
            public int dwControlsAccepted = 0;
            public int dwWin32ExitCode = 0;
            public int dwServiceSpecificExitCode = 0;
            public int dwCheckPoint = 0;
            public int dwWaitHint = 0;
        }

        [DllImport("advapi32.dll", EntryPoint = "OpenSCManagerA")]
        private static extern IntPtr OpenSCManager(string lpMachineName, string
        lpDatabaseName, ServiceManagerRights dwDesiredAccess);
        [DllImport("advapi32.dll", EntryPoint = "OpenServiceA",
        CharSet = CharSet.Ansi)]
        private static extern IntPtr OpenService(IntPtr hSCManager, string
        lpServiceName, ServiceRights dwDesiredAccess);
        [DllImport("advapi32.dll", EntryPoint = "CreateServiceA")]
        private static extern IntPtr CreateService(IntPtr hSCManager, string
        lpServiceName, string lpDisplayName, ServiceRights dwDesiredAccess, int
        dwServiceType, ServiceBootFlag dwStartType, ServiceError dwErrorControl,
        string lpBinaryPathName, string lpLoadOrderGroup, IntPtr lpdwTagId, string
        lpDependencies, string lp, string lpPassword);
        [DllImport("advapi32.dll")]
        private static extern int CloseServiceHandle(IntPtr hSCObject);
        [DllImport("advapi32.dll")]
        private static extern int QueryServiceStatus(IntPtr hService,
        SERVICE_STATUS lpServiceStatus);
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern int DeleteService(IntPtr hService);
        [DllImport("advapi32.dll")]
        private static extern int ControlService(IntPtr hService, ServiceControl
        dwControl, SERVICE_STATUS lpServiceStatus);
        [DllImport("advapi32.dll", EntryPoint = "StartServiceA")]
        private static extern int StartService(IntPtr hService, int
        dwNumServiceArgs, int lpServiceArgVectors);

        /// <summary>
        /// 
        /// </summary>
        public ServiceInstaller()
        {
        }

        public static void Uninstall(string ServiceName)
        {
            IntPtr scman = OpenSCManager(ServiceManagerRights.Connect);
            try
            {
                IntPtr service = OpenService(scman, ServiceName,
                ServiceRights.StandardRightsRequired | ServiceRights.Stop |
                ServiceRights.QueryStatus);
                if (service == IntPtr.Zero)
                {
                    throw new ApplicationException("Service not installed.");
                }
                try
                {
                    StopService(service);
                    int ret = DeleteService(service);
                    if (ret == 0)
                    {
                        int error = Marshal.GetLastWin32Error();
                        throw new ApplicationException("Could not delete service " + error);
                    }
                }
                finally
                {
                    CloseServiceHandle(service);
                }
            }
            finally
            {
                CloseServiceHandle(scman);
            }
        }

        public static bool ServiceIsInstalled(string ServiceName)
        {
            IntPtr scman = OpenSCManager(ServiceManagerRights.Connect);
            try
            {
                IntPtr service = OpenService(scman, ServiceName, ServiceRights.QueryStatus);
                if (service == IntPtr.Zero) return false;
                CloseServiceHandle(service);
                return true;
            }
            finally
            {
                CloseServiceHandle(scman);
            }
        }

        public static Boolean InstallService(string ServiceName, string DisplayName, string FileName, Boolean Isstarted)
        {
            Boolean lbSuccess = true;
            IntPtr scman = OpenSCManager(ServiceManagerRights.Connect |
            ServiceManagerRights.CreateService);
            try
            {
                IntPtr service = OpenService(scman, ServiceName,
                ServiceRights.QueryStatus | ServiceRights.Start);
                if (service == IntPtr.Zero)
                {
                    service = CreateService(scman, ServiceName, DisplayName,
                    ServiceRights.QueryStatus | ServiceRights.Start, SERVICE_WIN32_OWN_PROCESS,
                    ServiceBootFlag.AutoStart, ServiceError.Normal, FileName, null, IntPtr.Zero,
                    null, null, null);
                }
                if (service == IntPtr.Zero)
                {
                    //throw new ApplicationException("Failed to install service.");
                    lbSuccess = false;
                }
                try
                {
                    if (Isstarted == true)
                    {
                        StartService(service);
                    }
                }
                finally
                {
                    CloseServiceHandle(service);
                }
            }
            finally
            {
                CloseServiceHandle(scman);
            }
            return lbSuccess;
        }

        public static void StartService(string Name)
        {
            IntPtr scman = OpenSCManager(ServiceManagerRights.Connect);
            try
            {
                IntPtr hService = OpenService(scman, Name, ServiceRights.QueryStatus |
                ServiceRights.Start);
                if (hService == IntPtr.Zero)
                {
                    throw new ApplicationException("Could not open service.");
                }
                try
                {
                    StartService(hService);
                }
                finally
                {
                    CloseServiceHandle(hService);
                }
            }
            finally
            {
                CloseServiceHandle(scman);
            }
        }

        public static void StopService(string Name)
        {
            IntPtr scman = OpenSCManager(ServiceManagerRights.Connect);
            try
            {
                IntPtr hService = OpenService(scman, Name, ServiceRights.QueryStatus |
                ServiceRights.Stop);
                if (hService == IntPtr.Zero)
                {
                    throw new ApplicationException("Could not open service.");
                }
                try
                {
                    StopService(hService);
                }
                finally
                {
                    CloseServiceHandle(hService);
                }
            }
            finally
            {
                CloseServiceHandle(scman);
            }
        }

        private static void StartService(IntPtr hService)
        {
            SERVICE_STATUS status = new SERVICE_STATUS();
            StartService(hService, 0, 0);
            WaitForServiceStatus(hService, ServiceState.Starting, ServiceState.Run);
        }

        private static void StopService(IntPtr hService)
        {
            SERVICE_STATUS status = new SERVICE_STATUS();
            ControlService(hService, ServiceControl.Stop, status);
            WaitForServiceStatus(hService, ServiceState.Stopping, ServiceState.Stop);
        }

        public static ServiceState GetServiceStatus(string ServiceName)
        {
            IntPtr scman = OpenSCManager(ServiceManagerRights.Connect);
            try
            {
                IntPtr hService = OpenService(scman, ServiceName,
                ServiceRights.QueryStatus);
                if (hService == IntPtr.Zero)
                {
                    return ServiceState.NotFound;
                }
                try
                {
                    return GetServiceStatus(hService);
                }
                finally
                {
                    CloseServiceHandle(scman);
                }
            }
            finally
            {
                CloseServiceHandle(scman);
            }
        }

        private static ServiceState GetServiceStatus(IntPtr hService)
        {
            SERVICE_STATUS ssStatus = new SERVICE_STATUS();
            if (QueryServiceStatus(hService, ssStatus) == 0)
            {
                throw new ApplicationException("Failed to query service status.");
            }
            return ssStatus.dwCurrentState;
        }

        private static bool WaitForServiceStatus(IntPtr hService, ServiceState WaitStatus, ServiceState DesiredStatus)
        {
            SERVICE_STATUS ssStatus = new SERVICE_STATUS();
            int dwOldCheckPoint;
            int dwStartTickCount;

            QueryServiceStatus(hService, ssStatus);
            if (ssStatus.dwCurrentState == DesiredStatus) return true;
            dwStartTickCount = Environment.TickCount;
            dwOldCheckPoint = ssStatus.dwCheckPoint;

            while (ssStatus.dwCurrentState == WaitStatus)
            {
                int dwWaitTime = ssStatus.dwWaitHint / 10;

                if (dwWaitTime < 1000) dwWaitTime = 1000;
                else if (dwWaitTime > 10000) dwWaitTime = 10000;

                System.Threading.Thread.Sleep(dwWaitTime);

                // Check the status again.

                if (QueryServiceStatus(hService, ssStatus) == 0) break;

                if (ssStatus.dwCheckPoint > dwOldCheckPoint)
                {
                    // The service is making progress.
                    dwStartTickCount = Environment.TickCount;
                    dwOldCheckPoint = ssStatus.dwCheckPoint;
                }
                else
                {
                    if (Environment.TickCount - dwStartTickCount > ssStatus.dwWaitHint)
                    {
                        // No progress made within the wait hint
                        break;
                    }
                }
            }
            return (ssStatus.dwCurrentState == DesiredStatus);
        }

        private static IntPtr OpenSCManager(ServiceManagerRights Rights)
        {
            IntPtr scman = OpenSCManager(null, null, Rights);
            if (scman == IntPtr.Zero)
            {
                throw new ApplicationException("Could not connect to service control manager.");
            }
            return scman;
        }
    }
    #endregion

    #region class DeployServices
    class DeployServices
    {
        string strServiceName = string.Empty;
        Boolean bDoUninstall = true;

        #region DeployServices
        public DeployServices() { }
        #endregion

        #region SERVICE_NAME
        public string SERVICE_NAME
        {
            get { return strServiceName; }
            set { strServiceName = value; }
        }
        #endregion
        #region DO_UNINSTALL
        public Boolean DO_UNINSTALL
        {
            get { return bDoUninstall; }
            set { bDoUninstall = value; }
        }
        #endregion


        public Boolean UninstallServices()
        {
            Boolean SuccessUninstall = false;

            if (strServiceName != "")
            {
                try
                {
                    if (ServiceTools.ServiceInstaller.ServiceIsInstalled(strServiceName) == true)
                    {
                        ServiceTools.ServiceInstaller.StopService(strServiceName);
                        if (bDoUninstall == true)
                        {
                            ServiceTools.ServiceInstaller.Uninstall(strServiceName);

                        }
                        SuccessUninstall = true;
                    }
                }
                catch { }
            }
            return SuccessUninstall;
        }




    }
    #endregion
}
