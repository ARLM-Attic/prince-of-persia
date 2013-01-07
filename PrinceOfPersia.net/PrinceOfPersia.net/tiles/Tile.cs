﻿using System;
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

    public enum TileType
    {
        space = 0,
        floor = 1,
        spikes = 2,
        posts = 3,
        gate = 4,
        dpressplate = 5, //;down
        pressplate = 6,// ;up
        panelwif = 7,// ;w/floor
        pillarbottom = 8,
        pillartop = 9,
        flask = 10,
        loose = 11,
        panelwof = 12,// ;w/o floor
        mirror = 13,
        rubble = 14,
        upressplate = 15,
        exit = 16,
        exit2 = 17,
        slicer = 18,
        torch = 19,
        block = 20,
        bones = 21,
        sword = 22,
        window = 23,
        window2 = 24,
        archbot = 25,
        archtop1 = 26,
        archtop2 = 27,
        archtop3 = 28,
        archtop4 = 29,
        door = 30 //AMF 
    }
    
    public enum TileCollision
    {
        /// <summary>
        /// A passable tile is one which does not hinder player motion at all, like a space
        /// </summary>
        Passable = 0,

        /// <summary>
        /// An impassable tile is one which does not allow the player to move through
        /// it at all. It is completely solid, like a wall block
        /// </summary>
        Impassable = 1,

        /// <summary>
        /// Standard floor
        /// </summary>
        Platform = 2,
    }
    



    public class Tile
    {
        public Texture2D Texture;
        //private float Depth = 0.1f;
        public TileCollision collision;
        public TileType Type;
        public AnimationSequence tileAnimation = new AnimationSequence();
        //private StateTileElement stateTileElement = new StateTileElement();
        public TileState tileState = new TileState();

        //static for share purposes
        private static List<Sequence> tileSequence = new List<Sequence>();

        public static int GROUND = 20; //20;//18; //Height of the floor ground
        public static int WIDTH = 64; //used for build the grid of room
        public static int HEIGHT= 126; //used for build the grid of room 
        public static int REALHEIGHT = 148; //used for build view of romm
        public static int REALWIDTH = 128; //used for build view of romm
        public static int PERSPECTIVE = 26; //26 isometric shift x right
        public static readonly Vector2 Size = new Vector2(WIDTH, HEIGHT);

        private SpriteEffects flip = SpriteEffects.None;
        //private List<Tile> _tileReference = new List<Tile>();
        //protected List<Tile> tileReference
        //{
        //    get { return _tileReference; }
        //    set { _tileReference = value; }
        //}

        private Maze maze;

        private Position _position;
        public Position Position
        {
            get { return _position; }
            set { _position = value; }
        }


        public Tile()
        {}

        public Tile(Maze maze, ContentManager Content, TileType tileType, string state)
        {
            this.maze = maze;

            System.Xml.Serialization.XmlSerializer ax = new System.Xml.Serialization.XmlSerializer(tileSequence.GetType());
            Stream astream = this.GetType().Assembly.GetManifestResourceStream("PrinceOfPersia.resources." + tileType.ToString().ToUpper() + "_sequence.xml");
            tileSequence = (List<Sequence>)ax.Deserialize(astream);

            foreach (Sequence s in tileSequence)
            {
                s.Initialize(Content);
            }

            //Search in the sequence the right type
            Sequence result = tileSequence.Find(delegate(Sequence s)
            {
                //return s.name == tileType.ToString().ToUpper();
                return s.name == state;
            });

            if (result != null)
            {
                //AMF to be adjust....
                result.frames[0].texture = Content.Load<Texture2D>(result.frames[0].value);

                collision = result.collision;
                Texture = result.frames[0].texture;
            }
            Type = tileType;


            //change statetile element
            StateTileElement stateTileElement = new StateTileElement();
            stateTileElement.state = (StateTileElement.State)Enum.Parse(typeof(StateTileElement.State), state.ToLower());
            tileState.Add(stateTileElement);
            tileAnimation.PlayAnimation(tileSequence, tileState.Value());
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
            if (this.GetType() == typeof(Door))
            {
                ((Door)this).elapsedTimeOpen += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (((Door)this).elapsedTimeOpen > ((Door)this).timeOpen)
                    ((Door)this).Close(); 
            }

            if (this.GetType() == typeof(PressPlate))
            {
                ((PressPlate)this).elapsedTimeOpen += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (((PressPlate)this).elapsedTimeOpen > ((PressPlate)this).timeOpen & ((PressPlate)this).State == StateTileElement.State.dpressplate)
                    ((PressPlate)this).Normal();
            }

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            tileAnimation.UpdateFrameTile(elapsed, ref _position, ref flip, ref tileState);

        }
    }
}
