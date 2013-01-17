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
        private Enumeration.State _state;


        public Enumeration.State state
        {
            get { return _state; }
            set { _state = value; }
        }

        public StatePlayerElement()
        {
            _state = Enumeration.State.stand;
            Priority = Enumeration.PriorityState.Normal;
            Reverse = Enumeration.SequenceReverse.Normal;
            OffSet = Vector2.Zero;
        }

        public StatePlayerElement(Enumeration.State state, Enumeration.PriorityState priority)
        {
            _state = state;
            Priority  = priority;
            Reverse = Enumeration.SequenceReverse.Normal;
            OffSet = Vector2.Zero;
        }

        public StatePlayerElement(Enumeration.State state, Enumeration.PriorityState priority, Vector2 offSet)
        {
            _state = state;
            Priority = priority;
            Reverse = Enumeration.SequenceReverse.Normal;
            OffSet = offSet;
        }

        public StatePlayerElement(Enumeration.State state, Enumeration.PriorityState priority, bool? stoppable)
        {
            _state = state;
            Priority = priority;
            Stoppable = stoppable;
            Reverse = Enumeration.SequenceReverse.Normal;
            OffSet = Vector2.Zero;
        }

        public StatePlayerElement(Enumeration.State state, Enumeration.PriorityState priority, Enumeration.SequenceReverse reverse)
        {
            _state = state;
            Priority = priority;
            Reverse = reverse;
            OffSet = Vector2.Zero;
        }


        public StatePlayerElement(Enumeration.State state, Enumeration.PriorityState priority, bool? stoppable, Enumeration.SequenceReverse reverse)
        {
            _state = state;
            Priority = priority;
            Stoppable = stoppable;
            Reverse = reverse;
            OffSet = Vector2.Zero;
        }

        public StatePlayerElement(Enumeration.State state, Enumeration.PriorityState priority, bool? stoppable, Enumeration.SequenceReverse reverse, Vector2 offSet)
        {
            _state = state;
            Priority = priority;
            Stoppable = stoppable;
            Reverse = reverse;
            OffSet = offSet;
        }

        public StatePlayerElement(Enumeration.State state)
        {
            _state = state;
            Priority = Enumeration.PriorityState.Normal;
            Reverse = Enumeration.SequenceReverse.Normal;
            OffSet = Vector2.Zero;
            IfTrue = false;
        }

        public StatePlayerElement(Enumeration.State state, bool iftrue)
        {
            _state = state;
            Priority = Enumeration.PriorityState.Normal;
            Reverse = Enumeration.SequenceReverse.Normal;
            OffSet = Vector2.Zero;
            IfTrue = iftrue;
        }


        public new static Enumeration.State Parse(string name)
        {
            return (Enumeration.State)Enum.Parse(typeof(Enumeration.State), name.ToLower());
        }



    }
   
}
