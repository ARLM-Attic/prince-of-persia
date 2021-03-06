﻿	//-----------------------------------------------------------------------//
	// <copyright file="Utils.cs" company="A.D.F.Software">
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
using System.IO;
using System.Collections.Generic;
using System.Linq;
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

        public static string StreamToString(Stream stream)
        {
            //stream.Position = 0;
            using (StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

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

            
            
            //NOT IMPLEMENTED TEXTURE... SLICER or CHOMPER
            //if (iElement == 18)
            //{ iElement = 1; }

            //NOT IMPLEMENTED TEXTURE... PILLARTOP
            if (iElement == 9)
            { iElement = 1; }

            //NOT IMPLEMENTED TEXTURE... pillarbottom
            if (iElement == 8)
            { iElement = 1; }

            //NOT IMPLEMENTED TEXTURE... PANELWIF
            if (iElement == 7)
            { iElement = 1; }

            //NOT IMPLEMENTED TEXTURE...panelwof
            if (iElement == 12)
            { iElement = 1; }

            //NOT IMPLEMENTED TEXTURE...archtop...
            if ((iElement >= 26) & (iElement <= 29))
            { iElement = 1; }

            //NOT IMPLEMENTED TEXTURE...some...
            if ((iElement >= 23) & (iElement <= 25))
            { iElement = 1; }

            //NOT IMPLEMENTED TEXTURE...??...
            if (iElement >= 32)
            { iElement = 1; }

            return (Enumeration.TileType)Enum.Parse(typeof(Enumeration.TileType), iElement.ToString());
        }

        public static Enumeration.StateTile ParseStateType(ref Enumeration.TileType tileType, string modifier)
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
                    if (iModifier == 0 | iModifier == 255)
                        stateTile = Enumeration.StateTile.exit;
                    break;

                case Enumeration.TileType.exit2:
                    if (iModifier == 0 | iModifier == 255)
                        tileType = Enumeration.TileType.exit;
                        stateTile = Enumeration.StateTile.exit2;
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
