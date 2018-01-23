using Core.Service.FileZipperService;
using Core.Service.FileZipperService.Zippers;
using Core.Util;
using System.Configuration;
using System.Windows.Forms;

namespace FileZipper
{
    class Program
    {
        static void Main(string[] args)
        {
            FileZipperService fileZipperService = new FileZipperService(new ShellZipper());

            Util.CreateBatchFile();

            DialogResult dr = MessageBox.Show(string.Format("A batch file with all zip commands has been created into {0}. Proceed?", ConfigurationManager.AppSettings["BATCH_PATH"]), "FileZipper", MessageBoxButtons.YesNoCancel,
               MessageBoxIcon.Information);

            if (dr == DialogResult.Yes)
            {
                fileZipperService.ZipProjects();
            }
        }
    }
}
