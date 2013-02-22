using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrinceOfPersia
{


    public class Sprite
    {
        public bool IsAlive
        {
            get { return isAlive; }
            set { isAlive = value; }
        }
        bool isAlive;

        public int LivePoints
        {
            get { return livePoints; }
            set 
            { 
                livePoints = value;
                if (livePoints == 0)
                    IsAlive = false;
            }
        }
        int livePoints;


        public Sprite()
        { }
    }
}
