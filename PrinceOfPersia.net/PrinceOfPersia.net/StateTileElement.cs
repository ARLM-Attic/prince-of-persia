using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;

namespace PrinceOfPersia
{
    public class StateTileElement
    {
        private State _tileState;
        private PriorityState _priorityState;
        private bool? _stoppable;
        private SequenceReverse _reverse;
        private Vector2 _offset;

        public enum State
        {
            normal // my state
            ,fall
            ,animation
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
            get { return _tileState; }
            set { _tileState = value; }
        }

        public PriorityState priority
        {
            get { return _priorityState; }
            set { _priorityState = value; }
        }

        public StateTileElement()
        {
            _tileState = State.normal;
            _priorityState = PriorityState.Normal;
            _reverse = SequenceReverse.Normal;
            _offset = Vector2.Zero;
        }

        public StateTileElement(State state, PriorityState priority)
        {
            _tileState = state;
            _priorityState = priority;
            _reverse = SequenceReverse.Normal;
            _offset = Vector2.Zero;
        }

        public StateTileElement(State state, PriorityState priority, bool? stoppable)
        {
            _tileState = state;
            _priorityState = priority;
            _stoppable = stoppable;
            _reverse = SequenceReverse.Normal;
            _offset = Vector2.Zero;
        }

        public StateTileElement(State state, PriorityState priority, SequenceReverse reverse)
        {
            _tileState = state;
            _priorityState = priority;
            _reverse = reverse;
            _offset = Vector2.Zero;
        }


        public StateTileElement(State state, PriorityState priority, bool? stoppable, SequenceReverse reverse)
        {
            _tileState = state;
            _priorityState = priority;
            _stoppable = stoppable;
            _reverse = reverse;
            _offset = Vector2.Zero;
        }

        public StateTileElement(State state, PriorityState priority, bool? stoppable, SequenceReverse reverse, Vector2 offSet)
        {
            _tileState = state;
            _priorityState = priority;
            _stoppable = stoppable;
            _reverse = reverse;
            _offset = offSet;
        }
        public StateTileElement(State state)
        {
            _tileState = state;
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
