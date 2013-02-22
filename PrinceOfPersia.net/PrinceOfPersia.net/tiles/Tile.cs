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

    public class Tile
    {
        public Texture2D Texture;
        //private float Depth = 0.1f;
        public Enumeration.TileCollision collision;
        public Enumeration.TileType Type;
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

        //private Maze maze;
        protected RoomNew room;

        private Position _position;
        public Position Position
        {
            get { return _position; }
            set { _position = value; }
        }


        public Tile()
        {}

        public Tile(RoomNew room, ContentManager Content, Enumeration.TileType tileType, string state)
        {
            this.room = room;

            System.Xml.Serialization.XmlSerializer ax = new System.Xml.Serialization.XmlSerializer(tileSequence.GetType());
            TextReader txtReader = File.OpenText(PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_SEQUENCES + tileType.ToString().ToUpper() + "_sequence.xml");
            tileSequence = (List<Sequence>)ax.Deserialize(txtReader);

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
                result.frames[0].SetTexture(Content.Load<Texture2D>(result.frames[0].value));

                collision = result.collision;
                Texture = result.frames[0].texture;
            }
            Type = tileType;


            //change statetile element
            StateTileElement stateTileElement = new StateTileElement();
            stateTileElement.state = (Enumeration.StateTile)Enum.Parse(typeof(Enumeration.StateTile), state.ToLower());
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
            HandleCollision();

            if (this.GetType() == typeof(Spikes))
            {
                ((Spikes)this).elapsedTimeOpen += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (((Spikes)this).elapsedTimeOpen > ((Spikes)this).timeOpen)
                    ((Spikes)this).Close();
            }


            if (this.GetType() == typeof(Door))
            {
                ((Door)this).elapsedTimeOpen += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (((Door)this).elapsedTimeOpen > ((Door)this).timeOpen)
                    ((Door)this).Close(); 
            }

            if (this.GetType() == typeof(PressPlate))
            {
                ((PressPlate)this).elapsedTimeOpen += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (((PressPlate)this).elapsedTimeOpen > ((PressPlate)this).timeOpen & ((PressPlate)this).State == Enumeration.StateTile.dpressplate)
                    ((PressPlate)this).Normal();
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
                Rectangle tileBounds = room.GetBounds((int)v.X, (int)v.W);

                Vector2 depth = RectangleExtensions.GetIntersectionDepth(tileBounds, r);
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
                        Vector2 vs = new Vector2(this.Position.X, this.Position.Y);
                        if (tileType == Enumeration.TileType.loose)
                        {
                            Loose l = (Loose) room.GetTile((int)v.X, (int)v.W);
                            l.Fall(true);
                        }                        
                        else
                            room.SubsTile(vs, Enumeration.TileType.rubble);
                    }
                    
                }


                if (_position.Y >= RoomNew.BOTTOM_LIMIT - Tile.HEIGHT - Tile.PERSPECTIVE)
                {
                    //remove tiles from tilesTemporaney
                    lock (room.tilesTemporaney)
                    {
                        room.tilesTemporaney.Remove(this);
                    }
                    //exis from DOWN room
                    RoomNew roomDown = room.maze.DownRoom(room);
                    room = roomDown;
                    _position.Y = RoomNew.TOP_LIMIT - 10;

                    lock (room.tilesTemporaney)
                    {
                        room.tilesTemporaney.Add(this);
                    }
                }



            }
        }

    }

}