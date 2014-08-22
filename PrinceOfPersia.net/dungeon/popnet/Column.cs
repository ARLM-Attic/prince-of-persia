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
