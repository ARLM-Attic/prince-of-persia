﻿using System;
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
    public class Sword : Item
    {
        public Sword(ContentManager Content)
        {

            System.Xml.Serialization.XmlSerializer ax = new System.Xml.Serialization.XmlSerializer(ItemSequence.GetType());

            Stream txtReader = Microsoft.Xna.Framework.TitleContainer.OpenStream(PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_SEQUENCES + Enumeration.Items.sword.ToString().ToUpper() + "_sequence.xml");
            //TextReader txtReader = File.OpenText(PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_SEQUENCES + tileType.ToString().ToUpper() + "_sequence.xml");


            ItemSequence = (List<Sequence>)ax.Deserialize(txtReader);

            foreach (Sequence s in ItemSequence)
            {
                s.Initialize(Content);
            }

            //Search in the sequence the right type
            Sequence result = ItemSequence.Find(delegate(Sequence s)
            {
                return s.name == Enumeration.StateTile.normal.ToString().ToUpper();
            });

            if (result != null)
            {
                //AMF to be adjust....
                result.frames[0].SetTexture(Content.Load<Texture2D>(result.frames[0].value));

                Texture = result.frames[0].texture;
            }


            //change statetile element
            StateTileElement stateTileElement = new StateTileElement();
            stateTileElement.state = Enumeration.StateTile.normal;
            itemState.Add(stateTileElement);
            itemAnimation.PlayAnimation(ItemSequence, itemState.Value());

        
        }
    }
}
