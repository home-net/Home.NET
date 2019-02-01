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
        }

        public void AddTile(Tile tile)
        {
            
        }

        public void DeleteTile(Tile tile)
        {

        }

        public TileOnPanelInfo GetPositionForNewTile()
        {

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
