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
        #region First Method do be called

        public static void Short()
        {
            string rootPath = ConfigurationManager.AppSettings["ROOT_PATH"];

            string[] projectsPath = Directory.GetDirectories(rootPath);

            foreach (string currentProject in projectsPath)
            {
                try
                {
                    System.Console.WriteLine(string.Format("Shortening project's {0} file paths ", Path.GetFileName(currentProject)));

                    Treat(currentProject, Util.Enum.Action.REPLACE);

                    MountNewTree(currentProject);

                    Treat(currentProject, Util.Enum.Action.DELETE);

                    System.Console.WriteLine(string.Format("Project's {0} file paths shortened sucessfully", Path.GetFileName(currentProject)));

                }
                catch (Exception ex)
                {
                    System.Console.WriteLine(string.Format("Error shortening project {0} . Exception: {1}", Path.GetFileName(currentProject), ex.Message));
                }
            }
        }

        #endregion

        #region Main Methods 

        private static void MountNewTree(string path)
        {
            if (Directory.Exists(path))
            {
                string[] subdirectories = Directory.GetDirectories(path);

                foreach (string subdirectory in subdirectories)
                {
                    MountNewTree(subdirectory);
                }

                if (Path.GetFileName(path).Equals(Path.GetFileName(Directory.GetParent(path).ToString())))
                {
                    MoveElements(path, Directory.GetParent(path).ToString());

                    DeleteDirectory(path);
                }
            }
        }

        private static void Treat(string path, Util.Enum.Action action)
        {
            string[] subdirectories = Directory.GetDirectories(path);

            string pathName = string.Format(@"{0}", path);

            foreach (string subdirectory in subdirectories)
            {                
                Treat(subdirectory, action);
            }

            string newPath = TreatDirectoryName(pathName, action);

            if (!path.Equals(newPath))
            {
                CreateDirectory(pathName, newPath);

                DeleteDirectory(path);
            }
        }

        #endregion

        #region Support Methods

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

        private static string TreatDirectoryName(string path, Util.Enum.Action action)
        {
            string result = path;

            GeodatascanBHP_Entities dbContext = new GeodatascanBHP_Entities();

            var sortedByActionListExpression = (from e in dbContext.Expression where e.Action == (int)action select e);

            foreach (Expression expressionAux in sortedByActionListExpression)
            {
                if (result.Contains(expressionAux.Description) )
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

                string newPath = string.Format(@"{0}\{1}\", to, Path.GetFileName(directory));

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

        #endregion
    }
}
