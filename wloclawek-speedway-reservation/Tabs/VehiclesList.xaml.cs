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
using System.Windows.Shapes;
using DataBaseAccess.Models;
using wloclawek_speedway_reservation.Styling.Controls;

namespace wloclawek_speedway_reservation.Tabs
{
    /// <summary>
    /// Interaction logic for VehiclesList.xaml
    /// </summary>
    public partial class VehiclesList : Window
    {
        private List<CarModel> cars;
        private List<CarModel> usedCars;
        private CarModel selectedCar;

        public VehiclesList(List<CarModel> usedCars)
        {
            InitializeComponent();
            cars = DataBase.Access.LoadCars();
            this.usedCars = usedCars;

            foreach (CarModel car in cars)
            {
                if (!usedCars.Exists(usedCar => usedCar.id == car.id))
                {
                    Control_CarItem carItem = new Control_CarItem(car);
                    carsList.Children.Add(carItem);
                    carItem.buttonSelect.Click += selectVehicle;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void selectVehicle(object sender, RoutedEventArgs e)
        {
            var buttonTag = (sender as Button).Tag;
            CarModel car = (CarModel)buttonTag;
            selectedCar = car;
            DialogResult = true;
            this.Close();
        }

        public CarModel car
        {
            get { return selectedCar; }
        }
    }
}