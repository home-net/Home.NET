﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Home.NET
{
    /// <summary>
    /// Interaction logic for HomeDesktop.xaml
    /// </summary>
    public partial class HomeDesktop : Window
    {
        public HomeDesktop()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            Home.NET.Stuff.PickColor Pick = new Stuff.PickColor(Colors.Green, true);
            if ((bool)Pick.ShowDialog())
            {
                if (Pick.ResultIsAero)
                    Tiles.DwmApi.Glass(this);
                else
                    this.Background = new SolidColorBrush(Pick.ResultColor);
            }
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            TilesPanel.PanelStyle = Tiles.Tile.TileStyles.Aero;
        }

        private void btnMetro_Click(object sender, RoutedEventArgs e)
        {
            TilesPanel.PanelStyle = Tiles.Tile.TileStyles.Metro;
        }
    }
}
