using DataBaseAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace wloclawek_speedway_reservation.Styling.Controls
{
	/// <summary>
	/// Interaction logic for Control_ReservationCell.xaml
	/// </summary>
	public partial class Control_ReservationCell : UserControl
	{
		private ReservationViewModel _reservation;

		public ReservationViewModel ReservationView
		{
			get { return _reservation; }
		}

		public bool isReservation
		{
			get { return _reservation != null; }
		}

		public readonly DateTime date;

		public int row
		{
			get { return (int)this.GetValue(Grid.RowProperty); }
		}

		public int column
		{
			get { return (int)this.GetValue(Grid.ColumnProperty); }
		}

		public Control_ReservationCell(DateTime date)
		{
			InitializeComponent();
			this.date = date;
		}

		public void AddReservation(ReservationViewModel reservation)
		{
			_reservation = reservation;

			label_pesel.Content = string.Format("PESEL: {0}", reservation.client_pesel);

			label_cost.Content = string.Format("TOTAL COST: {0} PLN", reservation.total_price);

			label_pplCount.Content = string.Format("PEOPLE: {0}", reservation.people_count);
		}

		private void UserControl_MouseEnter(object sender, MouseEventArgs e)
		{
			if (date >= DateTime.Now || isReservation)
				this.Opacity = 0.6;
		}

		private void UserControl_MouseLeave(object sender, MouseEventArgs e)
		{
			this.Opacity = 1;
		}
	}
}