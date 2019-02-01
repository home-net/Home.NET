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

namespace Home.NET.Tiles
{
    /// <summary>
    /// Interaction logic for NormalContainer.xaml
    /// </summary>
    public partial class TileContainer : UserControl
    {
        public List<Tile> Tiles
        {
            get
            {
                List<Tile> result = new List<Tile>();

                foreach(var o in ContainerPanel.Children)
                {
                    if (o is Tile)
                        result.Add((Tile)o);

                    if (o is TileContainer)
                        result.AddRange((o as TileContainer).Tiles);
                }

                return result;
            }
        }

        public enum ContainerTypes
        {
            SmallToNormal,
            NormalToWide
        }

        public TileContainer()
        {
            InitializeComponent();

            ContainerType = ContainerTypes.NormalToWide;
        }

        public void AddTile(dynamic tile)
        {

            if (ContainerType == ContainerTypes.NormalToWide)
            {
                ContainerPanel.Orientation = Orientation.Horizontal;

                if (ContainerPanel.Children.Count == 0)
                {
                    tile.Margin = new Thickness(0, 0, 8, 0);
                }
                else if (ContainerPanel.Children.Count == 1)
                {
                    tile.Margin = new Thickness(0);
                }
                else // more than 2
                {
                    return;
                }

                ContainerPanel.Children.Add(tile);
            }
            else if (ContainerType == ContainerTypes.SmallToNormal)
            {
                ContainerPanel.Orientation = Orientation.Vertical;

                if (ContainerPanel.Children.Count == 0)
                {
                    tile.Margin = new Thickness(0, 0, 6, 0);
                }
                else if (ContainerPanel.Children.Count == 1)
                {
                    tile.Margin = new Thickness(0, 0, 0, 0);
                }
                else if (ContainerPanel.Children.Count == 3)
                {
                    tile.Margin = new Thickness(0, 6, 6, 0);
                }
                else if (ContainerPanel.Children.Count == 4)
                {
                    tile.Margin = new Thickness(0, 6, 0, 0);
                }
                else // more than 4
                    return;

                ContainerPanel.Children.Add(tile);
            }
        }

        public void RemoveTile(UIElement tile)
        {
            ContainerPanel.Children.Remove(tile);
        }

        private ContainerTypes containerType = ContainerTypes.NormalToWide;
        public ContainerTypes ContainerType
        {
            get => containerType;
            set
            {
                containerType = value;
                

                if (containerType == ContainerTypes.NormalToWide)
                {
                    DebugRect.Stroke = new SolidColorBrush(Colors.Red);

                    for(int i = 0; i < ContainerPanel.Children.Count; i++)
                    {
                        var o = ContainerPanel.Children[i];

                        if (o is Tile)
                        {
                            (o as Tile).TileSize = Tile.TileSizes.Normal;
                        }

                        if (i >= 3)
                            RemoveTile(o);
                    }

                    Height = 128;
                    Width = 256;
                }
                else if (containerType == ContainerTypes.SmallToNormal)
                {
                    DebugRect.Stroke = new SolidColorBrush(Colors.Blue);

                    for (int i = 0; i < ContainerPanel.Children.Count; i++)
                    {
                        var o = ContainerPanel.Children[i];

                        if (o is Tile)
                        {
                            (o as Tile).TileSize = Tile.TileSizes.Small;
                        }
                        
                        if (i >= 5)
                            RemoveTile(o as Tile);
                    }

                    Height = 128;
                    Width = 128;
                }
            }
        }
    }
}
