using Core.Service.FileZipperService;
using Core.Service.FileZipperService.Zippers;
using Core.Util.Log;

namespace FileZipper
{
    class Program
    {
        static void Main(string[] args)
        {
            Log log = new Log();
            FileZipperService fileZipperService = new FileZipperService(new ShellZipper(), log);

            fileZipperService.ZipProjects();
        }
    }
}
