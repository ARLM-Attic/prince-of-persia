using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Input;

namespace PrinceOfPersia
{


    public class LevelApoplexy
    {
        public RoomApoplexy[] rooms;
        public Enumeration.LevelName levelName = Enumeration.LevelName.dungeon_demo;
        public string number = "0";

        public LevelApoplexy()
        {
            rooms = new RoomApoplexy[10];

            for (int x = 0; x < rooms.Count(); x++)
            {
                rooms[x] = new RoomApoplexy();
            }
        }



        public void Serialize()
        { 
            System.Xml.Serialization.XmlSerializer ax = new System.Xml.Serialization.XmlSerializer(typeof(LevelApoplexy));
            TextWriter writer = new StreamWriter(@"C:\temp\LEVEL_"+ levelName.ToString() +".xml");
            ax.Serialize(writer, this);
        }

    }
}
