using Alphaleonis.Win32.Filesystem;
using Core.Service.FileZipperService.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;


namespace Core.Service.FileZipperService.Zippers
{
    public class ShellZipper : Zipper
    {
        public void ZipProject(string path)
        {
            //if (DirSize(new DirectoryInfo(path)) > 3000000)

            using (PowerShell PowerShellInstance = PowerShell.Create())
            {
                StringBuilder command = new StringBuilder(string.Format(@" & '{0}' a -afzip -ep1 -v3000M ", ConfigurationManager.AppSettings["WINRAR_DIRECTORY"]));
                command.Append(string.Format(@"{0}.zip ", path));
                command.Append(string.Format(@"{0} ", path));

                PowerShellInstance.AddScript(string.Format(@"{0}", command.ToString()));

                PowerShellInstance.Invoke();
            }
        }

        public static long DirSize(DirectoryInfo d)
        {
            long size = 0;
            // Add file sizes.
            FileInfo[] fis = d.GetFiles();
            foreach (FileInfo fi in fis)
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
    }
}
