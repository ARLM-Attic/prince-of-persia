	//-----------------------------------------------------------------------//
	// <copyright file="Guard.cs" company="A.D.F.Software">
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
        /// Constructors a new guard with a specific sequence
        /// </summary>
        /// 
        private string fileSequence = "guard_sequence.xml";

        public Guard(Room room, Vector2 position, GraphicsDevice GraphicsDevice, SpriteEffects spriteEffect, string FileSequence)
        {
            graphicsDevice = GraphicsDevice;
            myRoom = room;
            fileSequence = FileSequence;
            LoadContent();


            //TAKE PLAYER Position
            Reset(position, spriteEffect);
        }

        /// <summary>
        /// Constructors a new guard.
        /// </summary>
        public Guard(Room room, Vector2 position, GraphicsDevice GraphicsDevice, SpriteEffects spriteEffect)
        {
            graphicsDevice = GraphicsDevice;
            myRoom = room;
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

            Stream txtReader = Microsoft.Xna.Framework.TitleContainer.OpenStream(PoP.CONFIG_PATH_CONTENT + PoP.CONFIG_PATH_SEQUENCES + fileSequence);


            //TextReader txtReader = File.OpenText(PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_SEQUENCES + "KID_sequence.xml");
            //Stream astream = this.GetType().Assembly.GetManifestResourceStream("PrinceOfPersia.resources.KID_sequence.xml");
            spriteSequence = (List<Sequence>)ax.Deserialize(txtReader);

            foreach (Sequence s in spriteSequence)
            {
                s.Initialize();
            }

            // Calculate bounds within texture size.         
            localBounds = new Rectangle(0, 0, SPRITE_SIZE_X, SPRITE_SIZE_Y);

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
            sprite.UpdateFrame(elapsed, ref _position, ref face, ref spriteState);

            if (IsAlive == false)
            {
                DropDead();
                return;
            }

            HandleCollisions();

        }

        /// <summary>
        /// Draws the animated player.
        /// </summary>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            sprite.DrawSprite(gameTime, spriteBatch, _position, face, 0.5f);
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
            Energy = PoP.CONFIG_KID_START_ENERGY;

            face = spriteEffect;

            spriteState.Clear();

            Stand();

        }

     


    }
}
