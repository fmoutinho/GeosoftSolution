using Core.Service.FileZipperService;
using Core.Service.FileZipperService.Zippers;
using Core.Util;
using Core.Util.Log;
using System.Configuration;
using System.Windows.Forms;

namespace FileZipper
{
    class Program
    {
        static void Main(string[] args)
        {
            Log log = new Log();
            FileZipperService fileZipperService = new FileZipperService(new ShellZipper(), log);

            Util.CreateBatchFile();

            DialogResult dr = MessageBox.Show("A batch file with all zip commands has been created. Proceed?", "FileZipper", MessageBoxButtons.YesNoCancel,
               MessageBoxIcon.Information);

            if (dr == DialogResult.Yes)
            {
                fileZipperService.ZipProjects();
            }
        }
    }
}
