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
using System.IO;
using Microsoft.Xna.Framework.Input.Touch;


namespace PrinceOfPersia
{

    public class Player : Sprite
    {
 
        //Sequence static for share purpose
        private static List<Sequence> playerSequence = null;

        private GraphicsDevice graphicsDevice;

        private SpriteEffects flip = SpriteEffects.None;
        //private AnimationPlayer sprite;
        public AnimationSequence sprite;

        // Sounds
        private SoundEffect killedSound;
        private SoundEffect jumpSound;
        private SoundEffect fallSound;

        //Player Grid
        public const int PLAYER_GRID = 1;
        public const int PLAYER_VELOCITY = 120;
        public const int PLAYER_L_PENETRATION = 19;
        public const int PLAYER_R_PENETRATION = 30;//30??

        public const int PLAYER_STAND_BORDER_FRAME = 47; //47+20player+47=114
        public const int PLAYER_STAND_FRAME = 20;
        public const int PLAYER_STAND_WALL_PEN = 30; //wall penetration
        public const int PLAYER_STAND_FLOOR_PEN = 26; //floor border penetration
        public const int PLAYER_STAND_HANG_PEN = 46; //floor border penetration for hangup


        private Position _position;
        public PlayerState playerState = new PlayerState();

        private RoomNew _room;

        public bool IsAlive
        {
            get { return isAlive; }
        }
        bool isAlive;

        public Position Position
        {
            get { return _position; }
        }

   
        // Physics state
        public Vector2 PositionArrive
        {
            get { return positionArrive; }
            set { positionArrive = value; }
        }
        Vector2 positionArrive;

        private float previousBottom;

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        Vector2 velocity;

        // Constants for controling horizontal movement
        private const float MoveAcceleration = 13000.0f;
        private const float MaxMoveSpeed = 1750.0f;
        private const float GroundDragFactor = 0.48f;
        private const float AirDragFactor = 0.58f;

        // Constants for controlling vertical movement
        private const float MaxJumpTime = 0.35f;
        private const float JumpLaunchVelocity = -3500.0f;
        private const float GravityAcceleration = 3400.0f;
        private const float MaxFallSpeed = 100f;//550.0f;
        private const float JumpControlPower = 0.14f; 

        // Input configuration
        private const float MoveStickScale = 1.0f;
        private const float AccelerometerScale = 1.5f;
        private const Buttons JumpButton = Buttons.A;

        /// <summary>
        /// Gets whether or not the player's feet are on the ground.
        /// </summary>
        public bool IsOnGround
        {
            get { return isOnGround; }
        }
        bool isOnGround;

   

        private Rectangle localBounds;
        /// <summary>
        /// Gets a rectangle which bounds this player in world space.
        /// </summary>
        public Rectangle BoundingRectangle
        {
            get
            {
                int left = (int)Math.Round(_position.X) + localBounds.X;
                int top = (int)Math.Round(_position.Y - sprite.Origin.Y) + localBounds.Y;

                return new Rectangle(left, top, localBounds.Width, localBounds.Height);
            }
        }

        public Rectangle BoundingRectangleReal
        {
            get
            {
                int left = (int)Math.Round(_position.X) - (localBounds.Width); //square 114x114
                int top = (int)Math.Round(_position.Y) + localBounds.Y;

                return new Rectangle(left, top, localBounds.Width*2, localBounds.Height);
            }
        }

        /// <summary>
        /// Constructors a new player.
        /// </summary>
        public Player(RoomNew _room, Vector2 position, GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            this._room = _room;
            LoadContent();

            _position = new Position(new Vector2(graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height), new Vector2(114,114));

            Reset(position);
        }

        /// <summary>
        /// Loads the player sprite sheet and sounds.
        /// </summary>
        /// <note>i will add a parameter read form app.config</note>
        /// 
        /// 
        private void LoadContent()
        {

            playerSequence = new List<Sequence>();
            System.Xml.Serialization.XmlSerializer ax = new System.Xml.Serialization.XmlSerializer(playerSequence.GetType());
            Stream astream = this.GetType().Assembly.GetManifestResourceStream("PrinceOfPersia.resources.KID_sequence.xml");
            playerSequence = (List<Sequence>)ax.Deserialize(astream);

            foreach (Sequence s in playerSequence)
            {
                s.Initialize(_room.content);
            }

            // Calculate bounds within texture size.         
            //faccio un rettangolo che sia largo la metà del frame e che parta dal centro
            int top = 0; //StandAnimation.FrameHeight - height - 128;
            int left = 0; //PLAYER_L_PENETRATION; //THE LEFT BORDER!!!! 19
            int width = 114;//(int)(StandAnimation.FrameWidth);  //lo divido per trovare punto centrale *2)
            int height = 114;//(int)(StandAnimation.FrameHeight);


            localBounds = new Rectangle(left, top, width, height);

            // Load sounds.            
            killedSound = _room.content.Load<SoundEffect>("Sounds/PlayerKilled");
            jumpSound = _room.content.Load<SoundEffect>("Sounds/PlayerJump");
            fallSound = _room.content.Load<SoundEffect>("Sounds/PlayerFall");
        }

        /// <summary>
        /// Resets the player to life.
        /// </summary>
        /// <param name="position">The position to come to life at.</param>
        public void Reset(Vector2 position)
        {
            _position.X = position.X;
            _position.Y = position.Y;
            Velocity = Vector2.Zero;
            isAlive = true;
            //sprite.IsStoppable = true;

            playerState.Clear();

            Stand();
        
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

            //ApplyPhysicsNew(gameTime);
            HandleCollisionsNew();

            StateElement.Input input = GetInput(keyboardState, gamePadState, touchState, accelState, orientation);

            ParseInput(input);

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            // TODO: Add your game logic here.
            sprite.UpdateFrame(elapsed, ref _position, ref flip, ref playerState);
        
        }

        public void ParseInput(StateElement.Input input)
        {

            if (playerState.Value().Priority == StateElement.PriorityState.Normal & sprite.IsStoppable == false)
                return;

            switch (input)
            {
                case StateElement.Input.none :
                    switch (playerState.Value().state)
                    {
                        case StatePlayerElement.State.none:
                            Stand(playerState.Value().Priority);
                            break;
                        case StatePlayerElement.State.stand:
                            Stand(playerState.Value().Priority);
                            break;
                        case StatePlayerElement.State.startrun:
                            RunStop();
                            break;
                        case StatePlayerElement.State.running:
                            RunStop();
                            break;
                        case StatePlayerElement.State.step1:
                            Stand();
                            break;
                        case StatePlayerElement.State.stepfall:
                            StepFall();
                            break;
                        case StatePlayerElement.State.crouch:
                            StandUp();
                            break;
                        case StatePlayerElement.State.highjump:
                            Stand();
                            break;
                        case StatePlayerElement.State.hangstraight:
                        case StatePlayerElement.State.hang:
                            HangDrop();
                            break;
                        case StatePlayerElement.State.bump:
                            Bump(playerState.Value().Priority);
                            break;

                        default:
                            break;
                    }
                    break;
                //LEFT//////////////////////
                case StateElement.Input.left:
                    switch (playerState.Value().state)
                    {
                        case StatePlayerElement.State.stand:
                            if (flip == SpriteEffects.FlipHorizontally)
                                turn();
                            else
                                StartRunning();
                            break;
                        case StatePlayerElement.State.step1:
                            StartRunning();
                            break;
                        case StatePlayerElement.State.crouch:
                            if (flip == SpriteEffects.FlipHorizontally)
                                return;
                            Crawl();
                            break;
                        case StatePlayerElement.State.stepfall:
                            StepFall();
                            break;
                        case StatePlayerElement.State.startrun:
                            if (flip == SpriteEffects.FlipHorizontally)
                                RunTurn();
                            break;
                        //case StatePlayerElement.State.hang:
                        //    HangDrop();
                        //    break;
                        case StatePlayerElement.State.bump:
                            Bump(playerState.Value().Priority);
                            break;

                        default:
                            break;
                    }
                    break;
                //SHIFTLEFT//////////////////////
                case StateElement.Input.leftshift:
                    switch (playerState.Value().state)
                    {
                        case StatePlayerElement.State.stand:
                            if (flip == SpriteEffects.FlipHorizontally)
                                turn();
                            else
                                StepForward();
                            break;
                        //case StatePlayerElement.State.hang:
                        //    Hang();
                        //    break;
                        case StatePlayerElement.State.bump:
                            Bump(playerState.Value().Priority);
                            break;

                        default:
                            break;
                    }
                    break;
                //LEFTDOWN//////////////////////
                case StateElement.Input.leftdown:
                    switch (playerState.Value().state)
                    {
                        case StatePlayerElement.State.crouch:
                            if (flip == SpriteEffects.None)
                                Crawl();
                            break;
                        case StatePlayerElement.State.startrun:
                            GoDown(new Vector2(5, 0));
                            break;
                        default:
                            break;
                    }
                    break;
                //LEFTUP//////////////////////
                case StatePlayerElement.Input.leftup:
                    switch (playerState.Value().state)
                    {
                        case StatePlayerElement.State.stand:
                            StandJump();
                            break;
                        case StatePlayerElement.State.startrun:
                            RunJump();
                            break;
                        case StatePlayerElement.State.stepfall:
                            StepFall();
                            break;
                        default:
                            break;
                    }
                    break;

                //RIGHT//////////////////////
                case StatePlayerElement.Input.right:
                    switch (playerState.Value().state)
                    {
                        case StatePlayerElement.State.stand:
                            if (flip == SpriteEffects.None)
                                turn();
                            else
                                StartRunning();
                            break;
                        case StatePlayerElement.State.step1:
                            StartRunning();
                            break;
                        case StatePlayerElement.State.crouch:
                            if (flip == SpriteEffects.None)
                                return;
                            Crawl();
                            break;
                        case StatePlayerElement.State.stepfall:
                            StepFall();
                            break;
                        case StatePlayerElement.State.startrun:
                            if (flip == SpriteEffects.None)
                                RunTurn();
                            break;
                        //case StatePlayerElement.State.hang:
                        //    HangDrop();
                        //    break;

                        default:
                            break;
                    }
                    break;
                //SHIFTRIGHT//////////////////////
                case StatePlayerElement.Input.rightshift:
                    switch (playerState.Value().state)
                    {
                        case StatePlayerElement.State.stand:
                            if (flip == SpriteEffects.None)
                                turn();
                            else
                                StepForward();
                            break;
                        case StatePlayerElement.State.hang: 
                            Hang();
                            break;

                        default:
                            break;
                    }
                    break;
                //RIGHTDOWN//////////////////////
                case StatePlayerElement.Input.righdown:
                    switch (playerState.Value().state)
                    {
                        case StatePlayerElement.State.crouch:
                            if (flip != SpriteEffects.None)
                                Crawl();
                            break;
                        case StatePlayerElement.State.hang:
                            ClimbFail();
                            break;
                        case StatePlayerElement.State.startrun:
                            GoDown(new Vector2(5, 0));
                            break;
                        default:
                            break;
                    }
                    break;
                //RIGHTUP//////////////////////
                case StatePlayerElement.Input.rightup:
                    switch (playerState.Value().state)
                    {
                        case StatePlayerElement.State.stand:
                            StandJump();
                            break;
                        case StatePlayerElement.State.startrun:
                            RunJump();
                            break;
                        case StatePlayerElement.State.stepfall:
                            StepFall();
                            break;
                        default:
                            break;
                    }
                    break;

                //DOWN//////////////////////
                case StatePlayerElement.Input.down:
                    switch (playerState.Value().state)
                    {
                        case StatePlayerElement.State.stand:
                            GoDown();
                            break;
                        case StatePlayerElement.State.hang:
                        case StatePlayerElement.State.hangstraight:
                            HangDrop();
                            break;
                        case StatePlayerElement.State.startrun:
                            GoDown(new Vector2(5,0));
                            break;

                        default:
                            break;
                    }
                    break;
                //UP//////////////////////
                case StateElement.Input.up:
                    switch (playerState.Value().state)
                    {
                        case StatePlayerElement.State.running:
                        case StatePlayerElement.State.startrun:
                            RunStop();
                            break;
                        case StatePlayerElement.State.stand:
                            HighJump();
                            break;
                        case StatePlayerElement.State.jumphangMed:
                        case StatePlayerElement.State.hang:
                        case StatePlayerElement.State.hangstraight:
                            ClimbUp();
                            break;

                        default:
                            break;
                    }
                    break;
                //SHIFT/////////////////////////
                case StatePlayerElement.Input.shift:
                    switch (playerState.Value().state)
                    {
                        //case StatePlayerElement.State.hang:
                        //    Hang();
                        //    break;
                        case StatePlayerElement.State.jumphangMed:
                            Hang();
                            break;
                        default:
                            break;
                    }
                    break;

                default :
                    break;
            }
        
        
        }


        /// <summary>
        /// Gets player horizontal movement and jump commands from input.
        /// </summary>
        private StateElement.Input GetInput(
            KeyboardState keyboardState, 
            GamePadState gamePadState, 
            TouchCollection touchState,
            AccelerometerState accelState, 
            DisplayOrientation orientation)
        {
            if (playerState.Value().Priority == StateElement.PriorityState.Force)
                return StateElement.Input.none;


            //////////
            //SHIFT
            //////////
            if (keyboardState.GetPressedKeys().Count() == 1 && keyboardState.GetPressedKeys()[0] == (Keys.LeftShift | Keys.RightShift))
            {
                return StateElement.Input.shift;
            }


            //////////
            //LEFT 
            //////////
            if (keyboardState.IsKeyDown(Keys.Up) & keyboardState.IsKeyDown(Keys.Left))
            {
                return StateElement.Input.leftup;
            }
            else if (keyboardState.IsKeyDown(Keys.Down) & keyboardState.IsKeyDown(Keys.Left))
            {
                return StateElement.Input.leftdown;
            }
            else if (keyboardState.IsKeyDown(Keys.Left) & (keyboardState.IsKeyDown(Keys.LeftShift) | (keyboardState.IsKeyDown(Keys.RightShift))))
            {
                return StateElement.Input.leftshift;
            }
            else if (gamePadState.IsButtonDown(Buttons.DPadLeft) || keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            {
                return StateElement.Input.left;
            }

            //////////
            //RIGHT 
            //////////
            else if (keyboardState.IsKeyDown(Keys.Up) & keyboardState.IsKeyDown(Keys.Right))
            {
                return StateElement.Input.rightup;
            }
            else if (keyboardState.IsKeyDown(Keys.Down) & keyboardState.IsKeyDown(Keys.Right))
            {
                return StateElement.Input.righdown;
            }
            else if (keyboardState.IsKeyDown(Keys.Right) & (keyboardState.IsKeyDown(Keys.LeftShift) | (keyboardState.IsKeyDown(Keys.RightShift))))
            {
                return StateElement.Input.rightshift;
            }

            else if (gamePadState.IsButtonDown(Buttons.DPadRight) || keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            {
                return StateElement.Input.right;
            }
            //////
            //UP//
            //////
            else if (gamePadState.IsButtonDown(Buttons.DPadUp) || keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
            {
                return StateElement.Input.up;
            }
            ///////
            //DOWN/
            ///////
            else if (gamePadState.IsButtonDown(Buttons.DPadDown) || keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
            {
                return StateElement.Input.down;
            }
            ////////
            //NONE//
            else
            {
                return StateElement.Input.none;
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

  

        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// null is no wall
        /// true for bump +
        /// false fro bump -
        /// </returns>
        /// 
        private bool? IsFrontOfBlockNew(bool isForHang)
        {
            // Get the player's bounding rectangle and find neighboring tiles.
            Rectangle playerBounds = _position.Bounding;
            Vector2 v2 = _room.getCenterTile(playerBounds);

            int x = (int)v2.X;
            int y = (int)v2.Y;


            Rectangle tileBounds = _room.GetBounds(x, y);
            Vector2 depth = RectangleExtensions.GetIntersectionDepth(playerBounds, tileBounds);
            TileCollision tileCollision = _room.GetCollision(x, y);
            TileType tileType = _room.GetType(x, y);



            if (tileType != TileType.block)
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


        private bool? IsFrontOfBlock(bool isForHang)
        {
            // Get the player's bounding rectangle and find neighboring tiles.
            Rectangle playerBounds = _position.Bounding;
            Vector4 v4 = _room.getBoundTiles(playerBounds);

            int x,y =0;

            if (flip == SpriteEffects.FlipHorizontally)
            { x = (int)v4.Y; y = (int)v4.Z;}
            else
            { x = (int)v4.X; y = (int)v4.W;}

            Rectangle tileBounds = _room.GetBounds(x, y);
            Vector2 depth = RectangleExtensions.GetIntersectionDepth(playerBounds, tileBounds);
            TileCollision tileCollision = _room.GetCollision(x, y);
            TileType tileType = _room.GetType(x,y);
            


            if (tileType != TileType.block)
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

            Rectangle playerBounds = _position.Bounding;
            Vector2 v2 = _room.getCenterTile(playerBounds);
            Rectangle tileBounds = _room.GetBounds((int)v2.X,(int)v2.Y);

            if (_room.GetType((int)v2.X, (int)v2.Y) == TileType.floor | _room.GetType((int)v2.X, (int)v2.Y) == TileType.gate | _room.GetType((int)v2.X, (int)v2.Y) == TileType.torch)
            {
                if (playerBounds.Bottom >= tileBounds.Bottom)
                    isOnGround = true;
            }


            if (isOnGround == false)
            {
                if (sprite.sequence.raised == false)
                    //if (playerState.Value().state != StatePlayerElement.State.freefall &
                    //        playerState.Value().state != StatePlayerElement.State.highjump &
                    //        playerState.Value().state != StatePlayerElement.State.hang &
                    //        playerState.Value().state != StatePlayerElement.State.hangstraight &
                    //        playerState.Value().state != StatePlayerElement.State.hangdrop &
                    //        playerState.Value().state != StatePlayerElement.State.hangfall &
                    //        playerState.Value().state != StatePlayerElement.State.jumphangMed &
                    //        playerState.Value().state != StatePlayerElement.State.climbdown &
                    //    playerState.Value().state != StatePlayerElement.State.climbup
                    //    )
                {
                    //SNAP TO THE GROUND
                    playerState.Add(StatePlayerElement.State.stepfall);
                }
            }
            else
            {
                if (playerState.Value().state == StatePlayerElement.State.freefall)
                {
                    _position.Y = tileBounds.Bottom - _position._spriteRealSize.Y;
                    playerState.Add(StatePlayerElement.State.crouch, StateElement.PriorityState.Force, false);
                }
            }
        }

        public void HandleCollisionsNew()
        {
            isGround();

            Rectangle playerBounds = _position.Bounding;
            //Find how many tiles are near on the left
            Vector4 v4 = _room.getBoundTiles(playerBounds);

            // For each potentially colliding Tile, warning the for check only the player row ground..W
            for (int y = (int)v4.Z; y <= (int)v4.W; ++y)
            {
                for (int x = (int)v4.X; x <= (int)v4.Y; ++x)
                {
                    Rectangle tileBounds = _room.GetBounds(x, y);
                    Vector2 depth = RectangleExtensions.GetIntersectionDepth(playerBounds, tileBounds);
                    TileCollision tileCollision = _room.GetCollision(x, y);
                    TileType tileType = _room.GetType(x, y);

                    switch (tileType)
                    {
                        case TileType.block:
                            //if (isBehind(tileBounds, playerBounds) == true)
                            //{
                            //    _position.Value = new Vector2(_position.X, _position.Y);
                            //    playerBounds = BoundingRectangle;
                            //    break;
                            //}
                            //if sx wall i will penetrate..for perspective design
                            if (flip == SpriteEffects.FlipHorizontally)
                            {
                                //only for x pixel 
                                if (depth.X < (-Tile.PERSPECTIVE - PLAYER_R_PENETRATION))
                                {
    
                                    if (playerState.Value().state != StatePlayerElement.State.freefall &
                                        playerState.Value().state != StatePlayerElement.State.highjump &
                                        playerState.Value().state != StatePlayerElement.State.hang &
                                        playerState.Value().state != StatePlayerElement.State.hangstraight &
                                        playerState.Value().state != StatePlayerElement.State.hangdrop &
                                        playerState.Value().state != StatePlayerElement.State.hangfall &
                                        playerState.Value().state != StatePlayerElement.State.jumphangMed &
                                        playerState.Value().state != StatePlayerElement.State.climbup &
                                        playerState.Value().state != StatePlayerElement.State.climbdown
                                        )
                                    {
                                        _position.Value = new Vector2(_position.X + (depth.X - (-Tile.PERSPECTIVE - PLAYER_R_PENETRATION)), _position.Y);
                                        Bump(StateElement.PriorityState.Force);
                                        return;
                                    }
                                }
                                else
                                    _position.Value = new Vector2(_position.X, _position.Y);
                            }
                            else
                            {
                                if (depth.X > (Tile.PERSPECTIVE + PLAYER_L_PENETRATION)) //45
                                {
                                    //if(sprite.sequence.raised == false)
                                    if (playerState.Value().state != StatePlayerElement.State.freefall &
                                        playerState.Value().state != StatePlayerElement.State.highjump &
                                        playerState.Value().state != StatePlayerElement.State.hang &
                                        playerState.Value().state != StatePlayerElement.State.hangstraight &
                                        playerState.Value().state != StatePlayerElement.State.hangdrop &
                                        playerState.Value().state != StatePlayerElement.State.hangfall &
                                        playerState.Value().state != StatePlayerElement.State.jumphangMed &
                                        playerState.Value().state != StatePlayerElement.State.climbup &
                                        playerState.Value().state != StatePlayerElement.State.climbdown
                                            
                                            )
                                    {
                                        _position.Value = new Vector2(_position.X + (depth.X - (Tile.PERSPECTIVE + PLAYER_L_PENETRATION)), _position.Y);
                                        Bump(StateElement.PriorityState.Force);
                                        return;
                                    }
                                }
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
            previousBottom = playerBounds.Bottom;
        }

    
        /// <summary>
        /// Called when this player reaches the level's exit.
        /// </summary>
        public void OnReachedExit()
        {
            //sprite.PlayAnimation(celebrateAnimation);
        }


        public bool isSnapToPlayerGrid()
        {
            //FIX SNAP TO THE GRID
            if (_position.X % PLAYER_GRID != 0)
            {
                return false;
            }
            return true;
        }

        //public void SnapToPlayerGrid()
        //{
        //    FIX SNAP TO THE GRID
        //    if (Position.X % PLAYER_GRID != 0 & playerState.Value() == PlayerState.State.stand)
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
            sprite.DrawSprite(gameTime, spriteBatch, _position.Value, PositionArrive, flip, 0.5f);

        }

        public void turn()
        {
            if (flip == SpriteEffects.FlipHorizontally)
                flip = SpriteEffects.None;
            else
                flip = SpriteEffects.FlipHorizontally;

            playerState.Add(StatePlayerElement.State.turn); 
            sprite.PlayAnimation(playerSequence, playerState.Value());
            playerState.Add(StatePlayerElement.State.stand); 
            
        }

        public void RunStop()
        { RunStop(StateElement.PriorityState.Normal); }
        public void RunStop(StateElement.PriorityState priority)
        {
            playerState.Add(StatePlayerElement.State.runstop);
            sprite.PlayAnimation(playerSequence, playerState.Value());
            playerState.Add(StatePlayerElement.State.stand);
        }

        public void StartRunning()
        {
            //TODO: i will check if there a wall and do a BUMP...
            bool? isFront = IsFrontOfBlock(false);
            if (isFront == true)
            {
                Bump(StateElement.PriorityState.Normal, StateElement.SequenceReverse.Reverse);
                return;
            }
            else if (isFront == false)
            {
                Bump(StateElement.PriorityState.Normal, StateElement.SequenceReverse.Normal);
                return;
            }

            playerState.Add(StatePlayerElement.State.startrun);
            sprite.PlayAnimation(playerSequence, playerState.Value());
        }

        public void StepForward()
        {
            if (sprite.IsStoppable == false)
                return;

            //TODO: i will check if there a wall and do a BUMP...
            bool? isFront = IsFrontOfBlock(false);
            if (isFront == true)
            {
                Bump(StateElement.PriorityState.Normal, StateElement.SequenceReverse.Reverse);
                return;
            }
            else if (isFront == false)
            {
                Bump(StateElement.PriorityState.Normal, StateElement.SequenceReverse.Normal);
                return;
            }
            

            playerState.Add(StatePlayerElement.State.fullstep);
            sprite.PlayAnimation(playerSequence, playerState.Value());
            playerState.Add(StatePlayerElement.State.stand);
        }




        public void Crouch()
        { Crouch(StateElement.PriorityState.Normal, null); }

        public void Crouch(StateElement.PriorityState priority)
        { Crouch(priority, null); }

        public void Crouch(bool? stoppable)
        { Crouch(StateElement.PriorityState.Normal, stoppable); }
        
        public void Crouch(StateElement.PriorityState priority, bool? stoppable)
        { Crouch(StateElement.PriorityState.Normal, stoppable, Vector2.Zero); }

        public void Crouch(StateElement.PriorityState priority, bool? stoppable, Vector2 offset)
        {
            if (priority == StateElement.PriorityState.Normal & sprite.IsStoppable == false)
                return;

            playerState.Add(StatePlayerElement.State.crouch, priority, stoppable, offset);
            sprite.PlayAnimation(playerSequence, playerState.Value());
        }


        public void Crawl()
        {
            //if (playerState.Previous().state == StatePlayerElement.State.crawl)
            //    return;

            playerState.Add(StatePlayerElement.State.crawl);
            sprite.PlayAnimation(playerSequence, playerState.Value());
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
        private bool isClimbable()
        {

            // Get the player's bounding rectangle and find neighboring tiles.
            Rectangle playerBounds = _position.Bounding;
            Vector2 v2 = _room.getCenterTile(playerBounds);

            int x = (int)v2.X;
            int y = (int)v2.Y;


            //Rectangle tileBounds = _room.GetBounds(x, y);
            //Vector2 depth = RectangleExtensions.GetIntersectionDepth(playerBounds, tileBounds);
            //TileCollision tileCollision = _room.GetCollision(x, y);
            TileType tileType = _room.GetType(x, y-1);


            if (tileType == TileType.floor | tileType == TileType.gate)
            {
                //CHECK KID IS UNDER THE FLOOR CHECK NEXT TILE
                if (flip == SpriteEffects.FlipHorizontally)
                { x = x - 1; }
                else
                { x = x + 1; }
                tileType = _room.GetType(x, y - 1);
                if (tileType != TileType.space)
                    return false;
                x = ((int)v2.X); //THE FLOOR FOR CLIMB UP

            }
            else if (tileType == TileType.space)
            {
                if (flip == SpriteEffects.FlipHorizontally)
                { x = x + 1; }
                else
                { x = x - 1; }
                tileType = _room.GetType(x, y - 1);
                if (tileType != TileType.floor & tileType != TileType.gate)
                    return false;
            }
            else
                return false;

            //check is platform or gate forward up
            int xOffSet=0;
            if (flip == SpriteEffects.FlipHorizontally)
            { xOffSet = -Tile.REALWIDTH + Tile.PERSPECTIVE; }
         

            Rectangle tileBounds = _room.GetBounds(x, y - 1);
            _position.Value = new Vector2(tileBounds.Center.X + xOffSet, _position.Y);
            return true;

        }

        public void JumpHangMed()
        {
            if (sprite.IsStoppable == false)
                return;

            playerState.Add(StatePlayerElement.State.jumphangMed);
            sprite.PlayAnimation(playerSequence, playerState.Value());
        }


        public void ClimbUp()
        {
            if (sprite.IsStoppable == false)
                return;

            playerState.Add(StatePlayerElement.State.climbup);
            sprite.PlayAnimation(playerSequence, playerState.Value());
        }


        public void StandUp()
        { StandUp(StateElement.PriorityState.Normal); }
        public void StandUp(StateElement.PriorityState priority)
        {
            playerState.Add(StatePlayerElement.State.standup, priority);
            sprite.PlayAnimation(playerSequence, playerState.Value());
            playerState.Add(StatePlayerElement.State.stand, priority);
        }

        public void GoDown()
        { GoDown(StateElement.PriorityState.Normal); }
        public void GoDown(Vector2 offSet)
        { GoDown(StateElement.PriorityState.Normal, offSet); }
        public void GoDown(StateElement.PriorityState priority)
        {GoDown(StateElement.PriorityState.Normal, Vector2.Zero); }
        public void GoDown(StateElement.PriorityState priority, Vector2 offSet)
        {
            playerState.Add(StatePlayerElement.State.godown, priority, null, StateElement.SequenceReverse.Normal, offSet);
            sprite.PlayAnimation(playerSequence, playerState.Value());
            playerState.Add(StatePlayerElement.State.crouch, priority);
        }

        public void Stand()
        { Stand(StateElement.PriorityState.Normal, null); }

        public void Stand(bool stoppable)
        {
            Stand(StateElement.PriorityState.Normal, stoppable); 
        }

        public void Stand(StateElement.PriorityState priority)
        {
            Stand(priority, null);
        }

        public void Stand(StateElement.PriorityState priority, bool? stoppable)
        {
            if (priority == StateElement.PriorityState.Normal & sprite.IsStoppable == stoppable)
                return;

            switch (playerState.Previous().state)
            {
                case StatePlayerElement.State.freefall:
                    Crouch(StateElement.PriorityState.Force);
                    return;

                case StatePlayerElement.State.crouch:
                    StandUp();
                    return;
                case StatePlayerElement.State.startrun:
                    RunStop();
                    return;

                default:
                    break;
            }


            playerState.Add(StatePlayerElement.State.stand, priority);
            sprite.PlayAnimation(playerSequence, playerState.Value());
            playerState.Add(StatePlayerElement.State.stand, StateElement.PriorityState.Normal);

        }


        public void StandJump()
        { StandJump(StateElement.PriorityState.Normal); }
        public void StandJump(StateElement.PriorityState priority)
        {
            playerState.Add(StatePlayerElement.State.standjump, priority);
            sprite.PlayAnimation(playerSequence, playerState.Value());
        }

        public void StepFall()
        { StepFall(StateElement.PriorityState.Normal); }
        public void StepFall(StateElement.PriorityState priority)
        {
            playerState.Add(StatePlayerElement.State.stepfall, priority);
            sprite.PlayAnimation(playerSequence, playerState.Value());
            playerState.Add(StatePlayerElement.State.freefall, priority);
        }

        public void HighJump()
        {
            if (isClimbable() == true)
            {
                JumpHangMed();
                return;
            }
            playerState.Add(StatePlayerElement.State.highjump);
            sprite.PlayAnimation(playerSequence, playerState.Value());
        }

        public void ClimbFail()
        {
            playerState.Add(StatePlayerElement.State.climbfail);
            sprite.PlayAnimation(playerSequence, playerState.Value());
        }

        public void HangDrop()
        {
            playerState.Add(StatePlayerElement.State.hangdrop);
            sprite.PlayAnimation(playerSequence, playerState.Value());
        }



        public void RunTurn()
        {
            if (flip != SpriteEffects.FlipHorizontally)
                flip = SpriteEffects.None;
            else
                flip = SpriteEffects.FlipHorizontally;

            playerState.Add(StatePlayerElement.State.runturn);
            sprite.PlayAnimation(playerSequence, playerState.Value());
        }


        public void Hang()
        {
            //chek what kind of hang or hangstraight
            if (playerState.Value().state == StatePlayerElement.State.hang | playerState.Value().state == StatePlayerElement.State.hangstraight)
                return;

            if (IsFrontOfBlock(true) == true)
                playerState.Add(StatePlayerElement.State.hangstraight);
            else
                playerState.Add(StatePlayerElement.State.hang);

            sprite.PlayAnimation(playerSequence, playerState.Value());
        }    

        public void Bump()
        {
            Bump(StateElement.PriorityState.Normal, StateElement.SequenceReverse.Normal);
        }    

        public void Bump(StateElement.PriorityState priority)
        {
            Bump(priority, StateElement.SequenceReverse.Normal);
        }    

        public void Bump(StateElement.PriorityState priority, StateElement.SequenceReverse reverse)
        {
            playerState.Add(StatePlayerElement.State.bump, priority, false, reverse);
            sprite.PlayAnimation(playerSequence, playerState.Value());
            playerState.Add(StatePlayerElement.State.stand, StateElement.PriorityState.Normal);
        }

        public void RunJump()
        {
            playerState.Add(StatePlayerElement.State.runjump);
            sprite.PlayAnimation(playerSequence, playerState.Value());
        } 

    }
}
