using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Home.NET.Tiles.Extensions;

namespace Home.NET.Tiles
{
    /// <summary>
    /// Interaction logic for Tile.xaml
    /// </summary>
    public partial class Tile : UserControl
    {
        public System.Timers.Timer UpdateTimer = new System.Timers.Timer(500);

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

            GridMouseCollision.Visibility = Visibility.Visible;
            MouseGlow.Opacity = 0;

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
            TileColor = info.Color;

            MediaTypeBackground = info.Image;
            MediaTypeIcon = info.Icon;

            UpdateActions();
        }

        private TileInfo.MediaTypes mediaTypeBackground = TileInfo.MediaTypes.None;
        private TileInfo.MediaTypes mediaTypeIcon = TileInfo.MediaTypes.None;

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

        public void UpdateActions()
        {
            TileAction.Parent = this;

            if (TileAction.Action == TileAction.Actions.ProcessStart)
            {
                MediaTypeIcon = TileInfo.MediaTypes.ProcessFile;
            }
            else if (TileAction.Action == TileAction.Actions.None)
            {
                MediaTypeIcon = TileInfo.MediaTypes.Bytes;
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

        private ImageSource customMediaIcon = null;
        private ImageSource customMediaBackground = null;

        public TileInfo.MediaTypes MediaTypeBackground
        {
            get => mediaTypeBackground;
            set
            {
                mediaTypeBackground = value;
                TileUpdateImage(IconMedia, value, CustomMediaBackground);
            }
        }
        public TileInfo.MediaTypes MediaTypeIcon
        {
            get => mediaTypeIcon;
            set
            {
                mediaTypeIcon = value;
                TileUpdateImage(IconMedia, value, CustomMediaIcon);
            }
        }

        public ImageSource CustomMediaBackground
        {
            get => customMediaBackground;
            set
            {
                customMediaBackground = value;
                MediaTypeBackground = MediaTypeBackground;  // update
            }
        }
        public ImageSource CustomMediaIcon
        {
            get => customMediaIcon;
            set
            {
                customMediaIcon = value;
                MediaTypeIcon = MediaTypeIcon; // update
            }
        }

        public void TileUpdateImage(Image img, TileInfo.MediaTypes type, ImageSource custom)
        {
            if (type == TileInfo.MediaTypes.None)
            {
                if (img.Opacity >= 1)
                {
                    img.FadeOut(300);
                }
            }
            else if (type == TileInfo.MediaTypes.ProcessFile)
            {
                var icon = System.Drawing.SystemIcons.Error;
                FileInfo p = new FileInfo(TileAction.ProcessStartName);

                if(p.Exists)
                {
                    icon.Dispose();
                    icon = System.Drawing.Icon.ExtractAssociatedIcon(p.FullName);
                }

                img.Source = IconToImage(icon);

                icon.Dispose();

                if (img.Opacity < 1)
                {
                    img.FadeIn(300);
                }
            }
            else if (type == TileInfo.MediaTypes.Bytes)
            {
                if (custom != null)
                {
                    if (img.Opacity < 1)
                        img.FadeIn(300);

                    img.Source = custom;
                }
                else
                {
                    if (img.Opacity < 1)
                        img.FadeOut(300);
                }
            }

            if (TileSize == TileSizes.Small)
            {
                img.Width = 32;
                img.Height = 32;
            }
            else
            {
                img.Width = 64;
                img.Height = 64;
            }

        }

        public static ImageSource IconToImage(System.Drawing.Icon icon)
        {
            return Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()); 
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateTimer.Elapsed += UpdateTimer_Elapsed;
            UpdateTimer.Start();
        }

        private void UpdateTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(() => UpdateActions()));
            
        }

        private void GridMouseCollision_MouseMove(object sender, MouseEventArgs e)
        {
            var pos = e.GetPosition(GlowCanvas);
            var glowsize = MouseGlow.Width;

            Canvas.SetLeft(MouseGlow, pos.X - glowsize / 2);
            Canvas.SetTop(MouseGlow, pos.Y - glowsize / 2);

            //if (pos.X < glowsize || pos.X > glowsize + this.Width ||
            //   pos.Y < glowsize || pos.Y > glowsize + this.Height)
            //{
            //    if (MouseGlow.Opacity >= 1)
            //    {
            //        MouseGlow.FadeOut(200);
            //    }
            //}
            //else
            //{
            //    if (MouseGlow.Opacity < 1)
            //    {
            //        MouseGlow.FadeIn(200);
            //    }
            //}
        }

        private void GridMouseCollision_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void GridMouseCollision_MouseEnter(object sender, MouseEventArgs e)
        {
            MouseGlow.FadeIn(200);
        }

        private void GridMouseCollision_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TileAction.Do();
        }

        private void GridMouseCollision_MouseLeave(object sender, MouseEventArgs e)
        {
                MouseGlow.FadeOut(200);
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

        public enum MediaTypes
        {
            None,
            File,
            ProcessFile,
            Bytes
        }

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

        [NonSerialized]
        public Tile Parent = null;

        public void Do()
        {
            if (Action == Actions.ProcessStart)
                Process.Start(ProcessStartName, ProcessStartArguments);
        }

        public TileAction() { }

        public TileAction(Tile p)
        {
            Parent = p;
        }
    }
}
