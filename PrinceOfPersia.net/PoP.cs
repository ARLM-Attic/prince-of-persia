	//-----------------------------------------------------------------------//
	// <copyright file="PrinceOfPersia.cs" company="A.D.F.Software">
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
using System.Collections.Generic;
using System.IO;
using System.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace PrinceOfPersia
{

    public class PoP : GameScreen
    {
        public static bool CONFIG_DEBUG = true;
        public static float CONFIG_FRAMERATE = 0.09f;
        public static string CONFIG_SPRITE_KID = "sprites/player/kid_dos/";
        public static string CONFIG_SPRITE_GUARD = "sprites/guard/guard_dos/";
        public static string CONFIG_SPRITE_EFFECTS = "sprites/effects/dos/";
        public static string CONFIG_SPRITE_BOTTOM = "sprites/bottom/";
        public static string CONFIG_SONGS = "songs/dos/";
        public static string CONFIG_SOUNDS = "sounds/dos/";
        public static string CONFIG_TILES = "tiles/dos/";
        public static string CONFIG_ITEMS = "items/";
        public static string CONFIG_FONTS = "fonts/";
        public static string CONFIG_PATH_APOPLEXY = "apoplexy/";
        public static string CONFIG_PATH_PON = "pon/";
        public static string CONFIG_PATH_LEVELS = "levels/";
        public static string CONFIG_PATH_ROOMS = "rooms/";
        public static string CONFIG_PATH_SEQUENCES = "sequences/dos";
        public static bool LEVEL_APOPLEXY = true;
        public static int CONFIG_KID_START_ENERGY = 3;
        public static string CONFIG_PATH_CONTENT = "Content/";
        public static bool CONFIG_FULL_SCREEN = false;
        public static int CONFIG_SCREEN_WIDTH = 640;
        public static int CONFIG_SCREEN_HEIGHT = 400;
        public static bool CONFIG_TOUCHSCREEN_VISIBLE = true;

        public static Matrix scaleMatrix = Matrix.Identity;

        private FontFile fontPrinceOfPersia_bigger;
        private FontFile fontPrinceOfPersia_small;
        private Texture2D fontTexturePrinceOfPersia_bigger;
        private Texture2D fontTexturePrinceOfPersia_small;

        //private SpriteFont PoPFont;

        private Level[] levels = new Level[30];
        private bool wasContinuePressed;
        private Maze maze;

        private Texture2D player_livePoints;
        private Texture2D player_energy;

        private Texture2D enemy_livePoints;
        private Texture2D enemy_energy;

        private Texture2D texControl = null;
        private Texture2D texControlOn = null;
        private Texture2D texControlButton = null;

        //private ContentManager content;

        // We store our input states so that we only poll once per frame, 
        // then we use the same input state wherever needed
        private GamePadState gamePadState;
        private KeyboardState keyboardState;
        private TouchCollection touchState;
        private AccelerometerState accelerometerState;

        /// <summary>
        /// Touch control zone
        /// </summary>
        public static Rectangle CntUpZone = new Rectangle(0,0,50,50);
        public static Rectangle CntRightZone = new Rectangle(0,0,50,50);
        public static Rectangle CntDownZone = new Rectangle(0,0,50,50);
        public static Rectangle CntLeftZone = new Rectangle(0,0,50,50);
        public static Rectangle CntCenterZone = new Rectangle(0,0,50,50);
        public static Rectangle CntShiftZone = new Rectangle(0,0,50,50);
        public static Rectangle CntUpLeftZone = new Rectangle(0,0,50,50);
        public static Rectangle CntUpRightZone = new Rectangle(0,0,50,50);
        public static Rectangle CntDownLeftZone = new Rectangle(0,0,50,50);
        public static Rectangle CntDownRightZone = new Rectangle(0,0,50,50);

        public SpriteBatch spriteBatch
        {
            get { return base.ScreenManager.SpriteBatch; }
        }

        public GraphicsDevice GraphicsDevice
        {
            get { return base.ScreenManager.GraphicsDevice; }
        }

        public void LoadConfiguration()
        {
            //READ APP.CONFIG for configuration settings
#if ANDROID
            CONFIG_DEBUG = true;
            CONFIG_FRAMERATE = 0.09f;
            CONFIG_PATH_CONTENT = System.AppDomain.CurrentDomain.BaseDirectory + "Content/";
            CONFIG_PATH_LEVELS = "levels/";
            CONFIG_PATH_ROOMS = "rooms/";
            CONFIG_PATH_SEQUENCES = "sequences/dos/";
            CONFIG_TOUCHSCREEN_VISIBLE = true;
#else
            bool.TryParse(ConfigurationSettings.AppSettings["CONFIG_debug"], out CONFIG_DEBUG);
            float.TryParse(ConfigurationSettings.AppSettings["CONFIG_framerate"], out CONFIG_FRAMERATE);

            //READ CONTENT RESOURCES PATH
            CONFIG_SPRITE_KID = ConfigurationSettings.AppSettings["CONFIG_sprite_kid"].ToString();
            CONFIG_SPRITE_GUARD = ConfigurationSettings.AppSettings["CONFIG_sprite_guard"].ToString();
            CONFIG_SOUNDS = ConfigurationSettings.AppSettings["CONFIG_sound"].ToString();
            CONFIG_SONGS = ConfigurationSettings.AppSettings["CONFIG_songs"].ToString();
            CONFIG_TILES = ConfigurationSettings.AppSettings["CONFIG_tiles"].ToString();
            CONFIG_ITEMS = ConfigurationSettings.AppSettings["CONFIG_items"].ToString();
            CONFIG_PATH_SEQUENCES = ConfigurationSettings.AppSettings["CONFIG_path_Sequences"].ToString();
            CONFIG_SPRITE_EFFECTS = ConfigurationSettings.AppSettings["CONFIG_sprite_effects"].ToString();
            CONFIG_KID_START_ENERGY = int.Parse(ConfigurationSettings.AppSettings["CONFIG_kid_start_energy"].ToString());
            LEVEL_APOPLEXY = bool.Parse(ConfigurationSettings.AppSettings["LEVEL_APOPLEXY"].ToString());
            CONFIG_PATH_APOPLEXY = ConfigurationSettings.AppSettings["CONFIG_path_Apoplexy"].ToString();
            CONFIG_PATH_PON = ConfigurationSettings.AppSettings["CONFIG_path_PoN"].ToString();
            CONFIG_PATH_LEVELS = ConfigurationSettings.AppSettings["CONFIG_path_Levels"].ToString();
            

            bool.TryParse(ConfigurationSettings.AppSettings["CONFIG_TOUCHSCREEN_VISIBLE"], out CONFIG_TOUCHSCREEN_VISIBLE);
#endif 
        
#if LINUX
        CONFIG_PATH_CONTENT = System.AppDomain.CurrentDomain.BaseDirectory + "Content/";
#endif

        }


        public PoP()
        {
             LoadConfiguration();

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
                //LOAD MAZE
                maze = new Maze(GraphicsDevice);
                
                //ASSIGN some texture from content
                enemy_energy = (Texture2D)Maze.Content[CONFIG_SPRITE_BOTTOM + "enemy_live_full"];
                enemy_livePoints = (Texture2D)Maze.Content[CONFIG_SPRITE_BOTTOM + "enemy_live_empty"];
                player_energy = (Texture2D)Maze.Content[CONFIG_SPRITE_BOTTOM + "player_live_full"];
                player_livePoints = (Texture2D)Maze.Content[CONFIG_SPRITE_BOTTOM + "player_live_empty"];

                //Load texture touch control
                texControl = (Texture2D)Maze.Content["backgrounds/touchcontrol_arrow"];
                texControlButton = (Texture2D)Maze.Content["backgrounds/touchcontrol_button"];


                fontPrinceOfPersia_bigger = (FontFile)Maze.Content[CONFIG_FONTS+ "princeofpersia_bigger"];
                fontTexturePrinceOfPersia_bigger = (Texture2D)Maze.Content[CONFIG_FONTS + "princeofpersia_bigger_0"];
                fontPrinceOfPersia_small = (FontFile)Maze.Content[CONFIG_FONTS + "princeofpersia_small"];
                fontTexturePrinceOfPersia_small= (Texture2D)Maze.Content[CONFIG_FONTS + "princeofpersia_small_0"];
                    
                //NOW START FROM 1'LEVEL
                maze.player = new Player(this.GraphicsDevice, maze.CurrentLevel.StartRoom());
                maze.player.MyRoom.StartNewLife();
                
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
        }

    

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            TouchPanel.EnabledGestures = GestureType.Tap;

            HandleInput(gameTime, null);

            foreach (Room r in maze.player.MyLevel.rooms)
            {
                r.Update(gameTime, keyboardState, gamePadState, touchState, accelerometerState, Microsoft.Xna.Framework.DisplayOrientation.Default);
            }
            maze.player.Update(gameTime, keyboardState, gamePadState, touchState, accelerometerState, Microsoft.Xna.Framework.DisplayOrientation.Default);


            //Other sprites update
            foreach (Sprite s in maze.player.MyRoom.SpritesInRoom())
            {
   
                switch (s.GetType().Name)
                {
                    case "Guard":
                        {
                            ((Guard)s).Update(gameTime, keyboardState, gamePadState, touchState, accelerometerState, Microsoft.Xna.Framework.DisplayOrientation.Default);
                            break;
                        }

                    case "Splash":
                        {
                            ((Splash)s).Update(gameTime, keyboardState, gamePadState, touchState, accelerometerState, Microsoft.Xna.Framework.DisplayOrientation.Default);
                            break;
                        }

                    default:
                        break;
                }
                //delete object in state == delete
                if (s.spriteState.Value().state == Enumeration.State.delete)
                {
                    maze.player.MyLevel.sprites.Remove(s);
                    //s.sprite.FrameIndex = 0;
                }
            }


            base.Update(gameTime, otherScreenHasFocus, false);
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            //// get all of our input states
            keyboardState = Keyboard.GetState();
            touchState = TouchPanel.GetState();
            accelerometerState = Accelerometer.GetState();

            if (maze.player.IsAlive == false)
            {
                if (keyboardState.IsKeyDown(Keys.Space) || gamePadState.IsButtonDown(Buttons.A) || touchState.AnyTouch() == true)
                {
                    maze.StartRoom().StartNewLife();
                }
            }

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
            //base.ScreenManager.SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, scaleMatrix);
            base.ScreenManager.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, scaleMatrix); 
            //base.ScreenManager.SpriteBatch.Begin();
            
            maze.player.MyRoom.Draw(gameTime, spriteBatch);
            maze.player.Draw(gameTime, spriteBatch);
            
            //now drow sprites
            maze.player.MyRoom.DrawSprites(gameTime, spriteBatch);

            //now drow the mask
            maze.player.MyRoom.DrawMask(gameTime, spriteBatch);

            DrawBottom();
            DrawHud();
            DrawDebug(maze.player.MyRoom);
            DrawTouchControl();

            base.Draw(gameTime);

            base.ScreenManager.SpriteBatch.End();
        }

        private void DrawBottom()
        {
            Vector2 hudLocation = new Vector2(0, PoP.CONFIG_SCREEN_HEIGHT - Room.BOTTOM_BORDER);

            //DRAW BLACK SQUARE
            Rectangle r = new Rectangle(0, 0, PoP.CONFIG_SCREEN_WIDTH, PoP.CONFIG_SCREEN_HEIGHT);
            Texture2D dummyTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            dummyTexture.SetData(new Color[] { Color.Black });
            spriteBatch.Draw(dummyTexture, hudLocation, r, Color.White);
            
            //check if death
            hudLocation = new Vector2(PoP.CONFIG_SCREEN_WIDTH / 3, PoP.CONFIG_SCREEN_HEIGHT - Room.BOTTOM_BORDER);
            if (maze.player.IsAlive == false)
            {
                DrawShadowedString(fontPrinceOfPersia_bigger, "Press Space to Continue", hudLocation, Color.White);
            }

            Rectangle source = new Rectangle(0, 0, player_livePoints.Width, player_livePoints.Height);

            int offset = 1;
            Texture2D player_triangle = player_livePoints;
            for (int x = 1; x <= maze.player.LivePoints; x++)
            {
                hudLocation = new Vector2(0 + offset, PoP.CONFIG_SCREEN_HEIGHT - Room.BOTTOM_BORDER);

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
                foreach (Sprite s in maze.player.MyRoom.SpritesInRoom())
                {
                    switch (s.GetType().Name)
                    {
                        case "Guard":
                            {
                                offset = enemy_livePoints.Width +1;

                                Texture2D enemy_triangle = enemy_livePoints;
                                for (int x = 1; x <= maze.player.LivePoints; x++)
                                {
                                    hudLocation = new Vector2(PoP.CONFIG_SCREEN_WIDTH - offset, PoP.CONFIG_SCREEN_HEIGHT - Room.BOTTOM_BORDER);

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

        private void DrawDebug(Room room)
        {
            if (CONFIG_DEBUG == false)
                return;

            Rectangle titleSafeArea = GraphicsDevice.Viewport.TitleSafeArea;
            Vector2 hudLocation = new Vector2(titleSafeArea.X, titleSafeArea.Y);

            DrawShadowedString(fontPrinceOfPersia_bigger, "LEVEL NAME=" + maze.player.MyLevel.levelName, hudLocation, Color.White);
            hudLocation.Y = hudLocation.Y + 10;
        
            DrawShadowedString(fontPrinceOfPersia_bigger, "ROOM NAME=" + maze.player.MyRoom.roomName, hudLocation, Color.White);
            hudLocation.Y = hudLocation.Y + 10;

            DrawShadowedString(fontPrinceOfPersia_bigger, "POSTION X=" + maze.player.Position.X.ToString() + " Y=" + maze.player.Position.Y.ToString(), hudLocation, Color.White);
            hudLocation.Y = hudLocation.Y + 10;
            DrawShadowedString(fontPrinceOfPersia_bigger, "FRAME RATE=" + AnimationSequence.frameRate.ToString(), hudLocation, Color.White);

            hudLocation.Y = hudLocation.Y + 10;
            DrawShadowedString(fontPrinceOfPersia_bigger, "PLAYER STATE=" + maze.player.spriteState.Value().state + " SEQUENCE CountOffset=" + maze.player.sprite.sequence.CountOffSet, hudLocation, Color.White);

            hudLocation.Y = hudLocation.Y + 10;
            DrawShadowedString(fontPrinceOfPersia_bigger, "PLAYER FRAME=" + maze.player.spriteState.Value().Frame + " NAME=" + maze.player.spriteState.Value().Name , hudLocation, Color.White);

            hudLocation.Y = hudLocation.Y + 10;
            DrawShadowedString(fontPrinceOfPersia_bigger, "PLAYER SWORD=" + maze.player.Sword.ToString(), hudLocation, Color.White);

            TouchCollection touchState = TouchPanel.GetState();

            for (int x = 0; x < touchState.Count; x++)
            {
                hudLocation.Y = hudLocation.Y + 10;
                DrawShadowedString(fontPrinceOfPersia_bigger, "TOUCH_STATE=" + touchState[x].State.ToString(), hudLocation, Color.White);
                hudLocation.Y = hudLocation.Y + 10;
                DrawShadowedString(fontPrinceOfPersia_bigger, "TOUCH_LOCATION=" + touchState[x].Position.ToString(), hudLocation, Color.White);

            }

            hudLocation.Y = hudLocation.Y + 10;
            DrawShadowedString(fontPrinceOfPersia_bigger, "-SHIFTZONE=" + CntShiftZone.ToString(), hudLocation, Color.White);

            


            // Get the player's bounding rectangle and find neighboring tiles.
            Rectangle playerBounds = maze.player.Position.Bounding;
            Vector4 v4 = room.getBoundTiles(playerBounds);

            // For each potentially colliding Tile, warning the for check only the player row ground..W
            for (int y = (int)v4.Z; y <= (int)v4.W; ++y)
            {
                for (int x = (int)v4.X; x <= (int)v4.Y; ++x)
                {
                    //Rectangle tileBounds = room.GetBounds(x, y);
                    Tile myTile = room.GetTile(x, y);
                    Vector2 depth = RectangleExtensions.GetIntersectionDepth(playerBounds, myTile.Position.Bounding);
                    Enumeration.TileCollision tileCollision = room.GetCollision(x, y);
                    Enumeration.TileType tileType = room.GetType(x, y);

                    hudLocation.Y = hudLocation.Y + 10;
                    DrawShadowedString(fontPrinceOfPersia_bigger, "GRID X=" + x + " Y=" + y + " TILETYPE=" + tileType.ToString() + " BOUND X=" + myTile.Position.Bounding.X + " Y=" + myTile.Position.Bounding.Y + " DEPTH X=" + depth.X + " Y=" + depth.Y, hudLocation, Color.White);
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
            DrawShadowedString(fontPrinceOfPersia_bigger, "Compiled :" + RetrieveLinkerTimestamp().ToShortDateString(), hudLocation, Color.White);

        
        }

        private void DrawTouchControl()
        {
            if (PoP.CONFIG_TOUCHSCREEN_VISIBLE == false)
                return;

            Color colorLeft = Color.White;
            Color colorRight = Color.White;
            Color colorUp = Color.White;
            Color colorDown = Color.White;
            Color colorDownLeft = Color.White;
            Color colorDownRight = Color.White;
            Color colorUpLeft = Color.White;
            Color colorUpRight= Color.White;
            Color colorShift= Color.White;



            CntDownLeftZone = new Rectangle(texControl.Width * 0, PoP.CONFIG_SCREEN_HEIGHT - (texControl.Height * 1), texControl.Width, texControl.Height);
            CntDownZone = new Rectangle(texControl.Width * 1, PoP.CONFIG_SCREEN_HEIGHT - (texControl.Height * 1), texControl.Width, texControl.Height);
            CntDownRightZone = new Rectangle(texControl.Width * 2, PoP.CONFIG_SCREEN_HEIGHT - (texControl.Height * 1), texControl.Width, texControl.Height);

            CntLeftZone = new Rectangle(texControl.Width * 0, PoP.CONFIG_SCREEN_HEIGHT - (texControl.Height * 2), texControl.Width, texControl.Height);
            CntCenterZone = new Rectangle(texControl.Width * 1, PoP.CONFIG_SCREEN_HEIGHT - (texControl.Height * 2), texControl.Width, texControl.Height);
            CntRightZone = new Rectangle(texControl.Width * 2, PoP.CONFIG_SCREEN_HEIGHT - (texControl.Height * 2), texControl.Width, texControl.Height);

            CntUpLeftZone = new Rectangle(texControl.Width * 0, PoP.CONFIG_SCREEN_HEIGHT - (texControl.Height * 3), texControl.Width, texControl.Height);
            CntUpZone = new Rectangle(texControl.Width * 1, PoP.CONFIG_SCREEN_HEIGHT - (texControl.Height * 3), texControl.Width, texControl.Height);
            CntUpRightZone = new Rectangle(texControl.Width * 2, PoP.CONFIG_SCREEN_HEIGHT - (texControl.Height * 3), texControl.Width, texControl.Height);

            CntShiftZone = new Rectangle(PoP.CONFIG_SCREEN_WIDTH - (texControlButton.Width * 2), PoP.CONFIG_SCREEN_HEIGHT - (texControlButton.Height * 2), texControl.Width, texControlButton.Height);

            //Matrix resolution
            CntDownLeftZone.X = (int)(CntDownLeftZone.X * scaleMatrix.M11); CntDownLeftZone.Y = (int)(CntDownLeftZone.Y * scaleMatrix.M11);
            CntDownZone.X = (int)(CntDownZone.X * scaleMatrix.M11); CntDownZone.Y = (int)(CntDownZone.Y * scaleMatrix.M11);
            CntDownRightZone.X = (int)(CntDownRightZone.X * scaleMatrix.M11); CntDownRightZone.Y = (int)(CntDownRightZone.Y * scaleMatrix.M11);

            CntLeftZone.X = (int)(CntLeftZone.X * scaleMatrix.M11); CntLeftZone.Y = (int)(CntLeftZone.Y * scaleMatrix.M11);
            CntCenterZone.X = (int)(CntCenterZone.X * scaleMatrix.M11); CntCenterZone.Y = (int)(CntCenterZone.Y * scaleMatrix.M11);
            CntRightZone.X = (int)(CntRightZone.X * scaleMatrix.M11); CntRightZone.Y = (int)(CntRightZone.Y * scaleMatrix.M11);

            CntUpLeftZone.X = (int)(CntUpLeftZone.X * scaleMatrix.M11); CntUpLeftZone.Y = (int)(CntUpLeftZone.Y * scaleMatrix.M11);
            CntUpZone.X = (int)(CntUpZone.X * scaleMatrix.M11); CntUpZone.Y = (int)(CntUpZone.Y * scaleMatrix.M11);
            CntUpRightZone.X = (int)(CntUpRightZone.X * scaleMatrix.M11); CntUpRightZone.Y = (int)(CntUpRightZone.Y * scaleMatrix.M11);

            CntShiftZone.X = (int)(CntShiftZone.X * scaleMatrix.M11); CntShiftZone.Y = (int)(CntShiftZone.Y * scaleMatrix.M11);

            
            if (maze.player.Input == Enumeration.Input.leftdown)
                colorDownLeft = Color.Yellow;
            if (maze.player.Input == Enumeration.Input.down)
                colorDown = Color.Yellow;
            if (maze.player.Input == Enumeration.Input.righdown)
                colorDownRight = Color.Yellow;
            if (maze.player.Input == Enumeration.Input.left)
                colorLeft = Color.Yellow;
            if (maze.player.Input == Enumeration.Input.right)
                colorRight = Color.Yellow;
            if (maze.player.Input == Enumeration.Input.leftup)
                colorUpLeft = Color.Yellow;
            if (maze.player.Input == Enumeration.Input.up)
                colorUp = Color.Yellow;
            if (maze.player.Input == Enumeration.Input.rightup)
                colorUpRight = Color.Yellow;
            if (maze.player.Input == Enumeration.Input.shift)
                colorShift = Color.Yellow;

            if (maze.player.Input == Enumeration.Input.leftshift)
            {
                colorLeft = Color.Yellow;
                colorShift = Color.Yellow;
            }
            if (maze.player.Input == Enumeration.Input.rightshift)
            {
                colorRight = Color.Yellow;
                colorShift = Color.Yellow; 
            }
            if (maze.player.Input == Enumeration.Input.downshift)
            {
                colorDown = Color.Yellow;
                colorShift = Color.Yellow;
            } if (maze.player.Input == Enumeration.Input.upshift)
            {
                colorUp = Color.Yellow;
                colorShift = Color.Yellow;
            }



            spriteBatch.Draw(texControl, new Vector2((CntDownLeftZone.X / scaleMatrix.M11) + 20, (CntDownLeftZone.Y / scaleMatrix.M11) - 10), null, colorDownLeft, (float)(Math.PI / 180) * 225, new Vector2(45, 45), 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.Draw(texControl, new Vector2(CntDownZone.X / scaleMatrix.M11, CntDownZone.Y / scaleMatrix.M11), null, colorDown, (float)Math.PI, new Vector2(45, 45), 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.Draw(texControl, new Vector2(CntDownRightZone.X / scaleMatrix.M11 - 10, CntDownZone.Y / scaleMatrix.M11 + 20), null, colorDownRight, (float)(Math.PI / 180) * 135, new Vector2(45, 45), 1.0f, SpriteEffects.None, 0.0f);

            spriteBatch.Draw(texControl, new Vector2(CntLeftZone.X / scaleMatrix.M11 + 45, CntLeftZone.Y / scaleMatrix.M11), null, colorLeft, (float)(Math.PI / 180) * 270, new Vector2(45, 45), 1.0f, SpriteEffects.None, 0.0f);
            //spriteBatch.Draw(texControl, CntCenterZone, color);
            spriteBatch.Draw(texControl, new Vector2(CntRightZone.X / scaleMatrix.M11, CntRightZone.Y / scaleMatrix.M11 + 45), null, colorRight, (float)(Math.PI / 180) * 90, new Vector2(45, 45), 1.0f, SpriteEffects.None, 0.0f);

            spriteBatch.Draw(texControl, new Vector2(CntUpLeftZone.X / scaleMatrix.M11 + 55, CntUpLeftZone.Y / scaleMatrix.M11 + 20), null, colorUpLeft, (float)(Math.PI / 180) * 315, new Vector2(45, 45), 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.Draw(texControl, new Vector2(CntUpZone.X / scaleMatrix.M11 + 45, CntUpZone.Y / scaleMatrix.M11 + 45), null, colorUp, (float)(Math.PI / 180), new Vector2(45, 45), 1.0f, SpriteEffects.None, 0.0f);
            spriteBatch.Draw(texControl, new Vector2(CntUpRightZone.X / scaleMatrix.M11 + 20, CntUpRightZone.Y / scaleMatrix.M11 + 55), null, colorUpRight, (float)(Math.PI / 180) * 45, new Vector2(45, 45), 1.0f, SpriteEffects.None, 0.0f);

            spriteBatch.Draw(texControlButton, new Vector2(CntShiftZone.X / scaleMatrix.M11, CntShiftZone.Y / scaleMatrix.M11), colorShift);

            if (CONFIG_DEBUG == true)
            {
                Rectangle rect2 = new Rectangle((int)(Player.touchPositionRect.X / scaleMatrix.M11), (int)(Player.touchPositionRect.Y / scaleMatrix.M11), Player.touchPositionRect.Width, Player.touchPositionRect.Height);
                spriteBatch.Draw(texControlButton, rect2, Color.Red);

            }

        }

        


        private void DrawShadowedString(FontFile font, string value, Vector2 position, Color color)
        {
            FontRenderer fontRender = new FontRenderer(fontPrinceOfPersia_small, fontTexturePrinceOfPersia_small);

            fontRender.DrawString(spriteBatch, position, value);

            //fontRender.DrawString(font, value, position + new Vector2(1.0f, 1.0f), Color.Black);
            //spriteBatch.DrawString(font, value, position, color);
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

