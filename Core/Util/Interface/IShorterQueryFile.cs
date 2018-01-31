using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Util.Interface
{
    public interface IShorterQueryFile
    {
        void ListFoldersInfo(List<FolderInfo> foldersInfo, string projectPath);
    }
}
