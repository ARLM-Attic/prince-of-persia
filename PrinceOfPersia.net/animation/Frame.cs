	//-----------------------------------------------------------------------//
	// <copyright file="Frame.cs" company="A.D.F.Software">
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
//using Microsoft.Xna.Framework.Input.Touch;
using System.Collections.Generic;

namespace PrinceOfPersia
{
    public class Frame
    {
        public string name;
        public string sound;
        public bool soundLoop = false;
        public bool soundFirstTime = false; //for determine if already sounded for single loop purpose
        public string value;
        public Enumeration.TypeFrame type = Enumeration.TypeFrame.SPRITE;
        public string parameter;
       
        //public SoundEffect soundEffect = new SoundEffect();
        public bool stoppable = false;
        public int xOffSet = 0;
        public int yOffSet = 0;
        public bool raised = false; //for check if the frame is a jump, hang raised in air
        public float delay = 0; //delay animation frame


        public void PlaySound()
        {
            //Play Sound
            if (sound != null)
            {
                if (soundLoop == false)
                {
                    if (soundFirstTime == false)
                    {
                        soundeffect.Play();
                        soundFirstTime = true;
                    }
                }
                else
                    soundeffect.Play();
            }
        }

        public SoundEffect soundeffect
        {
            get
            {
                return osoundeffect;
            }
        }
        private SoundEffect osoundeffect;

        public Texture2D texture
        {
            get
            {
                return otexture;
            }
        }
        private Texture2D otexture;


        public void SetSound(SoundEffect value)
        {
            osoundeffect = value;
        }

        public void SetTexture(Texture2D value)
        {
            otexture = value;
        }

        public Frame()
        {}

        //public enum TypeFrame
        //{
        //    SPRITE,
        //    COMMAND,
        //}

        public Frame DeepCopy()
        {
            return (Frame)this.MemberwiseClone();
        }

    }
}
