using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoneymoonShop.Models
{
    public class DressFeature
    {
        public int DressID { get; set; }
        public Dress Dress { get; set; }

        public int FeatureID { get; set; }
        public Feature Feature { get; set; }
    }
}
