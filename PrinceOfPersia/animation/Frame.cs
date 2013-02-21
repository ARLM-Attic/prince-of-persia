using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;

namespace PrinceOfPersia
{
    public class Frame
    {
        public string name;
        public string sound;
        public string value;
        public TypeFrame type = TypeFrame.SPRITE;
        public string parameter;
        
        //public SoundEffect soundEffect = new SoundEffect();
        public bool stoppable = false;
        public int xOffSet = 0;
        public int yOffSet = 0;
        public bool raised = false; //for check if the frame is a jump, hang raised in air
        public float delay = 0; //delay animation frame

        public Texture2D texture
        {
            get
            {
                return otexture;
            }
        }
        private Texture2D otexture;

        public void SetTexture(Texture2D value)
        {
            otexture = value;
        }

        public Frame()
        {}

        public enum TypeFrame
        {
            SPRITE,
            COMMAND,
        }

        public enum TypeCommand
        {
            GOTOFRAME,
            GOTOSEQUENCE,
            ABOUTFACE,
            IFGOTOSEQUENCE,
            IFGOTOFRAME
        }

        public Frame DeepCopy()
        {
            return (Frame)this.MemberwiseClone();
        }

    }
}
