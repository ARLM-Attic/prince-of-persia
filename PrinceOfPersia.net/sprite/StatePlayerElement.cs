	//-----------------------------------------------------------------------//
	// <copyright file="StatePlayerElement.cs" company="A.D.F.Software">
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
            _state = Enumeration.State.none;
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

        public StatePlayerElement(Enumeration.State state, Enumeration.PriorityState priority, Vector2 offSet, Vector2 offSetTotal)
        {
            _state = state;
            Priority = priority;
            Reverse = Enumeration.SequenceReverse.Normal;
            OffSet = offSet;
            OffSetTotal = offSetTotal;
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
