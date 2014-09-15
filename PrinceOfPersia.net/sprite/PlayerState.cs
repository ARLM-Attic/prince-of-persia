	//-----------------------------------------------------------------------//
	// <copyright file="PlayerState.cs" company="A.D.F.Software">
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

    public class PlayerState
    {

        private const int iSize = 2;
        private Queue<StatePlayerElement> data = new Queue<StatePlayerElement>(iSize);

        public PlayerState()
        {
            Add(new StatePlayerElement());
        }

        public Enumeration.State Next()
        {
            return Next(Value().state);
        }

        public Enumeration.State Next(Enumeration.State state)
        {
            switch (state)
            {
                case Enumeration.State.freefall:
                    return Enumeration.State.crouch;
                default:
                    return Enumeration.State.stand;
            }

        }

        public void Add(Enumeration.State state)
        {
            //if (Value().state == state)
            //    return;

            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StatePlayerElement(state));
        }


        public void Add(Enumeration.State state, bool ifTrue)
        {
            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StatePlayerElement(state, ifTrue));
        }

        public void Add(Enumeration.State state, Enumeration.PriorityState priority)
        {
            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StatePlayerElement(state, priority));
        }

        public void Add(Enumeration.State state, Enumeration.PriorityState priority, Vector2 offSet)
        {
            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StatePlayerElement(state, priority, offSet));
        }

        public void Add(Enumeration.State state, Enumeration.PriorityState priority, Vector2 offSet, Vector2 offSetTotal)
        {
            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StatePlayerElement(state, priority, offSet, offSetTotal));
        }


        public void Add(Enumeration.State state, Enumeration.PriorityState priority, Enumeration.SequenceReverse reverse)
        {
            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StatePlayerElement(state, priority, reverse));
        }

        public void Add(Enumeration.State state, Enumeration.PriorityState priority, bool? stoppable)
        {
            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StatePlayerElement(state, priority, stoppable, Enumeration.SequenceReverse.Normal));
        }

        public void Add(Enumeration.State state, Enumeration.PriorityState priority, bool? stoppable, Vector2 offSet)
        {
            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StatePlayerElement(state, priority, stoppable, Enumeration.SequenceReverse.Normal, offSet));
        }

        public void Add(Enumeration.State state, Enumeration.PriorityState priority, bool? stoppable, Enumeration.SequenceReverse reverse)
        {
            if (data.Count == iSize)
            {
                data.Dequeue();
            }

            data.Enqueue(new StatePlayerElement(state, priority, stoppable, reverse));
        }


        public void Add(Enumeration.State state, Enumeration.PriorityState priority, bool? stoppable, Enumeration.SequenceReverse reverse, Vector2 offSet)
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

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Collections.ObjectModel;
//using Microsoft.Xna.Framework;

//namespace PrinceOfPersia
//{

//    public class PlayerState 
//    {
//        private const int iSize = 2;
//        private Queue<StatePlayerElement> data = new Queue<StatePlayerElement>(iSize);

//        public PlayerState()
//        {
//            //Add(new StatePlayerElement());
//        }

      
        
//        public Enumeration.State Next(Enumeration.State state)
//        {
//            switch (state)
//            {
//                case Enumeration.State.freefall:
//                    return Enumeration.State.crouch;
//                default:
//                    return Enumeration.State.stand;
//            }
        
//        }
        

//        public void Add(Enumeration.State state)
//        {
//            //if (data.Count == iSize)
//            //{
//            //    data.Dequeue();
//            //}

//            data.Enqueue(new StatePlayerElement(state));
//        }

//        public void Add(Enumeration.State state, bool ifTrue)
//        {
//            //if (data.Count == iSize)
//            //{
//            //    data.Dequeue();
//            //}

//            data.Enqueue(new StatePlayerElement(state, ifTrue));
//        }

//        public void Add(Enumeration.State state, Enumeration.PriorityState priority)
//        {
//            //if (data.Count == iSize)
//            //{
//            //    data.Dequeue();
//            //}

//            data.Enqueue(new StatePlayerElement(state, priority));
//        }

//        public void Add(Enumeration.State state, Enumeration.PriorityState priority, Vector2 offSet)
//        {
//            //if (data.Count == iSize)
//            //{
//            //    data.Dequeue();
//            //}

//            data.Enqueue(new StatePlayerElement(state, priority, offSet));
//        }

//        public void Add(Enumeration.State state, Enumeration.PriorityState priority, Enumeration.SequenceReverse reverse)
//        {
//            //if (data.Count == iSize)
//            //{
//            //    data.Dequeue();
//            //}

//            data.Enqueue(new StatePlayerElement(state, priority, reverse));
//        }

//        public void Add(Enumeration.State state, Enumeration.PriorityState priority, bool? stoppable)
//        {
//            //if (data.Count == iSize)
//            //{
//            //    data.Dequeue();
//            //}

//            data.Enqueue(new StatePlayerElement(state, priority, stoppable, Enumeration.SequenceReverse.Normal));
//        }

//        public void Add(Enumeration.State state, Enumeration.PriorityState priority, bool? stoppable, Vector2 offSet)
//        {
//            //if (data.Count == iSize)
//            //{
//            //    data.Dequeue();
//            //}

//            data.Enqueue(new StatePlayerElement(state, priority, stoppable, Enumeration.SequenceReverse.Normal, offSet));
//        }

//        public void Add(Enumeration.State state, Enumeration.PriorityState priority, bool? stoppable, Enumeration.SequenceReverse reverse)
//        {
//            //if (data.Count == iSize)
//            //{
//            //    data.Dequeue();
//            //}

//            data.Enqueue(new StatePlayerElement(state, priority, stoppable, reverse));
//        }


//        public void Add(Enumeration.State state, Enumeration.PriorityState priority, bool? stoppable, Enumeration.SequenceReverse reverse, Vector2 offSet)
//        {
//            //if (data.Count == iSize)
//            //{
//            //    data.Dequeue();
//            //}

//            data.Enqueue(new StatePlayerElement(state, priority, stoppable, reverse, offSet));
//        }


//        public void Add(StatePlayerElement stateElement)
//        {
//            //if (data.Count == iSize)
//            //{
//            //    data.Dequeue();
//            //}

//            data.Enqueue(stateElement);
//        }

//        public StatePlayerElement Previous()
//        {
//            if (data.Count == 0)
//            {
//                StatePlayerElement statePlayerElement = new StatePlayerElement();
//                statePlayerElement.state = Enumeration.State.stand;
//                Add(statePlayerElement); 
//            }

//            if (data.Count >= 2)
//            {
//                return data.ElementAt(1);
//            }
//            else
//                return data.First();
            
//        }


//        public StatePlayerElement Current()
//        {
//            if (data.Count == 0)
//                Add(new StatePlayerElement());

//            return data.First();
//        }

//        public void Clear()
//        {
//            data.Clear();
//        }

//        public StatePlayerElement Dequeue()
//        {
//            if (data.Count == 0)
//            {
//                StatePlayerElement statePlayerElement = new StatePlayerElement();
//                statePlayerElement.state = Enumeration.State.stand;
//                Add(statePlayerElement); 
//            }


//            return data.Dequeue();
//        }


//        public StatePlayerElement[] Values()
//        {
//            return data.ToArray();
//        }

//        public IEnumerable<StateElement> GetData()
//        {
//            // Need to go via array because Queue does not implement IList<T>
//            // which ReadOnlyCollection's takes.
//            return new ReadOnlyCollection<StateElement>(data.ToArray());
//        }
//    }
//}
