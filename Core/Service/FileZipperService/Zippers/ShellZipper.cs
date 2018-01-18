using Alphaleonis.Win32.Filesystem;
using Core.Service.FileZipperService.Interfaces;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Text;

namespace Core.Service.FileZipperService.Zippers
{
    public class ShellZipper : Zipper
    {
        public void ZipProject(string pathToZip)
        {
            using (PowerShell PowerShellInstance = PowerShell.Create())
            {
                StringBuilder command = new StringBuilder(string.Format(@" & '{0}' a -afzip -ep1 -v3000M ", ConfigurationManager.AppSettings["WINRAR_DIRECTORY"]));

                command.Append(string.Format(@"'{0}\{1}.zip' ", ConfigurationManager.AppSettings["DESTINY_FOLDER"], Path.GetFileName(pathToZip)));
                command.Append(string.Format(@"'{0}' ", pathToZip));

                PowerShellInstance.AddScript(string.Format(@"{0}", command.ToString()));

                PowerShellInstance.Invoke();

                Process[] currentProcess = Process.GetProcessesByName("WinRAR");

                foreach (Process aux in currentProcess)
                {
                    long timeout = int.Parse(ConfigurationManager.AppSettings["OFFSET"]) * (DirSize(new DirectoryInfo(pathToZip)) / int.Parse(ConfigurationManager.AppSettings["ONE_GIGABYTE"]));

                    if (timeout > int.MaxValue)
                    {
                        aux.WaitForExit();
                    }
                    else
                    {
                        aux.WaitForExit((int)timeout);

                        if (!aux.HasExited)
                        {
                            aux.Kill();
                            aux.Dispose();

                            System.Threading.Thread.Sleep(500);

                            List<string> filesToDelete = Directory.GetFiles(string.Format(@"{0}", ConfigurationManager.AppSettings["DESTINY_FOLDER"])).ToList();

                            filesToDelete.ForEach(x =>
                            {
                                if (x.Contains(Path.GetFileName(pathToZip)))
                                {

                                    File.Delete(x);
                                };
                            });

                            throw new System.Exception("Timeout Expired");
                        }
                    }

                    DeleteDirectory(pathToZip);
                }
            }
        }

        public static long DirSize(DirectoryInfo d)
        {
            long size = 0;
            // Add file sizes.
            Alphaleonis.Win32.Filesystem.FileInfo[] fis = d.GetFiles();
            foreach (Alphaleonis.Win32.Filesystem.FileInfo fi in fis)
            {
                size += fi.Length;
            }
            // Add subdirectory sizes.
            DirectoryInfo[] dis = d.GetDirectories();
            foreach (DirectoryInfo di in dis)
            {
                size += DirSize(di);
            }
            return size;
        }

        public static void DeleteDirectory(string target_dir)
        {
            string[] files = Directory.GetFiles(target_dir);
            string[] dirs = Directory.GetDirectories(target_dir);

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            foreach (string file in files)
            {
                File.SetAttributes(file, System.IO.FileAttributes.Normal);
                File.Delete(file);
            }

            Directory.Delete(target_dir, false);
        }
    }
}
