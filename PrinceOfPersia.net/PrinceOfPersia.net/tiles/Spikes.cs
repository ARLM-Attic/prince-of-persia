﻿using System;
using System.IO;
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

namespace PrinceOfPersia
{
    class Spikes : Tile
    {
        private static List<Sequence> tileSequence = new List<Sequence>();
        //private RoomNew room;
        //public int switchButton = 0;
        public float elapsedTimeOpen = 0;
        //public float timeFall = 0.5f;
        public float timeOpen = 6;

        public Enumeration.StateTile State
        {
            get { return tileState.Value().state; }
        }


        public Spikes(RoomNew room, ContentManager Content, Enumeration.TileType tileType, string state)
        {
            base.room = room;
            //this.switchButton = switchButton;
            System.Xml.Serialization.XmlSerializer ax = new System.Xml.Serialization.XmlSerializer(tileSequence.GetType());
            Stream astream = this.GetType().Assembly.GetManifestResourceStream("PrinceOfPersia.resources." + tileType.ToString().ToUpper() + "_sequence.xml");
            tileSequence = (List<Sequence>)ax.Deserialize(astream);

            foreach (Sequence s in tileSequence)
            {
                s.Initialize(Content);
            }

            //Search in the sequence the right type
            Sequence result = tileSequence.Find(delegate(Sequence s)
            {
                //return s.name == tileType.ToString().ToUpper();
                return s.name == state;
            });

            if (result != null)
            {
                //AMF to be adjust....
                result.frames[0].texture = Content.Load<Texture2D>(result.frames[0].value);

                collision = result.collision;
                Texture = result.frames[0].texture;
            }
            Type = tileType;


            //change statetile element
            tileState.Value().state = (Enumeration.StateTile)Enum.Parse(typeof(Enumeration.StateTile), state.ToLower());
            tileAnimation.PlayAnimation(tileSequence, tileState.Value());
        }

        public void Normal()
        {
            if (tileState.Value().state == Enumeration.StateTile.normal)
                return;

            tileState.Add(Enumeration.StateTile.normal);
            tileAnimation.PlayAnimation(tileSequence, tileState.Value());
        }


        public void Open()
        {
            elapsedTimeOpen = 0;

            if (tileState.Value().state == Enumeration.StateTile.open)
                return;
            if (tileState.Value().state == Enumeration.StateTile.opened)
                return;


            tileState.Add(Enumeration.StateTile.open);
            tileAnimation.PlayAnimation(tileSequence, tileState.Value());
        }

        public void Close()
        {
            elapsedTimeOpen = timeOpen;

            if (tileState.Value().state == Enumeration.StateTile.close)
                return;

            tileState.Add(Enumeration.StateTile.close);
            tileAnimation.PlayAnimation(tileSequence, tileState.Value());

        }




    }
}