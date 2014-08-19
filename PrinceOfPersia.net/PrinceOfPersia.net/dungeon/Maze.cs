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
        public int levelIndex = 1;
        public Player player;

        
        //List for retain and load maze tiles textures
        public static Dictionary<string, object> dContentRes = null;
        public Vector2 positionArrive;

        //test
        public static Effect dEffect = null;


        public Level CurrentLevel
        {
            get 
            {
                Enumeration.LevelName levelName = (Enumeration.LevelName)Enum.Parse(typeof(Enumeration.LevelName), levelIndex.ToString());
                foreach (Level l in levels)
                { 
                    if (l.levelName == levelName)
                        return l; 
                }
                return null; 
                
            }
        }

        public void NextLevel()
        {
            levelIndex++;
            //next level
            player.StartLevel(CurrentLevel.StartRoom());
            player.MyRoom.StartNewLife();
        }

        private void Apoplexy()
        {
            List<Apoplexy.level> levelsApoplexy = new List<Apoplexy.level>();
            levels = new List<Level>();

            var files = Directory
            .GetFiles(content.RootDirectory + "/" + PrinceOfPersiaGame.CONFIG_PATH_APOPLEXY+"/", "*.xml")
                //.Where(file => file.ToLower().EndsWith("xnb") || file.ToLower().EndsWith("xml"))
            .ToList();

            foreach (object f in files)
            {
                Stream txtReader = Microsoft.Xna.Framework.TitleContainer.OpenStream(f.ToString());
                
                System.Xml.Serialization.XmlSerializer ax = null;
                ax = new System.Xml.Serialization.XmlSerializer(typeof(Apoplexy.level));
                levelsApoplexy.Add((Apoplexy.level)ax.Deserialize(txtReader));
            }

            //load all room
            for (int z = 0; z < levelsApoplexy.Count(); z++)
            {
                Level level = new Level(this, levelsApoplexy[z].number);
                
                //int newX = 1;
                for (int y = 0; y < levelsApoplexy[z].rooms.Count(); y++)
                {
                    Room room = new Room(this, level, levelsApoplexy[z], levelsApoplexy[z].rooms[y].tile, levelsApoplexy[z].rooms[y].number, levelsApoplexy[z].rooms[y].links, levelsApoplexy[z].rooms[y].guard, levelsApoplexy[z].events, levelsApoplexy[z].prince, levelsApoplexy[z].userdata.field[5].value);
                    level.rooms.Add(room);
                }

                //i will associate events
                //ASSOCIATE EVENTS
                Apoplexy.@event eventPrec = null;
                foreach (Apoplexy.@event e in levelsApoplexy[z].events)
                {
                    foreach (Room r in level.rooms)
                    {
                        if (r.roomNumber.ToString() == e.room)
                        {
                            foreach (Tile t in r.tiles)
                            {
                                float tileLocation = 10 * t.Coordinates.Y + t.Coordinates.X + 1;
                                if (tileLocation == float.Parse(e.location))
                                {
                                    if (t.Type == Enumeration.TileType.gate)
                                    {
                                        if (eventPrec == null)
                                        {
                                            ((Gate)t).switchButtons.Add(int.Parse(e.number));
                                            if (e.next == "1")
                                            {
                                                eventPrec = e;
                                            }
                                        }
                                        else
                                        {
                                            ((Gate)t).switchButtons.Add(int.Parse(eventPrec.number));
                                            if (e.next != "1")
                                            {
                                                eventPrec = null;
                                            }
                                        }
                                    }
                                    else if (t.Type == Enumeration.TileType.exit)
                                    {
                                        if (eventPrec == null)
                                        {
                                            ((Exit)t).switchButtons.Add(int.Parse(e.number));
                                            if (e.next == "1")
                                            {
                                                eventPrec = e;
                                            }
                                        }
                                        else
                                        {
                                            ((Exit)t).switchButtons.Add(int.Parse(eventPrec.number));
                                            if (e.next != "1")
                                            {
                                                eventPrec = null;
                                            }
                                        }
                                    }


                                }
                            }
                        }
                    }
                }

                levels.Add(level);
            }

          


        }
        

        private void PopNet()
        {
            List<PopNet.Level> levelsPopNet = new List<PopNet.Level>();
            levels = new List<Level>();

            var files = Directory
            .GetFiles(content.RootDirectory + "/" + PrinceOfPersiaGame.CONFIG_PATH_LEVELS+ "/", "*.xml")
            .ToList();

            foreach (object f in files)
            {

                Stream txtReader = Microsoft.Xna.Framework.TitleContainer.OpenStream(f.ToString());

                System.Xml.Serialization.XmlSerializer ax = null;
                ax = new System.Xml.Serialization.XmlSerializer(typeof(Apoplexy.level));
                levelsPopNet.Add((PopNet.Level)ax.Deserialize(txtReader));


            }


            //load all room
            for (int z = 0; z < levelsPopNet.Count(); z++)
            {
                Level level = new Level(this, levelsPopNet[z].levelName.ToString());
                for (int y = 0; y < levelsPopNet[z].rows.Count(); y++)
                {
                    for (int x = 0; x < levelsPopNet[z].rows[y].columns.Count(); x++)
                    {
                        if (levelsPopNet[z].rows[y].columns[x].FilePath == string.Empty)
                            levelsPopNet[z].rows[y].columns[x].FilePath = "MAP_blockroom.xml";

                        Room room = new Room(this, level, PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_ROOMS + levelsPopNet[z].rows[y].columns[x].FilePath, levelsPopNet[z].rows[y].columns[x].RoomIndex);
                        room.roomStart = levelsPopNet[z].rows[y].columns[x].RoomStart;
                        room.roomNumber = y;
                        room.roomName = levelsPopNet[z].rows[y].columns[x].FilePath;
                        room.roomZ = z;
                        room.roomX = x;
                        room.roomY = y;
                        level.rooms.Add(room);

                    }
                }
            }


            //

        }



        public Maze(GraphicsDevice GraphicsDevice, ContentManager contentmanager)
        {
            content = contentmanager;
            graphicsDevice = GraphicsDevice;

            LoadContent();

            //dEffect = content.Load<Effect>(@"Effects\SwapColor");

            if (PrinceOfPersiaGame.LEVEL_APOPLEXY == true)
                Apoplexy();
            else
                PopNet();


      
        }

        //Load all texture in a dictiornary
        private void LoadContent()
        {
            //dSoundEffect = Program.LoadContent<SoundEffect>(content, string.Empty);
            dContentRes= Program.LoadContent<object>(content, string.Empty);


        }



      

        //public Room LeftRoom(Room room)
        //{
        //    int x = room.roomX;
        //    int y = room.roomY;
        //    int z = room.roomZ;

        //    string roomNext = string.Empty;

        //    if (PrinceOfPersiaGame.LEVEL_APOPLEXY == true)
        //    {
        //        if (room.link == null)
        //            return blockRoom;

        //        roomNext = room.link.left ;
        //    }
        //    else
        //    {
        //        if (x != 0)
        //            x = --x;
        //        else
        //            return blockRoom;
        //        //return new RoomNew(this, PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_ROOMS + "MAP_blockroom.xml");
        //    }
        //    foreach (Room r in player.SpriteLevel.rooms)
        //    {
        //        if (PrinceOfPersiaGame.LEVEL_APOPLEXY == true)
        //        {
        //            if (r.roomName == roomNext)
        //                return r;
        //        }
        //        else
        //        {
        //            if (r.roomX == x & r.roomY == y & r.roomZ == z)
        //                return r;
        //        }
        //    }
        //    return blockRoom;
        //    //return new RoomNew(this, PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_ROOMS + "MAP_blockroom.xml");
        //}

        //public Room DownRoom(Room room)
        //{
        //    int x = room.roomX;
        //    int y = room.roomY;
        //    int z = room.roomZ;

        //    string roomNext = string.Empty;

        //    if (PrinceOfPersiaGame.LEVEL_APOPLEXY == true)
        //    {
        //        if (room.link == null)
        //            return blockRoom;

        //        roomNext = room.link.down;
        //    }
        //    else
        //    {
        //        if (y != levels[z].rooms.Count() - 1)
        //            y = ++y;
        //        else
        //            return blockRoom;
        //        //return new RoomNew(this, PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_ROOMS + "MAP_blockroom.xml");
        //    }

        //    foreach (Room r in player.SpriteLevel.rooms)
        //    {
        //        if (PrinceOfPersiaGame.LEVEL_APOPLEXY == true)
        //        {
        //            if (room.link == null)
        //                return blockRoom;

        //            if (r.roomName == roomNext)
        //                return r;
        //        }
        //        else
        //        {
        //            if (r.roomX == x & r.roomY == y & r.roomZ == z)
        //                return r;
        //        }
        //    }
        //    return blockRoom;
        //    //return new RoomNew(this, PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_ROOMS + "MAP_blockroom.xml");
        //}

        //public Room RightRoom(Room room)
        //{
        //    int x = room.roomX;
        //    int y = room.roomY;
        //    int z = room.roomZ;

        //    string roomNext = string.Empty;

        //    if (PrinceOfPersiaGame.LEVEL_APOPLEXY == true)
        //    {
        //        if (room.link == null)
        //            return blockRoom;

        //        roomNext = room.link.right;
        //    }
        //    else
        //    {
        //        //AAAHAHAHA TO BE CHANGERSSSS
        //        //if (x != levels[z].rows[y].columns.Count() - 1)
        //        //    x = ++x;
        //        //else
        //        //    return blockRoom;
        //    }
        //    foreach (Room r in player.SpriteLevel.rooms)
        //    {
        //        if (PrinceOfPersiaGame.LEVEL_APOPLEXY == true)
        //        {
        //            if (r.roomName == roomNext)
        //                return r;
        //        }
        //        else
        //        {
        //            if (r.roomX == x & r.roomY == y & r.roomZ == z)
        //                return r;
        //        }
        //    }
        //    return blockRoom;
        //    //return new RoomNew(this, PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_ROOMS + "MAP_blockroom.xml");
        //}

        //public Room UpRoom(Room room)
        //{
        //    int x = room.roomX;
        //    int y = room.roomY;
        //    int z = room.roomZ;

        //    string roomNext = string.Empty;

        //    if (PrinceOfPersiaGame.LEVEL_APOPLEXY == true)
        //    {
        //        if (room.link == null)
        //            return blockRoom;

        //        roomNext = room.link.up;
        //    }
        //    else
        //    {
        //        if (y != levels[z].rooms.Count() - 1)
        //            y = --y;
        //        else
        //            return blockRoom;
        //        //return new RoomNew(this, PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_ROOMS + "MAP_blockroom.xml");
        //    }
        //    foreach (Room r in player.SpriteLevel.rooms)
        //    {
        //        if (PrinceOfPersiaGame.LEVEL_APOPLEXY == true)
        //        {
        //            if (r.roomName == roomNext)
        //                return r;
        //        }
        //        else
        //        {

        //            if (r.roomX == x & r.roomY == y & r.roomZ == z)
        //                return r;
        //        }
        //    }
        //    return blockRoom;
        //    //return new RoomNew(this, PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_ROOMS + "MAP_blockroom.xml");
        //}

        public Room StartRoom()
        {
            return player.MyLevel.StartRoom();
        }




    }
}
