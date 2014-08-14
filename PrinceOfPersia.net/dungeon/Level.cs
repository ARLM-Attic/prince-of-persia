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


    public class Level
    {
        public Maze maze;
        public Room blockRoom;
        public List<Room> rooms;
        public Enumeration.LevelName levelName = Enumeration.LevelName.dungeon_demo;
        public List<Sprite> sprites = new List<Sprite>();

        public Level(Maze Maze, string LevelNumber)
        {
            this.maze = Maze;
            this.levelName = (Enumeration.LevelName) Enum.Parse(typeof(Enumeration.LevelName), LevelNumber.ToString());
            rooms = new List<Room>();

            blockRoom = new Room(maze, this, PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_ROOMS + "MAP_blockroom.xml", 1);
        }

        public Room FindRoom(int roomNumber)
        {
            foreach(Room r in rooms)
            {
                if (r.roomNumber == roomNumber)
                    return r;
            }
            return blockRoom;
        }

        public List<Tile> GetTiles(Enumeration.TileType tileType)
        {
            List<Tile> list = new List<Tile>();
            foreach (Room r in rooms)
            {
                foreach (Tile t in r.GetTiles(tileType))
                {
                    list.Add(t);
                }
                //list.Concat(r.GetTiles(tileType));
            }
            return list;
        }


        public Room StartRoom()
        {
            foreach (Room r in rooms)
            {
                if (r.roomStart == true)
                {
                    return r;
                }
            }
            return blockRoom;
        }


        public void Serialize()
        { 
            System.Xml.Serialization.XmlSerializer ax = new System.Xml.Serialization.XmlSerializer(typeof(Level));
            TextWriter writer = new StreamWriter(@"C:\temp\LEVEL_"+ levelName.ToString() +".xml");
            ax.Serialize(writer, this);
        }




    }
}
