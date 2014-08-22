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
using PrinceOfPersia.PopNet;

namespace PrinceOfPersia.PopNet
{


    public class Level
    {
        public RoomRow[] rows;
        public Enumeration.LevelName levelName = Enumeration.LevelName.dungeon_demo;


        public Level()
        {
            //rows = new RoomRow[10];

            //for (int x = 0; x < rows.Count(); x++)
            //{
            //    rows[x] = new RoomRow(10);
            //}
        }

        public Level(int sizeRow, int sizeColumn)
        {
            rows = new RoomRow[sizeRow];

            for (int x = 0; x < rows.Count(); x++)
            {
                rows[x] = new RoomRow(sizeColumn);
            }

        }





        public void Serialize()
        {
            System.Xml.Serialization.XmlSerializer ax = new System.Xml.Serialization.XmlSerializer(typeof(Level));
            TextWriter writer = new StreamWriter(@"C:\temp\LEVEL_" + levelName.ToString() + ".xml");
            ax.Serialize(writer, this);
        }

    }
}
