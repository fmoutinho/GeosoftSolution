using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class FolderInfo
    {
        public FolderInfo(string Name)
        {
            this.Name = Name;
            this.Quantity = 0;
        }

        public string Name { get; set; }

        public int Quantity { get; set; }
    }
}
