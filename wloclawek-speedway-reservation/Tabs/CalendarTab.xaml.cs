using DataBaseAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
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

namespace wloclawek_speedway_reservation.Tabs
{
	/// <summary>
	/// Interaction logic for CalendarTab.xaml
	/// </summary>
	public partial class CalendarTab : UserControl
	{
		private readonly List<string> workhours = new List<string>
		{
			"00:00","01:00","02:00","03:00","04:00","05:00","06:00","07:00","08:00","09:00","10:00","11:00","12:00","13:00","14:00","15:00","16:00","17:00","18:00","19:00","20:00","21:00","22:00","23:00","24:00",
		};

		private List<WorkdayModel> workdays;

		public CalendarTab()
		{
			InitializeComponent();

			WeekSelector.WeekChanged += WeekSelector_WeekChanged;

			foreach (var control in HourSetter.Children)
			{
				if (control.GetType() == typeof(ComboBox))
				{
					(control as ComboBox).ItemsSource = workhours;
					(control as ComboBox).SelectedIndex = 0;
				}
			}

			RefreshAll();
		}

		private void WeekSelector_WeekChanged(DateTime CurrentWeekStartDate, DateTime CurrentWeekEndDate)
		{
			Console.WriteLine("changed week");
			RefreshAll();
		}

		private void RefreshAll()
		{
			RefreshButtons();
			RefreshComboBoxes();
			InitConfirmButtons();
		}

		private void RefreshButtons()
		{
			MondayButton.Content = string.Format("MONDAY {0}", WeekSelector.MondayDate.ToShortDateString());
			if (WeekSelector.MondayDate.Date < DateTime.Now) MondayButton.IsEnabled = false;
			else MondayButton.IsEnabled = true;

			TuesdayButton.Content = string.Format("TUESDAY {0}", WeekSelector.TuesdayDate.ToShortDateString());
			if (WeekSelector.TuesdayDate.Date < DateTime.Now) TuesdayButton.IsEnabled = false;
			else TuesdayButton.IsEnabled = true;

			WedensdayButton.Content = string.Format("WEDENSDAY {0}", WeekSelector.WednesdayDate.ToShortDateString());
			if (WeekSelector.WednesdayDate.Date < DateTime.Now) WedensdayButton.IsEnabled = false;
			else WedensdayButton.IsEnabled = true;

			ThursdatButton.Content = string.Format("THURSDAY {0}", WeekSelector.ThursdayDate.ToShortDateString());
			if (WeekSelector.ThursdayDate.Date < DateTime.Now) ThursdatButton.IsEnabled = false;
			else ThursdatButton.IsEnabled = true;

			FridayButton.Content = string.Format("FRIDAY {0}", WeekSelector.FridayDate.ToShortDateString());
			if (WeekSelector.FridayDate.Date < DateTime.Now) FridayButton.IsEnabled = false;
			else FridayButton.IsEnabled = true;

			SaturdayButton.Content = string.Format("SATURDAY {0}", WeekSelector.SaturdayDate.ToShortDateString());
			if (WeekSelector.SaturdayDate.Date < DateTime.Now) SaturdayButton.IsEnabled = false;
			else SaturdayButton.IsEnabled = true;

			SundayButton.Content = string.Format("SUNDAY {0}", WeekSelector.SundayDate.ToShortDateString());
			if (WeekSelector.SundayDate.Date < DateTime.Now) SundayButton.IsEnabled = false;
			else SundayButton.IsEnabled = true;
		}

		private void RefreshComboBoxes()
		{
			workdays = DataBase.Access.LoadWorkdays(WeekSelector.CurrentWeekStartDate, WeekSelector.CurrentWeekEndDate);

			var tempWorkday = workdays.Find(x => x.date.Day == WeekSelector.MondayDate.Day);
			if (tempWorkday != null)
			{
				MondayStart.SelectedIndex = tempWorkday.hour_start;
				MondayEnd.SelectedIndex = tempWorkday.hour_end;

				MondayButton.Background = Brushes.Green;

				MondayButton.Tag = tempWorkday;
			}
			else
			{
				MondayStart.SelectedIndex = 0;
				MondayEnd.SelectedIndex = 0;

				MondayButton.Background = (Brush)Application.Current.FindResource("RacingRedBrush");

				MondayButton.Tag = null;
			}

			tempWorkday = workdays.Find(x => x.date.Day == WeekSelector.TuesdayDate.Day);
			if (tempWorkday != null)
			{
				TuesdayStart.SelectedIndex = tempWorkday.hour_start;
				TuesdayEnd.SelectedIndex = tempWorkday.hour_end;

				TuesdayButton.Background = Brushes.Green;

				TuesdayButton.Tag = tempWorkday;
			}
			else
			{
				TuesdayStart.SelectedIndex = 0;
				TuesdayEnd.SelectedIndex = 0;

				TuesdayButton.Background = (Brush)Application.Current.FindResource("RacingRedBrush");
				TuesdayButton.Tag = null;
			}

			tempWorkday = workdays.Find(x => x.date.Day == WeekSelector.WednesdayDate.Day);
			if (tempWorkday != null)
			{
				WedensdayStart.SelectedIndex = tempWorkday.hour_start;
				WedensdayEnd.SelectedIndex = tempWorkday.hour_end;

				WedensdayButton.Background = Brushes.Green;
				WedensdayButton.Tag = tempWorkday;
			}
			else
			{
				WedensdayStart.SelectedIndex = 0;
				WedensdayEnd.SelectedIndex = 0;

				WedensdayButton.Background = (Brush)Application.Current.FindResource("RacingRedBrush");
				WedensdayButton.Tag = null;
			}

			tempWorkday = workdays.Find(x => x.date.Day == WeekSelector.ThursdayDate.Day);
			if (tempWorkday != null)
			{
				ThursdayStart.SelectedIndex = tempWorkday.hour_start;
				ThursdayEnd.SelectedIndex = tempWorkday.hour_end;

				ThursdatButton.Background = Brushes.Green;
				ThursdatButton.Tag = tempWorkday;
			}
			else
			{
				ThursdayStart.SelectedIndex = 0;
				ThursdayEnd.SelectedIndex = 0;

				ThursdatButton.Background = (Brush)Application.Current.FindResource("RacingRedBrush");
				ThursdatButton.Tag = null;
			}

			tempWorkday = workdays.Find(x => x.date.Day == WeekSelector.FridayDate.Day);
			if (tempWorkday != null)
			{
				FridayStart.SelectedIndex = tempWorkday.hour_start;
				FridayEnd.SelectedIndex = tempWorkday.hour_end;

				FridayButton.Background = Brushes.Green;
				FridayButton.Tag = tempWorkday;
			}
			else
			{
				FridayStart.SelectedIndex = 0;
				FridayEnd.SelectedIndex = 0;

				FridayButton.Background = (Brush)Application.Current.FindResource("RacingRedBrush");
				FridayButton.Tag = null;
			}

			tempWorkday = workdays.Find(x => x.date.Day == WeekSelector.SaturdayDate.Day);
			if (tempWorkday != null)
			{
				SaturdayStart.SelectedIndex = tempWorkday.hour_start;
				SaturdayEnd.SelectedIndex = tempWorkday.hour_end;

				SaturdayButton.Background = Brushes.Green;
				SaturdayButton.Tag = tempWorkday;
			}
			else
			{
				SaturdayStart.SelectedIndex = 0;
				SaturdayEnd.SelectedIndex = 0;

				SaturdayButton.Background = (Brush)Application.Current.FindResource("RacingRedBrush");
				SaturdayButton.Tag = null;
			}

			tempWorkday = workdays.Find(x => x.date.Day == WeekSelector.SundayDate.Day);
			if (tempWorkday != null)
			{
				SundayStart.SelectedIndex = tempWorkday.hour_start;
				SundayEnd.SelectedIndex = tempWorkday.hour_end;

				SundayButton.Background = Brushes.Green;
				SundayButton.Tag = tempWorkday;
			}
			else
			{
				SundayStart.SelectedIndex = 0;
				SundayEnd.SelectedIndex = 0;

				SundayButton.Background = (Brush)Application.Current.FindResource("RacingRedBrush");
				SundayButton.Tag = null;
			}
		}

		private void InitConfirmButtons()
		{
			List<ReservationViewModel> reservations = DataBase.Access.LoadReservationsView(WeekSelector.CurrentWeekStartDate, WeekSelector.CurrentWeekEndDate).FindAll(X => X.is_canceled == false);

			MondayConfirm.IsEnabled = false;
			if (reservations.Exists(x => x.date.Day == WeekSelector.MondayDate.Day) || MondayButton.IsEnabled == false)
			{
				MondayConfirm.Visibility = Visibility.Hidden;
				MondayStart.IsEnabled = false;
				MondayEnd.IsEnabled = false;
			}
			else
			{
				MondayConfirm.Visibility = Visibility.Visible;
				MondayStart.IsEnabled = true;
				MondayEnd.IsEnabled = true;
			}

			TuesdayConfirm.IsEnabled = false;
			if (reservations.Exists(x => x.date.Day == WeekSelector.TuesdayDate.Day) || TuesdayButton.IsEnabled == false)
			{
				TuesdayConfirm.Visibility = Visibility.Hidden;
				TuesdayStart.IsEnabled = false;
				TuesdayEnd.IsEnabled = false;
			}
			else
			{
				TuesdayConfirm.Visibility = Visibility.Visible;
				TuesdayStart.IsEnabled = true;
				TuesdayEnd.IsEnabled = true;
			}

			WedensdayConfirm.IsEnabled = false;
			if (reservations.Exists(x => x.date.Day == WeekSelector.WednesdayDate.Day) || WedensdayButton.IsEnabled == false)
			{
				WedensdayConfirm.Visibility = Visibility.Hidden;
				WedensdayStart.IsEnabled = false;
				WedensdayEnd.IsEnabled = false;
			}
			else
			{
				WedensdayConfirm.Visibility = Visibility.Visible;
				WedensdayStart.IsEnabled = true;
				WedensdayEnd.IsEnabled = true;
			}

			ThursdayConfirm.IsEnabled = false;
			if (reservations.Exists(x => x.date.Day == WeekSelector.ThursdayDate.Day) || ThursdatButton.IsEnabled == false)
			{
				ThursdayConfirm.Visibility = Visibility.Hidden;
				ThursdayStart.IsEnabled = false;
				ThursdayEnd.IsEnabled = false;
			}
			else
			{
				ThursdayConfirm.Visibility = Visibility.Visible;
				ThursdayStart.IsEnabled = true;
				ThursdayEnd.IsEnabled = true;
			}

			FridayConfirm.IsEnabled = false;
			if (reservations.Exists(x => x.date.Day == WeekSelector.FridayDate.Day) || FridayButton.IsEnabled == false)
			{
				FridayConfirm.Visibility = Visibility.Hidden;
				FridayStart.IsEnabled = false;
				FridayEnd.IsEnabled = false;
			}
			else
			{
				FridayConfirm.Visibility = Visibility.Visible;
				FridayStart.IsEnabled = true;
				FridayEnd.IsEnabled = true;
			}

			SaturdayConfirm.IsEnabled = false;
			if (reservations.Exists(x => x.date.Day == WeekSelector.SaturdayDate.Day) || SaturdayButton.IsEnabled == false)
			{
				SaturdayConfirm.Visibility = Visibility.Hidden;
				SaturdayStart.IsEnabled = false;
				SaturdayEnd.IsEnabled = false;
			}
			else
			{
				SaturdayConfirm.Visibility = Visibility.Visible;
				SaturdayStart.IsEnabled = true;
				SaturdayEnd.IsEnabled = true;
			}

			SundayConfirm.IsEnabled = false;
			if (reservations.Exists(x => x.date.Day == WeekSelector.SundayDate.Day) || SundayButton.IsEnabled == false)
			{
				SundayConfirm.Visibility = Visibility.Hidden;
				SundayStart.IsEnabled = false;
				SundayEnd.IsEnabled = false;
			}
			else
			{
				SundayConfirm.Visibility = Visibility.Visible;
				SundayStart.IsEnabled = true;
				SundayEnd.IsEnabled = true;
			}
		}

		private void ValidateRow(Button main, ComboBox start, ComboBox end, Button confirm)
		{
			if ((main.Background == Brushes.Green && end.SelectedIndex > start.SelectedIndex))
			{
				confirm.IsEnabled = true;
			}
			else
			{
				confirm.IsEnabled = false;
			}
		}

		private void DateButtons_Click(object sender, RoutedEventArgs e)
		{
			int currentRow = Grid.GetRow(sender as Button);

			Button main = (sender as Button);
			ComboBox comboStart = HourSetter.Children.Cast<UIElement>().First(x => Grid.GetRow(x) == currentRow && Grid.GetColumn(x) == 3) as ComboBox;
			ComboBox comboEnd = HourSetter.Children.Cast<UIElement>().First(x => Grid.GetRow(x) == currentRow && Grid.GetColumn(x) == 6) as ComboBox;
			Button confirm = HourSetter.Children.Cast<UIElement>().First(x => Grid.GetRow(x) == currentRow && Grid.GetColumn(x) == 8) as Button;

			if (main.Background == (Brush)Application.Current.FindResource("RacingRedBrush"))
			{
				main.Background = Brushes.Green;
				ValidateRow(main, comboStart, comboEnd, confirm);
			}
			else
			{
				List<ReservationViewModel> reservations;

				switch (currentRow)
				{
					case 0: reservations = DataBase.Access.LoadReservationsView(WeekSelector.MondayDate, WeekSelector.MondayDate).FindAll(x => x.is_canceled == false); break;
					case 1: reservations = DataBase.Access.LoadReservationsView(WeekSelector.TuesdayDate, WeekSelector.TuesdayDate).FindAll(x => x.is_canceled == false); break;
					case 2: reservations = DataBase.Access.LoadReservationsView(WeekSelector.WednesdayDate, WeekSelector.WednesdayDate).FindAll(x => x.is_canceled == false); break;
					case 3: reservations = DataBase.Access.LoadReservationsView(WeekSelector.ThursdayDate, WeekSelector.ThursdayDate).FindAll(x => x.is_canceled == false); break;
					case 4: reservations = DataBase.Access.LoadReservationsView(WeekSelector.FridayDate, WeekSelector.FridayDate).FindAll(x => x.is_canceled == false); break;
					case 5: reservations = DataBase.Access.LoadReservationsView(WeekSelector.SaturdayDate, WeekSelector.SaturdayDate).FindAll(x => x.is_canceled == false); break;
					case 6: reservations = DataBase.Access.LoadReservationsView(WeekSelector.SundayDate, WeekSelector.SundayDate).FindAll(x => x.is_canceled == false); break;
					default: reservations = new List<ReservationViewModel>(); break;
				}

				if (reservations.Count == 0)
				{
					main.Background = (Brush)Application.Current.FindResource("RacingRedBrush");

					comboStart.SelectedIndex = 0;
					comboEnd.SelectedIndex = 0;

					if ((main.Tag != null))
					{
						confirm.IsEnabled = true;
					}
				}
			}
		}

		private void StartComboBoxes_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			int currentRow = Grid.GetRow(sender as ComboBox);

			Button main = HourSetter.Children.Cast<UIElement>().First(x => Grid.GetRow(x) == currentRow && Grid.GetColumn(x) == 0) as Button;
			ComboBox comboStart = sender as ComboBox;
			ComboBox comboEnd = HourSetter.Children.Cast<UIElement>().First(x => Grid.GetRow(x) == currentRow && Grid.GetColumn(x) == 6) as ComboBox;
			Button confirm = HourSetter.Children.Cast<UIElement>().First(x => Grid.GetRow(x) == currentRow && Grid.GetColumn(x) == 8) as Button;

			ValidateRow(main, comboStart, comboEnd, confirm);
		}

		private void EndComboBoxes_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			int currentRow = Grid.GetRow(sender as ComboBox);

			Button main = HourSetter.Children.Cast<UIElement>().First(x => Grid.GetRow(x) == currentRow && Grid.GetColumn(x) == 0) as Button;
			ComboBox comboStart = HourSetter.Children.Cast<UIElement>().First(x => Grid.GetRow(x) == currentRow && Grid.GetColumn(x) == 3) as ComboBox;
			ComboBox comboEnd = sender as ComboBox;
			Button confirm = HourSetter.Children.Cast<UIElement>().First(x => Grid.GetRow(x) == currentRow && Grid.GetColumn(x) == 8) as Button;

			ValidateRow(main, comboStart, comboEnd, confirm);
		}

		private void ConfirmButtons_Click(object sender, RoutedEventArgs e)
		{
			//push workday to db here
			int currentRow = Grid.GetRow(sender as Button);

			Button main = HourSetter.Children.Cast<UIElement>().First(x => Grid.GetRow(x) == currentRow && Grid.GetColumn(x) == 0) as Button;
			ComboBox comboStart = HourSetter.Children.Cast<UIElement>().First(x => Grid.GetRow(x) == currentRow && Grid.GetColumn(x) == 3) as ComboBox;
			ComboBox comboEnd = HourSetter.Children.Cast<UIElement>().First(x => Grid.GetRow(x) == currentRow && Grid.GetColumn(x) == 6) as ComboBox;
			Button confirm = sender as Button;

			DateTime date;

			switch (currentRow)
			{
				case 0: date = WeekSelector.MondayDate; break;
				case 1: date = WeekSelector.TuesdayDate; break;
				case 2: date = WeekSelector.WednesdayDate; break;
				case 3: date = WeekSelector.ThursdayDate; break;
				case 4: date = WeekSelector.FridayDate; break;
				case 5: date = WeekSelector.SaturdayDate; break;
				case 6: date = WeekSelector.SundayDate; break;
				default: /* notification here */ return;
			}

			if (main.Background == (Brush)Application.Current.FindResource("RacingRedBrush") && main.Tag != null)
			{
				//delete
				DataBase.Access.DeleteWorkday((WorkdayModel)main.Tag);
			}
			else if (main.Background == Brushes.Green)
			{
				if (main.Tag == null)
				{
					//add
					DataBase.Access.AddWorkday(new WorkdayModel
					{
						date = date,
						hour_start = comboStart.SelectedIndex,
						hour_end = comboEnd.SelectedIndex
					});
				}
				else
				{
					//update
					DataBase.Access.UpdateWorkday(new WorkdayModel
					{
						id = ((WorkdayModel)main.Tag).id,
						date = ((WorkdayModel)main.Tag).date,
						hour_start = comboStart.SelectedIndex,
						hour_end = comboEnd.SelectedIndex
					});
				}
			}

			(sender as Button).IsEnabled = false;

			RefreshAll();
		}
	}
}