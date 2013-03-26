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
        
        
        protected List<Sequence> spriteSequence = null;

        protected GraphicsDevice graphicsDevice;
        protected SpriteEffects flip = SpriteEffects.None;
        protected Position _position;
        protected Maze _maze;

        public AnimationSequence sprite;

        // Physics state, used by calculate falldrop distance
        public Vector2 PositionStart
        {
            get { return positionStart; }
            set { positionStart = value; }
        }
        Vector2 positionStart = Vector2.Zero;


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



        public Sprite()
        { 
        
        
        }
    }
}
