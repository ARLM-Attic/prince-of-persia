using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using PrinceOfPersia.Apoplexy;

namespace PrinceOfPersia.Apoplexy
{
    public class prince
    {

        private string _room = "";
        private string _location = "";
        private string _direction = "";

        [XmlAttribute("room")]
        public string room
        {
            get { return _room; }
            set { _room = value; }
        }


        [XmlAttribute("location")]
        public string location
        {
            get { return _location; }
            set { _location = value; }
        }


        [XmlAttribute("direction")]
        public string direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

    }
}
