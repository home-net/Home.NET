using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;

namespace Home.NET.Tiles
{
    public class TilesUtils
    {
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }

    //public class TileSize
    //{
    //    public static Size Small = new Size(57, 57);
    //    public static Size Normal = new Size(128, 128);
    //    public static Size Wide = new Size(256, 128);
    //    public static Size Big = new Size(256, 256);
    //}
}
