using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseAccess.Models
{
    public class CarModel
    {
        public int id { get; set; }
        public string model { get; set; }
        public int production_year { get; set; }
        public string engine_type { get; set; }
        public byte[] image { get; set; }
        public int power { get; set; }
        public string drive { get; set; }
        public string transmission { get; set; }
        public int top_speed { get; set; }
        public double zero_to_hundred { get; set; }
        public bool is_available { get; set; }
        public double rent_price { get; set; }
    }
}