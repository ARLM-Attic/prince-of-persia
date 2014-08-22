using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System.Configuration;

namespace PrinceOfPersia
{
    public class Sequence : ICloneable
    {
        private const int FRAME_WIDTH = 114;
        private const int FRAME_HEIGHT = 114;
        //public const float FRAME_TIME = 0.1f;

        public string config_type;

        public List<Frame> frames = new List<Frame>();
        public string name;
        public bool raised = false;
        public Enumeration.TileType tileType;
        public Enumeration.TileCollision collision;

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
                    //loading texture
                    if (f.value != null)
                    {
						Texture2D t = null;
#if ANDROID
#else
                        t = (Texture2D)Maze.dContentRes[System.Configuration.ConfigurationSettings.AppSettings[config_type].ToString() + f.value];
#endif
                        if (t == null)
                            f.SetTexture(null);
                        else
                            f.SetTexture(t);
                    }
                    //loading sound
                    if (f.sound != null)
                    {
                        SoundEffect s = (SoundEffect)Maze.dContentRes[PrinceOfPersiaGame.CONFIG_SOUNDS + f.sound];
                        f.SetSound(s);
                    }
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("ERROR:Content.Load<dContentRes>"+ ex.ToString() + config_type + f.value); 
                }
            }
        }

        // Deep clone
        public Sequence DeepClone()
        {
            Sequence newSequence = new Sequence();
            newSequence.name = this.name;
            newSequence.raised = this.raised;
            newSequence.collision = this.collision;
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
