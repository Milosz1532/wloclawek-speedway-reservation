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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DataBase;
using DataBaseAccess;

using System.IO;

namespace wloclawek_speedway_reservation.Styling.Controls
{
    /// <summary>
    /// Interaction logic for Control_CarPreview.xaml
    /// </summary>
    public partial class Control_CarPreview : UserControl
    {
        public CarModel car;

        public Control_CarPreview(CarModel car)
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

            CarName.Content = string.Format("{0} {1}", this.car.model, this.car.production_year);
            CarPower.Content = string.Format("{0} hp", this.car.power);
            //if (this.car.engine_type == "Electric") CarEngine.Content = this.car.engine_type;
            //else CarEngine.Content = string.Format("{0}L {1}", string.Format("{0:0.0#}", this.car.capacity), this.car.engine_type);
            CarEngine.Content = this.car.engine_type;
            CarDrive.Content = this.car.drive;
            CarTransmission.Content = this.car.transmission;
            CarPrice.Content = string.Format("{0} pln/h", this.car.rent_price);
        }
    }
}