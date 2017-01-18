using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoneymoonShop.Models
{
    public class SuitColor
    {
        public int SuitID { get; set; }
        public Suit Suit { get; set; }

        public int ColorID { get; set; }
        public Color Color { get; set; }
    }
}
