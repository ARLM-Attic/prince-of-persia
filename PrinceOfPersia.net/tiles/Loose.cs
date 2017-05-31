﻿	//-----------------------------------------------------------------------//
	// <copyright file="Loose.cs" company="A.D.F.Software">
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
    class Loose : Tile
    {
        private static List<Sequence> tileSequence = new List<Sequence>();
        //private RoomNew room;
        //public int switchButton = 0;
        public float elapsedTimeOpen = 0;
        public float timeFall = 0.5f;

        public Enumeration.StateTile State
        {
            get { return tileState.Value().state; }
        }


        public Loose(Room room, Enumeration.TileType tileType, Enumeration.StateTile state, Enumeration.TileType NextTileType)
        {
            base.room = room;
            nextTileType = NextTileType;

            //this.switchButton = switchButton;
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
                //return s.name == tileType.ToString();
                return s.name.ToUpper() == state.ToString().ToUpper();
            });

            if (result != null)
            {
                //AMF to be adjust....
                result.frames[0].SetTexture((Texture2D)Maze.Content[PrinceOfPersiaGame.CONFIG_TILES + result.frames[0].value]); 
                //result.frames[0].SetTexture(Content.Load<Texture2D>(PrinceOfPersiaGame.CONFIG_TILES + result.frames[0].value));

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


        public void Press()
        {
            if (tileState.Value().state == Enumeration.StateTile.loose)
                return;

            tileState.Add(Enumeration.StateTile.loose);
            tileAnimation.PlayAnimation(tileSequence, tileState);
            //Fall();
        }

        public void Fall()
        {
            Fall(false);

        }

        public void Fall(bool force)
        {
            if (force != true)
                if (tileState.Value().state == Enumeration.StateTile.loosefall)
                    return;

            tileState.Add(Enumeration.StateTile.loosefall);
            tileAnimation.PlayAnimation(tileSequence, tileState);

            lock (room.tilesTemporaney)
            {
                room.tilesTemporaney.Add(this);
            }
            this.collision = Enumeration.TileCollision.Passable;

            //Vector2 v = new Vector2(Position.X, Position.Y);
            //Tile t = room.GetTile(v);
            room.SubsTile(Coordinates, Enumeration.TileType.space);
            //t = room.GetTile(v);

        }



        public void Shake()
        {
            if (tileState.Value().state != Enumeration.StateTile.normal)
                return;

            tileState.Add(Enumeration.StateTile.looseshake);
            tileAnimation.PlayAnimation(tileSequence, tileState);
        }

    }
}
