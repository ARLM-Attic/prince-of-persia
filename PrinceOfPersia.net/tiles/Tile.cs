﻿	//-----------------------------------------------------------------------//
	// <copyright file="Tile.cs" company="A.D.F.Software">
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

/**/

using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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

    public class Tile
    {
        public Enumeration.TileType nextTileType = Enumeration.TileType.space;

        public Texture2D Texture;
        //private float Depth = 0.1f;
        public Enumeration.TileCollision collision;
        public Enumeration.TileType Type;
        public AnimationSequence tileAnimation;
        //private StateTileElement stateTileElement = new StateTileElement();
        public TileState tileState = new TileState();
        //contains object like sword, potion...
        public Item item = null;


        public static int GROUND = 20; //20;//18; //Height of the floor ground
        public static int WIDTH = 64; //used for build the grid of room
        public static int HEIGHT = 126; //used for build the grid of room 
        public static int REALHEIGHT = 148; //used for build view of romm
        public static int REALWIDTH = 128; //used for build view of romm
        public static int PERSPECTIVE = 26; //26 isometric shift x right

        public static int CHOMPER_DISTANCE_PENETRATION_L = 14; //max distance of kid penetration on tile edge

        public static readonly Vector2 Size = new Vector2(WIDTH, HEIGHT);
        public static readonly int HEIGHT_VISIBLE = REALHEIGHT - HEIGHT - Room.TOP_BORDER; //is the real visible coordinate for the tile



        public static Rectangle MASK_FLOOR = new Rectangle(0, REALWIDTH, 62, GROUND); //floor 
        public static Rectangle MASK_POSTS = new Rectangle(0, 0, 54, REALHEIGHT); //gate
        public static Rectangle MASK_BLOCK = new Rectangle(0, 0, 64, REALHEIGHT); //block 
        public static Rectangle MASK_DOOR = new Rectangle(50, 0, 13, REALHEIGHT); //door
        public static Rectangle MASK_CHOMPER = new Rectangle(0, 0, PERSPECTIVE, REALHEIGHT); //chomper

        private SpriteEffects flip = SpriteEffects.None;

        //private Maze maze;
        protected Room room;
        public Room Room
        {
            get { return room; }
        }

        private Position _position;
        public Position Position
        {
            get { return _position; }
            set { _position = value; }
        }


        //to determine how to draw walls, calculate in room construction algorithm
        private int _panelInfo;
        public int panelInfo
        {
            get { return _panelInfo; }
            set { _panelInfo = value; }
        }


        public Vector2 Coordinates
        {
            get
            {
                int x = (int)Math.Floor((float)_position.X / Tile.WIDTH);
                int y = (int)Math.Ceiling(((float)(_position.Y) - Room.BOTTOM_BORDER) / Tile.HEIGHT);

                return new Vector2(x, y);
            }

        }

        //static for share purposes
        private static List<Sequence> tileSequence = new List<Sequence>();
        public List<Sequence> TileSequence
        {
            get { return tileSequence; }
        }



        public Tile()
        {
         tileAnimation = new AnimationSequence(this);
        }

        public Tile(Room room, Enumeration.TileType tileType, Enumeration.StateTile state, Enumeration.Items eitem, Enumeration.TileType NextTileType)
        {
            tileAnimation = new AnimationSequence(this);
            this.room = room;
            nextTileType = NextTileType;

            System.Xml.Serialization.XmlSerializer ax = new System.Xml.Serialization.XmlSerializer(tileSequence.GetType());

            Stream txtReader = Microsoft.Xna.Framework.TitleContainer.OpenStream(PoP.CONFIG_PATH_CONTENT + PoP.CONFIG_PATH_SEQUENCES + tileType.ToString() + "_sequence.xml");
            //TextReader txtReader = File.OpenText(PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_SEQUENCES + tileType.ToString() + "_sequence.xml");


            tileSequence = (List<Sequence>)ax.Deserialize(txtReader);

            foreach (Sequence s in tileSequence)
            {
                s.Initialize();
            }

            //Search in the sequence the right type
            Sequence result = tileSequence.Find(delegate(Sequence s)
            {
                return s.name.ToUpper() == state.ToString().ToUpper();
            });

            if (result != null)
            {
                //AMF to be adjust....
                result.frames[0].SetTexture((Texture2D)Maze.Content[PoP.CONFIG_TILES + result.frames[0].value]); 
                //result.frames[0].SetTexture(Content.Load<Texture2D>(PrinceOfPersiaGame.CONFIG_TILES + result.frames[0].value));

                collision = result.collision;
                Texture = result.frames[0].texture;
            }
            Type = tileType;


            //change statetile element
            StateTileElement stateTileElement = new StateTileElement();
            stateTileElement.state = state;
            tileState.Add(stateTileElement);
            tileAnimation.PlayAnimation(tileSequence, tileState);

            //load item
            switch (eitem)
            { 
                case Enumeration.Items.flask:
                    item = new Flask();
                    break;
                case Enumeration.Items.sword:
                    item = new Sword();
                    break;
            }

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
            HandleCollision();


            if (this.GetType() == typeof(Chomper))
            {
                ((Chomper)this).elapsedTimeOpen += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (((Chomper)this).elapsedTimeOpen > ((Chomper)this).timeOpen)
                    ((Chomper)this).Close();
            }

            if (this.GetType() == typeof(Spikes))
            {
                ((Spikes)this).elapsedTimeOpen += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (((Spikes)this).elapsedTimeOpen > ((Spikes)this).timeOpen)
                    ((Spikes)this).Close();
            }


            if (this.GetType() == typeof(Gate))
            {
                if (((Gate)this).infiniteOpen == true)
                    return;

                ((Gate)this).elapsedTimeOpen += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (((Gate)this).elapsedTimeOpen > ((Gate)this).timeOpen)
                    ((Gate)this).Close(); 
            }

            //REMAIN OPEN FOREVER!!!
            //if (this.GetType() == typeof(Exit))
            //{
            //    ((Exit)this).elapsedTimeOpen += (float)gameTime.ElapsedGameTime.TotalSeconds;
            //    if (((Exit)this).elapsedTimeOpen > ((Exit)this).timeOpen)
            //        ((Exit)this).Close();
            //}

            if (this.GetType() == typeof(PressPlate))
            {
                if (((PressPlate)this).infinitePress == true)
                    return;

                ((PressPlate)this).elapsedTimeOpen += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (((PressPlate)this).elapsedTimeOpen > ((PressPlate)this).timeOpen & ((PressPlate)this).State == Enumeration.StateTile.dpressplate)
                    ((PressPlate)this).DePress();
            }

            if (this.GetType() == typeof(Loose))
            {
                if (((Loose)this).tileState.Value().state == Enumeration.StateTile.loose)
                {
                    ((Loose)this).elapsedTimeOpen += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (((Loose)this).elapsedTimeOpen > ((Loose)this).timeFall)
                        ((Loose)this).Fall();
                }
            }

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            tileAnimation.UpdateFrameTile(elapsed, ref _position, ref flip, ref tileState);

        }

        public void HandleCollision()
        {

            if (this.Type == Enumeration.TileType.loose)
            {
                if (this.tileState.Value().state != Enumeration.StateTile.loosefall)
                    return;

                Rectangle r = new Rectangle((int)Position.X, (int) Position.Y, Tile.WIDTH, Tile.HEIGHT);
                Vector4 v = room.getBoundTiles(r);
                Tile myTile = room.GetTile((int)v.X, (int)v.W);
                //Rectangle tileBounds = room.GetBounds((int)v.X, (int)v.W);

                Vector2 depth = RectangleExtensions.GetIntersectionDepth(myTile.Position.Bounding, r);
                Enumeration.TileType tileType = room.GetType((int)v.X, (int)v.W);
                Enumeration.TileCollision tileCollision = room.GetCollision((int)v.X, (int)v.W);
                //Tile tile = room.GetTile(new Vector2((int)v.X, (int)v.W));
                if (tileCollision == Enumeration.TileCollision.Platform)
                //if (tileType == Enumeration.TileType.floor)
                {
                    if (depth.Y >= Tile.HEIGHT - Tile.PERSPECTIVE)
                    {
                        lock (room.tilesTemporaney)
                        {
                            room.tilesTemporaney.Remove(this);
                        }
                        //THE LOOSE FALL INTO FLOOR
                        if (tileType == Enumeration.TileType.loose)
                        {
                            Loose l = (Loose)room.GetTile((int)v.X, (int)v.W);
                            l.Fall(true);
                        }
                        else if (tileType == Enumeration.TileType.pressplate)
                        {
                            ((SoundEffect)Maze.Content[PoP.CONFIG_SOUNDS + "tile crashing into the floor"]).Play();
                            PressPlate pressPlate = (PressPlate)room.GetTile((int)v.X, (int)v.Y);
                            pressPlate.Press();
                        }
                        else
                        {
                            ((SoundEffect)Maze.Content[PoP.CONFIG_SOUNDS +"tile crashing into the floor"]).Play();
                            room.SubsTile(Coordinates, Enumeration.TileType.rubble);
                        }
                    }
                    
                }


                //if (_position.Y >= RoomNew.BOTTOM_LIMIT - Tile.HEIGHT - Tile.PERSPECTIVE)
                if (_position.Y >= Room.BOTTOM_LIMIT - Tile.PERSPECTIVE)
                {
                    //remove tiles from tilesTemporaney
                    lock (room.tilesTemporaney)
                    {
                        room.tilesTemporaney.Remove(this);
                    }
                    //exit from DOWN room
                    room = room.Down;
                    _position.Y = Room.TOP_LIMIT - 10;

                    lock (room.tilesTemporaney)
                    {
                        room.tilesTemporaney.Add(this);
                    }
                }



            }
        }

    }

}
