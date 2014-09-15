	//-----------------------------------------------------------------------//
	// <copyright file="TileState.cs" company="A.D.F.Software">
	// Copyright "A.D.F.Software" (c) 2014 All Rights Reserved
	// <author>Andrea M. Falappi</author>
	// <date>Wednesday, September 24, 2014 11:36:49 AM</date>
	// </copyright>
	//
	// * NOTICE:  All information contained herein is, and remains
	// * the property of Andrea M. Falappi and its suppliers,
	// * if any.  The intellectual and technical concepts contained
	// * herein are proprietary to A.D.F.Software
	// * and its suppliers and may be covered by World Wide and Foreign Patents,
	// * patents in process, and are protected by trade secret or copyright law.
	// * Dissemination of this information or reproduction of this material
	// * is strictly forbidden unless prior written permission is obtained
	// * from Andrea M. Falappi.
	//-----------------------------------------------------------------------//

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

        public Enumeration.StateTile Next()
        {
            return Next(Value().state);
        }
        public Enumeration.StateTile Next(Enumeration.StateTile state)
        {
            switch (state)
            {
                //case Enumeration.StateTile.freefall:
                //    return Enumeration.StateTile.crouch;
                default:
                    return Enumeration.StateTile.normal;
            }
        
        }


        public void Add(Enumeration.StateTile state)
        {
            //if (Value().state == state)
            //    return;

            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StateTileElement(state));
        }

        public void Add(Enumeration.StateTile state, bool ifTrue)
        {
            //if (Value().state == state)
            //    return;

            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StateTileElement(state, ifTrue));
        }

        public void Add(Enumeration.StateTile state, Enumeration.PriorityState priority)
        {
            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StateTileElement(state, priority));
        }

        public void Add(Enumeration.StateTile state, Enumeration.PriorityState priority, Enumeration.SequenceReverse reverse)
        {
            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StateTileElement(state, priority, reverse));
        }

        public void Add(Enumeration.StateTile state, Enumeration.PriorityState priority, bool? stoppable)
        {
            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StateTileElement(state, priority, stoppable, Enumeration.SequenceReverse.Normal));
        }

        public void Add(Enumeration.StateTile state, Enumeration.PriorityState priority, bool? stoppable, Vector2 offSet)
        {
            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StateTileElement(state, priority, stoppable, Enumeration.SequenceReverse.Normal, offSet));
        }

        public void Add(Enumeration.StateTile state, Enumeration.PriorityState priority, bool? stoppable, Enumeration.SequenceReverse reverse)
        {
            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StateTileElement(state, priority, stoppable, reverse));
        }


        public void Add(Enumeration.StateTile state, Enumeration.PriorityState priority, bool? stoppable, Enumeration.SequenceReverse reverse, Vector2 offSet)
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
