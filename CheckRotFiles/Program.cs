using Core.Model;
using System.Collections.Generic;
using System.Linq;
using EntityFramework.Extensions;

namespace CheckRotFiles
{
    class Program
    {

        static void Main(string[] args)
        {
            GeodatascanBHP_Entities context = new GeodatascanBHP_Entities();
            List<int> listDatasets = context.TempROT99
                     .Select(x => x.DataSetId).ToList();

            int total = listDatasets.Count();
            int quantidade = 0;

            foreach (int dataset in listDatasets)
            {
                TempROT99 aux = context.TempROT99.Where(x => x.DataSetId == dataset).First();
                System.Console.WriteLine("Processando {0} DatasetId {1}", ++quantidade, total);
                aux.FileExists = Alphaleonis.Win32.Filesystem.File.Exists(aux.CompleteName);
                context.TempROT99.Update(tr => tr.DataSetId == aux.DataSetId, tr => new TempROT99 { FileExists = aux.FileExists });
            }
        }
    }
}
