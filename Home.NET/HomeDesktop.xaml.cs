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


            var container = new TileContainer(TileContainer.ContainerTypes.NormalToWide);

            TileInfo i = new TileInfo();
            i.Action = new TileAction() { Action = TileAction.Actions.ProcessStart, ProcessStartName = "C:\\Windows\\explorer.exe" };
            i.ColorByte = new byte[] { 255, 4, 17, 75 };
            i.Text = "Explorer";
            i.Size = Tile.TileSizes.Normal;

            container.AddTile(new Tile(i));

            i = new TileInfo();
            i.Action = new TileAction() { Action = TileAction.Actions.ProcessStart, ProcessStartName = "Home.NET.exe" };
            i.ColorByte = new byte[] { 255, 75, 156, 206 };
            i.Text = "Home.NET";
            i.Size = Tile.TileSizes.Normal;

            container.AddTile(new Tile(i));

            TilesPanel.AddTile(container);

            TileInfo small = new TileInfo() { Size = Tile.TileSizes.Small };
            Tile a = new Tile(small), b = new Tile(small), c = new Tile(small), d = new Tile(small);
            a.TileColor = Colors.Red;
            b.TileColor = Colors.Green;
            c.TileColor = Colors.Blue;
            d.TileColor = Colors.Yellow;
            c.TileAction.ProcessStartName = "C:\\Windows\\System32\\control.exe";
            c.TileAction.Action = TileAction.Actions.ProcessStart;
            c.TileText = "Control Panel";

            TileContainer cont = new TileContainer(TileContainer.ContainerTypes.SmallToNormal);
            cont.AddTile(a);
            cont.AddTile(b);
            cont.AddTile(c);
            cont.AddTile(d);

            TilesPanel.AddTile(cont);

            foreach (var desk in Directory.EnumerateFiles(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)))
            {
                TileInfo deskTile = new TileInfo();

                deskTile.Action.Action = TileAction.Actions.ProcessStart;
                deskTile.Action.ProcessStartName = desk;
                deskTile.Text = new FileInfo(desk).Name;
                deskTile.Size = Tile.TileSizes.Wide;

                TilesPanel.AddTile(new Tile(deskTile));
            }


            for (int xx = 0; xx < 10; xx++)
                TilesPanel.AddTestTile("Tile #" + xx);


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
            TilesPanel.PanelStyle = Tiles.Tile.TileStyles.Aero;
        }

        private void btnMetro_Click(object sender, RoutedEventArgs e)
        {
            TilesPanel.PanelStyle = Tiles.Tile.TileStyles.Metro;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TilesPanel.AddTestTile("Test Tile");
        }
    }
}
