using Alphaleonis.Win32.Filesystem;
using Core.Service.FileZipperService.Interfaces;
using Core.Util;
using Ionic.Zip;
using System.Configuration;

namespace Core.Service.FileZipperService.Zippers
{
    public class DotNetZipZipper : Zipper
    {
        public void ZipProject(string path)
        {
            using (ZipFile zip = new ZipFile())
            {
                zip.AddDirectory(string.Format(@"{0}", path));

                zip.Comment = "This zip was created at " + System.DateTime.Now.ToString("G");

                zip.MaxOutputSegmentSize = Constants.ONE_GIGABYTE_SIZE * int.Parse(ConfigurationManager.AppSettings["SEGMENT_SIZE"]);

                zip.Save(string.Format(@"{0}\{1}.zip", ConfigurationManager.AppSettings["DESTINY_FOLDER"], Path.GetFileName(path)));
            }
        }
    }
}
