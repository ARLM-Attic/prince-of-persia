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
    class Block : Tile
    {
        private static List<Sequence> tileSequence = new List<Sequence>();
        

        //seed 8x8
        public static int[,] seed_graystone = new int[,] { { 2, 5, 14, 17, 56, 32, 35, 50 }, {0, 0, 0, 0, 0, 0, 0, 0}, {0, 0, 0, 0, 0, 0, 0, 0} };
        public static int[,] seed_left_bottom = new int[,] { { 2, 11, 36, 45, 0, 0, 0, 0 }, { 34, 47, 0, 0, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 } };
        public static int[,] seed_left_top = new int[,] { { 37, 0, 0, 0, 0, 0, 0, 0 }, { 9, 10, 0, 0, 0, 0, 0, 0 }, { 16, 0, 0, 0, 0, 0, 0, 0 } };
        public static int[,] seed_right_bottom = new int[,] { { 27, 33, 0, 0, 0, 0, 0, 0 }, { 2, 8, 25, 35, 0, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 } };
        public static int[,] seed_right_top = new int[,] { { 4, 10, 31, 37, 0, 0, 0, 0 }, { 6, 12, 23, 29, 39, 0, 0, 0 }, { 0, 0, 0, 0, 0, 0, 0, 0 } };
        public static int[,] seed_line_wall = new int[,] { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, }, { 0, 1, 1, 0, 0, 0, 1, 1, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 1, 1, 0, 0, 0, 1, 1, 0, 0, 0, 1, 1, 0, }, { 1, 0, 0, 1, 0, 0, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, } };
        public static int[,] seed_line_offset = new int[,] { { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, { 5, 4, 3, 3, 1, 5, 4, 2, 1, 1, 5, 3, 2, 1, 5, 4, 3, 2, 5, 4 }, { 5, 4, 3, 3, 1, 5, 4, 2, 1, 1, 5, 3, 2, 1, 5, 4, 3, 2, 5, 4 } };

        
        public Enumeration.StateTile State
        {
            get { return tileState.Value().state; }
        }


        public Block(Room room, ContentManager Content, Enumeration.TileType tileType, Enumeration.StateTile state, Enumeration.TileType NextTileType)
        {
            this.nextTileType = NextTileType;
            collision = Enumeration.TileCollision.Platform;
            base.room = room;
            System.Xml.Serialization.XmlSerializer ax = new System.Xml.Serialization.XmlSerializer(tileSequence.GetType());
            Stream txtReader = Microsoft.Xna.Framework.TitleContainer.OpenStream(PrinceOfPersiaGame.CONFIG_PATH_CONTENT + PrinceOfPersiaGame.CONFIG_PATH_SEQUENCES + tileType.ToString() + "_sequence.xml");


            tileSequence = (List<Sequence>)ax.Deserialize(txtReader);

            foreach (Sequence s in tileSequence)
            {
                s.Initialize(Content);
            }

       
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



    }
}
