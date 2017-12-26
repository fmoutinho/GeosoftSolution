using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alphaleonis.Win32.Filesystem;
using Core.Model;
using Core.Util.Enum;

namespace Core.Service
{
    public static class FolderRenamerService
    {
        public static void Short()
        {
            string rootPath = ConfigurationManager.AppSettings["ROOT_PATH"];

            MountNewTree(rootPath);

            Rename(rootPath);
        }

        private static void MountNewTree(string path)
        {
            if (Directory.Exists(path))
            {

                string[] subdirectories = Directory.GetDirectories(path);

                foreach (string subdirectory in subdirectories)
                {
                    if (Path.GetFileName(subdirectory).Equals(Path.GetFileName(path)))
                    {
                        MoveElements(subdirectory, path);

                        DeleteDirectory(subdirectory);

                        MountNewTree(path);

                        break;
                    }
                    else
                    {
                        MountNewTree(subdirectory);
                    }
                }
            }
        }

        private static void Rename(string path)
        {
            string[] subdirectories = Directory.GetDirectories(path);

            foreach (string subdirectory in subdirectories)
            {
                Rename(subdirectory);
            }

            string newPath = TreatDirectoryName(path);

            if (!path.Equals(newPath))
            {
                CreateDirectory(path, newPath);

                DeleteDirectory(path);
            }
        }

        private static void CreateDirectory(string path, string newPath)
        {

            Directory.CreateDirectory(newPath);

            MoveElements(path, newPath);

        }

        private static void DeleteDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path);
            }
        }

        private static string TreatDirectoryName(string path)
        {
            string result = path;

            GeodatascanBHP_Entities dbContext = new GeodatascanBHP_Entities();

            var sortedByActionListExpression = (from e in dbContext.Expression orderby e.Action select e);

            foreach (Expression expressionAux in sortedByActionListExpression)
            {
                if (result.Contains(expressionAux.Description))
                {
                    switch (expressionAux.Action)
                    {
                        case (int)Util.Enum.Action.REPLACE:

                            result = result.Replace(expressionAux.Description, expressionAux.Replacement);

                            break;
                        case (int)Util.Enum.Action.DELETE:

                            result = result.Replace(expressionAux.Description, string.Empty);

                            break;
                        default:
                            break;
                    }
                }
            }

            return result;
        }

        private static void MoveElements(string from, string to)
        {
            if (Directory.Exists(from))
            {
                MoveFiles(from, to);

                MoveDirectories(from, to);
            }
        }

        private static void MoveDirectories(string from, string to)
        {

            string[] directories = Directory.GetDirectories(from);

            foreach (string directory in directories)
            {
                string oldPath = string.Format(@"{0}", directory);

                string newPath = string.Format(@"{0}\{1}", to, Path.GetFileName(directory));

                Directory.Move(oldPath, newPath);
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
