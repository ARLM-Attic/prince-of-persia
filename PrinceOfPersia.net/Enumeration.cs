﻿	//-----------------------------------------------------------------------//
	// <copyright file="Enumeration.cs" company="A.D.F.Software">
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
using System.Linq;
using System.Text;

namespace PrinceOfPersia
{
    public abstract class Enumeration
    {

        public enum LevelName
        {
            dungeon_prison = 1,
            dungeon_guards = 2,
            dungeon_skeleton = 3,
            palace_mirror = 4,
            palace_thief = 5,
            palace_plunge = 6,
            dungeon_weightless = 7,
            dungeon_mouse = 8,
            dungeon_twisty = 9,
            palace_quad = 10,
            palace_fragile = 11,
            dungeon_tower = 12,
            dungeon_jaffar = 13,
            palace_rescue = 14,
            dungeon_potions = 15,
            dungeon_demo = 0
        }

        public enum SpriteType
        {
            nothing,
            kid,
            sultan,
            guard,
            guard2,
            skeleton
        }

        public enum Input
        {
            left,
            right,
            down,
            downshift,
            up,
            upshift,
            leftshift,
            rightshift,
            shift,
            leftup,
            rightup,
            leftdown,
            righdown,
            none
        }

        public enum PriorityState
        {
            Normal,
            Force
        }

        public enum SequenceReverse
        {
            Normal,
            Reverse, //reverse all and reset frame to 0
            FixFrame //don't reset frame counter
        }

        public enum TileCollision
        {
            /// <summary>
            /// A passable tile is one which does not hinder player motion at all, like a space
            /// </summary>
            Passable = 0,

            /// <summary>
            /// An impassable tile is one which does not allow the player to move through
            /// it at all. It is completely solid, like a wall block
            /// </summary>
            Impassable = 1,

            /// <summary>
            /// Standard floor
            /// </summary>
            Platform = 2,
        }

        public enum TileType
        {
            space = 0,
            floor = 1,
            spikes = 2,
            posts = 3,
            gate = 4,
            dpressplate = 5, //;down
            pressplate = 6,// ;up
            panelwif = 7,// ;w/floor  //AMF when loose shake?!?!!?!?
            pillarbottom = 8,
            pillartop = 9,
            flask = 10,
            loose = 11,
            panelwof = 12,// ;w/o floor
            mirror = 13,
            rubble = 14,
            upressplate = 15,
            exit = 16, //left door portion
            exit2 = 17, //right door portion
            chomper = 18,  //original name slicer
            torch = 19,
            block = 20,
            bones = 21,
            sword = 22,
            window = 23,
            window2 = 24,
            archbot = 25,
            archtop1 = 26,
            archtop2 = 27,
            archtop3 = 28,
            archtop4 = 29,
            tile_torch_rubble = 30, //taked from apoplexy norbert
            tile_loose = 43,  //taked from apoplexy norbert
            //door = 100 //changed from apoplexy norbert

            
        }

        //first 4 byte are element type the rest is the modifier, _x tells the result formula
        public enum Tile
        {
            space_1   = 0x00000000,
            space_2   = 0x00000001,
            space_3   = 0x00000002,
            space_4   = 0x00000003,
            space_5   = 0x000000FF, 

            floor_6   = 0x00010000,
            floor_7   = 0x00010001,
            floor_8   = 0x00010002,
            floor_9   = 0x00010003,
            floor_10  = 0x000100FF,

            spike_11  = 0x00020000,
            spike_12  = 0x00020001,
            spike_13  = 0x00020002,
            spike_14  = 0x00020003,
            spike_15  = 0x00020004,
            spike_16  = 0x00020005,
            spike_17  = 0x00020006,
            spike_18  = 0x00020007,
            spike_19  = 0x00020008,
            spike_20  = 0x00020009,

            posts_21  = 0x00030000,
            
            gate_22   = 0x00040000,
            gate_23   = 0x00040001,

            dpressplate_24 = 0x00050000,

            pressplate_25 = 0x00060000,

            panelwif_26 = 0x00070000,
            panelwif_27 = 0x00070001,
            panelwif_28 = 0x00070002,
            panelwif_29 = 0x00070003,

            pillarbottom_30 = 0x00080000,

            pillartop_31 = 0x00090001,

            flask_32 = 0x00100000,
            flask_33 = 0x00100001,
            flask_34 = 0x00100002,
            flask_35 = 0x00100003,
            flask_36 = 0x00100004,
            flask_37 = 0x00100005,
            flask_38 = 0x00100006,

            loose_39 = 0x00110000,

            panelwof_40 = 0x00120000,
            panelwof_41 = 0x00120001,
            panelwof_42 = 0x00120002,
            panelwof_43 = 0x00120003,
            panelwof_44 = 0x00120004,
            panelwof_45 = 0x00120005,
            panelwof_46 = 0x00120006,
            panelwof_47 = 0x00120007,

            mirror_48 = 0x00130000,

            rubble_49 = 0x00140000,

            upressplate_50 = 0x00150000,

            exit_51 = 0x00160000,
            exit2_52 = 0x00170000,

            slicer_53 = 0x00180000,
            slicer_54 = 0x00180001,
            slicer_55 = 0x00180002,
            slicer_56 = 0x00180003,
            slicer_57 = 0x00180004,
            slicer_58 = 0x00180005,

            torch_59 = 0x00190000,

            block_60 = 0x00200000,
            block_61 = 0x00200001,

            bones_62 = 0x00210000,

            sword_63 = 0x00220000,

            window_64 = 0x00230000,
            window2_65 = 0x00240000,

            archbot_66 = 0x00250000,
            archtop1_67 = 0x00260000,
            archtop2_68 = 0x00270000,
            archtop3_69 = 0x00280000,
            archtop4_70 = 0x00290000,

            tile_torch_rubble_71 = 0x00300000,
            tile_loose_72 = 0x00430000,
            
            //Dog = 0x00010000,
            //Cat = 0x00020000,
            //Alsation = Dog | 0x00000001,
            //Greyhound = Dog | 0x00000002,
            //Siamese = Cat | 0x00000001
        }

        public enum Modifier
        { 
            none = 0,
            one = 1,
            two = 2,
            three = 3,
            four = 4,
            five = 5,
            six = 6,
            seven = 7,
            eight = 8,
            nine = 9,
            all = 255
        }

        public enum Items
        { 
            none  //nothing default value
            ,sword
            ,flask
            ,potion
        }

        public enum StateTile
        {
            normal = 0  // normal state
            ,close //close animation
            ,closefast //close fast animation
            ,closed //close
            ,open //opening animation
            ,opened
            ,dpressplate //;down
            ,pressplate // ;up
            ,loose //loose floor
            ,loosefall //when loose fall
            ,rubble  //when floor break
            ,looseshake //when loose shake but dont fall
            ,brick //floor/space with brickwall
            ,mask //mask state
            //,sword //sword tile now item!!
            ,bones //bones
            ,exit
            //,exit_close_left_up
            ,exit2
            //,exit_close_right_up

            //apoplexy xml
            ,back_wall
            ,window
            ,stuck
            ,palace_pattern
            ,empty //flask
            ,kill //this is generic state for tell to kill player if touch it
        }

        //For understand if sprite is near, touch or not the wall in front of him
        public enum RETURN_COLLISION_WALL
        { 
            FAR,
            NEAR,
            TOUCH
        }

        public enum State
        {
            none // my state
            ,question  //my state for interpretation
            ,crouch //my state
            ,godown //my state invert standup
            ,ready //my state guard ready 
            ,sjland //my state for land over floor
            ,startrun,
            stand,
            standjump, //normal jump over the floor and land on floor
            runjump,
            turn,
            runturn,
            stepfall,
            jumphangMed,  //for climb
            hang,
            climbup,
            hangdrop,
            freefall,
            runstop,
            jumpup,
            fallhang,
            jumpbackhang,
            softland,
            jumpfall,
            stepfall2,
            medland,
            rjumpfall,
            hardland,
            hangfall,
            jumphangLong, //for climb
            hangstraight,
            rdiveroll,
            sdiveroll,
            highjump,
            step1,
            step2,
            step3,
            step4,
            step5,
            step6,
            step7,
            step8,
            step9,
            step10,
            step11,
            step12,
            step13,
            fullstep,
            turnrun,
            testfoot,
            bumpfall,
            hardbump,
            bump,
            superhijump,
            standup,
            stoop,
            impale,
            crush,
            deadfall,
            halve,
            //COMBAT ACTION
            engarde,
            advance,
            retreat,
            strike,
            flee,
            turnengarde,
            strikeblock,
            readyblock,
            landengarde,
            bumpengfwd,
            bumpengback,
            blocktostrike,
            strikeadv,
            climbdown,
            blockedstrike,
            climbstairs,
            dropdead,
            stepback,
            climbfail,
            stabbed,
            faststrike,
            strikeret,
            alertstand,
            drinkpotion,
            crawl,
            alertturn,
            fightfall,
            efightfall,
            efightfallfwd,
            running,
            stabkill,
            fastadvance,
            goalertstand,
            arise,
            turndraw,
            guardengarde,
            pickupsword,
            resheathe,
            fastsheathe,
            Pstand,
            Vstand,
            Vwalk,
            Vstop,
            Palert,
            Pback,
            Vexit,
            Mclimb,
            Vraise,
            Plie,
            patchfall,
            Mscurry,
            Mstop,
            Mleave,
            Pembrace,
            Pwaiting,
            Pstroke,
            Prise,
            Pcrouch,
            Pslump, 
            Mraise,
            splash_player, // my state PoP.net 
            splash_enemy, // my state PoP.net
            delete //my state for delete and disrupt a object from memory
        }

        public enum TypeFrame
        {
            SPRITE,
            COMMAND,
        }
        public enum TypeCommand
        {
            GOTOFRAME,
            GOTOSEQUENCE,
            ABOUTFACE,
            IFGOTOSEQUENCE,
            IFGOTOFRAME,
            FUNCTION_BOOL, //call sprite bool return function 
            DELETE //delete the sprite.. future implementation..??!?
        }

        //public enum SpriteEffects
        //{
        //    None = 0,
        //    FlipHorizontally = 1,
        //    FlipVertically = 2,
        //}


    }
}
