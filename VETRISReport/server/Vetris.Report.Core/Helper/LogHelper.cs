using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Vetris.Report.Core.Helper
{
    public static class LogHelper
    {
        public static void Log(string message, int outletId=0)
        {
            ChangeFileLocation(outletId);
            ILog logger = LogManager.GetLogger("Log");
            logger.Debug(message);
        }
        public static void ChangeFileLocation(int outletId)
        {
            var log4netRepository = log4net.LogManager.GetRepository(Assembly.GetEntryAssembly());
            log4net.Config.XmlConfigurator.Configure(log4netRepository, new FileInfo("log4net.config"));
            log4net.Repository.Hierarchy.Hierarchy hierarchy = (Hierarchy)log4netRepository;

            foreach (IAppender appender in hierarchy.Root.Appenders)
            {
                if (appender is RollingFileAppender)
                {
                    RollingFileAppender fa = (RollingFileAppender)appender;
                    string logFileLocation = fa.File.Replace("_0_", "_" + outletId.ToString() + "_");
                    if (!Directory.Exists(Path.GetDirectoryName(logFileLocation)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(logFileLocation));
                    }
                    fa.File = logFileLocation;
                    fa.ActivateOptions();
                    break;
                }
            }
        }
    }
}
