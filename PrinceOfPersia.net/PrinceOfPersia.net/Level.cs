using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.IO;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Input;

namespace PrinceOfPersia
{
    class Level
    {
        //Level dimensions x,y
        public int levelWidth = 0;
        public int levelHeight = 0;
        
        //array of all rooms for level
        public Room[,] rooms;
        
        //actual players room
        private Room actualRoom;

        public int numberOfLevels
        {
            get { return levelHeight * levelHeight; }
        }


        public Level(IServiceProvider serviceProvider, int levelNumber)
        {
            LoadLevel(serviceProvider, levelNumber);
        }

        public Room StartRoom()
        {
//            foreach (Room r in rooms)
//            {
                return rooms[0, 1];
//            }
        }


        private void LoadLevel(IServiceProvider serviceProvider, int levelNumber)
        {
            //int levelIndex= 0;
            int width = 0;
            
//            levelIndex = (++levelIndex) % numberOfLevels;

            string levelPath = string.Format("Content/Levels/{0}.txt", levelNumber);
                
            Stream fileStream = TitleContainer.OpenStream(levelPath);
            
            // Load the level and ensure all of the lines are the same length.
            List<string> lines = new List<string>();
            using (StreamReader reader = new StreamReader(fileStream))
            {
                string line = reader.ReadLine();
                width = line.Length;
                while (line != null)
                {
                    lines.Add(line);
                    if (line.Length != width)
                        throw new Exception(String.Format("The length of line {0} is different from all preceeding lines.", lines.Count));
                    line = reader.ReadLine();
                }
            }

            //associate level dimensions
            levelWidth = width;
            levelHeight = lines.Count;

            // Allocate the Room grid.
            rooms = new Room[levelWidth, levelHeight];

            // Loop over every Tile position,
            for (int x = 0; x < levelHeight; ++x)
            {
                for (int y = 0; y < levelWidth; ++y)
                {
                // to load each Tile.
                    string RoomPath;
                    char roomType = lines[x][y];
                    string[] par = new string[3];
                    par[0] = levelNumber.ToString();
                    par[1] = x.ToString();
                    par[2] = y.ToString();
                    if (roomType == '#')
                    { RoomPath = string.Format("Content/Rooms/{0}_{1}_{2}.txt", par); }
                    else
                    { RoomPath = string.Format("Content/Rooms/blockRoom.txt", par); }
                    fileStream = TitleContainer.OpenStream(RoomPath);
                    rooms[x, y] = new Room(serviceProvider, fileStream, this, x, y);
                }
            }

        }


    }
}
