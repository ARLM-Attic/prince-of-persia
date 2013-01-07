using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;

namespace PrinceOfPersia
{

    public class TileState
    {

        private const int iSize = 2;
        private Queue<StateTileElement> data = new Queue<StateTileElement>(iSize);

        public TileState()
        {
            Add(new StateTileElement());
        }

        public StateTileElement.State Next()
        {
            return Next(Value().state);
        }
        public StateTileElement.State Next(StateTileElement.State state)
        {
            switch (state)
            {
                //case StateTileElement.State.freefall:
                //    return StateTileElement.State.crouch;
                default:
                    return StateTileElement.State.normal;
            }
        
        }


        public void Add(StateTileElement.State state)
        {
            //if (Value().state == state)
            //    return;

            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StateTileElement(state));
        }

        public void Add(StateTileElement.State state, bool ifTrue)
        {
            //if (Value().state == state)
            //    return;

            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StateTileElement(state, ifTrue));
        }

        public void Add(StateTileElement.State state, StateElement.PriorityState priority)
        {
            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StateTileElement(state, priority));
        }

        public void Add(StateTileElement.State state, StateElement.PriorityState priority, StateElement.SequenceReverse reverse)
        {
            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StateTileElement(state, priority, reverse));
        }

        public void Add(StateTileElement.State state, StateElement.PriorityState priority, bool? stoppable)
        {
            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StateTileElement(state, priority, stoppable, StateElement.SequenceReverse.Normal));
        }

        public void Add(StateTileElement.State state, StateElement.PriorityState priority, bool? stoppable, Vector2 offSet)
        {
            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StateTileElement(state, priority, stoppable, StateElement.SequenceReverse.Normal, offSet));
        }

        public void Add(StateTileElement.State state, StateElement.PriorityState priority, bool? stoppable, StateElement.SequenceReverse reverse)
        {
            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StateTileElement(state, priority, stoppable, reverse));
        }


        public void Add(StateTileElement.State state, StateElement.PriorityState priority, bool? stoppable, StateElement.SequenceReverse reverse, Vector2 offSet)
        {
            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StateTileElement(state, priority, stoppable, reverse, offSet));
        }


        public void Add(StateTileElement stateElement)
        {
            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(stateElement);
        }

        public StateTileElement Previous()
        {
            if (data.Count == 0)
            {
                Add(new StateTileElement());
            }
            return data.First();
        }


        public StateTileElement Value()
        {
            if (data.Count == 0)
                Add(new StateTileElement());
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
