using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoneymoonShop.Models
{
    public class Feature
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public List<DressFeature> DressFeatures { get; set; }
    }
}
