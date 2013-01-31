#region File Description
//-----------------------------------------------------------------------------
// BackgroundScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GameStateManagement;
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
        Texture2D backgroundTexture;
        Texture2D titleTexture;
        Texture2D presentsTexture;
        Texture2D authorsTexture;
        Texture2D copyrightTexture;
        float TransitionAlphaTitle;
        int numTexture = 0;

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

                backgroundTexture = content.Load<Texture2D>("Backgrounds/main_background");
                presentsTexture = content.Load<Texture2D>("Backgrounds/presents");
                authorsTexture = content.Load<Texture2D>("Backgrounds/author");
                titleTexture = content.Load<Texture2D>("Backgrounds/main_title");
                copyrightTexture = content.Load<Texture2D>("Backgrounds/copyright");

            }
        }


        /// <summary>
        /// Unloads graphics content for this screen.
        /// </summary>
        public override void Unload()
        {
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

            spriteBatch.Draw(backgroundTexture, fullscreen, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));

            if (delay > 0)
            {
                delay = delay - 0.1f;
                if (textureToDisplay != null)
                {
                    TransitionAlphaTitle = TransitionAlphaTitle + 0.05f;
                    spriteBatch.Draw(textureToDisplay, new Rectangle(fullscreen.Width / 2 - textureToDisplay.Width / 2, fullscreen.Height / 2, textureToDisplay.Width, textureToDisplay.Height), new Color(TransitionAlphaTitle, TransitionAlphaTitle, TransitionAlphaTitle));
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
                        break;
                    case 2:
                        textureToDisplay = authorsTexture;
                        break;
                    case 3:
                        textureToDisplay = titleTexture;
                        break;
                    case 4:
                        textureToDisplay = null;
                        numTexture = 0;
                        break;
                }
            }

         
            
            spriteBatch.End();
        }


        #endregion
    }
}
