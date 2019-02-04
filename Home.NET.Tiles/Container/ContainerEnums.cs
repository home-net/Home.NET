using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using static Home.NET.Tiles.ContainerEnums;
using static Home.NET.Tiles.TileEnums;

namespace Home.NET.Tiles
{
    public class MaxTilesForType
    {
		//               |* *|
		//               |* *|
        public const int SmallToNormal = 4; // only 4 small tiles here
		//               |* *|
        public const int NormalToWide = 2; // only 2 normal tiles here
		//               | * |
		//               | * |
        public const int WideToBig = 2; // only 2 wide tiles here
		//               |   |
		//               |   |
        public const int Big = 1; // only 1 big tile here
    }

    public class ContainerPadding
    {
        public Thickness Thickness;
        public System.Windows.Controls.Orientation Orientation;

		public ContainerPadding(Thickness t, System.Windows.Controls.Orientation d)
        {
            Thickness = t;
            Orientation = d;
        }
    }

    public class ContainerEnums
    {
        public enum ContainerTypes
        {
            SmallToNormal,
            NormalToWide,
            WideToBig,
			Big
        }

        public static List<TileContainer> GenerateContainersForElements(params Tile[] elements)
        {
            List<TileContainer> result = new List<TileContainer>();

            TilesParsedBySize tiles = new TilesParsedBySize(elements);

            for (int i = 0; i < tiles.Small.Count; i++)
            {
                TileContainer cnt = new TileContainer(ContainerTypes.SmallToNormal);

                for (int idx = 0; idx < MaxTilesForType.SmallToNormal; idx++, i++)
                {
                    if (idx >= tiles.Small.Count)
                        break;
					
                    cnt.Add(tiles.Small[i]);
                }

				i--;

                result.Add(cnt);
            }

            for (int i = 0; i < tiles.Normal.Count; i++)
            {
                TileContainer cnt = new TileContainer(ContainerTypes.NormalToWide);

                for (int idx = 0; idx < MaxTilesForType.NormalToWide; idx++, i++)
                {
                    if (idx >= tiles.Normal.Count)
                        break;

                    cnt.Add(tiles.Normal[i]);
                }

                i--;

                result.Add(cnt);
            }

            for (int i = 0; i < tiles.Wide.Count; i++)
            {
                TileContainer cnt = new TileContainer(ContainerTypes.WideToBig);

                for (int idx = 0; idx < MaxTilesForType.WideToBig; idx++, i++)
                {
                    if (idx >= tiles.Wide.Count)
                        break;
					
                    cnt.Add(tiles.Wide[i]);
                }

                i--;

                result.Add(cnt);
            }

            for (int i = 0; i < tiles.Big.Count; i++)
            {
                TileContainer cnt = new TileContainer(ContainerTypes.Big);

                for (int idx = 0; idx < MaxTilesForType.Big; idx++, i++)
                {
                    if (idx >= tiles.Wide.Count)
                        break;

                    cnt.Add(tiles.Big[i]);
                }

                i--;

                result.Add(cnt);
            }

            return result;
        }
		
        public static ContainerPadding GetContainerPadding(TileContainer container, int tileIndex)
        {
            return GetContainerPadding(container.ContainerType, tileIndex);
        }

        public static ContainerPadding GetContainerPadding(ContainerTypes type, int tileIndex)
        {
            System.Windows.Controls.Orientation o = System.Windows.Controls.Orientation.Horizontal;

            if (type == ContainerTypes.SmallToNormal)
            {
                o = System.Windows.Controls.Orientation.Vertical;

                switch (tileIndex)
                {
                    case 0: return new ContainerPadding(new Thickness(0, 0, TilePadding, TilePadding), o); 
                    case 1: return new ContainerPadding(new Thickness(0, 0, TilePadding, 0), o);
                    case 2: return new ContainerPadding(new Thickness(0), o);
                    case 3: return new ContainerPadding(new Thickness(0), o);
                }
            }

            if (type == ContainerTypes.NormalToWide)
            {
                o = System.Windows.Controls.Orientation.Horizontal;

                switch (tileIndex)
                {
                    case 0: return new ContainerPadding(new Thickness(0, 0, TilePadding, 0), o);  // |*| | |
                    case 1: return new ContainerPadding(new Thickness(0), o);                     // | | |*|
                }
            }

            if (type == ContainerTypes.WideToBig)
            {
                o = System.Windows.Controls.Orientation.Vertical;

                switch (tileIndex)
                {
                    case 0: return new ContainerPadding(new Thickness(0, 0, 0, TilePadding), o);
                    case 1: return new ContainerPadding(new Thickness(0), o);
                }
            }

            if (type == ContainerTypes.Big)
            {
                o = System.Windows.Controls.Orientation.Horizontal;

                return new ContainerPadding(new Thickness(0), o);
            }
			
            return new ContainerPadding(new Thickness(0), o);
        }

        public static TileSizes GetContainerSize(TileContainer container)
        {
            return GetContainerSize(container.ContainerType);
        }
        
        public static TileSizes GetContainerSize(ContainerTypes type)
        {
            if (type == ContainerTypes.SmallToNormal)
                return TileSizes.Small;
            else if (type == ContainerTypes.NormalToWide)
                return TileSizes.Normal;
            else if (type == ContainerTypes.WideToBig)
                return TileSizes.Wide;
            else
                return TileSizes.Big;
        }
    }
}
