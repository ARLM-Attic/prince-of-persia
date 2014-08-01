using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using PrinceOfPersia.Apoplexy;

namespace PrinceOfPersia.Apoplexy
{
    public class links
    {

        private string _left = "";
        private string _right = "";
        private string _up = "";
        private string _down = "";

        [XmlAttribute("left")]
        public string left
        {
            get { return _left; }
            set { _left = value; }
        }


        [XmlAttribute("right")]
        public string right
        {
            get { return _right; }
            set { _right = value; }
        }

        [XmlAttribute("up")]
        public string up
        {
            get { return _up; }
            set { _up = value; }
        }

        [XmlAttribute("down")]
        public string down
        {
            get { return _down; }
            set { _down = value; }
        }

        public links()
        {
        }

   

    }
}
