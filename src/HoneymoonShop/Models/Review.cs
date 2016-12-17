using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoneymoonShop.Models
{
    public class Review
    {
        public int ID { get; set; }
        public int Rating { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        public string Description { get; set; }
    }
}
