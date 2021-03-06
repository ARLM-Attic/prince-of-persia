﻿	//-----------------------------------------------------------------------//
	// <copyright file="BackgroundScreen.cs" company="A.D.F.Software">
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


#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

#endregion

namespace PrinceOfPersia
{
    /// <summary>
    /// The background screen sits behind all the other menu screens.
    /// It draws a background image that remains fixed in place regardless
    /// of whatever transitions the screens on top of it may be doing.
    /// </summary>
    class BackgroundScreen : GameScreen
    {
        #region Fields

        float delay;
        ContentManager content;
        Texture2D textureToDisplay = null;
        Texture2D textureToDisplay2line = null;
        Texture2D backgroundTexture;
        Texture2D backgroundToDisplayTexture = null;

        Texture2D titleTexture;
        Texture2D presentsTexture;
        Texture2D authorsTexture;
        Texture2D copyrightTexture;
        Texture2D text0BackgroudTexture;
        Texture2D text1BackgroudTexture;
        Texture2D text2BackgroudTexture;

        float TransitionAlphaTitle;
        int numTexture = 0;
        int numLoops = 2;
        public static Song music;
        

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public BackgroundScreen()
        {
            delay = 10f;
            TransitionOnTime = TimeSpan.FromSeconds(1);
            TransitionOffTime = TimeSpan.FromSeconds(1);
        }


        /// <summary>
        /// Loads graphics content for this screen. The background texture is quite
        /// big, so we use our own local ContentManager to load it. This allows us
        /// to unload before going from the menus into the game itself, wheras if we
        /// used the shared ContentManager provided by the Game class, the content
        /// would remain loaded forever.
        /// </summary>
        public override void Activate(bool instancePreserved)
        {
            if (!instancePreserved)
            {
                if (content == null)
                    content = new ContentManager(ScreenManager.Game.Services, "Content");

                music = content.Load<Song>(PoP.CONFIG_SONGS + "main theme");

                MediaPlayer.IsRepeating = true;
                MediaPlayer.Volume = 1.0f;
                try
                {
                    //MediaPlayer.Play(music);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine(ex.ToString()+"Bug on mediaplayer in monogame 3.2 on some win7.. music off");
                }


                backgroundTexture = content.Load<Texture2D>("backgrounds/main_background");
                presentsTexture = content.Load<Texture2D>("backgrounds/presents");
                authorsTexture = content.Load<Texture2D>("backgrounds/author");
                titleTexture = content.Load<Texture2D>("backgrounds/main_title");
                copyrightTexture = content.Load<Texture2D>("backgrounds/copyright");

                //now for the text and other background like story etc
                text0BackgroudTexture = content.Load<Texture2D>("backgrounds/text_0_background");
                text1BackgroudTexture = content.Load<Texture2D>("backgrounds/text_1_background");
                text2BackgroudTexture = content.Load<Texture2D>("backgrounds/text_2_background");

                backgroundToDisplayTexture = backgroundTexture;

            }
        }


        /// <summary>
        /// Unloads graphics content for this screen.
        /// </summary>
        public override void Unload()
        {
            //stop music 
            MediaPlayer.Stop();

            content.Unload();
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the background screen. Unlike most screens, this should not
        /// transition off even if it has been covered by another screen: it is
        /// supposed to be covered, after all! This overload forces the
        /// coveredByOtherScreen parameter to false in order to stop the base
        /// Update method wanting to transition off.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
        }


        /// <summary>
        /// Draws the background screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
       
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);

            spriteBatch.Begin();

            spriteBatch.Draw(backgroundToDisplayTexture, fullscreen, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));

            if (delay > 0)
            {
                delay = delay - 0.1f;
                if (textureToDisplay != null)
                {
                    TransitionAlphaTitle = TransitionAlphaTitle + 0.05f;
                    spriteBatch.Draw(textureToDisplay, new Rectangle(fullscreen.Width / 2 - textureToDisplay.Width / 2, fullscreen.Height / 2, textureToDisplay.Width, textureToDisplay.Height), new Color(TransitionAlphaTitle, TransitionAlphaTitle, TransitionAlphaTitle));
                    if (textureToDisplay2line != null)
                        spriteBatch.Draw(textureToDisplay2line, new Rectangle(fullscreen.Width / 2 - textureToDisplay2line.Width / 2, fullscreen.Height - textureToDisplay2line.Height, textureToDisplay2line.Width, textureToDisplay2line.Height), new Color(TransitionAlphaTitle, TransitionAlphaTitle, TransitionAlphaTitle));
                }
            }
            else
            {
                numTexture++;
                delay = 20f;
                TransitionAlphaTitle = 0;
                switch (numTexture)
                {
                    case 1:
                        textureToDisplay = presentsTexture;
                        textureToDisplay2line = null;
                        break;
                    case 2:
                        textureToDisplay = authorsTexture;
                        textureToDisplay2line = null;
                        break;
                    case 3:
                        textureToDisplay = titleTexture;
                        textureToDisplay2line = copyrightTexture;
                        delay = 50f;
                        break;
                    case 4:
                        backgroundToDisplayTexture = text0BackgroudTexture;
                        textureToDisplay = null;
                        textureToDisplay2line = null;
                        ResetTransition();
                        delay = 40f;
                        break;
                    case 5:
                        backgroundToDisplayTexture = text1BackgroudTexture;
                        textureToDisplay = null;
                        textureToDisplay2line = null;
                        ResetTransition();
                        delay = 40f;
                        break;
                    case 6:
                        backgroundToDisplayTexture = text2BackgroudTexture;
                        textureToDisplay = null;
                        textureToDisplay2line = null;
                        ResetTransition();
                        delay = 40f;
                        break;

                    default:
                        backgroundToDisplayTexture = backgroundTexture;
                        textureToDisplay = null;
                        textureToDisplay2line = null;
                        ResetTransition();
                        numTexture = 0;
                        numLoops++;
                        break;
                }
            }

         
            
            spriteBatch.End();
        }


        #endregion
    }
}
