using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public abstract class Item
    {
        public Texture2D Texture;
        public AnimationSequence itemAnimation = new AnimationSequence();
        public TileState itemState = new TileState();
        private SpriteEffects flip = SpriteEffects.None;
        private Position position = new Position(new Vector2(0, 0), new Vector2(0, 0));

        private static List<Sequence> itemSequence = new List<Sequence>();
        public List<Sequence> ItemSequence
        {
            get { return itemSequence; }
            set { itemSequence = value; }
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState, GamePadState gamePadState, TouchCollection touchState, AccelerometerState accelState, DisplayOrientation orientation)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            itemAnimation.UpdateFrameItem(elapsed, ref position, ref flip, ref itemState);
        }

    }
}
