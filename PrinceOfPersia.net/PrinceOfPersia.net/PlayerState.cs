using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;

namespace PrinceOfPersia
{

    public class PlayerState
    {

        private const int iSize = 2;
        private Queue<StatePlayerElement> data = new Queue<StatePlayerElement>(iSize);

        public PlayerState()
        {
            Add(new StatePlayerElement());
        }

        public StatePlayerElement.State Next()
        {
            return Next(Value().state);
        }
        public StatePlayerElement.State Next(StatePlayerElement.State state)
        {
            switch (state)
            {
                case StatePlayerElement.State.freefall:
                    return StatePlayerElement.State.crouch;
                default:
                    return StatePlayerElement.State.stand;
            }
        
        }


        public void Add(StatePlayerElement.State state)
        {
            //if (Value().state == state)
            //    return;

            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StatePlayerElement(state));
        }

        public void Add(StatePlayerElement.State state, bool ifTrue)
        {
            //if (Value().state == state)
            //    return;

            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StatePlayerElement(state, ifTrue));
        }

        public void Add(StatePlayerElement.State state, StateElement.PriorityState priority)
        {
            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StatePlayerElement(state, priority));
        }

        public void Add(StatePlayerElement.State state, StateElement.PriorityState priority, StateElement.SequenceReverse reverse)
        {
            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StatePlayerElement(state, priority, reverse));
        }

        public void Add(StatePlayerElement.State state, StateElement.PriorityState priority, bool? stoppable)
        {
            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StatePlayerElement(state, priority, stoppable, StateElement.SequenceReverse.Normal));
        }

        public void Add(StatePlayerElement.State state, StateElement.PriorityState priority, bool? stoppable, Vector2 offSet)
        {
            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StatePlayerElement(state, priority, stoppable, StateElement.SequenceReverse.Normal, offSet));
        }

        public void Add(StatePlayerElement.State state, StateElement.PriorityState priority, bool? stoppable, StateElement.SequenceReverse reverse)
        {
            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StatePlayerElement(state, priority, stoppable, reverse));
        }


        public void Add(StatePlayerElement.State state, StateElement.PriorityState priority, bool? stoppable, StateElement.SequenceReverse reverse, Vector2 offSet)
        {
            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StatePlayerElement(state, priority, stoppable, reverse, offSet));
        }


        public void Add(StatePlayerElement stateElement)
        {
            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(stateElement);
        }

        public StatePlayerElement Previous()
        {
            if (data.Count == 0)
            {
                Add(new StatePlayerElement());
            }
            return data.First();
        }


        public StatePlayerElement Value()
        {
            if (data.Count == 0)
                Add(new StatePlayerElement());
            return data.Last();
        }

        public void Clear()
        {
            data.Clear();
        }

        public IEnumerable<StateElement> GetData()
        {
            // Need to go via array because Queue does not implement IList<T>
            // which ReadOnlyCollection's ctor takes.
            return new ReadOnlyCollection<StateElement>(data.ToArray());
        }
    }
}
