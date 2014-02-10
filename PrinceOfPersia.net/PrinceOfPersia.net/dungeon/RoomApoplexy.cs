using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrinceOfPersia
{
    public class RoomApoplexy
    {
        public string number = "0";
        private int size = 30;
        public TileApoplexy[] tiles;

        public RoomApoplexy()
        {
            tiles = new TileApoplexy[size];
            for (int x = 0; x < tiles.Length; x++)
            {
                tiles[x] = new TileApoplexy();
            }

        }

        //public RoomApoplexy(int sizeX)
        //{
        //    columns = new Column[sizeX];
        //    for(int x = 0; x < columns.Length; x++)
        //    {
        //        columns[x] = new Column();
        //    }
        //}

    }
}
