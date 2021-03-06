﻿	//-----------------------------------------------------------------------//
	// <copyright file="PoPGame.cs" company="A.D.F.Software">
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
using Microsoft.Xna.Framework;
using System.Configuration;


namespace PrinceOfPersia
{
    /// <summary>
    /// Sample showing how to manage different game states, with transitions
    /// between menu screens, a loading screen, the game itself, and a pause
    /// menu. This main game class is extremely simple: all the interesting
    /// stuff happens in the ScreenManager component.
    /// </summary>
    public class PoPGame : Game
    {
        GraphicsDeviceManager graphics;
        ScreenManager screenManager;
        ScreenFactory screenFactory;

        public int CONFIG_SCREEN_WIDTH = 640;
        public int CONFIG_SCREEN_HEIGHT = 400;


        /// <summary>
        /// The main game constructor.
        /// </summary>
        public PoPGame()
        {
#if ANDROID
            CONFIG_SCREEN_WIDTH = 320;
            CONFIG_SCREEN_HEIGHT = 200;
#else
            int.TryParse(ConfigurationSettings.AppSettings["CONFIG_screen_width"], out CONFIG_SCREEN_WIDTH);
            int.TryParse(ConfigurationSettings.AppSettings["CONFIG_screen_height"], out CONFIG_SCREEN_HEIGHT);
#endif
			Content.RootDirectory = AppDomain.CurrentDomain.BaseDirectory +"Content";

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = CONFIG_SCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = CONFIG_SCREEN_HEIGHT;
            //graphics.IsFullScreen = false;
            graphics.PreferredDepthStencilFormat = Microsoft.Xna.Framework.Graphics.DepthFormat.Depth24Stencil8;

            TargetElapsedTime = TimeSpan.FromTicks(333333);




#if WINDOWS_PHONE
            graphics.IsFullScreen = true;

            // Choose whether you want a landscape or portait game by using one of the two helper functions.
            InitializeLandscapeGraphics();
            // InitializePortraitGraphics();
#endif

                // Create the screen factory and add it to the Services
                screenFactory = new ScreenFactory();
                Services.AddService(typeof(IScreenFactory), screenFactory);

                // Create the screen manager component.
                screenManager = new ScreenManager(this);
                Components.Add(screenManager);

#if WINDOWS_PHONE
            // Hook events on the PhoneApplicationService so we're notified of the application's life cycle
            Microsoft.Phone.Shell.PhoneApplicationService.Current.Launching += 
                new EventHandler<Microsoft.Phone.Shell.LaunchingEventArgs>(GameLaunching);
            Microsoft.Phone.Shell.PhoneApplicationService.Current.Activated += 
                new EventHandler<Microsoft.Phone.Shell.ActivatedEventArgs>(GameActivated);
            Microsoft.Phone.Shell.PhoneApplicationService.Current.Deactivated += 
                new EventHandler<Microsoft.Phone.Shell.DeactivatedEventArgs>(GameDeactivated);
#else
                // On Windows and Xbox we just add the initial screens
                AddInitialScreens();
#endif
       

        }



        private void AddInitialScreens()
        {
            // Activate the first screens.
            screenManager.AddScreen(new BackgroundScreen(), null);
            //screenManager.AddScreen(new TextScreen(), null);

            // We have different menus for Windows Phone to take advantage of the touch interface
#if WINDOWS_PHONE
            screenManager.AddScreen(new PhoneMainMenuScreen(), null);
#else
            screenManager.AddScreen(new MainMenuScreen(), null);
#endif
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);

            // The real drawing happens inside the screen manager component.
            base.Draw(gameTime);
        }

#if WINDOWS_PHONE
        /// <summary>
        /// Helper method to the initialize the game to be a portrait game.
        /// </summary>
        private void InitializePortraitGraphics()
        {
            graphics.PreferredBackBufferWidth = 480;
            graphics.PreferredBackBufferHeight = 800;
        }

        /// <summary>
        /// Helper method to initialize the game to be a landscape game.
        /// </summary>
        private void InitializeLandscapeGraphics()
        {
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;
        }

        void GameLaunching(object sender, Microsoft.Phone.Shell.LaunchingEventArgs e)
        {
            AddInitialScreens();
        }

        void GameActivated(object sender, Microsoft.Phone.Shell.ActivatedEventArgs e)
        {
            // Try to deserialize the screen manager
            if (!screenManager.Activate(e.IsApplicationInstancePreserved))
            {
                // If the screen manager fails to deserialize, add the initial screens
                AddInitialScreens();
            }
        }

        void GameDeactivated(object sender, Microsoft.Phone.Shell.DeactivatedEventArgs e)
        {
            // Serialize the screen manager when the game deactivated
            screenManager.Deactivate();
        }
#endif
    }
}
