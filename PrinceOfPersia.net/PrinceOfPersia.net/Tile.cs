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
    }
    
    public enum TileCollision
    {
        /// <summary>
        /// A passable tile is one which does not hinder player motion at all.
        /// </summary>
        Passable = 0,

        /// <summary>
        /// An impassable tile is one which does not allow the player to move through
        /// it at all. It is completely solid.
        /// </summary>
        Impassable = 1,

        /// <summary>
        /// A platform tile is one which behaves like a passable tile except when the
        /// player is above it. A player can jump up through a platform as well as move
        /// past it to the left and right, but can not fall down through the top of it.
        /// </summary>
        Platform = 2,
       

    }
    



    class Tile
    {
        public Texture2D Texture;
        //private float Depth = 0.1f;
        public TileCollision Collision;
        public TileType Type;
        public AnimationSequence tileAnimation = new AnimationSequence();
        public StateTileElement stateTileElement = new StateTileElement();
        
        //static for share purposes
        private static List<Sequence> tileSequence = new List<Sequence>();

        public static int GROUND = 20; //20;//18; //Height of the floor ground
        public static int WIDTH = 64; //used for build the grid of room
        public static int HEIGHT= 126; //used for build the grid of room 
        public static int REALHEIGHT = 148; //used for build view of romm
        public static int REALWIDTH = 128; //used for build view of romm
        public static int PERSPECTIVE = 26; //26 isometric shift x right
        //38
        
        public static readonly Vector2 Size = new Vector2(WIDTH, HEIGHT);

        private SpriteEffects flip = SpriteEffects.None;
        public Position Position
        {
            get { return _position; }
            set { _position = value; }
        }
        private Position _position;
        public PlayerState playerState = new PlayerState();

        public Tile()
        {}

        public Tile(ContentManager Content, TileType tileType)
        {
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
                return s.name == "NORMAL";
            });

            if (result != null)
            {
                Collision = result.tileCollision;
                Texture = result.frames[0].texture;
            }
            Type = tileType;

            tileAnimation.PlayAnimation(tileSequence, stateTileElement);
            

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

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            tileAnimation.UpdateFrame(elapsed, ref _position, ref flip, ref playerState);

        }
    }
}
