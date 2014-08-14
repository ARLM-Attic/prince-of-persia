using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrinceOfPersia.PopNet;

namespace PrinceOfPersia.PopNet
{
    public class Row
    {
        public Column[] columns;

        public Row()
        {
            columns = new Column[10];
            for (int x = 0; x < columns.Length; x++)
            {
                columns[x] = new Column();
            }

        }

        public Row(int sizeX)
        {
            columns = new Column[sizeX];
            for(int x = 0; x < columns.Length; x++)
            {
                columns[x] = new Column();
            }
        }

    }
}
