using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PrinceOfPersia
{
    public class Utils
    {

        public static Enumeration.TileType ParseTileType(string element)
        {
            int iElement = int.Parse(element);

            //Norbert one exception XML prince original format
            if (iElement == 43)
            { iElement = 11; }  

            if (iElement > 32)
            { iElement = iElement - 32; }

            //NOT IMPLEMENTED TEXTURE...RUBBLLELEL
            if (iElement == 30)
            { iElement = 19; }

            //NOT IMPLEMENTED TEXTURE... BONES
            if (iElement == 21)
            { iElement = 19; }

            //NOT IMPLEMENTED TEXTURE... UPRESSPLATE
            if (iElement == 15)
            { iElement = 6; }

            //NOT IMPLEMENTED TEXTURE... EXIT2
            if (iElement == 17)
            { iElement = 16; }

            return (Enumeration.TileType)Enum.Parse(typeof(Enumeration.TileType), iElement.ToString());
        }

        public static Enumeration.StateTile ParseStateType(Enumeration.TileType tileType, string modifier)
        {
            int iModifier = int.Parse(modifier);
            Enumeration.StateTile stateTile = Enumeration.StateTile.normal;

            switch (tileType)
            { 
                case Enumeration.TileType.space :
                    if (iModifier == 0 | iModifier == 255)
                        stateTile = Enumeration.StateTile.normal;
                    if (iModifier >= 1 & iModifier <= 2)
                        //stateTile = Enumeration.StateTile.back_wall; //to be implement TEXTURE...
                        stateTile = Enumeration.StateTile.normal;
                    if (iModifier == 3)
                        //stateTile = Enumeration.StateTile.window;
                        stateTile = Enumeration.StateTile.normal; //to be implement TEXTURE...

                    break;

                case Enumeration.TileType.floor:
                    if (iModifier == 0 | iModifier == 255)
                        stateTile = Enumeration.StateTile.normal;
                    if (iModifier >= 1 & iModifier <= 3)
                        //stateTile = Enumeration.StateTile.back_wall;
                        stateTile = Enumeration.StateTile.normal;
                    break;

                case Enumeration.TileType.spikes:
                    if (iModifier == 0 | iModifier == 255)
                        stateTile = Enumeration.StateTile.normal;
                    if (iModifier == 9)
                        stateTile = Enumeration.StateTile.stuck;
                    break;

                case Enumeration.TileType.block:
                    if (iModifier == 0 | iModifier == 255)
                        stateTile = Enumeration.StateTile.normal;
                    if (iModifier == 1)
                        //stateTile = Enumeration.StateTile.palace_pattern;//to be implement TEXTURE...
                        stateTile = Enumeration.StateTile.normal;

                    break;

                case Enumeration.TileType.flask:
                    if (iModifier == 0 | iModifier == 255)
                        stateTile = Enumeration.StateTile.empty;

                    break;

                case Enumeration.TileType.exit:
                case Enumeration.TileType.exit2:
                    if (iModifier == 0 | iModifier == 255)
                        stateTile = Enumeration.StateTile.exit_close_left;

                    break;

                case Enumeration.TileType.pressplate:
                    stateTile = Enumeration.StateTile.normal;
                    break;
                
                case Enumeration.TileType.upressplate:
                    stateTile = Enumeration.StateTile.dpressplate;

                    break;
                default:
                    break;
            
            }

            return stateTile;
        }

        public static int ParseSwitchButton(Enumeration.TileType tileType, string modifier)
        {
            int iModifier = int.Parse(modifier);

            if (tileType == (Enumeration.TileType.pressplate & Enumeration.TileType.upressplate))
            {
                return iModifier+1;
            }

            return 0;
        }
    }
}
