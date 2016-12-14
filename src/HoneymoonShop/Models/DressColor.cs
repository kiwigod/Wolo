using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoneymoonShop.Models
{
    public class DressColor
    {
        public int DressID { get; set; }
        public Dress Dress { get; set; }

        public int ColorID { get; set; }
        public Color Color { get; set; }
    }
}
