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
using static Home.NET.Tiles.TileEnums;
using static Home.NET.Tiles.ContainerEnums;
using System.Diagnostics;

namespace Home.NET.Tiles
{
    /// <summary>
    /// Interaction logic for NormalContainer.xaml
    /// </summary>
    public partial class TileContainer : UserControl
    {
        public TilesPanel ParentPanel = null;

        public List<TileContainer> Containers
        {
            get
            {
                List<TileContainer> result = new List<TileContainer>();

                foreach (var c in ContainerPanel.Children)
                {
                    if (c is TileContainer)
                    {
                        result.Add((TileContainer)c);

                        result.AddRange((c as TileContainer).Containers);
                    }
                }

                return result;
            }
        }

        public List<Tile> LocalTiles
        {
            get
            {
                List<Tile> result = new List<Tile>();

                foreach (var o in ContainerPanel.Children)
                {
                    if (o is Tile)
                        result.Add((Tile)o);
                }

                return result;
            }
        }

        public List<Tile> Tiles
        {
            get
            {
                List<Tile> result = new List<Tile>();

                foreach (var o in ContainerPanel.Children)
                {
                    if (o is Tile)
                        result.Add((Tile)o);

                    if (o is TileContainer)
                        result.AddRange((o as TileContainer).Tiles);
                }

                return result;
            }
        }

        public int TotalObjects
        {
            get => ContainerPanel.Children.Count;
        }

        public bool CanAddMoreTiles
        {
            get
            {
                var total = TotalObjects;

                if (ContainerType == ContainerTypes.SmallToNormal && total >= MaxTilesForType.SmallToNormal)
                    return false;
                else if (ContainerType == ContainerTypes.NormalToWide && total >= MaxTilesForType.NormalToWide)
                    return false;
                else if (ContainerType == ContainerTypes.WideToBig && total >= MaxTilesForType.WideToBig)
                    return false;
                else if (ContainerType == ContainerTypes.Big && total >= MaxTilesForType.Big)
                    return false;

                return true;
            }
        }
        public bool IsContainerCompatibleWith(TileContainer another)
        {
            var parent = this;

            ContainerTypes p = parent.ContainerType;
            ContainerTypes c = another.ContainerType;

            if (p == ContainerTypes.SmallToNormal) // you CAN'T add any container into small one
                return false;
            else if (c == ContainerTypes.SmallToNormal && p == ContainerTypes.NormalToWide) // you CAN add normal container into wide
                return true;
            else if (c == ContainerTypes.NormalToWide && p == ContainerTypes.WideToBig) // you CAN add wide container into big
                return true;
            else if (c == ContainerTypes.WideToBig && p == ContainerTypes.Big) // you CAN'T add big into big
                return false;

            return false;
        }

        public TileContainer(ContainerTypes type = ContainerTypes.NormalToWide)
        {
            InitializeComponent();

            ContainerType = type;
        }

        public override string ToString()
        {
            return "{TileContainer: \"" + ContainerName + "\", ContainerType = " + ContainerType.ToString() + "}";
        }

        public string ContainerName = TilesUtils.RandomString(5);

        public bool CanThisObjectBeAdded(UIElement TileOrContainer)
        {
            if (TileOrContainer is Tile)
            {
                Tile tile = (Tile)TileOrContainer;

                if (tile.TileSize == GetContainerSize(this))
                    return true;
            }
            else if (TileOrContainer is TileContainer)
            {
                TileContainer cnt = (TileContainer)TileOrContainer;

                return this.IsContainerCompatibleWith(cnt);
            }

            return false;
        }

        /// <summary>
        /// Add Tile(s) to the container
        /// </summary>
        /// <param name="tile">Tile instance</param>
        public void Add(params Tile[] tiles)
        {
            for (int i = 0; i < tiles.Length; i++)
            {
                Tile tile = tiles[i];

                if (!CanAddMoreTiles)
                {
                    ParentPanel.Add(new Tile[] { tile }, this);
                    continue;
                }

                var padding = GetContainerPadding(this, i);
                tile.Margin = padding.Thickness;
                tile.TileSize = GetContainerSize(this);

                ContainerPanel.Orientation = padding.Orientation;
                ContainerPanel.Children.Add(tile);

                Debugger.Log(0, Debugger.DefaultCategory, this + " += " + tile);
            }
        }

        /// <summary>
        /// Add container to this container
        /// </summary>
        /// <param name="container">Container instance</param>
        public void Add(TileContainer container)
        {
            if (container.ContainerType == ContainerType)
                throw new Exception("Container has bad size");

            var padding = GetContainerPadding(this, TotalObjects);
            container.Margin = padding.Thickness;

            ContainerPanel.Orientation = padding.Orientation;
            ContainerPanel.Children.Add(container);

            Debugger.Log(0, Debugger.DefaultCategory, this + " += " + container);
        }
        
        /// <summary>
        /// Remove container or tile from this container
        /// </summary>
        /// <param name="TileOrContainer">Object instance</param>
        /// <param name="throwIntoPanel">Add objects to the panel instead of disposing them</param>
        public void Remove(UIElement TileOrContainer, bool ThrowIntoPanel)
        {
            var obj = TileOrContainer;

            ContainerPanel.Children.Remove(TileOrContainer);

            if (ThrowIntoPanel)
            {
                if (TileOrContainer is Tile)
                    ParentPanel.Add(new Tile[] { (Tile)TileOrContainer }, this);
                else if (TileOrContainer is TileContainer)
                    ParentPanel.Add(new TileContainer[] { (TileContainer)TileOrContainer }, this);
            }
        }

        private ContainerTypes containerType = ContainerTypes.NormalToWide;
        /// <summary>
        /// Set container's type / size.
        /// </summary>
        public ContainerTypes ContainerType
        {
            get => containerType;
            set
            {
                containerType = value;
                tbDebugText.Text = ContainerType.ToString();
                
                // Small
                if (containerType == ContainerTypes.SmallToNormal)
                {
                    var normalSize = EnumToSize(TileSizes.Normal);
                    Height = normalSize.Height;
                    Width = normalSize.Width;

                    for (int i = 0; i < ContainerPanel.Children.Count; i++)
                    {
                        var o = ContainerPanel.Children[i];

                        if (i >= MaxTilesForType.SmallToNormal)
                        {
                            Remove(o, true);
                        }
                    }
                }
                // Normal
                else if (containerType == ContainerTypes.NormalToWide)
                {
                    var wideSize = EnumToSize(TileSizes.Wide);
                    Height = wideSize.Height;
                    Width = wideSize.Width;

                    for (int i = 0; i < ContainerPanel.Children.Count; i++)
                    {
                        var o = ContainerPanel.Children[i];

                        if (i >= MaxTilesForType.NormalToWide)
                        {
                            Remove(o, true);
                        }
                    }
                }
                // Wide & Big
                else if (containerType == ContainerTypes.WideToBig || containerType == ContainerTypes.Big)
                {
                    var bigSize = EnumToSize(TileSizes.Big);
                    Height = bigSize.Height;
                    Width = bigSize.Width;

                    for (int i = 0; i < ContainerPanel.Children.Count; i++)
                    {
                        var o = ContainerPanel.Children[i];

                        if ((containerType == ContainerTypes.WideToBig && i >= MaxTilesForType.WideToBig) || (containerType == ContainerTypes.Big && i >= MaxTilesForType.Big))
                            Remove(o, true);
                    }
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
