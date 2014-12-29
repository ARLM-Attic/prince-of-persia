	//-----------------------------------------------------------------------//
	// <copyright file="Maze.cs" company="A.D.F.Software">
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
    public class Maze
    {
        public static Dictionary<string, object> Content = null;

        public GraphicsDevice graphicsDevice;
        public List<Level> levels = new List<Level>();
        public int levelIndex = 1;
        public Player player;


        private List<Apoplexy.level> levelsApoplexy = new List<Apoplexy.level>();
        private List<PoN.level> levelsPoN = new List<PoN.level>();

        //List for retain and load maze tiles textures
        
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
            StartLevel((Enumeration.LevelName)Enum.Parse(typeof(Enumeration.LevelName), levelIndex.ToString()));
            player.StartLevel(CurrentLevel.StartRoom());
            player.MyRoom.StartNewLife();
        }

        public void PreviousLevel()
        {
            levelIndex--;
            //next level
            StartLevel((Enumeration.LevelName)Enum.Parse(typeof(Enumeration.LevelName), levelIndex.ToString()));
            player.StartLevel(CurrentLevel.StartRoom());
            player.MyRoom.StartNewLife();
        }

        private void LoadLevels()
        {
#if ANDROID
            List<string> files = new List<string>();
            string sResults = string.Empty;
            var filePath = Path.Combine("", "results.txt");
            using (var stream = TitleContainer.OpenStream(filePath))
            {
                sResults =  Utils.StreamToString(stream);
                stream.Close();
            }
            
            string[] sArray =  sResults.Split('\r');

            for (int x = 0; x < sArray.Count(); x++ )
            {
                int a = sArray[x].IndexOf("Content\\");
                if (a == -1)
                    continue;
                sArray[x] = sArray[x].Remove(0, a);

                a = sArray[x].IndexOf("apoplexy\\");
                if (a == -1)
                    continue;

                //string sLevelNumber = ((int)levelName).ToString();

                if (sArray[x].Substring(sArray[x].Length - 4) != ".xml")
                    continue;
                files.Add(sArray[x]);
            }
#else
            var files = Directory
                .GetFiles(PoP.CONFIG_PATH_CONTENT + "/" + PoP.CONFIG_PATH_APOPLEXY + "/", "*.xml")
            //.Where(file => file.ToLower().Contains("level" + ((int)levelName).ToString()))
            .ToList();
#endif

            foreach (object f in files)
            {
                Stream txtReader = Microsoft.Xna.Framework.TitleContainer.OpenStream(f.ToString());

                System.Xml.Serialization.XmlSerializer ax = null;
                ax = new System.Xml.Serialization.XmlSerializer(typeof(Apoplexy.level));
                levelsApoplexy.Add((Apoplexy.level)ax.Deserialize(txtReader));
            }

        }


        private void LoadLevelsPoN()
        {
#if ANDROID
            List<string> files = new List<string>();
            string sResults = string.Empty;
            var filePath = Path.Combine("", "results.txt");
            using (var stream = TitleContainer.OpenStream(filePath))
            {
                sResults =  Utils.StreamToString(stream);
                stream.Close();
            }
            
            string[] sArray =  sResults.Split('\r');

            for (int x = 0; x < sArray.Count(); x++ )
            {
                int a = sArray[x].IndexOf("Content\\");
                if (a == -1)
                    continue;
                sArray[x] = sArray[x].Remove(0, a);

                a = sArray[x].IndexOf("apoplexy\\");
                if (a == -1)
                    continue;

                //string sLevelNumber = ((int)levelName).ToString();

                if (sArray[x].Substring(sArray[x].Length - 4) != ".xml")
                    continue;
                files.Add(sArray[x]);
            }
#else
            var files = Directory
                .GetFiles(PoP.CONFIG_PATH_CONTENT + "/" + PoP.CONFIG_PATH_PON + "/", "*.xml")
                //.Where(file => file.ToLower().Contains("level" + ((int)levelName).ToString()))
            .ToList();
#endif
            //PoN.room myRoom = new PoN.room();
            //myRoom.index = 1;
            //PoN.block[] bs = new PoN.block[10];
            //PoN.block b = new PoN.block();
            //PoN.item[] itms = new PoN.item[1];
            //PoN.item i = new PoN.item();

            //i.state = Enumeration.StateTile.normal;
            //i.type = Enumeration.TileType.sword;
            //itms[0] = i;
            //b.item = itms;
            //bs[0] = b;

            //myRoom.block = bs;
            //b.tile = new PoN.tile();
            //myRoom.Serialize();


            //PoN.level l = new PoN.level();
            //l.description = "desc";
            //l.index = 1;
            //PoN.room[] r = new PoN.room[30];
            //r[0] = myRoom;
            //r[1] = new PoN.room();
            //l.room = r;

            //l.name = Enumeration.LevelName.dungeon_guards;

            ////l.rooms = myRooms;
            //l.Serialize();

            foreach (object f in files)
            {
                Stream txtReader = Microsoft.Xna.Framework.TitleContainer.OpenStream(f.ToString());

                System.Xml.Serialization.XmlSerializer ax = null;
                ax = new System.Xml.Serialization.XmlSerializer(typeof(PoN.level));
                levelsPoN.Add((PoN.level)ax.Deserialize(txtReader));
            }

        
        }



        private void StartLevel(Enumeration.LevelName levelName)
        {
            levels = new List<Level>();

            if ((int)levelName >= levelsApoplexy.Count)
            { 
                levelName = Enumeration.LevelName.dungeon_demo;
                levelIndex = (int)levelName;
            }

            //load all room
            for (int z = 0; z < levelsApoplexy.Count(); z++)
            {
                if ((int)levelName != int.Parse( levelsApoplexy[z].number))
                    continue;

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
        

        private void StartLevelPoP(Enumeration.LevelName levelName)
        {

            levels = new List<Level>();

            //DISABLED POP HAVE ONLY 1 LEVEL.... someone can be draw all levels?!?
            //if ((int)levelName >= levelsPoN.Count)
            //{
            //    levelName = Enumeration.LevelName.dungeon_demo;
            //    levelIndex = (int)levelName;
            //}

            List<PopNet.Level> levelsPopNet = new List<PopNet.Level>();

            var files = Directory
                .GetFiles(PoP.CONFIG_PATH_CONTENT + "/" + PoP.CONFIG_PATH_LEVELS + "/", "*.xml")
            .ToList();

            foreach (object f in files)
            {

                Stream txtReader = Microsoft.Xna.Framework.TitleContainer.OpenStream(f.ToString());

                System.Xml.Serialization.XmlSerializer ax = null;
                ax = new System.Xml.Serialization.XmlSerializer(typeof(PopNet.Level));
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

                        Room room = new Room(this, level, PoP.CONFIG_PATH_CONTENT + PoP.CONFIG_PATH_ROOMS + levelsPopNet[z].rows[y].columns[x].FilePath, levelsPopNet[z].rows[y].columns[x].RoomIndex);
                        room.roomStart = levelsPopNet[z].rows[y].columns[x].RoomStart;
                        room.roomNumber = levelsPopNet[z].rows[y].columns[x].RoomIndex;
                        room.roomName = levelsPopNet[z].rows[y].columns[x].FilePath;
                        room.roomZ = z;
                        room.roomX = x;
                        room.roomY = y;
                        level.rooms.Add(room);


                        //Allocate room shortcut
                        if (x - 1 >= 0)
                            room._RoomLeft = levelsPopNet[z].rows[y].columns[x - 1].RoomIndex;
                        if (x + 1 < levelsPopNet[z].rows[y].columns.Count())
                            room._RoomRight = levelsPopNet[z].rows[y].columns[x + 1].RoomIndex;

                        if(y + 1 < levelsPopNet[z].rows.Count())
                            room._RoomDown = levelsPopNet[z].rows[y+1].columns[x].RoomIndex;

                        if (y - 1 >= 0)
                            room._RoomUp = levelsPopNet[z].rows[y-1].columns[x].RoomIndex;
                        


                    }
                }
                levels.Add(level);
            }


            //
           
            }



        public Maze(GraphicsDevice GraphicsDevice)
        {
            graphicsDevice = GraphicsDevice;

            if (PoP.LEVEL_APOPLEXY == true)
            {
                LoadLevels();
                StartLevel(Enumeration.LevelName.dungeon_prison);
            }
            else
            {
                LoadLevelsPoN();
                StartLevelPoP(Enumeration.LevelName.dungeon_prison);
            }

            
      
        }

        public Room StartRoom()
        {
            return player.MyLevel.StartRoom();
        }




    }
}
