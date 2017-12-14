using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFramework.Extensions;
using System.Threading;

namespace CheckRotFiles
{
    class Program
    {

        static void Main(string[] args)
        {
            GeodatascanBHP_Entities context = new GeodatascanBHP_Entities();
            List<int> listDatasets = context.TempROT99.Where(x => x.FileExists == null)
                     .Select(x => x.DataSetId).ToList();

            int total = listDatasets.Count();
            int quantidade = 0;

            foreach (int dataset in listDatasets)
            {
                TempROT99 aux = context.TempROT99.Where(x => x.DataSetId == dataset).First();
                System.Console.WriteLine("Processando {0} de {1}",++quantidade, total);
                aux.FileExists = System.IO.File.Exists(aux.CompleteName);
                context.TempROT99.Update(tr => tr.DataSetId == aux.DataSetId, tr => new TempROT99 { FileExists = aux.FileExists });
            }
        }
    }
}
