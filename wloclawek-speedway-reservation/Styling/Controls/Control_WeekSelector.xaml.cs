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
	/// Interaction logic for WeekSelector.xaml
	/// </summary>
	public partial class Control_WeekSelector : UserControl
	{
		public DateTime CurrentDate { get; private set; }

		public DateTime CurrentWeekStartDate { get; private set; }
		public DateTime CurrentWeekEndDate { get; private set; }

		public event SelectedWeekChanged WeekChanged;

		public delegate void SelectedWeekChanged(DateTime CurrentWeekStartDate, DateTime CurrentWeekEndDate);

		public DateTime MondayDate
		{ get { return CurrentWeekStartDate; } }

		public DateTime TuesdayDate
		{ get { return CurrentWeekStartDate.AddDays(1); } }

		public DateTime WednesdayDate
		{ get { return CurrentWeekStartDate.AddDays(2); } }

		public DateTime ThursdayDate
		{ get { return CurrentWeekStartDate.AddDays(3); } }

		public DateTime FridayDate
		{ get { return CurrentWeekStartDate.AddDays(4); } }

		public DateTime SaturdayDate
		{ get { return CurrentWeekStartDate.AddDays(5); } }

		public DateTime SundayDate
		{ get { return CurrentWeekStartDate.AddDays(6); } }

		public Control_WeekSelector()
		{
			InitializeComponent();

			CurrentDate = DateTime.Now;

			Refresh();
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			CurrentDate = CurrentDate.AddDays(7);
			Refresh();

			WeekChanged?.Invoke(CurrentWeekStartDate, CurrentWeekEndDate);
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			CurrentDate = CurrentDate.AddDays(-7);
			Refresh();

			WeekChanged?.Invoke(CurrentWeekStartDate, CurrentWeekEndDate);
		}

		private void Refresh()
		{
			int offset;

			if (CurrentDate.DayOfWeek == DayOfWeek.Monday)
			{
				offset = 0;
			}
			else if (CurrentDate.DayOfWeek == DayOfWeek.Tuesday)
			{
				offset = -1;
			}
			else if (CurrentDate.DayOfWeek == DayOfWeek.Wednesday)
			{
				offset = -2;
			}
			else if (CurrentDate.DayOfWeek == DayOfWeek.Thursday)
			{
				offset = -3;
			}
			else if (CurrentDate.DayOfWeek == DayOfWeek.Friday)
			{
				offset = -4;
			}
			else if (CurrentDate.DayOfWeek == DayOfWeek.Saturday)
			{
				offset = -5;
			}
			else if (CurrentDate.DayOfWeek == DayOfWeek.Sunday)
			{
				offset = -6;
			}
			else
			{
				throw new Exception("Error initiating grid");
			}

			CurrentWeekStartDate = CurrentDate.AddDays(0 + offset);
			CurrentWeekEndDate = CurrentDate.AddDays(6 + offset);

			DateRange.Content = string.Format("{0} - {1}", CurrentWeekStartDate.ToShortDateString(), CurrentWeekEndDate.ToShortDateString());
		}
	}
}