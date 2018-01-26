using Alphaleonis.Win32.Filesystem;
using Core.Model;
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
        public static void CreateShortQueryFile()
        {

            string[] projectsToShortPaths = Directory.GetDirectories(ConfigurationManager.AppSettings["ROOT_PATH"]);

            List<FolderInfo> foldersInfo = new List<FolderInfo>();

            foreach (string projectPath in projectsToShortPaths)
            {
                ListFoldersInfo(foldersInfo, projectPath);
            }

            List<string> queries = new List<string>();

            foreach(FolderInfo auxFolderInfo in foldersInfo.OrderByDescending(x => x.Quantity))
            {
                queries.Add(string.Format("--exec [dbo].[UP_Insert_Expression_To_Replace] @Expression = '{0}' , @Replacement = ''", auxFolderInfo.Name.Replace("'", "''")));
            }

            System.IO.File.WriteAllLines(string.Format(@"{0}\Batch_Queries_{1}.sql", ConfigurationManager.AppSettings["ROOT_PATH"], DateTime.Now.ToString("yyyyMMdd")), queries);
        }

        private static void ListFoldersInfo(List<FolderInfo> foldersInfo, string projectPath)
        {
            string[] directories = Directory.GetDirectories(projectPath);

            foreach (string directory in directories)
            {
                FolderInfo aux = foldersInfo.Where(x => x.Name.Equals(Path.GetFileName(directory))).FirstOrDefault();
                if (aux != null)
                {
                    aux.Quantity++;
                }
                else
                {
                    foldersInfo.Add(new FolderInfo(Path.GetFileName(directory)));
                }

                ListFoldersInfo(foldersInfo, directory);
            }
        }

        public static void CreateZipBatchFile()
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
