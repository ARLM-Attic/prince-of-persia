	//-----------------------------------------------------------------------//
	// <copyright file="Flask.cs" company="A.D.F.Software">
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
    public class Flask : Item
    {
        public Flask()
        {

            System.Xml.Serialization.XmlSerializer ax = new System.Xml.Serialization.XmlSerializer(ItemSequence.GetType());

            Stream txtReader = Microsoft.Xna.Framework.TitleContainer.OpenStream(PoP.CONFIG_PATH_CONTENT + PoP.CONFIG_PATH_SEQUENCES + Enumeration.Items.flask.ToString() + "_sequence.xml");
            //TextReader txtReader = File.OpenText(PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_SEQUENCES + tileType.ToString() + "_sequence.xml");


            ItemSequence = (List<Sequence>)ax.Deserialize(txtReader);

            foreach (Sequence s in ItemSequence)
            {
                s.Initialize();
            }

            //Search in the sequence the right type
            Sequence result = ItemSequence.Find(delegate(Sequence s)
            {
                return s.name.ToUpper() == Enumeration.StateTile.normal.ToString().ToUpper();
            });

            if (result != null)
            {
                //AMF to be adjust....
                result.frames[0].SetTexture((Texture2D) Maze.Content[PoP.CONFIG_ITEMS+ result.frames[0].value]); 

                Texture = result.frames[0].texture;
            }


            //change statetile element
            StateTileElement stateTileElement = new StateTileElement();
            stateTileElement.state = Enumeration.StateTile.normal;
            itemState.Add(stateTileElement);
            itemAnimation.PlayAnimation(ItemSequence, itemState);

        
        }
    }
}
