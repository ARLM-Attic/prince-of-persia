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
        private Enumeration.StateTile _state;

        public Enumeration.StateTile state
        {
            get { return _state; }
            set { _state = value; }
        }


        public StateTileElement()
        {
            _state = Enumeration.StateTile.normal;
            Priority = Enumeration.PriorityState.Normal;
            Reverse = Enumeration.SequenceReverse.Normal;
            OffSet = Vector2.Zero;
        }

        public StateTileElement(Enumeration.StateTile state, Enumeration.PriorityState priority)
        {
            _state = state;
            Priority  = priority;
            Reverse = Enumeration.SequenceReverse.Normal;
            OffSet = Vector2.Zero;
        }

        public StateTileElement(Enumeration.StateTile state, Enumeration.PriorityState priority, bool? stoppable)
        {
            _state = state;
            Priority = priority;
            Stoppable = stoppable;
            Reverse = Enumeration.SequenceReverse.Normal;
            OffSet = Vector2.Zero;
        }

        public StateTileElement(Enumeration.StateTile state, Enumeration.PriorityState priority, Enumeration.SequenceReverse reverse)
        {
            _state = state;
            Priority = priority;
            Reverse = reverse;
            OffSet = Vector2.Zero;
        }


        public StateTileElement(Enumeration.StateTile state, Enumeration.PriorityState priority, bool? stoppable, Enumeration.SequenceReverse reverse)
        {
            _state = state;
            Priority = priority;
            Stoppable = stoppable;
            Reverse = reverse;
            OffSet = Vector2.Zero;
        }

        public StateTileElement(Enumeration.StateTile state, Enumeration.PriorityState priority, bool? stoppable, Enumeration.SequenceReverse reverse, Vector2 offSet)
        {
            _state = state;
            Priority = priority;
            Stoppable = stoppable;
            Reverse = reverse;
            OffSet = offSet;
        }
        
        public StateTileElement(Enumeration.StateTile state)
        {
            _state = state;
            Priority = Enumeration.PriorityState.Normal;
            Reverse = Enumeration.SequenceReverse.Normal;
            OffSet = Vector2.Zero;
            IfTrue = false;
        }

        public StateTileElement(Enumeration.StateTile state, bool iftrue)
        {
            _state = state;
            Priority = Enumeration.PriorityState.Normal;
            Reverse = Enumeration.SequenceReverse.Normal;
            OffSet = Vector2.Zero;
            IfTrue = iftrue;
        }

    }
   
}
