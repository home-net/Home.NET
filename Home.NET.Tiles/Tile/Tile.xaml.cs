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
using static Home.NET.Tiles.TileEnums;

namespace Home.NET.Tiles
{
    /// <summary>
    /// Interaction logic for Tile.xaml
    /// </summary>
    public partial class Tile : UserControl
    {
        private System.Timers.Timer UpdateTimer = new System.Timers.Timer(500);

        private double tileScale = 1;

        public Tile()
        {
            InitializeComponent();

            GridMouseCollision.Visibility = Visibility.Visible;
            MouseGlow.Opacity = 0;

            TileSize = TileSizes.Normal;
        }

        public string TileText
        {
            get => TileTextBlock.Text;
            set => TileTextBlock.Text = value;
        }
        
        private MediaTypes mediaTypeBackground = MediaTypes.None;
        private MediaTypes mediaTypeIcon = MediaTypes.None;

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
                MediaTypeIcon = MediaTypes.ProcessFile;
            }
            else if (TileAction.Action == TileAction.Actions.None)
            {
                MediaTypeIcon = MediaTypes.Bytes;
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

        public MediaTypes MediaTypeBackground
        {
            get => mediaTypeBackground;
            set
            {
                mediaTypeBackground = value;
                TileUpdateImage(IconMedia, value, CustomMediaBackground);
            }
        }
        public MediaTypes MediaTypeIcon
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

        public void TileUpdateImage(Image img, MediaTypes type, ImageSource custom)
        {
            if (type == MediaTypes.None)
            {
                if (img.Opacity >= 1)
                {
                    img.FadeOut(300);
                }
            }
            else if (type == MediaTypes.ProcessFile)
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
            else if (type == MediaTypes.Bytes)
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
