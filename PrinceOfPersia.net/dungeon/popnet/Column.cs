	//-----------------------------------------------------------------------//
	// <copyright file="Column.cs" company="A.D.F.Software">
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
using Microsoft.Xna.Framework.Graphics;
using PrinceOfPersia.PopNet;

namespace PrinceOfPersia.PopNet
{
    public class Column
    {
        public Enumeration.TileType tileType = Enumeration.TileType.block;
        public Enumeration.SpriteType spriteType;
        public SpriteEffects spriteEffect;
        public Enumeration.StateTile state = Enumeration.StateTile.normal;
        public int switchButton = 0;
        public Enumeration.Items item = Enumeration.Items.none;
        
        public Column()
        { 
        
        }
    }
}
