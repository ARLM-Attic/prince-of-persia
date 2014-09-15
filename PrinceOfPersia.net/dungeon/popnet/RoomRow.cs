	//-----------------------------------------------------------------------//
	// <copyright file="RoomRow.cs" company="A.D.F.Software">
	// Copyright "A.D.F.Software" (c) 2014 All Rights Reserved
	// <author>Andrea M. Falappi</author>
	// <date>Wednesday, September 24, 2014 11:36:49 AM</date>
	// </copyright>
	//
	// * NOTICE:  All information contained herein is, and remains
	// * the property of Andrea M. Falappi and its suppliers,
	// * if any.  The intellectual and technical concepts contained
	// * herein are proprietary to A.D.F.Software
	// * and its suppliers and may be covered by World Wide and Foreign Patents,
	// * patents in process, and are protected by trade secret or copyright law.
	// * Dissemination of this information or reproduction of this material
	// * is strictly forbidden unless prior written permission is obtained
	// * from Andrea M. Falappi.
	//-----------------------------------------------------------------------//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PrinceOfPersia.PopNet;

namespace PrinceOfPersia.PopNet
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
