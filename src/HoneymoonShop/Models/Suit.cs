using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoneymoonShop.Models
{
    public class Suit
    {
        public int ID { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public int CategoryID { get; set; }
        public Category Category { get; set; }
        public int StyleID { get; set; }
        public Style Style { get; set; }
        public List<SuitColor> SuitColors { get; set; }
        public int ManuID { get; set; }
        public Manu Manu { get; set; }
        public List<SuitFeature> SuitFeatures { get; set; }
    }
}
