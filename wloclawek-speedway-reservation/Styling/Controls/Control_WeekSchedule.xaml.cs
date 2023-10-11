using DataBaseAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using wloclawek_speedway_reservation.Tabs;

namespace wloclawek_speedway_reservation.Styling.Controls
{
	/// <summary>
	/// Interaction logic for Control_WeekSchedule.xaml
	/// </summary>
	public partial class Control_WeekSchedule : UserControl
	{
		private List<WorkdayModel> workdays;
		private List<ReservationViewModel> reservations;

		public Control_ReservationCell[,] gridCells = new Control_ReservationCell[7, 24];

		private int _selectingColumn = -1;

		private int _firstSelection;
		private List<Control_ReservationCell> _selectedCells;

		public Control_WeekSchedule()
		{
			InitializeComponent();

			WeekSelector.WeekChanged += WeekSelector_WeekChanged;

			RefreshGrid();
		}

		private void WeekSelector_WeekChanged(DateTime CurrentWeekStartDate, DateTime CurrentWeekEndDate)
		{
			RefreshGrid();
		}

		private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (((Control_ReservationCell)sender).isReservation == false)
			{
				if ((sender as Control_ReservationCell).date >= DateTime.Now)
				{
					((Control_ReservationCell)sender).MainBorder.Background = Brushes.Green;

					_firstSelection = (sender as Control_ReservationCell).row;

					_selectingColumn = (sender as Control_ReservationCell).column;

					_selectedCells = new List<Control_ReservationCell>
					{
						sender as Control_ReservationCell
					};
				}
			}
			else
			{
				ReservationViewModel reservation = (sender as Control_ReservationCell).ReservationView;

				ReservationModel editReservation = new ReservationModel
				{
					id = reservation.reservation_id,
					client_pesel = reservation.client_pesel,
					date = reservation.date,
					hour_start = reservation.hour_start,
					hour_end = reservation.hour_end,
					people_count = reservation.people_count,
					offer_id = reservation.offer_id,
					is_canceled = reservation.is_canceled,
				};
				AddReservationWindow window = new AddReservationWindow(editReservation);
				window.ShowDialog();
				if (window.DialogResult == false)
				{
					RefreshGridCellsContent();
				}
			}
		}

		private void GridCell_MouseEnter(object sender, MouseEventArgs e)
		{
			/*last check in the if below is optional, dunno if to keep it*/
			if (Mouse.LeftButton == MouseButtonState.Pressed && _selectingColumn != -1 && (sender as Control_ReservationCell).column == _selectingColumn)
			{
				if ((sender as Control_ReservationCell).isReservation == true || (sender as Control_ReservationCell).date < DateTime.Now)
					return;

				_selectedCells.Add(sender as Control_ReservationCell);
				_selectedCells = _selectedCells.OrderBy(x => x.row).ToList();

				foreach (Control_ReservationCell control in _selectedCells)
				{
					control.MainBorder.Background = (Brush)Application.Current.FindResource("RacingRedBrush");
				}

				_selectedCells.Clear();

				bool foundReserved = false;

				if ((sender as Control_ReservationCell).row > _firstSelection)
				{
					for (int i = _firstSelection; i <= (sender as Control_ReservationCell).row; i++)
					{
						if (gridCells[_selectingColumn - 1, i] != null)
						{
							if (gridCells[_selectingColumn - 1, i].isReservation == false)
							{
								if (foundReserved == false)
								{
									gridCells[_selectingColumn - 1, i].MainBorder.Background = Brushes.Green;
									_selectedCells.Add(gridCells[_selectingColumn - 1, i]);
								}
							}
							else
							{
								foundReserved = true;
								break;
							}
						}
					}
				}
				else
				{
					for (int i = _firstSelection; i >= (sender as Control_ReservationCell).row; i--)
					{
						if (gridCells[_selectingColumn - 1, i] != null)
						{
							if (gridCells[_selectingColumn - 1, i].isReservation == false)
							{
								if (foundReserved == false)
								{
									gridCells[_selectingColumn - 1, i].MainBorder.Background = Brushes.Green;
									_selectedCells.Add(gridCells[_selectingColumn - 1, i]);
								}
							}
							else
							{
								foundReserved = true;
								break;
							}
						}
					}
				}
			}
		}

		private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (_selectingColumn != -1)
			{
				_selectedCells = _selectedCells.OrderBy(x => x.row).ToList();

				ReservationModel newReservation = new ReservationModel
				{
					hour_start = _selectedCells.First().row,
					hour_end = _selectedCells.Last().row + 1,
					date = _selectedCells.First().date
				};

				AddReservationWindow window = new AddReservationWindow(newReservation);
				window.ShowDialog();

				_selectingColumn = -1;
				_firstSelection = -1;
				RefreshGrid();
			}
		}

		public void RefreshGrid()
		{
			RefreshGridHeaders();

			RefreshGridCellsContent();
		}

		private void RefreshGridHeaders()
		{
			Date1.Content = WeekSelector.MondayDate.ToShortDateString();
			Date2.Content = WeekSelector.TuesdayDate.ToShortDateString();
			Date3.Content = WeekSelector.WednesdayDate.ToShortDateString();
			Date4.Content = WeekSelector.ThursdayDate.ToShortDateString();
			Date5.Content = WeekSelector.FridayDate.ToShortDateString();
			Date6.Content = WeekSelector.SaturdayDate.ToShortDateString();
			Date7.Content = WeekSelector.SundayDate.ToShortDateString();
		}

		public void RefreshGridCellsContent()
		{
			foreach (Control_ReservationCell cell in gridCells)
			{
				if (cell != null)
					MainGrid.Children.Remove(cell);
			}
			Array.Clear(gridCells, 0, 7 * 24);

			FillWorkdays();

			FillReservations();
		}

		private void FillReservations()
		{
			reservations = DataBase.Access.LoadReservationsView(WeekSelector.CurrentWeekStartDate, WeekSelector.CurrentWeekEndDate);

			foreach (ReservationViewModel reservation in reservations)
			{
				if (reservation.is_canceled == false)
				{
					int day;
					if ((int)reservation.date.DayOfWeek - 1 == -1) day = 6;
                    else day = (int)reservation.date.DayOfWeek - 1;
                    int hours = reservation.hour_end - reservation.hour_start;

                    for (int i = reservation.hour_start + 1; i < reservation.hour_end; i++)
					{
						MainGrid.Children.Remove(gridCells[day, i]);
						gridCells[day, i] = null;
					}

					Grid.SetRowSpan(gridCells[day, reservation.hour_start], hours);
					gridCells[day, reservation.hour_start].MainBorder.Background = Brushes.LimeGreen;

					gridCells[day, reservation.hour_start].AddReservation(reservation);
				}
			}
		}

		private void FillWorkdays()
		{
			workdays = DataBase.Access.LoadWorkdays(WeekSelector.CurrentWeekStartDate, WeekSelector.CurrentWeekEndDate);

			foreach (WorkdayModel workday in workdays)
			{
				int day;
				if ((int)workday.date.DayOfWeek - 1 == -1) day = 6;
				else day = (int)workday.date.DayOfWeek - 1;

				int starth = workday.hour_start;
				int endh = workday.hour_end;

				for (int hour = starth; hour < endh; hour++)
				{
					Control_ReservationCell gridCell = new Control_ReservationCell(workday.date);

					gridCell.SetValue(Grid.RowProperty, hour);
					gridCell.SetValue(Grid.ColumnProperty, day + 1);

					gridCells[day, hour] = gridCell;

					gridCell.MouseLeftButtonDown += Rectangle_MouseLeftButtonDown;
					gridCell.MouseEnter += GridCell_MouseEnter;

					gridCell.MainBorder.Background = (Brush)Application.Current.FindResource("RacingRedBrush");

					this.MainGrid.Children.Add(gridCell);
				}
			}
		}
	}
}