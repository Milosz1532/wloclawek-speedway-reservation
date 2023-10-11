using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseAccess.Models
{
	public class ReservationModel
	{
		public int id { get; set; }
		public string client_pesel { get; set; }
		public DateTime date { get; set; }
		public int hour_start { get; set; }
		public int hour_end { get; set; }
		public int people_count { get; set; }
		public int offer_id { get; set; }
		public bool is_canceled { get; set; }
	}
}