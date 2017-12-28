using Core.Service.FileZipperService;
using Core.Service.FileZipperService.Zippers;

namespace FileZipper
{
    class Program
    {
        static void Main(string[] args)
        {
            FileZipperService fileZipperService = new FileZipperService(new DotNetZipZipper());

            fileZipperService.ZipProjects();
        }
    }
}
