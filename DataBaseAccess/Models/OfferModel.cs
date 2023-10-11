using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseAccess.Models
{
	public class OfferModel
	{
		public int id { get; set; }
		public string name { get; set; }
		public double base_hour_price { get; set; }
		public bool is_available { get; set; }

        public override string ToString()
        {
			return name;
        }
    }
}