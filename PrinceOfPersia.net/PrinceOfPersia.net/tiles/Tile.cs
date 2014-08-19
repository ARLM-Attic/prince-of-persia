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
        public AnimationSequence tileAnimation = new AnimationSequence();
        //private StateTileElement stateTileElement = new StateTileElement();
        public TileState tileState = new TileState();
        //contains object like sword, potion...
        public Item item = null;


        public static int GROUND = 20; //20;//18; //Height of the floor ground
        public static int WIDTH = 64; //used for build the grid of room
        public static int HEIGHT= 126; //used for build the grid of room 
        public static int REALHEIGHT = 148; //used for build view of romm
        public static int REALWIDTH = 128; //used for build view of romm
        public static int PERSPECTIVE = 26; //26 isometric shift x right
        public static readonly Vector2 Size = new Vector2(WIDTH, HEIGHT);

        public static Rectangle MASK_FLOOR = new Rectangle(0, 128, 62, 20); //floor 
        public static Rectangle MASK_POSTS = new Rectangle(0, 0, 54, 148); //gate
        public static Rectangle MASK_BLOCK = new Rectangle(0, 0, 64, 148); //block 
        public static Rectangle MASK_DOOR = new Rectangle(50, 0, 13, 148); //door
        

        private SpriteEffects flip = SpriteEffects.None;

        //private Maze maze;
        protected Room room;

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
        {}

        public Tile(Room room, ContentManager Content, Enumeration.TileType tileType, Enumeration.StateTile state, Enumeration.Items eitem, Enumeration.TileType NextTileType)
        {
            this.room = room;
            nextTileType = NextTileType;

            System.Xml.Serialization.XmlSerializer ax = new System.Xml.Serialization.XmlSerializer(tileSequence.GetType());

            Stream txtReader = Microsoft.Xna.Framework.TitleContainer.OpenStream(PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_SEQUENCES + tileType.ToString().ToUpper() + "_sequence.xml");
            //TextReader txtReader = File.OpenText(PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_SEQUENCES + tileType.ToString().ToUpper() + "_sequence.xml");


            tileSequence = (List<Sequence>)ax.Deserialize(txtReader);

            foreach (Sequence s in tileSequence)
            {
                s.Initialize(Content);
            }

            //Search in the sequence the right type
            Sequence result = tileSequence.Find(delegate(Sequence s)
            {
                return s.name == state.ToString().ToUpper();
            });

            if (result != null)
            {
                //AMF to be adjust....
                result.frames[0].SetTexture(Content.Load<Texture2D>(PrinceOfPersiaGame.CONFIG_TILES + result.frames[0].value));

                collision = result.collision;
                Texture = result.frames[0].texture;
            }
            Type = tileType;


            //change statetile element
            StateTileElement stateTileElement = new StateTileElement();
            stateTileElement.state = state;
            tileState.Add(stateTileElement);
            tileAnimation.PlayAnimation(tileSequence, tileState.Value());

            //load item
            switch (eitem)
            { 
                case Enumeration.Items.flask:
                    item = new Flask(Content);
                    break;
                case Enumeration.Items.sword:
                    item = new Sword(Content);
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

            if (this.GetType() == typeof(Spikes))
            {
                ((Spikes)this).elapsedTimeOpen += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (((Spikes)this).elapsedTimeOpen > ((Spikes)this).timeOpen)
                    ((Spikes)this).Close();
            }


            if (this.GetType() == typeof(Gate))
            {
                ((Gate)this).elapsedTimeOpen += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (((Gate)this).elapsedTimeOpen > ((Gate)this).timeOpen)
                    ((Gate)this).Close(); 
            }

            if (this.GetType() == typeof(PressPlate))
            {
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
                        //Vector2 vs = new Vector2(this.Position.X, this.Position.Y);
                        if (tileType == Enumeration.TileType.loose)
                        {
                            Loose l = (Loose)room.GetTile((int)v.X, (int)v.W);
                            l.Fall(true);
                        }
                        else
                        {
                            ((SoundEffect)Maze.dContentRes[PrinceOfPersiaGame.CONFIG_SOUNDS +"tile crashing into the floor".ToUpper()]).Play();
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