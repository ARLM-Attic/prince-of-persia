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
        public const int PLAYER_STAND_FLOOR_PEN = 26; //floor border penetration
        public const int PLAYER_STAND_HANG_PEN = 46; //floor border penetration for hangup

        public Vector2 startPosition = Vector2.Zero; //where the sprite begins
        public SpriteEffects startFlip = SpriteEffects.None;


        protected List<Sequence> spriteSequence = null;
        protected GraphicsDevice graphicsDevice;
        public SpriteEffects flip = SpriteEffects.None;
        protected Position _position = null;
        //protected Maze _maze;
        protected Vector2 velocity;
        protected bool isOnGround;
        protected Rectangle localBounds;
        protected RoomNew spriteRoom = null;

        public AnimationSequence sprite;
        public PlayerState spriteState = new PlayerState();

        public Maze Maze
        {
            get
            {
                return spriteRoom.maze;
            }
        }


        // Physics state
        public RoomNew SpriteRoom
        {
            get
            {
                return spriteRoom;
            }
            set
            {
                spriteRoom = value;
            }
        }

        // Physics state, used by calculate falldrop distance
        public Vector2 PositionFall
        {
            get { return positionFall; }
            set { positionFall = value; }
        }
        protected Vector2 positionFall = Vector2.Zero;


        public bool IsAlive
        {
            get 
            {
                if (energy > 0)
                    return true;
                else
                    return false;
            }
            //set { isAlive = value; }
        }
        

        //How many energy triangle sprite have
        public int LivePoints
        {
            get { return livePoints; }
            set 
            { 
                livePoints = value;
            }
        }
        int livePoints = 3;

        //energy...
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
        int energy = 3;


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


        public bool CheckCollision(Position position)
        {
            if (position == Position)
                return true;
            return false;
        }

        public Sprite()
        { 
        
        
        }
    }
}
