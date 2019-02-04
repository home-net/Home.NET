using Home.NET.Tiles.Extensions;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Home.NET.Tiles.TileEnums;

namespace Home.NET.Tiles
{
    /// <summary>
    /// Interaction logic for TilesPanel.xaml
    /// </summary>
    public partial class TilesPanel : UserControl
    {
        public static Random tRand = new Random();

        private TileStyles panelStyle = TileStyles.Metro;
        public TileStyles PanelStyle
        {
            get => panelStyle;
            set
            {
                panelStyle = value;

                foreach (var tile in TilesList)
                {
                    tile.TileStyle = value;
                }
            }
        }

        private double panelScale = 1;
        public double PanelScale
        {
            get => panelScale;
            set
            {
                panelScale = value;

                foreach (var tile in TilesList)
                {
                    tile.TileScale = value;
                }
            }
        }

        public TilesPanel()
        {
            InitializeComponent();
        }

        public void AddTile(TileContainer tiles)
        {
            tiles.ParentPanel = this;

            if (tiles.ContainerType == TileContainer.ContainerTypes.SmallToNormal)
            {
                var big = new TileContainer();

                big.ContainerType = TileContainer.ContainerTypes.NormalToWide;
                big.ContainerPanel.Children.Add(tiles);
                big.ParentPanel = this;

                big.Margin = new Thickness(TilePadding / 2);

                MainGrid.Children.Add(big);
            }
            else
            {
                tiles.Margin = new Thickness(TilePadding / 2);
                MainGrid.Children.Add(tiles);
            }

            if (IsLoaded)
                foreach (var tile in tiles.Tiles)
                    tile.FadeIn(450);
        }

        public void AddTile(Tile tile)
        {
            tile.TileStyle = PanelStyle;
            tile.TileScale = PanelScale;

            UIElement obj = tile;

            if (tile.TileSize == TileSizes.Small)
            {
                var container = new TileContainer();

                container.ContainerType = TileContainer.ContainerTypes.SmallToNormal;
                container.ContainerPanel.Children.Add(tile);
                container.ParentPanel = this;

                var big = new TileContainer();

                big.ContainerType = TileContainer.ContainerTypes.NormalToWide;
                big.ContainerPanel.Children.Add(container);
                big.ParentPanel = this;

                obj = big;
            }
            else if (tile.TileSize == TileSizes.Normal)
            {
                var container = new TileContainer();

                container.ContainerType = TileContainer.ContainerTypes.NormalToWide;
                container.ContainerPanel.Children.Add(tile);
                container.ParentPanel = this;

                obj = container;
            }

            if (obj is Tile)
                (obj as Tile).Margin = new Thickness(TilePadding / 2);
            else if (obj is TileContainer)
                (obj as TileContainer).Margin = new Thickness(TilePadding / 2);

            MainGrid.Children.Add(obj);

            if (IsLoaded)
                tile.FadeIn(450);
        }

        public void DeleteTile(Tile tile)
        {
            MainGrid.Children.Remove(tile);
        }

        public void AddTestTile(string text)
        {
            Tile tile = new Tile()
            {
                TileText = text,
                TileColor = Color.FromArgb(255, (byte)tRand.Next(0, 255), (byte)tRand.Next(0, 255), (byte)tRand.Next(0, 255)),
                TileSize = TileSizes.Normal
            };

            AddTile(tile);
        }

        public List<Tile> TilesList
        {
            get
            {
                List<Tile> result = new List<Tile>();

                foreach (var o in MainGrid.Children)
                {
                    if (o is Tile)
                    {
                        result.Add((Tile)o);
                    }

                    if (o is TileContainer)
                    {
                        result.AddRange((o as TileContainer).Tiles);
                    }
                }

                return result;
            }
        }

        public Tile[] Tiles
        {
            get
            {
                return TilesList.ToArray();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            int dur = 450;
            int start = 0;
            foreach (var tile in Tiles)
            {
                tile.FadeIn(dur, start);
                start += 100;
            }
        }
    }
}
