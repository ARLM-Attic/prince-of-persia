	//-----------------------------------------------------------------------//
	// <copyright file="Position.cs" company="A.D.F.Software">
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
using Microsoft.Xna.Framework;

namespace PrinceOfPersia
{

    /**
     * This is useful for distinguish the draw coordinate and
     * the grid coordinate
     *
     */

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

        //
        //This is the draw position with correct perspective and tile ground
        //is only applied for sprite
        //
        public Vector2 ValueDraw
        {
            get
            {
                //return _vector2;
                return new Vector2(_vector2.X + Tile.PERSPECTIVE, _vector2.Y - Tile.GROUND);
            }

        }

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

        public bool CheckCollision(Position p)
        {
            if (p.Y == Y)
            {
                if (p.X +10 > X & p.X -10 < X)
                {
                    return true;
                }
            }
            return false;
        }


        public bool CheckOnRow(Position p)
        {
            if (p.Y == Y)
            {
                return true;
            }
            return false;

        }

        public float CheckOnRowDistancePixel(Position p)
        {
            if (p.Y == Y)
            {
                float distance = Math.Abs(p.X - X);
                //int ret = ((int)distance) / Tile.WIDTH;
                return distance;
            }
            return -1;

        }


        public float CheckOnRowDistance(Position p)
        {
            if (p.Y == Y)
            {
                float distance = Math.Abs(p.X - X);
                int ret =  ((int)distance) / Tile.WIDTH;
                return ret;
            }
            return -1;

        }

        //53 pix total
        public Rectangle Bounding
        {
            get
            {
                return new Rectangle((int)_vector2.X, (int)_vector2.Y, (int)_spriteRealSize.X, (int)_spriteRealSize.Y);
            }
        }


    }
}
