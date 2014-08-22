using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Content;

namespace PrinceOfPersia
{
    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Game game = new Game())
            {
                game.Run();
            }
        }



        public static Dictionary<String, T> LoadContent<T>(this ContentManager contentManager, string contentFolder)
        {
            Dictionary<String, T> result = new Dictionary<String, T>();
            string key = string.Empty;
#if ANDROID
            key = "Tiles/BlockA";
            result[key] = contentManager.Load<T>(key);
            key = "Tiles/GateA";
            result[key] = contentManager.Load<T>(key);
            key = "Tiles/SpaceA";
            result[key] = contentManager.Load<T>(key);
            key = "Tiles/SpaceB";
            result[key] = contentManager.Load<T>(key);
            return result;
#else
            //Load directory info, abort if none
			System.Console.WriteLine("---------------------------------------------");
			System.Console.WriteLine(contentManager.RootDirectory);
			DirectoryInfo dir = new DirectoryInfo(contentManager.RootDirectory);
			System.Console.WriteLine("---------------------------------------------");
            if (!dir.Exists)
                throw new DirectoryNotFoundException();



            var files = Directory
				.GetFiles(contentManager.RootDirectory + "/" + contentFolder, "*.xnb", SearchOption.AllDirectories)
            //.Where(file => file.ToLower().EndsWith("xnb") || file.ToLower().EndsWith("xml"))
            .ToList();


            foreach (object f in files)
            {
                key = string.Empty;
                string fileName = f.ToString().Replace('\\','/');
                int fileExtPos = fileName.LastIndexOf(".");
                if (fileExtPos >= 0)
                    fileName = fileName.Substring(0, fileExtPos);
                //remove contentManager.RootDirectory

                fileExtPos = fileName.LastIndexOf(contentManager.RootDirectory) + (contentManager.RootDirectory + "/").Length;
                if (fileExtPos >= 0)
                    key = fileName.Substring(fileExtPos);

      

                try
                {
                    //bug in the monogame load song, i will add the wav extension??!?!?
                    if (key.Contains("Songs/") == true)
                    {
                        result[key] = (T)(object)contentManager.Load<Song>(key+".wav");
                    }
                    else
                        result[key] = contentManager.Load<T>(key);
                }
                catch(Exception ex)
                {}
                //result[f.N] = contentManager.Load<T>(sFolder + key); 
            }
 
            return result;
#endif
        }

    }
}

