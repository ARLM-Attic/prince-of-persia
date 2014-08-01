using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using PrinceOfPersia.Apoplexy;

namespace PrinceOfPersia.Apoplexy
{
    public class guard
    {

        private string _location = "";
        private string _direction = "";
        private string _skill = "";
        private string _colors = "";

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

        [XmlAttribute("skill")]
        public string skill
        {
            get { return _skill; }
            set { _skill = value; }
        }

        [XmlAttribute("colors")]
        public string colors
        {
            get { return _colors; }
            set { _colors = value; }
        }

        public guard()
        {
        }

   

    }
}
