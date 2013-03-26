using System;
using Microsoft.Xna.Framework;

namespace PrinceOfPersia
{
    public class Position
    {
        public Vector2 _screenRealSize;
        public Vector2 _spriteRealSize;
        private Vector2 _vector2;

        

        public Position(Vector2 screenRealSize, Vector2 spriteRealSize)
        {
            _screenRealSize = screenRealSize;
            _spriteRealSize = spriteRealSize;
            _vector2 = Vector2.Zero;
        }

        /// <summary>
        /// The DrawValue is differtent coordinate position because the drawing routing
        /// draw sprite on the left upper corner and the real screen room screen begin to BOTTOM_BORDER
        /// 
        /// </summary>
        /// <returns></returns>

        //public Vector2 DrawValue()
        //{ return new Vector2(_vector2.X, _vector2.Y); }




        public Vector2 Value
        {
            set
            {
                _vector2 = value;
            }
            get
            {
                return _vector2;
            }

        }

        

        //mask X and Y values of Vector2 structure 
        public float X
        {
            set 
            {
                _vector2.X = value; 
            }
            get 
            {
                return _vector2.X;
            }

        }

        public float Y
        {
            set 
            { 
                _vector2.Y = value; 
            }
            get 
            { 
                return _vector2.Y ; 
            
            }
        }


        //53 pix total
        public Rectangle Bounding
        {
            get
            {
                return new Rectangle((int)_vector2.X, (int)_vector2.Y, (int)_spriteRealSize.X, (int)_spriteRealSize.Y);
                //return new Rectangle((int)_vector2.X + Player.PLAYER_STAND_BORDER_FRAME, (int)_vector2.Y, (int)Player.PLAYER_STAND_FRAME, (int)_spriteRealSize.Y);
            }
        }


        
  


        ////example to mask a method of Vector2 structure 
        //public static float Dot(Position value1, Position value2)
        //{
        //    return Vector2.Dot(value1, value2);
        //}

        ////override the cast to Vector2
        //public static implicit operator Vector2(Position value) 
        //{
        //    return new Vector2(value.X, value.Y);
        //}

    }
}
