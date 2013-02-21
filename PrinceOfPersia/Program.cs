using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace PrinceOfPersia
{
    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            try
            {
                using (Game game = new Game())
                {
                    game.Run();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace.ToString());
            }

        }
    }
}

