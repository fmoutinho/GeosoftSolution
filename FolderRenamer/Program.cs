using Core.Service.FolderRenameService;
using Core.Util.Log;

namespace FolderRenamer
{
    class Program
    {
        static void Main(string[] args)
        {
            Log log = new Log();
            FolderRenamerService.Short(log);
        }
    }
}
