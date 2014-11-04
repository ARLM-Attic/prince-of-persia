	//-----------------------------------------------------------------------//
	// <copyright file="AnimationSequence.cs" company="A.D.F.Software">
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
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace PrinceOfPersia
{
    /// <summary>
    /// Controls playback of an Animation
    /// </summary>
    public class AnimationSequence
    {
        bool firstTime;
        public Sequence sequence;
        public List<Sequence> lsequence;
        private float TotalElapsed;
        //private float time;
        private Sprite sprite;
        private Item item;
        private Tile tile;
        public static float frameRate = 0.09f;

        public AnimationSequence(Sprite sprite)
        {
            this.sprite = sprite;
        }

        public AnimationSequence(Item item)
        {
            this.item = item;
        }

        public AnimationSequence(Tile tile)
        {
            this.tile = tile;
        }

        public object source
        {
            get 
            {
                if (sprite != null)
                    return sprite;
                if (item != null)
                    return item;
                if (tile != null)
                    return tile;
                return null;

            }
        }


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
            get 
            { 
               
                return frameIndex; 
            }
            set
            {
                frameIndex = value;
            }
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
        /// Gets the index of the current frame in the animation.
        /// i count ONLY sprite command
        /// </summary>
        public int FrameSpriteCount()
        {
            int frameSprite = 0;
            foreach (Frame f in this.Frames)
            {
                if (f.type == Enumeration.TypeFrame.SPRITE)
                    frameSprite++;
            }
            return frameSprite;
        }


        /// <summary>
        /// Begins or continues playback of an animation.
        /// </summary>
        /// 

        public void PlayAnimation(List<Sequence> lsequence, TileState tileState)
        {
            StateTileElement stateTileElement = tileState.Value();
            string stateName = stateTileElement.state.ToString();
            

            // Start the new animation.
            if (stateTileElement.Priority == Enumeration.PriorityState.Normal & this.IsStoppable == false)
                return;

            //Check if the animation is already playing
            if (sequence != null && sequence.name == stateName)
                return;



            this.lsequence = lsequence;
            //Search in the sequence the right type
            Sequence result = lsequence.Find(delegate(Sequence s)
            {
                return s.name.ToUpper() == stateName.ToUpper();
            });

            if (result == null)
            {
                //will be an error 
                return;
            }

            //cloning for avoid reverse pemanently...
            sequence = result.DeepClone();

            if (stateTileElement.Stoppable != null)
            {
                foreach (Frame f in sequence.frames)
                {
                    f.stoppable = (bool)stateTileElement.Stoppable;
                }
            }

            //For increase offset depend of the state previus; for example when running and fall the x offset will be increase.
            if (stateTileElement.OffSet != Vector2.Zero)
            {
                foreach (Frame f in sequence.frames)
                {
                    f.xOffSet = f.xOffSet + (int)stateTileElement.OffSet.X;
                    f.yOffSet = f.yOffSet + (int)stateTileElement.OffSet.Y;
                }
            }

            //Check if reverse movement and reverse order and sign x,y
            if (stateTileElement.Reverse == Enumeration.SequenceReverse.Reverse)
            {
                List<Frame> newListFrame = new List<Frame>();
                List<Frame> newListCommand = new List<Frame>();

                foreach (Frame f in sequence.frames)
                {
                    if (f.type == Enumeration.TypeFrame.COMMAND)
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


            if (stateTileElement.Reverse == Enumeration.SequenceReverse.FixFrame)
            {
                int newIndex = this.FrameSpriteCount() - this.frameIndex;
                //fix bug result20140830
                if (sequence.frames[newIndex].type == Enumeration.TypeFrame.COMMAND)
                    frameIndex = newIndex - 1;
                else
                    frameIndex = newIndex;
            }
            else
            {
                frameIndex = 0;
            }

            firstTime = true;

            //Taking name of the frame usefull for hit combat..
            stateTileElement.Name = sequence.frames[frameIndex].name;

            if (sequence.raised == true)
                stateTileElement.Raised = true;
            else
                stateTileElement.Raised = sequence.frames[frameIndex].raised;


            //ASSIGN FRAME only for debug purpose
            stateTileElement.Frame = frameIndex;


        }

        public void PlayAnimation(List<Sequence> lsequence, PlayerState playerState)
        {
            StatePlayerElement statePlayerElement = ((PlayerState)playerState).Value();
            string stateName = statePlayerElement.state.ToString();

            // Check if is stoppable
            if (statePlayerElement.Priority == Enumeration.PriorityState.Normal & this.IsStoppable == false)
                return;
       

            //Check if the animation is already playing
            if (sequence != null && sequence.name.ToUpper() == stateName.ToUpper())
                return;



            this.lsequence = lsequence;
            //Search in the sequence the right type
            Sequence result = lsequence.Find(delegate(Sequence s)
            {
                return s.name.ToUpper() == stateName.ToUpper();
            });

            if (result == null)
            {
                //will be an error 
                return;
            }

            //cloning for avoid reverse pemanently...
            sequence = result.DeepClone();

            if (statePlayerElement.Stoppable != null)
            { 
                foreach(Frame f in sequence.frames)
                {
                    f.stoppable = (bool)statePlayerElement.Stoppable;
                }
            }

            //For increase offset depend of the state previus; for example when running and fall the x offset will be increase.
            if (statePlayerElement.OffSet != Vector2.Zero)
            {
                foreach (Frame f in sequence.frames)
                {
                    f.xOffSet = f.xOffSet + (int)statePlayerElement.OffSet.X;
                    f.yOffSet = f.yOffSet + (int)statePlayerElement.OffSet.Y;
                }
            }

            //Check if reverse movement and reverse order and sign x,y
            if (statePlayerElement.Reverse == Enumeration.SequenceReverse.Reverse)
            {
                List<Frame> newListFrame = new List<Frame>();
                List<Frame> newListCommand = new List<Frame>();

                foreach (Frame f in sequence.frames)
                {
                    if (f.type == Enumeration.TypeFrame.COMMAND)
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

            //For dynamic offset during movement
            if (statePlayerElement.OffSetTotal != Vector2.Zero)
            {
                int xCount = 0;
                foreach (Frame f in sequence.frames)
                {
                    if (f.type == Enumeration.TypeFrame.SPRITE)
                    {
                        xCount++;
                    }
                }
                int eachOffsetX = 0;
                int eachOffsetXR = 0;
                if (statePlayerElement.OffSetTotal.X != 0)
                {
                    eachOffsetX =  (int)statePlayerElement.OffSetTotal.X / xCount;
                    eachOffsetXR = (int)statePlayerElement.OffSetTotal.X % xCount;
                }
                int eachOffsetY = 0;
                int eachOffsetYR = 0;
                if (statePlayerElement.OffSetTotal.Y != 0)
                {
                    eachOffsetY = (int)statePlayerElement.OffSetTotal.Y / xCount;
                    eachOffsetYR = (int)statePlayerElement.OffSetTotal.Y % xCount;
                }
                int xCountNew = 0;
                foreach (Frame f in sequence.frames)
                {
                    if (f.type == Enumeration.TypeFrame.SPRITE)
                    {
                        f.xOffSet = eachOffsetX;
                        f.yOffSet = eachOffsetY;
                        if (++xCountNew == xCount)
                        {
                            f.xOffSet = eachOffsetX + eachOffsetXR;
                            f.yOffSet = eachOffsetY + eachOffsetYR;
                        }
                    }
                }
            }


            if (statePlayerElement.Reverse == Enumeration.SequenceReverse.FixFrame)
            {
                int newIndex = this.FrameSpriteCount() - this.frameIndex;
                //fix bug result20140830
                if (sequence.frames[newIndex].type == Enumeration.TypeFrame.COMMAND)
                    this.frameIndex = newIndex - 1;
                else
                    this.frameIndex = newIndex;
            }
            else
            {
                this.frameIndex = 0;
            }

            this.firstTime = true;

            //Taking name of the frame usefull for hit combat..
            statePlayerElement.Name = sequence.frames[frameIndex].name;


            //ASSIGN NOW THE RAISED
            if (sequence.raised == true)
                statePlayerElement.Raised = true;
            else
                statePlayerElement.Raised = sequence.frames[frameIndex].raised;


            //ASSIGN FRAME only for debug purpose
            statePlayerElement.Frame = frameIndex;


        }

        
       // public void UpdateFrame(float elapsed, ref Position position, ref SpriteEffects spriteEffects, ref PlayerState playerState)
       // {
       //     int flip = 0;

       //     //Resetting Name
       //     float TimePerFrame = 0;

       //     TimePerFrame = frameRate + sequence.frames[frameIndex].delay; //0.1
       //     TotalElapsed += elapsed;

       //     if (firstTime == true)
       //     {
       //         //Play Sound
       //         sequence.frames[frameIndex].PlaySound();

       //         if (spriteEffects == SpriteEffects.FlipHorizontally)
       //             flip = 1;
       //         else
       //             flip = -1;

       //         position.Value = new Vector2(position.X + (sequence.frames[frameIndex].xOffSet * flip), position.Y + sequence.frames[frameIndex].yOffSet);
       //         firstTime = false;

       //         //Taking name of the frame usefull for hit combat..
       //         playerState.Value().Name = sequence.frames[frameIndex].name;

       //         //ASSIGN NOW THE RAISED
       //         if (sequence.raised == true)
       //         {
       //             playerState.Value().Raised = true;
       //         }
       //         else
       //         { 
       //             //get single raised values from frameindex
       //             playerState.Value().Raised = sequence.frames[frameIndex].raised; 
       //         }

       //         //ASSIGN FRAME only for debug purpose
       //         playerState.Value().Frame = frameIndex;

       //         return;
       //     }


       //     if (TotalElapsed < TimePerFrame)
       //     { 
       //         return; 
       //     }

       //     //reset values
       //     playerState.Value().Raised = false;
       //     playerState.Value().Name = string.Empty;
       //     playerState.Value().Frame = 0;

          

       //     frameIndex = Math.Min(frameIndex + 1, Frames.Count - 1);
       //     TotalElapsed -= TimePerFrame;
            
            
          
       //     if (sequence.frames[frameIndex].type != Enumeration.TypeFrame.SPRITE)
       //     {

       //         //COMMAND
       //         // SEPARATOR :
       //         // "|" for each command
       //         // ":" for command parameter
       //         // "," for comma the parameter

                

       //         for (int c = frameIndex; c < (Frames.Count); c++)
       //         {
       //             if (sequence.frames[c].type != Enumeration.TypeFrame.COMMAND)
       //             {
       //                 break;
       //             }


       //             string[] aCommand = sequence.frames[c].name.Split('|');
       //             string[] aParameter = sequence.frames[c].parameter.Split('|');
                    
       //             for (int x = 0; x < aCommand.Length; x++)
       //             {
       //                 int result = 0;
       //                 Sequence sresult = null;
       //                 string par = string.Empty;
       //                 string[] apar = null;

       //                 Enumeration.TypeCommand aTypeCommand = (Enumeration.TypeCommand)Enum.Parse(typeof(Enumeration.TypeCommand), aCommand[x]);

       //                 switch (aTypeCommand)
       //                 {
       //                     case Enumeration.TypeCommand.ABOUTFACE:
       //                         if (spriteEffects == SpriteEffects.FlipHorizontally)
       //                             spriteEffects = SpriteEffects.None;
       //                         else
       //                             spriteEffects = SpriteEffects.FlipHorizontally;

       //                         break;
                                

       //                     case Enumeration.TypeCommand.GOTOFRAME:
       //                         par = aParameter[x].ToUpper();
       //                         result = sequence.frames.FindIndex(delegate(Frame f)
       //                         {
       //                             return f.name == par;
       //                         });
       //                         frameIndex = result;
       //                         break;

       //                     case Enumeration.TypeCommand.GOTOSEQUENCE:
       //                         apar = aParameter[x].Split(':');
       //                         sresult = lsequence.Find(delegate(Sequence s)
       //                         {
       //                             return s.name.ToUpper() == apar[0].ToUpper();
       //                         });
       //                         sequence = sresult;
       //                         frameIndex = 0;

       //                         if (par.Length > 1)
       //                         {
       //                             Vector2 v = new Vector2(float.Parse(apar[1].Split(',')[0]), float.Parse(apar[1].Split(',')[1]));
       //                             playerState.Add(StatePlayerElement.Parse(apar[0]), Enumeration.PriorityState.Normal, v);
       //                         }
       //                         else
       //                             playerState.Add(StatePlayerElement.Parse(apar[0]));
       //                         break;

       //                     case Enumeration.TypeCommand.IFGOTOSEQUENCE:
       //                         par = string.Empty;
       //                         if (playerState.Value().IfTrue == true)
       //                             par = aParameter[0];
       //                         else
       //                             par = aParameter[1];

       //                         sresult = lsequence.Find(delegate(Sequence s)
       //                         {
       //                             return s.name.ToUpper() == par.ToUpper();
       //                         });
       //                         sequence = sresult;
       //                         frameIndex = 0;
       //                         playerState.Add(StatePlayerElement.Parse(par));
       //                         break;

       //                     case Enumeration.TypeCommand.DELETE:
       //                         playerState.Add(Enumeration.State.delete, Enumeration.PriorityState.Force);
       //                         break;

       //                     //Call sprite function by reflection
       //                     case Enumeration.TypeCommand.FUNCTION_BOOL:
       //                         string methodName = aParameter[0];
       //                         string[] methodParam = { aParameter[1], aParameter[2] };
       //                         int iMethodParam = 0;

       //                         Type thisType = source.GetType();
       //                         PropertyInfo theProperty = null;
       //                         MethodInfo theMethod = null;
       //                         bool isMethod = false;
       //                         theProperty = thisType.GetProperty(methodName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.IgnoreReturn);
       //                         theMethod = thisType.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.IgnoreReturn);
       //                         if (theProperty == null & theMethod != null)
       //                         {
       //                             isMethod = true;
       //                         }

       //                         bool methodReturn;

       //                         if (isMethod == false)
       //                             methodReturn = (bool)theProperty.GetValue(sprite, null);
       //                         else
       //                             methodReturn = (bool)theMethod.Invoke(sprite, null);

       //                         if (methodReturn == true)
       //                             iMethodParam = 0;
       //                         else
       //                             iMethodParam = 1;

       //                         //FIND THE SEQUENCE
       //                         sresult = lsequence.Find(delegate(Sequence s)
       //                         {
       //                             return s.name.ToUpper() == methodParam[iMethodParam].ToUpper();
       //                         });
       //                         sequence = sresult;
       //                         frameIndex = 0;
       //                         playerState.Add(StatePlayerElement.Parse(methodParam[iMethodParam]));

       //                         break;


       //                     default: break;
       //                 }

       //             }                    

       //         }
       //     }

       //     //Play Sound
       //     sequence.frames[frameIndex].PlaySound();


       //     if (spriteEffects == SpriteEffects.FlipHorizontally) 
       //         flip = 1;
       //     else
       //         flip = -1;

       //     position.Value = new Vector2(position.X + (sequence.frames[frameIndex].xOffSet * flip), position.Y + sequence.frames[frameIndex].yOffSet);
         
       //     //Taking name of the frame usefull for hit combat..
       //     playerState.Value().Name = sequence.frames[frameIndex].name;

       //     //ASSIGN NOW THE RAISED
       //     if (sequence.raised == true)
       //         playerState.Value().Raised = true;
       //     else
       //         playerState.Value().Raised = sequence.frames[frameIndex].raised;


       //     //ASSIGN FRAME only for debug purpose
       //     playerState.Value().Frame = frameIndex;

       //}


        


        public void UpdateFrame(float elapsed, ref Position position, ref SpriteEffects spriteEffects, ref PlayerState playerState)
        {
            int flip = 0;

            //Resetting Name
            float TimePerFrame = 0;

            TimePerFrame = frameRate + sequence.frames[frameIndex].delay; //0.1
            TotalElapsed += elapsed;

            if (firstTime == false)
            {
                if (TotalElapsed < TimePerFrame)
                {
                    return;
                }
                else 
                {
                    frameIndex = Math.Min(frameIndex + 1, Frames.Count - 1);
                    TotalElapsed -= TimePerFrame;
                }

            }
            else
            {
                firstTime = false;
            }
            //reset values
            playerState.Value().Raised = false;
            playerState.Value().Name = string.Empty;
            playerState.Value().Frame = 0;


            if (sequence.frames[frameIndex].type != Enumeration.TypeFrame.SPRITE)
            {

                //COMMAND
                // SEPARATOR :
                // "|" for each command
                // ":" for command parameter
                // "," for comma the parameter



                for (int c = frameIndex; c < (Frames.Count); c++)
                {
                    if (sequence.frames[c].type != Enumeration.TypeFrame.COMMAND)
                    {
                        break;
                    }


                    string[] aCommand = sequence.frames[c].name.Split('|');
                    string[] aParameter = sequence.frames[c].parameter.Split('|');

                    for (int x = 0; x < aCommand.Length; x++)
                    {
                        int result = 0;
                        Sequence sresult = null;
                        string par = string.Empty;
                        string[] apar = null;

                        Enumeration.TypeCommand aTypeCommand = (Enumeration.TypeCommand)Enum.Parse(typeof(Enumeration.TypeCommand), aCommand[x]);

                        switch (aTypeCommand)
                        {
                            case Enumeration.TypeCommand.ABOUTFACE:
                                if (spriteEffects == SpriteEffects.FlipHorizontally)
                                    spriteEffects = SpriteEffects.None;
                                else
                                    spriteEffects = SpriteEffects.FlipHorizontally;

                                break;


                            case Enumeration.TypeCommand.GOTOFRAME:
                                par = aParameter[x].ToUpper();
                                result = sequence.frames.FindIndex(delegate(Frame f)
                                {
                                    return f.name == par;
                                });
                                frameIndex = result;
                                break;

                            case Enumeration.TypeCommand.GOTOSEQUENCE:
                                apar = aParameter[x].Split(':');
                                sresult = lsequence.Find(delegate(Sequence s)
                                {
                                    return s.name.ToUpper() == apar[0].ToUpper();
                                });
                                sequence = sresult;
                                frameIndex = 0;

                                if (par.Length > 1)
                                {
                                    Vector2 v = new Vector2(float.Parse(apar[1].Split(',')[0]), float.Parse(apar[1].Split(',')[1]));
                                    playerState.Add(StatePlayerElement.Parse(apar[0]), Enumeration.PriorityState.Normal, v);
                                }
                                else
                                    playerState.Add(StatePlayerElement.Parse(apar[0]));
                                break;

                            case Enumeration.TypeCommand.IFGOTOSEQUENCE:
                                par = string.Empty;
                                if (playerState.Value().IfTrue == true)
                                    par = aParameter[0];
                                else
                                    par = aParameter[1];

                                sresult = lsequence.Find(delegate(Sequence s)
                                {
                                    return s.name.ToUpper() == par.ToUpper();
                                });
                                sequence = sresult;
                                frameIndex = 0;
                                playerState.Add(StatePlayerElement.Parse(par));
                                break;

                            case Enumeration.TypeCommand.DELETE:
                                playerState.Add(Enumeration.State.delete, Enumeration.PriorityState.Force);
                                break;

                            //Call sprite function by reflection
                            case Enumeration.TypeCommand.FUNCTION_BOOL:
                                string methodName = aParameter[0];
                                string[] methodParam = { aParameter[1], aParameter[2] };
                                int iMethodParam = 0;

                                Type thisType = source.GetType();
                                PropertyInfo theProperty = null;
                                MethodInfo theMethod = null;
                                bool isMethod = false;
                                theProperty = thisType.GetProperty(methodName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.IgnoreReturn);
                                theMethod = thisType.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.IgnoreReturn);
                                if (theProperty == null & theMethod != null)
                                {
                                    isMethod = true;
                                }

                                bool methodReturn;

                                if (isMethod == false)
                                    methodReturn = (bool)theProperty.GetValue(sprite, null);
                                else
                                    methodReturn = (bool)theMethod.Invoke(sprite, null);

                                if (methodReturn == true)
                                    iMethodParam = 0;
                                else
                                    iMethodParam = 1;

                                //FIND THE SEQUENCE
                                sresult = lsequence.Find(delegate(Sequence s)
                                {
                                    return s.name.ToUpper() == methodParam[iMethodParam].ToUpper();
                                });
                                sequence = sresult;
                                frameIndex = 0;
                                playerState.Add(StatePlayerElement.Parse(methodParam[iMethodParam]));

                                break;


                            default: break;
                        }

                    }

                }
            }

            //Play Sound
            sequence.frames[frameIndex].PlaySound();


            if (spriteEffects == SpriteEffects.FlipHorizontally)
                flip = 1;
            else
                flip = -1;

            position.Value = new Vector2(position.X + (sequence.frames[frameIndex].xOffSet * flip), position.Y + sequence.frames[frameIndex].yOffSet);

            //Taking name of the frame usefull for hit combat..
            playerState.Value().Name = sequence.frames[frameIndex].name;

            //ASSIGN NOW THE RAISED
            if (sequence.raised == true)
                playerState.Value().Raised = true;
            else
                playerState.Value().Raised = sequence.frames[frameIndex].raised;


            //ASSIGN FRAME only for debug purpose
            playerState.Value().Frame = frameIndex;
        }




        public void UpdateFrameTile(float elapsed, ref Position position, ref SpriteEffects spriteEffects, ref TileState tileState)
        {
            float TimePerFrame = 0;
            TimePerFrame = frameRate + sequence.frames[frameIndex].delay; //0.1
            tileState.Value().Name = string.Empty;
            TotalElapsed += elapsed;

            if (TotalElapsed > TimePerFrame)
            {
                //Play Sound
                sequence.frames[frameIndex].PlaySound();


                frameIndex = Math.Min(frameIndex + 1, Frames.Count - 1);
                TotalElapsed -= TimePerFrame;

                //Taking name of the frame usefull for kill or other interactions..
                tileState.Value().Name = sequence.frames[frameIndex].name;

                    if (sequence.frames[frameIndex].type != Enumeration.TypeFrame.SPRITE)
                    {
                        //COMMAND
                        string[] aCommand = sequence.frames[frameIndex].name.Split('|');
                        string[] aParameter = sequence.frames[frameIndex].parameter.Split('|');
                        for (int x = 0; x < aCommand.Length; x++)
                        {
                            if (aCommand[x] == Enumeration.TypeCommand.ABOUTFACE.ToString())
                            {
                                if (spriteEffects == SpriteEffects.FlipHorizontally)
                                    spriteEffects = SpriteEffects.None;
                                else
                                    spriteEffects = SpriteEffects.FlipHorizontally;
                            }
                            else if (aCommand[x] == Enumeration.TypeCommand.GOTOFRAME.ToString())
                            {
                                string par = aParameter[x];
                                int result = sequence.frames.FindIndex(delegate(Frame f)
                                {
                                    return f.name.ToUpper() == par.ToUpper();
                                });
                                frameIndex = result;
                            }
                            else if (aCommand[x] == Enumeration.TypeCommand.GOTOSEQUENCE.ToString())
                            {
                                string par = aParameter[x];
                                Sequence result = lsequence.Find(delegate(Sequence s)
                                {
                                    return s.name.ToUpper() == par.ToUpper();
                                });
                                sequence = result;
                                frameIndex = 0;
                                tileState.Add(StateTileElement.Parse(par));
                            }
                            else if (aCommand[x] == Enumeration.TypeCommand.IFGOTOSEQUENCE.ToString())
                            {
                                string par = string.Empty;
                                if (tileState.Value().IfTrue == true)
                                    par = aParameter[0];
                                else
                                    par = aParameter[1];

                                Sequence result = lsequence.Find(delegate(Sequence s)
                                {
                                    return s.name.ToUpper() == par.ToUpper();
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

                //Play Sound
                sequence.frames[frameIndex].PlaySound();

            }

        }


        public void UpdateFrameItem(float elapsed, ref Position position, ref SpriteEffects spriteEffects, ref TileState itemState)
        {
            float TimePerFrame = 0;
            TimePerFrame = frameRate + sequence.frames[frameIndex].delay; //0.1
            //TimePerFrame = 0.9f + sequence.frames[frameIndex].delay; //0.1
            TotalElapsed += elapsed;

            if (TotalElapsed > TimePerFrame)
            {

                //Play Sound
                sequence.frames[frameIndex].PlaySound();


                frameIndex = Math.Min(frameIndex + 1, Frames.Count - 1);
                TotalElapsed -= TimePerFrame;

                


                if (sequence.frames[frameIndex].type != Enumeration.TypeFrame.SPRITE)
                {
                    //COMMAND
                    string[] aCommand = sequence.frames[frameIndex].name.Split('|');
                    string[] aParameter = sequence.frames[frameIndex].parameter.Split('|');
                    for (int x = 0; x < aCommand.Length; x++)
                    {
                        if (aCommand[x] == Enumeration.TypeCommand.ABOUTFACE.ToString())
                        {
                            if (spriteEffects == SpriteEffects.FlipHorizontally)
                                spriteEffects = SpriteEffects.None;
                            else
                                spriteEffects = SpriteEffects.FlipHorizontally;
                        }
                        else if (aCommand[x] == Enumeration.TypeCommand.GOTOFRAME.ToString())
                        {
                            string par = aParameter[x];
                            int result = sequence.frames.FindIndex(delegate(Frame f)
                            {
                                return f.name.ToUpper() == par.ToUpper();
                            });
                            frameIndex = result;
                        }
                        else if (aCommand[x] == Enumeration.TypeCommand.GOTOSEQUENCE.ToString())
                        {
                            string par = aParameter[x];
                            Sequence result = lsequence.Find(delegate(Sequence s)
                            {
                                return s.name.ToUpper() == par.ToUpper();
                            });
                            sequence = result;
                            frameIndex = 0;
                            itemState.Add(StateTileElement.Parse(par));
                        }
                        else if (aCommand[x] == Enumeration.TypeCommand.IFGOTOSEQUENCE.ToString())
                        {
                            string par = string.Empty;
                            if (itemState.Value().IfTrue == true)
                                par = aParameter[0];
                            else
                                par = aParameter[1];

                            Sequence result = lsequence.Find(delegate(Sequence s)
                            {
                                return s.name.ToUpper() == par.ToUpper();
                            });
                            sequence = result;
                            frameIndex = 0;
                            itemState.Add(StateTileElement.Parse(par));
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
                //Play Sound
                sequence.frames[frameIndex].PlaySound();


                int flip;
                if (spriteEffects == SpriteEffects.FlipHorizontally)
                    flip = 1;
                else
                    flip = -1;

                position.Value = new Vector2(position.X + (sequence.frames[frameIndex].xOffSet * flip), position.Y + sequence.frames[frameIndex].yOffSet);
                firstTime = false;
            }

        }



        public void DrawTile(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects, float depth, Texture2D texture)
        {
            // Calculate the source rectangle of the current frame.
            Rectangle source = new Rectangle(0, 0, texture.Width, texture.Height);

            // Draw the current tile.
            spriteBatch.Draw(texture, position, source, Color.White, 0.0f, Vector2.Zero, 1.0f, spriteEffects, depth);
        }


        public void DrawTile(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects, float depth)
        {
            // Calculate the source rectangle of the current frame.
                Rectangle source = new Rectangle(0, 0, sequence.frames[frameIndex].texture.Width, sequence.frames[frameIndex].texture.Height);

            //this can draw tiles with over height
            if (source.Height > Tile.REALHEIGHT)
            {
                position.Y -= source.Height - Tile.REALHEIGHT;
            }

               spriteBatch.Draw(sequence.frames[frameIndex].texture, position, source, Color.White, 0.0f, Vector2.Zero, 1.0f, spriteEffects, depth);
        }


        public void DrawTile(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects, float depth, Rectangle rectangleMask, Texture2D texture)
        {

            // Draw the current tile.
            spriteBatch.Draw(sequence.frames[frameIndex].texture, position, rectangleMask, Color.White, 0.0f, Vector2.Zero, 1.0f, spriteEffects, depth);

        }

        public void DrawTileMask(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects, float depth, Rectangle rectangleMask)
        {
            // Calculate the source rectangle of the current frame.
            //128-62
            //148-20
            //Rectangle source = new Rectangle(0, 128, 62, 20);
            //position.Y = position.Y + 128;
            //Rectangle source = new Rectangle(62, 20, sequence.frames[frameIndex].texture.Width, sequence.frames[frameIndex].texture.Height);
            Rectangle source = new Rectangle((int)position.X, (int)position.Y, rectangleMask.Width, rectangleMask.Height);
            // Draw the current tile.
            //spriteBatch.Draw(sequence.frames[frameIndex].texture, null, source, rectangleMask, null, 0f, null, Color.White, spriteEffects, depth);
            spriteBatch.Draw(sequence.frames[frameIndex].texture, position, rectangleMask, Color.White, 0.0f, Vector2.Zero, 1.0f, spriteEffects, depth);
        }


        public void DrawItem(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects, float depth)
        {
            // Calculate the source rectangle of the current frame.
            Rectangle source = new Rectangle(0, 0, sequence.frames[frameIndex].texture.Width, sequence.frames[frameIndex].texture.Height);

            // Draw the current tile.
            spriteBatch.Draw(sequence.frames[frameIndex].texture, position, source, Color.White, 0.0f, Vector2.Zero, 1.0f, spriteEffects, depth);
        }


        public void DrawSprite(GameTime gameTime, SpriteBatch spriteBatch, Position position, SpriteEffects spriteEffects, float depth)
        {

            // Calculate the source rectangle of the current frame.
            Rectangle source = new Rectangle(0, 0, sequence.frames[frameIndex].texture.Height, sequence.frames[frameIndex].texture.Height);

            //position = new Vector2(position.X + Tile.PERSPECTIVE, position.Y - Tile.GROUND);

            // Draw the current frame.
            spriteBatch.Draw(sequence.frames[frameIndex].texture, position.ValueDraw, source, Color.White, 0.0f, Vector2.Zero, 1.0f, spriteEffects, depth);


        }
     
    }






}
