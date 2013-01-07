using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;

namespace PrinceOfPersia
{
    public class StateTileElement : StateElement
    {
        private State _state;

        public new enum State
        {
            normal // normal state
            //,fall
            ,close
            ,open
            ,dpressplate, //;down
            pressplate, // ;up
        }

        public new State state
        {
            get { return _state; }
            set { _state = value; }
        }


        public StateTileElement()
        {
            _state = State.normal;
            Priority = PriorityState.Normal;
            Reverse = SequenceReverse.Normal;
            OffSet = Vector2.Zero;
        }

        public StateTileElement(State state, PriorityState priority)
        {
            _state = state;
            Priority  = priority;
            Reverse = SequenceReverse.Normal;
            OffSet = Vector2.Zero;
        }

        public StateTileElement(State state, PriorityState priority, bool? stoppable)
        {
            _state = state;
            Priority = priority;
            Stoppable = stoppable;
            Reverse = SequenceReverse.Normal;
            OffSet = Vector2.Zero;
        }

        public StateTileElement(State state, PriorityState priority, SequenceReverse reverse)
        {
            _state = state;
            Priority = priority;
            Reverse = reverse;
            OffSet = Vector2.Zero;
        }


        public StateTileElement(State state, PriorityState priority, bool? stoppable, SequenceReverse reverse)
        {
            _state = state;
            Priority = priority;
            Stoppable = stoppable;
            Reverse = reverse;
            OffSet = Vector2.Zero;
        }

        public StateTileElement(State state, PriorityState priority, bool? stoppable, SequenceReverse reverse, Vector2 offSet)
        {
            _state = state;
            Priority = priority;
            Stoppable = stoppable;
            Reverse = reverse;
            OffSet = offSet;
        }
        
        public StateTileElement(State state)
        {
            _state = state;
            Priority = PriorityState.Normal;
            Reverse = SequenceReverse.Normal;
            OffSet = Vector2.Zero;
            IfTrue = false;
        }

        public StateTileElement(State state, bool iftrue)
        {
            _state = state;
            Priority = PriorityState.Normal;
            Reverse = SequenceReverse.Normal;
            OffSet = Vector2.Zero;
            IfTrue = iftrue;
        }

    }
   
}
