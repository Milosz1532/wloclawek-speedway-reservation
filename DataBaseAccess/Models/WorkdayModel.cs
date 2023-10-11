using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseAccess.Models
{
	public class WorkdayModel
	{
		public int id { get; set; }
		public DateTime date { get; set; }
		public int hour_start { get; set; }
		public int hour_end { get; set; }
	}
}