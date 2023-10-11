using DataBaseAccess.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for Control_CarItem.xaml
    /// </summary>
    public partial class Control_CarItem : UserControl
    {
        public CarModel car;

        public Control_CarItem(CarModel car)
        {
            InitializeComponent();
            this.car = car;
            if (car.image != null)
            {
                Image image = new Image();
                using (MemoryStream stream = new MemoryStream(car.image))
                {
                    image.Source = BitmapFrame.Create(stream,
                    BitmapCreateOptions.None,
                    BitmapCacheOption.OnLoad);
                }

                CarImage.Source = image.Source;
            }
            labelModel.Content = car.model + " " + car.production_year;
            labelTransmission.Content = car.transmission;
            labelDrive.Content = car.drive;
            labelCost.Content = car.rent_price + " pln/h";
            buttonSelect.Tag = car;
        }
    }
}