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
        public Texture2D texture;
        //public SoundEffect soundEffect = new SoundEffect();
        public bool stoppable = false;
        public int xOffSet = 0;
        public int yOffSet = 0;
        //for check if the frame is a jump, hang raised in air
        public bool raised = false;

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
        }

        public Frame DeepCopy()
        {
            return (Frame)this.MemberwiseClone();
        }

    }
}
