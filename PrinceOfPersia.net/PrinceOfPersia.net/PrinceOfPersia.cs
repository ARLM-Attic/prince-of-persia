using System;
using System.Collections.Generic;
using System.IO;
using System.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Content;
using GameStateManagement;

namespace PrinceOfPersia
{

 

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class PrinceOfPersiaGame : GameScreen
    {

        private Texture2D[] playerTexture = new Texture2D[128];

        // Resources for drawing.
        //private GraphicsDeviceManager graphics;
        //private SpriteBatch spriteBatch;

        // Global content.
        [ContentSerializerIgnore] 
        List<Sequence> l ;
        [ContentSerializerIgnore] 
        private SpriteFont hudFont;
        [ContentSerializerIgnore]
        private SpriteFont PoPFont;
        [ContentSerializerIgnore] 
        private Texture2D winOverlay;
        [ContentSerializerIgnore] 
        private Texture2D loseOverlay;
        [ContentSerializerIgnore] 
        private Texture2D diedOverlay;

     

        // Meta-level game state.
        private Level[] levels = new Level[30];
        private int levelIndex = 0;
        private bool wasContinuePressed;
        private Maze maze;


        // When the time remaining is less than the warning time, it blinks on the hud
        private static readonly TimeSpan WarningTime = TimeSpan.FromSeconds(30);

        // We store our input states so that we only poll once per frame, 
        // then we use the same input state wherever needed
        private GamePadState gamePadState;
        private KeyboardState keyboardState;
        private TouchCollection touchState;
        private AccelerometerState accelerometerState;
        
        // The number of levels in the Levels directory of our content. We assume that
        // levels in our content are 0-based and that all numbers under this constant
        // have a level file present. This allows us to not need to check for the file
        // or handle exceptions, both of which can add unnecessary time to level loading.
        private const int numberOfLevels = 3;

        public static bool CONFIG_DEBUG = false;
        public static float CONFIG_FRAMERATE = 0.09f;
        public static string CONFIG_SPRITE_KID = "KID_DOS";
        //public static string CONFIG_PATH_RESOURCES = System.AppDomain.CurrentDomain.BaseDirectory + "/Content/";
        public static string CONFIG_PATH_CONTENT = System.AppDomain.CurrentDomain.BaseDirectory + @"Content\";
        public static string CONFIG_PATH_LEVELS = @"Levels\";
        public static string CONFIG_PATH_ROOMS = @"Rooms\";
        public static string CONFIG_PATH_SEQUENCES = @"Sequences\";


        

        ContentManager content;

        public SpriteBatch spriteBatch
        {
            get { return base.ScreenManager.SpriteBatch; }
        }

        public GraphicsDevice GraphicsDevice
        {
            get { return base.ScreenManager.GraphicsDevice; }
        }


        


        public PrinceOfPersiaGame()
        {

            //READ APP.CONFIG for configuration settings
            bool.TryParse(ConfigurationSettings.AppSettings["CONFIG_debug"], out CONFIG_DEBUG);
            float.TryParse(ConfigurationSettings.AppSettings["CONFIG_framerate"], out CONFIG_FRAMERATE);
            CONFIG_SPRITE_KID = ConfigurationSettings.AppSettings["CONFIG_sprite_kid"].ToString();
            //CONFIG_PATH_CONTENT = ConfigurationSettings.AppSettings["CONFIG_path_content"].ToString();
            
            AnimationSequence.frameRate = CONFIG_FRAMERATE;
            Accelerometer.Initialize();
        }



        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void Activate(bool instancePreserved)
        {
            if (!instancePreserved)
            {
                if (content == null)
                    content = new ContentManager(ScreenManager.Game.Services, CONFIG_PATH_CONTENT);

                LoadContent();

                //LOAD MAZE
                maze = new Maze(content);
                //NOW START
                maze.StartRoom().StartNewLife(ScreenManager.GraphicsDevice);
                
                // once the load has finished, we use ResetElapsedTime to tell the game's
                // timing mechanism that we have just finished a very long frame, and that
                // it should not try to catch up.
                ScreenManager.Game.ResetElapsedTime();
            }

        }


        public override void Deactivate()
        {
            base.Deactivate();
        }


        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void Unload()
        {
            content.Unload();

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, CONFIG_PATH_CONTENT);
                

            // Load fonts
            hudFont = content.Load<SpriteFont>("Fonts/Hud");
            PoPFont = content.Load<SpriteFont>("Fonts/PoP");


            //Known issue that you get exceptions if you use Media PLayer while connected to your PC
            //See http://social.msdn.microsoft.com/Forums/en/windowsphone7series/thread/c8a243d2-d360-46b1-96bd-62b1ef268c66
            //Which means its impossible to test this from VS.
            //So we have to catch the exception and throw it away
            try
            {
                //MediaPlayer.IsRepeating = true;
                //MediaPlayer.Play(Content.Load<Song>("Sounds/Music"));
            }
            catch { }


       
        


        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            HandleInput(gameTime, null);

            //maze.playerRoom.Update(gameTime, keyboardState, gamePadState, touchState, accelerometerState, Window.CurrentOrientation);

            foreach (RoomNew r in maze.rooms)
            {
                r.Update(gameTime, keyboardState, gamePadState, touchState, accelerometerState, Microsoft.Xna.Framework.DisplayOrientation.Default);
            }
            maze.player.Update(gameTime, keyboardState, gamePadState, touchState, accelerometerState, Microsoft.Xna.Framework.DisplayOrientation.Default);


            base.Update(gameTime, otherScreenHasFocus, false);
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            //// get all of our input states
            keyboardState = Keyboard.GetState();
            //gamePadState = GamePad.GetState(PlayerIndex.One);
            touchState = TouchPanel.GetState();
            accelerometerState = Accelerometer.GetState();

            // Exit the game when back is pressed.
           ///// if (gamePadState.Buttons.Back == ButtonState.Pressed)
               ////// Exit();

            bool continuePressed =
                keyboardState.IsKeyDown(Keys.Space) ||
                gamePadState.IsButtonDown(Buttons.A) ||
                touchState.AnyTouch();


            wasContinuePressed = continuePressed;
        }

        

        /// <summary>
        /// Draws the game from background to foreground.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime) 
        {
            //graphics.GraphicsDevice.Clear(Color.Black);

            ////spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied);
            base.ScreenManager.SpriteBatch.Begin();

            DrawBottom();

            maze.PlayerRoom.Draw(gameTime, spriteBatch);
            maze.player.Draw(gameTime, spriteBatch);

            DrawHud();
            DrawDebug(maze.PlayerRoom);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawBottom()
        {
            int width = base.ScreenManager.Game.Window.ClientBounds.Width;
            int height = base.ScreenManager.Game.Window.ClientBounds.Height;
            int bottomBorderHeight = RoomNew.BOTTOM_BORDER;

            Vector2 hudLocation = new Vector2(0, height - bottomBorderHeight);

            DrawShadowedString(PoPFont, maze.PlayerRoom.roomName, hudLocation, Color.White);
        }

        private void DrawDebug(RoomNew room)
        {
            //if (room.player.sprite.sequence == null)
            //    return;


            if (CONFIG_DEBUG == false)
                return;

            Rectangle titleSafeArea = GraphicsDevice.Viewport.TitleSafeArea;
            Vector2 hudLocation = new Vector2(titleSafeArea.X, titleSafeArea.Y);

            DrawShadowedString(hudFont, "ROMM NAME=" + maze.PlayerRoom.roomName, hudLocation, Color.White);
            hudLocation.Y = hudLocation.Y + 10;

            DrawShadowedString(hudFont, "POSTION X=" + maze.player.Position.X.ToString() + " Y=" + maze.player.Position.Y.ToString(), hudLocation, Color.White);
            hudLocation.Y = hudLocation.Y + 10;
            DrawShadowedString(hudFont, "FRAME RATE=" + AnimationSequence.frameRate.ToString(), hudLocation, Color.White);

            hudLocation.Y = hudLocation.Y + 10;
            DrawShadowedString(hudFont, "PLAYER STATE=" + maze.player.playerState.Value().state + " SEQUENCE CountOffset=" + maze.player.sprite.sequence.CountOffSet, hudLocation, Color.White);

            // Get the player's bounding rectangle and find neighboring tiles.
            Rectangle playerBounds = maze.player.Position.Bounding;
            Vector4 v4 = room.getBoundTiles(playerBounds);

            // For each potentially colliding Tile, warning the for check only the player row ground..W
            for (int y = (int)v4.Z; y <= (int)v4.W; ++y)
            {
                for (int x = (int)v4.X; x <= (int)v4.Y; ++x)
                {
                    Rectangle tileBounds = room.GetBounds(x, y);
                    Vector2 depth = RectangleExtensions.GetIntersectionDepth(playerBounds, tileBounds);
                    Enumeration.TileCollision tileCollision = room.GetCollision(x, y);
                    Enumeration.TileType tileType = room.GetType(x, y);

                    hudLocation.Y = hudLocation.Y + 10;
                    DrawShadowedString(hudFont, "GRID X=" + x + " Y=" + y + " TILETYPE=" + tileType.ToString() + " BOUND X=" + tileBounds.X + " Y=" + tileBounds.Y + " DEPTH X=" + depth.X + " Y=" + depth.Y, hudLocation, Color.White);
                }
            }
        }


        private void DrawHud()
        {
            //RoomNew room = maze.playerRoom;
            Rectangle titleSafeArea = GraphicsDevice.Viewport.TitleSafeArea;
            Vector2 hudLocation = new Vector2(titleSafeArea.X, titleSafeArea.Y);
            Vector2 center = new Vector2(titleSafeArea.X + titleSafeArea.Width / 2.0f,
                                         titleSafeArea.Y + titleSafeArea.Height / 2.0f);

            // Draw score
            hudLocation.X = hudLocation.X + 420;
            DrawShadowedString(hudFont, "PrinceOfPersia.net alpha version: " + RetrieveLinkerTimestamp().ToShortDateString(), hudLocation, Color.White);

        
        }

        private void DrawShadowedString(SpriteFont font, string value, Vector2 position, Color color)
        {
            if (value == null)
                value = "MAP_blockroom.xml";
            spriteBatch.DrawString(font, value, position + new Vector2(1.0f, 1.0f), Color.Black);
            spriteBatch.DrawString(font, value, position, color);
        }

        private DateTime RetrieveLinkerTimestamp()
        {
            string filePath = System.Reflection.Assembly.GetCallingAssembly().Location;
            const int c_PeHeaderOffset = 60;
            const int c_LinkerTimestampOffset = 8;
            byte[] b = new byte[2048];
            System.IO.Stream s = null;

            try
            {
                s = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                s.Read(b, 0, 2048);
            }
            finally
            {
                if (s != null)
                {
                    s.Close();
                }
            }

            int i = System.BitConverter.ToInt32(b, c_PeHeaderOffset);
            int secondsSince1970 = System.BitConverter.ToInt32(b, i + c_LinkerTimestampOffset);
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0);
            dt = dt.AddSeconds(secondsSince1970);
            dt = dt.AddHours(TimeZone.CurrentTimeZone.GetUtcOffset(dt).Hours);
            return dt;
        }

    }
}

