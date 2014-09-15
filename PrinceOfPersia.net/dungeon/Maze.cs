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

            var files = Directory
                .GetFiles(PrinceOfPersiaGame.CONFIG_PATH_CONTENT + "/" + PrinceOfPersiaGame.CONFIG_PATH_APOPLEXY + "/", "*.xml")
            //.Where(file => file.ToLower().Contains("level" + ((int)levelName).ToString()))
            .ToList();


            foreach (object f in files)
            {
                Stream txtReader = Microsoft.Xna.Framework.TitleContainer.OpenStream(f.ToString());

                System.Xml.Serialization.XmlSerializer ax = null;
                ax = new System.Xml.Serialization.XmlSerializer(typeof(Apoplexy.level));
                levelsApoplexy.Add((Apoplexy.level)ax.Deserialize(txtReader));
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
        

        private void PopNet()
        {
            List<PopNet.Level> levelsPopNet = new List<PopNet.Level>();
            levels = new List<Level>();

            var files = Directory
                .GetFiles(PrinceOfPersiaGame.CONFIG_PATH_CONTENT + "/" + PrinceOfPersiaGame.CONFIG_PATH_LEVELS + "/", "*.xml")
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



        public Maze(GraphicsDevice GraphicsDevice)
        {
            graphicsDevice = GraphicsDevice;

            if (PrinceOfPersiaGame.LEVEL_APOPLEXY == true)
                LoadLevels();
            else
                PopNet();

            StartLevel(Enumeration.LevelName.dungeon_prison);
      
        }

        public Room StartRoom()
        {
            return player.MyLevel.StartRoom();
        }




    }
}
