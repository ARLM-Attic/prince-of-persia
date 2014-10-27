	//-----------------------------------------------------------------------//
	// <copyright file="Room.cs" company="A.D.F.Software">
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
using System.IO;
using System.Collections.Generic;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace PrinceOfPersia
{
    /// <summary>
    /// A uniform grid of tiles with collections of gems and enemies.
    /// The level owns the player and controls the game's win and lose
    /// conditions as well as scoring.
    /// </summary>
    public class Room //: IDisposable
    {
        //COORDINATE ROOM
        private int _RoomRight = 0;
        private int _RoomLeft = 0;
        private int _RoomUp = 0;
        private int _RoomDown= 0;
        
        
        public Maze maze;
        public Level level;
        
        public PopNet.Map map;
        
        //
        public bool roomStart = false;
        public Vector2 roomStartPosition = new Vector2(0,0);
        public SpriteEffects roomStartDirection = SpriteEffects.None;


        public string roomName;
        public int roomNumber;

        public int roomZ;
        public int roomX;
        public int roomY;


        public Tile[,] tiles;
        // Physical structure of the level.

        private const int pWidth = 10;
        private const int pHeight = 3;
        private const int pSize = 30;
        public const int BOTTOM_BORDER = 16; //Bottom border for live energy and message space
        public const int TOP_BORDER = 6; // = (400 pixel - BOTTOM_BORDER % 3 ROWS)

        public const int LEFT_LIMIT = -70 -20;
        public const int RIGHT_LIMIT = 568 -10;
        public const int TOP_LIMIT = -50;
        public static int BOTTOM_LIMIT = PoP.CONFIG_SCREEN_HEIGHT - BOTTOM_BORDER - 50;  //

        public int widthInLevel = 0;
        public int heightInLevel = 0;

        public List<Sprite> elements = new List<Sprite>();

        public ArrayList tilesTemporaney = new ArrayList();



        //public ContentManager content
        //{
        //    get { return Maze.content; }
        //}

        public Room Left
        {
            get 
            {
                return level.FindRoom(_RoomLeft);
            }
            set 
            {
                _RoomLeft = value._RoomLeft;
            }
        }

        public Room Right
        {
            get
            {
                return level.FindRoom(_RoomRight);
            }
            set
            {
                _RoomRight = value._RoomRight;
            }

        }

        public Room Up
        {
            get
            {
                return level.FindRoom(_RoomUp);
            }
            set
            {
                _RoomUp = value._RoomUp;
            }

        }

        public Room Down
        {
            get
            {
                return level.FindRoom(_RoomDown);
            }
            set
            {
                _RoomDown = value._RoomDown;
            }

        }


        public Room(Maze Maze, Level Level, string filePath, int roomIndex)
        {
            this.maze = Maze;
            this.level = Level;
            this.roomNumber = roomIndex;


            //LOAD RES CONTENT
            System.Xml.Serialization.XmlSerializer ax;

            Stream txtReader = Microsoft.Xna.Framework.TitleContainer.OpenStream(filePath);

            //Stream astream = this.GetType().Assembly.GetManifestResourceStream(filePath);
            ax = new System.Xml.Serialization.XmlSerializer(typeof(PopNet.Map));
            map = ((PopNet.Map)ax.Deserialize(txtReader));

            LoadTilesPoPnet();
        }


        //Apoplexy.tile[] myTiles;
        public Room(Maze Maze, Level Level, Apoplexy.level ApoplexyLevel, Apoplexy.tile[] ApoplexyTiles, string roomNumber, Apoplexy.links ApoplexyLink, Apoplexy.guard[] ApoplexyGuards, Apoplexy.@event[] ApoplexyEvents, Apoplexy.prince ApoplexyPrince, string RoomName)
        {
            this.level = Level;
            this.maze = Maze;
            this.roomName = RoomName;
            this.roomNumber = int.Parse(roomNumber);

            _RoomDown = int.Parse(ApoplexyLink.down);
            _RoomUp = int.Parse(ApoplexyLink.up);
            _RoomLeft = int.Parse(ApoplexyLink.left);
            _RoomRight = int.Parse(ApoplexyLink.right);
            
            LoadTilesApoplexy(ApoplexyLevel, ApoplexyTiles, ApoplexyGuards, ApoplexyEvents);

            if (ApoplexyPrince.room == roomNumber)
            {
                if (ApoplexyPrince.direction == "1")
                    roomStartDirection = SpriteEffects.FlipHorizontally;
                else
                    roomStartDirection = SpriteEffects.None;

                int y = int.Parse(ApoplexyPrince.location) / 11;
                int x = (int.Parse(ApoplexyPrince.location) % 11)-1;

 
                int xPlayer = (x - 1) * Tile.WIDTH + Player.SPRITE_SIZE_X;
                int yPlayer = ((y + 1) * (Tile.HEIGHT)) - Sprite.SPRITE_SIZE_Y + Room.TOP_BORDER;

                roomStart = true;
                roomStartPosition = new Vector2(xPlayer, yPlayer);
            }

        }

        public List<Sprite> SpritesInRoom()
        {
            List<Sprite> list = new List<Sprite>();

            foreach (Sprite s in level.sprites)
            {
                if (s.MyRoom == this)
                    list.Add(s);
            }


            if (maze.player.MyRoom.roomNumber== this.roomNumber)
                list.Add(maze.player);

            return list;
        }


        public void LoadTilesApoplexy(Apoplexy.level Apoplexylevel, Apoplexy.tile[] ApoplexyTiles, Apoplexy.guard[] Apoplexyguards, Apoplexy.@event[] Apoplexyevents)
        {
            int ix = 0;
            int switchButton = 0;

            tiles = new Tile[10, 3]; //30 fix...sorry
            Enumeration.Items item = Enumeration.Items.none;
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    ix = 10 * y + x;

                    Enumeration.TileType nextTileType = Enumeration.TileType.space;
                    Enumeration.TileType myTileType = Enumeration.TileType.space;
                    Enumeration.StateTile myStateTile = Enumeration.StateTile.normal;
                    //convert TileType to 
                    myTileType = Utils.ParseTileType(ApoplexyTiles[ix].element);
                    myStateTile = Utils.ParseStateType(ref myTileType, ApoplexyTiles[ix].modifier);
                    switchButton = Utils.ParseSwitchButton(myTileType, ApoplexyTiles[ix].modifier);
                 

                    if (myTileType == Enumeration.TileType.flask)
                    {
                        myTileType = Enumeration.TileType.floor;
                        myStateTile = Enumeration.StateTile.normal;
                        item = Enumeration.Items.flask;
                    }
                    else if (myTileType == Enumeration.TileType.sword)
                    {
                        myTileType = Enumeration.TileType.floor;
                        myStateTile = Enumeration.StateTile.normal;
                        item = Enumeration.Items.sword;
                    }
                    else
                        item = Enumeration.Items.none;


                    tiles[x, y] = LoadTile(myTileType, myStateTile, switchButton, item, nextTileType);

                    Vector2 v = new Vector2(x * Tile.WIDTH, (y * Tile.HEIGHT) - Tile.HEIGHT_VISIBLE);

                    tiles[x, y].Position = new Position(Vector2.Zero, new Vector2(Tile.WIDTH, Tile.REALHEIGHT));
                    tiles[x, y].Position.X = v.X;
                    tiles[x, y].Position.Y = v.Y;

                }
            }
            //LOAD GUARD
            foreach(Apoplexy.guard g in Apoplexyguards)
            {
                if (g.location != "0")
                {
                    int iLocation = int.Parse(g.location);
                    int xGuard = iLocation % 10 -1;
                    int yGuard = iLocation / 10 +1;

                    xGuard = xGuard * Tile.WIDTH + Player.SPRITE_SIZE_X;
                    yGuard = (yGuard * (Tile.HEIGHT)) - Sprite.SPRITE_SIZE_Y + Room.TOP_BORDER;
                    Guard guardSprite = new Guard(this, new Vector2(xGuard, yGuard), maze.graphicsDevice, SpriteEffects.None); //to be fixed direction
                    level.sprites.Add(guardSprite);
                }
            }
        }


        public void LooseShake()
        {
            List<Tile> l = (List<Tile>)GetTiles(Enumeration.TileType.loose);
            foreach (Loose item in l)
            {
                item.Shake();
            }
        }


        private void LoadTilesPoPnet()
        {
            // Allocate the Tile grid.
            tiles = new Tile[map.rows[0].columns.Length, map.rows.Length];
            int x = 0; int y = 0; int newX = 0;

            foreach (PopNet.Row r in map.rows)
            {
                for (int ix = 0; ix < r.columns.Length; ix++ )
                {
                    Enumeration.TileType nextTileType = Enumeration.TileType.space;
                    if (ix+1 < r.columns.Length)
                        nextTileType = r.columns[ix+1].tileType;

                    tiles[x, y] = LoadTile(r.columns[ix].tileType, r.columns[ix].state, r.columns[ix].switchButton, r.columns[ix].item, nextTileType);
                    //tiles[x, y].tileAnimation.fra = maze.player.sprite.frameRate_background;
                    Rectangle rect = new Rectangle(x * (int)Tile.Size.X, y * (int)Tile.Size.Y - BOTTOM_BORDER, (int)tiles[x, y].Texture.Width, (int)tiles[x, y].Texture.Height);
                    Vector2 v = new Vector2(rect.X, rect.Y);

                    tiles[x, y].Position = new Position(v, v);
                    tiles[x, y].Position.X = v.X;
                    tiles[x, y].Position.Y = v.Y;

                    //x+1 for avoid base zero x array, WALL POSITION 0-29
                    tiles[x, y].panelInfo = newX + roomNumber;

                    switch (r.columns[ix].spriteType)
                    {
                        case Enumeration.SpriteType.kid :
                            int xPlayer = (x - 1) * Tile.WIDTH + Player.SPRITE_SIZE_X;
                            int yPlayer = ((y + 1) * (Tile.HEIGHT)) - Sprite.SPRITE_SIZE_Y + Room.TOP_BORDER;
                            maze.player = new Player(maze.graphicsDevice, this);
                            break;

                        case Enumeration.SpriteType.guard :
                            int xGuard = (x-1) * Tile.WIDTH + Player.SPRITE_SIZE_X;
                            //int yGuard = (y + 1) * (Tile.HEIGHT - Sprite.PLAYER_STAND_FLOOR_PEN - RoomNew.BOTTOM_BORDER + RoomNew.TOP_BORDER);
                            int yGuard = ((y + 1) * (Tile.HEIGHT)) - Sprite.SPRITE_SIZE_Y + Room.TOP_BORDER;
                            Guard g = new Guard(this, new Vector2(xGuard, yGuard), maze.graphicsDevice, r.columns[ix].spriteEffect);
                            level.sprites.Add(g);
                            break;


                        default:
                            break;
                    }


                    x++;
                    newX++;
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
            return new Tile(this, tiletype, Enumeration.StateTile.normal, Enumeration.Items.none, Enumeration.TileType.space);
        }

        private Tile LoadTile(Enumeration.TileType tiletype, Enumeration.StateTile state, int switchButton, Enumeration.Items item, Enumeration.TileType nextTileType)
        {
            switch (tiletype)
            {
                case Enumeration.TileType.spikes:
                    return new Spikes(this, tiletype, state, nextTileType);
                    break;

                case Enumeration.TileType.pressplate:
                    return new PressPlate(this, tiletype, state, switchButton, nextTileType);
                    break;

                case Enumeration.TileType.gate:
                    return new Gate(this, tiletype, state, switchButton, nextTileType);
                    break;

                case Enumeration.TileType.loose:
                    return new Loose(this, tiletype, state, nextTileType);
                    break;

                case Enumeration.TileType.block:
                    return new Block(this, tiletype, state, nextTileType);
                    break;

                case Enumeration.TileType.exit:
                    return new Exit(this, tiletype, state, switchButton, nextTileType);
                    break;

                case Enumeration.TileType.chomper:
                    return new Chomper(this, tiletype, state, nextTileType);
                    break;

                default:
                    return new Tile(this, tiletype, state, item, nextTileType);
            }
            
        }

        /// <summary>
        /// Unloads the level content.
        /// </summary>
        public void Dispose()
        {
            //content.Unload();
        }

        /// <summary>
        /// Restores the player to the starting point to try the level again.
        /// </summary>
        public void StartNewLife()
        {
            //Play Sound presentation
            //Maze.content.Load<SoundEffect>(PrinceOfPersiaGame.CONFIG_SOUNDS + "presentation").Play();
            ((SoundEffect)Maze.Content[PoP.CONFIG_SOUNDS + "presentation"]).Play();
            maze.player.Reset();
            //Maze.content.Load<SoundEffect>(PrinceOfPersiaGame.CONFIG_SOUNDS + "YouMustRescue").Play();
            ((SoundEffect)Maze.Content[PoP.CONFIG_SOUNDS + "YouMustRescue"]).Play();

        }

    

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
                if (y < 0)
                    return Left.tiles[Width - 1, Height - 1].collision;
                else
                    return Left.tiles[Width - 1, y].collision;
            }

            if (x >= Width)
            {
                if (y < 0)
                    return Right.tiles[0, Height - 1].collision;
                else
                    return Right.tiles[0, y].collision;
            }

            if (y >= Height)
            {
                return Down.tiles[x, 0].collision;
            }

            if (y < 0)
            {
                return Up.tiles[x, Height - 1].collision;
            }

            return tiles[x, y].collision;
        }

        public Enumeration.TileType GetType(int x, int y)
        {
            if (x < 0)
            {
                if (y < 0)
                    return Left.tiles[Width-1, Height - 1].Type;
                else
                    return Left.tiles[Width - 1, y].Type;
            }

            if (x >= Width)
            {
                if (y < 0)
                    return Right.tiles[0, Height - 1].Type;
                else
                    return Right.tiles[0, y].Type;
            }

            if (y >= Height)
            {
                return Down.tiles[x, 0].Type;
            }

            if (y < 0)
            {
                return Up.tiles[x, Height-1].Type;
            }
            return tiles[x, y].Type;
        }

      
        public Tile GetTile(Vector2 playerPosition)
        {
            int x = (int)Math.Floor((float)playerPosition.X / Tile.WIDTH);
            int y = (int)Math.Floor(((float)playerPosition.Y / Tile.HEIGHT));
            //int y = (int)Math.Floor(((float)(playerPosition.Y - RoomNew.BOTTOM_BORDER) / Tile.HEIGHT));

            return GetTile(x, y);
        }

        public Tile GetTile(int x, int y)
        {
            //int newX = x;
            //int newY = y;

            //if (x < 0)
            //    newX = Width - 1;
            //if (x >= Width)
            //    newX = 0;
            //if (y <0 )
            //    newY = Height - 1;
            //if (y >= Height)
            //    newY = 0;

            if (x < 0)
            {
                return Left.GetTile(Width - 1, y);
            }

            if (x >= Width)
            {
                return Right.GetTile(0, y);
            }

            if (y >= Height)
            {
                return Down.GetTile(x, 0);
            }

            if (y < 0)
            {
                return Up.GetTile(x, Height - 1);
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

            return new Rectangle(x * Tile.WIDTH, by, Tile.WIDTH, Tile.HEIGHT);
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

            UpdateTilesTemporaney(gameTime, keyboardState, gamePadState, touchState, accelState, orientation);
            UpdateTiles(gameTime, keyboardState, gamePadState, touchState, accelState, orientation);

            UpdateItems(gameTime, keyboardState, gamePadState, touchState, accelState, orientation);

            
        }


     

        #endregion

        #region Draw

        /// <summary>
        /// Draw everything in the level from background to foreground.
        /// </summary>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawTilesInverse(gameTime, spriteBatch);
        }

        /// <summary>
        /// Draw everything in the level from background to foreground.
        /// </summary>
        public void DrawMask(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawTilesMask(gameTime, spriteBatch);
            DrawTilesBlocks(gameTime, spriteBatch);
        }

        public void DrawSprites(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Sprite s in SpritesInRoom())
            {
                switch (s.GetType().Name)
                {
                    case "Guard":
                        ((Guard)s).Draw(gameTime, spriteBatch);
                        break;

                    case "Splash":
                        ((Splash)s).Draw(gameTime, spriteBatch);
                        break;
                    
                    default:
                        break;
                }
            }
            
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

        private void UpdateItems(GameTime gameTime, KeyboardState keyboardState, GamePadState gamePadState, TouchCollection touchState, AccelerometerState accelState, DisplayOrientation orientation)
        {
            for (int y = Height - 1; y >= 0; --y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    if (tiles[x, y].item != null)
                        tiles[x, y].item.Update(gameTime, keyboardState, gamePadState, touchState, accelState, orientation);
                }
            }
        }


        private void UpdateSprites(GameTime gameTime, KeyboardState keyboardState, GamePadState gamePadState, TouchCollection touchState, AccelerometerState accelState, DisplayOrientation orientation)
        {
            foreach(Sprite s in SpritesInRoom())
            {
                    ((Guard)s).Update(gameTime, keyboardState, gamePadState, touchState, accelState, orientation);
            }
            
        }

        private void UpdateTilesTemporaney(GameTime gameTime, KeyboardState keyboardState, GamePadState gamePadState, TouchCollection touchState, AccelerometerState accelState, DisplayOrientation orientation)
        {
            try
            {
                lock (tilesTemporaney)
                {
                    foreach (Tile item in tilesTemporaney)
                    {
                        item.Update(gameTime, keyboardState, gamePadState, touchState, accelState, orientation);    // Insert your code here.
                    }
                }
            }
            catch (Exception ex) { }
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
                        tiles[x, y].tileAnimation.DrawTile(gametime, spriteBatch, position, SpriteEffects.None, 0.1f);
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
                    
                    tiles[x, y].tileAnimation.DrawTile(gametime, spriteBatch, position, SpriteEffects.None, 0.1f);
                }
            }

        }


        private void DrawTilesDown(GameTime gametime, SpriteBatch spriteBatch)
        {
            // For each Tile position
            int y = 0;
            for (int x = 0; x < Width; ++x)
            {
                // If there is a visible Tile in that position
                Texture2D texture = null;
                texture = tiles[x, y].Texture;
                if (texture != null)
                {
                    Vector2 position;
                    Rectangle rect;
                    rect = new Rectangle(x * (int)Tile.Size.X, (Height-y) * (int)Tile.Size.Y - BOTTOM_BORDER, (int)texture.Width, (int)texture.Height);
                    position = new Vector2(rect.X, rect.Y);

                    tiles[x, y].tileAnimation.DrawTile(gametime, spriteBatch, position, SpriteEffects.None, 0.1f);
                }
            }

        }

        private void DrawTilesBlocks(GameTime gametime, SpriteBatch spriteBatch)
        {
            Texture2D texture = null;
            //Rectangle rectangleMask = new Rectangle();

            Vector2 position = new Vector2(0, 0);
            // For each Tile position
            for (int y = Height - 1; y >= 0; --y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    if (tiles[x, y].Type == Enumeration.TileType.block)
                    {

                        if (x > 0 && (tiles[x-1, y].Type == Enumeration.TileType.space | tiles[x-1, y].Type == Enumeration.TileType.floor))
                        {
                            position = new Vector2(tiles[x, y].Position.X, tiles[x, y].Position.Y);
                            texture = (Texture2D)Maze.Content[PoP.CONFIG_TILES + "Block_left"];
                            //texture = Maze.Content.Load<Texture2D>(PrinceOfPersiaGame.CONFIG_TILES + "Block_left");
                            tiles[x, y].tileAnimation.DrawTile(gametime, spriteBatch, position, SpriteEffects.None, 0.1f, texture);

                            //divider
                            position = new Vector2(tiles[x, y].Position.X+16, tiles[x, y].Position.Y+64);
                            texture = (Texture2D)Maze.Content[PoP.CONFIG_TILES + "Block_divider2"];
                            //texture = Maze.content.Load<Texture2D>(PrinceOfPersiaGame.CONFIG_TILES + "Block_divider2");
                            tiles[x, y].tileAnimation.DrawTile(gametime, spriteBatch, position, SpriteEffects.None, 0.1f, texture);

                        }

                        if (tiles[x, y].nextTileType == Enumeration.TileType.block)
                        {
                            for (int s = 0; s< Block.seed_graystone.GetLength(1); s++)
                            {
                                if (Block.seed_graystone[0, s] == tiles[x, y].panelInfo)
                                {
                                    position = new Vector2(tiles[x, y].Position.X, tiles[x, y].Position.Y+21);
                                    texture = (Texture2D)Maze.Content[PoP.CONFIG_TILES + "Block_random"];
                                    //texture = Maze.content.Load<Texture2D>(PrinceOfPersiaGame.CONFIG_TILES + "Block_random");
                                    tiles[x, y].tileAnimation.DrawTile(gametime, spriteBatch, position, SpriteEffects.None, 0.1f, texture);
                                }
                                if (Block.seed_left_top[2, s] == ((Block)tiles[x, y]).panelInfo)
                                {
                                    position = new Vector2(tiles[x, y].Position.X, tiles[x, y].Position.Y);
                                    texture = (Texture2D)Maze.Content[PoP.CONFIG_TILES + "Block_left"];
                                    //texture = Maze.content.Load<Texture2D>(PrinceOfPersiaGame.CONFIG_TILES + "Block_left");
                                    tiles[x, y].tileAnimation.DrawTile(gametime, spriteBatch, position, SpriteEffects.None, 0.1f, texture);

                                    //divider
                                    position = new Vector2(tiles[x, y].Position.X + 22, tiles[x, y].Position.Y + 64);
                                    texture = (Texture2D)Maze.Content[PoP.CONFIG_TILES + "Block_divider"];
                                    //texture = Maze.content.Load<Texture2D>(PrinceOfPersiaGame.CONFIG_TILES + "Block_divider");
                                    tiles[x, y].tileAnimation.DrawTile(gametime, spriteBatch, position, SpriteEffects.None, 0.1f, texture);

                                }


                            }

                            

                        }
                    }
                }
            }

            Up.DrawTilesUp(gametime, spriteBatch);
        }

        private void DrawTilesMask(GameTime gametime, SpriteBatch spriteBatch)
        {
            Rectangle rectangleMask = new Rectangle();
            Vector2 position = new Vector2(0, 0);
            // For each Tile position
            for (int y = Height - 1; y >= 0; --y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    position = new Vector2(tiles[x, y].Position.X, tiles[x, y].Position.Y);

               

                    switch(tiles[x, y].Type)
                    {
                        case Enumeration.TileType.chomper:
                            //rectangleMask = Tile.MASK_CHOMPER;
                            //position.Y = position.Y + 128;
                            rectangleMask = new Rectangle(0, 0, 26, 148);
                            break;

                        case Enumeration.TileType.posts:
                            rectangleMask = Tile.MASK_POSTS;
                            break;
                        case Enumeration.TileType.gate:
                            position.X = position.X + 50;
                            rectangleMask = Tile.MASK_DOOR;
                            break;
                        case Enumeration.TileType.block :
                            rectangleMask = Tile.MASK_BLOCK;
                            break;
                        case Enumeration.TileType.exit:
                            //position.Y -= 100;
                            //rectangleMask = Tile.MASK_FLOOR;
                            return;

                        default:
                            position.Y = position.Y + 128;
                            rectangleMask = Tile.MASK_FLOOR;
                            break;
                    }

                    tiles[x, y].tileAnimation.DrawTileMask(gametime, spriteBatch, position, SpriteEffects.None, 0.1f, rectangleMask);

                    if (tiles[x, y].item != null)
                    {
                        position = new Vector2(tiles[x, y].Position.X, tiles[x, y].Position.Y);
                        tiles[x, y].item.itemAnimation.DrawItem(gametime, spriteBatch, position, SpriteEffects.None, 0.1f);
                    }
                }
            }
            
            Up.DrawTilesUp(gametime, spriteBatch);
        }


        private void DrawTilesInverse(GameTime gametime, SpriteBatch spriteBatch)
        {
        

           Left.DrawTilesLeft(gametime, spriteBatch);
           Down.DrawTilesDown(gametime, spriteBatch);


            Vector2 position = new Vector2(0, 0);

            // For each Tile position
            for (int y = Height - 1; y >= 0; --y)
            {
                for (int x = 0; x < Width; ++x)
                {                    
                    Texture2D texture = null;
                    texture = tiles[x, y].Texture;

                    position = new Vector2(tiles[x, y].Position.X, tiles[x, y].Position.Y);

                    tiles[x, y].tileAnimation.DrawTile(gametime, spriteBatch, position, SpriteEffects.None, 0.1f);
                }
            }
            

            lock (tilesTemporaney)
            {
                foreach (Tile item in tilesTemporaney)
                {
                    Vector2 p = new Vector2(item.Position.X, item.Position.Y);
                    item.tileAnimation.DrawTile(gametime, spriteBatch, p, SpriteEffects.None, 0.1f);
                }
            }
        }


        public void SubsTile(Vector2 coordinate, Enumeration.TileType tileType)
        {
            int x = (int)coordinate.X;// (int)Math.Floor((float)position.X / Tile.WIDTH);
            int y = (int)coordinate.Y;  //(int)Math.Ceiling(((float)(position.Y - RoomNew.BOTTOM_BORDER) / Tile.HEIGHT));

            Tile t = new Tile(this, tileType, Enumeration.StateTile.normal, Enumeration.Items.none, Enumeration.TileType.space);
            Position p = new Position(tiles[x, y].Position._screenRealSize, tiles[x, y].Position._spriteRealSize);
            p.X = tiles[x, y].Position.X;
            p.Y = tiles[x, y].Position.Y;
            t.Position = p;
            tiles[x, y] = t;
            tiles[x, y].tileAnimation.PlayAnimation(t.TileSequence, t.tileState);
        }

        public void SubsTileState(Vector2 position, Enumeration.StateTile state)
        {
            int x = (int)position.X;
            int y = (int)position.Y;

            tiles[x, y].tileState.Value().state = state;
            tiles[x, y].tileAnimation.PlayAnimation(tiles[x, y].TileSequence, tiles[x, y].tileState);
        }


        public Vector4 getBoundTiles(Rectangle playerBounds)
        {
            int leftTile = (int)Math.Floor((float)playerBounds.Left / Tile.WIDTH);
            int rightTile = (int)Math.Ceiling(((float)playerBounds.Right / Tile.WIDTH)) - 1; //tile dal bordo sx dello schermo al bordo dx del rettangolo sprite
            int topTile = (int)Math.Floor((float)(playerBounds.Top - Room.BOTTOM_BORDER) / Tile.HEIGHT); //tiles from the top screen border
            int bottomTile = (int)Math.Ceiling(((float)(playerBounds.Bottom - Room.BOTTOM_BORDER) / Tile.HEIGHT)) - 1;

            //if (topTile < 0)
            //    topTile = 0;
            if (bottomTile > Room.pHeight-1)
                bottomTile = topTile;


            return new Vector4(leftTile,rightTile,topTile,bottomTile);
        }

        public Vector2 getCenterTilePosition(Rectangle playerBounds)
        {
            int leftTile = (int)(playerBounds.Center.X / Tile.WIDTH);
            if (playerBounds.Center.X < Sprite.PLAYER_STAND_FEET)
                leftTile = -1;

            //int leftTile = (int)Math.Floor((float)playerBounds.Center.X / Tile.WIDTH);
            //int leftTile = (int)Math.Floor((float)(playerBounds.Center.X) / Tile.WIDTH);
            int topTile = (int)Math.Floor((float)(playerBounds.Center.Y - Tile.HEIGHT_VISIBLE) / Tile.HEIGHT); //tiles from the top screen border
            return new Vector2(leftTile, topTile);
        }

        public Enumeration.TileType getCenterTileType(Rectangle playerBounds)
        {
            return getCenterTile(playerBounds).Type;
        }

        public Tile getCenterTile(Rectangle playerBounds)
        {
            //int leftTile = (int)Math.Floor((float)playerBounds.Center.X / Tile.WIDTH);
            //int topTile = (int)Math.Floor((float)(playerBounds.Center.Y - Tile.HEIGHT_VISIBLE) / Tile.HEIGHT); //tiles from the top screen border
            Vector2 v = getCenterTilePosition(playerBounds);
            return this.GetTile((int)v.X, (int)v.Y);
        }


        #endregion
    }
}
