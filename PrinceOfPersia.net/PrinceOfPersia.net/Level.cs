using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Input;

namespace PrinceOfPersia
{
    public enum LevelName
    { 
        dungeon_prison = 1,
		dungeon_guards = 2,
        dungeon_skeleton = 3,
        palace_mirror = 4,
        palace_thief = 5,
        palace_plunge = 6,
        dungeon_weightless = 7,
        dungeon_mouse = 8,
        dungeon_twisty = 9,
        palace_quad = 10,
        palace_fragile = 11,
        dungeon_tower = 12,
        dungeon_jaffar = 13,
        palace_rescue = 14,
        dungeon_potions = 15,
        dungeon_demo = 0
    }

    public class Level
    {
        public RoomRow[] rows;
        public LevelName levelName = LevelName.dungeon_demo;
        

        public Level()
        {
            rows = new RoomRow[10];

            for (int x = 0; x < rows.Count(); x++)
            {
                rows[x] = new RoomRow(10);
            }
        }

        public Level(int sizeRow, int sizeColumn)
        {
            rows = new RoomRow[sizeRow];

            for (int x = 0; x < rows.Count(); x++)
            {
                rows[x] = new RoomRow(sizeColumn);
            }

        }

        //public RoomNew UpRoom(RoomNew room)
        //{
        //    for (int y = 0; y < rows.Count(); y++)
        //    {
        //        for (int x = 0; x < rows[y].columns.Count(); x++)
        //        {
        //            rows[y].columns[x]. == 
        //        }
        //    }
        //    return null;
        //}


        public void Serialize()
        { 
            System.Xml.Serialization.XmlSerializer ax = new System.Xml.Serialization.XmlSerializer(typeof(Level));
            TextWriter writer = new StreamWriter(@"C:\temp\LEVEL_"+ levelName.ToString() +".xml");
            ax.Serialize(writer, this);
        }

    }
}
