using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrinceOfPersia
{
    public class RoomRow
    {
        public RoomColumn[] columns;

        public RoomRow()
        {
            columns = new RoomColumn[10];
            for (int x = 0; x < columns.Length; x++)
            {
                columns[x] = new RoomColumn();
            }

        }

        public RoomRow(int sizeX)
        {
            columns = new RoomColumn[sizeX];
            for(int x = 0; x < columns.Length; x++)
            {
                columns[x] = new RoomColumn();
            }
        }

        //public RoomColumn LeftColumn(RoomColumn roomColumn)
        //{
        //    for (int x = 0; x < columns.Length; x++)
        //    {
        //        if (columns[x] == roomColumn)
        //        {
        //            if (x != 0)
        //                return columns[x - 1];
        //        }
        //    }
        //    return null;
        //}

        //public RoomColumn RightColumn(RoomColumn roomColumn)
        //{
        //    for (int x = 0; x < columns.Length; x++)
        //    {
        //        if (columns[x] == roomColumn)
        //        {
        //            if (x != columns.Length-1)
        //                return columns[x + 1];
        //        }
        //    }
        //    return null;
        //}

    }
}
