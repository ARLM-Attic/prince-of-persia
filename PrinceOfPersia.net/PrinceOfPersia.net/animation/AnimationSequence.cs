using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace PrinceOfPersia
{
    /// <summary>
    /// Controls playback of an Animation
    /// </summary>
    public struct AnimationSequence
    {
        bool firstTime;
        public Sequence sequence;
        public List<Sequence> lsequence;
        private float TotalElapsed;
        //private float time;
        private float TimePerFrame;

        /// <summary>
        /// 
        /// </summary>
        public bool IsStoppable
        {
            get 
            {
                if (sequence != null)
                { return Frames[frameIndex].stoppable; }
                return true;
            }
            //set { stoppable = value; }
        }

        public List<Frame> Frames
        {
            get { return sequence.frames; }
        }

        /// <summary>
        /// Gets the index of the current frame in the animation.
        /// </summary>
        public int FrameIndex
        {
            get { return frameIndex; }
        }
        int frameIndex;

        /// <summary>
        /// Gets a texture origin at the bottom center of each frame.
        /// </summary>
        public Vector2 Origin
        {
            get 
            {
                if (sequence.frames[frameIndex].texture == null)
                    return Vector2.Zero;
                return new Vector2(sequence.frames[frameIndex].texture.Width / 2.0f, 0); 
            }
        }

        /// <summary>
        /// Begins or continues playback of an animation.
        /// </summary>
        public void PlayAnimation(List<Sequence> lsequence, StateElement stateElement)
        {
            StatePlayerElement statePlayerElement;
            StateTileElement stateTileElement;
            string stateName = string.Empty;
            if (typeof(StatePlayerElement) == stateElement.GetType())
            { 
                statePlayerElement = (StatePlayerElement) stateElement;
                stateName = statePlayerElement.state.ToString();
            }
            else if (typeof(StateTileElement) == stateElement.GetType())
            {
                stateTileElement = (StateTileElement)stateElement;
                stateName = stateTileElement.state.ToString();
            }
            else
            {
                stateName = stateElement.state.ToString();
            }


            // Start the new animation.
            if (stateElement.Priority == StateElement.PriorityState.Normal & this.IsStoppable == false)
                return;

            //Check if the animation is already playing
            if (sequence != null && sequence.name == stateName)
                return;



            this.lsequence = lsequence;
            //Search in the sequence the right type
            Sequence result = lsequence.Find(delegate(Sequence s)
            {
                return s.name == stateName.ToUpper();
            });

            if (result == null)
            {
                return;

            }

            //cloning for avoid reverse pemanently...
            sequence = result.DeepClone();
  
            if (stateElement.Stoppable != null)
            { 
                foreach(Frame f in sequence.frames)
                {
                    f.stoppable = (bool)stateElement.Stoppable;
                }
            }

            //For increase offset depend of the state previus; for example when running and fall the x offset will be increase.
            if (stateElement.OffSet != Vector2.Zero)
            {
                foreach (Frame f in sequence.frames)
                {
                    f.xOffSet = f.xOffSet + (int)stateElement.OffSet.X;
                    f.yOffSet = f.yOffSet + (int)stateElement.OffSet.Y;
                }
            }

            //Check if reverse movement and reverse order and sign x,y
            if (stateElement.Reverse == StateElement.SequenceReverse.Reverse)
            {
                List<Frame> newListFrame = new List<Frame>();
                List<Frame> newListCommand = new List<Frame>();

                foreach (Frame f in sequence.frames)
                {
                    if (f.type == Frame.TypeFrame.COMMAND)
                    {
                        newListCommand.Add(f);
                    }
                    else
                    {
                        f.xOffSet = -1 * f.xOffSet;
                        f.yOffSet = -1 * f.yOffSet;
                        newListFrame.Add(f);
                    }
                }
                newListFrame.Reverse();
                //add command
                foreach (Frame f in newListCommand)
                {
                    newListFrame.Add(f);
                }

                sequence.frames = newListFrame;
            }


            //this.time = 0.0f;
            this.frameIndex = 0;
            this.firstTime = true;
        }


        public void UpdateFrame(float elapsed, ref Position position, ref SpriteEffects spriteEffects, ref PlayerState playerState)
        {
            TimePerFrame = 0.09f + sequence.frames[frameIndex].delay; //0.1
            TotalElapsed += elapsed;
     
            if (TotalElapsed > TimePerFrame)
            {
                frameIndex = Math.Min(frameIndex + 1, Frames.Count - 1);
                TotalElapsed -= TimePerFrame;
                if (sequence.frames[frameIndex].type != Frame.TypeFrame.SPRITE)
                {
                    //COMMAND
                    string[] aCommand = sequence.frames[frameIndex].name.Split('|');
                    string[] aParameter = sequence.frames[frameIndex].parameter.Split('|');
                    for (int x = 0; x < aCommand.Length; x++)
                    {
                        if (aCommand[x] == Frame.TypeCommand.ABOUTFACE.ToString())
                        {
                            if (spriteEffects == SpriteEffects.FlipHorizontally)
                                spriteEffects = SpriteEffects.None;
                            else
                                spriteEffects = SpriteEffects.FlipHorizontally;
                        }
                        else if (aCommand[x] == Frame.TypeCommand.GOTOFRAME.ToString())
                        {
                            string par = aParameter[x];
                            int result = sequence.frames.FindIndex(delegate(Frame f)
                            {
                                return f.name == par;
                            });
                            frameIndex = result;
                        }
                        else if (aCommand[x] == Frame.TypeCommand.GOTOSEQUENCE.ToString())
                        {
                            string par = aParameter[x];
                            Sequence result = lsequence.Find(delegate(Sequence s)
                            {
                                return s.name == par;
                            });
                            sequence = result;
                            frameIndex = 0;
                            playerState.Add(StatePlayerElement.Parse(par));
                        }
                        else if (aCommand[x] == Frame.TypeCommand.IFGOTOSEQUENCE.ToString())
                        {
                            string par = string.Empty;
                            if (playerState.Value().IfTrue == true)
                                par = aParameter[0];
                            else
                                par = aParameter[1];

                            Sequence result = lsequence.Find(delegate(Sequence s)
                            {
                                return s.name == par;
                            });
                            sequence = result;
                            frameIndex = 0;
                            playerState.Add(StatePlayerElement.Parse(par));
                        }

                    }
                }
                
                int flip;
                if (spriteEffects == SpriteEffects.FlipHorizontally)
                    flip = 1;
                else
                    flip = -1;

                position.Value = new Vector2(position.X + (sequence.frames[frameIndex].xOffSet * flip), position.Y + sequence.frames[frameIndex].yOffSet);
            }
            else if (firstTime == true )
            {
                int flip;
                if (spriteEffects == SpriteEffects.FlipHorizontally)
                    flip = 1;
                else
                    flip = -1;

                position.Value = new Vector2(position.X + (sequence.frames[frameIndex].xOffSet * flip), position.Y + sequence.frames[frameIndex].yOffSet);
                firstTime = false;
            }

     

       }

        public void UpdateFrameTile(float elapsed, ref Position position, ref SpriteEffects spriteEffects, ref TileState tileState)
        {
            TimePerFrame = 0.09f + sequence.frames[frameIndex].delay; //0.1
            TotalElapsed += elapsed;

            if (TotalElapsed > TimePerFrame)
            {
                frameIndex = Math.Min(frameIndex + 1, Frames.Count - 1);
                TotalElapsed -= TimePerFrame;
                if (sequence.frames[frameIndex].type != Frame.TypeFrame.SPRITE)
                {
                    //COMMAND
                    string[] aCommand = sequence.frames[frameIndex].name.Split('|');
                    string[] aParameter = sequence.frames[frameIndex].parameter.Split('|');
                    for (int x = 0; x < aCommand.Length; x++)
                    {
                        if (aCommand[x] == Frame.TypeCommand.ABOUTFACE.ToString())
                        {
                            if (spriteEffects == SpriteEffects.FlipHorizontally)
                                spriteEffects = SpriteEffects.None;
                            else
                                spriteEffects = SpriteEffects.FlipHorizontally;
                        }
                        else if (aCommand[x] == Frame.TypeCommand.GOTOFRAME.ToString())
                        {
                            string par = aParameter[x];
                            int result = sequence.frames.FindIndex(delegate(Frame f)
                            {
                                return f.name == par;
                            });
                            frameIndex = result;
                        }
                        else if (aCommand[x] == Frame.TypeCommand.GOTOSEQUENCE.ToString())
                        {
                            string par = aParameter[x];
                            Sequence result = lsequence.Find(delegate(Sequence s)
                            {
                                return s.name == par;
                            });
                            sequence = result;
                            frameIndex = 0;
                            tileState.Add(StateTileElement.Parse(par));
                        }
                        else if (aCommand[x] == Frame.TypeCommand.IFGOTOSEQUENCE.ToString())
                        {
                            string par = string.Empty;
                            if (tileState.Value().IfTrue == true)
                                par = aParameter[0];
                            else
                                par = aParameter[1];

                            Sequence result = lsequence.Find(delegate(Sequence s)
                            {
                                return s.name == par;
                            });
                            sequence = result;
                            frameIndex = 0;
                            tileState.Add(StateTileElement.Parse(par));
                        }

                    }
                }

                int flip;
                if (spriteEffects == SpriteEffects.FlipHorizontally)
                    flip = 1;
                else
                    flip = -1;

                position.Value = new Vector2(position.X + (sequence.frames[frameIndex].xOffSet * flip), position.Y + sequence.frames[frameIndex].yOffSet);
            }
            else if (firstTime == true)
            {
                int flip;
                if (spriteEffects == SpriteEffects.FlipHorizontally)
                    flip = 1;
                else
                    flip = -1;

                position.Value = new Vector2(position.X + (sequence.frames[frameIndex].xOffSet * flip), position.Y + sequence.frames[frameIndex].yOffSet);
                firstTime = false;
            }



        }




        public void DrawTile(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, Vector2 positionArrive, SpriteEffects spriteEffects, float depth)
        {
            // Calculate the source rectangle of the current frame.
            Rectangle source = new Rectangle(0, 0, sequence.frames[frameIndex].texture.Height, sequence.frames[frameIndex].texture.Height);

            // Draw the current tile.
            spriteBatch.Draw(sequence.frames[frameIndex].texture, position, source, Color.White, 0.0f, Vector2.Zero, 1.0f, spriteEffects, depth);
        }


      
        public void DrawSprite(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, Vector2 positionArrive, SpriteEffects spriteEffects, float depth)
        {

            // Calculate the source rectangle of the current frame.
            Rectangle source = new Rectangle(0, 0, sequence.frames[frameIndex].texture.Height, sequence.frames[frameIndex].texture.Height);

            position = new Vector2(position.X + Tile.PERSPECTIVE, position.Y - Tile.GROUND);

            // Draw the current frame.
            spriteBatch.Draw(sequence.frames[frameIndex].texture, position, source, Color.White, 0.0f, Vector2.Zero, 1.0f, spriteEffects, depth);

        }
    }






}
