	//-----------------------------------------------------------------------//
	// <copyright file="Program.cs" company="A.D.F.Software">
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

        public static int LoaderCount = 0;
        public static bool LoaderFinish = false;
        //public static Game game = null;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        [STAThread]
        static void Main()
        {
            try
            {
            using (var game = new Game())
                game.Run();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
            }
        }






        public static Dictionary<String, T> LoadContent<T>(this ContentManager contentManager)
        {
            Dictionary<String, T> result = new Dictionary<String, T>();
            string key = string.Empty;
            string extension = string.Empty;
#if ANDROID

            string sResults = string.Empty;
            var filePath = Path.Combine("", "results.txt");
            using (var stream = TitleContainer.OpenStream(filePath))
            {
                sResults =  Utils.StreamToString(stream);
                stream.Close();
            }
            
            string[] sArray =  sResults.Split('\r');

            int xCount = 0;
            for (int x = 0; x < sArray.Count(); x++ )
            {
                LoaderCount = (int)Math.Round((double)(100 * ++xCount) / sArray.Count());
                int a = sArray[x].IndexOf("Content\\");
                if (a == -1)
                    continue;
                extension = sArray[x].Substring(sArray[x].Length - 4, 4);

                sArray[x] = sArray[x].Remove(0, a + "Content\\".Length);
                if (extension != ".xnb")
                    if (extension != ".fnt")
                        continue;

                sArray[x]  = sArray[x].Replace('\\', '/');
                key = sArray[x].Substring(0, sArray[x].Length - 4);
                
                if (extension == ".xnb")
                    result[key] = contentManager.Load<T>(key);
                else
                    result[key] = (T)(object)FontLoader.Load(PoP.CONFIG_PATH_CONTENT + key + extension);
                
            }
             
#else
            //Load directory info, abort if none
			System.Console.WriteLine("---------------------------------------------");
			System.Console.WriteLine(contentManager.RootDirectory);
			DirectoryInfo dir = new DirectoryInfo(contentManager.RootDirectory);
			System.Console.WriteLine("---------------------------------------------");
            if (!dir.Exists)
                throw new DirectoryNotFoundException();



            var files = Directory
				.GetFiles(contentManager.RootDirectory, "*.*", SearchOption.AllDirectories)
            .Where(file => file.ToLower().EndsWith("xnb") || file.ToLower().EndsWith("fnt"))
            .ToList();


            int xCount = 0;
            foreach (object f in files)
            {
                LoaderCount = (int)Math.Round((double)(100 * ++xCount) / files.Count);

                key = string.Empty;
                string fileName = f.ToString().Replace('\\','/');
                extension = fileName.Substring(fileName.Length - 4);

                int fileExtPos = fileName.LastIndexOf(".");
                if (fileExtPos >= 0)
                    fileName = fileName.Substring(0, fileExtPos);
                //remove contentManager.RootDirectory

                fileExtPos = fileName.LastIndexOf(contentManager.RootDirectory) + (contentManager.RootDirectory + "/").Length;
                if (fileExtPos > 1)
                    key = fileName.Substring(fileExtPos);


                try
                {
                    //bug in the monogame load song, i will add the wav extension??!?!?
                    if (extension.Contains(".wav") == true)
                        result[key] = (T)(object)contentManager.Load<Song>(key + extension);
                    else if (extension.Contains(".fnt") == true)
                    {
                        result[key] = (T)(object)FontLoader.Load(PoP.CONFIG_PATH_CONTENT + key + extension);
                    }
                    else
                        result[key] = contentManager.Load<T>(key);
                }
                catch(Exception ex)
                {
                    System.Console.WriteLine(ex.ToString());
                }
                //result[f.N] = contentManager.Load<T>(sFolder + key); 
            }
 
            
#endif
            
            return result;
        }
        
    }
}

