using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Alphaleonis.Win32.Filesystem;
using Core.Model;
using Core.Util.Enum;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;

namespace Core.Service.FolderRenameService
{
    public static class FolderRenamerService
    {
        #region First Method do be called

        public static void Short()
        {
            string rootPath = ConfigurationManager.AppSettings["ROOT_PATH"];

            string[] projectsPath = Directory.GetDirectories(rootPath);

            int sucess = 0;

            foreach (string currentProject in projectsPath)
            {
                try
                {
                    System.Console.ForegroundColor = ConsoleColor.Blue;
                    System.Console.WriteLine(string.Format("Shortening project's {0} file paths ", Path.GetFileName(currentProject)));

                    TreatProjects(currentProject, Util.Enum.Action.REPLACE);

                    //MountNewTree(currentProject);

                    System.Console.ForegroundColor = ConsoleColor.Green;
                    System.Console.WriteLine(string.Format("Project's {0} file paths shortened sucessfully", Path.GetFileName(currentProject)));
                    sucess++;

                }
                catch (Exception ex)
                {
                    System.Console.ForegroundColor = ConsoleColor.Red;
                    System.Console.WriteLine(string.Format("Error shortening project {0} . Exception: {1}", Path.GetFileName(currentProject), ex.Message));
                }

            }

            System.Console.ForegroundColor = ConsoleColor.Blue;
            System.Console.WriteLine(string.Format("{0} projects out of {1} have been shortened sucessfully, consult console for any doubt", sucess, projectsPath.Count()));
            System.Console.ReadKey();
        }

        private static void TreatProjects(string path, Util.Enum.Action action)
        {
            string[] subdirectories = Directory.GetDirectories(path);

            string pathName = string.Format(@"{0}", path);

            foreach (string subdirectory in subdirectories)
            {
                try
                {
                    Treat(subdirectory, path.Length, action);
                }
                catch (Exception ex)
                {
                    System.Console.ForegroundColor = ConsoleColor.Yellow;
                    System.Console.WriteLine(string.Format("Error mounting new tree for directory {0} file path. Exception: {1}", path, ex.Message));
                }

            }

            DeleteEmptyDirectories(path);
        }

        private static void DeleteEmptyDirectories(string path)
        {
            string[] directories = Directory.GetDirectories(path);

            string[] files = Directory.GetFiles(path);

            foreach (string directory in directories)
            {
                DeleteEmptyDirectories(directory);
            }

            directories = Directory.GetDirectories(path);

            if ((files.Count() == 0) && (directories.Count() == 0))
            {
                Directory.Delete(path);
            }
        }

        #endregion

        #region Main Methods 

        private static void MountNewTree(string path)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    string[] subdirectories = Directory.GetDirectories(path);

                    foreach (string subdirectory in subdirectories)
                    {
                        MountNewTree(subdirectory);
                    }

                    if (Path.GetFileName(path).ToUpper().Equals(Path.GetFileName(Directory.GetParent(path).ToString().ToUpper())))
                    {
                        MoveElements(path, Directory.GetParent(path).ToString());

                        DeleteDirectory(path);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.ForegroundColor = ConsoleColor.Yellow;
                System.Console.WriteLine(string.Format("Error mounting new tree for directory {0} file path. Exception: {1}", path, ex.Message));
            }
        }

        private static void Treat(string path, int projectLength, Util.Enum.Action action)
        {
            try
            {
                string[] subdirectories = Directory.GetDirectories(path);

                string pathName = string.Format(@"{0}", path);

                foreach (string subdirectory in subdirectories)
                {
                    Treat(subdirectory, projectLength, action);
                }

                string newPath = string.Format(@"{0}{1}", path.Substring(0, projectLength), TreatDirectoryName(pathName.Substring(projectLength), action));

                if (!path.Equals(newPath))
                {
                    MoveElements(pathName, newPath);

                    DeleteDirectory(path);
                }
            }
            catch (Exception ex)
            {
                System.Console.ForegroundColor = ConsoleColor.Yellow;
                System.Console.WriteLine(string.Format("Error treating {0} file path. Exception: {1}", Path.GetFileName(path), ex.Message));
            }
        }

        #endregion

        #region Support Methods

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

                if (result.ToUpper().Contains(expressionAux.Description.ToUpper()))
                {
                    switch (expressionAux.Action)
                    {
                        case (int)Util.Enum.Action.REPLACE:

                            result = result.Replace(expressionAux.Description, expressionAux.Replacement);
                            result = result.Replace(expressionAux.Description.ToLower(), expressionAux.Replacement);
                            result = result.Replace(expressionAux.Description.ToUpper(), expressionAux.Replacement);

                            break;
                        case (int)Util.Enum.Action.DELETE:

                            result = result.Replace(expressionAux.Description, string.Empty);
                            result = result.Replace(expressionAux.Description.ToLower(), string.Empty);
                            result = result.Replace(expressionAux.Description.ToUpper(), string.Empty);

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
            if (!Directory.Exists(to))
            {
                Directory.CreateDirectory(to);
            }

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

                MoveElements(oldPath, newPath);
                Directory.Delete(oldPath);
            }
        }

        private static void MoveFiles(string from, string to)
        {
            string[] files = Directory.GetFiles(from);

            foreach (string file in files)
            {
                try
                {
                    if (File.Exists(string.Format(@"{0}\{1}", to, Path.GetFileName(file))))
                    {
                        FileInfo fileToReplace = new FileInfo(string.Format(@"{0}\{1}", to, Path.GetFileName(file)));

                        FileInfo fileFromPath = new FileInfo(string.Format(@"{0}", file));

                        //Verifica qual o arquivo mais atual
                        if (fileFromPath.LastWriteTime >= fileToReplace.LastWriteTime)
                        {
                            //Apaga o arquivo existente para incluir o arquivo com mesmo nome
                            File.Delete(string.Format(@"{0}\{1}", to, Path.GetFileName(file)));

                            //Insere o novo arquivo
                            File.Move(string.Format(@"{0}", file), string.Format(@"{0}\{1}", to, Path.GetFileName(file)));
                        }
                        else
                        {
                            //Apaga o arquivo da pasta corrente, pois não é o mais atual
                            File.Delete(string.Format(@"{0}", file));
                        }
                    }
                    else
                    {
                        File.Move(string.Format(@"{0}", file), string.Format(@"{0}\{1}", to, Path.GetFileName(file)));
                    }
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine(string.Format("Error moving file: {0} , Exception: {1}", file, ex.Message));
                }
            }

            #endregion
        }
    }
}
