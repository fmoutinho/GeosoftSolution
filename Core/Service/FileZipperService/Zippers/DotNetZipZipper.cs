using Alphaleonis.Win32.Filesystem;
using Core.Service.FileZipperService.Interfaces;
using Core.Util;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Core.Service.FileZipperService.Zippers
{
    public class DotNetZipZipper : Zipper
    {
        public void ZipProject(string path)
        {
            string destinyFileName = string.Format(@"{0}\{1}.zip", ConfigurationManager.AppSettings["DESTINY_FOLDER"], Path.GetFileName(path));
            using (ZipFile zip = new ZipFile())
            {
                zip.AddDirectory(string.Format(@"{0}", path));

                zip.Comment = "This zip was created at " + System.DateTime.Now.ToString("G");

                zip.MaxOutputSegmentSize = int.MaxValue / 2;

                zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;

                zip.Save(string.Format(@"{0}\{1}.zip", ConfigurationManager.AppSettings["DESTINY_FOLDER"], Path.GetFileName(path)));

                if (zip.NumberOfSegmentsForMostRecentSave > 1)
                {
                    List<string> filesToDelete = Directory.GetFiles(string.Format(@"{0}", ConfigurationManager.AppSettings["DESTINY_FOLDER"])).ToList();

                    filesToDelete.ForEach(x =>
                    {
                        if (x.Contains(Path.GetFileName(path)))
                        {

                            File.Delete(x);
                        };
                    });

                    throw new Exception("Project will need more than one splitted file");
                }
            }
        }
    }
}
