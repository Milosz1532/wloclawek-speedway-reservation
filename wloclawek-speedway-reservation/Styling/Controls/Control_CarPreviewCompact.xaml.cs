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

namespace wloclawek_speedway_reservation.Styling.Controls
{
    /// <summary>
    /// Interaction logic for Control_CarPreviewCompact.xaml
    /// </summary>
    public partial class Control_CarPreviewCompact : UserControl
    {
        public Control_CarPreviewCompact(CarModel car = null)
        {
            InitializeComponent();
            if (car != null)
            {
                labelName.Content = car.model;
                labelCost.Content = car.rent_price + " pln/h";
            }
        }
    }
}