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
    /// Interaction logic for TilesPanel.xaml
    /// </summary>
    public partial class TilesPanel : UserControl
    {
        public TilesPanel()
        {
            InitializeComponent();

            ShowTestGrid = true;
        }

        public void AddTile(Tile tile)
        {
            
        }

        public void DeleteTile(Tile tile)
        {

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

        public TileOnPanelInfo GetPositionForNewTile()
        {
            return new TileOnPanelInfo() { X = 64, Y = 64 };
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
