	//-----------------------------------------------------------------------//
	// <copyright file="Player.cs" company="A.D.F.Software">
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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input.Touch;
using System.IO;



namespace PrinceOfPersia
{

    public class Player : Sprite
    {
        public static Rectangle touchPositionRect;

        private Enumeration.Input input;
        public Enumeration.Input Input
        {
            get { return input; }
        }

        /// <summary>
        /// Constructors a new player.
        /// </summary>
        public Player(GraphicsDevice GraphicsDevice, Room room)
        {
            graphicsDevice = GraphicsDevice;
            StartLevel(room);
        }


        public void StartLevel(Room room)
        {
            myRoom = room;
            LoadContent();
            Reset(myRoom.roomStartPosition, myRoom.roomStartDirection);
        }



        /// <summary>
        /// Loads the player sprite sheet and sounds.
        /// </summary>
        /// <note>i will add a parameter read form app.config</note>
        /// 
        /// 
        private void LoadContent()
        {

            spriteSequence = new List<Sequence>();
            System.Xml.Serialization.XmlSerializer ax = new System.Xml.Serialization.XmlSerializer(spriteSequence.GetType());

            Stream txtReader = Microsoft.Xna.Framework.TitleContainer.OpenStream(PoP.CONFIG_PATH_CONTENT + PoP.CONFIG_PATH_SEQUENCES + "kid_sequence.xml");

            spriteSequence = (List<Sequence>)ax.Deserialize(txtReader);

            foreach (Sequence s in spriteSequence)
            {
                s.Initialize();
            }

            // Calculate bounds within texture size.         
            //faccio un rettangolo che sia largo la metà del frame e che parta dal centro
            int top = 0; //StandAnimation.FrameHeight - height - 128;
            int left = 0; //PLAYER_L_PENETRATION; //THE LEFT BORDER!!!! 19
            int width = 114;//(int)(StandAnimation.FrameWidth);  //lo divido per trovare punto centrale *2)
            int height = 114;//(int)(StandAnimation.FrameHeight);


            localBounds = new Rectangle(left, top, width, height);

        }


        



        /// <summary>
        /// Handles input, performs physics, and animates the player sprite.
        /// </summary>
        /// <remarks>
        /// We pass in all of the input states so that our game is only polling the hardware
        /// once per frame. We also pass the game's orientation because when using the accelerometer,
        /// we need to reverse our motion when the orientation is in the LandscapeRight orientation.
        /// </remarks>
        public void Update(
            GameTime gameTime,
            KeyboardState keyboardState,
            GamePadState gamePadState,
            TouchCollection touchState,
            AccelerometerState accelState,
            DisplayOrientation orientation)
        {
            input = GetInput(keyboardState, gamePadState, touchState, accelState, orientation);
            ParseInput(input);

            if (IsAlive == false & isOnGround == true)
            {
                //DeadFall();
                //return;
            }

            //ApplyPhysicsNew(gameTime);
            HandleCollisions();


            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            sprite.UpdateFrame(elapsed, ref _position, ref face, ref spriteState);
        }



        public void ParseInput(Enumeration.Input input)
        {
            //if (spriteState.Value().state == Enumeration.State.question)
            //    Question();


            if (spriteState.Value().Priority == Enumeration.PriorityState.Normal & sprite.IsStoppable == false)
                return;

            switch (input)
            {
                case Enumeration.Input.none:
                    switch (spriteState.Value().state)
                    {
                        case Enumeration.State.none:
                            Stand(spriteState.Value().Priority);
                            break;
                        case Enumeration.State.stand:
                            Stand(spriteState.Value().Priority);
                            break;
                        case Enumeration.State.startrun:
                            RunStop();
                            break;
                        case Enumeration.State.running:
                            RunStop();
                            break;
                        case Enumeration.State.step1:
                            Stand();
                            break;
                        case Enumeration.State.stepfall:
                            StepFall(spriteState.Value().Priority, spriteState.Value().OffSet);
                            break;
                        case Enumeration.State.crouch:
                            StandUp();
                            break;
                        case Enumeration.State.highjump:
                            Stand();
                            break;
                        case Enumeration.State.hangstraight:
                        case Enumeration.State.hang:
                            HangDrop();
                            break;
                        case Enumeration.State.bump:
                            Bump(spriteState.Value().Priority);
                            break;

                        default:
                            break;
                    }
                    break;
                //LEFT//////////////////////
                case Enumeration.Input.left:
                    switch (spriteState.Value().state)
                    {
                        case Enumeration.State.stand:
                            if (face == SpriteEffects.FlipHorizontally)
                                Turn();
                            else
                                StartRunning();
                            break;
                        case Enumeration.State.step1:
                            StartRunning();
                            break;
                        case Enumeration.State.crouch:
                            if (face == SpriteEffects.FlipHorizontally)
                                return;
                            Crawl();
                            break;
                        case Enumeration.State.stepfall:
                            StepFall();
                            break;
                        case Enumeration.State.startrun:
                            if (face == SpriteEffects.FlipHorizontally)
                                RunTurn();
                            break;
                        //case Enumeration.State.hang:
                        //    HangDrop();
                        //    break;
                        case Enumeration.State.bump:
                            Bump(spriteState.Value().Priority);
                            break;
                        case Enumeration.State.ready:
                            if (face == SpriteEffects.FlipHorizontally)
                                Retreat();
                            else
                                Advance();
                            break;
                        
                        default:
                            break;
                    }
                    break;
                //SHIFTLEFT//////////////////////
                case Enumeration.Input.leftshift:
                    switch (spriteState.Value().state)
                    {
                        case Enumeration.State.stand:
                            if (face == SpriteEffects.FlipHorizontally)
                                Turn();
                            else
                                StepForward();
                            break;
                        case Enumeration.State.hangstraight:
                        case Enumeration.State.hang:
                            Hang();
                            break;
                        case Enumeration.State.bump:
                            Bump(spriteState.Value().Priority);
                            break;
                        case Enumeration.State.ready:
                            Strike(spriteState.Value().Priority);
                            break;


                        default:
                            break;
                    }
                    break;
                //LEFTDOWN//////////////////////
                case Enumeration.Input.leftdown:
                    switch (spriteState.Value().state)
                    {
                        case Enumeration.State.crouch:
                            if (face == SpriteEffects.None)
                                Crawl();
                            break;
                        case Enumeration.State.startrun:
                            Stoop(new Vector2(5, 0));
                            break;
                        default:
                            break;
                    }
                    break;
                //LEFTUP//////////////////////
                case Enumeration.Input.leftup:
                    switch (spriteState.Value().state)
                    {
                        case Enumeration.State.stand:
                            StandJump();
                            break;
                        case Enumeration.State.startrun:
                            RunJump();
                            break;
                        case Enumeration.State.stepfall:
                            StepFall();
                            break;
                        default:
                            break;
                    }
                    break;

                //RIGHT//////////////////////
                case Enumeration.Input.right:
                    switch (spriteState.Value().state)
                    {
                        case Enumeration.State.stand:
                            if (face == SpriteEffects.None)
                                Turn();
                            else
                                StartRunning();
                            break;
                        case Enumeration.State.step1:
                            StartRunning();
                            break;
                        case Enumeration.State.crouch:
                            if (face == SpriteEffects.None)
                                return;
                            Crawl();
                            break;
                        case Enumeration.State.stepfall:
                            StepFall();
                            break;
                        case Enumeration.State.startrun:
                            if (face == SpriteEffects.None)
                                RunTurn();
                            break;
                        case Enumeration.State.ready:
                            if (face == SpriteEffects.FlipHorizontally)
                                Advance();
                            else
                                Retreat();
                            break;

                        default:
                            break;
                    }
                    break;
                //SHIFTRIGHT//////////////////////
                case Enumeration.Input.rightshift:
                    switch (spriteState.Value().state)
                    {
                        case Enumeration.State.stand:
                            if (face == SpriteEffects.None)
                                Turn();
                            else
                                StepForward();
                            break;
                        case Enumeration.State.hang:
                            Hang();
                            break;
                        case Enumeration.State.ready:
                            Strike(spriteState.Value().Priority);
                            break;

                        default:
                            break;
                    }
                    break;
                //RIGHTDOWN//////////////////////
                case Enumeration.Input.righdown:
                    switch (spriteState.Value().state)
                    {
                        case Enumeration.State.crouch:
                            if (face != SpriteEffects.None)
                                Crawl();
                            break;
                        case Enumeration.State.hang:
                            ClimbFail();
                            break;
                        case Enumeration.State.startrun:
                            Stoop(new Vector2(5, 0));
                            break;
                        default:
                            break;
                    }
                    break;
                //RIGHTUP//////////////////////
                case Enumeration.Input.rightup:
                    switch (spriteState.Value().state)
                    {
                        case Enumeration.State.stand:
                            StandJump();
                            break;
                        case Enumeration.State.startrun:
                            RunJump();
                            break;
                        case Enumeration.State.stepfall:
                            StepFall();
                            break;
                        default:
                            break;
                    }
                    break;

                //DOWN//////////////////////
                case Enumeration.Input.down:
                case Enumeration.Input.downshift:
                    switch (spriteState.Value().state)
                    {
                        case Enumeration.State.stand:
                            Stoop();
                            break;
                        case Enumeration.State.hang:
                        case Enumeration.State.hangstraight:
                            HangDrop();
                            break;
                        case Enumeration.State.startrun:
                            Stoop(new Vector2(5, 0));
                            break;
                        case Enumeration.State.ready:
                            if (Sword == true)
                                PutOffSword();
                            break;
                        default:
                            break;
                    }
                    break;
                //UP//////////////////////
                case Enumeration.Input.up:
                case Enumeration.Input.upshift:
                    switch (spriteState.Value().state)
                    {
                        case Enumeration.State.running:
                        case Enumeration.State.startrun:
                            RunStop();
                            break;
                        case Enumeration.State.stand:
                            HighJump();
                            break;
                        case Enumeration.State.jumphangLong:
                        case Enumeration.State.jumphangMed:
                        case Enumeration.State.hang:
                        case Enumeration.State.hangstraight:
                            ClimbUp();
                            if (input == Enumeration.Input.upshift)
                                Hang();
                            break;

                        default:
                            break;
                    }
                    break;
                //SHIFT/////////////////////////
                case Enumeration.Input.shift:
                    switch (spriteState.Value().state)
                    {
                        //case Enumeration.State.hang:
                        //    Hang();
                        //    break;
                        case Enumeration.State.jumphangLong:
                        case Enumeration.State.jumphangMed:
                            Hang();
                            break;
                        case Enumeration.State.ready:
                            Strike(spriteState.Value().Priority);
                            break;
                        default:
                            CheckItemOnFloor();
                            break;
                    }
                    break;

                default:
                    break;
            }


        }
        private Enumeration.Input GetKeyboardInput(KeyboardState keyboardState)
        {
            Enumeration.Input input = Enumeration.Input.none;
            
            if (keyboardState.GetPressedKeys().Count() == 0)
                return input;

            Keys? shiftKey = null;

            //////////
            //SHIFT SUBST FOR LEFT OR RIGHT SHIFT
            //////////
            for (int x = 0; x < keyboardState.GetPressedKeys().Count(); x++)
            {
                if (keyboardState.GetPressedKeys()[x] == Keys.LeftShift)
                { 
                    shiftKey = Keys.LeftShift;
                    continue;
                }
                if (keyboardState.GetPressedKeys()[x] == Keys.RightShift)
                    shiftKey = Keys.LeftShift;
            }


            if (keyboardState.GetPressedKeys().Count() == 1 & shiftKey == Keys.LeftShift)
            {
                return Enumeration.Input.shift;
            }

            //////////
            //LEFT 
            //////////
            if (keyboardState.IsKeyDown(Keys.Up) & keyboardState.IsKeyDown(Keys.Left))
            {
                return Enumeration.Input.leftup;
            }

            if (keyboardState.IsKeyDown(Keys.Down) & keyboardState.IsKeyDown(Keys.Left))
            {
                return Enumeration.Input.leftdown;
            }
            
            if (keyboardState.IsKeyDown(Keys.Left) & (shiftKey == Keys.LeftShift))
            {
                return Enumeration.Input.leftshift;
            }
            
            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            {
                return Enumeration.Input.left;
            }

            //////////
            //RIGHT 
            //////////
            if (keyboardState.IsKeyDown(Keys.Up) & keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.E))
            {
                return Enumeration.Input.rightup;
            }

            if (keyboardState.IsKeyDown(Keys.Down) & keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.C))
            {
                return Enumeration.Input.righdown;
            }
            
            if (keyboardState.IsKeyDown(Keys.Right) & shiftKey == Keys.LeftShift)
            {
                return Enumeration.Input.rightshift;
            }

            if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            {
                return Enumeration.Input.right;
            }
            //////
            //UP//
            //////
            
            if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
            {
                return Enumeration.Input.up;
            }
            ///////
            //DOWN/
            ///////
            if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
            {
                return Enumeration.Input.down;
            }

            ////////
            //NONE//
            return Enumeration.Input.none;


        
        }

        
        private Enumeration.Input GetTouchInput(TouchCollection touchState)
        {
            Enumeration.Input lastInput = Enumeration.Input.none;
            bool shiftInput = false;
            int pixelToX = 0;
            int pixelToY = 0;
#if ANDROID 
            pixelToX = 50;
            pixelToY = 50;
#elif WINDOWS
            pixelToX = 50;
#endif

            Vector2 touchDepth = Vector2.Zero;

            if (touchState.Count() == 0)
            {
                touchPositionRect = Rectangle.Empty;
                return lastInput;
            }

            for (int x = 0; x < touchState.Count; x++)
            {
                touchPositionRect = new Rectangle((int)touchState[x].Position.X - pixelToX, (int)touchState[x].Position.Y - pixelToY, 60, 60);

                touchDepth = PoP.CntShiftZone.GetIntersectionDepth(touchPositionRect);
                if (touchDepth.X > 0 | touchDepth.Y > 0)
                { 
                    shiftInput = true;
                    lastInput = Enumeration.Input.shift;
                    continue;
                }

                touchDepth = PoP.CntUpZone.GetIntersectionDepth(touchPositionRect);
                if (touchDepth.X > 0 | touchDepth.Y > 0)
                {
                    lastInput = Enumeration.Input.up;
                    continue;
                }
                touchDepth = PoP.CntUpLeftZone.GetIntersectionDepth(touchPositionRect);
                if (touchDepth.X > 0 | touchDepth.Y > 0)
                {
                    lastInput = Enumeration.Input.leftup;
                    continue;
                }
                touchDepth = PoP.CntUpRightZone.GetIntersectionDepth(touchPositionRect);
                if (touchDepth.X > 0 | touchDepth.Y > 0)
                {
                    lastInput = Enumeration.Input.rightup;
                    continue;
                }
                touchDepth = PoP.CntLeftZone.GetIntersectionDepth(touchPositionRect);
                if (touchDepth.X > 0 | touchDepth.Y > 0)
                {
                    lastInput = Enumeration.Input.left;
                    continue;
                }
                touchDepth = PoP.CntLeftZone.GetIntersectionDepth(touchPositionRect);
                if (touchDepth.X > 0 | touchDepth.Y > 0)
                if (PoP.CntCenterZone.Contains((int)touchState[x].Position.X, (int)touchState[x].Position.Y))
                {
                    //lastInput = Enumeration.Input.none;
                    continue;
                }
                touchDepth = PoP.CntRightZone.GetIntersectionDepth(touchPositionRect);
                if (touchDepth.X > 0 | touchDepth.Y > 0)
                {
                    lastInput = Enumeration.Input.right;
                    continue;
                }
                touchDepth = PoP.CntDownLeftZone.GetIntersectionDepth(touchPositionRect);
                if (touchDepth.X > 0 | touchDepth.Y > 0)
                {
                    lastInput = Enumeration.Input.leftdown;
                    continue;
                }
                touchDepth = PoP.CntDownZone.GetIntersectionDepth(touchPositionRect);
                if (touchDepth.X > 0 | touchDepth.Y > 0)
                {
                    lastInput = Enumeration.Input.down;
                    continue;
                }
                touchDepth = PoP.CntDownRightZone.GetIntersectionDepth(touchPositionRect);
                if (touchDepth.X > 0 | touchDepth.Y > 0)
                {
                    lastInput = Enumeration.Input.righdown;
                    continue;
                }
                
            }

            if (shiftInput == false)
            {
                return lastInput;
            }

            switch (lastInput)
            {
                case Enumeration.Input.left: return Enumeration.Input.leftshift;
                case Enumeration.Input.right: return Enumeration.Input.rightshift;
                case Enumeration.Input.down: return Enumeration.Input.downshift;
                case Enumeration.Input.up: return Enumeration.Input.upshift;
                default:
                    return lastInput;
            }

            
        }


        /// <summary>
        /// Gets player horizontal movement and jump commands from input.
        /// </summary>
        private Enumeration.Input GetInput 
            (
            KeyboardState keyboardState,
            GamePadState gamePadState,
            TouchCollection touchState,
            AccelerometerState accelState,
            DisplayOrientation orientation
            )
        {
            if (spriteState.Value().Priority == Enumeration.PriorityState.Force)
                return Enumeration.Input.none;

            ////////
            // CHEAT DEBUG
            /////////
            if (PoP.CONFIG_DEBUG == true)
            {
                if (keyboardState.IsKeyDown(Keys.NumPad8))
                {
                    myRoom = myRoom.Up;
                    return Enumeration.Input.none;
                }
                if (keyboardState.IsKeyDown(Keys.NumPad2))
                {
                    myRoom = myRoom.Down;
                    return Enumeration.Input.none;
                }
                if (keyboardState.IsKeyDown(Keys.NumPad4))
                {
                    myRoom = myRoom.Left;
                    return Enumeration.Input.none;
                }
                if (keyboardState.IsKeyDown(Keys.NumPad6))
                {
                    myRoom = myRoom.Right;
                    return Enumeration.Input.none;
                }
                if (keyboardState.IsKeyDown(Keys.NumPad0))
                {
                    Maze.StartRoom().StartNewLife();
                    return Enumeration.Input.none;
                }
                if (keyboardState.IsKeyDown(Keys.OemMinus))
                {
                    AnimationSequence.frameRate = AnimationSequence.frameRate - 0.1f;
                    return Enumeration.Input.none;
                }
                if (keyboardState.IsKeyDown(Keys.OemPlus))
                {
                    AnimationSequence.frameRate = AnimationSequence.frameRate + 0.1f;
                    return Enumeration.Input.none;
                }
                if (keyboardState.IsKeyDown(Keys.NumPad5))
                {
                    //Maze.player.Resheathe();
                    Maze.player.Sword = true;
                    Maze.player.LivePoints = 15;
                    Maze.player.Energy = 15;
                    return Enumeration.Input.none;
                }
                if (keyboardState.IsKeyDown(Keys.Add))
                {
                    Maze.NextLevel();
                }
                if (keyboardState.IsKeyDown(Keys.Subtract))
                {
                    Maze.PreviousLevel();
                }
            }

            //////////
            //TouchControl
            //////////
            if (touchState.Count != 0)
                return GetTouchInput(touchState);

            return GetKeyboardInput(keyboardState);
        }

        /// <summary>
        /// Draws the animated player.
        /// </summary>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //DRAW SPRITE
            sprite.DrawSprite(gameTime, spriteBatch, _position, face, 0.5f);
        }




    }
}
