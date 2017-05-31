﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrinceOfPersia
{
    public class Map
    {
        public Row[] rows;

        public Map(int sizeX, int sizeY)
        {
            rows = new Row[sizeY];
            for (int x = 0; x < rows.Length; x++)
            {
                rows[x] = new Row(sizeX);
            }
        }

        public Map()
        {
            rows = new Row[3];
            for (int x = 0; x < rows.Length; x++)
            {
                rows[x] = new Row(10);
            }
        }
    }
}
