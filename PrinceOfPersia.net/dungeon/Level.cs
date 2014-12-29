	//-----------------------------------------------------------------------//
	// <copyright file="Level.cs" company="A.D.F.Software">
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
            blockRoom = new Room(maze, this, PoP.CONFIG_PATH_CONTENT + PoP.CONFIG_PATH_ROOMS + "MAP_blockroom.xml", 0);
            blockRoom.roomName = "MAP_blockroom.xml";
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
