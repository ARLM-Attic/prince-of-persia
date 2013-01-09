using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrinceOfPersia
{
    public class Column
    {
        public Enumeration.TileType tileType;
        public Enumeration.SpriteType spriteType;
        public string state = Enumeration.StateTile.normal.ToString();
        public int switchButton = 0;
        
        public Column()
        { 
        
        }
    }
}
