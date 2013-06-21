using System;

namespace PrinceOfPersiaEditor
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 

        public static frmMain o = new frmMain();

        [STAThread] 
        static void Main(string[] args)
        {

           
            o.Show();
            


           
            //using (Game1 game = new Game1())
            //{
            //    //game.Run();
                
            //}
        }
    }
#endif
}

