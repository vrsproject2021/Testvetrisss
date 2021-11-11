using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Vetris.Report.Core.Configurations
{
    public static class HostingEnvironmentExtensions
    {
        public enum TrimTerminalPathSepatorOption
        {
            None,
            Start,
            End,
            Both
        }
        public static IConfigurationRoot GetAppConfiguration(this IHostingEnvironment env)
        {
            return AppConfigurations.Get(env.ContentRootPath);
        }

        /// <summary>
        /// Convert path sperator according to Operating system. e.g, for Linux /, for Windows \. 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="trimTerminalSeperator">Default = 2  (clear end seperator), </param>
        /// <returns></returns>
        public static string ToOSPath(this string path, TrimTerminalPathSepatorOption trimTerminalSeperator = TrimTerminalPathSepatorOption.End)
        {
            var ospath = path ?? "";
            var slash = "\\";
            var invalidSlash = "/";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                invalidSlash = "\\";
                slash = "/";
                Regex pattern = new Regex(@"^(?<drive>[A-Za-z]:)?(?<path>.*)");
                //discard drive info like c:\ which is not valid
                if (pattern.IsMatch(ospath))
                {
                    ospath = pattern.Match(ospath).Groups["path"].Value;
                }
            }
            else
            {
                invalidSlash = "/";
                slash = "\\";
            }
            if (ospath.Contains(invalidSlash))
            {
                ospath = ospath.Replace(invalidSlash, slash);
            }
            if (trimTerminalSeperator == TrimTerminalPathSepatorOption.End && ospath.Length > 1 && ospath.EndsWith(slash))
            {
                ospath = ospath.Substring(0, ospath.Length - 1);
            }
            else if (trimTerminalSeperator == TrimTerminalPathSepatorOption.Start && ospath.EndsWith(slash))
            {
                ospath = ospath.Substring(1);
            }
            else if (trimTerminalSeperator == TrimTerminalPathSepatorOption.Both)
            {
                if (ospath.StartsWith(slash))
                {
                    ospath = ospath.Substring(1);
                }
                if (ospath.EndsWith(slash) && ospath.Length > 1)
                {
                    ospath = ospath.Substring(0, ospath.Length - 1);
                }

            }
            return ospath;
        }
        public static string ToMiddlePath(this string path)
        {
            return path.ToOSPath(TrimTerminalPathSepatorOption.Both);
        }
        public static string TrimPathSeperatorFromFirstLast(this string path)
        {
            var ospath = path ?? "";
            var slash = "\\";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                slash = "/";

            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                slash = "\\";
            }
            if (ospath.StartsWith(slash))
            {
                ospath = ospath.Substring(1);
            }
            if (ospath.EndsWith(slash))
            {
                ospath = ospath.Substring(0, ospath.Length - 1);
            }
            return ospath;
        }

        public static string ToActionPath(this string path, TrimTerminalPathSepatorOption trimTerminalSeperator = TrimTerminalPathSepatorOption.End)
        {
            path = path ?? "";
            if (path.Contains("\\"))
            {
                path = path.Replace("\\", "/");
            }

            if (trimTerminalSeperator == TrimTerminalPathSepatorOption.End && path.Length > 1 && path.EndsWith("/"))
            {
                path = path.Substring(0, path.Length - 1);
            }
            else if (trimTerminalSeperator == TrimTerminalPathSepatorOption.Start && path.EndsWith("/"))
            {
                path = path.Substring(1);
            }
            else if (trimTerminalSeperator == TrimTerminalPathSepatorOption.Both)
            {
                if (path.StartsWith("/"))
                {
                    path = path.Substring(1);
                }
                if (path.EndsWith("/") && path.Length > 1)
                {
                    path = path.Substring(0, path.Length - 1);
                }
            }
            return path;
        }
    }
}
