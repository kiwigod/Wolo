using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoneymoonShop.Models
{
    public class Color
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ColorCode { get; set; }

        public List<DressColor> DressColors { get; set; }
    }
}
