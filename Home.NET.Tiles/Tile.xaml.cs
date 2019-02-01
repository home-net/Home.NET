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
    /// Interaction logic for Tile.xaml
    /// </summary>
    public partial class Tile : UserControl
    {
        public enum TileSizes
        {
            Small,
            Normal,
            Wide,
            Big
        }

        public enum TileStyles
        {
            Aero,
            Metro,
            // Fluent
        }

        public static Size EnumToSize(TileSizes size)
        {
            if (size == TileSizes.Small)
                return new Size(57, 57);
            else if (size == TileSizes.Normal)
                return new Size(128, 128);
            else if (size == TileSizes.Wide)
                return new Size(128, 256);
            else if (size == TileSizes.Big)
                return new Size(256, 256);

            return new Size(128, 128);
        }

        public static TileSizes SizeToEnum(Size size)
        {
            if (size.Width == 57 && size.Height == 57)
                return TileSizes.Small;
            else if (size.Width == 128 && size.Height == 128)
                return TileSizes.Normal;
            else if (size.Width == 128 && size.Height == 256)
                return TileSizes.Wide;
            else if (size.Width == 256 && size.Height == 256)
                return TileSizes.Big;

            return TileSizes.Small;
        }

        private double tileScale = 1;

        public Tile(TileInfo info = null)
        {
            InitializeComponent();

            TileSize = TileSizes.Normal;

            if (info != null)
                ApplyTileInfo(info);
        }

        public void ApplyTileInfo(TileInfo info)
        {
            TileStyle = info.Style;
            TileScale = info.Scale;
            TileSize = info.Size;
        }

        public double TileScale
        {
            get => tileScale;

            set
            {
                if (value <= 0)
                    value = 1;
                tileScale = value;
            }
        }

        public TileSizes TileSize
        {
            get
            {
                return SizeToEnum(new Size(this.Width, this.Height));
            }
            set
            {
                var s = EnumToSize(value);

                this.Width = s.Width * TileScale;
                this.Height = s.Height * TileScale;
            }
        }

        private TileStyles tileStyle = TileStyles.Metro;
        public TileStyles TileStyle
        {
            get => tileStyle;
            set
            {
                tileStyle = value;

                BorderGrid.Children.Clear();

                //if(value == TilesUtils.TileStyle.Aero) fix me
                {
                    Stuff.BorderMetro b = new Stuff.BorderMetro();
                    BorderGrid.Children.Add(b);
                }
            }
        }

    }

    [Serializable]
    public class TileInfo
    {
        public double Scale = 1;
        public Tile.TileStyles Style = Tile.TileStyles.Metro;
        public Tile.TileSizes Size = Tile.TileSizes.Normal;


        public TileInfo() { }
        public TileInfo(Tile tile)
        {
            Scale = tile.TileScale;
            Style = tile.TileStyle;
            Size = tile.TileSize;
        }
    }
}
