using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;

namespace PrinceOfPersia
{
    public class StatePlayerElement : StateElement  
    {
        private State _state;

        public new enum State
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


        public new State state
        {
            get { return _state; }
            set { _state = value; }
        }

        public StatePlayerElement()
        {
            _state = State.stand;
            Priority = PriorityState.Normal;
            Reverse = SequenceReverse.Normal;
            OffSet = Vector2.Zero;
        }

        public StatePlayerElement(State state, PriorityState priority)
        {
            _state = state;
            Priority  = priority;
            Reverse = SequenceReverse.Normal;
            OffSet = Vector2.Zero;
        }

        public StatePlayerElement(State state, PriorityState priority, bool? stoppable)
        {
            _state = state;
            Priority = priority;
            Stoppable = stoppable;
            Reverse = SequenceReverse.Normal;
            OffSet = Vector2.Zero;
        }

        public StatePlayerElement(State state, PriorityState priority, SequenceReverse reverse)
        {
            _state = state;
            Priority = priority;
            Reverse = reverse;
            OffSet = Vector2.Zero;
        }


        public StatePlayerElement(State state, PriorityState priority, bool? stoppable, SequenceReverse reverse)
        {
            _state = state;
            Priority = priority;
            Stoppable = stoppable;
            Reverse = reverse;
            OffSet = Vector2.Zero;
        }

        public StatePlayerElement(State state, PriorityState priority, bool? stoppable, SequenceReverse reverse, Vector2 offSet)
        {
            _state = state;
            Priority = priority;
            Stoppable = stoppable;
            Reverse = reverse;
            OffSet = offSet;
        }
        public StatePlayerElement(State state)
        {
            _state = state;
            Priority = PriorityState.Normal;
            Reverse = SequenceReverse.Normal;
            OffSet = Vector2.Zero;
        }

        public new static StatePlayerElement.State Parse(string name)
        {
            return (StatePlayerElement.State)Enum.Parse(typeof(StatePlayerElement.State), name.ToLower());
        }



    }
   
}
