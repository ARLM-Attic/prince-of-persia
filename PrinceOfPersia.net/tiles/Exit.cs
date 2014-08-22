using System;
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
    public class Exit : Tile
    {
        private static List<Sequence> tileSequence = new List<Sequence>();
        public List<int> switchButtons = new List<int>();  
        public float elapsedTimeOpen = 0;
        public float timeOpen = 6;
        private int xSwitchButton = 0;

        public Enumeration.StateTile State
        {
            get { return tileState.Value().state; }
        }





        public Exit(Room room, ContentManager Content, Enumeration.TileType tileType, Enumeration.StateTile state, int switchButton, Enumeration.TileType NextTileType)
        {
            collision = Enumeration.TileCollision.Platform;
            base.room = room;
            nextTileType = NextTileType;


            switchButtons.Add(switchButton);
            System.Xml.Serialization.XmlSerializer ax = new System.Xml.Serialization.XmlSerializer(tileSequence.GetType());
            Stream txtReader = Microsoft.Xna.Framework.TitleContainer.OpenStream(PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_SEQUENCES + tileType.ToString() + "_sequence.xml");

            //TextReader txtReader = File.OpenText(PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_SEQUENCES + tileType.ToString() + "_sequence.xml");

            tileSequence = (List<Sequence>)ax.Deserialize(txtReader);

            foreach (Sequence s in tileSequence)
            {
                s.Initialize(Content);
            }

            if (state == Enumeration.StateTile.normal)
                state = Enumeration.StateTile.closed;

            //Search in the sequence the right type
            Sequence result = tileSequence.Find(delegate(Sequence s)
            {
                return s.name.ToUpper() == state.ToString().ToUpper();
            });

            if (result != null)
            {
                //AMF to be adjust....
                result.frames[0].SetTexture(Content.Load<Texture2D>(PrinceOfPersiaGame.CONFIG_TILES + result.frames[0].value));

                collision = result.collision;
                Texture = result.frames[0].texture;
            }
            Type = tileType;


            //change statetile element
            tileState.Value().state = state;
            tileAnimation.PlayAnimation(tileSequence, tileState.Value());
        }

        public void Normal()
        {
            tileState.Value().state = Enumeration.StateTile.normal;
            tileAnimation.PlayAnimation(tileSequence, tileState.Value());
        }


        //public void Close()
        //{
        //    elapsedTimeOpen = timeOpen;
        //    if (tileState.Value().state == Enumeration.StateTile.close)
        //        return;
        //    if (tileState.Value().state == Enumeration.StateTile.closed)
        //        return;

        //    if (tileState.Value().state == Enumeration.StateTile.open)
        //        tileState.Add(Enumeration.StateTile.close, Enumeration.PriorityState.Normal, Enumeration.SequenceReverse.Reverse);
        //    else
        //        tileState.Add(Enumeration.StateTile.close);

        //    tileAnimation.PlayAnimation(tileSequence, tileState.Value());
        //}

        public void Open()
        {
            //anim only the exit2 the dx room's portion
            if (tileState.Value().state == Enumeration.StateTile.exit)
                return;

            elapsedTimeOpen = 0;
            if (tileState.Value().state == Enumeration.StateTile.open)
                return;
            if (tileState.Value().state == Enumeration.StateTile.opened)
                return;
            //if (tileState.Value().state == Enumeration.StateTile.close)
            //    tileState.Add(Enumeration.StateTile.open, Enumeration.PriorityState.Normal, Enumeration.SequenceReverse.FixFrame);
            //else
                tileState.Add(Enumeration.StateTile.open);

            tileAnimation.PlayAnimation(tileSequence, tileState.Value());
        }

        public void ExitLevel()
        {
           //animation exit

        }


    }
}
