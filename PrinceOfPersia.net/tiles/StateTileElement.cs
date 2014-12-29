	//-----------------------------------------------------------------------//
	// <copyright file="StateTileElement.cs" company="A.D.F.Software">
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
