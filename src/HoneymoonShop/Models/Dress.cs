using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoneymoonShop.Models
{
    public class Dress
    {
        public int ID { get; set; }
        public int Price { get; set; }
        public int StyleID { get; set; }
        public Style Style { get; set; }
        public int NecklineID { get; set; }
        public Neckline Neckline { get; set; }
        public int SilhouetteID { get; set; }
        public Silhouette Silhouette { get; set; }
        public int ColorID { get; set; }
        public Color Color { get; set; }
        public int ManuID { get; set; }
        public Manu Manu { get; set; }
        public List<DressFeature> DressFeatures { get; set; }
    }
}
