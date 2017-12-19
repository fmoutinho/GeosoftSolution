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
                string[] subDirectories = Directory.GetDirectories(path);

                foreach (string subdirectory in subDirectories)
                {
                    if (HadSameNameInNextDirectory(subdirectory))
                    {
                        Rename(path);
                        break;
                    }
                    else
                    {
                        Rename(subdirectory);
                    }
                }
            }
        }

        private static bool HadSameNameInNextDirectory(string path)
        {
            if (Path.GetFileName(path).Equals(Directory.GetParent(path).Name))
            {
                MoveFiles(path);

                MoveDirectories(path);

                Directory.Delete(path);

                return true;
            }

            return false;
        }

        private static void MoveDirectories(string path)
        {
            string[] directories = Directory.GetDirectories(path);

            foreach (string directory in directories)
            {
                Directory.Move(string.Format(@"{0}", directory), string.Format(@"{0}\{1}", Directory.GetParent(path).FullName, Path.GetFileName(directory)));
            }
        }

        private static void MoveFiles(string path)
        {
            string[] files = Directory.GetFiles(path);

            foreach (string file in files)
            {
                File.Move(string.Format(@"{0}", file), string.Format(@"{0}\{1}", Directory.GetParent(path).FullName, Path.GetFileName(file)));
            }
        }
    }
}
