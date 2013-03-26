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
        public GraphicsDevice graphicsDevice;
        public ContentManager content;
        public List<Level> levels = new List<Level>();
        public List<RoomNew> rooms = new List<RoomNew>();
        public Player player;
        public List<Guard> guards = new List<Guard>();
        
        private RoomNew playerRoom;
        private static RoomNew blockRoom;


        //List for retain and load maze tiles textures
        public static Dictionary<string, object> dTexture = null;
        public Vector2 positionArrive;


        public Maze(GraphicsDevice GraphicsDevice, ContentManager contentmanager)
        {
            content = contentmanager;
            graphicsDevice = GraphicsDevice;

            LoadContent();

            //LOAD MXL CONTENT
            Stream txtReader;
//#if ANDROID
            //txtReader = Game.Activity.Assets.Open(@PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_LEVELS + "LEVEL_dungeon_prison.xml");
//#endif
            txtReader = Microsoft.Xna.Framework.TitleContainer.OpenStream(PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_LEVELS + "LEVEL_dungeon_prison.xml");
            
            //txtReader = (Stream) File.OpenText(PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_LEVELS + "LEVEL_dungeon_prison.xml");

            System.Xml.Serialization.XmlSerializer ax = null;
            ax = new System.Xml.Serialization.XmlSerializer(typeof(Level));
            levels.Add((Level)ax.Deserialize(txtReader));

            //Define and build a generic blockroom for usefull
            blockRoom = new RoomNew(this, PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_ROOMS + "MAP_blockroom.xml");
            

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

        //Load all texture in a dictiornary
        private void LoadContent()
        {
            dTexture = Program.LoadContent<object>(content, "");


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
                return blockRoom;
                //return new RoomNew(this, PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_ROOMS + "MAP_blockroom.xml");

            foreach (RoomNew r in rooms)
            {
                if (r.roomX == x & r.roomY == y & r.roomZ == z)
                    return r;
            }
            return blockRoom;
            //return new RoomNew(this, PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_ROOMS + "MAP_blockroom.xml");
        }

        public RoomNew DownRoom(RoomNew room)
        {
            int x = room.roomX;
            int y = room.roomY;
            int z = room.roomZ;

            if (y != levels[z].rows.Count() - 1)
                y = ++y;
            else
                return blockRoom;
                //return new RoomNew(this, PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_ROOMS + "MAP_blockroom.xml");

            foreach (RoomNew r in rooms)
            {
                if (r.roomX == x & r.roomY == y & r.roomZ == z)
                    return r;
            }
            return blockRoom;
            //return new RoomNew(this, PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_ROOMS + "MAP_blockroom.xml");
        }

        public RoomNew RightRoom(RoomNew room)
        {
            int x = room.roomX;
            int y = room.roomY;
            int z = room.roomZ;

            if (x != levels[z].rows[y].columns.Count() - 1)
                x = ++x;
            else
                return blockRoom;
                //return new RoomNew(this, PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_ROOMS + "MAP_blockroom.xml");

            foreach (RoomNew r in rooms)
            {
                if (r.roomX == x & r.roomY == y & r.roomZ == z)
                    return r;
            }
            return blockRoom;
            //return new RoomNew(this, PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_ROOMS + "MAP_blockroom.xml");
        }

        public RoomNew UpRoom(RoomNew room)
        {
            int x = room.roomX;
            int y = room.roomY;
            int z = room.roomZ;

            if (y != levels[z].rows.Count() - 1)
                y = --y;
            else
                return blockRoom;
                //return new RoomNew(this, PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_ROOMS + "MAP_blockroom.xml");

            foreach (RoomNew r in rooms)
            {
                if (r.roomX == x & r.roomY == y & r.roomZ == z)
                    return r;
            }
            return blockRoom;
            //return new RoomNew(this, PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_ROOMS + "MAP_blockroom.xml");
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
            return blockRoom;
            //return new RoomNew(this, PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_ROOMS + "MAP_blockroom.xml");
        }


        public RoomNew FindRoom(string roomName)
        {
            return null;
        }

    }
}
