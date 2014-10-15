	//-----------------------------------------------------------------------//
	// <copyright file="Game.cs" company="A.D.F.Software">
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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using System.Configuration;

#if ANDROID
using Android.App;
using Android.Content;
#endif

#if WINDOWS
using System.Windows.Forms;
#endif

namespace PrinceOfPersia
{
    /// <summary>
    /// Sample showing how to manage different game states, with transitions
    /// between menu screens, a loading screen, the game itself, and a pause
    /// menu. This main game class is extremely simple: all the interesting
    /// stuff happens in the ScreenManager component.
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        public static GraphicsDeviceManager graphics;
        public static ScreenManager screenManager;
        public static ScreenFactory screenFactory;


        /// <summary>
        /// The main game constructor.
        /// </summary>

        public Game() 
        {
            TargetElapsedTime = TimeSpan.FromTicks(333333);
            Content.RootDirectory = AppDomain.CurrentDomain.BaseDirectory + "Content";
            graphics = new GraphicsDeviceManager(this);

#if ANDROID
            //Context.Resources.DisplayMetrics;
            //var widthInDp = ConvertPixelsToDp(metrics.WidthPixels);
            //var heightInDp = ConvertPixelsToDp(metrics.HeightPixels);

            var scaleX = (float)graphics.PreferredBackBufferWidth / (float)PoP.CONFIG_SCREEN_WIDTH;
            var scaleY = (float)graphics.PreferredBackBufferHeight / (float)PoP.CONFIG_SCREEN_HEIGHT;
            Vector3 _screenScale = new Vector3(scaleX, scaleY, 1.0f);
            //Vector3 _screenScale = Vector3.One;

            int a = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            int b = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;

            PoP.scaleMatrix = Matrix.CreateScale(_screenScale);
            graphics.IsFullScreen = true;
            //graphics.PreferredBackBufferWidth = PrinceOfPersiaGame.CONFIG_SCREEN_WIDTH;
            //graphics.PreferredBackBufferHeight = PrinceOfPersiaGame.CONFIG_SCREEN_HEIGHT;
#else
            graphics.IsFullScreen = bool.TryParse(ConfigurationSettings.AppSettings["CONFIG_FULL_SCREEN"], out PoP.CONFIG_FULL_SCREEN);
            int.TryParse(ConfigurationSettings.AppSettings["CONFIG_SCREEN_WIDTH"], out PoP.CONFIG_SCREEN_WIDTH);
            int.TryParse(ConfigurationSettings.AppSettings["CONFIG_SCREEN_HEIGHT"], out PoP.CONFIG_SCREEN_HEIGHT);

#endif

#if LINUX
            graphics.IsFullScreen = false;
#endif

#if WINDOWS
            if (PoP.CONFIG_FULL_SCREEN == true)
            {
                System.Windows.Forms.Screen screen = null;
                foreach (System.Windows.Forms.Screen scr in System.Windows.Forms.Screen.AllScreens)
                {
                    if (scr.Primary == true)
                        screen = scr;
                }

                var scaleX = (float)screen.Bounds.Width / (float)PoP.CONFIG_SCREEN_WIDTH;
                var scaleY = (float)screen.Bounds.Height / (float)PoP.CONFIG_SCREEN_HEIGHT;
                Vector3 _screenScale = new Vector3(scaleX, scaleY, 1.0f);
                
                PoP.scaleMatrix = Matrix.CreateScale(_screenScale);

                Window.IsBorderless = true;
                //Window.Position = new Point(screen.Bounds.X, screen.Bounds.Y);
                graphics.PreferredBackBufferWidth = screen.Bounds.Width;
                graphics.PreferredBackBufferHeight = screen.Bounds.Height;
            }
            else
            {
                graphics.PreferredBackBufferWidth = PoP.CONFIG_SCREEN_WIDTH;
                graphics.PreferredBackBufferHeight = PoP.CONFIG_SCREEN_HEIGHT;
            }
#endif

#if LINUX
            graphics.PreferredBackBufferWidth = PrinceOfPersiaGame.CONFIG_SCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = PrinceOfPersiaGame.CONFIG_SCREEN_HEIGHT;
#endif


            graphics.PreferredDepthStencilFormat = Microsoft.Xna.Framework.Graphics.DepthFormat.Depth24Stencil8;
            graphics.ApplyChanges();
            

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

