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
        public RoomNew playerRoom;
        public List<RoomNew> rooms = new List<RoomNew>();
        private string path_resources;

        public Maze(ContentManager contentpar, string path_resourcespar)
        {
            path_resources = path_resourcespar;
            content = contentpar;

            //LOAD ALL LEVEL IN LIST
            System.Xml.Serialization.XmlSerializer ax;
            Stream astream = this.GetType().Assembly.GetManifestResourceStream(path_resources + "LEVEL_dungeon_prison.xml");
            ax = new System.Xml.Serialization.XmlSerializer(typeof(Level));
            levels.Add((Level)ax.Deserialize(astream));

  
            //load all room
            for (int z = 0; z < levels.Count(); z++)
            {
                for (int y = 0; y < levels[z].rows.Count(); y++)
                {
                    for (int x = 0; x < levels[z].rows[y].columns.Count(); x++)
                    {
                        if (levels[z].rows[y].columns[x].FilePath == string.Empty)
                            levels[z].rows[y].columns[x].FilePath = "MAP_blockroom.xml";

                        RoomNew room = new RoomNew(this, path_resources + levels[z].rows[y].columns[x].FilePath);
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

        public List<Tile> GetTiles(TileType tileType)
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
                return new RoomNew(this, path_resources + "MAP_blockroom.xml");

            foreach (RoomNew r in rooms)
            {
                if (r.roomX == x & r.roomY == y & r.roomZ == z)
                    return r;
            }
            return null;
        }

        public RoomNew DownRoom(RoomNew room)
        {
            int x = room.roomX;
            int y = room.roomY;
            int z = room.roomZ;

            if (y != levels[z].rows.Count() - 1)
                y = ++y;
            else
                return new RoomNew(this, path_resources + "MAP_blockroom.xml");

            foreach (RoomNew r in rooms)
            {
                if (r.roomX == x & r.roomY == y & r.roomZ == z)
                    return r;
            }
            return null;
        }

        public RoomNew RightRoom(RoomNew room)
        {
            int x = room.roomX;
            int y = room.roomY;
            int z = room.roomZ;

            if (x != levels[z].rows[y].columns.Count() - 1)
                x = ++x;
            else
                return new RoomNew(this, path_resources + "MAP_blockroom.xml");

            foreach (RoomNew r in rooms)
            {
                if (r.roomX == x & r.roomY == y & r.roomZ == z)
                    return r;
            }
            return null;
        }

        public RoomNew UpRoom(RoomNew room)
        {
            int x = room.roomX;
            int y = room.roomY;
            int z = room.roomZ;

            if (y != 0)
                y = --y;
            else
                return new RoomNew(this, path_resources + "MAP_blockroom.xml");

            foreach (RoomNew r in rooms)
            {
                if (r.roomX == x & r.roomY == y & r.roomZ == z)
                    return r;
            }
            return null;
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
            return new RoomNew(this, path_resources + "MAP_blockroom.xml");
        }

    }
}
