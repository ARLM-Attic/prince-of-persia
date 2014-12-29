	//-----------------------------------------------------------------------//
	// <copyright file="StateElement.cs" company="A.D.F.Software">
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
    public abstract class StateElement
    {
        private Enumeration.State _state = Enumeration.State.none;
        private Enumeration.PriorityState _priorityState;
        private bool? _stoppable;
        private Enumeration.SequenceReverse _reverse;
        private Vector2 _offset = Vector2.Zero;
        private Vector2 _offsetTotal = Vector2.Zero;
        private bool _raised = false;
        private bool _ifTrue = false;
        private string _name = string.Empty;
        private int _frame;

        public string Name
        {
            get { return _name.ToLower(); }
            set 
            { 
                if (value == null)
                    _name = string.Empty; 
                else
                    _name = value.ToLower(); 
            }
        }

        public bool Raised
        {
            get { return _raised; }
            set {_raised = value;}
        }


        public int Frame
        {
            get { return _frame; }
            set { _frame = value; }
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

        public Vector2 OffSetTotal
        {
            get { return _offsetTotal; }
            set { _offsetTotal = value; }
        }

        public Enumeration.SequenceReverse Reverse
        {
            get { return _reverse; }
            set { _reverse = value; }
        }

        public bool? Stoppable
        {
            get { return _stoppable; }
            set { _stoppable = value; }
        }

        public Enumeration.State state
        {
            get { return _state; }
            set { _state = value; }
        }

        public Enumeration.PriorityState Priority
        {
            get { return _priorityState; }
            set { _priorityState = value; }
        }

        public StateElement()
        {
            _state = Enumeration.State.none;
            _priorityState = Enumeration.PriorityState.Normal;
            _reverse = Enumeration.SequenceReverse.Normal;
            _offset = Vector2.Zero;
        }

        public StateElement(Enumeration.State state, Enumeration.PriorityState priority)
        {
            _state = state;
            _priorityState = priority;
            _reverse = Enumeration.SequenceReverse.Normal;
            _offset = Vector2.Zero;
        }

        public StateElement(Enumeration.State state, Enumeration.PriorityState priority, bool? stoppable)
        {
            _state = state;
            _priorityState = priority;
            _stoppable = stoppable;
            _reverse = Enumeration.SequenceReverse.Normal;
            _offset = Vector2.Zero;
        }

        public StateElement(Enumeration.State state, Enumeration.PriorityState priority, Enumeration.SequenceReverse reverse)
        {
            _state = state;
            _priorityState = priority;
            _reverse = reverse;
            _offset = Vector2.Zero;
        }


        public StateElement(Enumeration.State state, Enumeration.PriorityState priority, bool? stoppable, Enumeration.SequenceReverse reverse)
        {
            _state = state;
            _priorityState = priority;
            _stoppable = stoppable;
            _reverse = reverse;
            _offset = Vector2.Zero;
        }

        public StateElement(Enumeration.State state, Enumeration.PriorityState priority, bool? stoppable, Enumeration.SequenceReverse reverse, Vector2 offSet)
        {
            _state = state;
            _priorityState = priority;
            _stoppable = stoppable;
            _reverse = reverse;
            _offset = offSet;
        }
        public StateElement(Enumeration.State state)
        {
            _state = state;
            _priorityState = Enumeration.PriorityState.Normal;
            _reverse = Enumeration.SequenceReverse.Normal;
            _offset = Vector2.Zero;
        }

        public static Enumeration.StateTile Parse(string name)
        {
            return (Enumeration.StateTile)Enum.Parse(typeof(Enumeration.StateTile), name.ToLower());
        }



    }
   
}
