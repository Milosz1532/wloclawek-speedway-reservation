using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseAccess.Models
{
	public class ReservationViewModel
	{
		public int reservation_id { get; set; }
		public string client_pesel { get; set; }
		public DateTime date { get; set; }
		public int hour_start { get; set; }
		public int hour_end { get; set; }
		public int hours_total { get; set; }
		public int people_count { get; set; }
		public int offer_id { get; set; }
		public double base_hour_price { get; set; }
		public double cars_total_price_per_hour { get; set; }
		public double total_price { get; set; }
		public bool is_canceled { get; set; }
	}
}