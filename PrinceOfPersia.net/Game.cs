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


            graphics.IsFullScreen = bool.TryParse(ConfigurationSettings.AppSettings["CONFIG_FULL_SCREEN"], out PrinceOfPersiaGame.CONFIG_FULL_SCREEN);
            int.TryParse(ConfigurationSettings.AppSettings["CONFIG_SCREEN_WIDTH"], out PrinceOfPersiaGame.CONFIG_SCREEN_WIDTH);
            int.TryParse(ConfigurationSettings.AppSettings["CONFIG_SCREEN_HEIGHT"], out PrinceOfPersiaGame.CONFIG_SCREEN_HEIGHT);





#if WINDOWS
            if (PrinceOfPersiaGame.CONFIG_FULL_SCREEN == true)
            {
                System.Windows.Forms.Screen screen = null;
                foreach (System.Windows.Forms.Screen scr in System.Windows.Forms.Screen.AllScreens)
                {
                    if (scr.Primary == true)
                        screen = scr;
                }

                var scaleX = (float)screen.Bounds.Width / (float)PrinceOfPersiaGame.CONFIG_SCREEN_WIDTH;
                var scaleY = (float)screen.Bounds.Height / (float)PrinceOfPersiaGame.CONFIG_SCREEN_HEIGHT;
                Vector3 _screenScale = new Vector3(scaleX, scaleY, 1.0f);

                PrinceOfPersiaGame.scaleMatrix = Matrix.CreateScale(_screenScale);

                Window.IsBorderless = true;
                Window.Position = new Point(screen.Bounds.X, screen.Bounds.Y);
                graphics.PreferredBackBufferWidth = screen.Bounds.Width;
                graphics.PreferredBackBufferHeight = screen.Bounds.Height;
            }
            else
            {
                graphics.PreferredBackBufferWidth = PrinceOfPersiaGame.CONFIG_SCREEN_WIDTH;
                graphics.PreferredBackBufferHeight = PrinceOfPersiaGame.CONFIG_SCREEN_HEIGHT;
            }
#endif




            graphics.PreferredDepthStencilFormat = Microsoft.Xna.Framework.Graphics.DepthFormat.Depth24Stencil8;
            graphics.ApplyChanges();
            



            // Create the screen factory and add it to the Services
            screenFactory = new ScreenFactory();
            Services.AddService(typeof(IScreenFactory), screenFactory);

            // Create the screen manager component.
            screenManager = new ScreenManager(this);
            Components.Add(screenManager);


            // On Windows and Xbox we just add the initial screens
            AddInitialScreens();



        }



        private void AddInitialScreens()
        {
            // Activate the first screens.
            screenManager.AddScreen(new BackgroundScreen(), null);
            //screenManager.AddScreen(new TextScreen(), null);

            // We have different menus for Windows Phone to take advantage of the touch interface

            screenManager.AddScreen(new MainMenuScreen(), null);

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


    }
}

