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
using System.Xml.Serialization;
using PrinceOfPersia.Apoplexy;

namespace PrinceOfPersia.Apoplexy
{


    public class level
    {
        public room[] rooms;
        //public Enumeration.LevelName levelName = Enumeration.LevelName.dungeon_demo;
        private string _number = "0";

        public @event[] events;


        [XmlElement]
        public userdata userdata;


        [XmlElement]
        public prince prince;


        [XmlAttribute("number")]
        public string number
        {
            get { return _number; }
            set { _number = value; }
        }


        public level()
        {
            rooms = new room[10];

            for (int x = 0; x < rooms.Count(); x++)
            {
                rooms[x] = new room();
            }
        }



        public void Serialize()
        { 
            System.Xml.Serialization.XmlSerializer ax = new System.Xml.Serialization.XmlSerializer(typeof(level));
            //TextWriter writer = new StreamWriter(@"C:\temp\LEVEL_"+ levelName.ToString() +".xml");
            TextWriter writer = new StreamWriter(@"C:\temp\LEVEL_Apoplexy.xml");
            ax.Serialize(writer, this);
        }

    }
}
