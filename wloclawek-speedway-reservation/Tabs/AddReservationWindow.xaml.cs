using DataBaseAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using wloclawek_speedway_reservation.Styling.Controls;
using static wloclawek_speedway_reservation.Tabs.Notification;

namespace wloclawek_speedway_reservation.Tabs
{
	public partial class AddReservationWindow : Window
	{
		private enum modes
		{
			ADD, EDIT, VIEW
		}

		private modes _mode = modes.ADD;

		private List<CarModel> carsList = new List<CarModel>();

		private List<OfferModel> offersList = new List<OfferModel>();

		private ReservationModel reservation;

		public AddReservationWindow(ReservationModel reservation)
		{
			InitializeComponent();

			this.reservation = reservation;

			timestartbox.Text = new DateTime(reservation.date.Year, reservation.date.Month, reservation.date.Day, reservation.hour_start, 0, 0).ToString("yyyy-MM-dd HH:mm");
			timeendbox.Text = new DateTime(reservation.date.Year, reservation.date.Month, (reservation.hour_end == 24) ? reservation.date.Day + 1 : reservation.date.Day, (reservation.hour_end == 24) ? 0 : reservation.hour_end, 0, 0).ToString("yyyy-MM-dd HH:mm");

			offersList = DataBase.Access.LoadReservationOffer();
			offerType.ItemsSource = offersList;
			offerType.SelectedIndex = 0;

			Control_SelectCarButton selectButton;

			if (reservation.id != 0)
			{
				if (reservation.date > DateTime.Now)
				{
					_mode = modes.EDIT;
					labelTitle.Content = "Edit Reservation";
					buttonDelete.Visibility = Visibility.Visible;
					inputPesel.Text = reservation.client_pesel;
					inputPeopleCount.Text = reservation.people_count.ToString();
					offerType.SelectedIndex = reservation.offer_id;

					ClientModel client = DataBase.Access.getReservationClient(reservation.id);

					inputFirstName.Text = client.first_name;
					inputLastName.Text = client.last_name;

					carsList = DataBase.Access.getReservationVehicles(reservation.id);

					refreshList();
					return;
				}
				else
				{
					_mode = modes.VIEW;
					buttonSave.Visibility = Visibility.Collapsed;
					inputFirstName.Focusable = false;
					inputFirstName.Cursor = Cursors.No;
					inputLastName.Focusable = false;
					inputLastName.Cursor = Cursors.No;
					inputPesel.Focusable = false;
					inputPesel.Cursor = Cursors.No;
					inputPeopleCount.Focusable = false;
					inputPeopleCount.Cursor = Cursors.No;
					offerType.IsEnabled = false;
					offerType.Cursor = Cursors.No;
					labelTitle.Content = "View Reservation";
					inputPesel.Text = reservation.client_pesel;
					inputPeopleCount.Text = reservation.people_count.ToString();
					offerType.SelectedIndex = reservation.offer_id;

					ClientModel client = DataBase.Access.getReservationClient(reservation.id);

					inputFirstName.Text = client.first_name;
					inputLastName.Text = client.last_name;

					carsList = DataBase.Access.getReservationVehicles(reservation.id);
					refreshList();
					return;
				}
			}

			selectButton = new Control_SelectCarButton();
			CarsPanel.Children.Add(selectButton);
			selectButton.buttonSelect.Click += selectVehicle;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			string error = "";
			if (inputFirstName.Text.Length < 1)
			{
				error = "The first name is too short.";
			}
			if (inputLastName.Text.Length < 1)
			{
				error = "The last name is too short.";
			}
			if (inputPesel.Text.Length != 11)
			{
				error = "Enter the correct pesel";
			}
			if (inputPeopleCount.Text.Length < 1)
			{
				error = "Enter the amount of people";
			}
			if (error != "")
			{
				Notification notification = new Notification(notiType.ERROR, error);
				notification.ShowDialog();
				return;
			}
			reservation.client_pesel = inputPesel.Text;
			reservation.people_count = int.Parse(inputPeopleCount.Text);
			reservation.offer_id = 1;
			ClientModel client = new ClientModel
			{
				first_name = inputFirstName.Text,
				last_name = inputLastName.Text,
				pesel = inputPesel.Text
			};
			if (_mode == modes.EDIT)
			{
				DataBase.Access.EditReservation(reservation, client, carsList);
				Notification message = new Notification(Notification.notiType.INFO, "Reservation edited successfully.");
				message.Show();
			}
			else
			{
				DataBase.Access.AddReservation(reservation, client, carsList);
				Notification message = new Notification(Notification.notiType.INFO, "Reservation added successfully.");
				message.Show();
			}
			this.Close();
		}

		private void refreshCost()
		{
			double suma = 0;
			foreach (CarModel car in carsList)
			{
				suma += car.rent_price;
			}
			totalCost.Text = ((((offerType.SelectedItem as OfferModel).base_hour_price * int.Parse(inputPeopleCount.Text)) * (reservation.hour_end - reservation.hour_start)) + (suma * (reservation.hour_end - reservation.hour_start))).ToString();
		}

		private void selectVehicle(object sender, RoutedEventArgs e)
		{
			VehiclesList vehiclesList = new VehiclesList(carsList);
			vehiclesList.ShowDialog();
			if (vehiclesList.DialogResult == true)
			{
				CarModel car = vehiclesList.car;
				carsList.Add(car);
				refreshList();
			}
		}

		private void refreshList()
		{
			CarsPanel.Children.Clear();
			foreach (CarModel car in carsList)
			{
				Control_CarPreviewCompact CarPreviewCompact = new Control_CarPreviewCompact(car);

				CarsPanel.Children.Add(CarPreviewCompact);
				if (_mode != modes.VIEW)
				{
					CarPreviewCompact.buttonRemove.Click += removeVehicle;
					CarPreviewCompact.buttonRemove.Tag = car;
				}
				else
				{
					CarPreviewCompact.Focusable = false;
				}
			}

			if (_mode != modes.VIEW)
			{
				Control_SelectCarButton selectButton = new Control_SelectCarButton();
				CarsPanel.Children.Add(selectButton);
				selectButton.buttonSelect.Click += selectVehicle;
			}

			refreshCost();
		}

		private void removeVehicle(object sender, RoutedEventArgs e)
		{
			Notification notification = new Notification(Notification.notiType.QUESTION, "Do you want to delete this vehicle?");
			notification.ShowDialog();
			if (notification.DialogResult == true)
			{
				var buttonTag = (sender as Button).Tag;
				CarModel car = (CarModel)buttonTag;
				carsList.Remove(car);
				refreshList();
			}
		}

		private void onlyNumbers(object sender, TextCompositionEventArgs e)
		{
			Regex regex = new Regex("[^0-9]+");
			e.Handled = regex.IsMatch(e.Text);
		}

		private void onlyLetters(object sender, TextCompositionEventArgs e)
		{
			e.Handled = !Regex.IsMatch(e.Text, @"[A-Za-zżźćńółęąśŻŹĆĄŚĘŁÓŃ]+");
		}

		private void offerType_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			refreshCost();
		}

		private void inputPeopleCount_KeyUp(object sender, KeyEventArgs e)
		{
			if (inputPeopleCount.Text != "")
			{
				refreshCost();
			}
			if (inputPeopleCount.Text == "0")
			{
				inputPeopleCount.Text = "1";
				inputPeopleCount.Select(inputPeopleCount.Text.Length, 0);
				refreshCost();
			}
		}

		private void deleteReservation(object sender, RoutedEventArgs e)
		{
			Notification notification = new Notification(Notification.notiType.QUESTION, "Do you want to delete this reservation?");
			notification.ShowDialog();
			if (notification.DialogResult == true)
			{
				DataBase.Access.DeleteReservation(reservation.id);
				Notification message = new Notification(Notification.notiType.INFO, "The reservation has been deleted");
				message.Show();
				this.Close();
			}
		}
	}
}