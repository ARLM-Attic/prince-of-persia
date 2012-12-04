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
    /// <summary>
    /// A uniform grid of tiles with collections of gems and enemies.
    /// The level owns the player and controls the game's win and lose
    /// conditions as well as scoring.
    /// </summary>
    class Room : IDisposable
    {

        IServiceProvider iServiceProvider;

        // Physical structure of the level.
        private Tile[,] tiles;

        private const int pWidth = 10;
        private const int pHeight = 3;
        private const int pSize = 30;
        public const int BOTTOM_BORDER = 16; //Bottom border for live energy and message space
        public const int TOP_BORDER = 6; // = (400 pixel - BOTTOM_BORDER % 3 ROWS)

        public int widthInLevel = 0;
        public int heightInLevel = 0;

        public Level level;

        // Entities in the level.
        public Player Player
        {
            get { return player; }
        }
        Player player;

        // Key locations in the level.        
        private Vector2 start;
        private Point exit = InvalidPosition;
        private static readonly Point InvalidPosition = new Point(-1, -1);

        // Level game state.
        private Random random = new Random(354668); // Arbitrary, but constant seed

        public int Score
        {
            get { return score; }
        }
        int score;

        public TimeSpan TimeRemaining
        {
            get { return timeRemaining; }
        }
        TimeSpan timeRemaining;

        private const int PointsPerSecond = 5;

        // Level content.        
        public ContentManager Content
        {
            get { return content; }
        }
        ContentManager content;

        #region Loading

        /// <summary>
        /// Constructs a new level.
        /// </summary>
        /// <param name="serviceProvider">
        /// The service provider that will be used to construct a ContentManager.
        /// </param>
        /// <param name="fileStream">
        /// A stream containing the Tile data.
        /// </param>
        public Room(IServiceProvider serviceProvider, Stream fsRoom, Level pLevel, int HeightInLevel, int WidthInLevel)
        {
            iServiceProvider = serviceProvider;
            level = pLevel;
            heightInLevel = HeightInLevel;
            widthInLevel = WidthInLevel;
            // Create a new content manager to load content used just by this level.
            content = new ContentManager(serviceProvider, "Content");

            timeRemaining = TimeSpan.FromMinutes(10.0);

            LoadTiles(content, fsRoom);

        }

        /// <summary>
        /// Iterates over every Tile in the structure file and loads its
        /// appearance and behavior. This method also validates that the
        /// file is well-formed with a player start point, exit, etc.
        /// </summary>
        /// <param name="fileStream">
        /// A stream containing the Tile data.
        /// </param>
        private void LoadTiles(ContentManager content, Stream fileStream)
        {
            // Load the level and ensure all of the lines are the same length.
            int width;
            List<string> lines = new List<string>();

            if (fileStream.CanRead == false)
            { return; }

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

            // Allocate the Tile grid.
            tiles = new Tile[width, lines.Count];

            // Loop over every Tile position,
            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    // to load each Tile.
                    char tileType = lines[y][x];
                    tiles[x, y] = LoadTile(content, tileType, x, y);
                }
            }
        }

        /// <summary>
        /// Loads an individual Tile's appearance and behavior.
        /// </summary>
        /// <param name="tileType">
        /// The character loaded from the structure file which
        /// indicates what should be loaded.
        /// </param>
        /// <param name="x">
        /// The X location of this Tile in Tile space.
        /// </param>
        /// <param name="y">
        /// The Y location of this Tile in Tile space.
        /// </param>
        /// <returns>The loaded Tile.</returns>
        private Tile LoadTile(ContentManager content, char tileType, int x, int y)
        {
            switch (tileType)
            {
                // Blank space
                case '.':
                    return new Tile(content, TileType.space);

                // Player 1 start point
                case '1':
                    return LoadStartTile(content, x, y);

                case '#':
                    return new Tile(content, TileType.floor);

                case 'T':
                    return new Tile(content, TileType.torch);

                case 'G':
                    return new Tile(content, TileType.space);

                case 'W':
                    return new Tile(content, TileType.block);

                // Unknown Tile type character
                
                default:
                    throw new NotSupportedException(String.Format("Unsupported Tile type character '{0}' at position {1}, {2}.", tileType, x, y));
            }
        }

        /// <summary>
        /// Creates a new Tile. The other Tile loading methods typically chain to this
        /// method after performing their special logic.
        /// </summary>
        /// <param name="name">
        /// Path to a Tile texture relative to the Content/Tiles directory.
        /// </param>
        /// <param name="collision">
        /// The Tile collision type for the new Tile.
        /// </param>
        /// <returns>The new Tile.</returns>
        private Tile LoadTile(string name, TileType tiletype)
        {
                return new Tile(content, tiletype);
        }


        /// <summary>
        /// Loads a Tile with a random appearance.
        /// </summary>
        /// <param name="baseName">
        /// The content name prefix for this group of Tile variations. Tile groups are
        /// name LikeThis0.png and LikeThis1.png and LikeThis2.png.
        /// </param>
        /// <param name="variationCount">
        /// The number of variations in this group.
        /// </param>
        //private Tile LoadVarietyTile(string baseName, int variationCount, TileCollision collision, TileType tiletype)
        //{
        //    return LoadTile(baseName, collision, tiletype);
        //}


        /// <summary>
        /// Instantiates a player, puts him in the level, and remembers where to put him when he is resurrected.
        /// </summary>
        private Tile LoadStartTile(ContentManager content, int x, int y)
        {
            if (Player != null)
                throw new NotSupportedException("A level may only have one starting point.");

            //start = RectangleExtensions.(GetBounds(x, y));
            start = new Vector2(GetBounds(x, y).X,GetBounds(x, y).Y) ;

            return new Tile(content, TileType.space);
        }

        /// <summary>
        /// Remembers the location of the level's exit.
        /// </summary>
        //private Tile LoadExitTile(int x, int y)
        //{
        //    if (exit != InvalidPosition)
        //        throw new NotSupportedException("A level may only have one exit.");

        //    exit = GetBounds(x, y).Center;

        //    return LoadTile("ExitA", TileCollision.Passable, TileType.space);
        //}



        /// <summary>
        /// Unloads the level content.
        /// </summary>
        public void Dispose()
        {
            Content.Unload();
        }

        #endregion

        #region Bounds and collision
        /// <summary>
        /// Gets the collision mode of the Tile at a particular location.
        /// This method handles tiles outside of the levels boundries by making it
        /// impossible to escape past the left or right edges, but allowing things
        /// to jump beyond the top of the level and fall off the bottom.
        /// </summary>
        public TileCollision GetCollision(int x, int y)
        {
            // Prevent escaping past the level ends.
            if (x < 0 || x >= Width)
                return TileCollision.Impassable;
            // Allow jumping past the level top and falling through the bottom.
            if (y < 0 || y >= Height)
                return TileCollision.Passable;

            return tiles[x, y].Collision;
        }

        /// <summary>
        /// The grid star of X = 0 and Y = 2
        /// Remnber: for example the bottom gate is x=5 y=2
        /// .1.#####WW
        /// ###G.WWWWW
        /// WWWW#G###W
        /// 
        /// 
        /// </summary>
        public TileType GetType(int x, int y)
        {
            if (x < 0 || x >= Width)
                return TileType.block;
            // Allow jumping past the level top and falling through the bottom.
            if (y < 0 || y >= Height)
                return TileType.block;

            // Prevent escaping past the level ends.
            return tiles[x, y].Type;
        }

        public Tile GetTile(int x, int y)
        {
            return tiles[x, y];
        }


        /// <summary>
        /// Gets the bounding rectangle of a Tile in world space.
        /// </summary>        
        public Rectangle GetBounds(int x, int y)
        {
            int by = 0;
            if (y == 0)
                by = TOP_BORDER;
            else
                by = (y * Tile.HEIGHT) + TOP_BORDER;

            return new Rectangle(x * Tile.WIDTH, by, Tile.WIDTH, Tile.HEIGHT );
        }

    

        /// <summary>
        /// Width of level measured in tiles.
        /// </summary>
        public int Width
        {
            get { return  tiles.GetLength(0); }
        }

        /// <summary>
        /// Height of the level measured in tiles.
        /// </summary>
        public int Height
        {
            get { return tiles.GetLength(1); }
        }

        #endregion

        #region Update

        /// <summary>
        /// Updates all objects in the world, performs collision between them,
        /// and handles the time limit with scoring.
        /// </summary>
        public void Update(
            GameTime gameTime, 
            KeyboardState keyboardState, 
            GamePadState gamePadState, 
            TouchCollection touchState, 
            AccelerometerState accelState,
            DisplayOrientation orientation)
        {
            // Pause while the player is dead or time is expired.
            if (!Player.IsAlive || TimeRemaining == TimeSpan.Zero)
            {
                // Still want to perform physics on the player.
                Player.HandleCollisionsNew();
                //Player.ApplyPhysicsNew(gameTime);
            }
            else
            {
                timeRemaining -= gameTime.ElapsedGameTime;
                Player.Update(gameTime, keyboardState, gamePadState, touchState, accelState, orientation);
            }

            // Clamp the time remaining at zero.
            if (timeRemaining < TimeSpan.Zero)
                timeRemaining = TimeSpan.Zero;
        }


        /// <summary>
        /// Restores the player to the starting point to try the level again.
        /// </summary>
        public void StartNewLife(GraphicsDevice graphicsDevice)
        {
            if (player == null)
                player = new Player(this, start, graphicsDevice);
            else
                Player.Reset(start);
        }

        #endregion

        #region Draw

        /// <summary>
        /// Draw everything in the level from background to foreground.
        /// </summary>
        public void Draw(Game g, GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawTilesInverse(spriteBatch);
            Player.Draw(gameTime, spriteBatch);
        }


        private void DrawTilesLeft(SpriteBatch spriteBatch)
        {
            Vector2 v = new Vector2(0, 0);
            // For each Tile position
            for (int y = Height - 1; y >= 0; --y)
            {
                for (int x = Width -1; x < Width; ++x)
                {
                    // If there is a visible Tile in that position
                    Texture2D texture = null;
                    texture = tiles[x, y].Texture;
                    if (texture != null)
                    {
                        // Draw it in screen space.
                        //Vector2 position = new Vector2(x, y) * Tile.Size;
                        Rectangle rect = new Rectangle(-1 * (int)Tile.Size.X, y * (int)Tile.Size.Y - BOTTOM_BORDER, (int)texture.Width, (int)texture.Height);
                        if (tiles[x, y].Type == TileType.gate)
                        {
                            spriteBatch.Draw(texture, rect, null, Color.White, 0.0f, v, SpriteEffects.None, 0.1f);
                        }
                        else
                        {
                            spriteBatch.Draw(texture, rect, null, Color.White, 0.0f, v, SpriteEffects.None, 0.1f);
                        }
                    }
                }
            }

        }

        private void DrawTilesUp(SpriteBatch spriteBatch)
        {
            Vector2 v = new Vector2(0, 0);
            // For each Tile position
            int y = 0;
            for (int x = 0; x < Width; ++x)
            {
                // If there is a visible Tile in that position
                Texture2D texture = null;
                texture = tiles[x, y].Texture;
                if (texture != null)
                {
                    // Draw it in screen space.
                    //Vector2 position = new Vector2(x, y) * Tile.Size;
                    Rectangle rect = new Rectangle(x * (int)Tile.Size.X, -1 * (int)Tile.Size.Y - BOTTOM_BORDER, (int)texture.Width, (int)texture.Height);
                    if (tiles[x, y].Type == TileType.gate)
                    {
                        spriteBatch.Draw(texture, rect, null, Color.White, 0.0f, v, SpriteEffects.None, 0.1f);
                    }
                    else
                    {
                        spriteBatch.Draw(texture, rect, null, Color.White, 0.0f, v, SpriteEffects.None, 0.1f);
                    }
                }
            }

        }


        /// <summary>
        /// Draws each Tile in the level inverse order
        /// </summary>
        private void DrawTilesInverse(SpriteBatch spriteBatch)
        {
            //search if there are a left valid room
            RoomLeft().DrawTilesLeft(spriteBatch);
            

            Vector2 v = new Vector2(0, 0);
            // For each Tile position
            for (int y = Height-1; y >= 0; --y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    // If there is a visible Tile in that position
                    Texture2D texture = null;
                    texture = tiles[x, y].Texture;

                    if (texture == null)
                    {
                        continue;
                    }

                    // Draw it in screen space 
                    Rectangle rect = new Rectangle(x * (int)Tile.Size.X, y * (int)Tile.Size.Y - BOTTOM_BORDER, (int)texture.Width, (int)texture.Height);
                    if (tiles[x, y].Type == TileType.gate)
                    {
                        spriteBatch.Draw(texture, rect, null, Color.White, 0.0f, v, SpriteEffects.None, 0.1f);
                    }
                    else
                    {
                        spriteBatch.Draw(texture, rect, null, Color.White, 0.0f, v, SpriteEffects.None, 0.1f);
                    }
                }
            }
            RoomUp().DrawTilesUp(spriteBatch);
        }

        public Room RoomLeft()
        {
            try
            {
                return level.rooms[heightInLevel, widthInLevel - 1]; ;
            }
            catch (Exception ex)
            {
                //limit of the level
                return MakeblockRoom();
            }
        }

        public Room RoomUp()
        {
            try
            {
                return level.rooms[heightInLevel - 1, widthInLevel];
            }
            catch(Exception ex)
            {
                //limit of the level
                return MakeblockRoom();
            }
        }

        public Room MakeblockRoom()
        {
            string RoomPath;
            string[] par = new string[3];
            par[0] = "-1";
            par[1] = "-1";
            par[2] = "-1";
            RoomPath = string.Format("Content/Rooms/blockRoom.txt", par);

            Stream fileStream = TitleContainer.OpenStream(RoomPath);

            // Load the level and ensure all of the lines are the same length.
            List<string> lines = new List<string>();
            using (StreamReader reader = new StreamReader(fileStream))


            fileStream = TitleContainer.OpenStream(RoomPath);
            return new Room(iServiceProvider, fileStream, level, -1, -1);
        }

        public Vector4 getBoundTiles(Rectangle playerBounds)
        {
            int leftTile = (int)Math.Floor((float)playerBounds.Left / Tile.WIDTH);
            int rightTile = (int)Math.Ceiling(((float)playerBounds.Right / Tile.WIDTH)) - 1; //tile dal bordo sx dello schermo al bordo dx del rettangolo sprite
            int topTile = (int)Math.Floor((float)(playerBounds.Top - Room.BOTTOM_BORDER) / Tile.HEIGHT); //tiles from the top screen border
            int bottomTile = (int)Math.Ceiling(((float)(playerBounds.Bottom - Room.BOTTOM_BORDER) / Tile.HEIGHT)) - 1;
            return new Vector4(leftTile,rightTile,topTile,bottomTile);
        }

        public Vector2 getCenterTile(Rectangle playerBounds)
        {
            int leftTile = (int)Math.Floor((float)playerBounds.Center.X / Tile.WIDTH);
            int topTile = (int)Math.Floor((float)(playerBounds.Center.Y - Room.BOTTOM_BORDER) / Tile.HEIGHT); //tiles from the top screen border
            return new Vector2(leftTile, topTile);
        }

        #endregion
    }
}
