using System;

namespace PrinceOfPersia
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (PrinceOfPersiaGame game = new PrinceOfPersiaGame())
            {
                game.Run();
            }
        }
    }
}

