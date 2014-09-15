	//-----------------------------------------------------------------------//
	// <copyright file="PressPlate.cs" company="A.D.F.Software">
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
    class PressPlate : Tile
    {
        private static List<Sequence> tileSequence = new List<Sequence>();
        public int switchButton = 0;
        public float elapsedTimeOpen = 0;
        public float timeOpen = 0.3f;
        public bool infinitePress = false;

        public Enumeration.StateTile State
        {
            get { return tileState.Value().state; }
        }

        public PressPlate(Room room, Enumeration.TileType tileType, Enumeration.StateTile state, int switchButton, Enumeration.TileType NextTileType)
        {
            base.room = room;
            nextTileType = NextTileType;
            this.switchButton = switchButton;
            System.Xml.Serialization.XmlSerializer ax = new System.Xml.Serialization.XmlSerializer(tileSequence.GetType());

            Stream txtReader = Microsoft.Xna.Framework.TitleContainer.OpenStream(PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_SEQUENCES + tileType.ToString() + "_sequence.xml");
            //TextReader txtReader = File.OpenText(PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_SEQUENCES + tileType.ToString() + "_sequence.xml");

            tileSequence = (List<Sequence>)ax.Deserialize(txtReader);

            foreach (Sequence s in tileSequence)
            {
                s.Initialize();
            }

            //Search in the sequence the right type
            Sequence result = tileSequence.Find(delegate(Sequence s)
            {
                return s.name.ToUpper() == state.ToString().ToUpper();
            });

            if (result != null)
            {
                //AMF to be adjust....
                //result.frames[0].SetTexture(Content.Load<Texture2D>(PrinceOfPersiaGame.CONFIG_TILES + result.frames[0].value));
                result.frames[0].SetTexture((Texture2D) Maze.Content[PrinceOfPersiaGame.CONFIG_TILES + result.frames[0].value]);

                collision = result.collision;
                Texture = result.frames[0].texture;
            }
            Type = tileType;


            //change statetile element
            tileState.Value().state = state;
            tileAnimation.PlayAnimation(tileSequence, tileState);
        }

        public void Normal()
        {
            if (tileState.Value().state == Enumeration.StateTile.normal)
                return;

            tileState.Add(Enumeration.StateTile.normal);
            tileAnimation.PlayAnimation(tileSequence, tileState);
        }

        public void DePress()
        {
            if (tileState.Value().state == Enumeration.StateTile.pressplate)
                return;

            tileState.Add(Enumeration.StateTile.pressplate);
            tileAnimation.PlayAnimation(tileSequence, tileState);
        }

        public void Press(bool infinite)
        {
            infinitePress = infinite;
            elapsedTimeOpen = 0;

            if (tileState.Value().state == Enumeration.StateTile.dpressplate)
                return;

            tileState.Value().state = Enumeration.StateTile.dpressplate;
            tileAnimation.PlayAnimation(tileSequence, tileState);

            //i will open all door with the correct switchButton
            List<Tile> list = room.level.GetTiles(Enumeration.TileType.gate);
            foreach (Tile t in list)
            {
                foreach (int button in ((Gate)t).switchButtons)
                {
                    if (button == this.switchButton)
                        ((Gate)t).Open();
                }
            }

            bool openAllExit = false;
            list = room.level.GetTiles(Enumeration.TileType.exit);
            foreach (Tile t in list)
            {
                foreach (int button in ((Exit)t).switchButtons)
                {
                    if (button == this.switchButton)
                    {
                        //I WILL OPEN ALL EXIT IN THIS LEVEL
                        openAllExit = true;
                    }
                }
            }

            if (openAllExit == true)
            {
                foreach (Tile t in list)
                {
                    ((Exit)t).Open();
                }

            }
        }

        public void Press()
        {
            Press(false);
        }

    }
}
