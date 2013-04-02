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

        //Sequence static for share purpose
        //private static List<Sequence> spriteSequence = null;
        
        //private GraphicsDevice graphicsDevice;
        //private SpriteEffects flip = SpriteEffects.None;
        //private Position _position;
        //private Maze Maze;

        //public AnimationSequence sprite;

        // Sounds

        //Player Grid
        //public const int PLAYER_GRID = 1;
        //public const int PLAYER_VELOCITY = 120;
        //public const int PLAYER_L_PENETRATION = 19;
        //public const int PLAYER_R_PENETRATION = 30;

        //public const int PLAYER_STAND_BORDER_FRAME = 47; //47+20player+47=114
        //public const int PLAYER_STAND_FRAME = 20;
        //public const int PLAYER_STAND_WALL_PEN = 30; //wall penetration
        //public const int PLAYER_STAND_FLOOR_PEN = 26; //floor border penetration
        //public const int PLAYER_STAND_HANG_PEN = 46; //floor border penetration for hangup

        ////public const int SPRITE_SIZE_X = 114; //to be var
        ////public const int SPRITE_SIZE_Y = 114; //to be var

        //public spriteState spriteState = new spriteState();





        // Constants for controling horizontal movement
        //private const float MoveAcceleration = 13000.0f;
        //private const float MaxMoveSpeed = 1750.0f;
        //private const float GroundDragFactor = 0.48f;
        //private const float AirDragFactor = 0.58f;

        // Constants for controlling vertical movement
        //private const float MaxJumpTime = 0.35f;
        //private const float JumpLaunchVelocity = -3500.0f;
        //private const float GravityAcceleration = 3400.0f;
        //private const float MaxFallSpeed = 100f;//550.0f;
        //private const float JumpControlPower = 0.14f;

        // Input configuration
        //private const float MoveStickScale = 1.0f;
        //private const float AccelerometerScale = 1.5f;
        //private const Buttons JumpButton = Buttons.A;

        /// <summary>
        /// Gets whether or not the player's feet are on the ground.
        /// </summary>
     

        /// <summary>
        /// Constructors a new player.
        /// </summary>
        public Player(RoomNew room, Vector2 position, GraphicsDevice GraphicsDevice, SpriteEffects spriteEffect)
        {
            graphicsDevice = GraphicsDevice;
            spriteRoom = room;
            LoadContent();

            //TAKE PLAYER Position
            Reset(position, spriteEffect);
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

            Stream txtReader = Microsoft.Xna.Framework.TitleContainer.OpenStream(PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_SEQUENCES + "KID_sequence.xml");


            //TextReader txtReader = File.OpenText(PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_SEQUENCES + "KID_sequence.xml");
            //Stream astream = this.GetType().Assembly.GetManifestResourceStream("PrinceOfPersia.resources.KID_sequence.xml");
            spriteSequence = (List<Sequence>)ax.Deserialize(txtReader);

            foreach (Sequence s in spriteSequence)
            {
                s.Initialize(SpriteRoom.content);
            }

            // Calculate bounds within texture size.         
            //faccio un rettangolo che sia largo la metà del frame e che parta dal centro
            int top = 0; //StandAnimation.FrameHeight - height - 128;
            int left = 0; //PLAYER_L_PENETRATION; //THE LEFT BORDER!!!! 19
            int width = 114;//(int)(StandAnimation.FrameWidth);  //lo divido per trovare punto centrale *2)
            int height = 114;//(int)(StandAnimation.FrameHeight);


            localBounds = new Rectangle(left, top, width, height);

            // Load sounds.            
            //killedSound = _room.content.Load<SoundEffect>("Sounds/PlayerKilled");
            //jumpSound = _room.content.Load<SoundEffect>("Sounds/PlayerJump");
            //fallSound = _room.content.Load<SoundEffect>("Sounds/PlayerFall");
        }




        public void Reset()
        {
            Reset(startPosition, startFlip);
        }

        /// <summary>
        /// Resets the player to life.
        /// </summary>
        /// <param name="position">The position to come to life at.</param>
        public void Reset(Vector2 position, SpriteEffects spriteEffect)
        {
            startPosition = position;

            _position = new Position(new Vector2(graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height), new Vector2(Player.SPRITE_SIZE_X, Player.SPRITE_SIZE_Y));
            _position.X = position.X;
            _position.Y = position.Y;
            
            Velocity = Vector2.Zero;
            Energy = PrinceOfPersiaGame.CONFIG_KID_START_ENERGY;

            flip = spriteEffect;

            spriteState.Clear();

            Stand();

        }

        ///// <summary>
        ///// Start position the player to life.
        ///// </summary>
        ///// <param name="position">The position to come to life at.</param>
        //public void Start(Vector2 position)
        //{
        //    _position.X = position.X;
        //    _position.Y = position.Y;
        //    Velocity = Vector2.Zero;
        //    Energy = PrinceOfPersiaGame.CONFIG_KID_START_ENERGY;
            

        //    spriteState.Clear();

        //    Stand();

        //}


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
            if (IsAlive == false)
            {
                DeadFall();
                //return;
            }

            //ApplyPhysicsNew(gameTime);
            HandleCollisionsNew();

            Enumeration.Input input = GetInput(keyboardState, gamePadState, touchState, accelState, orientation);

            ParseInput(input);

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            // TODO: Add your game logic here.
            sprite.UpdateFrame(elapsed, ref _position, ref flip, ref spriteState);

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
                            if (flip == SpriteEffects.FlipHorizontally)
                                Turn();
                            else
                                StartRunning();
                            break;
                        case Enumeration.State.step1:
                            StartRunning();
                            break;
                        case Enumeration.State.crouch:
                            if (flip == SpriteEffects.FlipHorizontally)
                                return;
                            Crawl();
                            break;
                        case Enumeration.State.stepfall:
                            StepFall();
                            break;
                        case Enumeration.State.startrun:
                            if (flip == SpriteEffects.FlipHorizontally)
                                RunTurn();
                            break;
                        //case Enumeration.State.hang:
                        //    HangDrop();
                        //    break;
                        case Enumeration.State.bump:
                            Bump(spriteState.Value().Priority);
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
                            if (flip == SpriteEffects.FlipHorizontally)
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

                        default:
                            break;
                    }
                    break;
                //LEFTDOWN//////////////////////
                case Enumeration.Input.leftdown:
                    switch (spriteState.Value().state)
                    {
                        case Enumeration.State.crouch:
                            if (flip == SpriteEffects.None)
                                Crawl();
                            break;
                        case Enumeration.State.startrun:
                            GoDown(new Vector2(5, 0));
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
                            if (flip == SpriteEffects.None)
                                Turn();
                            else
                                StartRunning();
                            break;
                        case Enumeration.State.step1:
                            StartRunning();
                            break;
                        case Enumeration.State.crouch:
                            if (flip == SpriteEffects.None)
                                return;
                            Crawl();
                            break;
                        case Enumeration.State.stepfall:
                            StepFall();
                            break;
                        case Enumeration.State.startrun:
                            if (flip == SpriteEffects.None)
                                RunTurn();
                            break;
                        //case Enumeration.State.hang:
                        //    HangDrop();
                        //    break;

                        default:
                            break;
                    }
                    break;
                //SHIFTRIGHT//////////////////////
                case Enumeration.Input.rightshift:
                    switch (spriteState.Value().state)
                    {
                        case Enumeration.State.stand:
                            if (flip == SpriteEffects.None)
                                Turn();
                            else
                                StepForward();
                            break;
                        case Enumeration.State.hang:
                            Hang();
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
                            if (flip != SpriteEffects.None)
                                Crawl();
                            break;
                        case Enumeration.State.hang:
                            ClimbFail();
                            break;
                        case Enumeration.State.startrun:
                            GoDown(new Vector2(5, 0));
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
                    switch (spriteState.Value().state)
                    {
                        case Enumeration.State.stand:
                            GoDown();
                            break;
                        case Enumeration.State.hang:
                        case Enumeration.State.hangstraight:
                            HangDrop();
                            break;
                        case Enumeration.State.startrun:
                            GoDown(new Vector2(5, 0));
                            break;

                        default:
                            break;
                    }
                    break;
                //UP//////////////////////
                case Enumeration.Input.up:
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
                        default:
                            CheckItemOnFloor();
                            break;
                    }
                    break;

                default:
                    break;
            }


        }


        /// <summary>
        /// Gets player horizontal movement and jump commands from input.
        /// </summary>
        private Enumeration.Input GetInput(
            KeyboardState keyboardState,
            GamePadState gamePadState,
            TouchCollection touchState,
            AccelerometerState accelState,
            DisplayOrientation orientation)
        {
     



            if (spriteState.Value().Priority == Enumeration.PriorityState.Force)
                return Enumeration.Input.none;



            ////////
            // CHEAT DEBUG
            /////////
            if (PrinceOfPersia.PrinceOfPersiaGame.CONFIG_DEBUG == true)
            {
                if (keyboardState.IsKeyDown(Keys.NumPad8))
                {
                    SpriteRoom = Maze.UpRoom(SpriteRoom);
                    return Enumeration.Input.none;
                }
                if (keyboardState.IsKeyDown(Keys.NumPad2))
                {
                    SpriteRoom = Maze.DownRoom(SpriteRoom);
                    return Enumeration.Input.none;
                }
                if (keyboardState.IsKeyDown(Keys.NumPad4))
                {
                    SpriteRoom = Maze.LeftRoom(SpriteRoom);
                    return Enumeration.Input.none;
                }
                if (keyboardState.IsKeyDown(Keys.NumPad6))
                {
                    SpriteRoom = Maze.RightRoom(SpriteRoom);
                    return Enumeration.Input.none;
                }
                if (keyboardState.IsKeyDown(Keys.NumPad0))
                {
                    Maze.StartRoom().StartNewLife(graphicsDevice);
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

            }


            //////////
            //SHIFT
            //////////
            if (keyboardState.GetPressedKeys().Count() == 1 && keyboardState.GetPressedKeys()[0] == (Keys.LeftShift | Keys.RightShift))
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
            else if (keyboardState.IsKeyDown(Keys.Down) & keyboardState.IsKeyDown(Keys.Left))
            {
                return Enumeration.Input.leftdown;
            }
            else if (keyboardState.IsKeyDown(Keys.Left) & (keyboardState.IsKeyDown(Keys.LeftShift) | (keyboardState.IsKeyDown(Keys.RightShift))))
            {
                return Enumeration.Input.leftshift;
            }
            else if (gamePadState.IsButtonDown(Buttons.DPadLeft) || keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            {
                return Enumeration.Input.left;
            }

            //////////
            //RIGHT 
            //////////
            else if (keyboardState.IsKeyDown(Keys.Up) & keyboardState.IsKeyDown(Keys.Right))
            {
                return Enumeration.Input.rightup;
            }
            else if (keyboardState.IsKeyDown(Keys.Down) & keyboardState.IsKeyDown(Keys.Right))
            {
                return Enumeration.Input.righdown;
            }
            else if (keyboardState.IsKeyDown(Keys.Right) & (keyboardState.IsKeyDown(Keys.LeftShift) | (keyboardState.IsKeyDown(Keys.RightShift))))
            {
                return Enumeration.Input.rightshift;
            }

            else if (gamePadState.IsButtonDown(Buttons.DPadRight) || keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            {
                return Enumeration.Input.right;
            }
            //////
            //UP//
            //////
            else if (gamePadState.IsButtonDown(Buttons.DPadUp) || keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
            {
                return Enumeration.Input.up;
            }
            ///////
            //DOWN/
            ///////
            else if (gamePadState.IsButtonDown(Buttons.DPadDown) || keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
            {
                return Enumeration.Input.down;
            }
            ////////
            //NONE//
            else
            {
                return Enumeration.Input.none;
            }

        }


        //WARNING routine buggedddd
        private bool isBehind(Rectangle tileBounds, Rectangle playerBounds)
        {
            if (flip == SpriteEffects.FlipHorizontally)
            {
                //if (playerBounds.X > tileBounds.X - PLAYER_R_PENETRATION - Tile.PERSPECTIVE)
                if (playerBounds.X > tileBounds.X)
                    return true;
                else
                    return false;
            }
            else
            {
                if (playerBounds.X < tileBounds.X)
                    return true;
                else
                    return false;

            }

        }



        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns>
        ///// null is no wall
        ///// true for bump +
        ///// false fro bump -
        ///// </returns>
        ///// 
        //private bool? IsFrontOfBlockNew(bool isForHang)
        //{
        //    // Get the player's bounding rectangle and find neighboring tiles.
        //    Rectangle playerBounds = _position.Bounding;
        //    Vector2 v2 = _room.getCenterTile(playerBounds);

        //    int x = (int)v2.X;
        //    int y = (int)v2.Y;


        //    Rectangle tileBounds = _room.GetBounds(x, y);
        //    Vector2 depth = RectangleExtensions.GetIntersectionDepth(playerBounds, tileBounds);
        //    TileCollision tileCollision = _room.GetCollision(x, y);
        //    TileType tileType = _room.GetType(x, y);



        //    if (tileType != TileType.block)
        //        return null;

        //    if (isForHang == true)
        //        return true;

        //    if (flip == SpriteEffects.FlipHorizontally)
        //    {
        //        if (depth.X <= -18 & depth.X > (-Tile.PERSPECTIVE - PLAYER_R_PENETRATION)) //18*3 = 54 step forward....
        //            return true;
        //        else if (depth.X <= (-Tile.PERSPECTIVE - PLAYER_R_PENETRATION))
        //            return false;
        //        else
        //            return null;
        //    }
        //    else
        //    {
        //        //if (depth.X >= 18 & depth.X <= 27) //18*3 = 54 step forward....
        //        if (depth.X >= 18 & depth.X < (Tile.PERSPECTIVE + PLAYER_L_PENETRATION)) //18*3 = 54 step forward....
        //            return true;
        //        else if (depth.X >= (Tile.PERSPECTIVE + PLAYER_L_PENETRATION))
        //            return false;
        //        else
        //            return null;
        //    }
        //}


        private bool? IsDownOfBlock(bool isForClimbDown)
        {
            // Get the player's bounding rectangle and find neighboring tiles.
            Rectangle playerBounds = _position.Bounding;
            Vector4 v4 = SpriteRoom.getBoundTiles(playerBounds);

            int x, y = 0;

            if (flip == SpriteEffects.FlipHorizontally)
            { x = (int)v4.Y - 1; y = (int)v4.Z + 1; }
            else
            { x = (int)v4.X; y = (int)v4.W + 1; }

            Rectangle tileBounds = SpriteRoom.GetBounds(x, y);
            Vector2 depth = RectangleExtensions.GetIntersectionDepth(playerBounds, tileBounds);
            Enumeration.TileCollision tileCollision = SpriteRoom.GetCollision(x, y);
            Enumeration.TileType tileType = SpriteRoom.GetType(x, y);



            if (tileType != Enumeration.TileType.block)
                return false;

            if (isForClimbDown == true)
                return true;

            if (flip == SpriteEffects.FlipHorizontally)
            {
                if (depth.X <= -18 & depth.X > (-Tile.PERSPECTIVE - PLAYER_R_PENETRATION)) //18*3 = 54 step forward....
                    return true;
                else if (depth.X <= (-Tile.PERSPECTIVE - PLAYER_R_PENETRATION))
                    return false;
                else
                    return null;
            }
            else
            {
                //if (depth.X >= 18 & depth.X <= 27) //18*3 = 54 step forward....
                if (depth.X >= 18 & depth.X < (Tile.PERSPECTIVE + PLAYER_L_PENETRATION)) //18*3 = 54 step forward....
                    return true;
                else if (depth.X >= (Tile.PERSPECTIVE + PLAYER_L_PENETRATION))
                    return false;
                else
                    return null;
            }
        }

        private bool? IsFrontOfBlock(bool isForHang)
        {
            // Get the player's bounding rectangle and find neighboring tiles.
            Rectangle playerBounds = _position.Bounding;
            Vector4 v4 = SpriteRoom.getBoundTiles(playerBounds);

            int x, y = 0;

            if (flip == SpriteEffects.FlipHorizontally)
            { x = (int)v4.Y; y = (int)v4.Z; }
            else
            { x = (int)v4.X; y = (int)v4.W; }

            Rectangle tileBounds = SpriteRoom.GetBounds(x, y);
            Vector2 depth = RectangleExtensions.GetIntersectionDepth(playerBounds, tileBounds);
            Enumeration.TileCollision tileCollision = SpriteRoom.GetCollision(x, y);
            Enumeration.TileType tileType = SpriteRoom.GetType(x, y);



            if (tileType != Enumeration.TileType.block)
                return null;

            if (isForHang == true)
                return true;

            if (flip == SpriteEffects.FlipHorizontally)
            {
                if (depth.X <= -18 & depth.X > (-Tile.PERSPECTIVE - PLAYER_R_PENETRATION)) //18*3 = 54 step forward....
                    return true;
                else if (depth.X <= (-Tile.PERSPECTIVE - PLAYER_R_PENETRATION))
                    return false;
                else
                    return null;
            }
            else
            {
                //if (depth.X >= 18 & depth.X <= 27) //18*3 = 54 step forward....
                if (depth.X >= 18 & depth.X < (Tile.PERSPECTIVE + PLAYER_L_PENETRATION)) //18*3 = 54 step forward....
                    return true;
                else if (depth.X >= (Tile.PERSPECTIVE + PLAYER_L_PENETRATION))
                    return false;
                else
                    return null;
            }
        }


        public void isGround()
        {
            isOnGround = false;

            RoomNew room = null;
            Rectangle playerBounds = _position.Bounding;
            Vector2 v2 = SpriteRoom.getCenterTile(playerBounds);
            Rectangle tileBounds = SpriteRoom.GetBounds((int)v2.X, (int)v2.Y);

            //Check if kid outside Room 
            if (v2.X < 0)
                room = Maze.LeftRoom(SpriteRoom);
            else
                room = SpriteRoom;

            if (v2.Y > 2)
            {
                isOnGround = false;
            }
            else if (v2.Y < 0)
            {
                isOnGround = false;
            }
            else
            {
                if (room.GetCollision((int)v2.X, (int)v2.Y) != Enumeration.TileCollision.Passable)
                {
                    if (playerBounds.Bottom >= tileBounds.Bottom)
                        isOnGround = true;
                }
            }


            if (isOnGround == false)
            {
                if (sprite.sequence.raised == false)
                {
                    if (spriteState.Value().state == Enumeration.State.runjump)
                    {
                        spriteState.Add(Enumeration.State.rjumpfall, Enumeration.PriorityState.Force);
                        sprite.PlayAnimation(spriteSequence, spriteState.Value());
                    }
                    else
                    {
                        if (spriteState.Previous().state == Enumeration.State.runjump)
                            spriteState.Add(Enumeration.State.stepfall, Enumeration.PriorityState.Force, new Vector2(20,15));
                        else
                            if (spriteState.Value().state != Enumeration.State.freefall)
                                spriteState.Add(Enumeration.State.stepfall, Enumeration.PriorityState.Force);
                    }
                    SpriteRoom.LooseShake();
                    //and for upper room...
                    SpriteRoom.maze.UpRoom(SpriteRoom).LooseShake();
                }
                return;
            }

            
            //IS ON GROUND!
            if (spriteState.Value().state == Enumeration.State.freefall)
            {
                //Align to tile x
                _position.Y = tileBounds.Bottom - _position._spriteRealSize.Y;
                //CHECK IF LOOSE ENERGY...
                int Rem = 0;
                Rem = (int) Math.Abs(Position.Y - PositionFall.Y) / Tile.REALHEIGHT;

                if (Rem > 0)
                {
                    Energy = Energy - Rem;
                }
                spriteState.Add(Enumeration.State.crouch, Enumeration.PriorityState.Force, false);
            }
            
        }

        public void CheckItemOnFloor()
        {
            //Rectangle playerBounds = ;
            Vector2 v2 = SpriteRoom.getCenterTile(_position.Bounding);
            
            int x = (int)v2.X;
            int y = (int)v2.Y;

            Tile t = SpriteRoom.GetTile(x, y);

            if (t.item != null)
            {
                if (t.item.GetType() == typeof(Flask))
                {
                    DrinkPotion();
                }
                if (t.item.GetType() == typeof(Sword))
                {
                    PickupSword();
                }

                SpriteRoom.SubsTile(v2, Enumeration.TileType.floor);
            }

        }

        public void HandleCollisionsNew()
        {
            isGround();

            //Check Sprite collision
            foreach (Sprite s in SpriteRoom.SpritesInRoom())
            {
                if (s.Position.CheckCollision(Position))
                {
                    Impale();
                }
            }


            Rectangle playerBounds = _position.Bounding;
            //Find how many tiles are near on the left
            Vector4 v4 = SpriteRoom.getBoundTiles(playerBounds);

            // For each potentially colliding Tile, warning the for check only the player row ground..W
            for (int y = (int)v4.Z; y <= (int)v4.W; ++y)
            {
                for (int x = (int)v4.X; x <= (int)v4.Y; ++x)
                {
                    Rectangle tileBounds = SpriteRoom.GetBounds(x, y);
                    Vector2 depth = RectangleExtensions.GetIntersectionDepth(playerBounds, tileBounds);
                    Enumeration.TileCollision tileCollision = SpriteRoom.GetCollision(x, y);
                    Enumeration.TileType tileType = SpriteRoom.GetType(x, y);

                    switch (tileType)
                    {
                        case Enumeration.TileType.spikes :
                            if (IsAlive == false)
                            {
                                ((Spikes)SpriteRoom.GetTile(x, y)).Open(); 
                                return; }

                            if (flip == SpriteEffects.FlipHorizontally)
                            {
                                if (depth.X < 10 & depth.Y >= Player.SPRITE_SIZE_Y)
                                    ((Spikes)SpriteRoom.GetTile(x, y)).Open();

                                if (depth.X <= -30 & depth.Y >= Player.SPRITE_SIZE_Y & ((Spikes)SpriteRoom.GetTile(x, y)).State == Enumeration.StateTile.open)
                                    Impale();
                            }
                            else
                            {
                                if (depth.X > -10 & depth.Y >= Player.SPRITE_SIZE_Y)
                                    ((Spikes)SpriteRoom.GetTile(x, y)).Open();

                                if (depth.X >= 60 & depth.Y >= Player.SPRITE_SIZE_Y & ((Spikes)SpriteRoom.GetTile(x, y)).State == Enumeration.StateTile.open)
                                    Impale();
                            }

                            break;

                        case Enumeration.TileType.loose:
                            if (flip == SpriteEffects.FlipHorizontally)
                            {
                                if (depth.X < (-Tile.PERSPECTIVE - PLAYER_R_PENETRATION))
                                    ((Loose)SpriteRoom.GetTile(x, y)).Press();
                                else
                                    isLoosable();
                            }
                            else
                            {
                                if (depth.X > (Tile.PERSPECTIVE + PLAYER_L_PENETRATION)) //45
                                    ((Loose)SpriteRoom.GetTile(x, y)).Press();
                                else
                                    isLoosable();
                            }
                            break;

                        case Enumeration.TileType.pressplate:
                            ((PressPlate)SpriteRoom.GetTile(x, y)).Press();
                            break;
                        case Enumeration.TileType.door:
                        case Enumeration.TileType.block:
                            if (tileType == Enumeration.TileType.door)
                                if (((Door)SpriteRoom.GetTile(x, y)).State == Enumeration.StateTile.opened)
                                    break;
                            //if player are raised then not collide..


                            //if sx wall i will penetrate..for perspective design
                            if (flip == SpriteEffects.FlipHorizontally)
                            {
                                //only for x pixel 
                                if (depth.X < (-Tile.PERSPECTIVE - PLAYER_R_PENETRATION))
                                {
                                    if (spriteState.Value().state != Enumeration.State.freefall &
                                        spriteState.Value().state != Enumeration.State.highjump &
                                        spriteState.Value().state != Enumeration.State.hang &
                                        spriteState.Value().state != Enumeration.State.hangstraight &
                                        spriteState.Value().state != Enumeration.State.hangdrop &
                                        spriteState.Value().state != Enumeration.State.hangfall &
                                        spriteState.Value().state != Enumeration.State.jumphangMed &
                                        spriteState.Value().state != Enumeration.State.jumphangLong &
                                        spriteState.Value().state != Enumeration.State.climbup &
                                        spriteState.Value().state != Enumeration.State.climbdown
                                        )
                                    {
                                        _position.Value = new Vector2(_position.X + (depth.X - (-Tile.PERSPECTIVE - PLAYER_R_PENETRATION)), _position.Y);
                                        if (sprite.sequence.raised == false)
                                            Bump(Enumeration.PriorityState.Force);
                                        else
                                            RJumpFall(Enumeration.PriorityState.Force);
                                        return;
                                    }
                                }
                                else
                                {
                                    if (sprite.sequence.raised == true)
                                        _position.Value = new Vector2(_position.X, _position.Y);
                                    else
                                        _position.Value = new Vector2(_position.X, _position.Y);
                                }
                            }
                            else
                            {
                                if (depth.X > (Tile.PERSPECTIVE + PLAYER_L_PENETRATION)) //45
                                {
                                    //if(sprite.sequence.raised == false)
                                    if (spriteState.Value().state != Enumeration.State.freefall &
                                        spriteState.Value().state != Enumeration.State.highjump &
                                        spriteState.Value().state != Enumeration.State.hang &
                                        spriteState.Value().state != Enumeration.State.hangstraight &
                                        spriteState.Value().state != Enumeration.State.hangdrop &
                                        spriteState.Value().state != Enumeration.State.hangfall &
                                        spriteState.Value().state != Enumeration.State.jumphangMed &
                                        spriteState.Value().state != Enumeration.State.jumphangLong &
                                        spriteState.Value().state != Enumeration.State.climbup &
                                        spriteState.Value().state != Enumeration.State.climbdown

                                            )
                                    {
                                        _position.Value = new Vector2(_position.X + (depth.X - (Tile.PERSPECTIVE + PLAYER_L_PENETRATION)), _position.Y);
                                        Bump(Enumeration.PriorityState.Force);
                                        return;
                                    }
                                }
                                else
                                    if (sprite.sequence.raised == true)
                                        _position.Value = new Vector2(_position.X, _position.Y);
                                    else
                                        _position.Value = new Vector2(_position.X, _position.Y);
                            }
                            playerBounds = BoundingRectangle;
                            break;

                        //default:
                        //    _position.Value = new Vector2(_position.X, tileBounds.Bottom);
                        //    playerBounds = BoundingRectangle;
                        //    break;
                    }

                }
            }
            //???
            //previousBottom = playerBounds.Bottom;
            //check if out room
            if (_position.Y > RoomNew.BOTTOM_LIMIT+10)
            {
                RoomNew room = Maze.DownRoom(SpriteRoom);
                SpriteRoom = room;
                _position.Y = RoomNew.TOP_LIMIT + 27; // Y=77
                //For calculate height fall from damage points calculations..
                PositionFall = new Vector2(Position.X, (Game.CONFIG_SCREEN_HEIGHT - RoomNew.BOTTOM_LIMIT - PositionFall.Y));

                
            }
            else if (_position.X >= RoomNew.RIGHT_LIMIT)
            {
                RoomNew room = Maze.RightRoom(SpriteRoom);
                SpriteRoom = room;
                _position.X = RoomNew.LEFT_LIMIT + 10;
            }
            else if (_position.X <= RoomNew.LEFT_LIMIT)
            {
                RoomNew room = Maze.LeftRoom(SpriteRoom);
                SpriteRoom = room;
                _position.X = RoomNew.RIGHT_LIMIT - 10;
            }
            else if (_position.Y < RoomNew.TOP_LIMIT-10)
            {
                RoomNew room = Maze.UpRoom(SpriteRoom);
                SpriteRoom = room;
                _position.Y = RoomNew.BOTTOM_LIMIT - 24;  //Y=270
            }

        }


        /// <summary>
        /// Called when this player reaches the level's exit.
        /// </summary>
        public void OnReachedExit()
        {
            //sprite.PlayAnimation(celebrateAnimation);
        }


        //public bool isSnapToPlayerGrid()
        //{
        //    //FIX SNAP TO THE GRID
        //    if (_position.X % PLAYER_GRID != 0)
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        //public void SnapToPlayerGrid()
        //{
        //    FIX SNAP TO THE GRID
        //    if (Position.X % PLAYER_GRID != 0 & spriteState.Value() == spriteState.State.stand)
        //    {
        //        Position = new Vector2((float)Math.Round(Position.X / PLAYER_GRID) * PLAYER_GRID + ((Position.X % PLAYER_GRID) * movement), position.Y);
        //    }
        //}


        /// <summary>
        /// Draws the animated player.
        /// </summary>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //DRAW REAL COORDINATE
            //sprite.DrawNew(gameTime, spriteBatch, _position.Value, PositionArrive, flip, 0.5f);

            //DRAW SPRITE
            sprite.DrawSprite(gameTime, spriteBatch, _position.Value, flip, 0.5f);

        }

        public void DeadFall()
        {
            spriteState.Add(Enumeration.State.deadfall, Enumeration.PriorityState.Force);
            sprite.PlayAnimation(spriteSequence, spriteState.Value());
        }

        public void Impale()
        {
            spriteState.Add(Enumeration.State.impale);
            sprite.PlayAnimation(spriteSequence, spriteState.Value());
            //Energy = 0;
        }

        public void Turn()
        {
            if (flip == SpriteEffects.FlipHorizontally)
                flip = SpriteEffects.None;
            else
                flip = SpriteEffects.FlipHorizontally;

            spriteState.Add(Enumeration.State.turn);
            sprite.PlayAnimation(spriteSequence, spriteState.Value());
            spriteState.Add(Enumeration.State.stand);

        }

        public void RunStop()
        { RunStop(Enumeration.PriorityState.Normal); }
        public void RunStop(Enumeration.PriorityState priority)
        {
            spriteState.Add(Enumeration.State.runstop);
            sprite.PlayAnimation(spriteSequence, spriteState.Value());
            spriteState.Add(Enumeration.State.stand);
        }

        public void StartRunning()
        {
            //TODO: i will check if there a wall and do a BUMP...
            bool? isFront = IsFrontOfBlock(false);
            if (isFront == true)
            {
                Bump(Enumeration.PriorityState.Normal, Enumeration.SequenceReverse.Reverse);
                return;
            }
            else if (isFront == false)
            {
                Bump(Enumeration.PriorityState.Normal, Enumeration.SequenceReverse.Normal);
                return;
            }

            spriteState.Add(Enumeration.State.startrun);
            sprite.PlayAnimation(spriteSequence, spriteState.Value());
        }

        public void StepForward()
        {
            if (sprite.IsStoppable == false)
                return;

            //TODO: i will check if there a wall and do a BUMP...
            bool? isFront = IsFrontOfBlock(false);
            if (isFront == true)
            {
                Bump(Enumeration.PriorityState.Normal, Enumeration.SequenceReverse.Reverse);
                return;
            }
            else if (isFront == false)
            {
                Bump(Enumeration.PriorityState.Normal, Enumeration.SequenceReverse.Normal);
                return;
            }


            spriteState.Add(Enumeration.State.fullstep);
            sprite.PlayAnimation(spriteSequence, spriteState.Value());
            spriteState.Add(Enumeration.State.stand);
        }




        public void Crouch()
        { Crouch(Enumeration.PriorityState.Normal, null); }

        public void Crouch(Enumeration.PriorityState priority)
        { Crouch(priority, null); }

        public void Crouch(bool? stoppable)
        { Crouch(Enumeration.PriorityState.Normal, stoppable); }

        public void Crouch(Enumeration.PriorityState priority, bool? stoppable)
        { Crouch(Enumeration.PriorityState.Normal, stoppable, Vector2.Zero); }

        public void Crouch(Enumeration.PriorityState priority, bool? stoppable, Vector2 offset)
        {
            if (priority == Enumeration.PriorityState.Normal & sprite.IsStoppable == false)
                return;

            spriteState.Add(Enumeration.State.crouch, priority, stoppable, offset);
            sprite.PlayAnimation(spriteSequence, spriteState.Value());
        }


        public void Crawl()
        {
            //if (spriteState.Previous().state == Enumeration.State.crawl)
            //    return;

            spriteState.Add(Enumeration.State.crawl);
            sprite.PlayAnimation(spriteSequence, spriteState.Value());
        }

        /// Remnber: for example the top gate is x=3 y=1
        /// first row bottom = 2 the top row = 0..
        /// </summary>
        /// <returns>
        /// true = climbable floor
        /// false = no climb
        /// 
        /// </returns>
        private bool isClimbableDown()
        {

            // Get the player's bounding rectangle and find neighboring tiles.
            Rectangle playerBounds = _position.Bounding;
            Vector2 v2 = SpriteRoom.getCenterTile(playerBounds);

            int x = (int)v2.X;
            int y = (int)v2.Y;


            //Rectangle tileBounds = _room.GetBounds(x, y);
            //Vector2 depth = RectangleExtensions.GetIntersectionDepth(playerBounds, tileBounds);
            Enumeration.TileCollision tileCollision; // = _room.GetCollision(x, y);
            //Enumeration.TileType tileType;

            if (flip == SpriteEffects.FlipHorizontally)
                //tileType = _room.GetType(x-1, y);
                tileCollision = SpriteRoom.GetCollision(x - 1, y);
            else
                //tileType = _room.GetType(x+1, y);
                tileCollision = SpriteRoom.GetCollision(x + 1, y);


            //if (tileType != Enumeration.TileType.space)
            if (tileCollision != Enumeration.TileCollision.Passable)
            {
                return false;
            }

            //check is platform or gate forward up
            int xOffSet = 0;
            if (flip == SpriteEffects.FlipHorizontally)
            { xOffSet = -Tile.REALWIDTH + (Tile.PERSPECTIVE * 2); }
            else
            { xOffSet = -Tile.PERSPECTIVE; }

            Rectangle tileBounds = SpriteRoom.GetBounds(x, y + 1);
            _position.Value = new Vector2(tileBounds.Center.X + xOffSet, _position.Y);
            return true;

        }

        /// <summary>
        /// Remnber: for example the top gate is x=3 y=1
        /// first row bottom = 2 the top row = 0..
        /// </summary>
        /// <returns>
        /// true = climbable floor
        /// false = no climb
        /// 
        /// </returns>
        private Enumeration.State isClimbable()
        {

            // Get the player's bounding rectangle and find neighboring tiles.
            Rectangle playerBounds = _position.Bounding;
            Vector2 v2 = SpriteRoom.getCenterTile(playerBounds);

            int x = (int)v2.X;
            int y = (int)v2.Y;


            //Rectangle tileBounds = _room.GetBounds(x, y);
            //Vector2 depth = RectangleExtensions.GetIntersectionDepth(playerBounds, tileBounds);
            Enumeration.TileCollision tileCollision = SpriteRoom.GetCollision(x, y - 1);
            Enumeration.TileType tileType; // = _room.GetType(x, y - 1);


            //if (tileType == Enumeration.TileType.floor | tileType == Enumeration.TileType.gate)
            if (tileCollision == Enumeration.TileCollision.Platform)
            {
                //CHECK KID IS UNDER THE FLOOR CHECK NEXT TILE
                if (flip == SpriteEffects.FlipHorizontally)
                { x = x - 1; }
                else
                { x = x + 1; }
                tileCollision = SpriteRoom.GetCollision(x, y - 1);
                //tileType = _room.GetType(x, y - 1);
                //if (tileType != Enumeration.TileType.space)
                if (tileCollision != Enumeration.TileCollision.Passable)
                    return Enumeration.State.none;
                x = ((int)v2.X); //THE FLOOR FOR CLIMB UP

                Tile t = SpriteRoom.GetTile(new Vector2(x, y));
                if (t.Type == Enumeration.TileType.door)
                    if (t.tileState.Value().state != Enumeration.StateTile.opened)
                        return Enumeration.State.none;

            }
            //else if (tileType == Enumeration.TileType.space)
            else if (tileCollision == Enumeration.TileCollision.Passable)
            {
                if (flip == SpriteEffects.FlipHorizontally)
                { x = x + 1; }
                else
                { x = x - 1; }
                tileCollision = SpriteRoom.GetCollision(x, y - 1);
                //tileType = _room.GetType(x, y - 1);
                //if (tileType != Enumeration.TileType.floor & tileType != Enumeration.TileType.gate)
                if (tileCollision != Enumeration.TileCollision.Platform)
                    return Enumeration.State.none;

                Tile t = SpriteRoom.GetTile(new Vector2(x, y - 1));
                if (t.Type == Enumeration.TileType.door)
                    if (t.tileState.Value().state != Enumeration.StateTile.opened)
                        return Enumeration.State.none;

            }
            else
                return Enumeration.State.none;

            //If is a door close isnt climbable


            //check is platform or gate forward up
            int xOffSet = 0;
            if (flip == SpriteEffects.FlipHorizontally)
            { xOffSet = -Tile.REALWIDTH + Tile.PERSPECTIVE; }

            Rectangle tileBounds;
            //if under kid there's not a platform change the point to climbup..

            if (flip == SpriteEffects.FlipHorizontally)
             tileCollision = SpriteRoom.GetCollision(x - 1, y); 
            else
                tileCollision = SpriteRoom.GetCollision(x + 1, y);


            if (tileCollision == Enumeration.TileCollision.Passable & flip != SpriteEffects.FlipHorizontally)
            {
                tileBounds = SpriteRoom.GetBounds(x, y);
                xOffSet = -26;
                _position.Value = new Vector2(tileBounds.Center.X + xOffSet, _position.Y);
                return Enumeration.State.jumphangMed;
            }

            tileBounds = SpriteRoom.GetBounds(x, y - 1);
            _position.Value = new Vector2(tileBounds.Center.X + xOffSet, _position.Y);
            return Enumeration.State.jumphangLong;

        }

        private bool isLoosable()
        {

            // Get the player's bounding rectangle and find neighboring tiles.
            Rectangle playerBounds = _position.Bounding;
            Vector2 v2 = SpriteRoom.getCenterTile(playerBounds);

            int x = (int)v2.X;
            int y = (int)v2.Y;


            //Rectangle tileBounds = _room.GetBounds(x, y);
            //Vector2 depth = RectangleExtensions.GetIntersectionDepth(playerBounds, tileBounds);
            Enumeration.TileCollision tileCollision = SpriteRoom.GetCollision(x, y - 1);
            Enumeration.TileType tileType; // = _room.GetType(x, y - 1);


            //if (tileType == Enumeration.TileType.floor | tileType == Enumeration.TileType.gate)
            if (tileCollision == Enumeration.TileCollision.Platform)
            {
                //CHECK KID IS UNDER THE FLOOR CHECK NEXT TILE
                Tile t = SpriteRoom.GetTile(x, y - 1);
                if (t.Type != Enumeration.TileType.loose)
                {
                    return false;
                }
                ((Loose)t).Press();
               

            }
            return false;
        }


        public void JumpHangMed()
        {
            if (sprite.IsStoppable == false)
                return;

            spriteState.Add(Enumeration.State.jumphangMed);
            sprite.PlayAnimation(spriteSequence, spriteState.Value());
        }


        public void JumpHangLong()
        {
            if (sprite.IsStoppable == false)
                return;

            spriteState.Add(Enumeration.State.jumphangLong);
            sprite.PlayAnimation(spriteSequence, spriteState.Value());
        }

        public void ClimbUp()
        {
            if (sprite.IsStoppable == false)
                return;

            spriteState.Add(Enumeration.State.climbup);
            sprite.PlayAnimation(spriteSequence, spriteState.Value());
        }


        public void ClimbDown()
        {
            if (sprite.IsStoppable == false)
                return;

            bool isDownOfBlock = false;

            if (IsDownOfBlock(true) == true)
                isDownOfBlock = true;

            spriteState.Add(Enumeration.State.climbdown, isDownOfBlock);
            sprite.PlayAnimation(spriteSequence, spriteState.Value());
        }


        public void StandUp()
        { StandUp(Enumeration.PriorityState.Normal); }
        public void StandUp(Enumeration.PriorityState priority)
        {
            spriteState.Add(Enumeration.State.standup, priority);
            sprite.PlayAnimation(spriteSequence, spriteState.Value());
            spriteState.Add(Enumeration.State.stand, priority);
        }

        public void GoDown()
        { GoDown(Enumeration.PriorityState.Normal); }
        public void GoDown(Vector2 offSet)
        { GoDown(Enumeration.PriorityState.Normal, offSet); }
        public void GoDown(Enumeration.PriorityState priority)
        { GoDown(Enumeration.PriorityState.Normal, Vector2.Zero); }
        public void GoDown(Enumeration.PriorityState priority, Vector2 offSet)
        {
            if (isClimbableDown() == true)
            {
                ClimbDown();
                return;
            }
            spriteState.Add(Enumeration.State.godown, priority, null, Enumeration.SequenceReverse.Normal, offSet);
            sprite.PlayAnimation(spriteSequence, spriteState.Value());
            spriteState.Add(Enumeration.State.crouch, priority);
        }

        public void Stand()
        { Stand(Enumeration.PriorityState.Normal, null); }

        public void Stand(bool stoppable)
        {
            Stand(Enumeration.PriorityState.Normal, stoppable);
        }

        public void Stand(Enumeration.PriorityState priority)
        {
            Stand(priority, null);
        }

        public void Stand(Enumeration.PriorityState priority, bool? stoppable)
        {
            if (priority == Enumeration.PriorityState.Normal & sprite.IsStoppable == stoppable)
                return;

            switch (spriteState.Previous().state)
            {
                case Enumeration.State.freefall:
                    Crouch(Enumeration.PriorityState.Force);
                    return;

                case Enumeration.State.crouch:
                    StandUp();
                    return;
                case Enumeration.State.startrun:
                    RunStop();
                    return;

                default:
                    break;
            }


            spriteState.Add(Enumeration.State.stand, priority);
            sprite.PlayAnimation(spriteSequence, spriteState.Value());
            spriteState.Add(Enumeration.State.stand, Enumeration.PriorityState.Normal);

        }


        public void StandJump()
        { StandJump(Enumeration.PriorityState.Normal); }
        public void StandJump(Enumeration.PriorityState priority)
        {
            spriteState.Add(Enumeration.State.standjump, priority);
            sprite.PlayAnimation(spriteSequence, spriteState.Value());
        }

        public void StepFall()
        { StepFall(Enumeration.PriorityState.Normal); }
        public void StepFall(Enumeration.PriorityState priority)
        {
            Vector2 offSet = Vector2.Zero;
            StepFall(priority, offSet);
        }
        public void StepFall(Enumeration.PriorityState priority, Vector2 offSet)
        {
            PositionFall = Position.Value ;
            spriteState.Add(Enumeration.State.stepfall, priority, offSet);
            sprite.PlayAnimation(spriteSequence, spriteState.Value());
            spriteState.Add(Enumeration.State.freefall, priority);
        }

        public void HighJump()
        {
            switch (isClimbable())
            { 
                case Enumeration.State.jumphangMed:
                    JumpHangMed();
                    return;
                case Enumeration.State.jumphangLong:
                    JumpHangLong();
                    return;
            }

            spriteState.Add(Enumeration.State.highjump);
            sprite.PlayAnimation(spriteSequence, spriteState.Value());

            //isPressable();
        }

        public void ClimbFail()
        {
            spriteState.Add(Enumeration.State.climbfail);
            sprite.PlayAnimation(spriteSequence, spriteState.Value());
        }

        public void HangDrop()
        {
            spriteState.Add(Enumeration.State.hangdrop);
            sprite.PlayAnimation(spriteSequence, spriteState.Value());
        }



        public void RunTurn()
        {
            if (flip != SpriteEffects.FlipHorizontally)
                flip = SpriteEffects.None;
            else
                flip = SpriteEffects.FlipHorizontally;

            spriteState.Add(Enumeration.State.runturn);
            sprite.PlayAnimation(spriteSequence, spriteState.Value());
        }


        public void Hang()
        {
            //chek what kind of hang or hangstraight
            if (spriteState.Value().state == Enumeration.State.hang | spriteState.Value().state == Enumeration.State.hangstraight)
                return;

            if (IsFrontOfBlock(true) == true)
                spriteState.Add(Enumeration.State.hangstraight);
            else
                spriteState.Add(Enumeration.State.hang);

            sprite.PlayAnimation(spriteSequence, spriteState.Value());
        }

        public void Bump()
        {
            Bump(Enumeration.PriorityState.Normal, Enumeration.SequenceReverse.Normal);
        }

        public void Bump(Enumeration.PriorityState priority)
        {
            Bump(priority, Enumeration.SequenceReverse.Normal);
        }

        public void Bump(Enumeration.PriorityState priority, Enumeration.SequenceReverse reverse)
        {
            spriteState.Add(Enumeration.State.bump, priority, false, reverse);
            sprite.PlayAnimation(spriteSequence, spriteState.Value());
            spriteState.Add(Enumeration.State.stand, Enumeration.PriorityState.Normal);
        }


        public void RJumpFall()
        {
            RJumpFall(Enumeration.PriorityState.Normal, Enumeration.SequenceReverse.Normal);
        }

        public void RJumpFall(Enumeration.PriorityState priority)
        {
            RJumpFall(priority, Enumeration.SequenceReverse.Normal);
        }

        public void RJumpFall(Enumeration.PriorityState priority, Enumeration.SequenceReverse reverse)
        {
            spriteState.Add(Enumeration.State.rjumpfall, priority, false, reverse);
            sprite.PlayAnimation(spriteSequence, spriteState.Value());
        }



        public void RunJump()
        {
            spriteState.Add(Enumeration.State.runjump);
            sprite.PlayAnimation(spriteSequence, spriteState.Value());
        }

        public void PickupSword()
        {
            spriteState.Add(Enumeration.State.pickupsword);
            sprite.PlayAnimation(spriteSequence, spriteState.Value());
        }

        public void DrinkPotion()
        {
            spriteState.Add(Enumeration.State.drinkpotion);
            sprite.PlayAnimation(spriteSequence, spriteState.Value());
            this.Energy = this.LivePoints; 
        }


        public void Question()
        {
            if (spriteState.Value().state == Enumeration.State.hang | spriteState.Value().state == Enumeration.State.hangstraight)
            {
                Hang();
            }
        }


    }
}
