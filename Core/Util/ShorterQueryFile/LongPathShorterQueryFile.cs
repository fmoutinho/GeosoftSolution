using Core.Model;
using Core.Util.Interface;
using System.Collections.Generic;
using Alphaleonis.Win32.Filesystem;
using System.Configuration;
using System.Linq;

namespace Core.Util.ShorterQueryFile
{
    public class LongPathShorterQueryFile : IShorterQueryFile

    {
        public void ListFoldersInfo(List<FolderInfo> foldersInfo, string path)
        {
            string[] directories = Directory.GetDirectories(path);

            string[] files = Directory.GetFiles(path);

            foreach (string directory in directories)
            {
                ListFoldersInfo(foldersInfo, directory, path.Length);
            }
        }

        private void ListFoldersInfo(List<FolderInfo> foldersInfo, string directory, int projectPathLength)
        {
            List<string> directories = Directory.GetDirectories(directory).ToList();

            List<string> files = Directory.GetFiles(directory).ToList();

            foreach (string auxDirectory in directories)
            {
                ListFoldersInfo(foldersInfo, auxDirectory, projectPathLength);
            }

            foreach (string file in files)
            {
                if (file.Length > int.Parse(ConfigurationManager.AppSettings["LONG_PATH_SIZE"]))
                {
                    FileInfo fileInfo = new FileInfo(file);
                    string[] directoriesFromLongPath = fileInfo.DirectoryName.Substring(projectPathLength).Split(Path.DirectorySeparatorChar);

                    foreach(string directoryFromLongPath in directoriesFromLongPath)
                    {
                        FolderInfo aux = foldersInfo.Where(x => x.Name.Equals(directoryFromLongPath)).FirstOrDefault();

                        if (aux != null)
                        {
                            aux.Quantity++;
                        }
                        else
                        {
                            foldersInfo.Add(new FolderInfo(directoryFromLongPath));
                        }
                    }
                }
            }
        }
    }
}

