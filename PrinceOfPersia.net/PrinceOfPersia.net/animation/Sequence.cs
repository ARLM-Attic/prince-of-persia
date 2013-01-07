using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace PrinceOfPersia
{
    public class Sequence : ICloneable
    {
        private const int FRAME_WIDTH = 114;
        private const int FRAME_HEIGHT = 114;
        //public const float FRAME_TIME = 0.1f;

        public List<Frame> frames = new List<Frame>();
        public string name;
        public bool raised = false;
        public TileType tileType;
        public TileCollision tileCollision;

        /// <summary>
        /// Get the Total xOffset and yOffset
        /// 
        /// </summary>
        /// <retur>
        /// Vector2
        /// </retur>
        public Vector2 CountOffSet
        {
            get
            {
                if (this == null)
                { return Vector2.Zero; }
                int x = 0,y=0;
                foreach (Frame f in frames)
                { 
                    x += f.xOffSet;
                    y += f.yOffSet;
                }
                return new Vector2((int)x, (int)y); 
            }
        }

        
        
        public void Initialize(ContentManager Content)
        {
            foreach (Frame f in frames)
            {
                try
                {
                    if (f.value != null)
                    {
                        f.texture = Content.Load<Texture2D>(f.value);
                    }
                }
                catch (Exception ex)
                { System.Console.WriteLine("ERROR:Content.Load<Texture2D>" + f.value); }
            }
        }

        // Deep clone
        public Sequence DeepClone()
        {
            Sequence newSequence = new Sequence();
            newSequence.name = this.name;
            newSequence.raised = this.raised;
            newSequence.tileCollision = this.tileCollision;
            newSequence.tileType = this.tileType;

            //newSequence.frameTime = this.frameTime;
            foreach (Frame f in this.frames)
            {
                newSequence.frames.Add(f.DeepCopy());
            }
            return newSequence;

        }

        
        object ICloneable.Clone()
        {
            return this.Clone();
        }
        public Sequence Clone()
        {
            return (Sequence)this.MemberwiseClone();
        }



    }
}
