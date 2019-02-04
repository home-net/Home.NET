using Home.NET.Tiles.Extensions;
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
using static Home.NET.Tiles.ContainerEnums;
using static Home.NET.Tiles.TileEnums;

namespace Home.NET.Tiles
{
    /// <summary>
    /// Interaction logic for TilesPanel.xaml
    /// </summary>
    public partial class TilesPanel : UserControl
    {
        public List<TileContainer> Containers
        {
            get
            {
                List<TileContainer> result = new List<TileContainer>();

                foreach(var c in MainGrid.Children)
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

        public static Random tRand = new Random();

        private TileStyles panelStyle = TileStyles.Metro;
        public TileStyles PanelStyle
        {
            get => panelStyle;
            set
            {
                panelStyle = value;

                foreach (var tile in TilesList)
                {
                    tile.TileStyle = value;
                }
            }
        }

        private double panelScale = 1;
        public double PanelScale
        {
            get => panelScale;
            set
            {
                panelScale = value;

                foreach (var tile in TilesList)
                {
                    tile.TileScale = value;
                }
            }
        }

        public TilesPanel()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// Add Cotainer(s) to the Panel.
        /// </summary>
        /// <param name="containers">Cotainers Instances</param>
        public void Add(params TileContainer[] containers)
        {
            Add(containers, null);
        }

        /// <summary>
        /// Add Tile(s) to the Panel.
        /// </summary>
        /// <param name="tiles">Tiles Instances</param>
        public void Add(params Tile[] tiles)
        {
            Add(tiles, null);
        }

        /// <summary>
        /// Add Tiles to the Panel.
        /// </summary>
        /// <param name="tiles">Tiles array</param>
        /// <param name="ignore">Do not add in this container</param>
        public void Add(Tile[] tiles, TileContainer ignore = null)
        {
            List<Tile> ToAdd = new List<Tile>();

            for (int i = 0; i < tiles.Length; i++)
            {
                var compat = SearchForCompatibleContainer(tiles[i], ignore);
                if (compat != null)
                {
                    compat.Add(tiles[i]);

                    if (IsLoaded)
                        tiles[i].FadeIn(400);

                    continue;
                }
                else
                {
                    ToAdd.Add(tiles[i]);
                }
            }

            if (ToAdd.Count <= 0)
                return;

            List<TileContainer> NewContainers = GenerateContainersForElements(ToAdd.ToArray());

            foreach (var cont in NewContainers)
            {
                Add(cont);

                if (IsLoaded)
                    foreach (var tile in cont.Tiles)
                        tile.FadeIn(450);
            }
        }

        /// <summary>
        /// Add Containers to the Panel.
        /// </summary>
        /// <param name="tiles">Containers array</param>
        /// <param name="ignore">Do not add in this container</param>
        public void Add(TileContainer[] containers, TileContainer ignore = null)
        {

            for (int i = 0; i < containers.Length; i++)
            {
                Debugger.Log(0, Debugger.DefaultCategory, "Panel += " + containers[i]);

                var compat = SearchForCompatibleContainer(containers[i], ignore);
                if (compat != null)
                {
                    compat.Add(containers[i]);
                    continue;
                }
                else
                {
                    TileContainer resultContainer = containers[i];
                    TileContainer previousContainer;

                    if (resultContainer.ContainerType == ContainerTypes.SmallToNormal)
                    {
                        previousContainer = resultContainer;

                        resultContainer = new TileContainer(ContainerTypes.NormalToWide);
                        resultContainer.Add(previousContainer);
                    }

                    if (resultContainer.ContainerType == ContainerTypes.NormalToWide)
                    {
                        previousContainer = resultContainer;

                        resultContainer = new TileContainer(ContainerTypes.WideToBig);
                        resultContainer.Add(previousContainer);
                    }
                    
                    if (resultContainer.ContainerType == ContainerTypes.WideToBig)
                    {
                        previousContainer = resultContainer;

                        resultContainer = new TileContainer(ContainerTypes.Big);
                        resultContainer.Add(previousContainer);
                    }

                    resultContainer.Margin = TilesPanelEnums.PanelDefaultPadding;
                    MainGrid.Children.Add(resultContainer);
                }


                if (IsLoaded)
                    foreach (var tile in containers[i].Tiles)
                        tile.FadeIn(450);
            }
        }

        public TileContainer SearchForCompatibleContainer(TileContainer element, TileContainer ignore)
        {
            foreach (var cont in Containers)
            {
                if (cont == ignore)
                    continue;

                if (cont.CanAddMoreTiles && cont.IsContainerCompatibleWith(element))
                {
                    return cont;
                }
            }

            return null;
        }

        public TileContainer SearchForCompatibleContainer(Tile element, TileContainer ignore)
        {
            foreach (var cont in Containers)
            {
                if (cont == ignore)
                    continue;

                if (cont.CanAddMoreTiles && GetContainerSize(cont) == element.TileSize)
                {
                    return cont;
                }
            }

            return null;
        }

        public void DebugContainer(string name)
        {
            foreach (var c in Containers)
            {
                if (c.ContainerName == name.Trim(new char[] { ' ', '\t', "'"[0] }))
                {
                    var container = c;
                    // ^ here is this container

                    if (!Debugger.IsAttached)
                        Debugger.Launch();

                    Debugger.Break();

                    return;
                }
            }

            Debugger.Log(0, Debugger.DefaultCategory, $"Container '{name}' not found");
        }
        
        public void Delete(Tile tile)
        {
            MainGrid.Children.Remove(tile);
        }

        public void AddTestTile(string text)
        {
            Tile tile = new Tile()
            {
                TileText = text,
                TileColor = Color.FromArgb(255, (byte)tRand.Next(0, 255), (byte)tRand.Next(0, 255), (byte)tRand.Next(0, 255)),
                TileSize = (TileSizes)tRand.Next(0, 3)
            };

            Add(tile);
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

                    if (o is TileContainer)
                    {
                        result.AddRange((o as TileContainer).Tiles);
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            int dur = 450;
            int start = 0;
            foreach (var tile in Tiles)
            {
                tile.FadeIn(dur, start);
                start += 100;
            }
        }
    }
}
