using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFramework.Extensions;

namespace CheckRotFiles
{
    class Program
    {

        static void Main(string[] args)
        {
            //GeodatascanBHP_Entities context = new GeodatascanBHP_Entities();

            //List<TempROT99> TempROT99s = context.TempROT99.ToList();

            //foreach (TempROT99 aux in TempROT99s)
            //{
            //    if (System.IO.File.Exists(aux.CompleteName))
            //    {
            //        aux.FileExists = true;

            //        //System.Console.WriteLine(string.Format("Arquivo {0} encontrado.", aux.CompleteName));
            //    }
            //    else
            //    {
            //        aux.FileExists = false;
            //        //System.Console.WriteLine(string.Format("Arquivo {0} não encontrado.", aux.CompleteName));
            //    }

            //    context.Entry(aux).State = System.Data.Entity.EntityState.Modified;
            //    context.SaveChanges();

            //}


            using (GeodatascanBHP_Entities context = new GeodatascanBHP_Entities())
            {
                int quant = 0;
                int total = context.TempROT99.Where(x => x.FileExists == null).Count();
                foreach (TempROT99 aux in context.TempROT99.Where(x=> x.FileExists ==null))
                {
                    System.Console.WriteLine(string.Format("Processando {0} de {1}", ++quant, total));

                    aux.FileExists = System.IO.File.Exists(aux.CompleteName);
                    context.TempROT99.Update(tr => tr.DataSetId == aux.DataSetId, tr => new TempROT99 { FileExists = aux.FileExists });
                }
            }
        }
    }
}
