using Alphaleonis.Win32.Filesystem;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Util
{
    public static class Util
    {
        public static void CreateBatchFile()
        {
            string[] projectsToZip = Directory.GetDirectories(ConfigurationManager.AppSettings["PROJECTS_FOLDER"]);
            List<string> commands = new List<string>();

            foreach (string pathToZip in projectsToZip)
            {
                StringBuilder command = new StringBuilder(string.Format(@" & '{0}' a -afzip -ep1 -v3000M ", ConfigurationManager.AppSettings["WINRAR_DIRECTORY"]));

                command.Append(string.Format(@"'{0}\{1}.zip' ", ConfigurationManager.AppSettings["DESTINY_FOLDER"], Path.GetFileName(pathToZip)));
                command.Append(string.Format(@"'{0}' ", pathToZip));

                commands.Add(command.ToString());
            }

            System.IO.File.WriteAllLines(string.Format(@"{0}\Batch_Zip_{1}.txt", ConfigurationManager.AppSettings["BATCH_PATH"], DateTime.Now.ToString("yyyyMMdd")), commands);
        }
    }
}
