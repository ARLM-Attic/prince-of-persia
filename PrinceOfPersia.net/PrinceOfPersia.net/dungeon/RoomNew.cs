using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Input;

namespace PrinceOfPersia
{
    /// <summary>
    /// A uniform grid of tiles with collections of gems and enemies.
    /// The level owns the player and controls the game's win and lose
    /// conditions as well as scoring.
    /// </summary>
    public class RoomNew //: IDisposable
    {

        public Maze maze;
        public Map map;
        //public Player player;
        public bool roomStart = false;

        //Coordinate system
        public string roomName;
        public int roomIndex;
        public int roomZ;
        public int roomX;
        public int roomY;

        // Physical structure of the level.
        private Tile[,] tiles;

        private const int pWidth = 10;
        private const int pHeight = 3;
        private const int pSize = 30;
        public const int BOTTOM_BORDER = 16; //Bottom border for live energy and message space
        public const int TOP_BORDER = 6; // = (400 pixel - BOTTOM_BORDER % 3 ROWS)

        public const int LEFT_LIMIT = -70;
        public const int RIGHT_LIMIT = 568;
        public const int TOP_LIMIT = -50;
        public const int BOTTOM_LIMIT = 400 - BOTTOM_BORDER;  //

        public int widthInLevel = 0;
        public int heightInLevel = 0;

        //private List<Tile> tilesTemporaney = new List<Tile>();
        //public ArrayList tilesTemporaney = ArrayList.Synchronized(_tilesTemporaney);

        //System.Collections.Concurrent. a = new System.Collections.Concurrent();

        public ArrayList tilesTemporaney = new ArrayList();
        //public ArrayList tilesTemporaney;

        // Key locations in the level.        
        private Vector2 startPosition = Vector2.Zero;
        private SpriteEffects spriteEffect = SpriteEffects.None;


        public ContentManager content
        {
            get { return maze.content; }
        }
        #region Loading

        public RoomNew(Maze maze, string filePath)
        {
            this.maze = maze;


             //tilesTemporaney = ArrayList.Synchronized(_tilesTemporaney);

            //LOAD MXL CONTENT
            //map = content.Load<Map>(filePath);

            //LOAD RES CONTENT
            System.Xml.Serialization.XmlSerializer ax;
            Stream astream = this.GetType().Assembly.GetManifestResourceStream(filePath);
            ax = new System.Xml.Serialization.XmlSerializer(typeof(Map));
            map = ((Map)ax.Deserialize(astream));

            LoadTiles();
        }

        public void LooseShake()
        {
            List<Tile> l = (List<Tile>)GetTiles(Enumeration.TileType.loose);
            foreach (Loose item in l)
            {
                item.Shake();
            }
        }


        private void LoadTiles()
        {
            // Allocate the Tile grid.
            tiles = new Tile[map.rows[0].columns.Length, map.rows.Length];
            int x=0; int y = 0;

            foreach (Row r in map.rows)
            {
                foreach (Column c in r.columns)
                {
                    tiles[x, y] = LoadTile(c.tileType, c.state.ToUpper(), c.switchButton);
                    //tiles[x, y].tileAnimation.fra = maze.player.sprite.frameRate_background;
                    Rectangle rect = new Rectangle(x * (int)Tile.Size.X, y * (int)Tile.Size.Y - BOTTOM_BORDER, (int)tiles[x, y].Texture.Width, (int)tiles[x, y].Texture.Height);
                    Vector2 v = new Vector2(rect.X, rect.Y);

                    tiles[x, y].Position = new Position(v, v);
                    tiles[x, y].Position.X = v.X;
                    tiles[x, y].Position.Y = v.Y;

                    if (c.spriteType == Enumeration.SpriteType.kid)
                    {
                        int xPlayer = (x-1) * Tile.REALWIDTH;
                        int yPlayer = y * Tile.REALHEIGHT;
                        //if (xPlayer  < 0)
                        //    xPlayer = 0;
                        //if (yPlayer  == 0)
                        //    yPlayer = Tile.REALHEIGHT;


                        startPosition = new Vector2(xPlayer, yPlayer);

                        if (c.spriteEffect == SpriteEffects.FlipHorizontally)
                        {
                            spriteEffect = SpriteEffects.FlipHorizontally;
                        }

                        
                    }

                    x++;
                }
                x = 0;
                y++;
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
        private Tile LoadTile(Enumeration.TileType tiletype)
        {
            return new Tile(this, content, tiletype, Enumeration.StateTile.normal.ToString());
        }

        private Tile LoadTile(Enumeration.TileType tiletype, string state, int switchButton)
        {
            switch (tiletype)
            {
                case Enumeration.TileType.pressplate:
                    return new PressPlate(this, content, tiletype, state, switchButton);
                    break;

                case Enumeration.TileType.door:
                    return new Door(this, content, tiletype, state, switchButton);
                    break;

                case Enumeration.TileType.loose:
                    return new Loose(this, content, tiletype, state);
                    break;

                default:
                    return new Tile(this, content, tiletype, state);
            }
            
        }

        /// <summary>
        /// Unloads the level content.
        /// </summary>
        public void Dispose()
        {
            content.Unload();
        }

        /// <summary>
        /// Restores the player to the starting point to try the level again.
        /// </summary>
        public void StartNewLife(GraphicsDevice graphicsDevice)
        {
            if (maze.player == null)
            {
                maze.player = new Player(this, startPosition, graphicsDevice, spriteEffect);
                //maze.player = new Player(this, startPosition, graphicsDevice);
            }
            else
                maze.player.Reset(startPosition);
        }

        #endregion

        #region Bounds and collision
        /// <summary>
        /// Gets the collision mode of the Tile at a particular location.
        /// This method handles tiles outside of the levels boundries by making it
        /// impossible to escape past the left or right edges, but allowing things
        /// to jump beyond the top of the level and fall off the bottom.
        /// </summary>
        public Enumeration.TileCollision GetCollision(int x, int y)
        {
            if (x < 0)
            {
                return maze.LeftRoom(this).tiles[Width - 1, y].collision;
            }

            if (x >= Width)
            {
                return maze.RightRoom(this).tiles[0, y].collision;
            }

            if (y >= Height)
            {
                return maze.DownRoom(this).tiles[x, 0].collision;
            }

            if (y < 0)
            {
                return maze.UpRoom(this).tiles[x, Height - 1].collision;
            }

            return tiles[x, y].collision;
        }

        public Enumeration.TileType GetType(int x, int y)
        {
            if (x < 0)
            { 
                return maze.LeftRoom(this).tiles[Width-1,y].Type;
            }

            if (x >= Width)
            {
                return maze.RightRoom(this).tiles[0, y].Type;
            }

            if (y >= Height)
            {
                return maze.DownRoom(this).tiles[x, 0].Type;
            }

            if (y < 0)
            {
                return maze.UpRoom(this).tiles[x, Height-1].Type;
            }
            return tiles[x, y].Type;
        }

      
        public Tile GetTile(Vector2 playerPosition)
        {
            int x = (int)Math.Floor((float)playerPosition.X / Tile.WIDTH);
            int y = (int)Math.Ceiling(((float)(playerPosition.Y - RoomNew.BOTTOM_BORDER) / Tile.HEIGHT));

            return GetTile(x, y);
        }

        public Tile GetTile(int x, int y)
        {
            if (x < 0)
            {
                return maze.LeftRoom(this).tiles[Width - 1, y];
            }

            if (x >= Width)
            {
                return maze.RightRoom(this).tiles[0, y];
            }

            if (y >= Height)
            {
                return maze.DownRoom(this).tiles[x, 0];
            }

            if (y < 0)
            {
                return maze.UpRoom(this).tiles[x, Height - 1];
            }
            return tiles[x, y];
        }

        public List<Tile> GetTiles(Enumeration.TileType tileType)
        {
            List<Tile> list = new List<Tile>();
            for (int y = Height - 1; y >= 0; --y)
            {
                for (int x = 0; x < Width; ++x)
                {
                   if (tileType == tiles[x, y].Type)
                    list.Add(tiles[x, y]);
                }
            }
            return list;
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

        public void Update(GameTime gameTime, KeyboardState keyboardState, GamePadState gamePadState, TouchCollection touchState, AccelerometerState accelState, DisplayOrientation orientation)
        {
            //THIS IS FOR NOT UPDATE THE BLOCK ROOM AND SAVE SOME CPU TIME....
            if (this.roomName == "MAP_blockroom.xml")
                return;
            //player.Update(gameTime, keyboardState, gamePadState, touchState, accelState, orientation);

            UpdateTilesTemporaney(gameTime, keyboardState, gamePadState, touchState, accelState, orientation);
            //maze.LeftRoom(this).UpdateTilesLeft(gameTime, keyboardState, gamePadState, touchState, accelState, orientation);
            UpdateTiles(gameTime, keyboardState, gamePadState, touchState, accelState, orientation);
            //maze.UpRoom(this).UpdateTilesUp(gameTime, keyboardState, gamePadState, touchState, accelState, orientation);

            
        }


     

        #endregion

        #region Draw

        /// <summary>
        /// Draw everything in the level from background to foreground.
        /// </summary>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawTilesInverseNew(gameTime, spriteBatch);
            //player.Draw(gameTime, spriteBatch);
        }


        private void UpdateTilesLeft(GameTime gameTime, KeyboardState keyboardState, GamePadState gamePadState, TouchCollection touchState, AccelerometerState accelState, DisplayOrientation orientation)
        {
            for (int y = Height - 1; y >= 0; --y)
            {
                for (int x = Width - 1; x < Width; ++x)
                {
                    // If there is a visible Tile in that position
                    Texture2D texture = null;
                    texture = tiles[x, y].Texture;
                    if (texture != null)
                    {
                        // Draw it in screen space.
                        Rectangle rect = new Rectangle(-1 * (int)Tile.Size.X, y * (int)Tile.Size.Y - BOTTOM_BORDER, (int)texture.Width, (int)texture.Height);
                        Vector2 position = new Vector2(rect.X, rect.Y);
                        tiles[x, y].Update(gameTime, keyboardState, gamePadState, touchState, accelState, orientation);
                    }
                }
            }
        }

        private void UpdateTiles(GameTime gameTime, KeyboardState keyboardState, GamePadState gamePadState, TouchCollection touchState, AccelerometerState accelState, DisplayOrientation orientation)
        {
            for (int y = Height - 1; y >= 0; --y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    tiles[x, y].Update(gameTime, keyboardState, gamePadState, touchState, accelState, orientation);
                }
            }
        }

        private void UpdateTilesTemporaney(GameTime gameTime, KeyboardState keyboardState, GamePadState gamePadState, TouchCollection touchState, AccelerometerState accelState, DisplayOrientation orientation)
        {
            try //workaroud to thread unsafe...?
            {
                lock (tilesTemporaney)
                {
                    foreach (Tile item in tilesTemporaney)
                    {
                        item.Update(gameTime, keyboardState, gamePadState, touchState, accelState, orientation);    // Insert your code here.
                    }
                }
            }
            catch (Exception ex)
            { }
        }


        private void UpdateTilesUp(GameTime gameTime, KeyboardState keyboardState, GamePadState gamePadState, TouchCollection touchState, AccelerometerState accelState, DisplayOrientation orientation)
        {
            int y = Height - 1;
            for (int x = 0; x < Width; ++x)
            {
                // If there is a visible Tile in that position
                Texture2D texture = null;
                texture = tiles[x, y].Texture;
                if (texture != null)
                {
                    // Draw it in screen space.
                    Rectangle rect = new Rectangle(x * (int)Tile.Size.X, -1 * (int)Tile.Size.Y - BOTTOM_BORDER, (int)texture.Width, (int)texture.Height);
                    Vector2 position = new Vector2(rect.X, rect.Y);
                    tiles[x, y].Update(gameTime, keyboardState, gamePadState, touchState, accelState, orientation);
                }
            }
        }

        private void DrawTilesLeft(GameTime gametime, SpriteBatch spriteBatch)
        {
            // For each Tile position
            for (int y = Height - 1; y >= 0; --y)
            {
                for (int x = Width - 1; x < Width; ++x)
                {
                    // If there is a visible Tile in that position
                    Texture2D texture = null;
                    texture = tiles[x, y].Texture;
                    if (texture != null)
                    {
                        // Draw it in screen space.
                        Rectangle rect = new Rectangle(-1 * (int)Tile.Size.X, y * (int)Tile.Size.Y - BOTTOM_BORDER, (int)texture.Width, (int)texture.Height);
                        Vector2 position = new Vector2(rect.X, rect.Y);
                        tiles[x, y].tileAnimation.DrawTile(gametime, spriteBatch, position, Vector2.Zero, SpriteEffects.None, 0.1f);
                    }
                }
            }

        }

        private void DrawTilesUp(GameTime gametime, SpriteBatch spriteBatch)
        {
            // For each Tile position
            int y = Height-1;
            for (int x = 0; x < Width; ++x)
            {
                // If there is a visible Tile in that position
                Texture2D texture = null;
                texture = tiles[x, y].Texture;
                if (texture != null)
                {
                    Vector2 position;
                    Rectangle rect;
                    // Draw it in screen space.
                    //if (tiles[x, y].Type == Enumeration.TileType.loose & tiles[x, y].tileState.Value().state == Enumeration.StateTile.loose)
                    //{
                    //    position.X = tiles[x, y].Position.X;
                    //    position.Y = tiles[x, y].Position.Y;
                    //}
                    //else
                    {   
                        rect = new Rectangle(x * (int)Tile.Size.X, -1 * (int)Tile.Size.Y - BOTTOM_BORDER, (int)texture.Width, (int)texture.Height);
                        position = new Vector2(rect.X, rect.Y);
                    }
                    
                    tiles[x, y].tileAnimation.DrawTile(gametime, spriteBatch, position, Vector2.Zero, SpriteEffects.None, 0.1f);
                }
            }

        }

        private void DrawTilesInverseNew(GameTime gametime, SpriteBatch spriteBatch)
        {
        

           maze.LeftRoom(this).DrawTilesLeft(gametime, spriteBatch);
            //RoomLeft().DrawTilesLeft(spriteBatch);
            


            Vector2 position = new Vector2(0, 0);
            // For each Tile position
            for (int y = Height - 1; y >= 0; --y)
            {
                for (int x = 0; x < Width; ++x)
                {                    
                    Texture2D texture = null;
                    texture = tiles[x, y].Texture;

                    //Rectangle rect = new Rectangle(x * (int)Tile.Size.X, y * (int)Tile.Size.Y - BOTTOM_BORDER, (int)texture.Width, (int)texture.Height);
                    //tiles[x, y].Position = new Position(new Vector2(rect.X, rect.Y), new Vector2(rect.X, rect.Y));
                    position = new Vector2(tiles[x, y].Position.X, tiles[x, y].Position.Y);
                    tiles[x, y].tileAnimation.DrawTile(gametime, spriteBatch, position, Vector2.Zero, SpriteEffects.None, 0.1f);

                }
            }
            //RoomUp().DrawTilesUp(spriteBatch);
            maze.UpRoom(this).DrawTilesUp(gametime, spriteBatch);

            lock (tilesTemporaney)
            {
                foreach (Tile item in tilesTemporaney)
                {
                    Vector2 p = new Vector2(item.Position.X, item.Position.Y);
                    item.tileAnimation.DrawTile(gametime, spriteBatch, p, Vector2.Zero, SpriteEffects.None, 0.1f);
                }
            }
        }


        public void SubsTile(Vector2 position, Enumeration.TileType tileType)
        {
            int x = (int)Math.Floor((float)position.X / Tile.WIDTH);
            int y = (int)Math.Ceiling(((float)(position.Y - RoomNew.BOTTOM_BORDER) / Tile.HEIGHT));

            Tile t = new Tile(this, content, tileType, Enumeration.StateTile.normal.ToString().ToUpper());
            Position p = new Position(tiles[x, y].Position._screenRealSize, tiles[x, y].Position._spriteRealSize);
            p.X = tiles[x, y].Position.X;
            p.Y = tiles[x, y].Position.Y;
            t.Position = p;
            tiles[x, y] = t;
        }

        public Vector4 getBoundTiles(Rectangle playerBounds)
        {
            int leftTile = (int)Math.Floor((float)playerBounds.Left / Tile.WIDTH);
            int rightTile = (int)Math.Ceiling(((float)playerBounds.Right / Tile.WIDTH)) - 1; //tile dal bordo sx dello schermo al bordo dx del rettangolo sprite
            int topTile = (int)Math.Floor((float)(playerBounds.Top - RoomNew.BOTTOM_BORDER) / Tile.HEIGHT); //tiles from the top screen border
            int bottomTile = (int)Math.Ceiling(((float)(playerBounds.Bottom - RoomNew.BOTTOM_BORDER) / Tile.HEIGHT)) - 1;

            //if (topTile < 0)
            //    topTile = 0;
            if (bottomTile > RoomNew.pHeight-1)
                bottomTile = 0;


            return new Vector4(leftTile,rightTile,topTile,bottomTile);
        }

        public Vector2 getCenterTile(Rectangle playerBounds)
        {
            int leftTile = (int)Math.Floor((float)playerBounds.Center.X / Tile.WIDTH);
            int topTile = (int)Math.Floor((float)(playerBounds.Center.Y - RoomNew.BOTTOM_BORDER) / Tile.HEIGHT); //tiles from the top screen border
            return new Vector2(leftTile, topTile);
        }

        #endregion
    }
}
