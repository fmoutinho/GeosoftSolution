using Alphaleonis.Win32.Filesystem;
using Core.Service.FileZipperService.Interfaces;
using System;
using System.Configuration;
using System.Linq;

namespace Core.Service.FileZipperService
{
    public class FileZipperService
    {
        public Zipper Zip;

        public FileZipperService(Zipper zip)
        {
            this.Zip = zip;
        }

        public void ZipProjects()
        {
            string[] projectsToZip = Directory.GetDirectories(ConfigurationManager.AppSettings["PROJECTS_FOLDER"]);

            int sucess = 0;

            foreach (string currentProject in projectsToZip)
            {
                try
                {
                    System.Console.ForegroundColor = ConsoleColor.Blue;
                    System.Console.WriteLine(string.Format("Zipping project {0}", Path.GetFileName(currentProject)));

                    Zip.ZipProject(currentProject);

                    System.Console.ForegroundColor = ConsoleColor.Green;
                    System.Console.WriteLine(string.Format("Project {0} zipped successfully", Path.GetFileName(currentProject)));

                    sucess++;
                }
                catch (Exception ex)
                {
                    System.Console.ForegroundColor = ConsoleColor.Red;
                    System.Console.WriteLine(string.Format("Error zipping project {0} . Exception: {1}", Path.GetFileName(currentProject), ex.Message));
                }
            }

            System.Console.ForegroundColor = ConsoleColor.Blue;
            System.Console.WriteLine(string.Format("{0} projects out of {1} have been zipped sucessfully, consult console for any doubt", sucess, projectsToZip.Count()));
            System.Console.ReadKey();

        }
    }
}
