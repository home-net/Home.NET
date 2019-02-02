using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public const int TilePadding = 8;
        public const int TileMaxSize = 248;

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

        private double tileScale = 1;

        public Tile(TileInfo info = null)
        {
            InitializeComponent();

            TileSize = TileSizes.Normal;

            if (info != null)
                ApplyTileInfo(info);
        }

        public string TileText
        {
            get => TileTextBlock.Text;
            set => TileTextBlock.Text = value;
        }

        public void ApplyTileInfo(TileInfo info)
        {
            TileStyle = info.Style;
            TileScale = info.Scale;
            TileSize = info.Size;
            TileAction = info.Action;
            TileText = info.Text;
            TileColor = info.Image.Color;
        }

        public Color TileColor
        {
            get => ((SolidColorBrush)RectCollision.Fill).Color;
            set => RectCollision.Fill = new SolidColorBrush(value);
        }

        public TileAction TileAction = new TileAction();

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

                if (value == TileStyles.Metro)
                {
                    Stuff.BorderMetro b = new Stuff.BorderMetro();
                    BorderGrid.Children.Add(b);

                    RectCollision.RadiusX = 0;
                    RectCollision.RadiusY = 0;

                    foreach (var rect in Gradients.Children)
                    {
                        if (rect is Rectangle)
                        {
                            ((Rectangle)rect).RadiusX = 0;
                            ((Rectangle)rect).RadiusY = 0;
                        }
                    }

                    Gradients.Visibility = Visibility.Hidden;
                }
                else if (value == TileStyles.Aero)
                {
                    Stuff.BorderAero b = new Stuff.BorderAero();
                    BorderGrid.Children.Add(b);

                    RectCollision.RadiusX = 5;
                    RectCollision.RadiusY = 5;

                    foreach (var rect in Gradients.Children)
                    {
                        if (rect is Rectangle)
                        {
                            ((Rectangle)rect).RadiusX = 5;
                            ((Rectangle)rect).RadiusY = 5;
                        }
                    }

                    Gradients.Visibility = Visibility.Visible;
                }
            }
        }

        private void GridCollision_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            TileAction.Do();
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var size = SizeToEnum(new Size(this.Width, this.Height));

            if (size == TileSizes.Small)
                GridName.Visibility = Visibility.Hidden;
            else
                GridName.Visibility = Visibility.Visible;
        }
    }

    [Serializable]
    public class TileInfo
    {
        public double Scale = 1;
        public Tile.TileStyles Style = Tile.TileStyles.Metro;
        public Tile.TileSizes Size = Tile.TileSizes.Normal;
        public string Text = "Tile";
        public TileAction Action = new TileAction();
        public TileImage Image = new TileImage();

        public TileInfo() { }
        public TileInfo(Tile tile)
        {
            Scale = tile.TileScale;
            Style = tile.TileStyle;
            Size = tile.TileSize;
            Action = tile.TileAction;
            Image.ColorByte = new byte[] { tile.TileColor.A, tile.TileColor.R, tile.TileColor.G, tile.TileColor.B };
        }
    }

    [Serializable]
    public class TileImage
    {
        public byte[] ColorByte = { 255, 25, 25, 25 }; // dark gray

        public Color Color
        {
            get => Color.FromArgb(ColorByte[0], ColorByte[1], ColorByte[2], ColorByte[3]);
            set => ColorByte = new byte[] { value.A, value.R, value.G, value.B };
        }

        public TileImage() { }
    }

    [Serializable]
    public class TileAction
    {
        public enum Actions
        {
            None,
            ProcessStart,
            ReflectionInvoke
        }

        public Actions Action = Actions.None;

        public string ProcessStartName = "";
        public string ProcessStartArguments = "";

        public string NETInvoke = "";

        public void Do()
        {
            if (Action == Actions.ProcessStart)
                Process.Start(ProcessStartName, ProcessStartArguments);
        }

        public TileAction() { }
    }
}
