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

    public class Maze
    {
        // Level content.        
        public ContentManager content;
        public List<Level> levels = new List<Level>();
        private RoomNew playerRoom;
        public List<RoomNew> rooms = new List<RoomNew>();
        public Player player;

        // Physics state
        public RoomNew PlayerRoom
        {
            get 
            { 
                return playerRoom; 
            }
            set 
            { 
                playerRoom = value; 
            }
        }
        Vector2 positionArrive;


        public Maze(ContentManager contentmanager)
        {
            content = contentmanager;
            
            //LOAD MXL CONTENT
            //string binPath = System.AppDomain.CurrentDomain.BaseDirectory;
            TextReader txtReader = File.OpenText(PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_LEVELS + "LEVEL_dungeon_prison.xml");

            System.Xml.Serialization.XmlSerializer ax = null;
            ax = new System.Xml.Serialization.XmlSerializer(typeof(Level));
            levels.Add((Level)ax.Deserialize(txtReader));
            
            
            

            //LOAD ALL LEVEL IN LIST
            //System.Xml.Serialization.XmlSerializer ax;
            //Stream astream = this.GetType().Assembly.GetManifestResourceStream(path_resources + "LEVEL_dungeon_prison.xml");
            //ax = new System.Xml.Serialization.XmlSerializer(typeof(Level));
            //levels.Add((Level)ax.Deserialize(astream));

  
            //load all room
            for (int z = 0; z < levels.Count(); z++)
            {
                for (int y = 0; y < levels[z].rows.Count(); y++)
                {
                    for (int x = 0; x < levels[z].rows[y].columns.Count(); x++)
                    {
                        if (levels[z].rows[y].columns[x].FilePath == string.Empty)
                            levels[z].rows[y].columns[x].FilePath = "MAP_blockroom.xml";

                        RoomNew room = new RoomNew(this, PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_ROOMS + levels[z].rows[y].columns[x].FilePath);
                        //RoomNew room = new RoomNew(this, "Maze/"+ levels[z].rows[y].columns[x].FilePath);
                        room.roomStart = levels[z].rows[y].columns[x].RoomStart;
                        room.roomName = levels[z].rows[y].columns[x].FilePath;
                        room.roomIndex = levels[z].rows[y].columns[x].RoomIndex;
                        room.roomZ = z;
                        room.roomX = x;
                        room.roomY = y;
                        rooms.Add(room);
                    }
                }
            }
        }

        public List<Tile> GetTiles(Enumeration.TileType tileType)
        {
            List<Tile> list = new List<Tile>();
            foreach (RoomNew r in rooms)
            {
                foreach (Tile t in r.GetTiles(tileType))
                {
                    list.Add(t);
                }
                //list.Concat(r.GetTiles(tileType));
            }
            return list;
        }

        public RoomNew LeftRoom(RoomNew room)
        {
            int x = room.roomX;
            int y = room.roomY;
            int z = room.roomZ;

            if (x != 0)
                x = --x;
            else
                return new RoomNew(this, PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_ROOMS + "MAP_blockroom.xml");

            foreach (RoomNew r in rooms)
            {
                if (r.roomX == x & r.roomY == y & r.roomZ == z)
                    return r;
            }
            return new RoomNew(this, PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_ROOMS + "MAP_blockroom.xml");
        }

        public RoomNew DownRoom(RoomNew room)
        {
            int x = room.roomX;
            int y = room.roomY;
            int z = room.roomZ;

            if (y != levels[z].rows.Count() - 1)
                y = ++y;
            else
                return new RoomNew(this, PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_ROOMS + "MAP_blockroom.xml");

            foreach (RoomNew r in rooms)
            {
                if (r.roomX == x & r.roomY == y & r.roomZ == z)
                    return r;
            }
            return new RoomNew(this, PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_ROOMS + "MAP_blockroom.xml");
        }

        public RoomNew RightRoom(RoomNew room)
        {
            int x = room.roomX;
            int y = room.roomY;
            int z = room.roomZ;

            if (x != levels[z].rows[y].columns.Count() - 1)
                x = ++x;
            else
                return new RoomNew(this, PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_ROOMS + "MAP_blockroom.xml");

            foreach (RoomNew r in rooms)
            {
                if (r.roomX == x & r.roomY == y & r.roomZ == z)
                    return r;
            }
            return new RoomNew(this, PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_ROOMS + "MAP_blockroom.xml");
        }

        public RoomNew UpRoom(RoomNew room)
        {
            int x = room.roomX;
            int y = room.roomY;
            int z = room.roomZ;

            if (y != levels[z].rows.Count() - 1)
                y = --y;
            else
                return new RoomNew(this, PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_ROOMS + "MAP_blockroom.xml");

            foreach (RoomNew r in rooms)
            {
                if (r.roomX == x & r.roomY == y & r.roomZ == z)
                    return r;
            }
            return new RoomNew(this, PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_ROOMS + "MAP_blockroom.xml");
        }

        public RoomNew StartRoom()
        {
            foreach (RoomNew r in rooms)
            {
                if (r.roomStart == true)
                {
                    playerRoom = r;
                    return r;
                }
            }
            return new RoomNew(this, PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_ROOMS + "MAP_blockroom.xml");
        }


        public RoomNew FindRoom(string roomName)
        {
            return null;
        }

    }
}
