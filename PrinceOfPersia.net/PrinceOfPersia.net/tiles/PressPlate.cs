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
        private Maze maze;
        public int switchButton = 0;
        public float elapsedTimeOpen = 0;

        public float timeOpen = 0.3f;

        public StateTileElement.State State
        {
            get { return tileState.Value().state; }
        }


        public PressPlate(Maze maze, ContentManager Content, TileType tileType, string state, int switchButton)
        {
            this.maze = maze;
            this.switchButton = switchButton;
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

                Collision = result.tileCollision;
                Texture = result.frames[0].texture;
            }
            Type = tileType;


            //change statetile element
            tileState.Value().state = (StateTileElement.State)Enum.Parse(typeof(StateTileElement.State), state.ToLower());
            tileAnimation.PlayAnimation(tileSequence, tileState.Value());
        }

        public void Normal()
        {
            if (tileState.Value().state == StateTileElement.State.normal)
                return;

            tileState.Add(StateTileElement.State.normal);
            tileAnimation.PlayAnimation(tileSequence, tileState.Value());
        }


        public void Press()
        {
            elapsedTimeOpen = 0;
            //if (stateTileElement.state == StateTileElement.State.dpressplate)
            //    return;

            tileState.Value().state = StateTileElement.State.dpressplate;
            tileAnimation.PlayAnimation(tileSequence, tileState.Value());

            //i will open all door with the correct switchButton
            List<Tile> list = maze.GetTiles(TileType.door);
            foreach(Tile t in list)
            {
                if (((Door)t).switchButton == this.switchButton)
                    ((Door)t).Open();
            }


        }

    }
}
