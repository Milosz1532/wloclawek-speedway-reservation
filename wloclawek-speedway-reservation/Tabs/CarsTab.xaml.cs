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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using wloclawek_speedway_reservation.Styling.Controls;

namespace wloclawek_speedway_reservation.Tabs
{
    /// <summary>
    /// Interaction logic for CarsTab.xaml
    /// </summary>
    public partial class CarsTab : UserControl
    {
        private List<CarModel> cars;

        public CarsTab()
        {
            InitializeComponent();

            refreshCars();
        }

        public void refreshCars()
        {
            CarsPanel.Children.Clear();
            cars = DataBase.Access.LoadCars();
            Control_AddCarButton addNewVehicle = new Control_AddCarButton();
            addNewVehicle.addVehicleButton.Click += Button_Click;

            foreach (CarModel car in cars)
            {
                Control_CarPreview carControl = new Control_CarPreview(car);
                CarsPanel.Children.Add(carControl);
                carControl.editBtn.Click += editVehicle;
                carControl.editBtn.Tag = car;
            }
            CarsPanel.Children.Add(addNewVehicle);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddNewVehicleWindow window = new AddNewVehicleWindow();
            window.ShowDialog();
            if (window.DialogResult == false)
            {
                refreshCars();
            }
        }

        private void editVehicle(object sender, RoutedEventArgs e)
        {
            var buttonTag = (sender as Button).Tag;
            CarModel car = (CarModel)buttonTag;
            AddNewVehicleWindow window = new AddNewVehicleWindow(car);
            window.ShowDialog();
            if (window.DialogResult == false)
            {
                refreshCars();
            }
        }
    }
}