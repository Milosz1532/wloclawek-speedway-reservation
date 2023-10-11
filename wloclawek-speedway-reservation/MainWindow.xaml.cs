using DataBaseAccess.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Effects;
using wloclawek_speedway_reservation.Tabs;

namespace wloclawek_speedway_reservation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            TopPanel.MouseDown += delegate (object sender, MouseButtonEventArgs e) { if (e.ChangedButton == MouseButton.Left) DragMove(); };

            ReservationsRButton.Checked += ReservationsRButton_Checked;
            CarsRButton.Checked += CarsRButton_Checked;
            CalendarRButton.Checked += CalendarRButton_Checked;
            ReservationsRButton.IsChecked = true;
        }

        private void CalendarRButton_Checked(object sender, RoutedEventArgs e)
        {
            MainView.Children.Clear();
            MainView.Children.Add(new CalendarTab());
        }

        private void CarsRButton_Checked(object sender, RoutedEventArgs e)
        {
            MainView.Children.Clear();
            MainView.Children.Add(new CarsTab());
        }

        private void ReservationsRButton_Checked(object sender, RoutedEventArgs e)
        {
            MainView.Children.Clear();
            MainView.Children.Add(new ReservationsTab());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
            else
            {
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            }
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.BorderThickness = new Thickness(7);
            }
            else
            {
                this.BorderThickness = new Thickness(0);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            blurWindow.Effect = null;
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            BlurEffect blurEffect = new BlurEffect();
            blurEffect.Radius = 5;
            blurWindow.Effect = blurEffect;
        }

        private void logoutButton(object sender, RoutedEventArgs e)
        {
            Notification notification = new Notification(Notification.notiType.QUESTION, "Do you want to close app?");
            notification.ShowDialog();
            if (notification.DialogResult == true)
            {
                this.Close();
            }
        }
    }
}