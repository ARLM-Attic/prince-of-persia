using System;
using System.IO;
using System.Collections.Generic;
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
        public Player player;
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
        public const int BOTTOM_LIMIT = 400;

        public int widthInLevel = 0;
        public int heightInLevel = 0;

        // Key locations in the level.        
        private Vector2 start;

        public ContentManager content
        {
            get { return maze.content; }
        }
        #region Loading

        public RoomNew(Maze mazeRef, string filePath)
        {
            maze = mazeRef;

            System.Xml.Serialization.XmlSerializer ax;
            Stream astream = this.GetType().Assembly.GetManifestResourceStream(filePath);
            ax = new System.Xml.Serialization.XmlSerializer(typeof(Map));
            map = ((Map)ax.Deserialize(astream));

            LoadTiles();
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
                    tiles[x, y] = LoadTile(c.tileType, c.name);
                    Rectangle rect = new Rectangle(x * (int)Tile.Size.X, y * (int)Tile.Size.Y - BOTTOM_BORDER, (int)tiles[x, y].Texture.Width, (int)tiles[x, y].Texture.Height);
                    Vector2 v = new Vector2(rect.X, rect.Y);

                    tiles[x, y].Position = new Position(v, v);
                    tiles[x, y].Position.X = v.X;
                    tiles[x, y].Position.Y = v.Y;

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
        private Tile LoadTile(TileType tiletype)
        {
                return new Tile(content, tiletype, "NORMAL");
        }

        private Tile LoadTile(TileType tiletype, string name)
        {
            return new Tile(content, tiletype, name);
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
            if (player == null)
                player = new Player(this, start, graphicsDevice);
            else
                player.Reset(start);
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

        public void Update(GameTime gameTime, KeyboardState keyboardState, GamePadState gamePadState, TouchCollection touchState, AccelerometerState accelState, DisplayOrientation orientation)
        {
            player.Update(gameTime, keyboardState, gamePadState, touchState, accelState, orientation);

            for (int y = Height - 1; y >= 0; --y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    tiles[x, y].Update(gameTime, keyboardState, gamePadState, touchState, accelState, orientation);
                }
            }
        }


     

        #endregion

        #region Draw

        /// <summary>
        /// Draw everything in the level from background to foreground.
        /// </summary>
        public void Draw(Game g, GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawTilesInverseNew(gameTime, spriteBatch);
            player.Draw(gameTime, spriteBatch);
        }




        private void DrawTilesLeft(SpriteBatch spriteBatch)
        {
            Vector2 v = new Vector2(0, 0);
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
            int y = Height-1;
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

        private void DrawTilesInverseNew(GameTime gametime, SpriteBatch spriteBatch)
        {
            maze.LeftRoom(this).DrawTilesLeft(spriteBatch);
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
            maze.UpRoom(this).DrawTilesUp(spriteBatch);
        }




        public Vector4 getBoundTiles(Rectangle playerBounds)
        {
            int leftTile = (int)Math.Floor((float)playerBounds.Left / Tile.WIDTH);
            int rightTile = (int)Math.Ceiling(((float)playerBounds.Right / Tile.WIDTH)) - 1; //tile dal bordo sx dello schermo al bordo dx del rettangolo sprite
            int topTile = (int)Math.Floor((float)(playerBounds.Top - RoomNew.BOTTOM_BORDER) / Tile.HEIGHT); //tiles from the top screen border
            int bottomTile = (int)Math.Ceiling(((float)(playerBounds.Bottom - RoomNew.BOTTOM_BORDER) / Tile.HEIGHT)) - 1;
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
