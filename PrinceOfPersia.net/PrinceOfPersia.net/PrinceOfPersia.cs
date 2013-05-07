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


namespace PrinceOfPersia
{

 

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class PrinceOfPersiaGame : GameScreen
    {

       //private Texture2D[] playerTexture = new Texture2D[128];

        // Resources for drawing.
        //private GraphicsDeviceManager graphics;
        //private SpriteBatch spriteBatch;

        // Global content.
        //[ContentSerializerIgnore] 
        //List<Sequence> l ;
        //[ContentSerializerIgnore] 
        private SpriteFont hudFont;
        //[ContentSerializerIgnore]
        private SpriteFont PoPFont;
        //[ContentSerializerIgnore] 

        public static Texture2D player_livePoints;
        public static Texture2D player_energy;

        public static Texture2D enemy_livePoints;
        public static Texture2D enemy_energy;

        public static Texture2D player_splash;
        public static Texture2D enemy_splash;

        // Meta-level game state.
        private Level[] levels = new Level[30];
        //private int levelIndex = 0;
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
        public static string CONFIG_PATH_CONTENT = System.AppDomain.CurrentDomain.BaseDirectory + @"Content\";
        public static string CONFIG_PATH_LEVELS = @"Levels\";
        public static string CONFIG_PATH_ROOMS = @"Rooms\";
        public static string CONFIG_PATH_SEQUENCES = @"Sequences\";

        public static int CONFIG_KID_START_ENERGY = 3;
        
        

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

#if ANDROID
            CONFIG_PATH_CONTENT = System.AppDomain.CurrentDomain.BaseDirectory + "Content/";
            CONFIG_PATH_LEVELS = "Levels/";
            CONFIG_PATH_ROOMS = "Rooms/";
            CONFIG_PATH_SEQUENCES = "Sequences/";
#endif

#if WINDOWS
            //READ APP.CONFIG for configuration settings
            bool.TryParse(ConfigurationSettings.AppSettings["CONFIG_debug"], out CONFIG_DEBUG);
            float.TryParse(ConfigurationSettings.AppSettings["CONFIG_framerate"], out CONFIG_FRAMERATE);
            CONFIG_SPRITE_KID = ConfigurationSettings.AppSettings["CONFIG_sprite_kid"].ToString();

            CONFIG_KID_START_ENERGY = int.Parse(ConfigurationSettings.AppSettings["CONFIG_kid_start_energy"].ToString());

            CONFIG_PATH_CONTENT = "Content/";
#endif 
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
                    content = ScreenManager.content;
                    //content = new ContentManager(ScreenManager.Game.Services, CONFIG_PATH_CONTENT);

                LoadContent();

                //LOAD MAZE
                maze = new Maze(GraphicsDevice, content);
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
                
            // Load fonts
            hudFont = content.Load<SpriteFont>("Fonts/Hud");
            PoPFont = content.Load<SpriteFont>("Fonts/PoP");

            //energy...
            player_energy = content.Load<Texture2D>("Sprites/bottom/player_live_full");
            player_livePoints = content.Load<Texture2D>("Sprites/bottom/player_live_empty");
            enemy_energy = content.Load<Texture2D>("Sprites/bottom/enemy_live_full");
            enemy_livePoints = content.Load<Texture2D>("Sprites/bottom/enemy_live_empty");

            //Splash
            player_splash = content.Load<Texture2D>("Sprites/bottom/player_splash");
            enemy_splash = content.Load<Texture2D>("Sprites/bottom/enemy_splash");
            


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

            foreach (RoomNew r in maze.rooms)
            {
                r.Update(gameTime, keyboardState, gamePadState, touchState, accelerometerState, Microsoft.Xna.Framework.DisplayOrientation.Default);
            }
            maze.player.Update(gameTime, keyboardState, gamePadState, touchState, accelerometerState, Microsoft.Xna.Framework.DisplayOrientation.Default);

            //Other sprites update
            foreach (Sprite s in maze.player.SpriteRoom.SpritesInRoom())
            { 
               switch(s.GetType().Name)
                {
                    case "Guard" :
                    {
                        ((Guard)s).Update(gameTime, keyboardState, gamePadState, touchState, accelerometerState, Microsoft.Xna.Framework.DisplayOrientation.Default);
                        break;
                    }
                    default:
                        break;
                }
            }


            base.Update(gameTime, otherScreenHasFocus, false);
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            //// get all of our input states
            keyboardState = Keyboard.GetState();
            //gamePadState = GamePad.GetState(PlayerIndex.One);
            touchState = TouchPanel.GetState();
            accelerometerState = Accelerometer.GetState();

            if (maze.player.IsAlive == false)
            {
                if (keyboardState.IsKeyDown(Keys.Space) || gamePadState.IsButtonDown(Buttons.A) || touchState.AnyTouch() == true)
                {
                    maze.StartRoom().StartNewLife(ScreenManager.GraphicsDevice);
                }
            }

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
            
            //base.ScreenManager.SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);




            maze.player.SpriteRoom.Draw(gameTime, spriteBatch);
            maze.player.Draw(gameTime, spriteBatch);

            //now drow sprites
            maze.player.SpriteRoom.DrawSprites(gameTime, spriteBatch);


            //now drow the mask
            maze.player.SpriteRoom.DrawMask(gameTime, spriteBatch);


            DrawBottom();
            DrawHud();
            DrawDebug(maze.player.SpriteRoom);

            //spriteBatch.End();

            base.Draw(gameTime);

            base.ScreenManager.SpriteBatch.End();
        }

        private void DrawBottom()
        {
            Vector2 hudLocation = new Vector2(0, Game.CONFIG_SCREEN_HEIGHT - RoomNew.BOTTOM_BORDER);

            //DRAW BLACK SQUARE
            Rectangle r = new Rectangle(0,0, Game.CONFIG_SCREEN_WIDTH, Game.CONFIG_SCREEN_HEIGHT);
            Texture2D dummyTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            dummyTexture.SetData(new Color[] { Color.Black });
            //Texture2D tx = new Texture2D(spriteBatch.GraphicsDevice, Game.CONFIG_SCREEN_WIDTH, Game.CONFIG_SCREEN_HEIGHT);
            //Texture2D tx = livePoints;
            spriteBatch.Draw(dummyTexture, hudLocation, r, Color.White);
            

            //check if death
            hudLocation = new Vector2(Game.CONFIG_SCREEN_WIDTH / 3, Game.CONFIG_SCREEN_HEIGHT - RoomNew.BOTTOM_BORDER);
            if (maze.player.IsAlive == false)
            {
                DrawShadowedString(PoPFont, "Press Space to Continue", hudLocation, Color.White);
            }

            Rectangle source = new Rectangle(0, 0, player_livePoints.Width, player_livePoints.Height);

            int offset = 1;
            Texture2D player_triangle = player_livePoints;
            for (int x = 1; x <= maze.player.LivePoints; x++)
            {
                hudLocation = new Vector2(0 + offset, Game.CONFIG_SCREEN_HEIGHT - RoomNew.BOTTOM_BORDER);
                // Calculate the source rectangle of the current frame.
                

                if (x <= maze.player.Energy)
                    player_triangle = player_energy;
                else
                    player_triangle = player_livePoints;

                // Draw the current tile.
                spriteBatch.Draw(player_triangle, hudLocation, source, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
                offset += player_livePoints.Width + 1;
            }


            //Draw opponent energy
                offset = 1;
                foreach (Sprite s in maze.player.SpriteRoom.SpritesInRoom())
                {
                    switch (s.GetType().Name)
                    {
                        case "Guard":
                            {
                                offset = enemy_livePoints.Width +1;
                                Texture2D enemy_triangle = enemy_livePoints;
                                for (int x = 1; x <= maze.player.LivePoints; x++)
                                {
                                hudLocation = new Vector2(Game.CONFIG_SCREEN_WIDTH - offset, Game.CONFIG_SCREEN_HEIGHT - RoomNew.BOTTOM_BORDER);

                                if (x <= s.Energy)
                                    enemy_triangle = enemy_energy;
                                else
                                    enemy_triangle = enemy_livePoints;

                                // Draw the current tile.
                                spriteBatch.Draw(enemy_triangle, hudLocation, source, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
                                offset += enemy_livePoints.Width + 1;
                                }
                                break;
                            }
                        default:
                            break;
                    }
                
            }
            
            //DrawShadowedString(PoPFont, maze.PlayerRoom.roomName, hudLocation, Color.White);
        }

        private void DrawDebug(RoomNew room)
        {
            //if (room.player.sprite.sequence == null)
            //    return;

            Rectangle titleSafeArea = GraphicsDevice.Viewport.TitleSafeArea;
            Vector2 hudLocation = new Vector2(titleSafeArea.X, titleSafeArea.Y);


            //END OF DEVELOPMENT???????
            if (room.roomName == "MAP_dungeon_prison_9.xml")
            {
                string sMessage = string.Empty;
                sMessage = "Congratulations! Did you finish the first level of Prince of Persia.net!";
                DrawShadowedString(PoPFont, sMessage, hudLocation, Color.Yellow);
                hudLocation.Y = hudLocation.Y + 30;

                sMessage = "Today 7 May of 2013,";
                DrawShadowedString(PoPFont, sMessage, hudLocation, Color.White);
                hudLocation.Y = hudLocation.Y + 30;

                sMessage = "I ask for help to complete the development of this project!";
                DrawShadowedString(PoPFont, sMessage, hudLocation, Color.Red);
                hudLocation.Y = hudLocation.Y + 30;

                sMessage = "It was my intention to complete all levels and complete the";
                DrawShadowedString(PoPFont, sMessage, hudLocation, Color.White);
                hudLocation.Y = hudLocation.Y + 30;

                sMessage = "features that are missing, it would also be nice to bring this game on";
                DrawShadowedString(PoPFont, sMessage, hudLocation, Color.White);
                hudLocation.Y = hudLocation.Y + 30;

                sMessage = "other device like : Linux, Android and Apple iOS.";
                DrawShadowedString(PoPFont, sMessage, hudLocation, Color.White);
                hudLocation.Y = hudLocation.Y + 30;
                
                sMessage = "Unfortunately for complete Prince of Persia.net and porting to other device";
                DrawShadowedString(PoPFont, sMessage, hudLocation, Color.White);
                hudLocation.Y = hudLocation.Y + 30;

                sMessage = "I need your help to buy the kit developed by Xamarin ($ 999 business lic)";
                DrawShadowedString(PoPFont, sMessage, hudLocation, Color.White);
                hudLocation.Y = hudLocation.Y + 30;

                sMessage = "So if you liked my work to make a donation, see on my blog at:";
                DrawShadowedString(PoPFont, sMessage, hudLocation, Color.White);
                hudLocation.Y = hudLocation.Y + 30;

                sMessage = "http://princeofpersiadotnet.blogspot.it";
                DrawShadowedString(PoPFont, sMessage, hudLocation, Color.Yellow);
                hudLocation.Y = hudLocation.Y + 30;

                sMessage = "http://www.falappi.it";
                DrawShadowedString(PoPFont, sMessage, hudLocation, Color.Yellow);
                hudLocation.Y = hudLocation.Y + 30;

                
                return;
            }


            if (CONFIG_DEBUG == false)
                return;

        
            DrawShadowedString(hudFont, "ROMM NAME=" + maze.player.SpriteRoom.roomName, hudLocation, Color.White);
            hudLocation.Y = hudLocation.Y + 10;

            DrawShadowedString(hudFont, "POSTION X=" + maze.player.Position.X.ToString() + " Y=" + maze.player.Position.Y.ToString(), hudLocation, Color.White);
            hudLocation.Y = hudLocation.Y + 10;
            DrawShadowedString(hudFont, "FRAME RATE=" + AnimationSequence.frameRate.ToString(), hudLocation, Color.White);

            hudLocation.Y = hudLocation.Y + 10;
            DrawShadowedString(hudFont, "PLAYER STATE=" + maze.player.spriteState.Value().state + " SEQUENCE CountOffset=" + maze.player.sprite.sequence.CountOffSet, hudLocation, Color.White);

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
            DrawShadowedString(hudFont, "PrinceOfPersia.net beta version: " + RetrieveLinkerTimestamp().ToShortDateString(), hudLocation, Color.White);

        
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

