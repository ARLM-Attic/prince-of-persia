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
    public class Guard : Sprite
    {

        /// <summary>
        /// Constructors a new player.
        /// </summary>
        public Guard(Room room, Vector2 position, GraphicsDevice GraphicsDevice, SpriteEffects spriteEffect)
        {
            graphicsDevice = GraphicsDevice;
            myRoom = room;
            LoadContent();
            

            //TAKE PLAYER Position
            Reset(position, spriteEffect);
        }

        /// Loads the player sprite sheet and sounds.
        /// </summary>
        /// <note>i will add a parameter read form app.config</note>
        /// 
        /// 
        private void LoadContent()
        {

            spriteSequence = new List<Sequence>();
            System.Xml.Serialization.XmlSerializer ax = new System.Xml.Serialization.XmlSerializer(spriteSequence.GetType());

            Stream txtReader = Microsoft.Xna.Framework.TitleContainer.OpenStream(PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_SEQUENCES + "guard_sequence.xml");


            //TextReader txtReader = File.OpenText(PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_SEQUENCES + "KID_sequence.xml");
            //Stream astream = this.GetType().Assembly.GetManifestResourceStream("PrinceOfPersia.resources.KID_sequence.xml");
            spriteSequence = (List<Sequence>)ax.Deserialize(txtReader);

            foreach (Sequence s in spriteSequence)
            {
                s.Initialize(myRoom.content);
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

        /// <summary>
        /// Handles input, performs physics, and animates the player sprite.
        /// </summary>
        /// <remarks>
        /// We pass in all of the input states so that our game is only polling the hardware
        /// once per frame. We also pass the game's orientation because when using the accelerometer,
        /// we need to reverse our motion when the orientation is in the LandscapeRight orientation.
        /// </remarks>
        public void Update
        (
            GameTime gameTime,
            KeyboardState keyboardState,
            GamePadState gamePadState,
            TouchCollection touchState,
            AccelerometerState accelState,
            DisplayOrientation orientation)
        {


            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            // TODO: Add your game logic here.
            sprite.UpdateFrame(elapsed, ref _position, ref flip, ref spriteState);

            if (IsAlive == false)
            {
                DropDead();
                return;
            }


            //bool thereIsKid = false;
            foreach (Sprite s in myRoom.SpritesInRoom())
            {
                switch (s.GetType().Name)
                {
                    case "Player":
                        {
                            if(s.IsAlive == false)
                                break;

                            //thereIsKid = true;
                            if (s.Position.CheckOnRow(Position))
                            {
                                if (s.Position.CheckOnRowDistancePixel(Position) >= 0 & s.Position.CheckOnRowDistancePixel(Position) <= 70 & Alert == true & spriteState.Value().state == Enumeration.State.strike)
                                {
                                    if (spriteState.Value().Name == Enumeration.State.strike.ToString())
                                    {
                                        //check if block
                                        if (s.spriteState.Value().Name != Enumeration.State.readyblock.ToString())
                                        {
                                            spriteState.Value().Name = string.Empty;
                                            s.Splash(true, gameTime);
                                            //Splash splash = new Splash(myRoom, Position.Value, graphicsDevice, SpriteEffects.None, true);
                                            //Maze.sprites.Add(splash);

                                            s.Energy = s.Energy - 1;
                                            s.StrikeRetreat();
                                        }
                                        else
                                        {
                                            System.Console.WriteLine("P->"+Enumeration.State.readyblock.ToString());
                                        }
                                        //blocked
                                    }
                                    if (s.Energy == 0)
                                    { Fastheathe(); }

                                }

                                Alert = true;

                                //Chenge Flip player..
                                if (Position.X < s.Position.X)
                                    flip = SpriteEffects.None;
                                else
                                    flip = SpriteEffects.FlipHorizontally;

                                Advance(s.Position, flip);

                                

                       


                            }
                            else
                                Alert = false;
                            break;
                        }
                    default:
                        break;
                }

            }



            if (Alert == false)
                Stand();//Ready();
            //else
              //  Stand();

        }

        /// <summary>
        /// Draws the animated player.
        /// </summary>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            sprite.DrawSprite(gameTime, spriteBatch, _position.Value, flip, 0.5f);
        }

        /// <summary>
        /// Resets the player to life.
        /// </summary>
        /// <param name="position">The position to come to life at.</param>
        public void Reset(Vector2 position, SpriteEffects spriteEffect)
        {
            _position = new Position(new Vector2(graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height), new Vector2(Player.SPRITE_SIZE_X, Player.SPRITE_SIZE_Y));
            _position.X = position.X;
            _position.Y = position.Y;
            Velocity = Vector2.Zero;
            Energy = PrinceOfPersiaGame.CONFIG_KID_START_ENERGY;

            flip = spriteEffect;

            spriteState.Clear();

            Stand();

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

            spriteState.Add(Enumeration.State.stand, priority);
            sprite.PlayAnimation(spriteSequence, spriteState.Value());
        }


        //?? the original guard engarde is used?
        public void Engarde()
        {
            Engarde(Enumeration.PriorityState.Normal, null);
        }
        public void Engarde(Enumeration.PriorityState priority, bool? stoppable)
        {
            if (priority == Enumeration.PriorityState.Normal & sprite.IsStoppable == stoppable)
                return;

            spriteState.Add(Enumeration.State.engarde, priority);
            sprite.PlayAnimation(spriteSequence, spriteState.Value());

        }


        public void GuardEngarde()
        {
            GuardEngarde(Enumeration.PriorityState.Normal, null);
        }
        public void GuardEngarde(Enumeration.PriorityState priority, bool? stoppable)
        {
            if (priority == Enumeration.PriorityState.Normal & sprite.IsStoppable == stoppable)
                return;

            spriteState.Add(Enumeration.State.guardengarde, priority);
            sprite.PlayAnimation(spriteSequence, spriteState.Value());

        }

        public void Ready()
        {
            Ready(Enumeration.PriorityState.Normal, null);
        }
        public void Ready(Enumeration.PriorityState priority, bool? stoppable)
        {
            if (priority == Enumeration.PriorityState.Normal & sprite.IsStoppable == stoppable)
                return;

            spriteState.Add(Enumeration.State.ready, priority);
            sprite.PlayAnimation(spriteSequence, spriteState.Value());

        }

        public void Advance(Position position, SpriteEffects flip)
        {
            if (flip == SpriteEffects.FlipHorizontally)
                this.flip = SpriteEffects.None;
            else
                this.flip = SpriteEffects.FlipHorizontally;


            if (position.X + Sprite.SPRITE_SIZE_X - 30  <= Position.X)
            {
                //flip = SpriteEffects.None;
                spriteState.Add(Enumeration.State.advance, Enumeration.PriorityState.Normal);
            }
            else if (position.X - Sprite.SPRITE_SIZE_X + 30 >= Position.X)
            {
                spriteState.Add(Enumeration.State.advance, Enumeration.PriorityState.Normal);
            }
            else
            {
                //flip = SpriteEffects.FlipHorizontally;
                Strike();
                return;
                //spriteState.Add(Enumeration.State.ready, Enumeration.PriorityState.Normal);
            }
            
            sprite.PlayAnimation(spriteSequence, spriteState.Value());

        }


        public void HandleCollisionsNew()
        {
            isGround();

            Rectangle playerBounds = _position.Bounding;
            //Find how many tiles are near on the left
            Vector4 v4 = myRoom.getBoundTiles(playerBounds);

            // For each potentially colliding Tile, warning the for check only the player row ground..W
            for (int y = (int)v4.Z; y <= (int)v4.W; ++y)
            {
                for (int x = (int)v4.X; x <= (int)v4.Y; ++x)
                {
                    Rectangle tileBounds = myRoom.GetBounds(x, y);
                    Vector2 depth = RectangleExtensions.GetIntersectionDepth(playerBounds, tileBounds);
                    Enumeration.TileCollision tileCollision = myRoom.GetCollision(x, y);
                    Enumeration.TileType tileType = myRoom.GetType(x, y);

                    switch (tileType)
                    {
                        case Enumeration.TileType.spikes:
                            if (IsAlive == false)
                            {
                                ((Spikes)myRoom.GetTile(x, y)).Open();
                                return;
                            }

                            if (flip == SpriteEffects.FlipHorizontally)
                            {
                                if (depth.X < 10 & depth.Y >= Player.SPRITE_SIZE_Y)
                                    ((Spikes)myRoom.GetTile(x, y)).Open();

                               // if (depth.X <= -30 & depth.Y >= Player.SPRITE_SIZE_Y & ((Spikes)myRoom.GetTile(x, y)).State == Enumeration.StateTile.open)
                                 //   Impale();
                            }
                            else
                            {
                                if (depth.X > -10 & depth.Y >= Player.SPRITE_SIZE_Y)
                                    ((Spikes)myRoom.GetTile(x, y)).Open();

                                //if (depth.X >= 60 & depth.Y >= Player.SPRITE_SIZE_Y & ((Spikes)myRoom.GetTile(x, y)).State == Enumeration.StateTile.open)
                                  //  Impale();
                            }

                            break;

                        case Enumeration.TileType.loose:
                            if (flip == SpriteEffects.FlipHorizontally)
                            {
                                if (depth.X < (-Tile.PERSPECTIVE - PLAYER_R_PENETRATION))
                                    ((Loose)myRoom.GetTile(x, y)).Press();
                                //else
                                    //isLoosable();
                            }
                            else
                            {
                                if (depth.X > (Tile.PERSPECTIVE + PLAYER_L_PENETRATION)) //45
                                    ((Loose)myRoom.GetTile(x, y)).Press();
                                //else
                                    //isLoosable();
                            }
                            break;

                        case Enumeration.TileType.pressplate:
                            ((PressPlate)myRoom.GetTile(x, y)).Press();
                            break;
                        case Enumeration.TileType.gate:
                        case Enumeration.TileType.block:
                            if (tileType == Enumeration.TileType.gate)
                                if (((Gate)myRoom.GetTile(x, y)).State == Enumeration.StateTile.opened)
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
                                        //if (sprite.sequence.raised == false)
                                        //    Bump(Enumeration.PriorityState.Force);
                                        //else
                                        //    RJumpFall(Enumeration.PriorityState.Force);
                                        //return;
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
                                        //Bump(Enumeration.PriorityState.Force);
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
            if (_position.Y > Room.BOTTOM_LIMIT + 10)
            {
                myRoom = myRoom.Down;
                _position.Y = Room.TOP_LIMIT + 27; // Y=77
                //For calculate height fall from damage points calculations..
                PositionFall = new Vector2(Position.X, (Game.CONFIG_SCREEN_HEIGHT - Room.BOTTOM_LIMIT - PositionFall.Y));
            }
            else if (_position.X >= Room.RIGHT_LIMIT)
            {
                myRoom = myRoom.Right;
                _position.X = Room.LEFT_LIMIT + 10;
            }
            else if (_position.X <= Room.LEFT_LIMIT)
            {
                myRoom = myRoom.Left;
                _position.X = Room.RIGHT_LIMIT - 10;
            }
            else if (_position.Y < Room.TOP_LIMIT - 10)
            {
                myRoom = myRoom.Up;
                _position.Y = Room.BOTTOM_LIMIT - 24;  //Y=270
            }

        }


    }
}
