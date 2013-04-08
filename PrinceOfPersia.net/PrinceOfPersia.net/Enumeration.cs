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
            skeleton
        }

        public enum Input
        {
            left,
            right,
            down,
            up,
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
            exit = 16,
            exit2 = 17,
            slicer = 18,
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
            door = 30 //AMF 
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
            normal // normal state
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
        }


        public new enum State
        {
            none // my state
            ,question  //my state for interpretation
            ,crouch //my state
            ,godown //my state invert standup
            ,ready //guard ready my state
            ,startrun,
            stand,
            standjump,
            runjump,
            turn,
            runturn,
            stepfall,
            jumphangMed,
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
            jumphangLong,
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
            engarde,
            advance,
            retreat,
            strike,
            flee,
            turnengarde
                ,
            strikeblock
                ,
            readyblock
                ,
            landengarde
                ,
            bumpengfwd
                ,
            bumpengback
                ,
            blocktostrike
                ,
            strikeadv
                ,
            climbdown
                ,
            blockedstrike
                ,
            climbstairs
                ,
            dropdead
                ,
            stepback
                ,
            climbfail
                ,
            stabbed
                ,
            faststrike
                ,
            strikeret
                ,
            alertstand
                ,
            drinkpotion
                ,
            crawl
                ,
            alertturn
                ,
            fightfall
                ,
            efightfall
                ,
            efightfallfwd
                ,
            running
                ,
            stabkill
                ,
            fastadvance
                ,
            goalertstand
                ,
            arise
                ,
            turndraw
                ,
            guardengarde
                ,
            pickupsword
                ,
            resheathe
                ,
            fastsheathe
                ,
            Pstand
                ,
            Vstand
                ,
            Vwalk
                ,
            Vstop
                ,
            Palert
                ,
            Pback
                ,
            Vexit
                ,
            Mclimb
                ,
            Vraise
                ,
            Plie
                ,
            patchfall
                ,
            Mscurry
                ,
            Mstop
                ,
            Mleave
                ,
            Pembrace
                ,
            Pwaiting
                ,
            Pstroke
                ,
            Prise
                ,
            Pcrouch
                ,
            Pslump
                , Mraise
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
            IFGOTOFRAME
        }

        public enum SpriteEffects
        {
            None = 0,
            FlipHorizontally = 1,
            FlipVertically = 2,
        }


    }
}
