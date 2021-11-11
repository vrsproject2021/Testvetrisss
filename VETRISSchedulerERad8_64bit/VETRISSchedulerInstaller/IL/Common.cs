using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using IWshRuntimeLibrary;


namespace VETRISSchedulerInstaller.IL
{
    class Common
    {
        #region OpenFolderDialouge
        public static string OpenFolderDialouge(string initialPath, string Description)
        {
            string lsPath = string.Empty;
            FolderBrowserDialog FdialogFolder = new FolderBrowserDialog();
            FdialogFolder.RootFolder = Environment.SpecialFolder.Desktop;

            if (System.IO.Directory.Exists(initialPath) == true)
                FdialogFolder.SelectedPath = initialPath;
            else
                FdialogFolder.SelectedPath = "C:\\";

            FdialogFolder.Description = Description;
            if (FdialogFolder.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                lsPath = FdialogFolder.SelectedPath;
            }
            FdialogFolder.Dispose();
            FdialogFolder = null;
            return lsPath;
        }
        #endregion

        #region IsValidAYWApplicationPath
        public static Boolean IsValidAYWApplicationPath(string ApplicationBinDir)
        {
            Boolean lbValidPath = true;
            string lsWebConfigFile = string.Empty;
            string lsConfigAYWFile = string.Empty;
            if (System.IO.File.Exists(ApplicationBinDir + "\\AtYourWish.dll") == true)
            {
                if (ApplicationBinDir.IndexOf("\\bin") != -1)
                {
                    lsWebConfigFile = ApplicationBinDir.Replace("\\bin", "") + "\\web.config";
                    lsConfigAYWFile = ApplicationBinDir.Replace("\\bin", "") + "\\config.ayw";
                    if (System.IO.File.Exists(lsWebConfigFile) == false) { lbValidPath = false; }
                    if (System.IO.File.Exists(lsConfigAYWFile) == false) { lbValidPath = false; }
                    if (lbValidPath == true)
                    {
                        lbValidPath = true;
                    }
                }
                else { lbValidPath = false; }
            }
            else { lbValidPath = false; }

            return lbValidPath;
        }
        #endregion

        #region DeleteFilesFromList
        public static void DeleteFilesFromList(string[] lsFiles)
        {
            try
            {
                foreach (string lsfile in lsFiles)
                {
                    if (System.IO.File.Exists(lsfile) == true)
                    {
                        System.IO.File.Delete(lsfile);
                    }
                }
            }
            catch { }
        }
        #endregion

        #region DeleteFilesFromTarget
        public static void DeleteFilesFromTarget(string FolderPath, string Extention)
        {
            try
            {
                var files = Directory.GetFiles(FolderPath, Extention);
                foreach (var f in files)
                {
                    var attr = System.IO.File.GetAttributes(f);
                    // Is this file marked as 'read-only'?
                    if ((attr & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                    {
                        // Yes... Remove the 'read-only' attribute, then
                        System.IO.File.SetAttributes(f, attr ^ FileAttributes.ReadOnly);
                    }
                    // Delete the file
                    System.IO.File.Delete(f);
                }
            }
            catch { }
        }
        #endregion

        #region DeleteFilesFromTarget
        public static void DeleteFilesFromTarget(string FolderPath)
        {
            DirectoryInfo dir = new DirectoryInfo(FolderPath);
            FileSystemInfo[] infos = dir.GetFileSystemInfos();
            string parentFolder = string.Empty;

            try
            {
                if (infos != null)
                {
                    // Iterate through each item.
                    foreach (FileSystemInfo i in infos)
                    {
                        // Check to see if this is a DirectoryInfo object.
                        if (i is DirectoryInfo)
                        {

                            // Cast the object to a DirectoryInfo object.
                            DirectoryInfo dInfo = (DirectoryInfo)i;
                            DeleteFilesFromTarget(dInfo.FullName);

                        }
                        // Check to see if this is a FileInfo object.
                        else if (i is FileInfo)
                        {

                            parentFolder = i.FullName.Substring(0, i.FullName.LastIndexOf("\\"));

                            //delete the file
                            var attr = System.IO.File.GetAttributes(i.FullName);
                            if ((attr & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                            {
                                System.IO.File.SetAttributes(i.FullName, attr ^ FileAttributes.ReadOnly);
                            }
                            System.IO.File.Delete(i.FullName);


                            if (parentFolder.Trim() != string.Empty)
                            {
                                if (System.IO.Directory.GetDirectories(parentFolder.Trim()).Length == 0)
                                {
                                    if (System.IO.Directory.GetFiles(parentFolder.Trim()).Length == 0)
                                    {
                                        System.IO.Directory.Delete(parentFolder.Trim());
                                    }
                                }
                                else
                                    DeleteFilesFromTarget(parentFolder.Trim());

                            }

                        }
                    }
                }


            }
            catch { ;}
        }
        #endregion

        #region DeleteEmptyFolders
        public static void DeleteEmptyFolders(string FolderPath)
        {
            foreach (var directory in Directory.GetDirectories(FolderPath))
            {
                DeleteEmptyFolders(directory);
                if (Directory.GetFiles(directory).Length == 0 &&
                    Directory.GetDirectories(directory).Length == 0)
                {
                    Directory.Delete(directory, false);
                }
            }
        }
        #endregion


        #region DeleteDeskTopShortcut
        public static void DeleteDeskTopShortcut(string shortcutLinkName)
        {
            try
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                //File.Delete(Path.Combine(desktopPath, "Touch Data.lnk"));
                System.IO.File.Delete(Path.Combine(desktopPath, shortcutLinkName + ".lnk"));
            }
            catch { }
        }
        #endregion

        #region DeleteAllProgramShortcut
        public static void DeleteAllProgramShortcut(string shortcutLinkName)
        {
            try
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                System.IO.File.Delete(Path.Combine(desktopPath, shortcutLinkName));

                string commonStartMenuPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu);
                string appStartMenuAYWPath = Path.Combine(commonStartMenuPath, "Programs\\RAD 365", "VETRIS DICOM ROUTER");

                if (Directory.Exists(appStartMenuAYWPath))
                {
                    System.IO.File.Delete(Path.Combine(appStartMenuAYWPath, shortcutLinkName + ".lnk"));
                }
            }
            catch { }
        }
        #endregion

        #region CreateShortcutToDesktop
        public static void CreateShortcutToDesktop(string shortcutName, string Description, string targetFileLocation)
        {
            string shortcutPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string shortcutLocation = System.IO.Path.Combine(shortcutPath, shortcutName + ".lnk");
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);

            shortcut.Description = Description;
            //shortcut.IconLocation = @"c:\myicon.ico";
            shortcut.TargetPath = targetFileLocation;
            shortcut.Save();
        }
        #endregion

        #region CreateShortcutToAllProgram
        public static void CreateShortcutToAllProgram(string shortcutName, string Description, string targetFileLocation)
        {
            string commonStartMenuPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu);

            string appStartMenuPath = Path.Combine(commonStartMenuPath, "Programs", "RAD 365");
            if (!Directory.Exists(appStartMenuPath))
                Directory.CreateDirectory(appStartMenuPath);

            string appStartMenuAYWPath = Path.Combine(commonStartMenuPath, "Programs\\RAD 365", "VETRIS DICOM ROUTER");
            if (!Directory.Exists(appStartMenuAYWPath))
                Directory.CreateDirectory(appStartMenuAYWPath);

            string shortcutLocation = Path.Combine(appStartMenuAYWPath, shortcutName + ".lnk");
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);

            shortcut.Description = Description;
            shortcut.TargetPath = targetFileLocation;
            shortcut.Save();
        }
        #endregion

        #region CopyDirectory
        public static Boolean CopyDirectory(string Src, string Dst)
        {
            Boolean bSuccess = true;
            String[] Files;
            try
            {
                if (Dst[Dst.Length - 1] != Path.DirectorySeparatorChar)
                    Dst += Path.DirectorySeparatorChar;
                if (!Directory.Exists(Dst)) Directory.CreateDirectory(Dst);
                Files = Directory.GetFileSystemEntries(Src);
                foreach (string Element in Files)
                {
                    // Sub directories
                    if (Directory.Exists(Element))
                    {
                        CopyDirectory(Element, Dst + Path.GetFileName(Element));
                        bSuccess = CopyDirectory(Element, Dst + Path.GetFileName(Element));
                        if (bSuccess == false) break;
                    }
                    // Files in directory
                    else
                        System.IO.File.Copy(Element, Dst + Path.GetFileName(Element), true);
                }
            }
            catch { bSuccess = false; }
            return bSuccess;
        }
        #endregion

        #region GetLinesFromTextFile
        public static string[] GetLinesFromTextFile(string FilePath)
        {
            string[] Lines = new string[1];
            FileInfo Fi = new FileInfo(FilePath);
            try
            {
                if (Fi.Exists == true)
                {
                    Lines = System.IO.File.ReadAllLines(FilePath);
                }
            }
            catch { }
            finally { Fi = null; }
            return Lines;
        }
        #endregion

        #region WriteLinesToFile
        private void WriteLinesToFile(string[] Lines, string FilePath)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(FilePath))
            {
                foreach (string line in Lines)
                {
                    file.WriteLine(line);
                }
            }
        }
        #endregion
    }
}
