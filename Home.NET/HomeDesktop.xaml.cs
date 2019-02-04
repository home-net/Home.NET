using Home.NET.Tiles;
using System;
using System.Collections.Generic;
using System.IO;
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
using static Home.NET.Tiles.TileEnums;

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

            foreach (var desk in Directory.EnumerateFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)))
            {
                Tile tile = new Tile();
                tile.TileAction.Action = TileAction.Actions.ProcessStart;
                tile.TileAction.ProcessStartName = desk;
                tile.TileText = new FileInfo(desk).Name;
                tile.TileSize = TileSizes.Small;

                TilesPanel.Add(tile);
            }


            //for (int xx = 0; xx < 50; xx++)
            //    TilesPanel.AddTestTile("Tile #" + xx);


        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Home.NET.Stuff.PickColor Pick = new Stuff.PickColor(Color.FromArgb(255, 0, 142, 143), true);
            //if ((bool)Pick.ShowDialog())
            //{
            //    if (Pick.ResultIsAero)
            //        Tiles.DwmApi.Glass(this);
            //    else
            //        this.Background = new SolidColorBrush(Pick.ResultColor);
            //}
            this.Background = new SolidColorBrush(Color.FromArgb(255, 0, 142, 143));
            //this.Background = new SolidColorBrush(Colors.Green);
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            TilesPanel.PanelStyle = TileStyles.Aero;
        }

        private void btnMetro_Click(object sender, RoutedEventArgs e)
        {
            TilesPanel.PanelStyle = TileStyles.Metro;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TilesPanel.AddTestTile("Test Tile");
        }

        private void CntDebug_Click(object sender, RoutedEventArgs e)
        {
            TilesPanel.DebugContainer(cntDebugText.Text);
        }
    }
}
