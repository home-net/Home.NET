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

namespace Home.NET.Tiles
{
    /// <summary>
    /// Interaction logic for TilesPanel.xaml
    /// </summary>
    public partial class TilesPanel : UserControl
    {
        private Tile.TileStyles panelStyle = Tile.TileStyles.Metro;
        public Tile.TileStyles PanelStyle
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

            ShowTestGrid = false;
        }

        public void AddTile(TileContainer tiles)
        {

            if (tiles.ContainerType == TileContainer.ContainerTypes.SmallToNormal)
            {
                var big = new TileContainer();

                big.ContainerType = TileContainer.ContainerTypes.NormalToWide;
                big.ContainerPanel.Children.Add(tiles);

                big.Margin = new Thickness(Tile.TilePadding / 2);

                MainGrid.Children.Add(big);
            }
            else
            {
                tiles.Margin = new Thickness(Tile.TilePadding / 2);
                MainGrid.Children.Add(tiles);
            }
        }

        public void AddTile(Tile tile)
        {
            tile.TileStyle = PanelStyle;
            tile.TileScale = PanelScale;

            UIElement obj = tile;

            if (tile.TileSize == Tile.TileSizes.Small)
            {
                var container = new TileContainer();

                container.ContainerType = TileContainer.ContainerTypes.SmallToNormal;
                container.ContainerPanel.Children.Add(tile);

                var big = new TileContainer();

                big.ContainerType = TileContainer.ContainerTypes.NormalToWide;
                big.ContainerPanel.Children.Add(container);
                
                obj = big;
            }
            else if (tile.TileSize == Tile.TileSizes.Normal)
            {
                var container = new TileContainer();

                container.ContainerType = TileContainer.ContainerTypes.NormalToWide;
                container.ContainerPanel.Children.Add(tile);

                obj = container;
            }

            if (obj is Tile)
                (obj as Tile).Margin = new Thickness(Tile.TilePadding / 2);
            else if (obj is TileContainer)
                (obj as TileContainer).Margin = new Thickness(Tile.TilePadding / 2);

            MainGrid.Children.Add(obj);
        }

        public void DeleteTile(Tile tile)
        {
            MainGrid.Children.Remove(tile);
        }

        Random c = new Random();

        public void AddTestTile(string text)
        {

            TileInfo i = new TileInfo();
            i.Text = text;
            i.Image = new TileImage() { ColorByte = new byte[] { 255, (byte)c.Next(0, 255), (byte)c.Next(0, 255), (byte)c.Next(0, 255) } };
            
            i.Size = (Tile.TileSizes)c.Next(0, 4);

            AddTile(new Tile(i));
        }

        public bool ShowTestGrid
        {
            get
            {
                if (GridX.Visibility != Visibility.Hidden)
                    return true;
                else
                    return false;
            }
            set
            {
                if(!value)
                {
                    GridX.Children.Clear();
                    GridX.Visibility = Visibility.Hidden;
                }
                else
                {
                    GridX.Visibility = Visibility.Visible;
                    GridX.Children.Clear();

                    for (int y = 0; y < 100; y++)
                    {
                        for (int x = 0; x < 100; x++)
                        {
                            Border b = new Border();
                            b.Background = new SolidColorBrush(Color.FromArgb(70, 25, 25, 25));

                            GridX.Children.Add(b);
                        }
                    }
                }
            }
        }

        //public TileOnPanelInfo GetPositionForNewTile()
        //{
        //    Size size = Tile.EnumToSize(Tile.TileSizes.Normal); // 128x128
        //    int padding = 16 / 2;
        //    int x = 30, y = 30;

        //    foreach(var tile in TilesList)
        //    {
        //        Point tilePoint = new Point((int)Canvas.GetLeft(tile), (int)Canvas.GetTop(tile));
        //        Point newTile = new Point(x, y);

        //        if (tilePoint == newTile)
        //        {
        //            y += padding;// + (int)tile.Height;
        //        }
        //        else
        //            break;
        //    }

        //    return new TileOnPanelInfo() { X = x, Y = y };
        //}

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
        }
    }

    [Serializable]
    public class TilePanelInfo
    {
    }

    [Serializable]
    public class TilePanelBlock
    {

    }

    [Serializable]
    public class TileOnPanelInfo
    {
        public double X, Y;
        public Size Size;
        public TileInfo TileInfo;
    }
    
}
