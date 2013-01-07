using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrinceOfPersia
{
    public class Column
    {
        public TileType tileType;
        public SpriteType spriteType;
        public string state = StateTileElement.State.normal.ToString();
        public int switchButton = 0;
        
        public Column()
        { 
        
        }
    }
}
