using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;

namespace PrinceOfPersia
{
    public class StateElement
    {
        private State _playerState;
        private PriorityState _priorityState;
        private bool? _stoppable;
        private SequenceReverse _reverse;
        private Vector2 _offset;

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

        public enum State
        {
            none // my state
            ,crouch //my state
            ,godown //my state invert standup
            , startrun,
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
            stepfall2
                ,
            medland
                ,
            rjumpfall
                ,
            hardland
                ,
            hangfall
                ,
            jumphangLong
                ,
            hangstraight
                ,
            rdiveroll
                ,
            sdiveroll
                ,
            highjump
                ,
            step1
                ,
            step2
                ,
            step3
                ,
            step4
                ,
            step5
                ,
            step6
                ,
            step7
                ,
            step8
                ,
            step9
                ,
            step10
                ,
            step11
                ,
            step12
                ,
            step13
                ,
            fullstep
                ,
            turnrun
                ,
            testfoot
                ,
            bumpfall
                ,
            hardbump
                ,
            bump
                ,
            superhijump
                ,
            standup
                ,
            stoop
                ,
            impale
                ,
            crush
                ,
            deadfall
                ,
            halve
                ,
            engarde
                ,
            advance
                ,
            retreat
                ,
            strike
                ,
            flee
                ,
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

        public enum PriorityState
        {
            Normal,
            Force
        }

        public enum SequenceReverse
        {
            Normal,
            Reverse
        }

        public Vector2 OffSet
        {
            get { return _offset; }
            set { _offset = value; }
        }

        public SequenceReverse Reverse
        {
            get { return _reverse; }
            set { _reverse = value; }
        }

        public bool? Stoppable
        {
            get { return _stoppable; }
            set { _stoppable = value; }
        }

        public State state
        {
            get { return _playerState; }
            set { _playerState = value; }
        }

        public PriorityState priority
        {
            get { return _priorityState; }
            set { _priorityState = value; }
        }

        public StateElement()
        {
            _playerState = State.stand;
            _priorityState = PriorityState.Normal;
            _reverse = SequenceReverse.Normal;
            _offset = Vector2.Zero;
        }

        public StateElement(State state, PriorityState priority)
        {
            _playerState = state;
            _priorityState = priority;
            _reverse = SequenceReverse.Normal;
            _offset = Vector2.Zero;
        }

        public StateElement(State state, PriorityState priority, bool? stoppable)
        {
            _playerState = state;
            _priorityState = priority;
            _stoppable = stoppable;
            _reverse = SequenceReverse.Normal;
            _offset = Vector2.Zero;
        }

        public StateElement(State state, PriorityState priority, SequenceReverse reverse)
        {
            _playerState = state;
            _priorityState = priority;
            _reverse = reverse;
            _offset = Vector2.Zero;
        }


        public StateElement(State state, PriorityState priority, bool? stoppable, SequenceReverse reverse)
        {
            _playerState = state;
            _priorityState = priority;
            _stoppable = stoppable;
            _reverse = reverse;
            _offset = Vector2.Zero;
        }

        public StateElement(State state, PriorityState priority, bool? stoppable, SequenceReverse reverse, Vector2 offSet)
        {
            _playerState = state;
            _priorityState = priority;
            _stoppable = stoppable;
            _reverse = reverse;
            _offset = offSet;
        }
        public StateElement(State state)
        {
            _playerState = state;
            _priorityState = PriorityState.Normal;
            _reverse = SequenceReverse.Normal;
            _offset = Vector2.Zero;
        }

        public static StateElement.State Parse(string name)
        {
            return (StateElement.State)Enum.Parse(typeof(StateElement.State), name.ToLower());
        }



    }
   
}
