using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Home.NET.Tiles
{
    public class TileEnums
    {
        public static Color TileDefaultColor = Color.FromArgb(255, 25, 25, 25);

        public const int TilePadding = 8;
        public const int TileMaxSize = 248;

        // 1366x768 - original screen size
        public static Size EnumToSize(TileSizes size)
        {
            if (size == TileSizes.Small)
                return new Size(56, 56);
            else if (size == TileSizes.Normal)
                return new Size(120, 120);
            else if (size == TileSizes.Wide)
                return new Size(248, 120);
            else if (size == TileSizes.Big)
                return new Size(248, 248);

            return EnumToSize(TileSizes.Normal);
        }

        public static TileSizes SizeToEnum(Size size)
        {
            if (size.Width == 56 && size.Height == 56)
                return TileSizes.Small;
            else if (size.Width == 120 && size.Height == 120)
                return TileSizes.Normal;
            else if (size.Width == 248 && size.Height == 120)
                return TileSizes.Wide;
            else if (size.Width == 248 && size.Height == 248)
                return TileSizes.Big;

            return TileSizes.Small;
        }
        public enum MediaTypes
        {
            None,
            File,
            ProcessFile,
            Bytes
        }

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
    }
}
