using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoneymoonShop.Models
{
    public class Blog
    {
        public Blog()
        {
            Posts = new List<Post>();
        }
        public int ID { get; set; }
        public string Type { get; set; }

        public List<Post> Posts { get; set; }
    }
}
