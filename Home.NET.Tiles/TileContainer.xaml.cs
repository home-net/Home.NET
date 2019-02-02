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

namespace Home.NET.Tiles
{
    /// <summary>
    /// Interaction logic for NormalContainer.xaml
    /// </summary>
    public partial class TileContainer : UserControl
    {
        public TilesPanel ParentPanel = null;

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

        public TileContainer(ContainerTypes type = ContainerTypes.NormalToWide)
        {
            InitializeComponent();

            ContainerType = type;
        }

        public void AddTile(dynamic tile)
        {

            if (ContainerType == ContainerTypes.NormalToWide)
            {
                ContainerPanel.Orientation = Orientation.Horizontal;

                if (ContainerPanel.Children.Count == 0)
                {
                    tile.Margin = new Thickness(0, 0, Tile.TilePadding, 0);
                }
                else if (ContainerPanel.Children.Count == 1)
                {
                    tile.Margin = new Thickness(0);
                }
                else // more than 2
                {
                    // Add new tile to panel
                    if (ParentPanel != null && tile is Tile)
                        ParentPanel.AddTile(tile);

                    return;
                }

                ContainerPanel.Children.Add(tile);
            }
            else if (ContainerType == ContainerTypes.SmallToNormal)
            {
                ContainerPanel.Orientation = Orientation.Horizontal;

                if (ContainerPanel.Children.Count == 0)
                {
                    tile.Margin = new Thickness(0, 0, Tile.TilePadding, 0);
                }
                else if (ContainerPanel.Children.Count == 1)
                {
                    tile.Margin = new Thickness(0, 0, 0, 0);
                }
                else if (ContainerPanel.Children.Count == 2)
                {
                    tile.Margin = new Thickness(0, Tile.TilePadding, Tile.TilePadding, 0);
                }
                else if (ContainerPanel.Children.Count == 3)
                {
                    tile.Margin = new Thickness(0, Tile.TilePadding, 0, 0);
                }
                else // more than 4
                {
                    // Add new tile to panel
                    if (ParentPanel != null && tile is Tile)
                        ParentPanel.AddTile(tile);

                    return;
                }

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

                    var normalSize = Tile.EnumToSize(Tile.TileSizes.Wide);
                    Height = normalSize.Height;
                    Width = normalSize.Width;
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

                    var normalSize = Tile.EnumToSize(Tile.TileSizes.Normal);
                    Height = normalSize.Height;
                    Width = normalSize.Width;
                }
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //int dur = 500;
            //foreach(var tile in Tiles)
            //{
            //    tile.FadeIn(dur);
            //    dur += 500;
            //}
        }
    }
}
