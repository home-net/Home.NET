using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Home.NET.Tiles;

namespace Home.NET.Stuff
{
    /// <summary>
    /// Interaction logic for PickColor.xaml
    /// </summary>
    public partial class PickColor : Window
    {
        public bool allowAero = false;
        public bool ResultIsAero = false;
        public Color ResultColor = Colors.Black;

        public PickColor(Color Original, bool AllowAero = false)
        {
            allowAero = AllowAero;

            InitializeComponent();

            ResultColor = Original;

            btnStaticColorChoose.Background = new SolidColorBrush(Original);

            sliderTP.Value = ResultColor.A;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!DwmApi.CheckAeroEnabled())
                rbAero.IsEnabled = false;
        }

        private void btnStaticColorChoose_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Forms.ColorDialog picker = new System.Windows.Forms.ColorDialog();
            picker.Color = System.Drawing.Color.FromArgb(ResultColor.A + ResultColor.R + ResultColor.G + ResultColor.B);
            picker.FullOpen = true;

            if(picker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ResultColor = Color.FromArgb(picker.Color.A, picker.Color.R, picker.Color.G, picker.Color.B);

                btnStaticColorChoose.Background = new SolidColorBrush(ResultColor);
                sliderTP.Value = ResultColor.A;
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            byte opacity = (byte)e.NewValue;

            ResultColor.A = opacity;

            btnStaticColorChoose.Background = new SolidColorBrush(ResultColor);
        }

        private void OKBtn_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)rbAero.IsChecked)
                ResultIsAero = true;

            this.DialogResult = true;
            this.Close();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
