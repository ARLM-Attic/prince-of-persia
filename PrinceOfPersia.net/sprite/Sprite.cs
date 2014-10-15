	//-----------------------------------------------------------------------//
	// <copyright file="Sprite.cs" company="A.D.F.Software">
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
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input.Touch;

namespace PrinceOfPersia
{
    public class Sprite
    {
        public const int SPRITE_SIZE_X = 114; //to be var
        public const int SPRITE_SIZE_Y = 114; //to be var

        public const int PLAYER_L_PENETRATION = 19;
        public const int PLAYER_R_PENETRATION = 30;

        public const int PLAYER_STAND_BORDER_FRAME = 47; //47+20player+47=114
        public const int PLAYER_STAND_FRAME = 20;
        public const int PLAYER_STAND_WALL_PEN = 30; //wall penetration
        public const int PLAYER_STAND_FLOOR_PEN = 26; //floor border penetration is PERSPECTIVE??!?
        public const int PLAYER_STAND_HANG_PEN = 46; //floor border penetration for hangup

        private bool sword = false; // the sword for combat, set true when player get it
        private int livePoints = PoP.CONFIG_KID_START_ENERGY; //default live point
        private int energy = PoP.CONFIG_KID_START_ENERGY;  //default energy point
        private bool alert = false; //set true when theres an enemy in the row's (y) room 
        private bool looseWarning = false; //set true when player on testfoot have touched a loose, next time player fall into loose

        protected Vector2 positionFall = Vector2.Zero;
        protected List<Sequence> spriteSequence = null;
        protected GraphicsDevice graphicsDevice;
        protected Position _position = null;
        protected Vector2 velocity;
        protected bool isOnGround;
        protected Rectangle localBounds;
        protected Room myRoom = null;


        public Vector2 startPosition = Vector2.Zero;
        public SpriteEffects startFace = SpriteEffects.None;
        public SpriteEffects face = SpriteEffects.None;
        public AnimationSequence sprite;
        public PlayerState spriteState = new PlayerState();

        public bool isClimbing
        {
            get
            {
                if (
                    spriteState.Value().state == Enumeration.State.climbdown ||
                    spriteState.Value().state == Enumeration.State.climbfail ||
                    spriteState.Value().state == Enumeration.State.climbup ||
                    spriteState.Value().state == Enumeration.State.jumphangLong
                    )
                    return true;
                else
                    return false;
            }
        }




        public bool Sword
        {
            set
            {
                sword = value;
            }
            get
            {
                return sword;
            }
        }

        public Maze Maze
        {
            get
            {
                return myRoom.maze;
            }
        }

        // Alert state
        public bool Alert
        {
            get
            {
                return alert;
            }
            set
            {
                alert = value;
            }
        }

        public Room MyRoom
        {
            get
            {
                return myRoom;
            }
            set
            {
                myRoom = value;
            }
        }

        public Level MyLevel
        {
            get
            {
                return myRoom.level;
            }
        }



        // Physics state, used by calculate falldrop distance
        public Vector2 PositionFall
        {
            get { return positionFall; }
            set { positionFall = value; }
        }
       


        public bool IsAlive
        {
            get 
            {
                if (energy > 0)
                    return true;
                else
                    return false;
            }
        }
        

        //How many energy triangle have this sprite
        public int LivePoints
        {
            get { return livePoints; }
            set 
            { 
                livePoints = value;
            }
        }
        

        //energy of the sprite when zero is dead
        public int Energy
        {
            get { return energy; }
            set
            {
                energy = value;
                if (energy > livePoints)
                    energy = livePoints;
            }
        }
        


        public Position Position
        {
            get { return _position; }
        }

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }


        public bool IsOnGround
        {
            get { return isOnGround; }
        }

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

                return new Rectangle(left, top, localBounds.Width * 2, localBounds.Height);
            }
        }


        public void CheckGround()
        {
            isOnGround = false;


            Rectangle playerBounds = _position.Bounding;
            Vector2 v2 = myRoom.getCenterTilePosition(playerBounds);
            Tile myTile = myRoom.getCenterTile(playerBounds);

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
                if (myRoom.GetCollision((int)v2.X, (int)v2.Y) == Enumeration.TileCollision.Platform)
                {
                    if (playerBounds.Bottom >= myTile.Position.Bounding.Bottom)
                    {
                        isOnGround = true;
                    }
                    else
                    { }
                }
                else
                { }
            }


            if (isOnGround == false)
            {
                if (spriteState.Value().Raised == false)
                {
                    switch (spriteState.Value().state)
                    {
                        case Enumeration.State.hangdrop: 
                            return; 
                            break;
                             
                        default:
                            break;
                    }

                    if (spriteState.Previous().state == Enumeration.State.runjump)
                        spriteState.Add(Enumeration.State.stepfall, Enumeration.PriorityState.Force, new Vector2(20, 15));
                    else
                        if (spriteState.Value().state != Enumeration.State.freefall)
                            spriteState.Add(Enumeration.State.stepfall, Enumeration.PriorityState.Force);
                    
                    myRoom.Up.LooseShake();
                }
                return;
            }

            CalulateFallingDamage();

        }


        public void CalulateFallingDamage()
        {
            Rectangle playerBounds = _position.Bounding;
            Vector2 v2 = myRoom.getCenterTilePosition(playerBounds);
            Tile myTile = myRoom.getCenterTile(playerBounds);

            //IS ON GROUND!
            if (spriteState.Value().state == Enumeration.State.freefall)
            {
                //Align to tile y
                _position.Y = myTile.Position.Bounding.Bottom - _position._spriteRealSize.Y;
                //CHECK IF LOOSE ENERGY...
                int Rem = 0;
                Rem = (int)Math.Abs(Position.Y - PositionFall.Y) / Tile.REALHEIGHT;

                if (Rem == 0)
                {
                    ((SoundEffect)Maze.Content[PoP.CONFIG_SOUNDS + "falling echo"]).Play();
                }
                else if (Rem >= 1 & Rem < 3)
                {
                    ((SoundEffect)Maze.Content[PoP.CONFIG_SOUNDS + "loosing a life falling"]).Play();
                }
                else
                {
                    ((SoundEffect)Maze.Content[PoP.CONFIG_SOUNDS + "falling"]).Play();
                    Energy = 0;
                }
                Energy = Energy - Rem;
                spriteState.Add(Enumeration.State.crouch, Enumeration.PriorityState.Force, false);
                myRoom.LooseShake();

                if (Energy <= 0)
                {
                    DeadFall();
                }
            }
        }

        public void DeadFall()
        {
            spriteState.Add(Enumeration.State.deadfall, Enumeration.PriorityState.Force);
            sprite.PlayAnimation(spriteSequence, spriteState);
            energy = 0;
        }

        public void DropDead()
        {
            energy = 0;
            if (spriteState.Value().state != Enumeration.State.dropdead)
            {
                spriteState.Add(Enumeration.State.dropdead, Enumeration.PriorityState.Force);
                sprite.PlayAnimation(spriteSequence, spriteState);
            }
        }

        public void Resheathe()
        {
            spriteState.Add(Enumeration.State.resheathe, Enumeration.PriorityState.Normal);
            sprite.PlayAnimation(spriteSequence, spriteState);
        }

        public void Fastheathe(Enumeration.PriorityState state)
        {
            spriteState.Add(Enumeration.State.fastsheathe, state);
            sprite.PlayAnimation(spriteSequence, spriteState);
        }

        public void Fastheathe()
        {
            spriteState.Add(Enumeration.State.fastsheathe, Enumeration.PriorityState.Normal);
            sprite.PlayAnimation(spriteSequence, spriteState);
        }

        public void Retreat()
        {
            spriteState.Add(Enumeration.State.retreat, Enumeration.PriorityState.Normal);
            sprite.PlayAnimation(spriteSequence, spriteState);
        }

        public void StrikeRetreat()
        {
            spriteState.Add(Enumeration.State.strikeret, Enumeration.PriorityState.Force);
            sprite.PlayAnimation(spriteSequence, spriteState);
        }


        //show splash hit
        public void Splash(bool player, GameTime gametime)
        {
            
            Splash splash = new Splash(Maze.player.myRoom, Position.Value, graphicsDevice, SpriteEffects.None, player);
            Maze.player.MyLevel.sprites.Add(splash);

        }

        public void RunJump()
        {
            SetAlignEdge();
            spriteState.Add(Enumeration.State.runjump);
            sprite.PlayAnimation(spriteSequence, spriteState);
        }

        public void PutOffSword()
        {
            spriteState.Add(Enumeration.State.resheathe);
            sprite.PlayAnimation(spriteSequence, spriteState);
            //Sword = true;
        }


        public void PickupSword()
        {
            spriteState.Add(Enumeration.State.pickupsword);
            sprite.PlayAnimation(spriteSequence, spriteState);
            sword = true;
            ((SoundEffect)Maze.Content[PoP.CONFIG_SOUNDS + "presentation"]).Play();
        }

        public void DrinkPotion()
        {
            spriteState.Add(Enumeration.State.drinkpotion);
            sprite.PlayAnimation(spriteSequence, spriteState);
            energy = livePoints;
        }

        public void Advance()
        {
            spriteState.Add(Enumeration.State.advance, Enumeration.PriorityState.Normal);
            sprite.PlayAnimation(spriteSequence, spriteState);
        }

        public void Strike()
        {
            Strike(Enumeration.PriorityState.Normal);
        }

        public void Strike(Enumeration.PriorityState priority)
        {
            spriteState.Add(Enumeration.State.strike, priority);
            sprite.PlayAnimation(spriteSequence, spriteState);
        }

        public void Hang()
        {
            //chek what kind of hang or hangstraight
            if (spriteState.Value().state == Enumeration.State.hang | spriteState.Value().state == Enumeration.State.hangstraight)
                return;

            if (TypeFrontOfBlock(this.face) == Enumeration.RETURN_COLLISION_WALL.FAR)
                spriteState.Add(Enumeration.State.hang);
            else
                spriteState.Add(Enumeration.State.hangstraight); //oscilla...

            sprite.PlayAnimation(spriteSequence, spriteState);
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
            sprite.PlayAnimation(spriteSequence, spriteState);
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
            sprite.PlayAnimation(spriteSequence, spriteState);
        }


        private void SetAlignEdge()
        {
            Rectangle playerBounds = _position.Bounding;
            Vector2 v2 = myRoom.getCenterTilePosition(playerBounds);

            Rectangle tileBounds = myRoom.GetBounds((int)v2.X, (int)v2.Y);
            Vector2 depth = RectangleExtensions.GetIntersectionDepth(playerBounds, tileBounds);
            Enumeration.TileType tileType = myRoom.GetType((int)v2.X, (int)v2.Y);

            if (face == SpriteEffects.FlipHorizontally)
            {
                if (depth.X < (-Tile.PERSPECTIVE - PLAYER_R_PENETRATION))
                {
                    //???? TO BE COMPLETED ???
                }
            }
        }

        public void Question()
        {
            if (spriteState.Value().state == Enumeration.State.hang | spriteState.Value().state == Enumeration.State.hangstraight)
            {
                Hang();
            }
        }

        public void JumpHangMed()
        {
            if (sprite.IsStoppable == false)
                return;

            spriteState.Add(Enumeration.State.jumphangMed);
            sprite.PlayAnimation(spriteSequence, spriteState);
        }

        public void JumpHangLong()
        {
            if (sprite.IsStoppable == false)
                return;

            spriteState.Add(Enumeration.State.jumphangLong);
            sprite.PlayAnimation(spriteSequence, spriteState);
        }

        public void ClimbUp()
        {
            if (sprite.IsStoppable == false)
                return;
            
            //check if i can hold on and climb
            spriteState.Add(isHoldOn());
            sprite.PlayAnimation(spriteSequence, spriteState);
        }

        private bool? IsDownOfBlock(bool isForClimbDown)
        {
            // Get the player's bounding rectangle and find neighboring tiles.
            Rectangle playerBounds = _position.Bounding;
            Vector4 v4 = myRoom.getBoundTiles(playerBounds);

            int x, y = 0;

            if (face == SpriteEffects.FlipHorizontally)
            { x = (int)v4.Y - 1; y = (int)v4.Z + 1; }
            else
            { x = (int)v4.X; y = (int)v4.W + 1; }

            Rectangle tileBounds = myRoom.GetBounds(x, y);
            Vector2 depth = RectangleExtensions.GetIntersectionDepth(playerBounds, tileBounds);
            Enumeration.TileCollision tileCollision = myRoom.GetCollision(x, y);
            Enumeration.TileType tileType = myRoom.GetType(x, y);

            if (tileType != Enumeration.TileType.block)
                return false;

            if (isForClimbDown == true)
                return true;

            if (face == SpriteEffects.FlipHorizontally)
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


        //WARNING routine buggedddd
        private bool isBehind(Rectangle tileBounds, Rectangle playerBounds)
        {
            if (face == SpriteEffects.FlipHorizontally)
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


        public void Reset()
        {
            Reset(startPosition, startFace);
        }

        /// <summary>
        /// Resets the player to life.
        /// </summary>
        /// <param name="position">The position to come to life at.</param>
        public void Reset(Vector2 position, SpriteEffects spriteEffect)
        {
            startPosition = position;
            myRoom = MyLevel.StartRoom();

            _position = new Position(new Vector2(graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height), new Vector2(Player.SPRITE_SIZE_X, Player.SPRITE_SIZE_Y));
            _position.X = position.X;
            _position.Y = position.Y;

            Velocity = Vector2.Zero;
            Energy = PoP.CONFIG_KID_START_ENERGY;

            face = spriteEffect;

            spriteState.Clear();


            Stand(Enumeration.PriorityState.Force);

        }

        public void CheckItemOnFloor()
        {
            Vector2 v2 = myRoom.getCenterTilePosition(_position.Bounding);

            int x = (int)v2.X;
            int y = (int)v2.Y;

            Tile t = myRoom.GetTile(x, y);

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

                myRoom.SubsTile(v2, Enumeration.TileType.floor);
            }

        }

        public void ClimbDown()
        {
            if (sprite.IsStoppable == false)
                return;

            bool isDownOfBlock = false;

            if (IsDownOfBlock(true) == true)
                isDownOfBlock = true;

            spriteState.Add(Enumeration.State.climbdown, isDownOfBlock);
            sprite.PlayAnimation(spriteSequence, spriteState);
        }


        public void StandUp()
        { StandUp(Enumeration.PriorityState.Normal); }
        public void StandUp(Enumeration.PriorityState priority)
        {
            spriteState.Add(Enumeration.State.standup, priority);
            sprite.PlayAnimation(spriteSequence, spriteState);
            spriteState.Add(Enumeration.State.stand, priority);
        }

        public void Stoop()
        { Stoop(Enumeration.PriorityState.Normal); }
        public void Stoop(Vector2 offSet)
        { Stoop(Enumeration.PriorityState.Normal, offSet); }
        public void Stoop(Enumeration.PriorityState priority)
        { Stoop(Enumeration.PriorityState.Normal, Vector2.Zero); }
        public void Stoop(Enumeration.PriorityState priority, Vector2 offSet)
        {
            if (TypeFrontOfBlock(face) == Enumeration.RETURN_COLLISION_WALL.TOUCH)
            {
                Bump();
                return;
            }

            if (isClimbableDown() == true)
            {
                ClimbDown();
                return;
            }
            spriteState.Add(Enumeration.State.stoop, priority, null, Enumeration.SequenceReverse.Normal, offSet);
            sprite.PlayAnimation(spriteSequence, spriteState);
            spriteState.Add(Enumeration.State.crouch, priority);
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
            sprite.PlayAnimation(spriteSequence, spriteState);
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
            sprite.PlayAnimation(spriteSequence, spriteState);
            spriteState.Add(Enumeration.State.stand, Enumeration.PriorityState.Normal);
        }


        public void StandJump()
        { StandJump(Enumeration.PriorityState.Normal); }
        public void StandJump(Enumeration.PriorityState priority)
        {
            spriteState.Add(Enumeration.State.standjump, priority);
            sprite.PlayAnimation(spriteSequence, spriteState);
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
            PositionFall = Position.Value;
            spriteState.Add(Enumeration.State.stepfall, priority, offSet);
            sprite.PlayAnimation(spriteSequence, spriteState);
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
            sprite.PlayAnimation(spriteSequence, spriteState);

            //isPressable();
        }

        public void ClimbFail()
        {
            spriteState.Add(Enumeration.State.climbfail);
            sprite.PlayAnimation(spriteSequence, spriteState);
        }

        public void HangDrop()
        {
            spriteState.Add(Enumeration.State.hangdrop);
            sprite.PlayAnimation(spriteSequence, spriteState);
        }

        public void RunTurn()
        {
            if (face != SpriteEffects.FlipHorizontally)
                face = SpriteEffects.None;
            else
                face = SpriteEffects.FlipHorizontally;

            spriteState.Add(Enumeration.State.runturn);
            sprite.PlayAnimation(spriteSequence, spriteState);
        }

        public void Impale(Enumeration.PriorityState priority)
        {
            spriteState.Add(Enumeration.State.impale, priority);
            sprite.PlayAnimation(spriteSequence, spriteState);
            Energy = 0;
        }

        public void Impale()
        {
            Impale(Enumeration.PriorityState.Normal);
        }

        public void Turn()
        {
            if (face == SpriteEffects.FlipHorizontally)
                face = SpriteEffects.None;
            else
                face = SpriteEffects.FlipHorizontally;

            spriteState.Add(Enumeration.State.turn);
            sprite.PlayAnimation(spriteSequence, spriteState);
            spriteState.Add(Enumeration.State.stand);

        }

        public void RunStop()
        {
            RunStop(Enumeration.PriorityState.Normal);
        }

        public void RunStop(Enumeration.PriorityState priority)
        {
            spriteState.Add(Enumeration.State.runstop);
            sprite.PlayAnimation(spriteSequence, spriteState);
            spriteState.Add(Enumeration.State.stand);
        }

        private Enumeration.RETURN_COLLISION_WALL TypeFrontOfBlock(SpriteEffects flip)
        {
            int x, y = 0;
            Enumeration.RETURN_COLLISION_WALL bReturn = Enumeration.RETURN_COLLISION_WALL.FAR;
            int player_penetration = 0;
            Rectangle playerBounds = _position.Bounding;
            Vector4 v4 = myRoom.getBoundTiles(playerBounds);

            if (flip == SpriteEffects.FlipHorizontally)
            { x = (int)v4.Y; y = (int)v4.Z; }
            else
            { x = (int)v4.X; y = (int)v4.W; }

            //Rectangle tileBounds = myRoom.GetBounds(x, y);
            Tile myTile = myRoom.GetTile(x, y);
            Vector2 depth = RectangleExtensions.GetIntersectionDepth(playerBounds, myTile.Position.Bounding);
            Enumeration.TileCollision tileCollision = myRoom.GetCollision(x, y);
            Enumeration.TileType tileType = myRoom.GetType(x, y);

            if (tileType != Enumeration.TileType.block)
            {
                return bReturn = Enumeration.RETURN_COLLISION_WALL.FAR;
            }

            if (flip == SpriteEffects.FlipHorizontally)
                player_penetration = -PLAYER_R_PENETRATION;
            else
                player_penetration = +PLAYER_L_PENETRATION;

            if (flip == SpriteEffects.FlipHorizontally)
            {
                if (depth.X <= -18 & depth.X > (-Tile.PERSPECTIVE + player_penetration)) //18*3 = 54 step forward....
                    bReturn = Enumeration.RETURN_COLLISION_WALL.NEAR;
                else if (depth.X <= (-Tile.PERSPECTIVE + player_penetration))
                    bReturn = Enumeration.RETURN_COLLISION_WALL.TOUCH;
                else
                    bReturn = Enumeration.RETURN_COLLISION_WALL.FAR;
            }
            else
            {
                if (depth.X >= -18 & depth.X < (+Tile.PERSPECTIVE + player_penetration)) //18*3 = 54 step forward....
                    bReturn = Enumeration.RETURN_COLLISION_WALL.NEAR;
                else if (depth.X >= (-Tile.PERSPECTIVE + player_penetration))
                    bReturn = Enumeration.RETURN_COLLISION_WALL.TOUCH;
                else
                    bReturn = Enumeration.RETURN_COLLISION_WALL.FAR;
            }

            return bReturn;
        }

        private int GetTileEdgeDistance()
        {
            int distance = 0;
            Rectangle playerBounds = _position.Bounding;
            Tile tile = myRoom.getCenterTile(playerBounds);
            Vector2 depth = RectangleExtensions.GetIntersectionDepth(playerBounds, tile.Position.Bounding);
            if (face == SpriteEffects.FlipHorizontally)
            {
                distance = (tile.Position.Bounding.X + Tile.WIDTH) - (playerBounds.Center.X - Tile.PERSPECTIVE + PLAYER_STAND_FLOOR_PEN + 1); //Right isnt correct for bound tile
            }
            else
            {
                distance = (playerBounds.Center.X + Tile.PERSPECTIVE + PLAYER_STAND_FLOOR_PEN) - (tile.Position.Bounding.X + Tile.WIDTH); //Right isnt correct for bound tile
            }

            return distance;
        }


        public void StartRunning()
        {
            //TODO: i will check if there a wall and do a BUMP...
            Enumeration.RETURN_COLLISION_WALL isFront = TypeFrontOfBlock(this.face);
            if (isFront == Enumeration.RETURN_COLLISION_WALL.NEAR)
            {
                //step to the limit
                int distance = GetTileEdgeDistance();
                StepForward(distance);
                return;
            }
            else if (isFront == Enumeration.RETURN_COLLISION_WALL.TOUCH)
            {
                //int x = GetTileEdgeDistance();
                Bump(Enumeration.PriorityState.Normal, Enumeration.SequenceReverse.Normal);
                return;
            }


            spriteState.Add(Enumeration.State.startrun);
            sprite.PlayAnimation(spriteSequence, spriteState);
        }


        public void StepForward(int lenghtStep)
        {
            if (sprite.IsStoppable == false)
                return;

            Vector2 vDistance = new Vector2(lenghtStep, 0);
            spriteState.Add(Enumeration.State.fullstep, Enumeration.PriorityState.Normal, Vector2.Zero, vDistance);
            sprite.PlayAnimation(spriteSequence, spriteState);
        }

        public void TestFoot(int lenghtStep)
        {
            if (sprite.IsStoppable == false)
                return;

            Vector2 vDistance = new Vector2(lenghtStep, 0);
            spriteState.Add(Enumeration.State.testfoot, Enumeration.PriorityState.Normal, Vector2.Zero, vDistance);
            sprite.PlayAnimation(spriteSequence, spriteState);
        }

        public void TestFoot()
        {
            TestFoot(0);
        }

        public void StepForward()
        {
            if (sprite.IsStoppable == false)
                return;

            if (isNearEdge() == true)
            {
                int lenght = GetTileEdgeDistance();
                if (lenght != 0)
                {
                    StepForward(lenght);
                    return;
                }
            }

            //check if is loosable is yes i must rewind
            if (isLoose() == true)
            {
                int lenght = GetTileEdgeDistance();
                if (lenght != 0)
                {
                    StepForward(lenght);
                    looseWarning = false;
                    return;
                }

                if (looseWarning == false)
                {
                    TestFoot();
                    looseWarning = true;
                    return;
                }

                looseWarning = false;
                StepFall();
                return;
            }

            //TODO: i will check if there a wall and do a BUMP...
            Enumeration.RETURN_COLLISION_WALL isFront = TypeFrontOfBlock(this.face);
            if (isFront == Enumeration.RETURN_COLLISION_WALL.NEAR)
            {
                int lenght = GetTileEdgeDistance();
                StepForward(lenght);
                return;
            }
            else if (isFront == Enumeration.RETURN_COLLISION_WALL.TOUCH)
            {
                Bump(Enumeration.PriorityState.Normal, Enumeration.SequenceReverse.Normal);
                return;
            }
            else
                spriteState.Add(Enumeration.State.fullstep);


            sprite.PlayAnimation(spriteSequence, spriteState);
        }


        public void Engarde()
        { Engarde(Enumeration.PriorityState.Normal, null); }

        public void Engarde(bool? stoppable)
        { Engarde(Enumeration.PriorityState.Normal, stoppable); }

        public void Engarde(Enumeration.PriorityState priority, bool? stoppable)
        { Engarde(priority, stoppable, Vector2.Zero); }

        public void Engarde(Enumeration.PriorityState priority, bool? stoppable, Vector2 offset)
        {
            if (priority == Enumeration.PriorityState.Normal & sprite.IsStoppable == false)
                return;
            //TODO: ??? to be moved on calling routine??
            if (spriteState.Value().state == Enumeration.State.ready)
            { return; }
            //if (spriteState.Value().state == Enumeration.State.engarde)
            //{ return; }
            //if (spriteState.Value().state == Enumeration.State.advance)
            //{ return; }

            spriteState.Add(Enumeration.State.engarde, priority, stoppable, offset);
            sprite.PlayAnimation(spriteSequence, spriteState);
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
            sprite.PlayAnimation(spriteSequence, spriteState);
        }

        public void CrawlAndCrouch(Enumeration.PriorityState priority)
        {
            spriteState.Add(Enumeration.State.bump, priority, false);
            sprite.PlayAnimation(spriteSequence, spriteState);
            spriteState.Add(Enumeration.State.crouch, Enumeration.PriorityState.Normal);
        }

        public void Crawl(Enumeration.PriorityState priority)
        {
            spriteState.Add(Enumeration.State.crawl, priority);
            sprite.PlayAnimation(spriteSequence, spriteState);
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
            sprite.PlayAnimation(spriteSequence, spriteState);

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
            sprite.PlayAnimation(spriteSequence, spriteState);

        }

        public void Advance(Position position, SpriteEffects flip)
        {
            if (flip == SpriteEffects.FlipHorizontally)
                this.face = SpriteEffects.None;
            else
                this.face = SpriteEffects.FlipHorizontally;


            if (position.X + Sprite.SPRITE_SIZE_X - 30 <= Position.X)
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

            sprite.PlayAnimation(spriteSequence, spriteState);

        }

        public void Crawl()
        {
            Crawl(Enumeration.PriorityState.Normal);
        }

        /// <summary>
        /// Remnber: for example the top gate is x=3 y=1
        /// first row bottom y = 2 the top row y = 0..
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
            Vector2 v2 = myRoom.getCenterTilePosition(playerBounds);

            int x = (int)v2.X;
            int y = (int)v2.Y;

            Enumeration.TileCollision tileCollision; 

            if (face == SpriteEffects.FlipHorizontally)
                tileCollision = myRoom.GetCollision(x - 1, y);
            else
                tileCollision = myRoom.GetCollision(x + 1, y);

            if (tileCollision != Enumeration.TileCollision.Passable)
            {
                return false;
            }

            //check is platform or gate forward up
            int xOffSet = 0;
            if (face == SpriteEffects.FlipHorizontally)
            { xOffSet = -Tile.REALWIDTH + (Tile.PERSPECTIVE * 2); }
            else
            { xOffSet = -Tile.PERSPECTIVE; }

            Rectangle tileBounds = myRoom.GetBounds(x, y + 1);
            _position.Value = new Vector2(tileBounds.Center.X + xOffSet, _position.Y);
            return true;
        }


        /// <summary>
        /// This is used when sprite is grapped on the platform edge 
        /// and want to climbup the platform over he.
        /// </summary>
        /// <returns>
        /// the state ClimbUp or ClimbFail (if there isn't space like a gate closed)
        /// </returns>
        private Enumeration.State isHoldOn()
        {
            Rectangle playerBounds = _position.Bounding;
            Vector2 v2 = myRoom.getCenterTilePosition(playerBounds);

            int x = (int)v2.X;
            int y = (int)v2.Y;

            if (face == SpriteEffects.FlipHorizontally)
            { x = x + 1; }
            else
            { x = x - 1; }

            Tile t = myRoom.GetTile(new Vector2(x, y));
            if (t.Type == Enumeration.TileType.gate & t.tileState.Value().state == Enumeration.StateTile.closed)
            {
                return Enumeration.State.climbfail;
            }   
            else
                return Enumeration.State.climbup;
        }

        /// <summary>
        /// Remnber: for example the top gate is x=3 y=1
        /// first row bottom y= 2 the top row y = 0..
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        private Enumeration.State isClimbable()
        {

            // Get the player's bounding rectangle and find neighboring tiles.
            Rectangle playerBounds = _position.Bounding;
            Vector2 v2 = myRoom.getCenterTilePosition(playerBounds);

            int x = (int)v2.X;
            int y = (int)v2.Y;

            Enumeration.TileCollision tileCollision = myRoom.GetCollision(x, y - 1);

            if (tileCollision == Enumeration.TileCollision.Platform)
            {
                //CHECK KID IS UNDER THE FLOOR CHECK NEXT TILE
                if (face == SpriteEffects.FlipHorizontally)
                { x = x - 1; }
                else
                { x = x + 1; }

                tileCollision = myRoom.GetCollision(x, y - 1);
                if (tileCollision != Enumeration.TileCollision.Passable)
                    return Enumeration.State.none;

                x = ((int)v2.X); //THE FLOOR FOR CLIMB UP
            }
            else if (tileCollision == Enumeration.TileCollision.Passable)
            {
                if (face == SpriteEffects.FlipHorizontally)
                { x = x + 1; }
                else
                { x = x - 1; }

                tileCollision = myRoom.GetCollision(x, y - 1);
                if (tileCollision != Enumeration.TileCollision.Platform)
                    return Enumeration.State.none;
            }
            else
                return Enumeration.State.none;

            //check is platform or gate forward up
            int xOffSet = 0;
            if (face == SpriteEffects.FlipHorizontally)
            { xOffSet = -Tile.REALWIDTH + Tile.PERSPECTIVE; }

            if (face == SpriteEffects.FlipHorizontally)
                tileCollision = myRoom.GetCollision(x - 1, y);
            else
                tileCollision = myRoom.GetCollision(x + 1, y);

            Rectangle tileBounds;
            
            if (tileCollision == Enumeration.TileCollision.Passable & face != SpriteEffects.FlipHorizontally)
            {
                tileBounds = myRoom.GetBounds(x, y);
                xOffSet = -26;
                _position.Value = new Vector2(tileBounds.Center.X + xOffSet, _position.Y);
                return Enumeration.State.jumphangMed;
            }

            tileBounds = myRoom.GetBounds(x, y - 1);
            _position.Value = new Vector2(tileBounds.Center.X + xOffSet, _position.Y);
            return Enumeration.State.jumphangLong;

        }

        //removed not used TO BE DELETED!!!!!!
        protected bool isLoosable_old()
        {
            // Get the player's bounding rectangle and find neighboring tiles.
            Rectangle playerBounds = _position.Bounding;
            Vector2 v2 = myRoom.getCenterTilePosition(playerBounds);

            int x = (int)v2.X;
            int y = (int)v2.Y;


            //Rectangle tileBounds = _room.GetBounds(x, y);
            //Vector2 depth = RectangleExtensions.GetIntersectionDepth(playerBounds, tileBounds);
            Enumeration.TileCollision tileCollision = myRoom.GetCollision(x, y - 1);
            Enumeration.TileType tileType; // = _room.GetType(x, y - 1);


            //if (tileType == Enumeration.TileType.floor | tileType == Enumeration.TileType.gate)
            if (tileCollision == Enumeration.TileCollision.Platform)
            {
                //CHECK KID IS UNDER THE FLOOR CHECK NEXT TILE
                Tile t = myRoom.GetTile(x, y - 1);
                if (t.Type != Enumeration.TileType.loose)
                {
                    return false;
                }
                ((Loose)t).Press();


            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        protected bool isLoose()
        {
            // Get the player's bounding rectangle and find neighboring tiles.
            Rectangle playerBounds = _position.Bounding;
            Vector2 v2 = myRoom.getCenterTilePosition(playerBounds);

            int x = (int)v2.X;
            int y = (int)v2.Y;

            if (face == SpriteEffects.FlipHorizontally)
            { x = x + 1; }
            else
            { x = x - 1; }

            Enumeration.TileCollision tileCollision = myRoom.GetCollision(x, y);

            if (tileCollision != Enumeration.TileCollision.Platform)
            {
                return false;
            }

            Tile t = myRoom.GetTile(x, y);
            if (t.Type != Enumeration.TileType.loose)
            {
                return false;
            }
            ((Loose)t).Shake();
            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        protected bool isNearEdge()
        {
            // Get the player's bounding rectangle and find neighboring tiles.
            Rectangle playerBounds = _position.Bounding;
            Vector2 v2 = myRoom.getCenterTilePosition(playerBounds);

            int x = (int)v2.X;
            int y = (int)v2.Y;

            if (face == SpriteEffects.FlipHorizontally)
            { x = x + 1; }
            else
            { x = x - 1; }

            Enumeration.TileCollision tileCollision = myRoom.GetCollision(x, y);

            if (tileCollision != Enumeration.TileCollision.Passable)
            {
                return false;
            }

            Tile t = myRoom.GetTile(x, y);
            if (t.collision != Enumeration.TileCollision.Passable)
            {
                return false;
            }
            return true;
        }


        public void HandleCollisions()
        {
            if (IsAlive == false)
                return;

            CheckGround();

            HandleCollisionSprite();

            HandleCollisionTile();

        }

        private void HandleCollisionSprite()
        {

            //Check opposite sprite like guards..
            bool thereAreEnemy = false;
            string myName = this.GetType().Name;
            foreach (Sprite sprite in myRoom.SpritesInRoom())
            {
                if (myName == sprite.GetType().Name)
                    continue;

                switch (sprite.GetType().Name)
                {
                    case "Player":
                        {
                            thereAreEnemy = true;
                            if (sprite.IsAlive == false)
                            {
                                sprite.DeadFall();
                                break;
                            }
                            if (sprite.Position.CheckOnRow(Position))
                            {
                                if (sprite.Position.CheckOnRowDistancePixel(Position) >= 0 & sprite.Position.CheckOnRowDistancePixel(Position) <= 70 & Alert == true & spriteState.Value().state == Enumeration.State.strike)
                                {
                                    if (spriteState.Value().Name == Enumeration.State.strike.ToString())
                                    {
                                        //check if block
                                        if (sprite.spriteState.Value().Name != Enumeration.State.readyblock.ToString())
                                        {
                                            spriteState.Value().Name = string.Empty;
                                            sprite.Splash(true, null);

                                            sprite.Energy = sprite.Energy - 1;
                                            sprite.StrikeRetreat();
                                        }
                                        else
                                        {
                                            System.Console.WriteLine("P->" + Enumeration.State.readyblock.ToString());
                                        }
                                        //blocked
                                    }
                                    if (sprite.Energy == 0)
                                    { Fastheathe(); }

                                }

                                Alert = true;

                                //Chenge Flip player..
                                if (Position.X < sprite.Position.X)
                                    face = SpriteEffects.None;
                                else
                                    face = SpriteEffects.FlipHorizontally;

                                Advance(sprite.Position, face);






                            }
                            else
                                Alert = false;
                            break;
                        }

                    case "Guard":
                        {
                            if (sprite.IsAlive == false)
                                break;

                            thereAreEnemy = true;
                            if (sprite.Position.CheckOnRow(Position))
                            {

                                //ENGARDE
                                if (sprite.Position.CheckOnRowDistance(Position) >= 0 & sprite.Position.CheckOnRowDistance(Position) <= 3 & Alert == false)
                                {
                                    if (Sword == true)
                                    {
                                        Engarde(Enumeration.PriorityState.Force, true);
                                        Alert = true;
                                    }
                                    else
                                    {
                                        if (sprite.Position.CheckOnRowDistancePixel(Position) >= 0 & sprite.Position.CheckOnRowDistancePixel(Position) <= 70 & Alert == false)
                                        {
                                            Energy = 0;
                                            return;
                                        }
                                    }
                                }

                                //STRIKE/HIT
                                if (sprite.Position.CheckOnRowDistancePixel(Position) >= 0 & sprite.Position.CheckOnRowDistancePixel(Position) <= 70 & Alert == true)
                                {
                                    //Change Flip player..
                                    if (Position.X > sprite.Position.X)
                                        face = SpriteEffects.None;
                                    else
                                        face = SpriteEffects.FlipHorizontally;

                                    if (spriteState.Value().Name == Enumeration.State.strike.ToString())
                                    {
                                        if (sprite.spriteState.Value().Name != Enumeration.State.readyblock.ToString())
                                        {
                                            //RESET STRIKE
                                            spriteState.Value().Name = string.Empty;
                                            //GameTime g = null;
                                            sprite.Splash(false, null);
                                            sprite.Energy = sprite.Energy - 1;
                                            sprite.StrikeRetreat();
                                        }
                                        else
                                        {
                                            System.Console.WriteLine("G->" + Enumeration.State.readyblock.ToString());
                                        }
                                    }

                                    if (sprite.Energy == 0)
                                    { Fastheathe(); }

                                }
                            }
                            else
                            {
                                Alert = false;
                            }
                            break;
                        }
                    default:
                        break;
                }

            }
            if (thereAreEnemy == false & Alert == true)
            {
                Alert = false;
                Stand();
            }

        }

        private void HandleCollisionTile()
        {
            Rectangle playerBounds = _position.Bounding;
            //Find how many tiles are near on the left
            Vector4 v4 = myRoom.getBoundTiles(playerBounds);

            // For each potentially colliding Tile, warning the for check only the player row ground..W
            for (int y = (int)v4.Z; y <= (int)v4.W; ++y)
            {
                for (int x = (int)v4.X; x <= (int)v4.Y; ++x)
                {
                    //i use myRoom.GetBounds because it get actual room position
                    Rectangle tileBounds = myRoom.GetBounds(x, y);
                    Vector2 depth = RectangleExtensions.GetIntersectionDepth(playerBounds, tileBounds);
                    Enumeration.TileCollision tileCollision = myRoom.GetCollision(x, y);
                    Enumeration.TileType tileType = myRoom.GetType(x, y); ;
                    Tile tileCenter = myRoom.getCenterTile(playerBounds);

                    switch (tileType)
                    {
                        case Enumeration.TileType.chomper:
                            if (IsAlive == false)
                            {
                                ((Chomper)myRoom.GetTile(x, y)).Open();
                                return;
                            }

                            if (face == SpriteEffects.None)
                            {
                                if (depth.X < (-Tile.PERSPECTIVE - PLAYER_STAND_WALL_PEN)) //-58
                                    if (myRoom.GetTile(x, y).tileState.Value().Name == Enumeration.StateTile.kill.ToString())
                                    {
                                        DeadFall();
                                    }
                            }
                            else
                            {
                                if (depth.X < (-Tile.PERSPECTIVE - PLAYER_STAND_FRAME))  //-50...-46
                                    if (myRoom.GetTile(x, y).tileState.Value().Name == Enumeration.StateTile.kill.ToString())
                                    {
                                        DeadFall();
                                    }
                            }

                            break;

                        case Enumeration.TileType.spikes:
                            if (IsAlive == false)
                            {
                                ((Spikes)myRoom.GetTile(x, y)).Open();
                                return;
                            }

                            if (depth.X < (-Tile.PERSPECTIVE - PLAYER_R_PENETRATION + Player.PLAYER_STAND_FRAME))
                                ((Spikes)myRoom.GetTile(x, y)).Open();
                            else if (depth.X > (Tile.PERSPECTIVE + PLAYER_L_PENETRATION - Player.PLAYER_STAND_FRAME)) //45
                                ((Spikes)myRoom.GetTile(x, y)).Open();

                            if (depth.Y >= 0)
                            {
                                if (myRoom.GetTile(x, y).tileState.Value().state == Enumeration.StateTile.open)
                                {
                                    if (this.spriteState.Value().state == Enumeration.State.bump | this.spriteState.Value().state == Enumeration.State.crouch)
                                    {
                                        Impale(Enumeration.PriorityState.Force);
                                        return;
                                    }
                                }
                                if (myRoom.GetTile(x, y).tileState.Value().state == Enumeration.StateTile.opened)
                                {
                                    if (this.spriteState.Value().state == Enumeration.State.bump | this.spriteState.Value().state == Enumeration.State.crouch)
                                    {
                                        Impale(Enumeration.PriorityState.Force);
                                        return;
                                    }
                                    if (this.spriteState.Value().state == Enumeration.State.startrun)
                                    {
                                        Impale(Enumeration.PriorityState.Force);
                                        return;
                                    }
                                }
                            }

                            break;

                        case Enumeration.TileType.loose:
                            if (depth.X < (-Tile.PERSPECTIVE - PLAYER_R_PENETRATION))
                                ((Loose)myRoom.GetTile(x, y)).Press();
                            else if (depth.X > (Tile.PERSPECTIVE + PLAYER_L_PENETRATION)) //45
                                ((Loose)myRoom.GetTile(x, y)).Press();
                            //dont usefull?!?!?!
                            //else
                            //    isLoosable();
                            break;

                        case Enumeration.TileType.pressplate:
                            if (depth.X < (-Tile.PERSPECTIVE - PLAYER_R_PENETRATION))
                                ((PressPlate)myRoom.GetTile(x, y)).Press();
                            else if (depth.X > (Tile.PERSPECTIVE + PLAYER_L_PENETRATION)) //45
                                ((PressPlate)myRoom.GetTile(x, y)).Press();
                            break;

                        case Enumeration.TileType.exit:
                            if (depth.X < (-Tile.PERSPECTIVE - PLAYER_R_PENETRATION))
                                if (((Exit)myRoom.GetTile(x, y)).State == Enumeration.StateTile.opened)
                                {
                                    ((Exit)myRoom.GetTile(x, y)).ExitLevel();
                                    Maze.NextLevel();
                                }
                                else if (depth.X > (Tile.PERSPECTIVE + PLAYER_L_PENETRATION)) //45
                                {
                                    if (((Exit)myRoom.GetTile(x, y)).State == Enumeration.StateTile.opened)
                                    {
                                        ((Exit)myRoom.GetTile(x, y)).ExitLevel();
                                        Maze.NextLevel();
                                    }
                                }



                            break;

                        case Enumeration.TileType.gate:
                        case Enumeration.TileType.block:
                            if (tileType == Enumeration.TileType.gate)
                                if (((Gate)myRoom.GetTile(x, y)).State == Enumeration.StateTile.opened)
                                    break;



                            //if sx wall i will penetrate..for perspective design
                            if (face == SpriteEffects.FlipHorizontally)
                            {
                                //only for x pixel 
                                if (depth.X < (-Tile.PERSPECTIVE - PLAYER_R_PENETRATION))
                                {
                                    {
                                        _position.Value = new Vector2(_position.X + (depth.X - (-Tile.PERSPECTIVE - PLAYER_R_PENETRATION)), _position.Y); //WALLTOUCH
                                        if (spriteState.Value().Raised == false)
                                            Bump(Enumeration.PriorityState.Force);
                                        else
                                        {
                                            if (IsOnGround == true)
                                            {
                                                if (isClimbing == false)
                                                    CrawlAndCrouch(Enumeration.PriorityState.Force);
                                                else
                                                { }
                                            }
                                            else
                                            {
                                                if (isClimbing == false)
                                                    if (spriteState.Value().state != Enumeration.State.freefall)
                                                        StepFall(Enumeration.PriorityState.Normal);
                                            }
                                        }
                                        return;
                                    }

                                }
                                else
                                {
                                    if (spriteState.Value().Raised == true)
                                        _position.Value = new Vector2(_position.X, _position.Y);
                                }
                            }
                            else
                            {
                                if (depth.X > (Tile.PERSPECTIVE + PLAYER_L_PENETRATION)) //45
                                {
                                    {
                                        _position.Value = new Vector2(_position.X + (depth.X - (+Tile.PERSPECTIVE + PLAYER_L_PENETRATION)), _position.Y); //WALLTOUCH
                                        if (spriteState.Value().Raised == false)
                                        {
                                            Bump(Enumeration.PriorityState.Force);
                                        }
                                        else
                                        {
                                            //i must check if under me there's a floor
                                            if (IsOnGround == true)
                                            {
                                                if (isClimbing == false)
                                                    CrawlAndCrouch(Enumeration.PriorityState.Force);
                                                else
                                                { }
                                            }
                                            else
                                            {
                                                if (isClimbing == false)
                                                    if (spriteState.Value().state != Enumeration.State.freefall)
                                                        StepFall(Enumeration.PriorityState.Normal);
                                            }

                                        }
                                    }
                                }
                                else
                                    if (spriteState.Value().Raised == true)
                                        _position.Value = new Vector2(_position.X, _position.Y);
                            }
                            break;

                        //default:
                        //    _position.Value = new Vector2(_position.X, tileBounds.Bottom);
                        //    playerBounds = BoundingRectangle;
                        //    break;
                    }

                }
            }


            //only the player can change room
            if (this.GetType().Name == "Player")
            {

                if (_position.Y > Room.BOTTOM_LIMIT + 10)
                {
                    myRoom = myRoom.Down;
                    _position.Y = Room.TOP_LIMIT + 27; // Y=77
                    //For calculate height fall from damage points calculations..
                    PositionFall = new Vector2(Position.X, (PoP.CONFIG_SCREEN_HEIGHT - Room.BOTTOM_LIMIT - PositionFall.Y));


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

}
