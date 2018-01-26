using Core.Service.FolderRenameService;
using Core.Util;
using System.Configuration;
using System.Windows.Forms;

namespace FolderRenamer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (MessageBox.Show("Create a query file with all folders' names to edit? ", "FolderRenamer", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                Util.CreateShortQueryFile();
            }

            if (MessageBox.Show("Procceed shortening file's paths?", "FolderRenamer", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                FolderRenamerService.Short();
            }
        }
    }
}
