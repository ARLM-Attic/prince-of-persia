using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace PrinceOfPersia
{
    public class Column
    {
        public Enumeration.TileType tileType;
        public Enumeration.SpriteType spriteType;
        public SpriteEffects spriteEffect;
        public string state = Enumeration.StateTile.normal.ToString();
        public int switchButton = 0;
        
        public Column()
        { 
        
        }
    }
}
