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
using Microsoft.Xna.Framework.Input.Touch;
using System.IO;

namespace PrinceOfPersia
{
    public class Guard : Sprite
    {

        /// <summary>
        /// Constructors a new player.
        /// </summary>
        public Guard(RoomNew room, Vector2 position, GraphicsDevice GraphicsDevice, SpriteEffects spriteEffect)
        {
            graphicsDevice = GraphicsDevice;
            spriteRoom = room;
            LoadContent();
            

            //TAKE PLAYER Position
            Reset(position, spriteEffect);
        }

        /// Loads the player sprite sheet and sounds.
        /// </summary>
        /// <note>i will add a parameter read form app.config</note>
        /// 
        /// 
        private void LoadContent()
        {

            spriteSequence = new List<Sequence>();
            System.Xml.Serialization.XmlSerializer ax = new System.Xml.Serialization.XmlSerializer(spriteSequence.GetType());

            Stream txtReader = Microsoft.Xna.Framework.TitleContainer.OpenStream(PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_SEQUENCES + "GUARD_sequence.xml");


            //TextReader txtReader = File.OpenText(PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_SEQUENCES + "KID_sequence.xml");
            //Stream astream = this.GetType().Assembly.GetManifestResourceStream("PrinceOfPersia.resources.KID_sequence.xml");
            spriteSequence = (List<Sequence>)ax.Deserialize(txtReader);

            foreach (Sequence s in spriteSequence)
            {
                s.Initialize(SpriteRoom.content);
            }

            // Calculate bounds within texture size.         
            //faccio un rettangolo che sia largo la metà del frame e che parta dal centro
            int top = 0; //StandAnimation.FrameHeight - height - 128;
            int left = 0; //PLAYER_L_PENETRATION; //THE LEFT BORDER!!!! 19
            int width = 114;//(int)(StandAnimation.FrameWidth);  //lo divido per trovare punto centrale *2)
            int height = 114;//(int)(StandAnimation.FrameHeight);


            localBounds = new Rectangle(left, top, width, height);

            // Load sounds.            
            //killedSound = _room.content.Load<SoundEffect>("Sounds/PlayerKilled");
            //jumpSound = _room.content.Load<SoundEffect>("Sounds/PlayerJump");
            //fallSound = _room.content.Load<SoundEffect>("Sounds/PlayerFall");
        }

        /// <summary>
        /// Handles input, performs physics, and animates the player sprite.
        /// </summary>
        /// <remarks>
        /// We pass in all of the input states so that our game is only polling the hardware
        /// once per frame. We also pass the game's orientation because when using the accelerometer,
        /// we need to reverse our motion when the orientation is in the LandscapeRight orientation.
        /// </remarks>
        public void Update
        (
            GameTime gameTime,
            KeyboardState keyboardState,
            GamePadState gamePadState,
            TouchCollection touchState,
            AccelerometerState accelState,
            DisplayOrientation orientation)
        {
         

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            // TODO: Add your game logic here.
            sprite.UpdateFrame(elapsed, ref _position, ref flip, ref spriteState);
            
        }

        /// <summary>
        /// Draws the animated player.
        /// </summary>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //DRAW REAL COORDINATE
            //sprite.DrawNew(gameTime, spriteBatch, _position.Value, PositionArrive, flip, 0.5f);

            //DRAW SPRITE
            sprite.DrawSprite(gameTime, spriteBatch, _position.Value, flip, 0.5f);

        }

        /// <summary>
        /// Resets the player to life.
        /// </summary>
        /// <param name="position">The position to come to life at.</param>
        public void Reset(Vector2 position, SpriteEffects spriteEffect)
        {
            _position = new Position(new Vector2(graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height), new Vector2(Player.SPRITE_SIZE_X, Player.SPRITE_SIZE_Y));
            _position.X = position.X;
            _position.Y = position.Y;
            Velocity = Vector2.Zero;
            Energy = PrinceOfPersiaGame.CONFIG_KID_START_ENERGY;

            flip = spriteEffect;

            spriteState.Clear();

            Stand();

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

            spriteState.Add(Enumeration.State.stand, priority);
            sprite.PlayAnimation(spriteSequence, spriteState.Value());
            spriteState.Add(Enumeration.State.stand, Enumeration.PriorityState.Normal);

        }
    }
}
