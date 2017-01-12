using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoneymoonShop.Models
{
    public class Appointment
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public DateTime MDate { get; set; }
        public string PNumber { get; set; }
        public string Mail { get; set; }
        public bool newsletter { get; set; }
    }
}
