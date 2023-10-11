using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace wloclawek_speedway_reservation.Tabs
{
    /// <summary>
    /// Interaction logic for Notification.xaml
    /// </summary>
    ///
    public partial class Notification : Window
    {
        public enum notiType
        {
            ERROR,
            INFO,
            QUESTION
        }

        public Notification(notiType type, string text)
        {
            InitializeComponent();
            switch (type)
            {
                case notiType.ERROR:
                    inputTitle.Content = "ERROR";
                    inputText.Content = text;
                    buttonButton1.Visibility = Visibility.Visible;
                    buttonButton1.Content = "OK";
                    buttonButton1.Click += closeWindow;
                    break;

                case notiType.INFO:
                    inputTitle.Content = "INFO";
                    inputText.Content = text;
                    buttonButton1.Visibility = Visibility.Visible;
                    buttonButton1.Content = "OK";
                    buttonButton1.Click += closeWindow;
                    break;

                case notiType.QUESTION:
                    inputTitle.Content = "Are you sure?";
                    inputText.Content = text;
                    buttonButton1.Visibility = Visibility.Visible;
                    buttonButton1.Content = "YES";
                    buttonButton1.Click += questionYES;
                    buttonButton2.Visibility = Visibility.Visible;
                    buttonButton2.Content = "NO";
                    buttonButton2.Click += questionNO;
                    break;
            }
        }

        private void questionYES(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }

        private void questionNO(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        private void closeWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}