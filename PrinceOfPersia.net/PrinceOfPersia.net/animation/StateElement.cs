using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;

namespace PrinceOfPersia
{
    public abstract class StateElement
    {
        private State _state;
        private PriorityState _priorityState;
        private bool? _stoppable;
        private SequenceReverse _reverse;
        private Vector2 _offset;
        private bool _ifTrue = false;

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
            none // new derived class of this
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

        public bool IfTrue
        {
            get { return _ifTrue; }
            set { _ifTrue = value; }
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
            get { return _state; }
            set { _state = value; }
        }

        public PriorityState Priority
        {
            get { return _priorityState; }
            set { _priorityState = value; }
        }

        public StateElement()
        {
            _state = State.none;
            _priorityState = PriorityState.Normal;
            _reverse = SequenceReverse.Normal;
            _offset = Vector2.Zero;
        }

        public StateElement(State state, PriorityState priority)
        {
            _state = state;
            _priorityState = priority;
            _reverse = SequenceReverse.Normal;
            _offset = Vector2.Zero;
        }

        public StateElement(State state, PriorityState priority, bool? stoppable)
        {
            _state = state;
            _priorityState = priority;
            _stoppable = stoppable;
            _reverse = SequenceReverse.Normal;
            _offset = Vector2.Zero;
        }

        public StateElement(State state, PriorityState priority, SequenceReverse reverse)
        {
            _state = state;
            _priorityState = priority;
            _reverse = reverse;
            _offset = Vector2.Zero;
        }


        public StateElement(State state, PriorityState priority, bool? stoppable, SequenceReverse reverse)
        {
            _state = state;
            _priorityState = priority;
            _stoppable = stoppable;
            _reverse = reverse;
            _offset = Vector2.Zero;
        }

        public StateElement(State state, PriorityState priority, bool? stoppable, SequenceReverse reverse, Vector2 offSet)
        {
            _state = state;
            _priorityState = priority;
            _stoppable = stoppable;
            _reverse = reverse;
            _offset = offSet;
        }
        public StateElement(State state)
        {
            _state = state;
            _priorityState = PriorityState.Normal;
            _reverse = SequenceReverse.Normal;
            _offset = Vector2.Zero;
        }

        public static StateTileElement.State Parse(string name)
        {
            return (StateTileElement.State)Enum.Parse(typeof(StateTileElement.State), name.ToLower());
        }



    }
   
}
