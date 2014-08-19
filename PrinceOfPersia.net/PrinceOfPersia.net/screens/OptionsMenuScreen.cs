#region File Description
//-----------------------------------------------------------------------------
// OptionsMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
#endregion

namespace PrinceOfPersia
{
    /// <summary>
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    class OptionsMenuScreen : MenuScreen
    {
        #region Fields

        //MenuEntry ungulateMenuEntry;
        MenuEntry languageMenuEntry;
        //MenuEntry frobnicateMenuEntry;
        //MenuEntry elfMenuEntry;

        //enum Ungulate
        //{
        //    BactrianCamel,
        //    Dromedary,
        //    Llama,
        //}

        //static Ungulate currentUngulate = Ungulate.Dromedary;

        static string[] languages = { "Italian", "English"};
        static int currentLanguage = 1;

        static bool frobnicate = true;

        //static int elf = 23;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public OptionsMenuScreen()
            : base("Options")
        {
            // Create our menu entries.
            languageMenuEntry = new MenuEntry(string.Empty);

            SetMenuEntryText();

            MenuEntry back = new MenuEntry("Back");

            // Hook up menu event handlers.
            languageMenuEntry.Selected += LanguageMenuEntrySelected;
            back.Selected += OnCancel;
            
            // Add entries to the menu.
            MenuEntries.Add(languageMenuEntry);
            MenuEntries.Add(back);
        }


        /// <summary>
        /// Fills in the latest values for the options screen menu text.
        /// </summary>
        void SetMenuEntryText()
        {
            languageMenuEntry.Text = "Language: " + languages[currentLanguage];
        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Event handler for when the Language menu entry is selected.
        /// </summary>
        void LanguageMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            currentLanguage = (currentLanguage + 1) % languages.Length;

            SetMenuEntryText();
        }



        #endregion
    }
}
