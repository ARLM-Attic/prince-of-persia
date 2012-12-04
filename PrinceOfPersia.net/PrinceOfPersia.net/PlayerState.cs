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
        private Queue<StateElement> data = new Queue<StateElement>(iSize);

        public PlayerState()
        {
            Add(new StateElement());
        }

        public StateElement.State Next()
        {
            return Next(Value().state);
        }
        public StateElement.State Next(StateElement.State state)
        { 
            switch (state)
            {
                case StateElement.State.freefall:
                    return StateElement.State.crouch;
                default:
                    return StateElement.State.stand;
            }
        
        }


        public void Add(StateElement.State state)
        {
            //if (Value().state == state)
            //    return;

            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StateElement(state));
        }

        public void Add(StateElement.State state, StateElement.PriorityState priority)
        {
            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StateElement(state, priority));
        }

        public void Add(StateElement.State state, StateElement.PriorityState priority, StateElement.SequenceReverse reverse)
        {
            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StateElement(state, priority, reverse));
        }

        public void Add(StateElement.State state, StateElement.PriorityState priority, bool? stoppable)
        {
            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StateElement(state, priority, stoppable, StateElement.SequenceReverse.Normal));
        }

        public void Add(StateElement.State state, StateElement.PriorityState priority, bool? stoppable, Vector2 offSet)
        {
            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StateElement(state, priority, stoppable, StateElement.SequenceReverse.Normal, offSet));
        }

        public void Add(StateElement.State state, StateElement.PriorityState priority, bool? stoppable, StateElement.SequenceReverse reverse)
        {
            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StateElement(state, priority, stoppable, reverse));
        }


        public void Add(StateElement.State state, StateElement.PriorityState priority, bool? stoppable, StateElement.SequenceReverse reverse, Vector2 offSet)
        {
            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StateElement(state, priority, stoppable, reverse, offSet));
        }


        public void Add(StateElement stateElement)
        {
            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(stateElement);
        }

        public StateElement Previous()
        {
            if (data.Count == 0)
            {
                Add(new StateElement());
            }
            return data.First();
        }


        public StateElement Value()
        {
            if (data.Count == 0)
                Add(new StateElement());
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
