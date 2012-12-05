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
        private State _state;

        public new enum State
        {
            normal // normal state
            ,fall
            ,animation
        }

        public new State state
        {
            get { return _state; }
            set { _state = value; }
        }

        public StateTileElement()
        {
            _state = State.normal;
        }

    }
   
}
