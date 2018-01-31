using Alphaleonis.Win32.Filesystem;
using Core.Model;
using Core.Util.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Util.ShorterQueryFile
{
    public class NaiveShorterQueryFile : IShorterQueryFile
    {
        public void ListFoldersInfo(List<FolderInfo> foldersInfo, string projectPath)
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
    }
}
