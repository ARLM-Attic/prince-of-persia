using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using PrinceOfPersia.Apoplexy;

namespace PrinceOfPersia.Apoplexy
{
    public class @event
    {

        private string _number = "";
        private string _room = "";
        private string _location = "";
        private string _next = "";

        [XmlAttribute("number")]
        public string number
        {
            get { return _number; }
            set { _number = value; }
        }


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

        [XmlAttribute("next")]
        public string next
        {
            get { return _next; }
            set { _next = value; }
        }


   

    }
}
