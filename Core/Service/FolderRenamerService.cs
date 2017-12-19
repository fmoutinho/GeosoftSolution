using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public static class FolderRenamerService
    {
        public static void Rename()
        {
            string rootPath = ConfigurationManager.AppSettings["ROOT_PATH"];

            Rename(rootPath);
        }

        private static void Rename(string path)
        {
            if (Directory.Exists(path))
            {
                TreatSameNameInNextDirectory(path);

                string[] subDirectories = Directory.GetDirectories(path);

                foreach (string subdirectory in subDirectories)
                {
                    Rename(subdirectory);
                }
            }
        }

        private static void TreatSameNameInNextDirectory(string path)
        {



        }
    }
}
