using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoneymoonShop.Models
{
    public class Style
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public List<Dress> Dresses { get; set; }
    }
}
