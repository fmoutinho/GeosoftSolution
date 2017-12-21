using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alphaleonis.Win32.Filesystem;

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
                string newPath = TreatDirectoryName(path);

                if (!path.Equals(newPath))
                {
                    Directory.CreateDirectory(newPath);

                    MoveElements(path, newPath);
                }

                string[] subdirectories = Directory.GetDirectories(newPath);

                foreach (string subdirectory in subdirectories)
                {
                    if (Path.GetFileName(subdirectory).Equals(Path.GetFileName(newPath)))
                    {
                        MoveElements(subdirectory, newPath);

                        Directory.Delete(subdirectory);

                        Rename(newPath);

                        break;
                    }
                    else
                    {
                        Rename(subdirectory);
                    }
                }

                if (!path.Equals(newPath))
                {
                    Directory.Delete(path);
                }
            }
        }

        private static string TreatDirectoryName(string path)
        {
            string result = path;

            return result;
        }

        private static void MoveElements(string from, string to)
        {
                MoveFiles(from, Directory.GetParent(to).FullName);

                MoveDirectories(from, Directory.GetParent(to).FullName);
        }

        private static void MoveDirectories(string from, string to)
        {
            string[] directories = Directory.GetDirectories(from);

            foreach (string directory in directories)
            {
                Directory.Move(string.Format(@"{0}", directory), string.Format(@"{0}\{1}", to, Path.GetFileName(directory)));
            }
        }

        private static void MoveFiles(string from, string to)
        {
            string[] files = Directory.GetFiles(from);

            foreach (string file in files)
            {
                File.Move(string.Format(@"{0}", file), string.Format(@"{0}\{1}", to, Path.GetFileName(file)));
            }
        }
    }
}
