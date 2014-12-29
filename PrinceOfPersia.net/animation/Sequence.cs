	//-----------------------------------------------------------------------//
	// <copyright file="Sequence.cs" company="A.D.F.Software">
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

        
        
        public void Initialize()
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
                        switch (config_type)
                        {
                            case "CONFIG_tiles" :
                                t = (Texture2D)Maze.Content[PoP.CONFIG_TILES + f.value];
                                break;
                            case "CONFIG_sprite_kid" :
                                t = (Texture2D)Maze.Content[PoP.CONFIG_SPRITE_KID + f.value];
                                break;
                            case "CONFIG_sprite_guard":
                                t = (Texture2D)Maze.Content[PoP.CONFIG_SPRITE_GUARD + f.value];
                                break;
                            case "CONFIG_items":
                                t = (Texture2D)Maze.Content[PoP.CONFIG_ITEMS + f.value];
                                break;

                            case "CONFIG_sprite_effects":
                                t = (Texture2D)Maze.Content[PoP.CONFIG_SPRITE_EFFECTS + f.value];
                                break;
                            default:
                                break;
                        
                        }
                        
#else
                        //t = (Texture2D) Maze.content.Load<Texture2D>(System.Configuration.ConfigurationSettings.AppSettings[config_type].ToString() + f.value);
                        t = (Texture2D)Maze.Content[System.Configuration.ConfigurationSettings.AppSettings[config_type].ToString() + f.value];
#endif
                        if (t == null)
                            f.SetTexture(null);
                        else
                            f.SetTexture(t);
                    }
                    //loading sound
                    if (f.sound != null)
                    {
                        //SoundEffect s = (SoundEffect) Maze.content.Load<SoundEffect>(PrinceOfPersiaGame.CONFIG_SOUNDS + f.sound);
                        SoundEffect s = (SoundEffect)Maze.Content[PoP.CONFIG_SOUNDS + f.sound];
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
