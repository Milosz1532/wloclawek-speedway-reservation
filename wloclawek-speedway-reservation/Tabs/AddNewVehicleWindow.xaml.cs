using DataBaseAccess.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
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
using Image = System.Windows.Controls.Image;
using static wloclawek_speedway_reservation.Tabs.Notification;

namespace wloclawek_speedway_reservation.Tabs
{
    public partial class AddNewVehicleWindow : Window
    {
        private List<string> engines = new List<string>();

        private CarModel car = null;

        public AddNewVehicleWindow(CarModel car = null)
        {
            InitializeComponent();
            engines = DataBase.Access.getVehiclesEngines();
            foreach (string engine in engines)
            {
                EngineType.Items.Add(engine);
            }
            inputDrive.Items.Add("FWD");
            inputDrive.Items.Add("RWD");
            inputDrive.Items.Add("AWD");
            this.car = car;

            if (car != null)
            {
                labelTitle.Content = "Edit Vehicle";
                buttonAdd.Content = "Edit Vehicle";
                buttonDelete.Visibility = Visibility.Visible;
                if (car.image != null)
                {
                    Image image = new Image();
                    using (MemoryStream stream = new MemoryStream(car.image))
                    {
                        image.Source = BitmapFrame.Create(stream,
                        BitmapCreateOptions.None,
                        BitmapCacheOption.OnLoad);
                        //image = stream.ToArray;
                    }

                    imgPhoto.Source = image.Source;
                }
                inputModel.Text = car.model;
                inputYear.Text = car.production_year.ToString(new CultureInfo("en-US"));
                EngineType.Text = car.engine_type;
                inputTransmission.Text = car.transmission;
                inputPower.Text = car.power.ToString(new CultureInfo("en-US"));
                inputDrive.Text = car.drive;
                inputTopSpeed.Text = car.top_speed.ToString(new CultureInfo("en-US"));
                inputZTH.Text = car.zero_to_hundred.ToString(new CultureInfo("en-US"));
                inputRentPrice.Text = car.rent_price.ToString(new CultureInfo("en-US"));
                image = car.image;
            }
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private byte[] image = null;

        private void UploadImage(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                BitmapImage carImage = new BitmapImage(new Uri(op.FileName));
                imgPhoto.Source = carImage;
                selectImage.Visibility = Visibility.Collapsed;

                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(carImage));
                using (MemoryStream ms = new MemoryStream())
                {
                    encoder.Save(ms);
                    image = ms.ToArray();
                }
            }
        }

        private void sendData(object sender, RoutedEventArgs e)
        {
            string error = "";
            if (inputModel.Text.Length < 5)
            {
                error = "The model is too short.";
            }
            if (inputYear.Text.Length < 4)
            {
                error = "The year is incorrect";
            }
            if (EngineType.Text == "")
            {
                error = "Select an engine type";
            }
            if (inputTransmission.Text.Length < 3)
            {
                error = "The transmission is too short.";
            }
            if (inputPower.Text.Length < 2)
            {
                error = "The Power is incorrect";
            }
            if (inputDrive.Text == "")
            {
                error = "Select an Drive type";
            }
            if (inputTopSpeed.Text.Length < 2)
            {
                error = "Enter the top speed";
            }
            if (inputZTH.Text.Length < 3)
            {
                error = "Enter a time from zero to one hundred";
            }
            if (inputRentPrice.Text == "")
            {
                error = "Enter the rent price";
            }
            if (error != "")
            {
                Notification notification = new Notification(notiType.ERROR, error);
                notification.ShowDialog();
                return;
            }
            CarModel vehicle = new CarModel
            {
                model = inputModel.Text,
                production_year = int.Parse(inputYear.Text),
                engine_type = EngineType.Text,
                image = image,
                power = int.Parse(inputPower.Text),
                drive = inputDrive.Text,
                transmission = inputTransmission.Text,
                top_speed = int.Parse(inputTopSpeed.Text),
                zero_to_hundred = double.Parse(inputZTH.Text, CultureInfo.InvariantCulture),
                is_available = true,
                rent_price = double.Parse(inputRentPrice.Text, CultureInfo.InvariantCulture)
            };

            if (car == null)
            {
                DataBase.Access.AddVehicle(vehicle);
                Notification message = new Notification(notiType.INFO, "Vehicle added successfully");
                message.Show();
            }
            else
            {
                DataBase.Access.updateVehicle(vehicle, car.id);
                Notification message = new Notification(notiType.INFO, "Vehicle updated successfully");
                message.Show();
            }
            this.Close();
        }

        public void deleteVehicle(object sender, RoutedEventArgs e)
        {
            if (car == null) return;
            Notification message = new Notification(notiType.QUESTION, "Are you sure you want to delete this vehicle?");
            message.ShowDialog();
            if (message.DialogResult == true)
            {
                DataBase.Access.deleteVehicle(car.id);
                this.Close();
            }
        }

        private void onlyNumbers(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void onlyDecimal(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }
    }
}