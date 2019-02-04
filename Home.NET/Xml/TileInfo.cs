using Home.NET.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using static Home.NET.Tiles.TileEnums;

namespace Home.NET.Xml
{

    [Serializable]
    public class TileInfo
    {
        public double Scale = 1;
        public TileStyles Style = TileStyles.Metro;
        public TileSizes Size = TileSizes.Normal;
        public string Text = "Tile";
        public TileAction Action = new TileAction();

        public MediaTypes Icon = MediaTypes.None;
        public MediaTypes Image = MediaTypes.None;

        public byte[] ColorByte = { 255, 25, 25, 25 }; // dark gray

        public Color Color
        {
            get => Color.FromArgb(ColorByte[0], ColorByte[1], ColorByte[2], ColorByte[3]);
            set => ColorByte = new byte[] { value.A, value.R, value.G, value.B };
        }


        public TileInfo() { }
        public TileInfo(Tile tile)
        {
            Scale = tile.TileScale;
            Style = tile.TileStyle;
            Size = tile.TileSize;
            Action = tile.TileAction;
            Color = tile.TileColor;
            Icon = tile.MediaTypeIcon;
            Image = tile.MediaTypeBackground;
        }
    }
}
